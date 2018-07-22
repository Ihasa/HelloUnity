﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiler : MonoBehaviour
{
    private Rigidbody rb;

    public float testV0;
    public float testDistance;
    public int testSig;
    public int testDirection;
    public float testMinV0Rate;
    public float testSpin;
    private float testProjectiledZ;
    private float testGroundZ;
    private float gravity;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log(""+this.gameObject.transform.localScale.y);
        rb.maxAngularVelocity = 100*2*Mathf.PI;
        gravity = Physics.gravity.y;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Mathf.Abs(gameObject.transform.position.z) + testDistance;
        if (Input.GetKeyDown(KeyCode.K))
        {
            projectile(testDirection * testV0, distance, testSig, testSpin);
            testDirection = -testDirection;
            testProjectiledZ = gameObject.transform.position.z;
        }
        if (Input.GetKeyDown("l"))
        {
            projectile(distance, testSig, testDirection, testSpin);
            testDirection = -testDirection;
            testProjectiledZ = gameObject.transform.position.z;
        }

        if (Input.GetKeyDown("j"))
        {
            projectileMax(distance, testSig, testDirection, testSpin);
            testDirection = -testDirection;
            testProjectiledZ = gameObject.transform.position.z;
        }

        if (Input.GetKeyDown("space"))
        {
            jump(3);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
    }

    //空気抵抗あるやつはhttps://qiita.com/kamasu/items/0874022be9a327446665の兄貴がやってる
    private void projectile(float v0, float distance, int sig, float spin)
    {
        float y0 = groundY();
        float A = (gravity * distance * distance / (2 * v0 * v0));
        float B = distance;
        float C = y0 + A;
        float D = B * B - 4 * A * C;
        float angle;
        if (D < 0)
        {
            angle = Mathf.PI / 4;
            Debug.Log("no angle calclated");
        }
        else
        {
            float tanTheta = (-B + sig * Mathf.Sqrt(D)) / (2 * A);
            Debug.Log("tanTheta=" + tanTheta);
            angle = Mathf.Atan(tanTheta);
        }

        //Z - Y
        setVel(v0, spin, angle);
    }

    private void setVel(float v0, float spin, float angle)
    {
        Vector3 vel = new Vector3(0, Mathf.Abs(v0) * Mathf.Sin(angle), v0 * Mathf.Cos(angle));
        rb.AddForce(vel - rb.velocity, ForceMode.VelocityChange);
        rb.AddTorque(new Vector3(testDirection * spin, 0, 0) * Mathf.PI * 2 - rb.angularVelocity, ForceMode.VelocityChange);
        Debug.Log("angle = " + Mathf.Rad2Deg * angle + ",velocity = " + rb.velocity);
        Debug.Log("projectile:" + gameObject.transform.position.z);
    }

    private void projectile(float distance, int sig, int direction, float spin)
    {
        float y0 = groundY();
        float v0 = Mathf.Sqrt(-y0 - gravity * Mathf.Sqrt(y0 * y0 + distance * distance));
        Debug.Log("Min Velocity = " + v0);
        projectile(direction * v0*testMinV0Rate, distance, sig, spin);
    }
    private void projectileMax(float distance, int sig, int direction, float spin)
    {
        float y0 = groundY();
        float y1 = 0.914f+0.15f;
        float x0 = Mathf.Abs(gameObject.transform.position.z);
        float x1 = distance;
        float tanTheta = ((y1 - y0)*x1*x1 + x0*x0*y0) / (x0*x1 * (x1-x0));
        tanTheta = Mathf.Max(tanTheta, -y0 / x1 / 2);
        float tmp = (-gravity * x1 * x1 * (tanTheta * tanTheta + 1) / (2*(x1*tanTheta+y0)));
        float v0 = Mathf.Sqrt(tmp);
        float angle = Mathf.Atan(tanTheta);
        setVel(v0*direction, spin, angle);
    }

    private float groundY()
    {
        return this.gameObject.transform.position.y - this.gameObject.transform.localScale.y / 2;
    }

    private void jump(float y)
    {
        float speedY = Mathf.Sqrt(2.000f * (-gravity) * y); /* 重力と揚力はともに上が+ */
        Vector3 vel = new Vector3(rb.velocity.x, speedY, rb.velocity.z);
        rb.AddForce(vel - rb.velocity, ForceMode.VelocityChange);
        rb.AddTorque(new Vector3(testDirection, 0, 0) - rb.angularVelocity, ForceMode.VelocityChange);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("飛距離:" + Mathf.Abs(testProjectiledZ - gameObject.transform.position.z));
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Net"))
        {
            Debug.Log("y = " + groundY());
        }
    }
}

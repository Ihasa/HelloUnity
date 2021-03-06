﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiler : MonoBehaviour
{
    private Rigidbody rb;
    public float ballRadius = 0.075f;
    public Vector3? aimedPoint = null;
    public int bounds;

    private float gravity;
    private Vector3 projectiled;
    private float currentSpin;
    private float currentVel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        gravity = Physics.gravity.y;
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log(""+this.gameObject.transform.localScale.y);
        rb.maxAngularVelocity = 100*2*Mathf.PI;
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, gravity, 0), ForceMode.Acceleration);
    }

    //空気抵抗あるやつはhttps://qiita.com/kamasu/items/0874022be9a327446665の兄貴がやってる
    private void projectile(float v0, Vector3 aim, int sig, float spin)
    {
        Vector3 current = gameObject.transform.position;
        float distance = getDistance(aim);
        float distanceAbs = Mathf.Abs(distance);
        
        gravity = -9.8f - magnus(spin);
        float y0 = groundY();
        float A = (gravity * distanceAbs * distanceAbs / (2 * v0 * v0));
        float B = distanceAbs;
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
        setVel(v0, aim, spin, angle);
    }

    private void setVel(float v0, Vector3 aim, float spin, float angle)
    {
        float angleX = getAngleX(aim);
        float distance = getDistance(aim);
        float directionZ = Mathf.Sign(distance);
        v0 = v0 * directionZ;
        float vz = v0 * Mathf.Cos(angle);
        Vector3 vel = new Vector3(vz * Mathf.Tan(angleX), Mathf.Abs(v0) * Mathf.Sin(angle), vz);
        Debug.Log("angle = " + Mathf.Rad2Deg * angle + ",velocity = " + rb.velocity);
        Debug.Log("projectile:" + gameObject.transform.position.z);
        rb.AddForce(vel - rb.velocity, ForceMode.VelocityChange);
        rb.AddTorque(new Vector3(Mathf.Cos(angleX)*spin, 0, -Mathf.Sin(angleX)*spin) * Mathf.PI * 2 - rb.angularVelocity, ForceMode.VelocityChange);

        bounds = 0;
        aimedPoint = aim;
        currentVel = v0;
        currentSpin = spin;
        projectiled = gameObject.transform.position;
    }

    private void projectile(Vector3 aim, int sig, float spin)
    {
        float distance = getDistance(aim);

        gravity = -9.8f - magnus(spin);
        float y0 = groundY();
        float v0 = Mathf.Sqrt(-y0 - gravity * Mathf.Sqrt(y0 * y0 + distance * distance));
        Debug.Log("Min Velocity = " + v0);
        projectile(v0, aim, sig, spin);
    }
    public void projectile(Vector3 aim, Vector3 aim2, float spin)
    {
        Vector3 current = gameObject.transform.position;
        float distance = getDistance(aim);
        float distanceAbs = Mathf.Abs(distance);

        gravity = -9.8f - magnus(spin);
        float y0 = groundY();
        float y1 = aim2.y;
        float y2 = aim.y;
        float x1 = Mathf.Abs(gameObject.transform.position.z) + aim2.z;
        float x2 = distanceAbs;
        float tanTheta = ((y1 - y0)*x2*x2 + x1*x1*(y0-y2)) / (x1*x2 * (x2-x1));
        float tanThetaMin = -(y0 - y2) / x2;
        if(tanTheta < tanThetaMin)
        {
            float d = Mathf.Tan(Mathf.Deg2Rad * 10);
            tanTheta = (tanThetaMin + d) / (1 - tanThetaMin * d);
        }
        float tmp = (-gravity * x2 * x2 * (tanTheta * tanTheta + 1) / (2*(x2*tanTheta+y0-y2)));
        if (tmp < 0) Debug.Log("HEY:" + tmp + ", Tan" + tanTheta);
        float v0 = Mathf.Sqrt(tmp);

        setVel(v0, aim, spin, Mathf.Atan(tanTheta));
    }

    private float getDistance(Vector3 aim)
    {
        Vector3 current = gameObject.transform.position;
        return aim.z - current.z;
    }

    private float getAngleX(Vector3 aim)
    {
        Vector3 current = gameObject.transform.position;
        return Mathf.Atan2(aim.x - current.x, aim.z - current.z);
    }

    private float magnus(float spin)
    {
        return Mathf.Lerp(-4, 4, (spin + 50) / 100);
        //なんかもう断念した
        /*
        float p = 1.184f;
        float d = gameObject.transform.localScale.x;
        float v = new Vector2(rb.velocity.x, rb.velocity.z).magnitude;
        float f = (Mathf.PI * Mathf.PI * p * d * d * d * v * spin)/8;
        return f / rb.mass;
        */
    }

    private float groundY()
    {
        return this.gameObject.transform.position.y - ballRadius;
    }

    public void jump(float y)
    {
        float speedY = Mathf.Sqrt(2.000f * (-gravity) * y); /* 重力と揚力はともに上が+ */
        Vector3 vel = new Vector3(0, speedY, 0);
        rb.AddForce(vel - rb.velocity, ForceMode.VelocityChange);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            GetComponent<AudioSource>().Play();
            bounds++;
        }
        if (collision.gameObject.CompareTag("Wall"))
        {
            bounds++;
        }
        gravity = -9.8f;
        Debug.Log("飛距離:" + Mathf.Abs(projectiled.z - gameObject.transform.position.z));
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Net"))
        {
            Debug.Log("y = " + groundY());
            Debug.Log("angVel = " + rb.angularVelocity.x + "magnus = " + magnus(currentSpin));
        }
    }
}

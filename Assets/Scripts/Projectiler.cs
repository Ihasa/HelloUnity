using System.Collections;
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
    private float testProjectiledZ;
    private float testGroundZ;

    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Debug.Log(""+this.gameObject.transform.localScale.y);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            projectile(testDirection * testV0, testDistance, testSig);
            testDirection = -testDirection;
            testProjectiledZ = gameObject.transform.position.z;
        }
        if (Input.GetKeyDown("l"))
        {
            projectile(testDistance, testSig, testDirection);
            testDirection = -testDirection;
            testProjectiledZ = gameObject.transform.position.z;
        }
        if (Input.GetKeyDown("space"))
        {
            jump(3);
        }
    }

    //空気抵抗あるやつはhttps://qiita.com/kamasu/items/0874022be9a327446665の兄貴がやってる
    private void projectile(float v0, float distance, int sig)
    {
        float y0 = groundY();
        float gravity = -Physics.gravity.y;
        float A = -(gravity * distance * distance / (2 * v0 * v0));
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
        Vector3 vel = new Vector3(0, Mathf.Abs(v0) * Mathf.Sin(angle), v0 * Mathf.Cos(angle));
        rb.AddForce(vel - rb.velocity, ForceMode.VelocityChange);
        rb.AddTorque(new Vector3(30, 0, 0) * Mathf.Sign(v0), ForceMode.VelocityChange);
        Debug.Log("angle = "+Mathf.Rad2Deg * angle+",velocity = " + rb.velocity);
        Debug.Log("projectile:"+gameObject.transform.position.z);
    }
    private void projectile(float distance, int sig, int direction)
    {
        float y0 = groundY();
        float gravity = -Physics.gravity.y;
        float v0 = Mathf.Sqrt(-y0 + gravity * Mathf.Sqrt(y0 * y0 + distance * distance));
        Debug.Log("Min Velocity = " + v0);
        projectile(direction * v0*testMinV0Rate, distance, sig);
    }

    private float groundY()
    {
        return this.gameObject.transform.position.y - this.gameObject.transform.localScale.y / 2;
    }

    private void jump(float y)
    {
        float speedY = Mathf.Sqrt(2.000f * (-Physics.gravity.y) * y); /* 重力と揚力はともに上が+ */
        Vector3 vel = new Vector3(rb.velocity.x, speedY, rb.velocity.z);
        rb.AddForce(vel - rb.velocity, ForceMode.VelocityChange);
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("飛距離:" + Mathf.Abs(testProjectiledZ - gameObject.transform.position.z));
    }
}

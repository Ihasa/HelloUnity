using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectiler : MonoBehaviour
{
    private Rigidbody rb;

    public float testV0;
    public Vector3 testAim;
    public int testSig;
    public int testDirection;
    public float testSpin;
    public float netHeight;
    private float testProjectiledZ;
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
        Vector3 aim = new Vector3(testDirection * testAim.x, 0, testDirection * testAim.z);
        if (Input.GetKeyDown(KeyCode.K))
        {
            projectile(testV0, aim, testSig, testSpin);
            testDirection = -testDirection;
            testProjectiledZ = gameObject.transform.position.z;
        }
        if (Input.GetKeyDown("l"))
        {
            projectile(aim, testSig, testSpin);
            testDirection = -testDirection;
            testProjectiledZ = gameObject.transform.position.z;
        }

        if (Input.GetKeyDown("j"))
        {
            projectileMax(aim, testSig, testSpin);
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
    private void projectile(float v0, Vector3 aim, int sig, float spin)
    {
        Vector3 current = gameObject.transform.position;
        float distance = getDistance(aim);
        float distanceAbs = Mathf.Abs(distance);
        float angleX = getAngleX(aim);
        
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
        setVel(v0, Mathf.Sign(distance), spin, angle, angleX);
    }

    private void setVel(float v0, float directionZ, float spin, float angle, float angleX)
    {
        v0 = v0 * directionZ;
        float vz = v0 * Mathf.Cos(angle);
        Vector3 vel = new Vector3(vz * Mathf.Tan(angleX), Mathf.Abs(v0) * Mathf.Sin(angle), vz);
        rb.AddForce(vel - rb.velocity, ForceMode.VelocityChange);
        rb.AddTorque(new Vector3(Mathf.Cos(angleX)*spin, 0, -Mathf.Sin(angleX)*spin) * Mathf.PI * 2 - rb.angularVelocity, ForceMode.VelocityChange);
        Debug.Log("angle = " + Mathf.Rad2Deg * angle + ",velocity = " + rb.velocity);
        Debug.Log("projectile:" + gameObject.transform.position.z);
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
    private void projectileMax(Vector3 aim, int sig, float spin)
    {
        Vector3 current = gameObject.transform.position;
        float distance = getDistance(aim);
        float distanceAbs = Mathf.Abs(distance);

        gravity = -9.8f - magnus(spin);
        float y0 = groundY();
        float y1 = netHeight;
        float x0 = Mathf.Abs(gameObject.transform.position.z);
        float x1 = distanceAbs;
        float tanTheta = ((y1 - y0)*x1*x1 + x0*x0*y0) / (x0*x1 * (x1-x0));
        tanTheta = Mathf.Max(tanTheta, -y0 / x1 / 2);
        float tmp = (-gravity * x1 * x1 * (tanTheta * tanTheta + 1) / (2*(x1*tanTheta+y0)));
        float v0 = Mathf.Sqrt(tmp);
        projectile(v0, aim, sig, spin);
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
        return this.gameObject.transform.position.y - this.gameObject.transform.localScale.y / 2;
    }

    private void jump(float y)
    {
        float speedY = Mathf.Sqrt(2.000f * (-gravity) * y); /* 重力と揚力はともに上が+ */
        Vector3 vel = new Vector3(rb.velocity.x, speedY, rb.velocity.z);
        rb.AddForce(vel - rb.velocity, ForceMode.VelocityChange);
    }

    public void OnCollisionEnter(Collision collision)
    {
        gravity = -9.8f;
        Debug.Log("飛距離:" + Mathf.Abs(testProjectiledZ - gameObject.transform.position.z));
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Net"))
        {
            Debug.Log("y = " + groundY());
            Debug.Log("angVel = " + rb.angularVelocity.x + "magnus = " + magnus(testSpin));
        }
    }
}

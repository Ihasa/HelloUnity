using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisPlayer : MonoBehaviour {
    public GameObject ballObject;
    public GameObject aimMark;
    public Camera mainCamera;
    public float runSpeed;
    public Vector3 aimVia;
    public float spinA;
    public float spinB;
    public bool isAutoShot;
    public bool isAutoMove;
    public bool isAutoAim;

    private IShotController shotController;
    private IMoveController moveController;
    private IAimController aimController;
    private Projectiler ballController;
    private Rigidbody rb;
    private AudioSource shotSound;
    private AudioSource runSound;
    private Vector2 cStateMovePrev;
    private bool shottable = false;

    // Use this for initialization
    void Start () {
        shotController = isAutoShot ? (IShotController)new AutoShotController(gameObject, ballObject.GetComponent<Rigidbody>()) : (IShotController)new KeyShotController();
        moveController = isAutoMove ? (IMoveController)new AutoMoveController(gameObject, ballObject.GetComponent<Projectiler>(), ballObject.GetComponent<Rigidbody>()) : (IMoveController)new KeyMoveController();
        aimController = isAutoAim ? (IAimController)new AutoAimController(gameObject,aimVia,new Vector2(-4,4),new Vector2(6,9)) : (IAimController)new MouseAimController(mainCamera,aimVia);

        ballController = ballObject.GetComponent<Projectiler>();
        rb = GetComponent<Rigidbody>();
        AudioSource[] clips = GetComponents<AudioSource>();
        runSound = clips[0];
        shotSound = clips[1];
        cStateMovePrev = new Vector2();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 cStateMove = moveController.GetDirection();
        rb.AddForce(new Vector3(cStateMove.x, 0, cStateMove.y) * runSpeed - rb.velocity, ForceMode.VelocityChange);
        if(cStateMovePrev == Vector2.zero && cStateMove != Vector2.zero)
        {
            runSound.Play();
        } else if(cStateMovePrev != Vector2.zero && cStateMove == Vector2.zero)
        {
            runSound.Stop();
        }
        cStateMovePrev = cStateMove;

        AimControllerState cStateAim = aimController.GetAim();
        aimMark.transform.position = cStateAim.aim;

        ShotControllerState cStateShot = shotController.GetControllerState();
        aimVia = cStateAim.aimVia;
        if (shottable) {
            if (cStateShot.shotA)
            {
                ballController.projectile(cStateAim.aim, cStateAim.aimVia, spinA);
                shotSound.Play();
            }
            else if (cStateShot.shotB)
            {
                ballController.projectile(cStateAim.aim, cStateAim.aimVia, spinB);
                shotSound.Play();
            }
        } else if (cStateShot.toss)
        {
            if (ballController.bounds >= 2)
            {
                toss(3);
            }
        }
    }

    public void toss(float height)
    {
        ballObject.transform.position = gameObject.transform.position + Vector3.forward;
        ballController.jump(height);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            shottable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            shottable = false;
        }
    }

}

struct ShotControllerState
{
    public bool shotA;
    public bool shotB;
    public bool toss;
}

interface IShotController
{
    ShotControllerState GetControllerState();
}

class KeyShotController : IShotController
{
    public ShotControllerState GetControllerState()
    {
        ShotControllerState ret = new ShotControllerState();
        ret.shotA = Input.GetMouseButtonDown(0);
        ret.shotB = Input.GetMouseButtonDown(1);
        ret.toss = Input.GetKeyDown("space");
        return ret;
    }
}

class AutoShotController : IShotController
{
    private GameObject player;
    private Rigidbody ballRigidbody;
    private ShotControllerState prev;
    public AutoShotController(GameObject player, Rigidbody ballRigidbody)
    {
        this.player = player;
        this.ballRigidbody = ballRigidbody;
        prev = new ShotControllerState();
    }
    public ShotControllerState GetControllerState()
    {
        ShotControllerState cState = new ShotControllerState();
        if(Mathf.Sign(ballRigidbody.transform.position.z) == Mathf.Sign(player.transform.position.z) && 
           Mathf.Sign(ballRigidbody.velocity.z) == Mathf.Sign(player.transform.position.z))
        {
            cState.shotA = !prev.shotA;
        }
        prev = cState;
        return cState;
    }
}

interface IMoveController
{
    Vector2 GetDirection();
}

class KeyMoveController : IMoveController
{
    public Vector2 GetDirection()
    {
        Vector2 direction = new Vector2();
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
        return direction;
    }
}

class AutoMoveController : IMoveController
{
    private GameObject player;
    private Projectiler ball;
    private Rigidbody rb;
    public AutoMoveController(GameObject player, Projectiler ball, Rigidbody rb)
    {
        this.player = player;
        this.ball = ball;
        this.rb = rb;
    }
    public Vector2 GetDirection()
    {
        Vector3 pos;
        if (Mathf.Sign(rb.velocity.z) == Mathf.Sign(player.transform.position.z) && ball.aimedPoint != null && ball.bounds <= 2)
        {
            pos = (Vector3)ball.aimedPoint + new Vector3(rb.velocity.x, 0, rb.velocity.z)*0.5f;
        } else
        {
            pos = new Vector3(0, 0, 13 * Mathf.Sign(player.transform.position.z));
        }
        Vector3 diff = (pos - player.transform.position).normalized;

        Vector2 res;
        if ((player.transform.position - pos).magnitude > 1)
        {
            res = new Vector2(diff.x, diff.z);
        }
        else
        {
            res = Vector2.zero;
        }
        return res;
    }
}

struct AimControllerState
{
    public Vector3 aim;
    public Vector3 aimVia;
    public AimControllerState(Vector3 aim, Vector3 aimVia)
    {
        this.aim = aim;
        this.aimVia = aimVia;
    }
}

interface IAimController
{
    AimControllerState GetAim();
}

class MouseAimController : IAimController
{
    private Camera mainCamera;
    private Vector3 prevAim = new Vector3(0, 10, 0);
    private Vector3 defaultAimVia;
    private float aimViaHeight = 0;
    private const float minAimZ = 2;
    public MouseAimController(Camera mainCamera, Vector3 defaultAimVia)
    {
        this.mainCamera = mainCamera;
        this.defaultAimVia = defaultAimVia;
    }
    public AimControllerState GetAim()
    {
        Vector3? result = null;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                result = hit.point;
                if(Mathf.Abs(hit.point.z) < minAimZ)
                {
                    result = new Vector3(hit.point.x, hit.point.y, Mathf.Sign(hit.point.z) * minAimZ);
                }
                prevAim = (Vector3)result;
                break;
            }
        }

        //0.1ずつ
        aimViaHeight += Input.GetAxis("Mouse ScrollWheel");

        return new AimControllerState(
            result ?? prevAim,
            defaultAimVia + new Vector3(0, aimViaHeight, 0)
        );
    }
}

class AutoAimController : IAimController
{
    private GameObject player;
    private Vector3 defaultAimVia;
    private Vector2 aimRangeX;
    private Vector2 aimRangeZ;
    public AutoAimController(GameObject player, Vector3 defaultAimVia, Vector2 aimRangeX, Vector2 aimRangeZ)
    {
        this.player = player;
        this.defaultAimVia = defaultAimVia;
        this.aimRangeX = aimRangeX;
        this.aimRangeZ = aimRangeZ;
    }
    public AimControllerState GetAim()
    {
        return new AimControllerState(
            new Vector3(Random.Range(aimRangeX.x, aimRangeX.y), 0, -Mathf.Sign(player.transform.position.z) * Random.Range(aimRangeZ.x, aimRangeZ.y)),
            defaultAimVia // + Vector3.up * Random.Range(-aimRangeY.x, aimRangeY.y)
        );
    }
}



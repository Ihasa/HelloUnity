﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisPlayer : MonoBehaviour {
    public GameObject ballObject;
    public GameObject aimMark;
    public Camera mainCamera;
    public float runSpeed;
    public Vector3 aim;
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
    private AudioSource shotSound;
    private AudioSource runSound;
    private Vector2 cStateMovePrev;

    // Use this for initialization
    void Start () {
        shotController = isAutoShot ? (IShotController)new AutoShotController(gameObject, ballObject.GetComponent<Rigidbody>()) : (IShotController)new KeyShotController();
        moveController = isAutoMove ? (IMoveController)new AutoMoveController(gameObject, ballObject.GetComponent<Rigidbody>()) : (IMoveController)new KeyMoveController();
        aimController = isAutoAim ? (IAimController)new AutoAimController(gameObject) : (IAimController)new MouseAimController(mainCamera);

        ballController = ballObject.GetComponent<Projectiler>();
        AudioSource[] clips = GetComponents<AudioSource>();
        runSound = clips[0];
        shotSound = clips[1];
        cStateMovePrev = new Vector2();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 cStateAim = aimController.GetAim();
        aim = cStateAim;
        aimMark.transform.position = aim;


        Vector2 cStateMove = moveController.GetDirection();
        this.gameObject.transform.position += new Vector3(cStateMove.x, 0, cStateMove.y) * runSpeed * Time.deltaTime;
        if(cStateMovePrev == Vector2.zero && cStateMove != Vector2.zero)
        {
            runSound.Play();
        } else if(cStateMovePrev != Vector2.zero && cStateMove == Vector2.zero)
        {
            runSound.Stop();
        }
        cStateMovePrev = cStateMove;

        ShotControllerState cStateShot = shotController.GetControllerState();
        if (cStateShot.shotA)
        {
            ballController.projectile(aim, aimVia, spinA);
            shotSound.Play();
        } else if (cStateShot.shotB)
        {
            ballController.projectile(aim, aimVia, spinB);
            shotSound.Play();
        } else if (cStateShot.toss)
        {
            toss(3);
        }
    }

    public void toss(float height)
    {
        ballController.jump(height);
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
            if (Mathf.Abs(ballRigidbody.transform.position.z) > 6.4f)
            {
                cState.shotA = !prev.shotA;
            }
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
    private Rigidbody rb;
    public AutoMoveController(GameObject player, Rigidbody rb)
    {
        this.player = player;
        this.rb = rb;
    }
    public Vector2 GetDirection()
    {
        Vector3 pos;
        if (Mathf.Sign(rb.velocity.z) == Mathf.Sign(player.transform.position.z) && rb.velocity.z != 0)
        {
            pos = rb.transform.position;
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

interface IAimController
{
    Vector3 GetAim();
}

class MouseAimController : IAimController
{
    private Camera mainCamera;
    public MouseAimController(Camera mainCamera)
    {
        this.mainCamera = mainCamera;
    }
    public Vector3 GetAim()
    {
        Vector3 result = Vector3.zero;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                result = hit.point;
            }
        }
        return result;
    }
}

class AutoAimController : IAimController
{
    private GameObject player;
    public AutoAimController(GameObject player)
    {
        this.player = player;
    }
    public Vector3 GetAim()
    {
        return new Vector3(Random.Range(-2,2), 0, Mathf.Sign(player.transform.position.z) * -7);
    }
}



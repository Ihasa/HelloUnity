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
        shotController = GetComponent<IShotController>();
        moveController = GetComponent<IMoveController>();
        aimController = GetComponent<IAimController>();
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

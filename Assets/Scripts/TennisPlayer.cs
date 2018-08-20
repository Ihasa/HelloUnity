using System.Collections;
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

    private IController controller;
    private Projectiler ballController;
    private AudioSource shotSound;
    private AudioSource runSound;
    private ControllerState cStatePrev;

    // Use this for initialization
    void Start () {
        controller = new ArrowController();
        ballController = ballObject.GetComponent<Projectiler>();
        AudioSource[] clips = GetComponents<AudioSource>();
        runSound = clips[0];
        shotSound = clips[1];
        cStatePrev = new ControllerState();
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                aim = hit.point;
                aimMark.transform.position = aim;
            }
        }

        ControllerState cState = controller.GetControllerState();
        this.gameObject.transform.position += new Vector3(cState.direction.x, 0, cState.direction.y) * runSpeed * Time.deltaTime;
        if(cStatePrev.direction == Vector2.zero && cState.direction != Vector2.zero)
        {
            runSound.Play();
        } else if(cStatePrev.direction != Vector2.zero && cState.direction == Vector2.zero)
        {
            runSound.Stop();
        }
        if (cState.shotA)
        {
            ballController.projectile(aim, aimVia, spinA);
            shotSound.Play();
        } else if (cState.shotB)
        {
            ballController.projectile(aim, aimVia, spinB);
            shotSound.Play();
        } else if (cState.toss)
        {
            toss(3);
        }
        cStatePrev = cState;
    }

    public void toss(float height)
    {
        ballObject = Instantiate(ballObject, this.transform.position + Vector3.forward + Vector3.up, Quaternion.identity);
        ballController = ballObject.GetComponent<Projectiler>();
        ballController.jump(height);
    }

}

struct ControllerState
{
    public Vector2 direction;
    public bool shotA;
    public bool shotB;
    public bool toss;
}

interface IController
{
    ControllerState GetControllerState();
}

class ArrowController : IController
{
    public ControllerState GetControllerState()
    {
        ControllerState ret = new ControllerState();
        ret.direction.x = Input.GetAxis("Horizontal");
        ret.direction.y = Input.GetAxis("Vertical");
        ret.shotA = Input.GetMouseButtonDown(0);
        ret.shotB = Input.GetMouseButtonDown(1);
        ret.toss = Input.GetKeyDown("space");
        return ret;
    }
}


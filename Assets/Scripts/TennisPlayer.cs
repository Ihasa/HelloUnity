using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisPlayer : MonoBehaviour {
    public Projectiler ball;
    public GameObject aimMark;
    public Camera mainCamera;
    public float runSpeed;
    public Vector3 aim;
    public Vector3 aimVia;
    public float spin;

    private IController controller;
    private AudioSource shotSound;
    private AudioSource runSound;
    private ControllerState cStatePrev;

    // Use this for initialization
    void Start () {
        controller = new ArrowController();
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
        if (cState.shot)
        {
            ball.projectile(aim, aimVia, spin);
            shotSound.Play();
        }
        cStatePrev = cState;
    }

}

struct ControllerState
{
    public Vector2 direction;
    public bool shot;
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
        ret.shot = Input.GetMouseButtonDown(0);
        return ret;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour {

    public TennisPlayer player;
    public GameObject ball;
    public Vector3 posOffset = new Vector3(0, 1.7f, 3f);
    public ICameraMode cameraMode;
    private bool lookBall;
    
	// Use this for initialization
	void Start () {
        cameraMode = new FixedCamera();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!lookBall)
            {
                cameraMode = new BallLookCamera();
                lookBall = true;
            }

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (lookBall)
            {
                cameraMode = new PlayerCamera();
                lookBall = false;
            }
        }
    }

    // Update is called once per frame
    void LateUpdate () {
        CameraControlData cControl = cameraMode.GetCameraControlData(player, ball, posOffset);

        Vector3 camPos = cControl.Position;
        this.transform.position = camPos;

        Vector3 lookAt = cControl.LookAt;
        this.transform.LookAt(lookAt);
	}
}

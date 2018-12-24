using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamaraController : MonoBehaviour {

    public TennisPlayer player;
    public GameObject ball;
    public Vector3 offset = new Vector3(0, 1.7f, 3f);
    private bool lookBall;
    
	// Use this for initialization
	void Start () {
	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            lookBall = true;

        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            lookBall = false;
        }
    }

    // Update is called once per frame
    void LateUpdate () {
        Vector3 camPos = new Vector3(player.transform.position.x - player.direction * offset.x, player.transform.position.y + offset.y, player.transform.position.z - player.direction * offset.z);
        this.transform.position = camPos;

        Vector3 lookAt;
        if (lookBall)
        {
            lookAt = ball.transform.position;
        } else
        {
            lookAt = camPos + Vector3.forward;
        }
        this.transform.LookAt(lookAt);
	}
}

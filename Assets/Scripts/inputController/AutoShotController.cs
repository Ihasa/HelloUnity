using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShotController : MonoBehaviour, IShotController {
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
        if (Mathf.Sign(ballRigidbody.transform.position.z) == Mathf.Sign(player.transform.position.z) &&
           Mathf.Sign(ballRigidbody.velocity.z) == Mathf.Sign(player.transform.position.z))
        {
            cState.shotA = !prev.shotA;
        }
        prev = cState;
        return cState;
    }
}
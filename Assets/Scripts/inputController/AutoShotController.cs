using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShotController : MonoBehaviour, IShotController {
    public GameObject ball;
    private Rigidbody ballRigidbody;
    private ShotControllerState prev;

    void Awake()
    {
        ballRigidbody = ball.GetComponent<Rigidbody>();
        prev = new ShotControllerState();
    }

    public ShotControllerState GetControllerState()
    {
        ShotControllerState cState = new ShotControllerState();
        if (Mathf.Sign(ballRigidbody.transform.position.z) == Mathf.Sign(gameObject.transform.position.z) &&
           Mathf.Sign(ballRigidbody.velocity.z) == Mathf.Sign(gameObject.transform.position.z))
        {
            cState.shotA = !prev.shotA;
        }
        prev = cState;
        return cState;
    }
}
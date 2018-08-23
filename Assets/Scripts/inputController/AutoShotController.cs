using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoShotController : MonoBehaviour, IShotController {
    public GameObject ball;
    private Rigidbody ballRigidbody;
    private ShotControllerState prev;
    private bool result = false;

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
            result = !result;
            if (Random.Range(0.0f, 1.0f) < 0.5f)
            {
                cState.shotA = result;
            } else
            {
                cState.shotB = result;
            }
        }
        prev = cState;
        return cState;
    }
}
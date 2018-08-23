using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveController : MonoBehaviour, IMoveController {
    public GameObject ball;
    private Projectiler ballController;
    private Rigidbody rb;

    void Awake()
    {
        ballController = ball.GetComponent<Projectiler>();
        rb = ball.GetComponent<Rigidbody>();
    }

    public Vector2 GetDirection()
    {
        Vector3 pos;
        if (Mathf.Sign(rb.velocity.z) == Mathf.Sign(gameObject.transform.position.z) && ballController.aimedPoint != null && ballController.bounds <= 2)
        {
            pos = (Vector3)ballController.aimedPoint + new Vector3(rb.velocity.x, 0, rb.velocity.z) * 0.5f;
        }
        else
        {
            pos = new Vector3(0, 0, 13 * Mathf.Sign(gameObject.transform.position.z));
        }
        Vector3 diff = (pos - gameObject.transform.position).normalized;

        Vector2 res;
        if ((gameObject.transform.position - pos).magnitude > 1)
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

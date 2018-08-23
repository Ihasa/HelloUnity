using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoMoveController : MonoBehaviour, IMoveController {
    private GameObject player;
    private Projectiler ball;
    private Rigidbody rb;
    public AutoMoveController(GameObject player, Projectiler ball, Rigidbody rb)
    {
        this.player = player;
        this.ball = ball;
        this.rb = rb;
    }
    public Vector2 GetDirection()
    {
        Vector3 pos;
        if (Mathf.Sign(rb.velocity.z) == Mathf.Sign(player.transform.position.z) && ball.aimedPoint != null && ball.bounds <= 2)
        {
            pos = (Vector3)ball.aimedPoint + new Vector3(rb.velocity.x, 0, rb.velocity.z) * 0.5f;
        }
        else
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

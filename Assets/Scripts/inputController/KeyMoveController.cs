using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMoveController : MonoBehaviour, IMoveController{
    public Vector2 GetDirection()
    {
        Vector2 direction = new Vector2();
        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");
        return direction;
    }
}

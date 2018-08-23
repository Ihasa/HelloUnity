using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyShotController : MonoBehaviour, IShotController {
    public ShotControllerState GetControllerState()
    {
        ShotControllerState ret = new ShotControllerState();
        ret.shotA = Input.GetMouseButtonDown(0);
        ret.shotB = Input.GetMouseButtonDown(1);
        ret.toss = Input.GetKeyDown("space");
        return ret;
    }
}
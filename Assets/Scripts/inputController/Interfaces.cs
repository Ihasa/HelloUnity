using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ShotControllerState
{
    public bool shotA;
    public bool shotB;
    public bool toss;
}

public interface IShotController
{
    ShotControllerState GetControllerState();
}

public interface IMoveController
{
    Vector2 GetDirection();
}

public struct AimControllerState
{
    public Vector3 aim;
    public Vector3 aimVia;
    public AimControllerState(Vector3 aim, Vector3 aimVia)
    {
        this.aim = aim;
        this.aimVia = aimVia;
    }
}

public interface IAimController
{
    AimControllerState GetAim();
}

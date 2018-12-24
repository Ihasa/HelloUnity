using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : ICameraMode
{
    public virtual CameraControlData GetCameraControlData(TennisPlayer tennisPlayer, GameObject ball, Vector3 offset)
    {
        CameraControlData cData = new CameraControlData();
        cData.Position = new Vector3(tennisPlayer.transform.position.x - tennisPlayer.direction * offset.x, tennisPlayer.transform.position.y + offset.y, tennisPlayer.transform.position.z - tennisPlayer.direction * offset.z);
        cData.LookAt = cData.Position + Vector3.forward * tennisPlayer.direction;
        return cData;
    }
}

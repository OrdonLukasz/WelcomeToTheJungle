using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public ScriptManager scriptManager;
    public float cameraSpeed;
    public Vector3 cameraOffset;
    public void CameraFollow()
    {
        transform.position = Vector3.Slerp(transform.position, scriptManager.player.transform.position + cameraOffset, cameraSpeed * Time.deltaTime);
    }
    void Update()
    {
        CameraFollow();
    }
}

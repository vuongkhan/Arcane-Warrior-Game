using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public float smoothSpeed = 0.125f;
    public float ySmoothSpeed = 0.02f; 
    public Vector3 offset; 

    private Transform player;
    void Update()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Cannot find player");
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {
            Vector3 desiredPosition = player.position + offset;
            float smoothedX = Mathf.Lerp(transform.position.x, desiredPosition.x, smoothSpeed);
            float smoothedY = Mathf.Lerp(transform.position.y, desiredPosition.y, ySmoothSpeed);
            float smoothedZ = Mathf.Lerp(transform.position.z, desiredPosition.z, smoothSpeed);
            transform.position = new Vector3(smoothedX, smoothedY, smoothedZ);
        }
    }
}

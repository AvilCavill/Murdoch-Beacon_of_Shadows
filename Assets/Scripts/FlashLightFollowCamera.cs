using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightFollowCamera : MonoBehaviour
{
    public Transform cameraTransform; // Transform de la cámara del jugador
    public Vector3 flashlightOffset = new Vector3(0.2f, -0.2f, 0.5f);

    void Update()
    {
        // Alinear la posición y rotación con la cámara en cada frame
        transform.position = cameraTransform.position + cameraTransform.TransformDirection(flashlightOffset);
        transform.rotation = cameraTransform.rotation;
    }
}


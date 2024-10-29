using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCamera : MonoBehaviour
{
    public float mouseSensitivity = 0.5f;
    public float xRotation = 0f;
    public float yRotation = 0f;
    public float minVerticalAngle = 0f;
    public float maxVerticalAngle = 90f;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        minVerticalAngle = -90f;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        xRotation += mouseX;
        
        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, minVerticalAngle, maxVerticalAngle);

        transform.localRotation = Quaternion.Euler(yRotation, xRotation, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateHead : MonoBehaviour
{
    public float rotateSpeed = 45;
    private float rotationAnglesY = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotationAnglesY =+ rotationAnglesY + rotateSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(0, rotationAnglesY, 0);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    
    public float walkSpeed = 15.0f;
    public float runSpeed = 25.0f;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private bool playerIsWalking = false;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerMoveSpeed;
    private float playerWalkSpeed = 15.0f;
    private float playerSprintSpeed = 25.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    
    public StaminaController _staminaController;

    public Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        _staminaController = gameObject.GetComponent<StaminaController>();
        controller = gameObject.AddComponent<CharacterController>();

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    public void SetRunSpeed(float speed)
    {
        playerSprintSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerMoveSpeed = playerSprintSpeed;
        }
        else 
        {
            playerMoveSpeed = playerWalkSpeed;
        }
        // Obtener axis
        Vector3 inputDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (playerIsWalking)
        {
            _staminaController.playerIsSprinting = false;
        }

        // if (!playerVelocity &&)
        // {
        //     if(playerMoveSpeed.)
        // }
        // Ejecutar solo si hay input del jugador
        if (inputDirection.magnitude > 0.1f)
        {
            // Convertir la dirección de entrada en relación a la dirección de la cámara
            Vector3 moveDirection =
                cameraTransform.right * inputDirection.x + cameraTransform.forward * inputDirection.z;
            moveDirection.y = 0f; // Asegurarse de que no se mueva en el eje Y

            // Mover el jugador
            controller.Move(moveDirection * Time.deltaTime * playerMoveSpeed);

            // Rotar el jugador hacia la dirección de movimiento
            transform.forward = moveDirection;

            // Changes the height position of the player..
            if (Input.GetButtonDown("Jump") && groundedPlayer)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            }

            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);
        }
    }
}

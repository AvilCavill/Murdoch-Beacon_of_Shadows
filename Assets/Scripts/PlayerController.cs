using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 3f; // Velocidad de caminata
    public float runSpeed = 6f; // Velocidad de correr
    public float gravity = -9.81f; // Gravedad
    public Transform orientation; // Referencia de la orientación de la cámara

    private CharacterController controller;
    private Vector3 velocity; // Velocidad vertical (caída)

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {

        // Obtener entrada de movimiento
        float horizontal = Input.GetAxis("Horizontal"); // Movimiento A/D
        float vertical = Input.GetAxis("Vertical"); // Movimiento W/S
        Vector3 direction = orientation.forward * vertical + orientation.right * horizontal;

        // Verificar si el jugador está corriendo
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Aplicar movimiento al CharacterController
        controller.Move(direction * speed * Time.deltaTime);

        // Hacer que el jugador rote con la cámara (solo en el eje Y)
        RotateWithCamera();

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void RotateWithCamera()
    {
        // Ajusta la rotación del jugador para que coincida con la orientación de la cámara en el eje Y
        Vector3 rotation = new Vector3(0, orientation.eulerAngles.y, 0);
        transform.eulerAngles = rotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightHouseLightMovement : MonoBehaviour
{
    [Header("Configuración del Movimiento")]
    public float anguloMaximo = 45f; // Ángulo máximo de oscilación
    public float velocidad = 2f;     // Velocidad de movimiento

    private float anguloInicial;

    void Start()
    {
        // Guardar el ángulo inicial del faro al inicio
        anguloInicial = transform.eulerAngles.y;
    }

    void Update()
    {
        // Calcular el ángulo oscilante usando una onda senoidal
        float anguloActual = Mathf.Sin(Time.time * velocidad) * anguloMaximo;
        transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, anguloInicial + anguloActual, transform.eulerAngles.z));
    }
}

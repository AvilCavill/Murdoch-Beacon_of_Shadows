using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy1 : MonoBehaviour
{
    public float enemySpeed = 1.0f;
    public float rangeDesplazamiento = 4.0f;

    private Vector3 puntoInicial;
    private int faseMovimiento = 0; // 0: derecha, 1: adelante, 2: izquierda, 3: atrás

    void Start()
    {
        // Guardamos la posición inicial del objeto
        puntoInicial = transform.position;
    }

    void Update()
    {
        switch (faseMovimiento)
        {
            // Fase 0: Movimiento hacia la derecha en el eje X
            case 0:
                transform.position += new Vector3(enemySpeed * Time.deltaTime, 0, 0);
                if (transform.position.x >= puntoInicial.x + rangeDesplazamiento)
                {
                    faseMovimiento = 1; // Cambiar a la fase de movimiento en el eje Z positivo
                }
                break;

            // Fase 1: Movimiento hacia adelante en el eje Z
            case 1:
                transform.position += new Vector3(0, 0, enemySpeed * Time.deltaTime);
                if (transform.position.z >= puntoInicial.z + rangeDesplazamiento)
                {
                    faseMovimiento = 2; // Cambiar a la fase de movimiento hacia la izquierda
                }
                break;

            // Fase 2: Movimiento hacia la izquierda en el eje X
            case 2:
                transform.position += new Vector3(-enemySpeed * Time.deltaTime, 0, 0);
                if (transform.position.x <= puntoInicial.x - rangeDesplazamiento)
                {
                    faseMovimiento = 3; // Cambiar a la fase de movimiento en el eje Z negativo
                }
                break;

            // Fase 3: Movimiento hacia atrás en el eje Z
            case 3:
                transform.position += new Vector3(0, 0, -enemySpeed * Time.deltaTime);
                if (transform.position.z <= puntoInicial.z - rangeDesplazamiento)
                {
                    faseMovimiento = 0; // Volver a la fase de movimiento hacia la derecha
                }
                break;
        }
    }
}

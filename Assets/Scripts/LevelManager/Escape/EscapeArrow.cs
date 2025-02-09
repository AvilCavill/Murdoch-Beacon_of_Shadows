using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeArrow : MonoBehaviour
{
    public Transform player;       // Referencia al jugador
    public Transform escapePoint;  // Punto de escape
    public RectTransform arrowUI;  // La imagen de la flecha en la UI

    void Update()
    {
        Vector3 direction = escapePoint.position - player.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        arrowUI.rotation = Quaternion.Euler(0, 0, -angle); // Ajusta la rotación
    }
}

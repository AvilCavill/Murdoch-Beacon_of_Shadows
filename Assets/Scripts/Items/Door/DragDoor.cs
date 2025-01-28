using UnityEngine;

namespace Items.Door
{
    public class DragDoor : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera; // Cámara del jugador
        [SerializeField] private LayerMask doorLayer; // Capa de las puertas
        private HingeJoint currentDoorJoint; // La bisagra de la puerta seleccionada
        private bool isDragging = false; // Si el jugador está arrastrando la puerta
        private float dragMultiplier = 3f; // Sensibilidad del arrastre
        private float returnSpeed = 50f; // Velocidad al volver a la posición inicial
        private float returnThreshold = 3f; // Umbral para considerar la puerta "cerrada"

        void Update()
        {
            // Detectar clic inicial en la puerta
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 10f, doorLayer))
                {
                    HingeJoint joint = hit.collider.GetComponent<HingeJoint>();
                    if (joint != null)
                    {
                        currentDoorJoint = joint;
                        isDragging = true;
                    }
                }
            }

            // Arrastrar la puerta
            if (isDragging && currentDoorJoint != null)
            {
                DragDoorWithMouse();
            }

            // Soltar la puerta
            if (Input.GetMouseButtonUp(0))
            {
                if (currentDoorJoint != null)
                {
                    currentDoorJoint.useMotor = false;
                    currentDoorJoint = null;
                }
                isDragging = false;
            }

            // Volver la puerta a la posición inicial si no está siendo arrastrada
            if (!isDragging && currentDoorJoint == null)
            {
                ResetDoorPosition();
            }
        }

        private void DragDoorWithMouse()
        {
            // Raycast para determinar la posición del mouse en el espacio 3D
            Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
            Plane dragPlane = new Plane(Vector3.up, currentDoorJoint.transform.position);
    
            if (dragPlane.Raycast(ray, out float rayDistance))
            {
                // Calcular el ángulo basado en el movimiento del mouse
                Vector3 hitPoint = ray.GetPoint(rayDistance);
                Vector3 direction = hitPoint - currentDoorJoint.transform.position;
                direction.y = 0;

                // Solo aplicar fuerza si estamos arrastrando
                if (isDragging)
                {
                    float angle = Vector3.SignedAngle(currentDoorJoint.transform.forward, direction, Vector3.up);

                    // Aplicar el ángulo como velocidad al motor
                    JointMotor motor = currentDoorJoint.motor;
                    motor.targetVelocity = angle * dragMultiplier;
                    motor.force = 100f; // Fuerza máxima del motor
                    currentDoorJoint.motor = motor;
                    currentDoorJoint.useMotor = true; // Activar el motor solo cuando se esté arrastrando
                }
                else
                {
                    // Si no se está arrastrando, desactivamos el motor
                    currentDoorJoint.useMotor = false;
                }
            }
        }

        private void ResetDoorPosition()
        {
            foreach (HingeJoint hinge in FindObjectsOfType<HingeJoint>())
            {
                // Obtener el ángulo actual de la puerta
                float currentAngle = hinge.transform.localEulerAngles.y;
                if (currentAngle > 180f) currentAngle -= 360f; // Ajustar ángulos negativos

                // Calcular el paso para volver a la posición inicial
                float step = returnSpeed * Time.deltaTime;
                if (Mathf.Abs(currentAngle) > returnThreshold)
                {
                    JointMotor motor = hinge.motor;
                    motor.targetVelocity = -Mathf.Sign(currentAngle) * step;
                    motor.force = 50f;
                    hinge.motor = motor;
                    hinge.useMotor = true;
                }
                else
                {
                    hinge.useMotor = false; // Detener el motor si está suficientemente cerca
                }
            }
        }
    }
}

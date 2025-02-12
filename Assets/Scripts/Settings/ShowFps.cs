using TMPro;
using UnityEngine;

namespace Settings
{
    public class ShowFps : MonoBehaviour
    {
        public float updateInterval = 0.5f; // Intervalo de tiempo para actualizar los FPS
        public TMP_Text fpsText;

        private float accum = 0f; // Acumulador de tiempo
        private int frames = 0; // Contador de frames
        private float timeLeft; // Tiempo restante para la próxima actualización

        void Start()
        {
            timeLeft = updateInterval;
        }

        void Update()
        {
            timeLeft -= Time.deltaTime;
            accum += Time.timeScale / Time.deltaTime;
            frames++;

            // Si ha pasado el intervalo de tiempo, actualiza los FPS
            if (timeLeft <= 0f)
            {
                float fps = accum / frames; // Calcula el promedio de FPS
                fpsText.text = string.Format("{0:0.} FPS", fps);

                // Reinicia los contadores
                timeLeft = updateInterval;
                accum = 0f;
                frames = 0;
            }
        }
    }
}

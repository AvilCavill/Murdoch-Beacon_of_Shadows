using UnityEngine;

namespace ScoreManager
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager instance;

        public int currentScore = 5000;
        private bool isEscapePhase = false;

        private float escapeTimer = 0f;
        public float losePointsInterval = 1f; // Cada cuanto tiempo pierde puntos
        public int pointsLostPerInterval = 10; // Cuantos puntos pierde
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject); // Mantener entre escenas
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            
            if (isEscapePhase)
            {
                escapeTimer += Time.deltaTime;

                if (escapeTimer >= losePointsInterval)
                {
                    currentScore -= pointsLostPerInterval;
                    currentScore = Mathf.Max(currentScore, 0);
                    escapeTimer = 0f; // Reiniciar el temporizador
                }
                Debug.Log("ScoreManager: " + currentScore);
            }
            
        }
        
        public void AddScore(int amount)
        {
            currentScore += amount;
        }

        public void StartEscapePhase()
        {
            isEscapePhase = true;
        }

        public void StopEscapePhase()
        {
            isEscapePhase = false;
        }

        public int GetScore()
        {
            return currentScore;
        }

        // 🧹 Método para resetear el score a 5000
        public void ResetScore()
        {
            currentScore = 5000;
            isEscapePhase = false;
        }
    }
}
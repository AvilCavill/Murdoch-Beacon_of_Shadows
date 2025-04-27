using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Canvas YouDiedCanvas;
    public Canvas HudCanvas;
    public Canvas PauseMenuCanvas;
    public Canvas YouLoseCanvas;
    public Canvas YouWinCanvas; 
    public HealthBarSystem HealthBarSystem;
    public TMP_Text initalObjectiveText;
    public TMP_Text timerText;
    public TMP_Text escapeText; // Texto de escape

    public Transform escapePoint; // Punto al que el jugador debe llegar
    public float timeLimit = 200f;
    public float WinTime = 300f;
    public float escapeTime = 30f; // Tiempo extra para escapar

    private float timer;
    private bool gameLost = false;
    private bool gameWon = false;
    private bool escapePhase = false; // Nueva fase de escape

    public GameObject escapeMarker; // Un marcador visual para el escape

    void Start()
    {
        YouDiedCanvas.enabled = false;
        YouLoseCanvas.enabled = false;
        YouWinCanvas.enabled = false;
        timer = 0f;
        escapeText.gameObject.SetActive(false); // Ocultar mensaje de escape

        if (timerText != null)
            timerText.text = $"Time left: {timeLimit:F1} s";

        if (escapeMarker != null)
            escapeMarker.SetActive(false); // Ocultar el marcador de escape hasta que sea necesario

        StartCoroutine(HideTextAfterDelay());
    }

    private IEnumerator HideTextAfterDelay()
    {
        yield return new WaitForSeconds(20);
        initalObjectiveText.gameObject.SetActive(false);
    }

    void Update()
    {
        if (HealthBarSystem.healthSlider.value <= 0)
        {
            YouDied();
            return;
        }

        if (!gameLost && !gameWon)
        {
            timer += Time.deltaTime;

            if (!escapePhase)
            {
                float remainingTime = Mathf.Max(timeLimit - timer, 0f);
                if (timerText != null)
                    timerText.text = $"Time left: {remainingTime:F1} s";

                if (timer >= WinTime)
                {
                    StartEscapePhase();
                }
                else if (remainingTime <= 0f)
                {
                    YouLost();
                }
            }
            else
            {
                float remainingEscapeTime = Mathf.Max(escapeTime - (timer - WinTime), 0f);
                if (timerText != null)
                    timerText.text = $"Escape Time: {remainingEscapeTime:F1} s";

                if (remainingEscapeTime <= 0f)
                {
                    YouLost();
                }
            }
        }
    }

    void StartEscapePhase()
    {
        escapePhase = true;
        escapeText.gameObject.SetActive(true);
        escapeText.text = "¡Run to the escape point";
        timerText.color = Color.yellow;

        if (escapeMarker != null)
            escapeMarker.SetActive(true); // Mostrar el marcador de escape

        if (escapePoint == null)
        {
            Debug.LogError("No se ha asignado un punto de escape en el LevelManager.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (escapePhase && other.CompareTag("Player"))
        {
            Debug.Log("Player entered");
            YouWin();
        }
    }

    public void AddTime(float additionalTime)
    {
        timeLimit += additionalTime;
        if (timerText != null)
        {
            float remainingTime = Mathf.Max(timeLimit - timer, 0f);
            timerText.text = $"Time left: {remainingTime:F1} s";
        }
    }

    private void YouDied()
    {
        PauseMenuCanvas.enabled = false;
        HudCanvas.enabled = false;
        YouDiedCanvas.enabled = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void YouLost()
    {
        PauseMenuCanvas.enabled = false;
        HudCanvas.enabled = false;
        YouLoseCanvas.enabled = true;
        YouWinCanvas.enabled = false;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameLost = true;
    }

    public void YouWin()
    {
        PauseMenuCanvas.enabled = false;
        HudCanvas.enabled = false;
        YouWinCanvas.enabled = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameWon = true;
    }

    public void ClassificationMenu()
    {
        SceneManager.LoadScene(2);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ExitMenu()
    {
        HealthBarSystem.healthSlider.value = 100;
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
    }
    
    
    
}

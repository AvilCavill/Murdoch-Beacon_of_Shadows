using System.Collections;
using System.Collections.Generic;
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
    public Canvas YouWinCanvas; // Canvas para la victoria
    public HealthBarSystem HealthBarSystem;
    public TMP_Text initalObjectiveText;

    public float timeLimit = 200f;
    private float timer;
    private bool gameLost = false;
    private bool gameWon = false; // Nuevo indicador para victoria

    public TMP_Text timerText;

    void Start()
    {
        YouDiedCanvas.enabled = false;
        YouLoseCanvas.enabled = false;
        YouWinCanvas.enabled = false; // Asegurarse de que el Canvas de victoria esté desactivado
        timer = 0f;

        // Inicializar texto del temporizador
        if (timerText != null)
        {
            timerText.text = $"Time left: {timeLimit:F1} s";
        }

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
        }

        if (!gameLost && !gameWon) // Procesar solo si no se perdió ni se ganó
        {
            timer += Time.deltaTime;

            // Calcular tiempo restante
            float remainingTime = Mathf.Max(timeLimit - timer, 0f);

            if (timerText != null)
            {
                timerText.text = $"Time left: {remainingTime:F1} s";
            }

            // Detectar si el jugador ha ganado
            if (timer >= 250f) // Si han pasado 10 segundos
            {
                YouWin();
            }

            // Detectar si el jugador ha perdido
            if (remainingTime <= 0f)
            {
                YouLost();
            }

            
        }
    }

    public void AddTime(float additionalTime)
    {
        timeLimit += additionalTime;

        // Actualizar texto del temporizador para reflejar el nuevo tiempo límite
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

    private void YouWin()
    {
        PauseMenuCanvas.enabled = false;
        HudCanvas.enabled = false;
        YouWinCanvas.enabled = true; // Mostrar el Canvas de victoria
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameWon = true;
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

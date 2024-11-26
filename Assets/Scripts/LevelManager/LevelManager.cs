using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Canvas YouDiedCanvas;
    public Canvas HudCanvas;
    public Canvas PauseMenuCanvas;
    public HealthBarSystem HealthBarSystem;
    
    void Start()
    {
        YouDiedCanvas.enabled = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (HealthBarSystem.healthSlider.value<= 0)
        {
            YouDied();
        }
    }
    
    // public void Retry()
    // {
    //     
    // }

    private void YouDied()
    {
        PauseMenuCanvas.enabled = false;
        HudCanvas.enabled = false;
        YouDiedCanvas.enabled = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
    public void ExitMenu()
    {
        SceneManager.LoadScene(0);
    }
}

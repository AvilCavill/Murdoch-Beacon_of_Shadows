using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public GameObject generalHUD;
    public Canvas menu;
    public Button resume;
    public Button quit;

    public bool on;
    public bool off;
    void Start()
    {
        menu.enabled = false;
        off = true;
        on = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (off && Input.GetKeyDown(KeyCode.P))
        {
            generalHUD.SetActive(false);
            Time.timeScale = 0;
            menu.enabled = true;
            off = false;
            on = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (on && Input.GetKeyDown(KeyCode.P))
        {
            generalHUD.SetActive(true);
            Time.timeScale = 1;
            menu.enabled = false;
            off = true;
            on = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }        
    }

    public void ResumeGame()
    {
        generalHUD.SetActive(true);
        Time.timeScale = 1;
        menu.enabled = false;
        off = true;
        on = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}

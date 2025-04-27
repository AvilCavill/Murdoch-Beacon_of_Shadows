using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public GameObject generalHUD;
    public Canvas menu;
    public Button resume;
    public Button quit;
    public AudioSource buttonSound;
    public GameObject controlsMenu;

    public bool on;
    public bool off;
    void Start()
    {
        controlsMenu.SetActive(false);
        Time.timeScale = 1;
        menu.enabled = false;
        off = true;
        on = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (off && Input.GetKeyDown(KeyCode.Escape))
        {
            generalHUD.SetActive(false);
            Time.timeScale = 0;
            menu.enabled = true;
            off = false;
            on = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (on && Input.GetKeyDown(KeyCode.Escape))
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
        buttonSound.Play();
    }

    public void ShowControls()
    {
        controlsMenu.SetActive(true);
    }

    public void ReturnToPauseMenu()
    {
        controlsMenu.SetActive(false);
        menu.enabled = true;
    }
    
    public void ExitGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(0);
        buttonSound.Play();
    }
}

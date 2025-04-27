using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLogic : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject loadingMenu;
    public AudioSource buttonSound;
    
    void Start()
    {
        
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
        loadingMenu.SetActive(false);
        
    }

    public void StartGame()
    {
        buttonSound.Play();
        mainMenu.SetActive(false);
        loadingMenu.SetActive(true);
        Invoke("LoadGame", 2f);
    }

    public void LoadGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void OptionsButton()
    {
        buttonSound.Play();
        mainMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ExitGameButton()
    {
        buttonSound.Play();
        Application.Quit(); 
    }

    public void ReturnToMainMenuButton()
    {
        buttonSound.Play();
        mainMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }
    void Update()
    {
        
    }
}

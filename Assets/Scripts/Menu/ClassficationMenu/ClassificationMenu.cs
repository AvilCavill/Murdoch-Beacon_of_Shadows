using System;
using System.Collections;
using System.Collections.Generic;
using Menu.Login_Register_Menu.Api_Actions;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ClassificationMenu : MonoBehaviour
{
    public NetworkingDataScriptableObject networkingData;

    public GameObject listTile;
    public GameObject leaderBoardPanel;
    
    


    private void Start()
    {
        StartCoroutine(GetClasification());
    }

    private IEnumerator GetClasification()
    {
        UnityWebRequest httpRequest = UnityWebRequest.Get(networkingData.apiUrl + "/classification/" + networkingData.token);
    
        httpRequest.SetRequestHeader("Accept", "application/json");
     
        yield return httpRequest.SendWebRequest();
    
        if (httpRequest.result != UnityWebRequest.Result.Success)
        {
            throw new Exception(httpRequest.error);
        }

        // DEBUG opcional para ver el JSON crudo
        Debug.Log(httpRequest.downloadHandler.text);

        var response = JsonConvert.DeserializeObject<ClasificationResponse>(httpRequest.downloadHandler.text);
        var clasification = response.data;

        foreach (var gameL1Data in clasification)
        {
            GameObject newLine = Instantiate(listTile, leaderBoardPanel.transform);
            newLine.GetComponent<TextMeshProUGUI>().text = gameL1Data.name + "\t\t" + gameL1Data.puntuacion;
        }
    }

    

    
    public void ReturnMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}

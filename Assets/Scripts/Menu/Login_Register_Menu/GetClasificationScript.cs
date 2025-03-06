using System;
using System.Collections;
using System.Collections.Generic;
using Menu.Login_Register_Menu.Api_Actions;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Menu.Login_Register_Menu
{
   public class GetClasificationScript : MonoBehaviour
   {
      public NetworkingDataScriptableObject loginDataSO;
      public GameObject listTile;
      
      public GameObject leaderBoardPanel;
      
      public void GetClasificationTable()
      {
         StartCoroutine(GetClasification());
         Debug.Log("Get Clasification Table...");
      }

      private IEnumerator GetClasification()
      {
         UnityWebRequest httpRequest = UnityWebRequest.Get(loginDataSO.apiUrl + "/LeaderboardL1/GetClassificationLevel1");
         httpRequest.SetRequestHeader("Accept", "application/json");
         httpRequest.SetRequestHeader("Authorization", "bearer" + loginDataSO.token);
         
         yield return httpRequest.SendWebRequest();

         if (httpRequest.result != UnityWebRequest.Result.Success)
         {
            throw new Exception(httpRequest.error);
         }
         
         var clasification = JsonConvert.DeserializeObject<List<GameLevel1Dto>>(httpRequest.downloadHandler.text);

         foreach (var gameL1Data in clasification)
         {
            GameObject newLine = Instantiate(listTile, leaderBoardPanel.transform);
            newLine.GetComponent<TextMeshProUGUI>().text = gameL1Data.UserName + "\t" + gameL1Data.temps;
         }
         
      }
   }
}

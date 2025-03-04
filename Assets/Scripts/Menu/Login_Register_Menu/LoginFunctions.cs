using System;
using System.Collections;
using System.Text;
using Menu.Login_Register_Menu.Api_Actions;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Menu.Login_Register_Menu
{
    public class LoginFunctions : MonoBehaviour
    {
        public NetworkingDataScriptableObject loginDataSO;

        public GameObject usuari;
        public TMP_InputField userInput;
        public TMP_InputField emailInput;
        public TMP_InputField passwordInput;
        
        
        public void Login()
        {
            Debug.Log("Login...");
            StartCoroutine(TryLogin());
        }

        private IEnumerator TryLogin()
        {
            if (usuari == null)
            {
                UnityWebRequest httpClient = new UnityWebRequest();
                httpClient.method = UnityWebRequest.kHttpVerbPOST;
                httpClient.url = loginDataSO.apiUrl + "/Auth/Login";
                httpClient.SetRequestHeader("Content-Type", "application/json");
                httpClient.SetRequestHeader("Accept", "application/json");
                
                RegisterUserDTO loginDataUsuari = new RegisterUserDTO();
                loginDataUsuari.nom_usuari = userInput.text;
                loginDataUsuari.email = emailInput.text;
                loginDataUsuari.password = passwordInput.text;
                Debug.Log(loginDataUsuari.nom_usuari);
                Debug.Log(loginDataUsuari.email);
                Debug.Log(loginDataUsuari.password);
                
                string jsonData = JsonConvert.SerializeObject(loginDataUsuari);
                byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
                
                httpClient.uploadHandler = new UploadHandlerRaw(dataToSend);
                httpClient.downloadHandler = new DownloadHandlerBuffer();
                
                yield return httpClient.SendWebRequest();

                if (httpClient.result == UnityWebRequest.Result.ConnectionError || httpClient.result ==UnityWebRequest.Result.ProtocolError)
                {
                    throw new Exception("Login: " + httpClient.error);
                }
                
                string jsonResponse = httpClient.downloadHandler.text;
                
                AuthTokenDto authTokenDto = JsonConvert.DeserializeObject<AuthTokenDto>(jsonResponse);
                loginDataSO.token = authTokenDto.token;
                Debug.Log(authTokenDto.token);
                httpClient.Dispose();
            }
        }
        
    }
}

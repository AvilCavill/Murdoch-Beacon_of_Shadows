using System.Collections;
using System.Text;
using Menu.Login_Register_Menu.Api_Actions;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Menu.Login_Register_Menu
{
    public class RegisterFunctions : MonoBehaviour
    {
        public NetworkingDataScriptableObject loginDataSO;
    
        public TextMeshProUGUI nomUsuari;
        public TextMeshProUGUI emailInput;
        public TextMeshProUGUI passwordInput;
        public void register()
        {
            Debug.Log("Register...");
            StartCoroutine(TryRegister());
        }

        private IEnumerator TryRegister()
        {
            UnityWebRequest httpRequest = new UnityWebRequest();
            httpRequest.method = UnityWebRequest.kHttpVerbPOST;
            httpRequest.url = loginDataSO.apiUrl + "/Auth/Register";
            httpRequest.SetRequestHeader("Content-Type", "application/json");
            httpRequest.SetRequestHeader("Accept", "application/json");
            
            RegisterUserDTO registerUserDto = new RegisterUserDTO();
            registerUserDto.nom = nomUsuari.text;
            registerUserDto.email = emailInput.text;
            registerUserDto.password = passwordInput.text;
            
            string jsonData = JsonConvert.SerializeObject(registerUserDto);
            byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);

            yield return httpRequest.SendWebRequest();

            if (httpRequest.result == UnityWebRequest.Result.ConnectionError || httpRequest.result ==UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error: " + httpRequest.error);
            }
             
            Debug.Log(httpRequest.result.ToString());
        
            string jsonResponse = httpRequest.downloadHandler.text;
            UserDTO registeredUser = JsonConvert.DeserializeObject<UserDTO>(jsonResponse);
            Debug.Log("Usuari creat " + registeredUser.id_usuari + " : " + registeredUser.nom_usuari + " : " + registeredUser.email);
        }
    }
}

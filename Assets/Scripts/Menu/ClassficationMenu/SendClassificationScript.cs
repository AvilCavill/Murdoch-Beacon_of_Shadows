using System.Collections;
using System.Text;
using Menu.Login_Register_Menu.Api_Actions;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

namespace Menu.ClassficationMenu
{
    public class SendClassificationScript : MonoBehaviour
    {
 
        public TMP_InputField nombreUsuario;
        public TextMeshProUGUI puntuaciónTexto;
        public float puntuación;
    
        public NetworkingDataScriptableObject networkingData;

        private void Start()
        {
            puntuación = ScoreManager.ScoreManager.instance != null ? ScoreManager.ScoreManager.instance.GetScore() : 0;
            puntuaciónTexto.text = "Puntuación: " + puntuación; 
        }

        public void SendClasification()
        {
            StartCoroutine(SendDataClassification());
            
        }

        private IEnumerator SendDataClassification()
        {
            UnityWebRequest httpRequest = new UnityWebRequest();
            httpRequest.method = UnityWebRequest.kHttpVerbPOST;
            httpRequest.url = networkingData.apiUrl + "/classification";
            httpRequest.SetRequestHeader("Content-Type", "application/json");
            httpRequest.SetRequestHeader("Accept", "application/json");
        
            SendClassificationDTO dataClassification = new SendClassificationDTO();
            dataClassification.api_token = networkingData.token;
            dataClassification.name = nombreUsuario.text;
            dataClassification.puntuacion = puntuación;
        
            Debug.Log(dataClassification.api_token);
            Debug.Log(dataClassification.name);
            Debug.Log(dataClassification.puntuacion);
        
            string jsonData = JsonConvert.SerializeObject(dataClassification);
            byte[] dataToSend = Encoding.UTF8.GetBytes(jsonData);
            httpRequest.uploadHandler = new UploadHandlerRaw(dataToSend);
            httpRequest.downloadHandler = new DownloadHandlerBuffer();

            yield return httpRequest.SendWebRequest();

            if (httpRequest.result == UnityWebRequest.Result.ConnectionError || httpRequest.result ==UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("Error: " + httpRequest.error);
            }
             
            Debug.Log(httpRequest.result.ToString());
        
            string jsonResponse = httpRequest.downloadHandler.text;
            UserDTO registeredUser = JsonConvert.DeserializeObject<UserDTO>(jsonResponse);
        }
    
    
    }
}

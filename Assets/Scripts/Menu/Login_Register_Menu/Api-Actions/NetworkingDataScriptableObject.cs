using UnityEngine;

namespace Menu.Login_Register_Menu.Api_Actions
{
    [CreateAssetMenu(fileName = "LoginData", menuName = "ScriptableObjects/NetworkingManagerScriptableObject", order = 1)]

    public class NetworkingDataScriptableObject : ScriptableObject
    {
        public string apiUrl = "https://api-murdoch-beacon-of-shadows.azurewebsites.net/api";
        public string token;
    }
}
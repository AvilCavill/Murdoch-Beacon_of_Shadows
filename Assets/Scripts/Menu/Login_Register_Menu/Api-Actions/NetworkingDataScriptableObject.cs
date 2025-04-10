using UnityEngine;

namespace Menu.Login_Register_Menu.Api_Actions
{
    [CreateAssetMenu(fileName = "LoginData", menuName = "ScriptableObjects/NetworkingManagerScriptableObject", order = 1)]

    public class NetworkingDataScriptableObject : ScriptableObject
    {
        public string apiUrl = "https://phpstack-1076337-5399863.cloudwaysapps.com/api";
        public string token = "GHCki2tUqpMxPYBI6KnHEZD98ffEEeZMcts6J1QAh7FLDh3H7MpLcX4YQ7km";
    }
}
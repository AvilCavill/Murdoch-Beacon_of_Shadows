using UnityEngine;

namespace Menu.Login_Register_Menu
{
    public class Login_Register_Logic : MonoBehaviour
    {
        public GameObject loginPanel;
        public GameObject registerPanel;
        public GameObject mainMenu;
        public GameObject leaderBoardMenu;

        private void Start()
        {
            leaderBoardMenu.SetActive(false);
            loginPanel.SetActive(false);
            registerPanel.SetActive(false);
        }

        public void RegisterButton()
        {
            registerPanel.SetActive(true);
            mainMenu.SetActive(false);
            loginPanel.SetActive(false);
            leaderBoardMenu.SetActive(false);
        }

        public void LoginButton()
        {
            loginPanel.SetActive(true);
            registerPanel.SetActive(false);
            mainMenu.SetActive(false);
            leaderBoardMenu.SetActive(false);
        }

        public void LeaderBoardButton()
        {
            leaderBoardMenu.SetActive(true);
            loginPanel.SetActive(false);
            registerPanel.SetActive(false);
            mainMenu.SetActive(false);
        }
        
        public void GoBackButton()
        {
            mainMenu.SetActive(true);
            registerPanel.SetActive(false);
            loginPanel.SetActive(false);
            leaderBoardMenu.SetActive(false);
        }
    }
}

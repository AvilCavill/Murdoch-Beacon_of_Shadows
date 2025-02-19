using UnityEngine;

namespace Menu.Login_Register_Menu
{
    public class Login_Register_Logic : MonoBehaviour
    {
        public GameObject loginPanel;
        public GameObject registerPanel;
        public GameObject mainMenu;

        private void Start()
        {
            loginPanel.SetActive(false);
            registerPanel.SetActive(false);
        }

        public void RegisterButton()
        {
            registerPanel.SetActive(true);
            mainMenu.SetActive(false);
            loginPanel.SetActive(false);
        }

        public void LoginButton()
        {
            loginPanel.SetActive(true);
            registerPanel.SetActive(false);
            mainMenu.SetActive(false);
        }

        public void GoBackButton()
        {
            mainMenu.SetActive(true);
            registerPanel.SetActive(false);
            loginPanel.SetActive(false);
        }
    }
}

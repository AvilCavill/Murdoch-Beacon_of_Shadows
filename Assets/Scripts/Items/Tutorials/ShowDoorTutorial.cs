using UnityEngine;

namespace Items.Tutorials
{
    public class ShowDoorTutorial : MonoBehaviour
    {
        public GameObject doorTutorialText;
        // Start is called before the first frame update
        void Start()
        {
            doorTutorialText.SetActive(false);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                doorTutorialText.SetActive(true);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                doorTutorialText.SetActive(false);
            }
        }
    }
}

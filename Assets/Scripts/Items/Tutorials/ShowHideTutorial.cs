using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideTutorial : MonoBehaviour
{
    public GameObject hideTutorialText;
    // Start is called before the first frame update
    void Start()
    {
        hideTutorialText.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hideTutorialText.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hideTutorialText.SetActive(false);
        }
    }
}

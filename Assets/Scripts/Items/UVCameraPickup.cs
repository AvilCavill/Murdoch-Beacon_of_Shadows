using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVCameraPickup : MonoBehaviour
{
    public GameObject PickUpText;
    public GameObject UVCameraOnPlayer;
    public UVCamera uvCamera;
    public Transform cameraTransform;
    // Start is called before the first frame update
    void Start()
    {
        UVCameraOnPlayer.SetActive(false);
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PickUpText.SetActive(true);
            if (Input.GetKey(KeyCode.E))
            {
                uvCamera.hasUVCamera = true;
                this.gameObject.SetActive(false);
                UVCameraOnPlayer.SetActive(true);
                PickUpText.SetActive(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PickUpText.SetActive(false);
    }
}

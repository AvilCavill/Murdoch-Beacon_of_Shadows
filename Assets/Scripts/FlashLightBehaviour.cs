using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLightBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    public Light flashLight;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            flashLight.enabled = !flashLight.enabled;
        }
    }
}

using System;
using System.Collections;
using UnityEngine;

namespace Items.CameraUV
{
    public class CameraFlash : MonoBehaviour
    {
        public Light flashCamera;
        public float flashDuration = 0.1f;

        private void Start()
        {
            flashCamera.enabled = false;
        }

        public void TriggerFlash()
        {
            StartCoroutine(FlashRoutine());
        }
    
        private IEnumerator FlashRoutine()
        {
            flashCamera.enabled = true;
        
            yield return new WaitForSeconds(flashDuration);
        
            flashCamera.enabled = false;
        }
    }
}

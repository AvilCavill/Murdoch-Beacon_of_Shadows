using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideSpot : MonoBehaviour
{
   public Transform hidePoint;
   private bool isPlayerNear = false;

   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         isPlayerNear = true;
      }
   }

   private void OnTriggerExit(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         isPlayerNear = false;
      }
   }

   private void Update()
   {
      if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
      {
         PlayerController player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
         if (player.IsHiding())
         {
            player.SetHiding(false);
         }
         else
         {
            player.transform.position = hidePoint.position;
            player.SetHiding(true);
         }
      }
   }
}

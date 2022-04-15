using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class CameraFollow : MonoBehaviour {
      public GameObject player;
      public GameObject facingTarget;
      // public Vector3 offset = new Vector3(0f, 2.65f, -3f);
  
      void Update()
      {
        Vector3 pos = player.transform.position;
        transform.position = pos;

        transform.LookAt(facingTarget.transform);
      }
  }
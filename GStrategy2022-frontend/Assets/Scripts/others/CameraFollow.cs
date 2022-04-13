using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 public class CameraFollow : MonoBehaviour {
      public GameObject player;
      public Vector3 offset = new Vector3(0f, 3f, -2.5f);
  
      void Update()
      {
        Vector3 pos = player.transform.position;
        transform.position = pos + offset;
      }
  }
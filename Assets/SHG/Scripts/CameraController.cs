using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
  [SerializeField]
  Transform cameraFollow;
  [SerializeField]
  Vector3 followOffset;

  Transform player;
  
  // Start is called before the first frame update
  void Start()
  {
    this.player = GameObject.FindWithTag("Player").transform;
  }

  void LateUpdate()
  {
    this.cameraFollow.position = this.player.position + this.followOffset;
  }
}

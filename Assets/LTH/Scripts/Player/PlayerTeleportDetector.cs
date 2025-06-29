using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeleportDetector : MonoBehaviour
{
    private PlayerMovement movement;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Teleport"))
        {
            movement.SetZFixed(false);
            float teleportZ = other.transform.position.z;
            movement.ForceZPosition(teleportZ);
        }
    }
}

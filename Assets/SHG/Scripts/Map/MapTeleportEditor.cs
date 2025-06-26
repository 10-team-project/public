using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  [RequireComponent (typeof(MapTeleporter))]
  public class MapTeleportEditor : MonoBehaviour
  {
    [SerializeField, ReadOnly]
    MapTeleportPoint pointA;
    [SerializeField, ReadOnly]
    MapTeleportPoint pointB;

    const float INTERACT_DISTANCE = 1.5f;

    MapTeleporter mapTeleporter;

    void Awake()
    {
      this.mapTeleporter = this.GetComponent<MapTeleporter>();
    }

    #if UNITY_EDITOR
    [Button ("Create new teleport points")]
    void CreateNewTeleportPoints(string name)
    {
      if (name.Replace(" ", "").Length == 0) {
        Debug.LogError("Name is empty");
        return;
      }
      var pointA = this.CreateTeleportPoint(name);   
      var pointB = this.CreateTeleportPoint(name);
      pointA.transform.parent = this.transform;
      pointB.transform.parent = this.transform;
      this.pointA = pointA;
      this.pointB = pointB;
    }

    MapTeleportPoint CreateTeleportPoint(string name)
    {
      var gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
      gameObject.name = name;
      var teleportPoint = gameObject.AddComponent<MapTeleportPoint>();
      var collider = gameObject.GetComponent<SphereCollider>();
      collider.radius = INTERACT_DISTANCE;
      collider.isTrigger = true;
      gameObject.layer = LayerMask.NameToLayer(MapTeleporter.TELEPORT_POINT_LAYER);
      gameObject.tag = MapTeleporter.TELEPORT_POINT_TAG;
      return (teleportPoint);
    }

    void AddTeleportPointsTest(string name)
    {
      if (this.pointA == null || this.pointB == null) {
        Debug.LogError($"teleport points are not set");
        return ;
      }
      try {
        this.mapTeleporter.AddPointPair(name, this.pointA, this.pointB);
      }
      catch (Exception e) {
        Debug.LogError(e.Message);
      }
      this.pointA = null;
      this.pointB = null; 
    }
    #endif
  }
}

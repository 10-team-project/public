using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  [RequireComponent (typeof(MapTeleporter))]
  public class MapTeleportTest : MonoBehaviour
  {
    [SerializeField]
    MapTeleportPoint pointA;
    [SerializeField]
    MapTeleportPoint pointB;

    MapTeleporter mapTeleporter;

    void Awake()
    {
      this.mapTeleporter = this.GetComponent<MapTeleporter>();
    }

    [Button ("Add teleport points")]
    void TestAddTeleportPoints()
    {
      if (this.pointA == null || this.pointB == null) {
        Debug.LogError($"teleport points are not set");
        return ;
      }
      this.mapTeleporter.AddPointPair(this.pointA, this.pointB);
      this.pointA = null;
      this.pointB = null; 
    }
  }
}

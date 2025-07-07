using System;
using System.Collections.Generic;
using UnityEngine;

namespace SHG
{
  [Serializable]
  struct TeleportPointPair: IEquatable<TeleportPointPair>
  {
    public string Name { get; private set; }
    public MapTeleportPoint PointA { get; private set; }
    public MapTeleportPoint PointB { get; private set; } 

    public TeleportPointPair(string name, MapTeleportPoint pointA, MapTeleportPoint pointB)
    {
      this.Name = name;
      this.PointA = pointA;
      this.PointB = pointB;
    }

    public bool Contains(MapTeleportPoint point)
    {
      return (this.PointA == point || this.PointB == point);
    }

    public bool Equals(TeleportPointPair other)
    {
      if (other is TeleportPointPair teleportPointPair) {
        return (this.PointA == teleportPointPair.PointA &&
          this.PointB == teleportPointPair.PointB);
      }
      return (false);
    }

    public override bool Equals(object obj)
    {
      if (obj is TeleportPointPair other) {
        return (this.Equals(other));
      }
      return (false);
    }

    public override int GetHashCode()
    {
      return base.GetHashCode();
    }

    public override string ToString()
    {
      return base.ToString();
    }

    public static bool operator== (TeleportPointPair lhs, TeleportPointPair rhs) {
      return (lhs.Equals(rhs));
    }

    public static bool operator!= (TeleportPointPair lhs, TeleportPointPair rhs) {
      return (!(lhs == rhs));
    }
  }

  public class MapTeleporter : MonoBehaviour
  {
    public const string TELEPORT_POINT_TAG = "TeleportPoint";
    public const string TELEPORT_POINT_LAYER = "Teleport";
    [SerializeField]
    Dictionary<string, TeleportPointPair> teleportPoints = new ();

    public void AddPointPair(string name, MapTeleportPoint pointA, MapTeleportPoint pointB)
    {
      if (this.teleportPoints.ContainsKey(name)) {
        throw (new ArgumentException($"TeleportPoint for {name} is exist"));
      }
      pointA.OnTrigger += this.OnTriggerTeleportFrom;
      pointB.OnTrigger += this.OnTriggerTeleportFrom;
      this.teleportPoints.Add(name, new TeleportPointPair (name, pointA, pointB));
    }

    void OnTriggerTeleportFrom(MapTeleportPoint point, Transform playerTransform)
    {
      var pair = this.FindPair(point);
      if (pair == null) {
        throw (new ApplicationException("Unable to find opponent point to teleport"));
      }
      if (pair.PointA == point) {
        this.MovePlayerTo(pair.PointB.TeleportPosition,
          playerTransform);
      }
      else {
        this.MovePlayerTo(pair.PointA.TeleportPosition, playerTransform);
      }
    }

    TeleportPointPair FindPair(MapTeleportPoint point)
    {
      foreach (var (name, pointPair) in this.teleportPoints) {
        if (pointPair.Contains(point)) {
          return (pointPair);
        } 
      }
      throw (new ApplicationException($"Unable to find teleport point for {point}"));
    }

    void MovePlayerTo(Vector3 position, Transform playerTransform)
    {
      playerTransform.position = position;
    }

    void Start()
    {
      this.FindAllTeleportPointInMap();
    }

    void FindAllTeleportPointInMap()
    {
      var points = GameObject.FindGameObjectsWithTag("TeleportPoint");
      var addedNames = new HashSet<string>();
      foreach (var point in points) {
        if (addedNames.Contains(point.name)) {
          continue;
        }
        var found = Array.Find(points, p => 
          p != point && p.name == point.name 
          );
        if (found == null) {
          Debug.LogError($"No teleport pair for {point.name}");
          continue;
        }
        var pointA = point.GetComponent<MapTeleportPoint>();
        var pointB = found.GetComponent<MapTeleportPoint>();
        if (pointA == null || pointB == null) {
          Debug.LogError($"points not have {nameof(MapTeleportPoint)} component {found.name} {point.name}");
          continue;
        }
        this.AddPointPair(point.name, pointA, pointB);
        addedNames.Add(point.name);
      }
    }
  }
}

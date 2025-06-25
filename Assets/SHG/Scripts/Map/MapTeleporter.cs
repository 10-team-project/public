using System;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;

namespace SHG
{
  [Serializable]
  struct TeleportPointPair: IEquatable<TeleportPointPair>
  {
    public MapTeleportPoint PointA { get; private set; }
    public MapTeleportPoint PointB { get; private set; } 

    public TeleportPointPair(MapTeleportPoint pointA, MapTeleportPoint pointB)
    {
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
    [SerializeField]
    List<TeleportPointPair> teleportPoints = new ();

    public void AddPointPair(MapTeleportPoint pointA, MapTeleportPoint pointB)
    {
      if (this.teleportPoints.FindIndex(
          pair => pair.Contains(pointA) || pair.Contains(pointB)
          ) != -1) {
        throw (new ArgumentException($""));
      }
      pointA.OnTrigger += this.OnTriggerTeleportFrom;
      pointB.OnTrigger += this.OnTriggerTeleportFrom;
      this.teleportPoints.Add(new TeleportPointPair (pointA, pointB));
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
      return (this.teleportPoints.Find(pair => pair.Contains(point)));
    }

    void MovePlayerTo(Vector3 position, Transform playerTransform)
    {
      playerTransform.position = position;
    }
  }
}

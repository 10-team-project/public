using System;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;

[Serializable]
public class ItemSpawn
{
  [SerializeField, ShowInInspector] HashSet<ItemData> itemsToSpawn;
  [SerializeField] List<Transform> spawnPoints;
  Dictionary<int, ItemData> items;

  public ItemSpawn()
  {
    this.itemsToSpawn = new();
    this.spawnPoints = new();
    this.items = new();
  }

  public void AddSpawnPoints(params Transform[] points)
  {
    foreach (var point in points) {
       this.spawnPoints.Add(point); 
    }
  }

  public void AddItems(params ItemData[] items)
  {
    foreach (var item in items) {
      if (!this.itemsToSpawn.Contains(item)) {
        this.itemsToSpawn.Add(item);
        this.items[this.items.Count] = item;
      }
    }
  }

  public void SpawnItemCount(int count)
  {
    if (itemsToSpawn.Count == 0 || spawnPoints.Count == 0) return;

    List<Transform> points = new List<Transform>(spawnPoints);
    Shuffle(points);

    int spawnCount = Mathf.Min(count, points.Count);

    for (int i = 0; i < spawnCount; i++)
    {
      int itemIndex = UnityEngine.Random.Range(0, this.items.Count );
      var itemObject = Item.CreateItemObjectFrom(this.items.GetValueOrDefault(itemIndex));
      itemObject.transform.position = points[i].position;
    }
  }

  private void Shuffle(List<Transform> list) // Fisher-Yates 셔플 알고리즘
  {
    for (int i = list.Count - 1; i > 0; i--)
    {
      int j = UnityEngine.Random.Range(0, i + 1);
      (list[i], list[j]) = (list[j], list[i]);
    }
  }
}


using UnityEngine;
using EditorAttributes;

namespace SHG
{
  public class ItemSpawnTest : MonoBehaviour
  {
    [SerializeField, ReadOnly]
    ItemSpawn itemSpawner = new();
    const string ITEM_DIR = "Assets/SHG/Test/Items";
    [SerializeField]
    Transform[] spawnPoints;

    void Awake()
    {
      var items = Utils.LoadAllFrom<ItemData>(ITEM_DIR);
      this.itemSpawner.AddItems(items);
      this.itemSpawner.AddSpawnPoints(this.spawnPoints);
    }

    [Button ("Spawn item")]
    public void SpawnItem(int count)
    {
      this.itemSpawner.SpawnItemCount(count);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
  }
}

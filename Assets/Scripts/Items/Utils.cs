using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SHG
{
  public static partial class Utils 
  {
    public static T[] LoadAllFrom<T>(in string dir) where T: UnityEngine.Object
    {
#if UNITY_EDITOR
      string[] guids = AssetDatabase.FindAssets(
        $"t:{typeof(T).Name}", new[] { dir });
      int count = guids.Length;
      if (count == 0) {
        Debug.Log($"No {typeof(T).Name} is found in {dir}");
      }
      T[] loaded = new T[count];
      for(int i = 0; i < count; i++) {
        var path = AssetDatabase.GUIDToAssetPath(guids[i]);
        loaded[i] = AssetDatabase.LoadAssetAtPath<T>(path);
      }
      return (loaded);
#else
      T[] loaded = Resources.LoadAll(dir) as T[];
      if (loaded.Length == 0) {
        Debug.Log($"No {typeof(T).Name} is found in {dir}");
      }
      return (loaded);
#endif
    }
  }
}

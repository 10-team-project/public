using System.Collections.Generic;
using System;
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
//#if UNITY_EDITOR
//      string[] guids = AssetDatabase.FindAssets(
//        $"t:{typeof(T).Name}", new[] { dir });
//      int count = guids.Length;
//      if (count == 0) {
//        Debug.Log($"No {typeof(T).Name} is found in {dir}");
//      }
//      T[] loaded = new T[count];
//      for(int i = 0; i < count; i++) {
//        var path = AssetDatabase.GUIDToAssetPath(guids[i]);
//        loaded[i] = AssetDatabase.LoadAssetAtPath<T>(path);
//      }
//      return (loaded);
//#else
      object[] objects = Resources.LoadAll(dir);
      T[] loaded = new T[objects.Length];
      for (int i = 0; i < loaded.Length; i++) {
        loaded[i] = objects[i] as T;
        if (loaded[i] == null) {
          Debug.Log($"type mismatch: {objects[i]}");
        }
      }
      if (loaded.Length == 0) {
        Debug.Log($"No {typeof(T).Name} is found in {dir}");
        return (null);
      }
      return (loaded);
//#endif
    }
  }
}

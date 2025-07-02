using System;
using UnityEngine;
using UnityEditor;

namespace SHG
{
  public class ScriptableObjectIdAttribute : PropertyAttribute
  {
  }

  #if UNITY_EDITOR
  [CustomPropertyDrawer (typeof(ScriptableObjectIdAttribute))]  
    public class ScriptableObejctIdDrawer: PropertyDrawer 
  {
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      GUI.enabled = false;
      if (string.IsNullOrEmpty(property.stringValue)) {
        property.stringValue = Guid.NewGuid().ToString();
      }
      EditorGUI.PropertyField(
        position: position,
        property: property,
        label: label,
        includeChildren: true);
      GUI.enabled = true;
    }
  }
  #endif

  public class IdentifiableScriptableObject: ScriptableObject
  {
    [SerializeField, ScriptableObjectIdAttribute]
    string uuid;
    public string Id => this.uuid; 
  }
}

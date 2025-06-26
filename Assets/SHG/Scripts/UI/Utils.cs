using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public static partial class Utils 
{

  public static T FindUIElementFrom<T>(VisualElement target) where T: VisualElement
  {
    if (target == null) {
      return (null);
    }
    if (target is T targetElement) {
      return (targetElement);
    }
    return (target.GetFirstAncestorOfType<T>());
  }
}

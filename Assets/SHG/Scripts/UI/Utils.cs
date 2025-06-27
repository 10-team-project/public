using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace SHG
{
  public static partial class Utils 
  {
    public static void ShowVisualElement<T>(T element) where T: VisualElement, IHideableUI
    {
      element.style.display = DisplayStyle.Flex;
      element.BringToFront();
    }
    public static void HideVisualElement(VisualElement element)
    {
      element.style.display = DisplayStyle.None;
      element.SendToBack();
    }

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

}

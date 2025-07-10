using System;
using System.Collections;
using System.Collections.Generic;
using LTH;
using Patterns;
using UnityEditor;
using UnityEngine;

namespace LTH
{
    public class PopupManager : SingletonBehaviour<PopupManager>
    {
        [SerializeField] private List<GameObject> popupPrefabs;
        [SerializeField] private Transform popupParent;

        private Dictionary<Type, BasePopup> instances = new();

        public T ShowPopup<T>() where T : BasePopup
        {
            if (!instances.TryGetValue(typeof(T), out var popup))
            {
                var prefab = popupPrefabs.Find(p => p.GetComponent<T>() != null);

                if (prefab == null) return null;

                popup = Instantiate(prefab, popupParent).GetComponent<T>();
                instances[typeof(T)] = popup;
            }

            popup.Open();
            return (T)popup;
        }
    }
}

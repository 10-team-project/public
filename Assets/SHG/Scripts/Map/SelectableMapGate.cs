using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SHG
{
  static class MapUI
  {
    public static GameObject LIBRARY_SCIENCE;
    public static GameObject ROOFTOP;
    public static GameObject RESTAURANT_CAFE;

    static MapUI()
    {
      LIBRARY_SCIENCE = Resources.Load<GameObject>("LibraryScienceMapUI");
      ROOFTOP = Resources.Load<GameObject>("RooftopMapUI");
      RESTAURANT_CAFE = Resources.Load<GameObject>("RestaurantCafeMapUI");
    }
  }

  public class SelectableMapGate : MapGate
  {
    bool IsPresentingWindow = false;
    MapSelectWindow LibararyScience;
    MapSelectWindow Rooftop;
    MapSelectWindow RestaurantCafe;
    int currentUIIndex;
    MapSelectWindow[] Windows;

    protected override void Awake()
    {
      base.Awake();
      this.currentUIIndex = 0;
      this.LibararyScience = Instantiate(MapUI.LIBRARY_SCIENCE).GetComponent<MapSelectWindow>();
      this.RestaurantCafe = Instantiate(MapUI.RESTAURANT_CAFE).GetComponent<MapSelectWindow>();
      this.Rooftop = Instantiate(MapUI.ROOFTOP).GetComponent<MapSelectWindow>();
      this.Windows = new MapSelectWindow[]
      {
        this.LibararyScience,
        this.RestaurantCafe,
        this.Rooftop
      };
      foreach (var window in this.Windows) {
        this.InitWindow(window); 
      }
    }

    void InitWindow(MapSelectWindow window)
    {
      window.OnClickNext += this.OnClickNext;
      window.OnClickExit += this.HideWindow;
      window.OnClickPrev += this.OnClickPrev;
      window.OnClickMapButton += this.OnClickMapButton;
    }

    void OnClickMapButton(GameScene scene)
    {
      var mode = App.Instance.CurrentMode as ShelterMode; 
      if (scene.Name == "Rooftop" && 
        !App.Instance.ItemTracker.HasRadioItem) {
        return ;
      }
      mode.OnEnterFarmingGate(scene);
    }

    void OnClickPrev()
    {
      this.HideWindow();
      this.currentUIIndex = Math.Max(this.currentUIIndex - 1, 0);
      this.ShowWindow();
    }

    void OnClickNext()
    {
      this.HideWindow();
      currentUIIndex = Math.Min(currentUIIndex + 1, this.Windows.Length - 1);
      this.ShowWindow();
    }

    void HideWindow()
    {
      this.Windows[this.currentUIIndex].gameObject.SetActive(false);
      this.IsPresentingWindow = false;
    }

    void ShowWindow()
    {
      this.Windows[this.currentUIIndex].gameObject.SetActive(true);
      this.IsPresentingWindow = true;
    }

    public override void Interact()
    {
      if (!this.IsPresentingWindow) {
        this.ShowWindow();
      }
      else {
        this.HideWindow();
      }
    }
  }
}

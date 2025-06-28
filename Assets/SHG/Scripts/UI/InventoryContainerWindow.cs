using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Patterns;

namespace SHG
{
  public struct WindowTab : IHideableUI, IEquatable<WindowTab>
  {
    public bool IsVisiable => this.inner.IsVisiable;
    IHideableUI inner;
    public VisualElement Content { get; private set; }

    public WindowTab(VisualElement content)
    {
      if (!(content is IHideableUI)) {
        throw (new ArgumentException("Content for window tab need to be IHideableUI"));
      }
      this.Content = content;
      content.AddToClassList("window-tab-container");
      this.inner = (this.Content as IHideableUI);
    }

    public void Hide()
    {
      this.inner.Hide();
    }

    public void Show()
    {
      this.inner.Show();
    }

    public bool Equals(WindowTab other)
    {
      return (other.Content == this.Content);
    }
  }

  public class InventoryContainerWindow : VisualElement, IHideableUI
  {
    public bool IsVisiable { get; private set; }
    public WindowTab StoryItemTab { get; private set; } 
    public WindowTab NormalItemTab { get; private set; }
    public ObservableValue<WindowTab> CurrentTab;
    ItemBox floatingBox;
    Button normalItemTabButton;
    Button storyItemTabButton;
    Label title;

    public InventoryContainerWindow(ItemBox floatingBox)
    {
      this.name = "inventory-window-container";
      this.AddToClassList("window-container");
      this.floatingBox = floatingBox;
      this.CreateUI();
      this.StoryItemTab.Hide();
      this.StoryItemTab.Content.SetEnabled(false);
      this.title.text = "Normal Item";
      this.storyItemTabButton.SetEnabled(true);
      this.CurrentTab = new (this.NormalItemTab);
      this.CurrentTab.Value.Show();
      this.CurrentTab.Value.Content.SetEnabled(true);
      this.normalItemTabButton.SetEnabled(false);
    }

    public void AddDropTargets(IEnumerable<ItemStorageWindow> targets)
    {

      var normalItemTab = this.NormalItemTab.Content as InventoryWindow;
      var storyItemTab = this.StoryItemTab.Content as InventoryWindow; 
      normalItemTab.AddDropTargets(targets);
      storyItemTab.AddDropTargets(targets);
    }

    public void Hide()
    {
      this.IsVisiable = false;
      Utils.HideVisualElement(this);
    }

    public void Show()
    {
      this.IsVisiable = false;
      Utils.ShowVisualElement(this);
    }

    void CreateUI()
    {
      this.title = new Label();
      title.AddToClassList("window-label");
      this.Add(title);
      var tabButtonContainer = new VisualElement();
      tabButtonContainer.name = "inventory-window-tab-button-container";
      this.storyItemTabButton = new Button();
      this.storyItemTabButton.text = "Story";
      this.storyItemTabButton.AddToClassList("inventory-window-tab-button");
      this.storyItemTabButton.RegisterCallback<ClickEvent>(this.OnClickStoryTab);
      this.normalItemTabButton = new Button();
      this.normalItemTabButton.text = "Normal";
      this.normalItemTabButton.AddToClassList("inventory-window-tab-button");
      this.normalItemTabButton.RegisterCallback<ClickEvent>(this.OnClickNormalTab);
      tabButtonContainer.Add(this.storyItemTabButton);
      tabButtonContainer.Add(this.normalItemTabButton);
      this.Add(tabButtonContainer);

      this.NormalItemTab = new WindowTab(
        new InventoryWindow(this.IsNormalItem, this.floatingBox)
        );
      this.StoryItemTab = new WindowTab(
        new InventoryWindow(this.IsStoryItem, this.floatingBox)
        );
      var scrollView = new ScrollView(ScrollViewMode.Vertical);
      scrollView.AddToClassList("item-container-scroll-view");
      scrollView.Add(this.NormalItemTab.Content);
      scrollView.Add(this.StoryItemTab.Content);
      this.Add(scrollView);
      var closeButton = new Button();
      closeButton.text = "close";
      closeButton.RegisterCallback<ClickEvent>(click => this.Hide());
      closeButton.AddToClassList("window-close-button"); 
      this.Add(closeButton);
    }

    void OnClickNormalTab(ClickEvent click)
    {
      this.ChangeTabTo(this.NormalItemTab);
    }

    void OnClickStoryTab(ClickEvent click)
    {
      this.ChangeTabTo(this.StoryItemTab);
    }

    bool IsNormalItem(ItemData item)
    {
      return (!item.IsStoryItem);
    }

    bool IsStoryItem(ItemData item)
    {
      return (item.IsStoryItem);
    }

    void ChangeTabTo(WindowTab tab)
    {
      if (tab.Equals(this.StoryItemTab)) {
        this.title.text = "Story Item"; 
        this.storyItemTabButton.SetEnabled(false);
        this.normalItemTabButton.SetEnabled(true);
      }
      else {
        this.title.text = "Normal Item";
        this.storyItemTabButton.SetEnabled(true);
        this.normalItemTabButton.SetEnabled(false);
      }
      this.CurrentTab.Value.Hide();
      this.CurrentTab.Value.Content.SetEnabled(false);
      this.CurrentTab.Value = tab;
      this.CurrentTab.Value.Show();
      this.CurrentTab.Value.Content.SetEnabled(true);
    }
  }
}

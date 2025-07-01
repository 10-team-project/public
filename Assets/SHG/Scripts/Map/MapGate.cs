using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using EditorAttributes;
using UnityEngine.SceneManagement;
using KSH;
using LTH;

namespace SHG
{
  [RequireComponent (typeof(InteractUI))]
  public class MapGate : MonoBehaviour, IInteractable
  {
    public string SceneToMove => this.sceneToMove;
    public Action<string> OnMove;
    InteractUI interactUI;
    bool isPresentingUI;
    bool isMoved;
    [SerializeField]
    TextMeshProUGUI gateLabel;
    [SerializeField, FilePath(filters: "unity")] [Validate("Scene must in build settings", nameof(IsInvalidScene), MessageMode.Error, buildKiller: true)]
    string sceneToMove;

    void Awake()
    {
      this.isPresentingUI = false;
      this.interactUI = this.GetComponent<InteractUI>();
    }

    void Start()
    {
      if (this.gateLabel != null) {
        this.gateLabel.text =  $"Move to: {this.sceneToMove}";
      }
    }

    void OnEnable()
    {
      this.isMoved = false;
      this.interactUI.OnInteract += this.OnChangePresting;
    }

    void OnDisable()
    {
      this.isPresentingUI = false;
      this.interactUI.OnInteract -= this.OnChangePresting;
    }

    void OnChangePresting(bool isPresenting)
    {
      this.isPresentingUI = isPresenting;
    }

    public void Interact()
    {
      if (this.isPresentingUI && !this.isMoved) {
          var popup = PopupManager.Instance.ShowPopup<ConfirmPopup>();
          if (popup == null) {
          #if UNITY_EDITOR
            Debug.LogError("Popup ui is not available");
          #endif
            return;
          }
          popup.Show("이동하시겠습니까?","예", 
            this.OnConfirm,
            "아니요",
            this.OnDismiss);
        }
    }

    void OnConfirm()
    {
      this.isMoved = true;
      this.OnMove?.Invoke(this.sceneToMove); 
    }

    void OnDismiss()
    {

    }

    protected bool IsInvalidScene()
    {
      if (this.sceneToMove.Replace(" ", "").Length == 0) {
        return (true);
      }
#if UNITY_EDITOR
      var index = SceneUtility.GetBuildIndexByScenePath(this.sceneToMove);
      if (index == -1) {
        return (true);
      }
#endif
      return (false);
    }
  }
}

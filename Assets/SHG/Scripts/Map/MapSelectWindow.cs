using System;
using UnityEngine;
using UnityEngine.UI;
using EditorAttributes;
using LTH;

namespace SHG
{
  [Serializable]
  public struct MapButton
  {
    public Button Button;
    public GameScene Scene;
  }

  public class MapSelectWindow : MonoBehaviour, IInputLockHandler
  {
    [SerializeField]
    public Button ExitButton;
    [SerializeField]
    public Button NextButton;
    [SerializeField]
    public Button PrevButton;
    [SerializeField] 
      public MapButton[] Buttons;

    public Action OnClickNext;
    public Action OnClickPrev;
    public Action OnClickExit;
    public Action<GameScene> OnClickMapButton;

    public bool IsInputBlocked(InputType inputType)
    {
      return (inputType == InputType.Move);
    }

    public bool OnInputEnd()
    {
      this.gameObject.SetActive(false);
      return (true);
    }

    public bool OnInputStart()
    {
      return (true);
    }

    void OnEnable()
    {
      App.Instance.InputManager.StartInput(this);
    }

    void OnDisable()
    {
      App.Instance.InputManager.EndInput(this);
    }

    void Awake()
    {
      this.ExitButton.onClick.AddListener(() => {
        this.OnClickExit?.Invoke();
        this.gameObject.SetActive(false);
        });
      if (this.NextButton != null) {
        this.NextButton.onClick.AddListener(() => 
          this.OnClickNext?.Invoke());
      }
      if (this.PrevButton != null) {
        this.PrevButton.onClick.AddListener(() => 
          this.OnClickPrev?.Invoke());
      }
      foreach (var mapButton in this.Buttons) {
        mapButton.Button.onClick.AddListener(() => {
          this.gameObject.SetActive(false);
          this.OnClickMapButton?.Invoke(mapButton.Scene);
          });
      }
    }
  }
}

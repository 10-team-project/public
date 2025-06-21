using System;
using UnityEngine;

namespace Patterns
{
  /// <summary>
  /// 값이 변경되는 것을 Action을 통해 감지할 수 있는 class
  /// </summary>
  /// <typeparam name="T">변경 되는 값의 타입, reference 타입은 변경을 감지하기 어려워 struct로 제한</typeparam>
  public class ObservableValue<T> where T: struct
  {
    public T Value {
      get => this.innerValue;
      set {
        if (!this.innerValue.Equals(value)) {
          this.innerValue = value;
          this.OnChanged?.Invoke(this.innerValue);
        }
      }
    }

    public ObservableValue(T initialValue = default)
    {
      this.innerValue = initialValue;
    }

    /// <summary>
    /// 필요하다면 class를 해제할 때를 감지하는 기능
    /// </summary>
    public void DestroySelf()
    {
      if (this.OnDestroy != null) {
        this.OnDestroy.Invoke();
      }
    }

    public Action<T> OnChanged;
    public Action OnDestroy;

    [SerializeField]
    T innerValue;
  }
}

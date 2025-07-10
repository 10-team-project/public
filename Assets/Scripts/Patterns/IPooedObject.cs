using System;

namespace Patterns
{
  /// <summary> 오브젝트 풀을 사용하는 대상 </summary>
  public interface IPooledObject 
  {
    public Action<IPooledObject> OnDisabled { get; set; }
  }
}

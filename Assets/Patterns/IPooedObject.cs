using System;

namespace Patterns
{
  public interface IPooledObject 
  {
    public Action<IPooledObject> OnDisabled { get; set; }
  }
}

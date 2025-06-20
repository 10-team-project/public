using System;

namespace SHG
{
  public interface IUsable
  {
    public Action<IUsable> OnUsed { get; set; }
  }
}

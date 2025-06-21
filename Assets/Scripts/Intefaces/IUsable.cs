using System;

public interface IUsable
{
  public Action<IUsable> OnUsed { get; set; }
}

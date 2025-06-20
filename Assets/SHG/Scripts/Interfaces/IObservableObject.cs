using System;

namespace SHG
{
  public interface IObservableObject<T> where T: class
  {
    /// <summary>
    ///  데이터가 변경 되기 전에 실행되는 이벤트, 변경되기 전의 데이터가 전달됨
    /// </summary>
    public Action<T> WillChange { get; set; }
    /// <summary>
    ///  데이터가 변경 된 이후 실행되는 이벤트,  변경된  데이터가 전달됨
    /// </summary>
    public Action<T> OnChange { get; set; }
  }
}

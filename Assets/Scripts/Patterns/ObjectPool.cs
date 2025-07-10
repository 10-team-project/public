using System;
using UnityPool = UnityEngine.Pool;

namespace Patterns
{
  /// <summary>
  ///  모든 오브젝트 풀의 인터페이스를 정하는 기초 class 
  ///  내부적으로 유니티의 ObjectPool을 사용함
  /// </summary>
  /// <typeparam name="T">오브젝트 풀에 저장되는 타입</typeparam>
  public abstract class ObjectPool<T> where T: class, IPooledObject
  {
    UnityPool.ObjectPool<T> pool;
    const int DEFAULT_MAX_POOL_SIZE = 10000;
    public ObjectPool(int defaultPoolSize, int? maxPoolSize = DEFAULT_MAX_POOL_SIZE, bool collectionCheck = true)
    {
      if (defaultPoolSize <= 0) {
        throw new ArgumentException($"{nameof(defaultPoolSize)} must be greater than 0");
      }
      if (defaultPoolSize > maxPoolSize) {
        throw new ArgumentException($"{nameof(defaultPoolSize)} must be less than ${nameof(maxPoolSize)}");
      }

      this.pool = new UnityPool.ObjectPool<T>(
          this.createPooledObject,
          this.OnTakeFromPool,
          this.OnReturnedToPool,
          this.OnDestroyPoolObject,
          collectionCheck: collectionCheck,
          defaultPoolSize,
          maxPoolSize ?? DEFAULT_MAX_POOL_SIZE
          );
    }

    public T Get() {
      return (this.pool.Get());
    }

    /// <summary>
    /// 오브젝트 풀에 저장되기 전 해당 타입을 생성하는 방법을 정의
    /// </summary>
    abstract protected T CreatePooledObject();

    T createPooledObject() {
      T newObject = this.CreatePooledObject();
      newObject.OnDisabled += (pooledObject) => {
        this.pool.Release((pooledObject as T));
      };
      return (newObject);
    }

    protected virtual void OnReturnedToPool(T obj) {}

    protected virtual void OnTakeFromPool(T obj) {}

    protected virtual void OnDestroyPoolObject(T obj) { }

    protected void ReturnToPool(T obj) 
    {
      this.pool.Release(obj);
    }
  }
}

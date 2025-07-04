using System.Collections;
using System.Collections.Generic;
using Patterns;
using UnityEngine;

public class ResourceManager : SingletonBehaviour<ResourceManager>
{
    private Dictionary<string,ResourceData> saveresourceDic = new Dictionary<string, ResourceData>();
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
    
    public void SaveResource(string resourceName, float cur, float max)
    {
        saveresourceDic[resourceName] = new ResourceData(cur, max);
    }

    public ResourceData LoadResource(string resourceName)
    {
        return saveresourceDic.ContainsKey(resourceName) ? saveresourceDic[resourceName] : null;
    }

    [System.Serializable]
    public class ResourceData
    {
        public float cur;
        public float max;

        public ResourceData(float c, float m)
        {
            cur = c;
            max = m;
        }
    }
    
}

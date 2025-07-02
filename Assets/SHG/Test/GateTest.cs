using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EditorAttributes;
using KSH;

namespace SHG
{
  public class GateTest : MonoBehaviour
  {
    [SerializeField] [Required]
    MapGate mapGate;

    // Start is called before the first frame update
    void Start()
    {
      this.mapGate.OnMove += scene => TestSceneManager.Instance.GameLoadScene(scene);
    }
  }
}

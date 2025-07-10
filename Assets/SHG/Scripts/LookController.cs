using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using EditorAttributes;

namespace SHG
{
  public class LookController : MonoBehaviour
  {
    const float LOOK_DISTANCE = 10f;
    [SerializeField] [Range(1f, 3f)]
    float lookAtSpeed;
    [SerializeField]
    string targetTagname;
    [SerializeField] [Required]
    Transform headAimTarget;
    [SerializeField] [Required]
    MultiAimConstraint headAim;
    [SerializeField] [Required]
    MultiAimConstraint bodyAim;
    Coroutine lookAction;
    bool isFocusing;

    void OnEnable()
    {
      App.Instance.ScriptManager.OnStart += this.OnStartScript;
      App.Instance.ScriptManager.OnEnd += this.OnEndScript;
    }

    void OnDisable()
    {
      App.Instance.ScriptManager.OnStart -= this.OnStartScript;
      App.Instance.ScriptManager.OnEnd -= this.OnEndScript;
    }

    void OnStartScript()
    {
      var npc = GameObject.FindWithTag(this.targetTagname);
      if (npc != null) {
        if (Vector3.Distance(npc.transform.position, this.transform.position) < LOOK_DISTANCE) {

          this.LookAt(new Vector3(
              npc.transform.position.x,
              npc.transform.position.y + 1.2f,
              npc.transform.position.z
              ));
        }
      }
    }

    void OnEndScript()
    {
      if (this.isFocusing) {
        this.LookForward(); 
      } 
    }

    void LookTarget(Transform target)
    {
      if (this.lookAction != null) {
        this.ClearLookAction();
      }
      this.headAimTarget.position = target.position;
      this.lookAction = this.StartCoroutine(this.StartLookAt(target));
    }

    [Button ("Look at")]
    void LookAt(Vector3 position)
    {
      if (this.lookAction != null) {
        this.ClearLookAction();
      }
      this.headAimTarget.position = position;
      this.lookAction = this.StartCoroutine(this.StartLookAt());
    }

    [Button ("Look foward")]
    void LookForward()
    {
      if (this.lookAction != null) {
        this.ClearLookAction();
      }
      this.lookAction = this.StartCoroutine(this.StartLookFoward());
    }

    void ClearLookAction()
    {
      this.StopCoroutine(this.lookAction);
      this.lookAction = null;
    }

    IEnumerator StartLookAt(Transform target = null)
    {
      this.isFocusing = true;
      if (target != null) {
        while (this.headAim.weight <= 0.9f) {
          this.headAim.weight = Mathf.Lerp(
            this.headAim.weight,
            1f,
            this.lookAtSpeed * Time.deltaTime
            );
          this.bodyAim.weight = Mathf.Lerp(
            this.headAim.weight,
            0.7f,
            this.lookAtSpeed * Time.deltaTime
            );
          this.headAimTarget.position = target.position;
          yield return (null);
        }
      }
      else {
        while (this.headAim.weight <= 0.9f) {
          this.headAim.weight =  Mathf.Lerp(
            this.headAim.weight,
            1f,
            this.lookAtSpeed * Time.deltaTime
            );
          this.bodyAim.weight = Mathf.Lerp(
            this.headAim.weight,
            0.7f,
            this.lookAtSpeed * Time.deltaTime
            );
          yield return (null);
        }
      }
      this.headAim.weight = 1f;
      this.bodyAim.weight = 0.7f;
    }

    IEnumerator StartLookFoward()
    {
      this.isFocusing = false;
      float weight = 0f;
      while (this.headAim.weight >= 0.1f) {
        weight = Mathf.Lerp(
          this.headAim.weight,
          0f,
          this.lookAtSpeed * Time.deltaTime
          );
        this.headAim.weight = weight;
        this.bodyAim.weight = weight;
        yield return (null);
      }
      this.headAim.weight = 0f;
      this.bodyAim.weight = 0f;
    }

  }
}

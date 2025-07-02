using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using EditorAttributes;
using SHG;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CameraController : MonoBehaviour
{
  public enum FocusDirection
  {
    Foward,
    Left,
    Right
  }

  [SerializeField, ReadOnly]
  Transform cameraFollow;
  [SerializeField, ReadOnly]
  Transform cameraLook;
  [SerializeField]
  Vector3 followOffset;
  Coroutine cameraRoutine;
  Action<CameraController> onCommandEnded;
  CinemachineVirtualCamera virtualCamera; 
  [SerializeField] [Range (1f, 10f)]
  float horizontalFocusDist;
  [SerializeField] [Range (1f, 10f)]
  float forwardFocusDist;
  [SerializeField] [Range(0.1f, 1f)]
  float cameraMoveSpeed;
  float cameraMoveProgress;
  Queue<(IEnumerator, Action<CameraController>)> cameraCommandQueue;
  #if UNITY_EDITOR
  [SerializeField]
  Transform focusTarget;
  #endif

  Transform player;

  void Awake()
  {
    this.cameraCommandQueue = new ();
    this.cameraFollow = new GameObject("Camera Follow").transform;
    this.cameraLook = new GameObject("Camera Look").transform;
    this.virtualCamera = this.GetComponent<CinemachineVirtualCamera>();
    this.virtualCamera.Follow = this.cameraFollow;
    this.virtualCamera.LookAt = this.cameraLook;
  }
  
  // Start is called before the first frame update
  void Start()
  {
    this.player = GameObject.FindWithTag("Player").transform;
  }

  void LateUpdate()
  {
    if (this.cameraRoutine == null) {
      if (!this.cameraCommandQueue.TryDequeue(out (IEnumerator routine, Action<CameraController> onEnded) command)) {
        this.cameraLook.position = this.player.position;
        this.cameraFollow.position = this.player.position + this.followOffset;
      }
      else {
        this.onCommandEnded = command.onEnded;
        this.cameraRoutine = this.StartCoroutine(command.routine);
      }
    }
  }

  #if UNITY_EDITOR
  [Button("Focus test")]
  void FocusTest(FocusDirection direction)
  {
    if (this.focusTarget == null) {
      Debug.LogError("focusTarget is none");
      return ;
    }
    this.AddFocus(this.focusTarget, direction, 
      onEnded: (camera) => {
      Debug.Log("Focus ended");
      camera.OnCommandEnd(camera);
      });
  }

  [Button("Reset test")]
  void ResetTest()
  {
    this.AddReset((camera) => {
      Debug.Log("Reset ended");
      camera.OnCommandEnd(camera);
      });
  }
  #endif

  public CameraController AddFocus(
    Transform target,
    FocusDirection focusDirection,
    Action<CameraController> onEnded = null,
    Nullable<float> focusDist = null)
  {
    this.cameraCommandQueue.Enqueue(
      (this.MoveCameraRoutine(target, focusDirection), onEnded ?? this.onCommandEnded));
    return (this);
  }

  public CameraController AddReset(Action<CameraController> OnEnded = null)
  {
    this.cameraCommandQueue.Enqueue(
      (this.ResetCameraRoutine(), OnEnded ?? this.OnCommandEnd)); 
    return (this);
  }

  Vector3 CalcFollowPosition(Transform target, FocusDirection focusDirection, Nullable<float> dist = null)
  {
    switch (focusDirection) {
      case FocusDirection.Foward:
        return (target.position + 
          new Vector3(0, 
            this.followOffset.y,
            -(dist ?? this.forwardFocusDist)));
      case FocusDirection.Left:
        return (
          target.position +
          new Vector3(
            -(dist ?? this.horizontalFocusDist),
            this.followOffset.y,
            0));
      case FocusDirection.Right:
        return (
          target.position + 
          new Vector3(
            dist ?? this.horizontalFocusDist,
            this.followOffset.y,
            0));
      default: 
        return this.cameraFollow.position;
    }
  }

  IEnumerator MoveCameraRoutine(Transform lookTarget, FocusDirection focusDirection)
  {
    var followPosition = this.CalcFollowPosition(lookTarget, focusDirection);
    var targetPosition = lookTarget.position;
    while (this.cameraMoveProgress < 1) {
      this.cameraFollow.position = Vector3.Lerp(
        this.cameraFollow.position,
        followPosition,
        this.cameraMoveProgress
        );
      this.cameraLook.position = Vector3.Lerp(
        this.cameraLook.position,
        lookTarget != null ? lookTarget.position : targetPosition,
        this.cameraMoveProgress
        );
      this.cameraMoveProgress += this.cameraMoveSpeed * Time.deltaTime;
      yield return (null);
    }
    this.onCommandEnded?.Invoke(this);
  }

  IEnumerator ResetCameraRoutine()
  {
    Vector3 followDest = this.player.position + this.followOffset;
    while (this.cameraMoveProgress < 1f) {
      this.cameraFollow.position = Vector3.Lerp(
        this.cameraFollow.position,
        followDest,
        this.cameraMoveProgress
        );
      this.cameraLook.position = Vector3.Lerp(
        this.cameraLook.position,
        this.player.position,
        this.cameraMoveProgress
        );
      this.cameraMoveProgress += this.cameraMoveSpeed * Time.deltaTime;
      yield return (null);
    }
    this.onCommandEnded?.Invoke(this);
  }

  public void OnCommandEnd(CameraController camera = null)
  {
    CameraController cam = camera ?? this;
    cam.onCommandEnded = null;
    cam.cameraMoveProgress = 0f;
    cam.cameraRoutine = null;
    cam.cameraMoveProgress = 0f;
  }

  void OnEnable()
  {
    App.Instance.SetCameraController(this);
  }

  void OnDisable()
  {
    App.Instance.SetCameraController(null);
  }
}

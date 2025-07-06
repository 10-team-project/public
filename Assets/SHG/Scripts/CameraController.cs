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

  [SerializeField]
  Transform cameraFollowObject;
  [SerializeField]
  Transform cameraLookObject;
  [SerializeField]
  Vector3 followOffset;
  Rigidbody cameraFollow;
  Rigidbody cameraLook;
  Coroutine cameraRoutine;
  Action<CameraController> onCommandEnded;
  CinemachineVirtualCamera virtualCamera; 
  [SerializeField] [Range (1f, 10f)]
  float horizontalFocusDist;
  [SerializeField] [Range (1f, 10f)]
  float forwardFocusDist;
  [SerializeField] [Range(0.1f, 1f)]
  float cameraFocusSpeed;
  [SerializeField] [Range(5f, 20f)]
  float cameraFollowSpeed;
  float cameraMoveProgress;
  float depthHeightRatio;
  Queue<(IEnumerator, Action<CameraController>)> cameraCommandQueue;
  #if UNITY_EDITOR
  [SerializeField]
  Transform focusTarget;
  #endif

  Transform player;

  void Awake()
  {
    this.cameraCommandQueue = new ();
    this.virtualCamera = this.GetComponent<CinemachineVirtualCamera>();
    this.depthHeightRatio = Math.Abs(this.followOffset.y / this.followOffset.z );
    this.cameraFollow = this.cameraFollowObject.GetComponent<Rigidbody>();
    this.cameraLook = this.cameraLookObject.GetComponent<Rigidbody>();
  }
  
  // Start is called before the first frame update
  void Start()
  {
    this.player = GameObject.FindWithTag("Player").transform;
    this.cameraLookObject.position = this.player.position;
    this.cameraFollow.position = this.player.position + this.followOffset;
  }

  void LateUpdate()
  {
    if (this.cameraRoutine == null) {
      if (!this.cameraCommandQueue.TryDequeue(out (IEnumerator routine, Action<CameraController> onEnded) command)) {
        this.cameraLook.velocity = (this.player.position - this.cameraLook.position) * this.cameraFollowSpeed;
        this.cameraFollow.velocity = 
         (this.player.position + this.followOffset - this.cameraFollow.position ) * this.cameraFollowSpeed;
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
    if (onEnded != null) {

      this.cameraCommandQueue.Enqueue(
        (this.MoveCameraRoutine(target, focusDirection, focusDist), 
         onEnded));
    }
    else {
      this.cameraCommandQueue.Enqueue(
        (this.MoveCameraRoutine(target, focusDirection, focusDist), 
         this.OnCommandEnd));
    }
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
    if (dist == null) {
      dist = focusDirection == FocusDirection.Foward ? this.forwardFocusDist: this.horizontalFocusDist;
    }
    float depth = dist.Value;
    switch (focusDirection) {
      case FocusDirection.Foward:
        return (target.position + 
          new Vector3(
            0, 
            Math.Abs(depth * this.depthHeightRatio),
            -depth));
      case FocusDirection.Left:
        return (
          target.position +
          new Vector3(
            -(depth),
            Math.Abs(depth * this.depthHeightRatio),
            -depth));
      case FocusDirection.Right:
        return (
          target.position + 
          new Vector3(
            depth,
            Math.Abs(depth * this.depthHeightRatio),
            -depth));
      default: 
        return this.cameraFollow.position;
    }
  }

  IEnumerator MoveCameraRoutine(Transform lookTarget, FocusDirection focusDirection, Nullable<float> focusDist = null)
  {
    var followPosition = this.CalcFollowPosition(lookTarget, focusDirection, focusDist);
    var targetPosition = lookTarget.position;
    while (this.cameraMoveProgress < 1) {
      this.cameraFollowObject.position = Vector3.Lerp(
        this.cameraFollowObject.position,
        followPosition,
        this.cameraMoveProgress
        );
      this.cameraLookObject.position = Vector3.Lerp(
        this.cameraLookObject.position,
        lookTarget != null ? lookTarget.position : targetPosition,
        this.cameraMoveProgress
        );
      this.cameraMoveProgress += this.cameraFocusSpeed * Time.deltaTime;
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
      this.cameraLookObject.position = Vector3.Lerp(
        this.cameraLookObject.position,
        this.player.position,
        this.cameraMoveProgress
        );
      this.cameraMoveProgress += this.cameraFocusSpeed * Time.deltaTime;
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
    if (App.Instance != null) {
      App.Instance.SetCameraController(null);
    }
  }
}

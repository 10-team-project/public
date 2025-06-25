using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTH;
using UnityEditor.Experimental.GraphView;

namespace LTH
{
    public class PlayerMovement : MonoBehaviour, IInputLockHandler
    {
        [SerializeField] float moveSpeed;
        [SerializeField] float runSpeed;
        [SerializeField] float climbSpeed;

        private Rigidbody _rigid;
        private Vector3 moveInput;

        private bool isRunning;
        private bool isOnLadder = false;

        private Vector3 ladderPosition;
        private Vector3 ladderDirection;

        private bool isLadderEnterZone = false;
        private bool isLadderExitZone = false;
        private bool isAtLadderTop = false;

        private LadderClimber ladderClimber;

        [HideInInspector] public bool IsOnLadder => isOnLadder;

        private bool IsGrounded()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            return Physics.Raycast(ray, 1.03f, LayerMask.GetMask("Ground"));
        }

        private void Start()
        {
            _rigid = GetComponent<Rigidbody>();
            _rigid.freezeRotation = true;

            ladderClimber = new LadderClimber(_rigid, climbSpeed, EndClimb);
        }

        private void Update()
        {
            if (!isOnLadder && InputManager.Instance.IsBlocked(InputType.Move)) return;

            PlayerInput();

            if (isLadderEnterZone && Input.GetKeyDown(KeyCode.F))
            {
                StartClimbing(ladderPosition, ladderDirection);
            }
            else if (isOnLadder && Input.GetKeyDown(KeyCode.F) && isLadderExitZone)
            {
                EndClimb();
            }
        }

        private void FixedUpdate()
        {
            if (isOnLadder)
            {
                ladderClimber.Climb(moveInput, IsGrounded(), isAtLadderTop);
            }
            else
            {
                Move();
            }
        }

        private void PlayerInput()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(moveX) > 0 && Mathf.Abs(moveY) > 0)
            {
                moveInput = Vector3.zero;
            }
            else
            {
                moveInput = new Vector3(moveX, moveY, 0f);
            }

            isRunning = Input.GetKey(KeyCode.LeftShift);
        }

        private void Move()
        {

            float speed;

            if (isRunning)
            {
                speed = runSpeed;
            }
            else
            {
                speed = moveSpeed;
            }

            Vector3 move = Vector3.right * moveInput.x * speed;

            Ray ray = new Ray(transform.position, move.normalized);
            float rayDistance = 0.6f;

            bool isBlocked = Physics.Raycast(ray, rayDistance, LayerMask.GetMask("Blocker"));

            if (!isBlocked)
            {
                _rigid.MovePosition(_rigid.position + move * Time.fixedDeltaTime);
                PlayerCam(moveInput.x);
            }
        }

        private void PlayerCam(float dir)
        {
            if (isOnLadder) return;

            Vector3 lookDir = Vector3.zero;

            if (dir > 0)
            {
                lookDir = Vector3.right;
            }
            else if (dir < 0)
            {
                lookDir = Vector3.left;
            }

            if (lookDir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, 0.15f);
            }
        }

        public void StartClimbing(Vector3 pos, Vector3 forward)
        {
            InputManager.Instance.StartInput(this);
            isOnLadder = true;
            _rigid.useGravity = false;
            _rigid.velocity = Vector3.zero;

            Vector3 targetPos = new Vector3(pos.x, transform.position.y, pos.z) - forward * 0.3f;
            targetPos.y += 0.1f;
            transform.position = targetPos;

            transform.rotation = Quaternion.LookRotation(forward);
        }

        public void EndClimb()
        {
            isOnLadder = false;
            _rigid.useGravity = true;
            InputManager.Instance.EndInput(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("LadderDownStart") || other.CompareTag("LadderUpStart"))
            {
                ladderPosition = other.transform.position;
                ladderDirection = other.transform.forward;
                isLadderEnterZone = true;
            }
            else if (other.CompareTag("LadderDownEnd"))
            {
                isLadderExitZone = true;
            }
            else if (other.CompareTag("LadderUpEnd"))
            {
                isAtLadderTop = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ladder"))
            {
                InputManager.Instance.EndInput(this);
                isOnLadder = false;
                _rigid.useGravity = true;
            }
            else if (other.CompareTag("LadderUpEnd"))
            {
                isAtLadderTop = false;
            }
        }

        public bool IsInputBlocked(InputType inputType)
        {
            return inputType == InputType.Move;
        }

        public bool OnInputStart()
        {
            return true;
        }

        public bool OnInputEnd()
        {
            return true;
        }
    }
}


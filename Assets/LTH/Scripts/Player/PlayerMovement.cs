using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LTH;

namespace LTH 
{
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] float moveSpeed;
        [SerializeField] float runSpeed;
        [SerializeField] float climbSpeed;

        private Rigidbody _rigid;
        private Vector3 moveInput;

        private bool isRunning;
        private bool isOnLadder = false;

        private bool IsGrounded()
        {
            Ray ray = new Ray(transform.position, Vector3.down);
            return Physics.Raycast(ray, 0.5f, LayerMask.GetMask("Ground"));
        }

        private void Start()
        {
            _rigid = GetComponent<Rigidbody>();
            _rigid.freezeRotation = true;
        }

        private void Update()
        {
            PlayerInput();
        }

        private void FixedUpdate()
        {
            if (isOnLadder)
            {
                ClimbLadder();
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

            moveInput = new Vector3(moveX, moveY, 0f);
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
            _rigid.MovePosition(_rigid.position + move * Time.fixedDeltaTime);
            PlayerCam(moveInput.x);
        }

        private void PlayerCam(float dir)
        {
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

        private void ClimbLadder()
        {
            Vector3 climb = Vector3.up * moveInput.y * climbSpeed;
            _rigid.velocity = climb;

            if (IsGrounded())
            {
                isOnLadder = false;
                _rigid.useGravity = true;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ladder"))
            {
                isOnLadder = true;
                _rigid.useGravity = false;
                _rigid.velocity = Vector3.zero;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Ladder"))
            {
                isOnLadder = false;
                _rigid.useGravity = true;
            }
        }
    }
}


using UnityEngine;
using LTH;

public class PlayerMovement : MonoBehaviour, IInputLockHandler
{

    [Header("Movement Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float runSpeed;
    [SerializeField] float climbSpeed;

    private Rigidbody _rigid;

    private Vector3 moveInput;
    private bool isRunning;

    private bool isOnLadder = false;
    private bool isAtLadderTop = false;

    private LadderClimber ladderClimber;

    private Animator animator; // 애니메이션(테스트용 삭제 예정)

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

        animator = GetComponent<Animator>(); // 애니메이션(테스트용 삭제 예정)
        ladderClimber = new LadderClimber(_rigid, climbSpeed, EndClimb, EndClimbFromTop);
    }

    private void Update()
    {
        if (!isOnLadder && InputManager.Instance.IsBlocked(InputType.Move)) return;

        PlayerInput();
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

        UpdateAnimation();  // 애니메이션(테스트용 삭제 예정)
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

    public void StartClimbing(Vector3 pos, Vector3 forward, LadderTriggerType type)
    {
        InputManager.Instance.StartInput(this);
        isOnLadder = true;

        _rigid.useGravity = false;
        _rigid.velocity = Vector3.zero;

        Vector3 targetPos = new Vector3(pos.x, transform.position.y, pos.z);
        targetPos.y += 0.1f;
        transform.position = targetPos;

        transform.rotation = Quaternion.LookRotation(Vector3.right);

        // 애니메이션(테스트용 삭제 예정)
        animator.SetBool("Ladder", true);

        if (type == LadderTriggerType.EnterBottom)
            animator.SetTrigger("LadderUpStart");
        else if (type == LadderTriggerType.EnterTop)
            animator.SetTrigger("LadderDownStart");
    }

    public void EndClimbFromTop()
    {
        // 애니메이션(테스트용 삭제 예정)
        animator.SetTrigger("LadderUpEnd");
        animator.SetBool("Ladder", false);
        ExitLadder(transform.forward * 0.7f + Vector3.up * 0.7f);
    }

    public void EndClimb()
    {
        // 애니메이션(테스트용 삭제 예정)
        animator.SetTrigger("LadderDownEnd");
        animator.SetBool("Ladder", false);

        ExitLadder(transform.forward * 0.5f);
    }

    private void ExitLadder(Vector3 offset)
    {
        isOnLadder = false;
        _rigid.useGravity = true;

        InputManager.Instance.EndInput(this);

        Vector3 targetPos = transform.position + offset;

        RaycastHit hit;
        if (Physics.Raycast(targetPos, Vector3.down, out hit, 2f, LayerMask.GetMask("Ground")))
        {
            targetPos.y = hit.point.y + 0.01f;
        }

        _rigid.MovePosition(targetPos);
    }

    public void SetAtLadderTop(bool value)
    {
        isAtLadderTop = value;
    }

    private void UpdateAnimation()  // 애니메이션(테스트용 삭제 예정)
    {
        if (animator == null) return;

        animator.SetBool("Ladder", isOnLadder);

        if (isOnLadder)
        {
            float vertical = moveInput.y;

            if (vertical > 0.01f)
            {
                animator.SetBool("LadderUpPlay", true);
                animator.SetBool("LadderDownPlay", false);
            }
            else if (vertical < -0.01f)
            {
                animator.SetBool("LadderUpPlay", false);
                animator.SetBool("LadderDownPlay", true);
            }
            else
            {
                animator.SetBool("LadderUpPlay", false);
                animator.SetBool("LadderDownPlay", false);
            }
            return;
        }


        if (InputManager.Instance.IsBlocked(InputType.Move))
        {
            animator.SetBool("IsMoving", false);
            animator.SetFloat("Speed", 0f);
            return;
        }

        bool isMoving = Mathf.Abs(moveInput.x) > 0.01f;
        animator.SetBool("IsMoving", isMoving);
        animator.SetFloat("Speed", isMoving ? (isRunning ? 1f : 0.5f) : 0f);
    }


    public bool IsInputBlocked(InputType inputType)
    {
        return inputType == InputType.Move || inputType == InputType.Interaction;
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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderClimber
{
    private Rigidbody _rigid;
    private float _climbSpeed;
    private Action _onClimbEnd;
    private Action onClimbEndFromTop;

    public LadderClimber(Rigidbody rigid, float climbSpeed, Action onClimbEnd, Action onClimbEndFromTop)
    {
        _rigid = rigid;
        _climbSpeed = climbSpeed;
        _onClimbEnd = onClimbEnd;
        this.onClimbEndFromTop = onClimbEndFromTop;
    }

    public void Climb(Vector3 moveInput, bool isGrounded, bool isAtTop)
    {
        Vector3 targetVelocity = Vector3.up * moveInput.y * _climbSpeed;
        _rigid.velocity = Vector3.Lerp(_rigid.velocity, targetVelocity, 0.1f);

        if (isAtTop && moveInput.y > 0)
        {
            onClimbEndFromTop?.Invoke();
        }

        else if (isGrounded && moveInput.y < 0)
        {
            _onClimbEnd?.Invoke();
        }
    }

    public void StopClimb()
    {
        _rigid.velocity = Vector3.zero;
        _onClimbEnd?.Invoke();
    }
}

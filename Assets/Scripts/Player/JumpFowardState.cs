using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFowardState : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private float _moveSpeed;
    private Rigidbody2D _rb;
    private float _jumpForce = 10f;
    private GroundCheck _groundCheck;
    private FlipCharacter _flip;

    public JumpFowardState(StateMachine<PlayerStateMachine.PlayerState> stateMachine, Animator animator, float moveSpeed, Rigidbody2D rb, float jumpForce, GroundCheck groundCheck, FlipCharacter flip)
        : base(stateMachine, PlayerStateMachine.PlayerState.JumpForward)
    {
        _animator = animator;
        _rb = rb;
        _jumpForce = jumpForce;
        _groundCheck = groundCheck;
        _flip = flip;
        _moveSpeed = moveSpeed;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
    }


    public override PlayerStateMachine.PlayerState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private float _moveSpeed;
    private Rigidbody2D _rb;
    private float _jumpForce = 10f;
    private GroundCheck _groundCheck;
    private bool canJump=false;

    public JumpState(StateMachine<PlayerStateMachine.PlayerState> stateMachine, Animator animator, float moveSpeed, Rigidbody2D rb, float jumpForce, GroundCheck groundCheck)
        : base(stateMachine, PlayerStateMachine.PlayerState.Jump)
    {
        _animator = animator;
        _rb = rb;
        _jumpForce = jumpForce;
        _groundCheck = groundCheck;
        _moveSpeed = moveSpeed;
    }

    public override void EnterState()
    {
        canJump= true;
        _animator.SetTrigger("Jump");
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);

    }

    public override void ExitState()
    {
        Debug.Log("Exiting Jump State");
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.JumpAttack);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Magic);
        }
        if (Input.GetKeyDown(KeyCode.X)&& canJump)
        {
            canJump = false;
            _animator.SetTrigger("Jump");
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
        }

    }


    public override PlayerStateMachine.PlayerState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallState : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private float _moveSpeed;
    private Rigidbody2D _rb;
    private GroundCheck _groundCheck;
    private FlipCharacter _flip;
    

    public FallState(StateMachine<PlayerStateMachine.PlayerState> stateMachine, Animator animator, float moveSpeed, Rigidbody2D rb, GroundCheck groundCheck, FlipCharacter flip)
        : base(stateMachine, PlayerStateMachine.PlayerState.Fall)
    {
        _animator = animator;
        _rb = rb;
        _groundCheck = groundCheck;
        _flip = flip;
        _moveSpeed= moveSpeed;
    }

    public override void EnterState()
    {
        _animator.SetBool("isFalling",true);
    }

    public override void ExitState()
    {
        _animator.SetBool("isFalling", false);
    }
    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Magic);
        }
        if (_rb.velocity.y <= 0)
        {
            _animator.SetTrigger("Grounded");
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Idle);
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

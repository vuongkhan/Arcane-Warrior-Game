using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private float _moveSpeed;
    private GroundCheck _groundCheck;
    private HealthManager _health;

    public IdleState(StateMachine<PlayerStateMachine.PlayerState> stateMachine,Animator animator, float moveSpeed,GroundCheck groundCheck, HealthManager health)
        : base(stateMachine, PlayerStateMachine.PlayerState.Idle)
    {
        _animator = animator;
        _moveSpeed= moveSpeed;
        _groundCheck = groundCheck;
        _health = health;
    }

    public override void EnterState()
    {
        _groundCheck.canMove = true;
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput !=0)
        {
            Debug.Log("Entering State");
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Run);
        }
        if (Input.GetKeyDown(KeyCode.X) && _groundCheck.IsGrounded())
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Jump);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Attack1);
        }
        if (Input.GetKeyDown(KeyCode.F) && _health.GetCurrentMana() >=10)
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Shoot);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Magic);
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


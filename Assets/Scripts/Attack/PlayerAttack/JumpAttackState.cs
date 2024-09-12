using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackState : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private float _moveSpeed;

    public JumpAttackState(StateMachine<PlayerStateMachine.PlayerState> stateMachine, Animator animator, float moveSpeed )
        : base(stateMachine, PlayerStateMachine.PlayerState.JumpAttack)
    {
        _animator = animator;
        _moveSpeed = moveSpeed;
    }

    public override void EnterState()
    {
        _animator.SetTrigger("attackjump");
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


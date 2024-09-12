using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAttackState : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private float _moveSpeed;
    private Rigidbody2D _rb;

    public WalkAttackState(StateMachine<PlayerStateMachine.PlayerState> stateMachine, Animator animator, Rigidbody2D rb)
        : base(stateMachine, PlayerStateMachine.PlayerState.WalkAttack)
    {
        _animator = animator;
        _rb = rb;
    }

    public override void EnterState()
    {
        _animator.SetTrigger("WalkAttack");
        StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Idle);
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

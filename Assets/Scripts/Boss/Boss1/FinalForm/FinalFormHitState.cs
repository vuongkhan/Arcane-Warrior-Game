using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFormHitState : BaseState<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;


    public FinalFormHitState(StateMachine<FinalFormStateMachine.FinalFormState> stateMachine, Animator animator, Rigidbody2D rb)
        : base(stateMachine, FinalFormStateMachine.FinalFormState.Hit)
    {
        _animator = animator;
        _rb = rb;
    }

    public override void EnterState()
    {
        _animator.SetTrigger("hit");
        StateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.IdleFinal);
    }

    public override void ExitState()
    {}

    public override void UpdateState()
    {}

    public override FinalFormStateMachine.FinalFormState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}

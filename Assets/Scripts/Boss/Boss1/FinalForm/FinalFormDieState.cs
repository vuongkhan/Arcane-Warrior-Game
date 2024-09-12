using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFormDieState : BaseState<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private EnemyAudioManager _audioManager;

    public FinalFormDieState(StateMachine<FinalFormStateMachine.FinalFormState> stateMachine, Animator animator, Rigidbody2D rb, EnemyAudioManager audioManager)
        : base(stateMachine, FinalFormStateMachine.FinalFormState.Die)
    {
        _animator = animator;
        _rb = rb;
        _audioManager = audioManager;
    }

    public override void EnterState()
    {
        _audioManager.PlaySoundEffect("die");
        _animator.SetTrigger("die");
        Object.Destroy(StateMachine.gameObject, 2f);
    }

    public override void ExitState()
    {


    }

    public override void UpdateState()
    {

    }

    public override FinalFormStateMachine.FinalFormState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}

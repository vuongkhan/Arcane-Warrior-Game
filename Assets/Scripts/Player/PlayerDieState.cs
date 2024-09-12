using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDieState : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private FlipCharacter _flip;
    private GroundCheck _groundCheck;
    private AudioManager _audioManager;


    public PlayerDieState(StateMachine<PlayerStateMachine.PlayerState> stateMachine, Animator animator, Rigidbody2D rb, GroundCheck groundCheck, AudioManager audioManager)
        : base(stateMachine, PlayerStateMachine.PlayerState.Die)
    {
        _animator = animator;
        _rb = rb;
        _groundCheck = groundCheck;
        _audioManager = audioManager;
    }

    public override void EnterState()
    {
        _animator.SetTrigger("die");
        _audioManager.PlaySoundEffect("death");
        _groundCheck.canMove = false;
        Object.Destroy(StateMachine.gameObject, 2f);
 
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

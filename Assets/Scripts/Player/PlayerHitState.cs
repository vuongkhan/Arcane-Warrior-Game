using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private GroundCheck _groundCheck;
    private float _enterTime;
    private GameObject[] _dust;
    private AudioManager _audioManager;

    public PlayerHitState(StateMachine<PlayerStateMachine.PlayerState> stateMachine, Animator animator, Rigidbody2D rb, GroundCheck groundCheck, GameObject[] dust, AudioManager audioManager)
        : base(stateMachine, PlayerStateMachine.PlayerState.Hit)
    {
        _animator = animator;
        _rb = rb;
        _groundCheck = groundCheck;
        _dust = dust;
        _audioManager= audioManager;
    }

    public override void EnterState()
    {
        _groundCheck.canMove = false;  
        _animator.SetTrigger("hurt");  
        _enterTime = Time.time;
        GameObject dust = Object.Instantiate(_dust[5], _rb.position, Quaternion.identity);
        _audioManager.PlaySoundEffect("grunt");
    }

    public override void ExitState()
    {
        _groundCheck.canMove = true;
    }

    public override void UpdateState()
    {
        if (Time.time - _enterTime >= 0.5f)
        {
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

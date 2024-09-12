using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private float _moveSpeed;
    private Rigidbody2D _rb;
    private FlipCharacter _flip;
    private GroundCheck _groundCheck;
    private AudioManager _audioManager;

    public RunState(StateMachine<PlayerStateMachine.PlayerState> stateMachine, Animator animator, float moveSpeed, Rigidbody2D rb, FlipCharacter flip, GroundCheck groundCheck, AudioManager audioManager)
        : base(stateMachine, PlayerStateMachine.PlayerState.Run)
    {
        _animator = animator;
        _moveSpeed = moveSpeed;
        _rb = rb;
        _flip = flip;
        _groundCheck = groundCheck;
        _audioManager = audioManager;
    }

    public override void EnterState()
    {
        _audioManager.PlayOneShotSoundEffect("walking");
    }

    public override void ExitState()
    {
        _audioManager.StopOneShotSoundEffect();
    }

    public override void UpdateState()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        _animator.SetFloat("Speed", Mathf.Abs(horizontalInput * _moveSpeed));


        if (horizontalInput == 0)
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Idle);
        }

        if (Input.GetKeyDown(KeyCode.X) && _groundCheck.IsGrounded())
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Jump);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Attack1);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Sprint);
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

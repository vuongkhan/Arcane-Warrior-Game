using System.Collections.Generic;
using UnityEngine;

public class ReadyState : BaseState<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private float _timeStart;
    private WallCheck _wallCheck;
    private bool _canBack;
    private EnemyAudioManager _audioManager;
    private const float fightDuration = 7f; 
    private bool _isFighting;
    private float _fightStartTime;

    public ReadyState(StateMachine<FinalFormStateMachine.FinalFormState> stateMachine, Animator animator, Rigidbody2D rb, WallCheck wallCheck, EnemyAudioManager audioManager)
        : base(stateMachine, FinalFormStateMachine.FinalFormState.Ready)
    {
        _animator = animator;
        _rb = rb;
        _wallCheck = wallCheck;
        _audioManager = audioManager;
    }

    public override void EnterState()
    {
        _timeStart = Time.time;
        _animator.SetBool("back", false);
        _canBack = true;
        _isFighting = false;
    }

    public override void ExitState()
    {
        _animator.SetBool("back", false);
        _rb.velocity = Vector2.zero;

    }
    public override void UpdateState()
    {
        if (_wallCheck.distanceToWall > 1 && _canBack && !_isFighting)
        {
            _animator.SetBool("back", true);
            _animator.SetFloat("Speed", 2f);
            Vector2 wallPosition = new Vector2(_wallCheck.wallObject.position.x, _rb.position.y);
            Vector2 direction = (wallPosition - _rb.position).normalized;
            _rb.velocity = direction * 10f;
        }
        else if (_wallCheck.distanceToWall <= 2 && _canBack)
        {
            _animator.SetBool("back", false);
            _animator.SetTrigger("fight");
            _audioManager.PlaySoundEffect("Serious");
            _fightStartTime = Time.time;
            _canBack = false;
        }
        else if (Time.time >= _fightStartTime + fightDuration)
        {
            StateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.IdleFinal);
            Debug.Log("Transitioning to IdleFinal");
        }
    }


    public override FinalFormStateMachine.FinalFormState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other){}
    public override void OnTriggerStay2D(Collider2D other){}
    public override void OnTriggerExit2D(Collider2D other){}
}

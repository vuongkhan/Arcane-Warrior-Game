using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : BaseState<EnemyStateMachine.EnemyState>
{
    private Animator _animator;
    private float _moveSpeed;
    private Rigidbody2D _rb;
    private float _hp;
    private EnemyNormalAudioManager _enemyAudio;
    private bool _soundPlayed = false;  
    private bool _isAttackComplete = false; 
    private float _attackDuration = 1.5f; 
    private float _attackTimer = 0f; 
    public HitState(StateMachine<EnemyStateMachine.EnemyState> stateMachine, Animator animator, Rigidbody2D rb,float Hp, EnemyNormalAudioManager enemyAudio)
        : base(stateMachine, EnemyStateMachine.EnemyState.Hit)
    {
        _animator = animator;
        _rb = rb;
        _hp = Hp;
        _enemyAudio = enemyAudio;
    }

    public override void EnterState()
    {
        _animator.SetTrigger("Hurt");
        if (!_soundPlayed)
        {
            _enemyAudio.PlaySoundEffect("monstergrunt");
            _soundPlayed = true;  
        }

        _isAttackComplete = false;
        _attackTimer = 0f;
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (!_isAttackComplete)
        {
            _attackTimer += Time.deltaTime;  

            if (_attackTimer >= _attackDuration)  
            {
                _isAttackComplete = true;  
                StateMachine.QueueNextState(EnemyStateMachine.EnemyState.Chase); 
            }
        }
    }

    public override EnemyStateMachine.EnemyState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}

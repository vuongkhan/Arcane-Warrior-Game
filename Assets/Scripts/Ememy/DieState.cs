using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieState : BaseState<EnemyStateMachine.EnemyState>
{
    private Animator _animator;
    private float _moveSpeed;
    private Rigidbody2D _rb;
    private float _hp;
    private EnemyNormalAudioManager _enemyAudio;
    private bool _soundPlayed = false;

    public DieState(StateMachine<EnemyStateMachine.EnemyState> stateMachine, Animator animator, Rigidbody2D rb, float Hp, EnemyNormalAudioManager enemyAudio)
        : base(stateMachine, EnemyStateMachine.EnemyState.Die)
    {
        _animator = animator;
        _rb = rb;
        _hp = Hp;
        _enemyAudio= enemyAudio;
    }

    public override void EnterState()
    {
        Debug.Log("vào");
        _animator.SetTrigger("Die");
        Object.Destroy(StateMachine.gameObject, 2f);
        if (!_soundPlayed)
        {
            _enemyAudio.PlaySoundEffect("monsterdie");
            _soundPlayed = true;  
        }
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
    }



    public override EnemyStateMachine.EnemyState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}

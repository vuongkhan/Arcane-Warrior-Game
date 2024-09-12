using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemyDieState : BaseState<RangeEnemyStateMachine.RangeEnemy>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private bool _soundPlayed = false;
    private EnemyNormalAudioManager _enemyAudio; 
    public RangeEnemyDieState(StateMachine<RangeEnemyStateMachine.RangeEnemy> stateMachine, Animator animator, Rigidbody2D rb, EnemyNormalAudioManager enemyAudio)
        : base(stateMachine, RangeEnemyStateMachine.RangeEnemy.Die)
    {
        _animator = animator;
        _rb = rb;
        _enemyAudio = enemyAudio;
    }

    public override void EnterState()
    {

        _animator.SetTrigger("death");
        if (!_soundPlayed)
        {
            _enemyAudio.PlaySoundEffect("dragon-hurt-47161");
            _soundPlayed = true;
        }
        Object.Destroy(StateMachine.gameObject, 2f);
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {

    }


    public override RangeEnemyStateMachine.RangeEnemy GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}

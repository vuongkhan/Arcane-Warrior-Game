using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseState : BaseState<EnemyStateMachine.EnemyState>
{
    private Animator _animator;
    private float _moveSpeed;
    private Rigidbody2D _rb;
    private CheckPlayer _checkPlayer;
    private FlipCharacter _flip;

    public ChaseState(StateMachine<EnemyStateMachine.EnemyState> stateMachine, Animator animator, Rigidbody2D rb, float moveSpeed, CheckPlayer checkPlayer, FlipCharacter flip)
        : base(stateMachine, EnemyStateMachine.EnemyState.Chase)
    {
        _animator = animator;
        _rb = rb;
        _moveSpeed = moveSpeed;
        _checkPlayer = checkPlayer;
        _flip = flip;
    }

    public override void EnterState()
    {
        _moveSpeed = 3f;
    }

    public override void ExitState()
    {
        _moveSpeed = 1f;
    }

    public override void UpdateState()
    {
        if (_checkPlayer.IsPlayer())
        {
            Vector2 playerPosition = _checkPlayer.GetPlayerPosition();

            Vector2 direction = (_checkPlayer.GetPlayerPosition() - (Vector2)_rb.transform.position).normalized;
            _rb.velocity = new Vector2(direction.x * _moveSpeed, _rb.velocity.y);

            if ((direction.x > 0 && !_flip.IsFacingRight()) || (direction.x < 0 && _flip.IsFacingRight()))
            {
                _flip.Flip();
            }
            if (Vector2.Distance(_rb.transform.position, playerPosition) <= 1f)
            {
                StateMachine.QueueNextState(EnemyStateMachine.EnemyState.Attack);
            }
        }
        else
        {
            StateMachine.QueueNextState(EnemyStateMachine.EnemyState.Patrol);
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

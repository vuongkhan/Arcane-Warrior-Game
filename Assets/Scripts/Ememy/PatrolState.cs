using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState<EnemyStateMachine.EnemyState>
{
    private Animator _animator;
    private float _moveSpeed;
    private Rigidbody2D _rb;
    private Transform[] _patrolPoint;
    private int _currentPoint=0;
    private bool isWaiting = false;
    private float waitTime = 1;
    private CheckPlayer _checkPlayer;
    private FlipCharacter _flip;

    public PatrolState(StateMachine<EnemyStateMachine.EnemyState> stateMachine, Animator animator, Rigidbody2D rb, Transform[] patrolPoint, float moveSpeed, CheckPlayer checkPlayer, FlipCharacter flip)
        : base(stateMachine, EnemyStateMachine.EnemyState.Patrol)
    {
        _animator = animator;
        _rb = rb;
        _patrolPoint= patrolPoint;
        _moveSpeed= moveSpeed;
        _checkPlayer = checkPlayer;
        _flip = flip;
    }

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        if (!isWaiting)
        {
            Patrol();
        }
        else
        {
            Wait();
        }
        if (_checkPlayer.IsPlayer())
        {
            StateMachine.QueueNextState(EnemyStateMachine.EnemyState.Chase);
        }
        if (_rb.velocity.x > 0 && !_flip.IsFacingRight())
        {
            _flip.Flip();
        }
        else if (_rb.velocity.x < 0 && _flip.IsFacingRight())
        {
            _flip.Flip();
        }
    }
    void Wait()
    {
        Debug.Log("Wait");
        _animator.SetBool("isMoving", false);
        waitTime -= Time.deltaTime;
        if (waitTime <= 0)
        {
            isWaiting = false;
            waitTime = 1f;
        }
    }
    void Patrol()
    {
        _animator.SetBool("isMoving", true);
        if (_patrolPoint.Length == 0) return;
        Transform targetPoint = _patrolPoint[_currentPoint];
        Vector2 direction = (targetPoint.position - _rb.transform.position).normalized;
        _rb.velocity = direction * _moveSpeed;

        float distance = Vector2.Distance(_rb.transform.position, targetPoint.position);
        if (distance < 1.1)
        {
        isWaiting = true;
        _currentPoint = (_currentPoint + 1) % _patrolPoint.Length;
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

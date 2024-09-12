using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1IdleState : BaseState<Boss1StateMachine.Boss1State>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private List<Boss1StateMachine.Boss1State> _attackStates;
    private float _attackChangeInterval = 1f; 
    private float _timeSinceLastAttackChange;

    public Boss1IdleState(StateMachine<Boss1StateMachine.Boss1State> stateMachine, Animator animator, Rigidbody2D rb)
        : base(stateMachine, Boss1StateMachine.Boss1State.Idle)
    {
        _animator = animator;
        _rb = rb;
        _attackStates = new List<Boss1StateMachine.Boss1State>
        {
            Boss1StateMachine.Boss1State.Attack1,
            Boss1StateMachine.Boss1State.Attack2,
            Boss1StateMachine.Boss1State.Attack3,
            Boss1StateMachine.Boss1State.Attack5,
        };
    }

    public override void EnterState()
    {
        _timeSinceLastAttackChange = 0f;
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {
        _timeSinceLastAttackChange += Time.deltaTime;

        if (_timeSinceLastAttackChange >= _attackChangeInterval)
        {
            Boss1StateMachine.Boss1State randomAttackState = _attackStates[Random.Range(0, _attackStates.Count)];
            StateMachine.QueueNextState(randomAttackState);
            _timeSinceLastAttackChange = 0f;
        }
    }

    public override Boss1StateMachine.Boss1State GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}

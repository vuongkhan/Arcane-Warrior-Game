using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1HitState : BaseState<Boss1StateMachine.Boss1State>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private string[] triggers;


    public Boss1HitState(StateMachine<Boss1StateMachine.Boss1State> stateMachine, Animator animator, Rigidbody2D rb)
        : base(stateMachine, Boss1StateMachine.Boss1State.Hit)
    {
        _animator = animator;
        _rb = rb;
        triggers = new string[] { "hurt1", "hurt2", "hurt3" };
    }

    public override void EnterState()
    {
        if (_animator == null || triggers.Length == 0)
        {
            return;
        }

        SetRandomTrigger();
        StateMachine.QueueNextState(Boss1StateMachine.Boss1State.Idle);
    }

    public override void ExitState()
    {
    }

    public override void UpdateState()
    {

    }
    void SetRandomTrigger()
    {
        string randomTrigger = triggers[Random.Range(0, triggers.Length)];
        _animator.SetTrigger(randomTrigger);
    }
    public override Boss1StateMachine.Boss1State GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}

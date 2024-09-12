using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1ChargeState : BaseState<Boss1StateMachine.Boss1State>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private float timeCharge = 2f;
    private float enterTime;

    public Boss1ChargeState(StateMachine<Boss1StateMachine.Boss1State> stateMachine, Animator animator, Rigidbody2D rb)
        : base(stateMachine, Boss1StateMachine.Boss1State.Charge)
    {
        _animator = animator;
        _rb = rb;
    }

    public override void EnterState()
    {
        _animator.SetTrigger("Charge");
        enterTime = Time.time; 
    }

    public override void ExitState()
    {
       
    }

    public override void UpdateState()
    {
        float elapsedTime = Time.time - enterTime; 

        if (elapsedTime >= timeCharge)
        {
            StateMachine.QueueNextState(Boss1StateMachine.Boss1State.Attack1);
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

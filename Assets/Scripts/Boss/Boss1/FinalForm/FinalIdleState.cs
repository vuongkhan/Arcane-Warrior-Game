using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalIdleState : BaseState<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private List<FinalFormStateMachine.FinalFormState> _attackStates;
    private float _attackChangeInterval = 1f;
    private float _timeSinceLastAttackChange;
    private WallCheck _wallCheck;
    private bool _isCloseWall = false;

    public FinalIdleState(StateMachine<FinalFormStateMachine.FinalFormState> stateMachine, Animator animator, Rigidbody2D rb, WallCheck wallCheck)
        : base(stateMachine, FinalFormStateMachine.FinalFormState.IdleFinal)
    {
        _animator = animator;
        _rb = rb;
        _wallCheck = wallCheck;
        _attackStates = new List<FinalFormStateMachine.FinalFormState>
        {
           //FinalFormStateMachine.FinalFormState.Attack4,
           FinalFormStateMachine.FinalFormState.Attack1,
           // FinalFormStateMachine.FinalFormState.Attack7,
           //FinalFormStateMachine.FinalFormState.Attack3,
           //FinalFormStateMachine.FinalFormState.Attack2,
           //FinalFormStateMachine.FinalFormState.Attack5,
        };
    }

    public override void EnterState()
    {
        _timeSinceLastAttackChange = 0f;
        _isCloseWall = false; 
    }

    public override void ExitState()
    {
        _animator.SetBool("back", false);
        _animator.SetFloat("Speed", 0f);
    }

    public override void UpdateState()
    {
        if (_wallCheck.distanceToWall <= 1)
        {
                _animator.SetBool("back", false);
                _animator.SetFloat("Speed", 0f);
                _isCloseWall = true;
        }
        else
        {
            if (!_isCloseWall)
            {
              
                _isCloseWall = true;
                _animator.SetBool("back", true);
                _animator.SetFloat("Speed", 2f);

                Vector2 wallPosition = new Vector2(_wallCheck.wallObject.position.x, _rb.position.y);
                Vector2 direction = (wallPosition - _rb.position).normalized;
                _rb.velocity = direction * 10f;
            }
        }
        if (_isCloseWall)
        {
            _timeSinceLastAttackChange += Time.deltaTime;

            if (_timeSinceLastAttackChange >= _attackChangeInterval)
            {
                FinalFormStateMachine.FinalFormState randomAttackState = _attackStates[Random.Range(0, _attackStates.Count)];
                StateMachine.QueueNextState(randomAttackState);
                _timeSinceLastAttackChange = 0f;
            }
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

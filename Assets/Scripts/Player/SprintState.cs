using System.Collections;
using UnityEngine;

public class SprintState : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private float _moveSpeed = 10f; 
    private float _sprintDuration = 0.3f; 
    private Rigidbody2D _rb;
    private Coroutine _sprintCoroutine;
    private FlipCharacter _flip;
    private bool _isDashing = false; 
    private GameObject[] _dust;
    private Transform _dustPosition;
    private GroundCheck _groundCheck;
    private AudioManager _audioManager;

    public SprintState(StateMachine<PlayerStateMachine.PlayerState> stateMachine, Animator animator, Rigidbody2D rb, FlipCharacter flip, Transform dustPosition, GameObject[] dust, GroundCheck groundCheck, AudioManager audioManager)
        : base(stateMachine, PlayerStateMachine.PlayerState.Sprint)
    {
        _animator = animator;
        _rb = rb;
        _flip = flip;
        _dust = dust;
        _dustPosition = dustPosition;
        _groundCheck = groundCheck;
        _audioManager = audioManager;
    }

    public override void EnterState()
    {
        _audioManager.PlaySoundEffect("dash");
        _groundCheck.canMove = false;
        GameObject currentDust = Object.Instantiate(_dust[0], _dustPosition.position, _dustPosition.rotation);
        FlipEffect(currentDust);
        _animator.SetTrigger("dash");
        if (_sprintCoroutine != null)
        {
            StateMachine.StopCoroutine(_sprintCoroutine);
        }

        _isDashing = true;
        _sprintCoroutine = StateMachine.StartCoroutine(SprintCoroutine());
    }

    private IEnumerator SprintCoroutine()
    {
        Vector2 moveDirection = new Vector2(_flip.IsFacingRight() ? 1 : -1, 0);
        _rb.velocity = new Vector2(moveDirection.x * _moveSpeed, _rb.velocity.y);
        yield return new WaitForSeconds(_sprintDuration);
        _rb.velocity = Vector2.zero;
        _isDashing = false;
        StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Idle);
    }
    private void FlipEffect(GameObject effect)
    {
        if (!_flip.IsFacingRight())
        {
            Vector3 scale = effect.transform.localScale;
            scale.x *= -1;  
            effect.transform.localScale = scale;
        }
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Sprint State");
        _groundCheck.canMove = true;
        if (_sprintCoroutine != null)
        {
            StateMachine.StopCoroutine(_sprintCoroutine);
            _sprintCoroutine = null;
        }

        _isDashing = false;
    }

    public override void UpdateState()
    {
        if (Input.GetKeyDown(KeyCode.F) && _isDashing)
        {
            StateMachine.QueueNextState(PlayerStateMachine.PlayerState.WalkAttack);
        }
    }


    public override PlayerStateMachine.PlayerState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}

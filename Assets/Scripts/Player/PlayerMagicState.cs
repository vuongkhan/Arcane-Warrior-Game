using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicState : BaseState<PlayerStateMachine.PlayerState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private Transform _shootPosition;
    private GameObject _thunder;
    private FlipCharacter _flip;
    private Vector3 thunderspearPosition;
    private GameObject thunderspear;
    private GameObject[] _dust;
    private AudioManager _audioManager;

    public PlayerMagicState(StateMachine<PlayerStateMachine.PlayerState> stateMachine, Animator animator, Rigidbody2D rb, Transform shootPosition, GameObject thunder, FlipCharacter flip, GameObject[] dust, AudioManager audioManager)
        : base(stateMachine, PlayerStateMachine.PlayerState.Magic)
    {
        _animator = animator;
        _rb = rb;
        _shootPosition = shootPosition;
        _thunder = thunder;
        _flip = flip;
        _dust= dust;
        _audioManager = audioManager;
    }

    public override void EnterState()
    {
        PlayerStateMachine player = (PlayerStateMachine)StateMachine;
        bool isMagic = player.isMagic;

        if (!isMagic)
        {
            player.isMagic = true;
            _animator.SetTrigger("FastMagic");
            _audioManager.PlaySoundEffect("bird-magic-skill");

            thunderspear = Object.Instantiate(_thunder, _shootPosition.position, _shootPosition.rotation);
            Rigidbody2D thunderrb = thunderspear.GetComponent<Rigidbody2D>();
            if (thunderrb != null)
            {
                Vector2 shootDirection = _flip.IsFacingRight() ? Vector2.right : Vector2.left;
                thunderrb.velocity = shootDirection * 8f;
                Flip(thunderspear);
            }
            thunderspearPosition = thunderspear.transform.position;
        }
        else 
        {
            if (thunderspear != null)
            {
                _audioManager.PlaySoundEffect("telport");
                GameObject effectthrow = Object.Instantiate(_dust[3], _shootPosition.position, _shootPosition.rotation);
                _rb.position = thunderspear.transform.position;
                GameObject effectthrow1 = Object.Instantiate(_dust[3], thunderspear.transform.position, _shootPosition.rotation);
                Object.Destroy(thunderspear);
                thunderspear = null;
                player.isMagic = false;
            }
        }

        StateMachine.QueueNextState(PlayerStateMachine.PlayerState.Idle);
    }

    private void Flip(GameObject effect)
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
    }

    public override void UpdateState()
    {

    }

    public override PlayerStateMachine.PlayerState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine<PlayerStateMachine.PlayerState>
{
    [SerializeField] private float _moveSpeed = 5f;
    public Animator _animator;
    private Rigidbody2D _rb;
    private FlipCharacter _flip;
    [SerializeField] private float _jumpForce;
    private GroundCheck _groundCheck;
    [SerializeField] private Transform _shootPosition;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField]  private Transform _punchPosition;
    [SerializeField] private Transform _dustPosition;
    [SerializeField] private GameObject[] _bulletType;
    [SerializeField] private GameObject[] _effectCharge;
    [SerializeField] private GameObject[] _dust;
    [SerializeField] private Transform _playerPosition;
    [SerializeField] private GameObject _thunder;
    public bool isMagic;
    private AudioManager _audioManager;
    private HealthManager _health;


    public enum PlayerState
    {
        Idle,
        Run,
        Jump,
        Fall,
        Attack1,
        Attack2,
        Attack3,
        WalkAttack,
        Sprint,
        Shoot,
        Hit,
        Die,
        JumpForward,
        Magic,
        JumpAttack
    }

    void Awake()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _flip = GetComponent<FlipCharacter>();
        _groundCheck = GetComponent<GroundCheck>();
        _health = GetComponent<HealthManager>();

        RegisterState(PlayerState.Idle, new IdleState(this,_animator,_moveSpeed, _groundCheck, _health));
        RegisterState(PlayerState.JumpAttack, new JumpAttackState(this, _animator, _moveSpeed));
        RegisterState(PlayerState.Run, new RunState(this, _animator, _moveSpeed, _rb, _flip, _groundCheck, _audioManager));
        RegisterState(PlayerState.Sprint, new SprintState(this, _animator, _rb, _flip, _dustPosition, _dust, _groundCheck, _audioManager));
        RegisterState(PlayerState.Jump, new JumpState(this, _animator, _moveSpeed, _rb, _jumpForce, _groundCheck));
        RegisterState(PlayerState.JumpForward, new JumpFowardState(this, _animator, _moveSpeed, _rb, _jumpForce, _groundCheck, _flip));
        RegisterState(PlayerState.Fall, new FallState(this, _animator, _moveSpeed, _rb, _groundCheck, _flip));
        RegisterState(PlayerState.Attack1, new Attack1State(this, _animator, _rb, _punchPosition, _audioManager));
        RegisterState(PlayerState.Attack3, new Attack3State(this, _animator, _rb, _punchPosition, _audioManager));
        RegisterState(PlayerState.Attack2, new Attack2State(this, _animator, _rb, _punchPosition, _audioManager));
        RegisterState(PlayerState.WalkAttack, new WalkAttackState(this, _animator, _rb));
        RegisterState(PlayerState.Shoot, new ShootState(this, _animator, _rb, _shootPosition,_flip, _bulletType, _effectCharge, _dustPosition, _dust, _groundCheck, _audioManager, _health));
        RegisterState(PlayerState.Hit, new PlayerHitState(this, _animator, _rb, _groundCheck, _dust, _audioManager));
        RegisterState(PlayerState.Die, new PlayerDieState(this, _animator, _rb, _groundCheck, _audioManager));
        RegisterState(PlayerState.Magic, new PlayerMagicState(this, _animator, _rb, _shootPosition, _thunder, _flip, _dust, _audioManager));
        SetInitialState(PlayerState.Idle);
    }

}


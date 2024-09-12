using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1StateMachine : StateMachine<Boss1StateMachine.Boss1State>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    public float _Hp;
    public bool _canHurt=true;
    [SerializeField] private Transform _pointPosition;
    [SerializeField] private GameObject _firePrefab;
    [SerializeField] private Transform _pointUlti;
    [SerializeField] private GameObject _meteorPrefab;
    private WallCheck _wallCheck;
    private FlipCharacter _flip;
    [SerializeField] private GameObject _charge;
    [SerializeField] private GameObject _finalForm;
    private Transform _playerTransform;
    [SerializeField] private GameObject[] _effect;
    private EnemyAudioManager _audioManager;
    public enum Boss1State
    {
        Idle,
        Charge,
        Attack1,
        Attack2,
        Attack3,
        Attack5,
        Hit,
        Die
        
    }
    void Awake()
    {
        GameObject playerObject = GameObject.Find("Player");
        if (playerObject != null)
        {
            _playerTransform = playerObject.transform;
        }
        _audioManager = FindObjectOfType<EnemyAudioManager>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _flip = GetComponent<FlipCharacter>();
        _wallCheck = GetComponent<WallCheck>();
        _flip = GetComponent<FlipCharacter>();

        RegisterState(Boss1State.Idle, new Boss1IdleState(this, _animator, _rb));
        RegisterState(Boss1State.Charge, new Boss1ChargeState(this, _animator, _rb));

        RegisterState(Boss1State.Hit, new Boss1HitState(this, _animator, _rb));
        RegisterState(Boss1State.Attack1, new Boss1Attack1State(this, _animator, _rb,_playerTransform, _wallCheck, _audioManager));
        RegisterState(Boss1State.Attack2, new Boss1Attack2State(this, _animator, _rb, _wallCheck, _flip, _charge, _pointPosition, _effect, _audioManager));
        RegisterState(Boss1State.Attack3, new Boss1Attack3State(this, _animator, _rb, _meteorPrefab, _pointUlti, _flip, _effect, _audioManager));
        RegisterState(Boss1State.Attack5, new Boss1Attack5State(this, _animator, _rb, _meteorPrefab, _pointUlti, _flip, _effect, _audioManager, _playerTransform));
        RegisterState(Boss1State.Die, new Boss1DieState(this, _animator, _rb, _effect, _finalForm, _audioManager));

        SetInitialState(Boss1State.Idle);
    }
}

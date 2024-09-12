using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFormStateMachine : StateMachine<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    public float _Hp;
    public bool _canHurt = true;
    [SerializeField] private Transform _pointPosition;
    [SerializeField] private GameObject _firePrefab;
    [SerializeField] private Transform _pointAttackFinal;
    [SerializeField] private GameObject[] _blastPrefab;
    private WallCheck _wallCheck;
    private FlipCharacter _flip;
    private Transform _playerTransform;
    [SerializeField] private GameObject[] _effect;
    private EnemyAudioManager _audioManager;

    public enum FinalFormState
    {
        Ready,
        IdleFinal,
        Attack1,
        Attack2,
        Attack3,
        Attack4,
        Attack5,
        Attack6,
        Attack7,
        Attack8,
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

        RegisterState(FinalFormState.IdleFinal, new FinalIdleState(this, _animator, _rb, _wallCheck));
        RegisterState(FinalFormState.Ready, new ReadyState(this, _animator, _rb,_wallCheck, _audioManager));
        RegisterState(FinalFormState.Attack1, new FinalFormAttack1State(this, _animator, _rb, _playerTransform, _wallCheck, _audioManager));
        RegisterState(FinalFormState.Attack2, new FinalFormAttack2State(this, _animator, _rb, _playerTransform, _wallCheck, _blastPrefab, _pointAttackFinal,_audioManager, _effect));
        RegisterState(FinalFormState.Attack8, new FinalFormAttack8State(this, _animator, _rb, _playerTransform, _wallCheck, _blastPrefab, _pointAttackFinal, _audioManager));
        RegisterState(FinalFormState.Attack3, new FinalFormAttack3State(this, _animator, _rb, _blastPrefab, _flip, _audioManager));
        RegisterState(FinalFormState.Attack4, new FinalFormAttack4State(this, _animator, _rb, _blastPrefab, _flip, _audioManager));
        RegisterState(FinalFormState.Attack5, new FinalFormAttack5State(this, _animator, _rb, _blastPrefab, _flip, _audioManager));
        RegisterState(FinalFormState.Attack6, new FinalFormAttack6State(this, _animator, _rb, _playerTransform, _flip, _audioManager));
        RegisterState(FinalFormState.Attack7, new FinalFormAttack7State(this, _animator, _rb, _blastPrefab, _flip, _audioManager, _playerTransform));
        RegisterState(FinalFormState.Hit, new FinalFormHitState(this, _animator, _rb));
        RegisterState(FinalFormState.Die, new FinalFormDieState(this, _animator, _rb, _audioManager));
        SetInitialState(FinalFormState.Ready);
    }
}

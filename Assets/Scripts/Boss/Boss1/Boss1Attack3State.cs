using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Attack3State : BaseState<Boss1StateMachine.Boss1State>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private GameObject _meteorPrefab;
    private Transform _pointUlti;
    private FlipCharacter _flipCharacter;
    private GameObject _spawnedBigBangAttack; // Renamed from _spawnedMeteor
    private float _growthRate = 1.5f;
    private float _maxScale = 4.0f;
    private bool _isGrowing = true;
    private GameObject[] _effect;
    private GameObject instantiatedEffect;
    private float charge;
    private bool attacking = false;
    private EnemyAudioManager _audioManager;

    public Boss1Attack3State(StateMachine<Boss1StateMachine.Boss1State> stateMachine, Animator animator, Rigidbody2D rb, GameObject meteorPrefab, Transform pointUlti, FlipCharacter flipCharacter, GameObject[] effect, EnemyAudioManager audioManager)
        : base(stateMachine, Boss1StateMachine.Boss1State.Attack3)
    {
        _animator = animator;
        _rb = rb;
        _meteorPrefab = meteorPrefab;
        _pointUlti = pointUlti;
        _flipCharacter = flipCharacter;
        _effect = effect;
        _audioManager = audioManager;
    }

    public override void EnterState()
    {
        Boss1StateMachine boss = (Boss1StateMachine)StateMachine;
        boss._canHurt = false;
        _audioManager.PlayOneShotSoundEffect("aura");
        _audioManager.PlayOneShotSoundEffect("chargeBigbangAttack");
        _animator.SetBool("chargeBigbang", true);
        _spawnedBigBangAttack = GameObject.Instantiate(_meteorPrefab, _pointUlti.position, Quaternion.identity); // Updated variable name
        _spawnedBigBangAttack.transform.localScale = Vector3.one;
        if (_effect != null)
        {
            instantiatedEffect = GameObject.Instantiate(_effect[1], new Vector3(_rb.position.x + 0.1f, _rb.position.y + 0.5f, 0), Quaternion.identity);
        }
    }

    public override void ExitState()
    {
        Boss1StateMachine boss = (Boss1StateMachine)StateMachine;
        _animator.SetBool("chargeBigbang", false);
        _isGrowing = true;
        boss._canHurt = true;
        GameObject.Destroy(instantiatedEffect);
        _audioManager.StopOneShotSoundEffect();
        if (attacking)
        {
            GameObject.Destroy(_spawnedBigBangAttack); 
        }

    }

    public override void UpdateState()
    {
        if (_isGrowing)
        {
            attacking = true;
            Vector3 currentScale = _spawnedBigBangAttack.transform.localScale; 
            float newScale = Mathf.MoveTowards(currentScale.x, _maxScale, _growthRate * Time.deltaTime);
            _spawnedBigBangAttack.transform.localScale = new Vector3(newScale, newScale, 1); 

            if (newScale >= _maxScale)
            {
                _isGrowing = false;
                var rb = _spawnedBigBangAttack.GetComponent<Rigidbody2D>(); 
                if (rb != null)
                {
                    _audioManager.PlaySoundEffect("fireBigbangattack");
                    float direction = _flipCharacter.IsFacingRight() ? -1f : 1f;
                    rb.velocity = new Vector2(direction * 8, rb.velocity.y);
                    StateMachine.QueueNextState(Boss1StateMachine.Boss1State.Attack5);
                    GameObject.Destroy(instantiatedEffect);
                    attacking = false;
                }
            }
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

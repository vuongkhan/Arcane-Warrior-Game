using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalFormAttack4State : BaseState<FinalFormStateMachine.FinalFormState>
{
    private Animator _animator;
    private Rigidbody2D _rb;
    private GameObject[] _blastPrefab;
    private FlipCharacter _flip;
    private float hoverHeight = 3f; 
    private float hoverDelay = 0.5f;
    private float hoverForce = 15f; 
    private float projectileSpeed = 10f; 
    private bool hasSpawnedProjectile = false;
    private float reducedGravityScale = 0.3f;
    private EnemyAudioManager _audioManager;

    public FinalFormAttack4State(StateMachine<FinalFormStateMachine.FinalFormState> stateMachine, Animator animator, Rigidbody2D rb, GameObject[] blastPrefab, FlipCharacter flip, EnemyAudioManager audioManager)
        : base(stateMachine, FinalFormStateMachine.FinalFormState.Attack4)
    {
        _animator = animator;
        _rb = rb;
        _blastPrefab = blastPrefab;
        _flip = flip;
        _audioManager = audioManager;
    }

    public override void EnterState()
    {
        _animator.SetTrigger("fly");
        _rb.velocity = new Vector2(_rb.velocity.x, hoverForce);
        hasSpawnedProjectile = false; 
        StateMachine.StartCoroutine(Lock());
    }


    private IEnumerator Lock()
    {
        yield return new WaitForSeconds(0.1f);
        _rb.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(hoverDelay - 0.1f);
        _animator.SetTrigger("charge genkidama");
        if (!hasSpawnedProjectile)
        {
            SpawnObject();
            hasSpawnedProjectile = true;
        }
    }

    private void SpawnObject()
    {
        if (_blastPrefab.Length > 0)
        {
            Vector3 spawnPosition = new Vector3(_rb.position.x, _rb.position.y, 0) + new Vector3(0, hoverHeight, 0);
            GameObject spawnedObject = GameObject.Instantiate(_blastPrefab[2], spawnPosition, Quaternion.identity);
            Rigidbody2D projectileRb = spawnedObject.GetComponent<Rigidbody2D>();
            if (projectileRb != null)
            {
                projectileRb.gravityScale = 0;
            }
            StateMachine.StartCoroutine(ScaleUp(spawnedObject.transform, projectileRb));
        }
    }

    private IEnumerator ScaleUp(Transform projectileTransform, Rigidbody2D projectileRb)
    {
        Vector3 initialScale = projectileTransform.localScale;
        Vector3 targetScale = new Vector3(8f, 8f, 1f);
        float duration = 3f;

        _audioManager.PlaySoundEffect("bomb");
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {

            projectileTransform.localScale = Vector3.Lerp(initialScale, targetScale, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        projectileTransform.localScale = targetScale;
        Vector2 direction = new Vector2(-1, -1).normalized;
        if (projectileRb != null)
        {
            _animator.SetTrigger("throw");
            projectileRb.velocity = direction * projectileSpeed;
            projectileRb.gravityScale = reducedGravityScale;
            StateMachine.QueueNextState(FinalFormStateMachine.FinalFormState.Attack7);
        }
    }

    public override void ExitState()
    {
        _rb.constraints = RigidbodyConstraints2D.None;
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb.velocity = new Vector2(_rb.velocity.x, 0);
        hasSpawnedProjectile = false;
        _animator.ResetTrigger("throw");
        _animator.ResetTrigger("charge genkidama");
        _animator.ResetTrigger("fly");

    }

    public override void UpdateState() { }

    private void FlipEffect(GameObject effect)
    {
        if (!_flip.IsFacingRight())
        {
            Vector3 scale = effect.transform.localScale;
            scale.x *= -1;
            effect.transform.localScale = scale;
        }
    }

    public override FinalFormStateMachine.FinalFormState GetNextState()
    {
        return StateMachine.GetQueuedState();
    }

    public override void OnTriggerEnter2D(Collider2D other) { }
    public override void OnTriggerStay2D(Collider2D other) { }
    public override void OnTriggerExit2D(Collider2D other) { }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private float checkDistance = 0.2f; // Khoảng cách kiểm tra
    [SerializeField] private LayerMask groundLayer; // Lớp mặt đất
    [SerializeField] private float moveSpeed = 2f; // Tốc độ di chuyển
    private Rigidbody2D _rb;
    private bool isGrounded;
    private Animator _animator;
    private PlayerStateMachine _playerStateMachine;
    private FlipCharacter _flip;
    public bool canMove=true;
    private AudioManager _audioManager;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _playerStateMachine = GetComponent<PlayerStateMachine>();
        _flip = GetComponent<FlipCharacter>();
        _audioManager = FindObjectOfType<AudioManager>();

        if (_rb == null)
        {
            Debug.LogError("Rigidbody2D component not found!");
        }

        if (_animator == null)
        {
            Debug.LogError("Animator component not found!");
        }

        if (_playerStateMachine == null)
        {
            Debug.LogError("PlayerStateMachine component not found!");
        }

        if (_flip == null)
        {
            Debug.LogError("FlipCharacter component not found!");
        }
    }

    public bool IsGrounded() => isGrounded;

    private void FixedUpdate()
    {
        CheckIfGrounded();
        HandleMovement();
    }

    private void CheckIfGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, checkDistance, groundLayer);
        bool wasGrounded = isGrounded;
        isGrounded = hit.collider != null;
        if (wasGrounded && !isGrounded)
        {
            _animator.SetBool("isFalling", true);
        }
        else if (!wasGrounded && isGrounded)
        {
            _audioManager.PlaySoundEffect("land");
            _animator.SetTrigger("Grounded");
            _animator.SetBool("isFalling", false);
            Debug.Log("adawdawdawdawdssdxczxc");
            _playerStateMachine.QueueNextState(PlayerStateMachine.PlayerState.Idle);
        }
    }

    private void HandleMovement()
    {
        if (canMove)
        {
        float horizontalInput = Input.GetAxis("Horizontal");
        _rb.velocity = new Vector2(horizontalInput * moveSpeed, _rb.velocity.y);

        if (horizontalInput > 0 && !_flip.IsFacingRight())
        {
            _flip.Flip();
        }
        else if (horizontalInput < 0 && _flip.IsFacingRight())
        {
            _flip.Flip();
        }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * checkDistance);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    private PlayerStateMachine player;
    private bool isMovingBack = false;
    private bool isSKeyHeld = false;
    private float moveSpeed = 25f;
    private float holdDuration = 1f;
    private float keyHeldTime = 0f;
    public bool hasCollided = false;
    private Animator _animator;

    private Vector3 startPoint;
    private Vector3 controlPoint;
    private Vector3 endPoint;
    private float t = 0f;
    private AudioManager _audioManager;

    private void Start()
    {
        _audioManager = FindObjectOfType<AudioManager>();
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerStateMachine>();
            if (player != null)
            {
                Debug.Log("PlayerStateMachine component found");
            }
            else
            {
                Debug.LogWarning("PlayerStateMachine component cannot found");
            }
        }
        else
        {
            Debug.LogWarning("Player not found.");
        }
        _animator = GetComponent<Animator>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (isMovingBack)
            {
                player.isMagic = false;
                Destroy(gameObject);

            }
        }
        else
        {
            _animator.SetBool("hasCollided", true);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            player.isMagic = false;
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("bigbang"))
        {
            player.isMagic = false;
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {

            if (!isSKeyHeld)
            {
                isSKeyHeld = true;
                keyHeldTime = Time.time;
            }
            else if (Time.time - keyHeldTime >= holdDuration)
            {
                if (!isMovingBack)
                {
                    _audioManager.PlaySoundEffect("bird-thunder-back");
                    player._animator.SetTrigger("back");
                    _animator.SetBool("hasCollided", false);
                    isMovingBack = true;
                    startPoint = transform.position;
                    endPoint = player.transform.position;
                    controlPoint = (startPoint + endPoint) / 2 + Vector3.up * 2f;
                }
            }
        }
        else
        {
            isSKeyHeld = false;
            keyHeldTime = 0f;
        }

        if (isMovingBack)
        {
            t += moveSpeed * Time.deltaTime / Vector3.Distance(startPoint, endPoint);
            t = Mathf.Clamp01(t); 

            transform.position = calculateBezier(t, startPoint, controlPoint, endPoint);
            if (t >= 1f)
            {
                player.isMagic = false;
                Destroy(gameObject);
            }
        }
    }
    private Vector3 calculateBezier(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0; 
        p += 2 * u * t * p1; 
        p += tt * p2;
        return p;
    }
}

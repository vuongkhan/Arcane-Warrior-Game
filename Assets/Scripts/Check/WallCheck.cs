using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCheck : MonoBehaviour
{
    [SerializeField] private float checkDistance = 0.2f; 
    [SerializeField] private LayerMask wallLayer; 

    private bool isTouched;
    private FlipCharacter _flipCharacter; 
    private Boss1StateMachine boss1statemachine;
    public float distanceToWall;
    public Transform wallObject;


    private void Awake()
    {
        _flipCharacter = GetComponent<FlipCharacter>();
        boss1statemachine = GetComponent<Boss1StateMachine>();
    }

    public bool IsTouched() => isTouched;

    private void Update()
    {
        CheckIfWalled();
    }

    private void CheckIfWalled()
    {
        if (_flipCharacter != null)
        {
            Vector2 direction = _flipCharacter.IsFacingRight() ? Vector2.right : Vector2.left;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, checkDistance, wallLayer);
            if (hit.collider != null)
            {
                wallObject = hit.collider.transform; 
                isTouched = true;
                distanceToWall = hit.distance;
            }
            else
            {
                isTouched = false;
            }
        }
    }
}

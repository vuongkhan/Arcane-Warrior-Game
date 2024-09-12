using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer : MonoBehaviour
{
    [SerializeField] private float checkDistance = 2.2f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float timeToLosePlayer = 2f; // Thời gian để chuyển isPlayer thành false

    private Transform lastHitTransform; // Lưu thông tin về đối tượng va chạm gần nhất
    private bool isPlayer;
    private float timeSinceLastDetection;

    private FlipCharacter flipCharacter; // Tham chiếu đến FlipCharacter script

    private void Start()
    {
        flipCharacter = GetComponent<FlipCharacter>();
        if (flipCharacter == null)
        {
            Debug.LogError("Script not be found");
        }
    }

    public bool IsPlayer() => isPlayer;
    public Vector2 GetPlayerPosition() => lastHitTransform != null ? (Vector2)lastHitTransform.position : Vector2.zero;

    private void Update()
    {
        CheckIfPlayer();
    }

    private void CheckIfPlayer()
    {
        if (playerLayer == 0)
        {
            return;
        }

        if (flipCharacter == null) return;
        Vector2 rayDirection = flipCharacter.IsFacingRight() ? Vector2.right : Vector2.left;
        Vector2 rayStartPosition = new Vector2(transform.position.x, transform.position.y - 0.5f); 
        RaycastHit2D hit = Physics2D.Raycast(rayStartPosition, rayDirection, checkDistance, playerLayer);

        if (hit.collider != null)
        {
            isPlayer = true;
            lastHitTransform = hit.collider.transform; 
            timeSinceLastDetection = 0f; 
        }
        else
        {
            timeSinceLastDetection += Time.deltaTime;
            if (timeSinceLastDetection >= timeToLosePlayer)
            {
                isPlayer = false; 
            }
        }
    }
}

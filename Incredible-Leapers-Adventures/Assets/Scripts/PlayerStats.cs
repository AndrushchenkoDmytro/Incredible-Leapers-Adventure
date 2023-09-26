using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats",menuName = "PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public float health = 100f;
    [Header("Move State")]
    public float movementSpeed = 10f;

    [Header("Jump State")]
    public float jumpForce = 5.5f;
    public int maxJumpCount = 2;
    public int jumpCount = 2;

    [Header("Check Variables")]
    public float GroundCheckRadius = 0.25f;
    public LayerMask groundLayer;
}

using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private static readonly int Swing = Animator.StringToHash("Swing");

    /// <summary>
    /// This script handles all things combat-related for the player sans holding the PlayerStats.
    /// Please see PlayerMovement.cs for movement and jumping mechanics, and PlayerStats.cs for player stats and health management.
    /// </summary>

    // [Header("Equipment")]
    // [SerializeField] private GameObject playerSword;   // assigned in inspector on the Player prefab
    // [SerializeField] private GameObject playerShield;

    [Header("State")]
    private bool canSwing;
    private bool canBlock;

    [Header("Components")]
    private PlayerEffects effects;
    private Animator animator;

    private void Awake()
    {
        effects = GetComponent<PlayerEffects>();
        if (!effects)
        {
            Debug.LogError("PlayerEffects component is missing from the Player prefab!");
        }
    }

    private void Start()
    {
        // playerSword.SetActive(false);
        // playerShield.SetActive(false);

        canSwing = true;
        canBlock = true;
        
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSwing)
        {
            Debug.Log("Clicky");
            SwingSword();
        }
        if (Input.GetMouseButtonDown(1) && canBlock)
        { 
            UseShield();
        }
    }

    // SwingSword() and UseShield() both only affect cooldowns and display time.
    // The actual hit logic will live on the enemy script; the enemy script has a collision detector that grabs player damage from PlayerStats.
    private void SwingSword()
    {
        if (!canSwing) return;         // Fail safe.
        Debug.Log("Swangin'");
        PlayerMovement.Instance.canMove = false;
        canSwing = false;
        canBlock = false;
        animator.SetTrigger(Swing);
        StartCoroutine(AttackCooldown());
    }
    
    // This below was designed this way so that player cannot brute force their way through by holding down both buttons. 
    // You either swing, or you block. This way, the player has to make rapid meaningful choices in combat.
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(.75f);    // Times with the animator
        canSwing = true;
        canBlock = true;
        PlayerMovement.Instance.canMove = true;
    }
    private void UseShield()
    {
        if (!canBlock) return;         // Fail safe.

        canSwing = false;
        canBlock = false;
        PlayerMovement.Instance.canMove = false;
        StartCoroutine(BlockCooldown());
    }
    private IEnumerator BlockCooldown()
    {
        yield return new WaitForSeconds(.25f);
        canSwing = true;
        canBlock = true;
        PlayerMovement.Instance.canMove = true;
    }
}

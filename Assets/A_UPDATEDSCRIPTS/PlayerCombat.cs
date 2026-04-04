using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    /// <summary>
    /// This script handles all things combat-related for the player sans holding the PlayerStats. Please see PlayerMovement.cs for movement and jumping mechanics, and PlayerStats.cs for player stats and health management.
    /// </summary>
    [SerializeField] private GameObject playerSword;    // assigned in inspector on the Player prefab
    [SerializeField] private GameObject playerShield;
    
    private bool canSwing;
    private bool canBlock;

    private float attackWindow;
    private float blockWindow;

    private PlayerEffects effects;
    private void Awake()
    {
        effects = GetComponent<PlayerEffects>();
        if (effects == null)
        {
            Debug.LogError("PlayerEffects component is missing from the Player prefab!");
        }
    }

    private void Start()
    {
        playerSword.SetActive(false);
        playerShield.SetActive(false);

        attackWindow = PlayerStats.Instance.PlayerAttackSpeed;
        blockWindow = PlayerStats.Instance.PlayerBlockTime;

        canSwing = true;
        canBlock = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && canSwing)
        {
            SwingSword();
        }
        if (Input.GetMouseButtonDown(1) && canBlock)
        { 
            if (canBlock) UseShield();
        }
    }

    // SwingSword() and UseShield() both only affect cooldowns and display time.
    // The actual hit logic will live on the enemy script; the enemy script has a collision detector that grabs player damage from PlayerStats.
    private void SwingSword()
    {
        if (!canSwing) return;         // Fail safe.

        canSwing = false;
        canBlock = false;
        effects.StartCoroutine(effects.ShowSword(attackWindow));
        StartCoroutine(AttackCooldown());
    }
    // This below was designed this way so that player cannot brute force their way through by holding down both buttons. 
    // You either swing, or you block. This way the player has to make rapid meaningful choices in combat.
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(attackWindow);
        canSwing = true;
        canBlock = true;
    }
    private void UseShield()
    {
        if (!canBlock) return;         // Fail safe.

        effects.StartCoroutine(effects.ShowShield(blockWindow));
        StartCoroutine(BlockCooldown());
    }
    private IEnumerator BlockCooldown()
    {
        yield return new WaitForSeconds(blockWindow);
        canSwing = true;
        canBlock = true;
    }
}

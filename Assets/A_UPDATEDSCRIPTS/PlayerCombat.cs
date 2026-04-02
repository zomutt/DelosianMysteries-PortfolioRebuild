using System.Collections;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    /// <summary>
    /// This script handles all things combat-related for the player sans holding the PlayerStats. Please see PlayerMovement.cs for movement and jumping mechanics, and PlayerStats.cs for player stats and health management.
    /// </summary>
    [SerializeField] private GameObject playerSword;    // assigned in inspector on the Player prefab
    private bool canSwing;

    [SerializeField] private GameObject playerShield;
    private bool canBlock;

    [SerializeField] private float activeTime;          // Swing and block will both have the same duration for balance and thoughtful combat choices.
    
    private PlayerEffects effects;
    private void Awake()
    {
        effects = GetComponent<PlayerEffects>();        // Both scripts live on Player prefab
    }
    private void Start()
    {
        playerSword.SetActive(false);
        playerShield.SetActive(false);

        canSwing = true;
        canBlock = true;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canSwing) SwingSword();
        }
        if (Input.GetMouseButtonDown(1))
        { 
            if (canBlock) UseShield();
        }
    }

    // SwingSword() and UseShield() both only affect cooldowns and display time.
    // The actual hit logic will live on the enemy script; the enemy script has a collision detector that grabs player damage from PlayerStats.
    private void SwingSword()
    {
        if (!canSwing) return;         // Fail safe.
        effects.StartCoroutine(effects.ShowSword(activeTime));
        StartCoroutine(Cooldown());
    }

    private void UseShield()
    {
        if (!canBlock) return;         // Fail safe.
        effects.StartCoroutine(effects.ShowShield(activeTime));
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        // This was designed this way so that player cannot brute force their way through by holding down both buttons. 
        // You either swing, or you block. This way the player has to make rapid meaningful choices in combat.
        canSwing = false;
        canBlock = false;
        yield return new WaitForSeconds (activeTime);
        canSwing = true;
        canBlock = true;
    }
}

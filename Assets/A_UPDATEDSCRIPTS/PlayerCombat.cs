using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class PlayerCombat : MonoBehaviour
{
    /// <summary>
    /// This script handles all things combat-related for the player sans holding the PlayerStats. Please see PlayerMovement.cs for movement and jumping mechanics, and PlayerStats.cs for player stats and health management.
    /// </summary>
    [SerializeField] private GameObject playerSword;    // assigned in inspector on the Player prefab
    private Rigidbody2D rbSword;
    private bool canSwing;

    [SerializeField] private GameObject playerShield;
    private Rigidbody2D rbShield;
    private bool canBlock;

    [SerializeField] private float activeTime;          // swing and block will both have the same duration for balance and thoughtful combat choices.
    
    private PlayerEffects effects;
    private void Awake()
    {
        effects = GetComponent<PlayerEffects>();        // both scripts live on Player prefab
    }
    private void Start()
    {
        if (playerSword != null)
        {
            rbSword = playerSword.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.Log("PlayerCombat can't find playerSword GameObject.");
        }

        if (playerShield != null)
        {
            rbShield = playerShield.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.Log("PlayerCombat can't find playerShield GameObject.");
        }
        
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
    private void SwingSword()
    {
        effects.StartCoroutine(effects.ShowSword(activeTime));
        StartCoroutine(Cooldown());
    }

    private void UseShield()
    {
        effects.StartCoroutine(effects.ShowShield(activeTime));
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        // this was designed this way so that player cannot brute force their way through by holding down both buttons. 
        // you either swing, or you block. this way the player has to make meaningful quick choices in combat.
        canSwing = false;
        canBlock = false;
        yield return new WaitForSeconds (activeTime);
        canSwing = true;
        canBlock = true;
    }
}

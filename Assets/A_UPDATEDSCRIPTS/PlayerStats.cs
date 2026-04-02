using UnityEngine;

public class PlayerStats
{
    public static PlayerStats Instance { get; } = new PlayerStats();

    /// CONSTRUCTOR
    ///  More stuff can be added later (i.e. stamina, other stats)
    private PlayerStats()     
    {
        ResetStats();
    }

    /// HEALTH DATA
    private float maxHealth = 100;
    internal float MaxHealth => maxHealth;

    private float currentHealth;
    internal float CurrentHealth => currentHealth;

    /// HEALTH METHODS
    internal void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UIController.Instance.UpdateUI();
    }
    internal void HealDamage(float heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UIController.Instance.UpdateUI();
    }
    // Player can permanently increase their maxHP by finding pomegranate pickups
    internal void IncreaseMaxHealth(float healthBoost)
    {
        maxHealth += healthBoost;
        currentHealth += healthBoost;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log("Player max health increased, new max health: " + maxHealth);
        UIController.Instance.UpdateUI();
    }

    /// DAMAGE METHODS
    private float playerDamage = 10;
    internal float PlayerDamage => playerDamage;

    // Called when player finds sword upgrades, meant to be persistent and not reset. Player is unable to interact with upgrades that have already been grabbed.
    internal void IncreaseDamage(float damage)        
    {
        playerDamage += damage;
        Debug.Log("Player damage increased, new damage: " + playerDamage);
        UIController.Instance.UpdateUI();
    }

    /// RESET/SPAWN LOGIC
    internal void ResetStats()
    {
        currentHealth = maxHealth;
        UIController.Instance.UpdateUI();
    }
}


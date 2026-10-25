using Unity.Burst.CompilerServices;
using UnityEngine;

public class PlayerStats
{
    public static PlayerStats Instance { get; } = new PlayerStats();

    /// CONSTRUCTOR
    ///  More stuff can be added later (i.e. stamina, other stats).
    private PlayerStats()     
    {
        currentHealth = maxHealth;
    }

    /// HEALTH DATA
    private int maxHealth = 100;
    internal int MaxHealth => maxHealth;

    private int currentHealth;
    internal int CurrentHealth => currentHealth;

    /// HEALTH METHODS
    internal void TakeDamage(int damage)
    {
        currentHealth -= damage;
        ClampHealth();
        if (UIController.Instance) UIController.Instance.UpdateUI();
    }
    internal void HealPlayer(int heal)
    {
        currentHealth += heal;
        ClampHealth();
        if (UIController.Instance) UIController.Instance.UpdateUI();
    }
    private void ClampHealth()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }
    // Player can permanently increase their maxHP by finding pomegranate pickups
    internal void IncreaseMaxHealth(int healthBoost)
    {
        maxHealth += healthBoost;
        currentHealth += healthBoost;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        Debug.Log($"Player max health increased, new max health: {maxHealth}");
        if (UIController.Instance) UIController.Instance.UpdateUI();
    }

    /// DAMAGE METHODS
    internal int PlayerDamage { get; private set; } = 10;

    internal float PlayerAttackSpeed { get; } = .25f;

    internal float PlayerBlockTime { get; } = .25f;

    // Called when player finds sword upgrades, meant to be persistent and not reset. Player is unable to interact with upgrades that have already been grabbed.
    internal void IncreaseDamage(int damage)        
    {
        PlayerDamage += damage;
        Debug.Log($"Player damage increased, new damage: {PlayerDamage}");
        if (UIController.Instance) UIController.Instance.UpdateUI();
    }

    /// RESET/SPAWN LOGIC
    internal void ResetStats()
    {
        currentHealth = maxHealth;
        if (UIController.Instance) UIController.Instance.UpdateUI();
    }
}


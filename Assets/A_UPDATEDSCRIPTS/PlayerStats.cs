using UnityEngine;

public class PlayerStats
{
    public static PlayerStats Instance { get; } = new PlayerStats();

    /// CONSTRUCTOR
    private PlayerStats()      // constructor,, more stuff can be added later (i.e. stamina, other stats)
    {
        currentHealth = maxHealth;
    }

    /// HEALTH DATA
    private float maxHealth = 100;
    internal float MaxHealth => maxHealth;

    internal float currentHealth;
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

    /// DAMAGE METHODS
    private float playerDamage = 10;
    internal float PlayerDamage => playerDamage;
    internal void IncreaseDamage(float damage)        // called when player finds sword upgrades
    { 
        playerDamage += damage;
        UIController.Instance.UpdateUI();
    }
}

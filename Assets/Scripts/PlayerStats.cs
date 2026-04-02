using UnityEngine;

public class PlayerStats
{
    private static PlayerStats instance;
    public static PlayerStats Instance       // prevents accidental re-instantiation
    {
        get
        {
            if (instance == null)
                instance = new PlayerStats();
            return instance;
        }
    }
    /// CONSTRUCTOR
    private PlayerStats()      // constructor,, more stuff can be added later (i.e. stamina, other stats)
    {
        currentHealth = maxHealth;
    }

    /// HEALTH DATA
    private float maxHealth = 100;
    public float MaxHealth => maxHealth;

    private float currentHealth;
    public float CurrentHealth => currentHealth;

    /// HEALTH METHODS
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
        UIController.Instance.UpdateUI();
    }
    public void HealDamage(float heal)
    {
        currentHealth += heal;
        if (currentHealth > maxHealth) currentHealth = maxHealth;
        UIController.Instance.UpdateUI();
    }

    /// DAMAGE METHODS
    private float playerDamage = 10;
    public float PlayerDamage => playerDamage;
    public void IncreaseDamage(float damage)        // called when player finds sword upgrades
    { 
        playerDamage += damage;
        UIController.Instance.UpdateUI();
    }
}

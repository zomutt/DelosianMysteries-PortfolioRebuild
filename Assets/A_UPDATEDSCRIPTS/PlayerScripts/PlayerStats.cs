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
        if (UIController.Instance != null) UIController.Instance.UpdateUI();
    }
    internal void HealPlayer(int heal)
    {
        currentHealth += heal;
        ClampHealth();
        PlayerEffects.Instance.FlashPlayerColor(Color.green);
        if (UIController.Instance != null) UIController.Instance.UpdateUI();
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
        if (UIController.Instance != null) UIController.Instance.UpdateUI();
    }

    /// DAMAGE METHODS
    private int playerDamage = 10;
    internal int PlayerDamage => playerDamage;
    [SerializeField] private float playerAttackSpeed = .25f;
    internal float PlayerAttackSpeed => playerAttackSpeed;
    [SerializeField] private float playerBlockTime = .25f;
    internal float PlayerBlockTime => playerBlockTime;

    // Called when player finds sword upgrades, meant to be persistent and not reset. Player is unable to interact with upgrades that have already been grabbed.
    internal void IncreaseDamage(int damage)        
    {
        playerDamage += damage;
        Debug.Log($"Player damage increased, new damage: {playerDamage}");
        if (UIController.Instance != null) UIController.Instance.UpdateUI();
    }

    /// RESET/SPAWN LOGIC
    internal void ResetStats()
    {
        currentHealth = maxHealth;
        if (UIController.Instance != null) UIController.Instance.UpdateUI();
    }
}


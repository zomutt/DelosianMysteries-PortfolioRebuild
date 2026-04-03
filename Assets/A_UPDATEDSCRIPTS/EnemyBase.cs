using UnityEngine;

/// <summary>
/// This serves as the blueprint for which each enemy will use and derive behaviour from.
/// </summary>
public abstract class EnemyBase : MonoBehaviour
{
    [Header("Base Enemy Stats")]
    [SerializeField] protected int maxHealth;
    protected int currentHealth;
    protected int damage;

    [Header("Combat Timers")]
    protected float attackCD;
    protected float iframe;

    protected bool canAttack;
    protected bool canTakeDmg;

    [Header("Visuals")]
    // Most enemies will have a skull that rises and fades, but certain bosses and special enemies will have this differ.
    [SerializeField] protected GameObject deathSprite;

    // These are for Object Pooling and Resetting. Each group of enemies have their own "zone".
    // This is done to reduce clutter and system load, so each cluster can activate from the OP based on proximity.
    protected Vector3 startingPosition;
    protected Quaternion startingRotation;        // Will typically remain 0, but there are outliers.


    protected virtual void Awake()
    {
        // These are stored for reset purposes
        startingPosition = transform.position;
        startingRotation = transform.rotation;
    }

    protected virtual void OnEnable()
    {
        // This ensures stats are what they should be upon activation.
        currentHealth = maxHealth;
        ResetState();
    }
    internal virtual void TakeDamage(int amount)
    {
        // Occurs on collision with player sword
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (currentHealth <= 0) 
            DisableEnemy();
    }
    internal virtual void DisableEnemy()
    {
        // Either a sprite or animated skull will spawn. These images have their own scripts that will automatically be disabled/destroyed as needed.
        if (deathSprite != null)
            deathSprite.SetActive(true);
        gameObject.SetActive(false);
    }

    // This resets movement, AI, animations, etc.
    internal virtual void ResetState() 
    {
        // This is called in OnEnable, but we all know that things can just happen, so I like to have failsafes.
        currentHealth = maxHealth;
        canTakeDmg = true;
        canAttack = true;
    }

    // Handles unique movement logic (patrol, chase, wander, etc.)
    protected abstract void HandleMovement();
    // This handles unique behaviours (rules, visuals, timing, cds, etc.)
    protected abstract void AttackBehaviour();
    // And of course, the damage itself. This method is used to call the PlayerStats take damage in whichever way is most appropriate.
    protected abstract void ApplyDamage(int damage);
}

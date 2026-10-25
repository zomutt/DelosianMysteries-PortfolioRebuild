using UnityEngine;
using System.Collections;

/// <summary>
/// This serves as the blueprint for which each enemy will use and derive behaviour from.
/// </summary>
[RequireComponent(typeof(EnemyBase))]
public abstract class EnemyBase : MonoBehaviour
{
    public static EnemyBase Instance;
    [Header("Base Enemy Stats")]
    [SerializeField] protected int maxHealth;
    protected int currentHealth;
    [SerializeField] protected int damage;

    [Header("Combat Timers")]
    [SerializeField] protected float attackCD;

    protected float iframe = .25f;

    protected bool canAttack;
    protected bool canTakeDmg;

    [Header("Visuals")]
    // Most enemies will have a skull that rises and fades, but certain bosses and special enemies will have this differ.
    [SerializeField] protected GameObject deathSprite;
    // Hit feedback.
    private SpriteRenderer sr;
    private Color originalColor;
    [SerializeField] private float deathYOffset;

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
        canAttack = true;
        canTakeDmg = true;
        ResetState();
    }
    
    internal void OnTriggerEnter2D(Collider2D other)
    {
        // Player attacks enemy.
        if (other.CompareTag("Sword") && canTakeDmg)
        {
            // Prevents double hits.
            canTakeDmg = false;

            int damage = PlayerStats.Instance.PlayerDamage;
            TakeDamage(damage);

            Debug.Log($"{gameObject.name} has been hit. Dmg taken: {damage}, New hp: {currentHealth}");

            if (currentHealth > 0)
            {
                StartCoroutine(HitFlash());
                StartCoroutine(IFrameLogic());
            }
            return;
        }

        // Enemy attacks player.
        if (other.CompareTag("Player") && canAttack)
        {
            ApplyDamage();
            Debug.Log($"{gameObject.name} attacked player for {damage}. Player HP now: {PlayerStats.Instance.CurrentHealth}");
        }
    }

    public virtual void TakeDamage(int amount)
    {
        // Occurs on collision with player sword
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"{gameObject.name} has been hit for {amount} damage. New hp: {currentHealth}");
        if (currentHealth <= 0) 
            DisableEnemy();
    }

    protected virtual void DisableEnemy()
    {
        StopAllCoroutines();
        // Spawn death effect above enemy.
        Vector3 spawnPos = transform.position + new Vector3(0, deathYOffset, 0);
        Instantiate(deathSprite, spawnPos, Quaternion.identity);
        gameObject.SetActive(false);
    }

    // This resets movement, AI, animations, etc.
    protected virtual void ResetState() 
    {
        // This is called in OnEnable, but we all know that things can just happen, so I like to have failsafes.
        currentHealth = maxHealth;
        canTakeDmg = true;
        canAttack = true;
        
        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }

        originalColor = sr.color;
        sr.color = originalColor;
    }

    private IEnumerator IFrameLogic()
    {
        yield return new WaitForSeconds(iframe);
        canTakeDmg = true; 
    }

    protected virtual IEnumerator CooldownLogic()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }
    
    private IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sr.color = originalColor;
    }

    protected virtual void ApplyDamage()
    {
        PlayerStats.Instance.TakeDamage(damage);
        StartCoroutine(CooldownLogic());
    }
    // Handles unique movement logic (patrol, chase, wander, etc.)
    protected abstract void HandleMovement();
    // This handles unique behaviours (rules, visuals, timing, cds, etc.)
    protected abstract void AttackBehaviour();
}

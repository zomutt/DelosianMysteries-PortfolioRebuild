using System.Collections;
using UnityEngine;

/// <summary>
/// SnakeEnemy handles collision-based combat interactions for the snake-type enemy.
/// Damage, iframe logic, and death behavior are inherited from EnemyBase.
/// This class adds hit-flash feedback and a floating death sprite.
/// </summary>

public class SnakeEnemy : EnemyBase
{
    // Hit feedback.
    private SpriteRenderer sr;
    private Color originalColor;

    // Vertical offset for death sprite (floating skull).
    [SerializeField] protected float deathYOffset = 1f;

    protected override void OnEnable()
    {
        base.OnEnable();

        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }
    private void OnTriggerEnter2D(Collider2D other)
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
                StartCoroutine(IframeLogic());
            }
            return;
        }

        // Enemy attacks player.
        if (other.CompareTag("Player") && canAttack)
        {
            ApplyDamage(base.damage);
            Debug.Log($"{gameObject.name} attacked player for {base.damage}. Player HP now: {PlayerStats.Instance.CurrentHealth}");
        }
    }

    protected override void HandleMovement() { }

    protected override void AttackBehaviour() { }
    protected override void ApplyDamage(int damage)
    {
        PlayerStats.Instance.TakeDamage(damage);
        StartCoroutine(CooldownLogic());
    }

    internal override void ResetState()
    {
        base.ResetState();

        if (sr == null)
        {
            sr = GetComponent<SpriteRenderer>();
        }

        originalColor = sr.color;
        sr.color = originalColor;
    }

    internal override void DisableEnemy()
    {
        // Spawn death effect above enemy.
        Vector3 spawnPos = transform.position + new Vector3(0, deathYOffset, 0);
        Instantiate(deathSprite, spawnPos, Quaternion.identity);

        // Handles SetActive(false);
        base.DisableEnemy();
    }

    private IEnumerator IframeLogic()
    {
        yield return new WaitForSeconds(iframe);
        canTakeDmg = true;
    }

    private IEnumerator CooldownLogic()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }

    // Enemy turns red when they are hit for visual feedback.
    private IEnumerator HitFlash()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(.1f);
        sr.color = originalColor;
    }
}
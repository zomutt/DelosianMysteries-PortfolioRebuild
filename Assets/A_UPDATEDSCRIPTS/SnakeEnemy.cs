using System.Collections;
using UnityEngine;

public class SnakeEnemy : EnemyBase
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword") && canTakeDmg)
        {
            int damage = PlayerStats.Instance.PlayerDamage;
            TakeDamage(damage);

            Debug.Log($"{gameObject} has been hit. Dmg taken: {damage}, New hp: {currentHealth}");

            StartCoroutine(IframeLogic());
        }

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
    }

    internal override void DisableEnemy()
    {
        // Add snake-specific death logic
    }

    private IEnumerator IframeLogic()
    {
        canTakeDmg = false;
        yield return new WaitForSeconds(iframe);
        canTakeDmg = true;
    }

    private IEnumerator CooldownLogic()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }
}
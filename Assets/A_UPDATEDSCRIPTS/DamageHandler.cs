using System.Collections;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    /// handles all damage-over-time effects applied to the player
    /// this exists because PlayerStats is a pure C# class and cannot run coroutines or InvokeRepeating
    /// keeping DoT timing on the player ensures effects continue even if the enemy that applied them is destroyed

    private int dotDamage;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))            // testing cheat
        {
            Debug.Log("Cheat used, DOT applied");
            ApplyDOT(1, .5f, 3);         // damage, interval (how often it triggers), duration
        }
    }
    public void ApplyDOT(int damage, float interval, float duration)
    {
        dotDamage = damage;
        InvokeRepeating(nameof(ApplyTick), 0.1f, interval);
        Invoke(nameof(StopDOT), duration);
    }
    private void ApplyTick()
    {
        PlayerStats.Instance.TakeDamage(dotDamage);
        Debug.Log("DOT did damage, new HP: " + PlayerStats.Instance.CurrentHealth);
    }
    private void StopDOT()
    {
        CancelInvoke(nameof(ApplyTick));
    }
}
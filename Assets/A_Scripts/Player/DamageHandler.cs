using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    /// Handles all damage-over-time effects applied to the player.
    /// This exists because PlayerStats is a pure C# class and cannot run coroutines or InvokeRepeating
    /// Keeping DoT timing on the player ensures effects continue even if the enemy that applied them is destroyed

    private int dotDamage;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))            // testing cheat
        {
            Debug.Log("Cheat used, DOT applied");
            ApplyDOT(1, .5f, 3);         // Do x damage, every y second, for z amount of time.
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
        if (PlayerStats.Instance == null) return;
        PlayerStats.Instance.TakeDamage(dotDamage);
        Debug.Log($"DOT did damage, new HP: {PlayerStats.Instance.CurrentHealth}");
    }
    private void StopDOT()
    {
        CancelInvoke(nameof(ApplyTick));
    }
}
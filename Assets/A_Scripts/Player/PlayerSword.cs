using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            PlayerCombat.Instance.AttackEnemy(other.gameObject);
        }
    }
}

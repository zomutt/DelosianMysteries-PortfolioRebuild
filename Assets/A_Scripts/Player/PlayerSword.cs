using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Sword hit enemy");
            PlayerCombat.Instance.AttackEnemy(other.gameObject);
        }
    }
}

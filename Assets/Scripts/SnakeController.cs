using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    [SerializeField] private float snakeHP = 20f;
    [SerializeField] private float maxSnakeHP = 20f;
    [SerializeField] private float snakeDMG = 5f;

    private bool canAttack;
    private bool canTakeDamage;
    [SerializeField] private float atkCooldown = .5f;
    private GameObject sword;


    private void Awake()
    {
        snakeHP = maxSnakeHP;
        canTakeDamage = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Sword"))
        {
            Debug.Log("Enemy hit by sword!");
            if (canTakeDamage)
            {
                snakeHP -= PlayerStats.Instance.PlayerDamage;
                Debug.Log("Snake damaged for " + PlayerStats.Instance.PlayerDamage + ", New HP: " + snakeHP);
                StartCoroutine(iframe());
            }
            else return;
        }
    }
    private IEnumerator iframe()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(.2f);
        canTakeDamage = true;
    }
}

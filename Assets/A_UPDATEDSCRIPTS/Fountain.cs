using UnityEngine;
using TMPro;
using System.Collections;


/// <summary>
/// Every x seconds, the player can visit the fountain to be restored to full health.
/// This script handles the healing logic along with the countdown logic.
/// </summary>

public class Fountain : MonoBehaviour
{
    [Header("Healing")]
    [SerializeField] private float cooldownTime;

    [Header("Display")]
    [SerializeField] private TMP_Text countdownText;
    private float timer;
    private bool isOnCooldown;

    private void Start()
    {
        if (countdownText == null)
        {
            Debug.Log($"{gameObject.name} is missing child countdown text reference.");
            return;
        }
        else
        {
            countdownText.text = "";
        }
        timer = 0f;
        isOnCooldown = false;
    }
    private void Update()
    {
        // Updates the text that lives on top of the fountain.
        if (isOnCooldown)
        {
            timer -= Time.deltaTime;
            if (timer > 0)
            {
                countdownText.text = Mathf.Ceil(timer).ToString("0");
            }
            else
            {
                countdownText.text = "";
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOnCooldown)
        {
            Debug.Log("Player visited fountain.");
            // Fountains tells player to restore health to max HP.
            PlayerStats.Instance.HealPlayer(PlayerStats.Instance.MaxHealth);
            StartCoroutine(StartCooldown());
        }
    }
    private IEnumerator StartCooldown()
    {
        isOnCooldown = true;
        timer = cooldownTime;
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }
}

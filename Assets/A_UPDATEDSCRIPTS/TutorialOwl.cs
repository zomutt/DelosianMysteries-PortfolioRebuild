using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// There are several NPCs with dialogue (ex: the tutorial owls).
/// When the player walks up, the NPC "hands" them a scroll for the player to read.
/// When the player walks away, the scroll closes.
/// </summary>
public class TutorialOwl : MonoBehaviour
{
    [SerializeField] private GameObject scroll;

    private void Start()
    {
        if (scroll == null)
        {
            Debug.Log("Scroll not assigned in inspector.");
        }
        else scroll.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            scroll.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            scroll.SetActive(false);
        }
    }
}

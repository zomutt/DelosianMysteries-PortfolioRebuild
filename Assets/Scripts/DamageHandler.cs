using System.Collections;
using UnityEngine;

public class DamageHandler : MonoBehaviour
{
    /// handles all damage-over-time effects applied to the player
    /// this exists because PlayerStats is a pure C# class and cannot run coroutines or InvokeRepeating
    /// keeping DoT timing on the player ensures effects continue even if the enemy that applied them is destroyed

    private int dotDamage;
    private Color flashingColor;          // stored so InvokeRepeating can access it

    private SpriteRenderer sr;
    private Color originalColor;

    [SerializeField] private float colorDuration = 0.1f;     // tweakable if i don't like it
    [SerializeField] private GameObject owSprite;
    [SerializeField] private GameObject bleedSprite;
    [SerializeField] private GameObject activeSprite;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;        // baseline color

        owSprite = transform.Find("OwEffect").gameObject;                    // more damage based visuals can fit in here as i add them
        if (owSprite == null)
            Debug.LogError("OwEffect child not found!");
        owSprite.SetActive(false);       // makes sure no visuals are active

        bleedSprite = transform.Find("BleedEffect").gameObject;
        if (bleedSprite == null)
            Debug.LogError("BleedEffect child not found!");
        bleedSprite.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad1))            // testing cheat
        {
            Debug.Log("Cheat used, DOT applied");
            ApplyDOT(1, .5f, 3, Color.red);
        }
    }
    public void ApplyDOT(int damage, float interval, float duration, Color flashingColor)
    {
        dotDamage = damage;
        this.flashingColor = flashingColor;   // store the color for each tick

        InvokeRepeating(nameof(ApplyTick), 0.1f, interval);
        Invoke(nameof(StopDOT), duration);

        if (flashingColor == Color.red)           // bleed image,, player also flashes red to show he got hurt
        {
            bleedSprite.SetActive(true);
            activeSprite = bleedSprite;
        }
    }
    private void ApplyTick()
    {
        PlayerStats.Instance.TakeDamage(dotDamage);
        ChangeColor(flashingColor);
        Debug.Log("DOT did damage, new HP: " + PlayerStats.Instance.CurrentHealth);
    }
    private void StopDOT()
    {
        CancelInvoke(nameof(ApplyTick));
        activeSprite.SetActive(false);
    }
    private void ChangeColor(Color flashingColor)
    {
        StartCoroutine(ColorFlash(flashingColor));
    }
    private IEnumerator ColorFlash(Color flashingColor)
    {
        sr.color = flashingColor;
        yield return new WaitForSeconds(colorDuration);
        sr.color = originalColor;
    }
}
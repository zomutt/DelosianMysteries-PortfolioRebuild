using System;
using System.Collections;
using UnityEngine;
public class PlayerEffects : MonoBehaviour
{
    /// <summary>
    /// This script lives on the Player prefab and it serves the purpose of giving appropriate visual feedback and a way for the player to immediately identify what they are affected by.
    /// </summary>
    public static PlayerEffects Instance { get; private set; }

    private SpriteRenderer sr;
    private Color playerBaseColor;              // stored when FindPlayer() is called
    private bool isFlashing;

    internal enum DamageType
    {
        Fire,
        Poison,
        Bleed,
        Web,        // web is not technically a damage, but it still needs the visual feedback
        Basic,
    }

    // when harmed, the player has pop-ups flash over their head. assigned in inspector.
    [SerializeField] private GameObject fireEffect;             // flame
    [SerializeField] private GameObject poisonEffect;           // fangs
    [SerializeField] private GameObject bleedEffect;            // blood drop
    [SerializeField] private GameObject webEffect;              // spider web
    [SerializeField] private GameObject basicEffect;            // "ow!" popup

    // called from player combat script when the player attacks or blocks.
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private GameObject swordObject;
    [SerializeField] private GameObject swooshObject;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        playerBaseColor = sr.color;
        DisableAllVisuals();
        isFlashing = false;
    }
    private void DisableAllVisuals()              // prevents visuals from being seen upon start,, this allows the visuals to be cleanly found but *immediately* hidden. they should be hidden by default, but this is a failsafe.
    {         
        fireEffect.SetActive(false);
        poisonEffect.SetActive(false);
        bleedEffect.SetActive(false);
        webEffect.SetActive(false);
        basicEffect.SetActive(false);
        swordObject.SetActive(false);
        shieldObject.SetActive(false);
        swooshObject.SetActive(false);
    }


    // more or less a method to encapsulate,, this takes the damage type and duration, then feeds that to the private IEnumerator that handles actually activating it
    internal void GrantStatusEffect(DamageType effect, float duration)    
    { 
        GameObject effectToActivate = null;
        switch (effect)
        { 
            case DamageType.Fire:
                effectToActivate = fireEffect;
                break;
            case DamageType.Poison:
                effectToActivate = poisonEffect;
                break;
            case DamageType.Bleed:
                effectToActivate = bleedEffect;
                break;
            case DamageType.Web:
                effectToActivate = webEffect;
                break;
            case DamageType.Basic:
                effectToActivate = basicEffect;
                break;
        }
        if (effectToActivate != null)
        {
            StartCoroutine(EffectFlash(effectToActivate, duration));
        }
    }
    private IEnumerator EffectFlash(GameObject effectObject, float duration)
    {
        effectObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        effectObject.SetActive(false);
    }

    // called by damage script when the player is harmed, this is meant to be flashed quickly and repeatedly if undergoing repeated damage or a DOT
    internal IEnumerator FlashPlayerColor(Color flashColor)      
    {
        if (isFlashing) yield break;      // this prevents overlap and visual bugs
        sr.color = flashColor;
        isFlashing = true;
        yield return new WaitForSeconds(.1f);
        sr.color = playerBaseColor;
        isFlashing = false;
    }

    // this is meant to be tweakable by the combat script to fit whatever just feels right
    internal IEnumerator ShowSword(float duration)           // gets duration from PlayerCombat.cs
    {
        swordObject.SetActive(true);
        swooshObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        swordObject.SetActive(false);
        swooshObject.SetActive(false);
    }
    internal IEnumerator ShowShield(float duration)
    {
        shieldObject.SetActive(true);
        yield return new WaitForSeconds(duration);
        shieldObject.SetActive(false);
    }
}

using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public static PlayerCombat Instance;
    private static readonly int Swinging = Animator.StringToHash("Swinging");

    /// <summary>
    /// This script handles all things combat-related for the player sans holding the PlayerStats.
    /// Please see PlayerMovement.cs for movement and jumping mechanics, and PlayerStats.cs for player stats and health management.
    /// </summary>
    
    [Header("State")]
    private bool canSwing = true;
    private bool canBlock = true;
    private bool isSwinging = false;

    [Header("Components")]
    private PlayerEffects effects;
    private Animator animator;
    [SerializeField] private AnimationClip atkAnim;     // Used for length
    private GameObject sword;
    
    private void Awake()
    {
        effects = GetComponent<PlayerEffects>();
        if (!effects)
        {
            Debug.LogError("PlayerEffects component is missing from the Player prefab!");
        }

        if (!Instance) Instance = this;
        else Destroy(this);
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        sword = GetComponentInChildren<PlayerSword>().gameObject;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1) && canBlock)
        { 
            UseShield();
        }
    }

    private void OnAttack()
    {
        if (!canSwing || isSwinging) return;       // if you swang while swangin, u aint gonna swang        
        canSwing = false;
        canBlock = false;
        StartCoroutine(AttackSequence());
    }
    
    // This below was designed this way so that player cannot brute force their way through by holding down both buttons. 
    // You either swing, or you block. This way, the player has to make rapid meaningful choices in combat.
    private IEnumerator AttackSequence()
    {
        isSwinging = true;
        sword.SetActive(true);
        animator.SetBool(Swinging, true);
        
        yield return new WaitForSeconds(atkAnim.length);
        
        sword.SetActive(false);
        animator.SetBool(Swinging, false);
        
        isSwinging = false;
        canSwing = true;
        canBlock = true;
    }
    private void UseShield()
    {
        if (!canBlock) return;      

        canSwing = false;
        canBlock = false;
        PlayerMovement.Instance.canMove = false;
        StartCoroutine(BlockCooldown());
    }
    private IEnumerator BlockCooldown()
    {
        yield return new WaitForSeconds(.25f);
        canSwing = true;
        canBlock = true;
        PlayerMovement.Instance.canMove = true;
    }

    public void AttackEnemy(GameObject target)      // Called by PlayerSword.cs
    {
        EnemyBase enemyBase = target.GameObject().GetComponent<EnemyBase>();
        if (!enemyBase) Debug.LogWarning("wtf bruh how did we even get here");
        enemyBase.TakeDamage(PlayerStats.Instance.PlayerDamage);
    }
}

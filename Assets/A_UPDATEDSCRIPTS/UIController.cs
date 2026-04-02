using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    [SerializeField] private GameObject statsTextPanel;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI damageText;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        UpdateUI();
    }
    // Any time a player stat or other display change is needed, this is called by that script.
    public void UpdateUI()          
    {
        healthText.text = "Health: " + PlayerStats.Instance.CurrentHealth + "/" + PlayerStats.Instance.MaxHealth;
        healthText.text = $"Health: {PlayerStats.Instance.CurrentHealth}/{PlayerStats.Instance.MaxHealth}";
        damageText.text = "Damage: " + PlayerStats.Instance.PlayerDamage;
    }
}

using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
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
        statsTextPanel.SetActive(true);
        UpdateUI();
    }
    public void UpdateUI()           // called any time a stat changes
    {
        healthText.text = "Health: " + PlayerStats.Instance.CurrentHealth + "/" + PlayerStats.Instance.MaxHealth;
        damageText.text = "Damage: " + PlayerStats.Instance.PlayerDamage;
    }
}

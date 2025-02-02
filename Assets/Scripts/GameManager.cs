using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int coins;
    public float dmg;  // Default damage value
    public bool isGun1;
    public bool isGun2;
    public bool isGun3;

    public bool isGun4;

    public bool isShotgun;

    private TextMeshProUGUI damageText;

    public int gunPrice;
    public int healPrice;
    public int healthPrice;

    public int score;

    public float plrMoveSpeed;
    public float enemyMoveSpeed;

    public float fireDelay; // Time delay between shots (in seconds)
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Ensures GameManager persists across scenes
        }
        else
        {
            Debug.LogWarning("Multiple instances of GameManager detected. Destroying duplicate.");
            Destroy(gameObject);  // Prevents multiple instances of GameManager
        }
    }

    private void Start()
{
    // Log to confirm dmg value when the game starts
    Debug.Log("GameManager initialized with damage value: " + dmg);
    dmg = 20;

    isGun1 = true;
    isGun2 = false;

    gunPrice = 75;
    healPrice = 30;
    healthPrice = 135;
    plrMoveSpeed = 2f;

    if (isGun1 == true)
    {
        fireDelay = .75f;
    }

    GameObject damage = GameObject.FindGameObjectWithTag("DamageTracker");
    if (damage != null)
    {
        damageText = damage.GetComponent<TextMeshProUGUI>();
        if (damageText != null)
        {
            damageText.text = "Damage: " + dmg;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component is missing on DamageTracker object.");
        }
    }
    else
    {
        Debug.LogError("DamageTracker object not found in the scene.");
    }
}
}

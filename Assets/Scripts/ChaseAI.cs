using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DC.HealthSystem;
using TMPro;

public class ChaseAI : MonoBehaviour
{
    private GameObject player;

    public float moveSpeed;
    private float distance;

    private Health health;
    private Image healthBar;
    private Health plrHealth;

    private GameObject fxLive;
    [SerializeField] GameObject fx;

    public Rigidbody2D rb;
    public float dmgFromExplosion;
    public float dmg;

    public TMP_Text coinText;
    public int reward;
    public int coins;

    public bool isWeak;
    public bool isStrong;
    public bool isBoss;

    void Start()
{
    player = GameObject.FindGameObjectWithTag("Player");
    if (player == null)
    {
        Debug.LogError("No GameObject with the 'Player' tag found in the scene.");
        return;
    }

    plrHealth = player.GetComponent<Health>();
    if (plrHealth == null)
    {
        Debug.LogError("The 'Player' GameObject does not have a 'Health' component.");
        return;
    }

    health = GetComponent<Health>();
    coinText = GameObject.FindGameObjectWithTag("Coins").GetComponent<TMP_Text>();

    Transform canvas = transform.Find("Canvas");
    if (canvas != null)
    {
        Transform healthBarTransform = canvas.Find("HealthBar");
        if (healthBarTransform != null)
        {
            healthBar = healthBarTransform.GetComponent<Image>();
        }
        else
        {
            Debug.LogError("HealthBar not found under Canvas. Check the hierarchy.");
        }
    }
    else
    {
        Debug.LogError("Canvas not found. Check the hierarchy.");
    }
}


    void FixedUpdate()
    {
        if (player != null)
        {
            // Calculate direction and move enemy
            Vector2 direction = (player.transform.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);

            // Rotate enemy to face the player
            float angle = Mathf.Atan2(player.transform.position.y - transform.position.y, 
                                      player.transform.position.x - transform.position.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
        }
    }

    public void OnDeath()
    {
        GameManager.Instance.coins += reward;

        if (coinText != null)
        {
            coinText.text = "COINS: " + GameManager.Instance.coins;
        }
        else
        {
            Debug.LogError("coinText is not assigned.");
        }

        fxLive = Instantiate(fx, transform.position, transform.rotation);
        Destroy(fxLive, 0.27f);

        Debug.Log("Coins after death: " + GameManager.Instance.coins);
    }

    public void OnDamage()
    {
        if (healthBar != null && health != null)
        {
            healthBar.fillAmount = (float)health.HealthValue / health.MaxValue;
        }
        else
        {
            Debug.LogError("HealthBar or Health is not assigned.");
        }
    }

    void OnCollisionEnter2D(Collision2D _col)
    {
        if (_col.gameObject.CompareTag("DeathFX"))
        {
            health.Damage(dmgFromExplosion);
        }
        else if (_col.gameObject.CompareTag("Player"))
        {
            plrHealth.Damage(dmg);
        }
    }
}

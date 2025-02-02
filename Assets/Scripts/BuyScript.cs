using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro
using DC.HealthSystem;

public class BuyScript : MonoBehaviour
{
    public bool isHeal;
    public bool isDmg;
    public bool isHealth;

    public int price;
    public float healAmount;
    public float healthAmount;

    public float dmgMult;

    public Health health;
    public GameObject player;
    private RectTransform healthBarRect;
    private TextMeshProUGUI coinsText;
    private TextMeshProUGUI damageText;

    private TextMeshProUGUI buyHealth;
    private TextMeshProUGUI buyGun;
    private TextMeshProUGUI buyHeal;

    public GameObject Gun1;
    public GameObject Gun2;
    public GameObject Gun3;
    public GameObject Gun4;
    public GameObject Gun;

    private Transform plrTransform;

    public GameObject FirePoint;

    public int healthPrice;

    void Start()
    {
        // Find the player and get its Health component
        FirePoint = GameObject.Find("FirePoint");
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            health = player.GetComponent<Health>();
            plrTransform = player.GetComponent<Transform>();

            // Find the health bar under the player's Canvas
            Canvas playerCanvas = player.GetComponentInChildren<Canvas>();
            if (playerCanvas != null)
            {
                Transform healthBar = playerCanvas.transform.Find("HealthBar");
                if (healthBar != null)
                {
                    healthBarRect = healthBar.GetComponent<RectTransform>();
                }
                else
                {
                    Debug.LogError("HealthBar not found under player's Canvas.");
                }
            }
            else
            {
                Debug.LogError("Canvas not found under the Player GameObject.");
            }
        }
        else
        {
            Debug.LogError("Player not found. Ensure the Player GameObject is tagged correctly.");
        }

        // Find the TextMeshPro component tagged as "Coins"
        GameObject coinsTextObject = GameObject.FindGameObjectWithTag("Coins");
        if (coinsTextObject != null)
        {
            coinsText = coinsTextObject.GetComponent<TextMeshProUGUI>();
            UpdateCoinsText();
        }
        else
        {
            Debug.LogError("Coins text object not found. Ensure it is tagged correctly.");
        }

        GameObject damage = GameObject.FindGameObjectWithTag("DamageTracker");
        if (damage != null)
        {
            damageText = damage.GetComponent<TextMeshProUGUI>();
            UpdateDamageText();
        }

        GameObject buyHealthText = GameObject.FindGameObjectWithTag("buyHealth");
        if (buyHealthText != null)
        {
            buyHealth = buyHealthText.GetComponent<TextMeshProUGUI>();
            buyHealth.text = "Health +10: " + healthPrice;
        }
        else
        {
            Debug.LogError("Health text object not found. Ensure it is tagged correctly.");
        }

        GameObject buyHealText = GameObject.FindGameObjectWithTag("buyHeal");
        if (buyHealText != null)
        {
            buyHeal = buyHealText.GetComponent<TextMeshProUGUI>();
            buyHeal.text = "Heal 15: " + price;
        }
        else
        {
            Debug.LogError("Heal text object not found. Ensure it is tagged correctly.");
        }

        GameObject buyGunText = GameObject.FindGameObjectWithTag("buyGun");
        if (buyGunText != null)
        {
            buyGun = buyGunText.GetComponent<TextMeshProUGUI>();
            buyGun.text = "Upgrade Gun: " + price;
        }
        else
        {
            Debug.LogError("buyGun text object not found. Ensure it is tagged correctly.");
        }
    }

    public void onClick()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("Cannot perform purchase. GameManager.Instance is missing.");
            return;
        }

        if (GameManager.Instance.coins >= price)
        {
            if (isHeal && health != null)
            {
                health.Heal(healAmount);
                GameManager.Instance.coins -= price;
            }
            
            else if (isDmg)
            {
                HandleGunUpgrade();
            }

            UpdateCoinsText();
            UpdateDamageText();
        }
        else if (GameManager.Instance.coins >= healthPrice)
        {

            if (isHealth && health != null && health.MaxValue != 200)
            {
                health.MaxValue += healthAmount;
                health.Heal(healthAmount);
                GameManager.Instance.coins -= healthPrice;

                if (healthBarRect != null)
                {
                    healthBarRect.localScale += new Vector3(0.1f, 0.1f, 0);
                    plrTransform.localScale += new Vector3(.5f,.5f,0);
                    if (GameManager.Instance.plrMoveSpeed != .5f)
                    {
                        GameManager.Instance.plrMoveSpeed -= .2f;
                    }
                    
                }

                healthPrice = Mathf.RoundToInt(healthPrice + healthPrice * .5f);
                buyHealth.text = "Health +10: " +  healthPrice;
            }

        }
        else
        {
            Debug.Log("Not enough coins to purchase.");
            buyHeal.text = "Too Expensive";
            buyHealth.text = "Too Expensive";
            buyGun.text = "Too Expensive";
        }
    }

    private void UpdateCoinsText()
    {
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + GameManager.Instance.coins;
        }
        else
        {
            Debug.LogError("Coins text is not assigned or missing.");
        }
    }

    private void UpdateDamageText()
    {
        if (damageText != null)
        {
            damageText.text = "Damage: " + GameManager.Instance.dmg;
        }
        else
        {
            Debug.LogError("damage text is not assigned or missing.");
        }
    }

    public void HandleGunUpgrade()
    {
        if (GameManager.Instance.isGun1)
        {
            Gun = GameObject.FindGameObjectWithTag("Gun1");
            UpgradeGun(Gun, Gun2, 10, 0.15f);
            GameManager.Instance.isGun1 = false;
            GameManager.Instance.isGun2 = true;
        }
        else if (GameManager.Instance.isGun2)
        {
            Gun = GameObject.FindGameObjectWithTag("Gun2");
            UpgradeGun(Gun, Gun3, 100, 1f);
            GameManager.Instance.isGun2 = false;
            GameManager.Instance.isGun3 = true;
        }
        else if (GameManager.Instance.isGun3)
        {
            Gun = GameObject.FindGameObjectWithTag("Gun3");
            UpgradeGun(Gun, Gun4, 15, 0.5f);
            GameManager.Instance.isGun3 = false;
            GameManager.Instance.isGun4 = true;
            GameManager.Instance.isShotgun = true;
        }
        else if (GameManager.Instance.isGun4)
        {
            buyGun.text = "Fully Upgraded";
        }
    }

    private void UpgradeGun(GameObject currentGun, GameObject nextGun, int newDmg, float newFireDelay)
    {
        if (currentGun != null)
        {
            currentGun.SetActive(false);
        }

        InstantiateGun(nextGun);
        nextGun.SetActive(true);
        GameManager.Instance.dmg = newDmg;
        GameManager.Instance.fireDelay = newFireDelay;
        GameManager.Instance.coins -= price;
        price = Mathf.RoundToInt(price + price * .3f);
        buyGun.text = "Upgrade Gun: " + price;
    }

public void InstantiateGun(GameObject newGun)
{
    Collider2D gunCollider = newGun.GetComponent<Collider2D>();
    if (gunCollider != null)
    {
        // Calculate the top of the gun by adding half of its height to its position
        float gunTopY = gunCollider.bounds.size.y / 2 + newGun.transform.position.y;

        // Instantiate the new gun at the FirePoint's X position, with its top aligned with FirePoint
        Vector3 gunPosition = new Vector3(FirePoint.transform.position.x, FirePoint.transform.position.y, 0);

        // Instantiate the new gun with the player's rotation
        GameObject instantiatedGun = Instantiate(newGun, gunPosition, plrTransform.rotation, plrTransform);

        // Activate the new gun
        instantiatedGun.SetActive(true);
    }
    else
    {
        Debug.LogError("Gun doesn't have a Collider2D to calculate offset.");
    }
}



}

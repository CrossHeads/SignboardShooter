using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro
using DC.HealthSystem;
public class BuyHealth : MonoBehaviour
{
    public float healthAmount;

    private Health health;
    private GameObject player;
    private RectTransform healthBarRect;
    private TextMeshProUGUI coinsText;
    private TextMeshProUGUI buyHealth;

    private Transform plrTransform;
    
    // Start is called before the first frame update
    void Start()
    {
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

        GameObject buyHealthText = GameObject.FindGameObjectWithTag("buyHealth");
        if (buyHealthText != null)
        {
            buyHealth = buyHealthText.GetComponent<TextMeshProUGUI>();
            buyHealth.text = "Health +10: " + GameManager.Instance.healthPrice;
        }
        else
        {
            Debug.LogError("Health text object not found. Ensure it is tagged correctly.");
        }

    }

    // Update is called once per frame
    public void onClick()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("Cannot perform purchase. GameManager.Instance is missing.");
            return;
        }

        if (GameManager.Instance.coins >= GameManager.Instance.healthPrice)
        {
            health.MaxValue += healthAmount;
            health.Heal(healthAmount);
            GameManager.Instance.coins -= GameManager.Instance.healthPrice;
            UpdateCoinsText();
            

            if (healthBarRect != null)
            {
                healthBarRect.localScale += new Vector3(0.1f, 0.1f, 0);
                plrTransform.localScale += new Vector3(.5f,.5f,0);
                if (GameManager.Instance.plrMoveSpeed != .5f)
                {
                    GameManager.Instance.plrMoveSpeed -= .2f;
                }
                    
            }

                GameManager.Instance.healthPrice = Mathf.RoundToInt(GameManager.Instance.healthPrice + GameManager.Instance.healthPrice * .5f);
                buyHealth.text = "Health +10: " +  GameManager.Instance.healthPrice;
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
}

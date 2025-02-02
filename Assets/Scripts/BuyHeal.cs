using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro
using DC.HealthSystem;

    
public class BuyHeal : MonoBehaviour
{
    private Health health;
    private GameObject player;
    private RectTransform healthBarRect;
    private TextMeshProUGUI coinsText;
    private TextMeshProUGUI buyHeal;

    
    public float healAmount;

    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            health = player.GetComponent<Health>();
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

         GameObject buyHealText = GameObject.FindGameObjectWithTag("buyHeal");
        if (buyHealText != null)
        {
            buyHeal = buyHealText.GetComponent<TextMeshProUGUI>();
            buyHeal.text = "Heal 15: " + GameManager.Instance.healPrice;
        }
        else
        {
            Debug.LogError("Heal text object not found. Ensure it is tagged correctly.");
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

        if (GameManager.Instance.coins >= GameManager.Instance.healPrice)
        {
            health.Heal(healAmount);
            GameManager.Instance.coins -= GameManager.Instance.healPrice;
            UpdateCoinsText();
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

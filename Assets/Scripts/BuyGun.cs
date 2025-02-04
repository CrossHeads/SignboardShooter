using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // For TextMeshPro
using DC.HealthSystem;
public class BuyGun : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject FirePoint;
    private GameObject player;
    private Transform plrTransform;
    private GameObject Gun;
    public GameObject Gun2;
    public GameObject Gun3;
    public GameObject Gun4;

    private TextMeshProUGUI buyGun;
    private TextMeshProUGUI coinsText;
    private TextMeshProUGUI damageText;
    
    void Start()
    {
        FirePoint = GameObject.Find("FirePoint");
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            plrTransform = player.GetComponent<Transform>();
        }

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

        GameObject buyGunText = GameObject.FindGameObjectWithTag("buyGun");
        if (buyGunText != null)
        {
            buyGun = buyGunText.GetComponent<TextMeshProUGUI>();
            buyGun.text = "Upgrade Gun: " + GameManager.Instance.gunPrice;
        }
        else
        {
            Debug.LogError("buyGun text object not found. Ensure it is tagged correctly.");
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

        if (GameManager.Instance.coins >= GameManager.Instance.gunPrice)
        {
            HandleGunUpgrade();
            UpdateCoinsText();
            UpdateDamageText();
            buyGun.text = "Upgrade Gun: " + GameManager.Instance.gunPrice;
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
        GameManager.Instance.coins -= GameManager.Instance.gunPrice;
        GameManager.Instance.gunPrice = Mathf.RoundToInt(GameManager.Instance.gunPrice + GameManager.Instance.gunPrice * .3f);
        
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

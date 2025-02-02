using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DC.HealthSystem;
using UnityEngine.UI;
public class PlayerMovement : MonoBehaviour
{
    

    [SerializeField] Rigidbody2D rb;
    [SerializeField] GameObject player;

    [SerializeField] GameObject restartScreen;

    public Camera cam;
    Vector2 movement;
    Vector2 mousePos;

    private Health health;

    private Image healthBar;
    // Update is called once per frame

    private GameObject fxLive;
    [SerializeField] GameObject fx;

    void Start()
    {

        health = GetComponent<Health>();
        // Find the HealthBar in the Canvas
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
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
       
        if (Input.GetMouseButton(1))
        {
            
            Vector2 mover = mousePos - new Vector2(player.transform.position.x,player.transform.position.y);
            rb.MovePosition(rb.position +  mover * GameManager.Instance.plrMoveSpeed * Time.fixedDeltaTime);
        }

       
    }

    void FixedUpdate()
    {
      

      Vector2 lookDir = mousePos - rb.position;
      float angle = Mathf.Atan2(lookDir.y,lookDir.x) * Mathf.Rad2Deg - 90f;
      rb.rotation = angle;

       

      
    }

    void OnCollisionEnter2D(Collision2D _col)
    {

        print("collided");

    }

 public void OnDeath()
{
    // Find all enemies and spawners
    var enemies = GameObject.FindGameObjectsWithTag("Enemy");
    var spawners = GameObject.FindGameObjectsWithTag("Spawner");

    // Disable all enemies before destroying them
    foreach (var enemy in enemies)
    {
        // Disable AI or other scripts on the enemy to prevent interactions
        var enemyAI = enemy.GetComponent<ChaseAI>();
        if (enemyAI != null)
        {
            enemyAI.enabled = false;
        }

        // Destroy enemy with a delay
        Destroy(enemy, 0.2f);
    }

   

    fxLive = Instantiate(fx,transform.position,transform.rotation);
    Destroy(fxLive, 0.27f);

    Instantiate(restartScreen);
}   

    public void OnDamage()
    {

        // check if healthbar exists, if so decrease, else print a error.
        if (healthBar != null && health != null)
        {
            healthBar.fillAmount = (float)health.HealthValue / health.MaxValue;
        }
        else
        {
            Debug.LogError("HealthBar or Health is not assigned.");
        }
    }

    public void OnHeal()
    {

         // check if healthbar exists, if so decrease, else print a error.
        if (healthBar != null && health != null)
        {
            healthBar.fillAmount = (float)health.HealthValue / health.MaxValue;
        }
        else
        {
            Debug.LogError("HealthBar or Health is not assigned.");
        }

    }

   
}

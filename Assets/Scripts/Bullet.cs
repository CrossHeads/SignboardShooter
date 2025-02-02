using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DC.HealthSystem;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;

    private Health health;
    private GameObject enemy;
    private Rigidbody2D enemyRB;

    public float knockback;

    void OnTriggerEnter2D(Collider2D _col)
    {
        if (_col.gameObject.CompareTag("Player"))
        {
            print("player");
        }
        else if (_col.gameObject.CompareTag("Enemy") || _col.gameObject.CompareTag("Boss"))
        {
            enemy = _col.gameObject;
            enemyRB = enemy.GetComponent<Rigidbody2D>();

            // Spawn hit effect and destroy bullet
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);

            // Ensure GameManager.Instance is initialized and check dmg value
            if (GameManager.Instance != null)
            {
                Debug.Log("Damage value from GameManager: " + GameManager.Instance.dmg);

                // Access enemy's Health component
                health = enemy.GetComponent<Health>();
                if (health != null)
                {
                    health.Damage(GameManager.Instance.dmg);  // Apply damage from GameManager
                }
                else
                {
                    Debug.LogError("Enemy does not have a Health component.");
                }
            }
            else
            {
                Debug.LogError("GameManager.Instance is null.");
            }

            // Apply knockback to the enemy if Rigidbody2D exists
            if (enemyRB != null)
            {
                if (GameManager.Instance.isShotgun == true)
                {
                    Vector2 push = (enemy.transform.position - transform.position).normalized; // Using transform.position for accuracy
                    enemyRB.AddForce(push * 15, ForceMode2D.Impulse);

                }

                else
                {
                    Vector2 push = (enemy.transform.position - transform.position).normalized; // Using transform.position for accuracy
                    enemyRB.AddForce(push * knockback, ForceMode2D.Impulse);

                }
                
            }
            else
            {
                Debug.LogError("Enemy does not have a Rigidbody2D component.");
            }

            // Destroy hit effect after some time
            Destroy(effect, 0.22f);
        }
        else if (_col.gameObject.CompareTag("Walls"))
        {
            // If it hits something else
            GameObject effect = Instantiate(hitEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
            Destroy(effect, 0.22f);
        }
        
        else
        {

            print("float on my nigga");

        }
    }
}

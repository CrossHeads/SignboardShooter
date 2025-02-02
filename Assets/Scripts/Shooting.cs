using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    [SerializeField] Transform FirePoint;
    [SerializeField] GameObject bulletPrefab;

    public float bulletForce = 20f;
    public float shotgunSpreadAngle = 10f;  // Controls the spread of shotgun bullets
    public float bulletLifetime = 10f;  // Lifetime of shotgun bullets

    private float nextFireTime = 0f; // Tracks when the player can shoot again

    void Update()
    {
        // Check if the player is shooting and if enough time has passed between shots
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + GameManager.Instance.fireDelay; // Set the next allowed fire time
        }

        bulletLifetime = 10f;
    }

    void Shoot()
    {
        if (GameManager.Instance.isShotgun)  // Check if shotgun mode is active
        {
            // Fire 4 bullets with spread
            for (int i = -1; i <= 1; i++)  // Three bullets in total (-1, 0, 1) to simulate a spread
            {
                GameObject bullet = Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

                // Apply force to the bullet with some spread
                Vector2 direction = FirePoint.up + new Vector3(Random.Range(-shotgunSpreadAngle, shotgunSpreadAngle), 0f, 0f);
                rb.AddForce(direction.normalized * bulletForce, ForceMode2D.Impulse);

                // Destroy the bullet after a short time (shotgun bullets only)
                Destroy(bullet, bulletLifetime);
            }
        }
        else
        {
            // Regular single bullet shot (without lifetime)
            GameObject bullet = Instantiate(bulletPrefab, FirePoint.position, FirePoint.rotation);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(FirePoint.up * bulletForce, ForceMode2D.Impulse);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DC.HealthSystem;
public class DamageExplosion : MonoBehaviour
{
    private Health health;

    void OnTriggerEnter2D(Collider2D _col)
    {

        if (_col.gameObject.CompareTag("Enemy") || _col.gameObject.CompareTag("Boss"))
        {

            
            health = _col.gameObject.GetComponent<Health>();
            health.Damage(50);
    
        }

    }
}

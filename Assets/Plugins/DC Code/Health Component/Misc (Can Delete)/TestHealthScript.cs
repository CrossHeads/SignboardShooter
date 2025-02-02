using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DC.HealthSystem;
using UnityEngine.UI;

namespace DC.Test
{
    public class TestHealthScript : MonoBehaviour
    {
        [SerializeField] private Image healthBar;

        private Health health;


        private void Awake()
        {
            health = FindObjectOfType<Health>();
        }

        private void OnEnable()
        {
            health.OnHeal += OnHeal;
            health.OnDamage += OnDamage;
            health.OnDeath += OnDeath;
        }

        private void OnDisable()
        {
            health.OnHeal -= OnHeal;
            health.OnDamage -= OnDamage;
            health.OnDeath -= OnDeath;
        }

        public void DoHeal(float amount)
        {
            if(health == null)
            {
                Debug.LogError("Health Component not found", gameObject);
                return;
            }

            health.Heal(amount);
        }

        public void DoDamage(float amount)
        {
            if (health == null)
            {
                Debug.LogError("Health Component not found", gameObject);
                return;
            }

            health.Damage(amount);
        }

        private void OnHeal(float amount)
        {
            Debug.Log($"Get Healed by {amount}");

            healthBar.fillAmount = health.HealthValue / health.MaxValue;
        }

        private void OnDamage(float amount)
        {
            Debug.Log($"Get Damaged by {amount}");

            healthBar.fillAmount = health.HealthValue / health.MaxValue;
        }

        private void OnDeath(float amount)
        {
            Debug.Log($"Get Damaged by {amount} and Die");
        }








    }
}

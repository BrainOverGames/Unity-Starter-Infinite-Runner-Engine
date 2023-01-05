using System;
using UnityEngine;

namespace BOG
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        public Animator animator;
        public float healthAmount = 100f;
        float maxHealth;
        public static Action<float> OnHealthChangedEvent;

        protected void Awake()
        {
            maxHealth = healthAmount;
            animator.SetTrigger("idle");
        }

        protected virtual void OnTriggerEnter2D(Collider2D collidingObject)
        {
            PlayableCharacter player = collidingObject.GetComponent<PlayableCharacter>();
            if (player == null) { return; }
            gameObject.SetActive(false);
        }

        public virtual void TakeDamage(float damageAmount)
        {
            healthAmount -= damageAmount;
            float healthPct = healthAmount / maxHealth;
            OnHealthChangedEvent?.Invoke(healthPct);
            if (healthAmount <= 0)
            {
                Die();
            }
        }

        public virtual void Die()
        {
        }
    }
}

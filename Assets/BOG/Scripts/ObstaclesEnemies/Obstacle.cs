using UnityEngine;

namespace BOG
{
    public class Obstacle : MonoBehaviour, IDamageable
    {
        public bool isBreakable;
        public float healthAmount = 100;

        public virtual void TakeDamage(float damageAmount)
        {
            if(isBreakable)
                healthAmount -= damageAmount;
            if(healthAmount <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}

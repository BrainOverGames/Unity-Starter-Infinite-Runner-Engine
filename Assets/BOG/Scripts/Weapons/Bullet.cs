using UnityEngine;

namespace BOG
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 20f;
        public float damageAmount = 100f;
        public Rigidbody2D rb2D;
        public Vector3 localScale = new Vector3(1.5f, 1f, 1f);

        public void Launch(Vector3 direction)
        {
            transform.localScale = localScale;
            rb2D.velocity = direction * speed;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damageAmount);
            }
            Destroy(gameObject);
        }
    }
}

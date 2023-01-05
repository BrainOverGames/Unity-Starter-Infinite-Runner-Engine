using UnityEngine;

namespace BOG
{
    public class Boss : Enemy
    {
        public Weapon bossWeapon;

        [Header("Jump Params")]
        public float jumpStartTime = 3f;
        public float jumpRepeatRate = 3f;

        [Header("Attack Params")]
        public float attackStartTime = 1f;
        public float attackRepeatRate = 1f;

        void Start()
        {
            InvokeRepeating("Jump", jumpStartTime, jumpRepeatRate);
            InvokeRepeating("FireWeapon", attackStartTime, attackRepeatRate);
        }

        void Jump()
        {
            animator.SetTrigger("jump");
        }

        void FireWeapon()
        {
            bossWeapon.FireWeapon();
        }

        public override void Die()
        {
            animator.SetTrigger("die");
            GameManager.Instance.SetStatus(GameManager.GameStatus.GoalReached);
        }
    }
}

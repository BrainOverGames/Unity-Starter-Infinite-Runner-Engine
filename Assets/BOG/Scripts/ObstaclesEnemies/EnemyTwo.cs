namespace BOG
{
    public class EnemyTwo : Enemy
    {
        public override void Die()
        {
            gameObject.SetActive(false);
        }
    }
}

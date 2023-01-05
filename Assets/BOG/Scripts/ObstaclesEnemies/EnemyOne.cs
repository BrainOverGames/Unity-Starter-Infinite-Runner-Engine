namespace BOG
{
    public class EnemyOne : Enemy
    {
        public override void Die()
        {
            gameObject.SetActive(false);
        }
    }
}

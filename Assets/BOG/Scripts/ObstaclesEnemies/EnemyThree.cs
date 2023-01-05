namespace BOG
{
    public class EnemyThree : Enemy
    {
        public override void Die()
        {
            gameObject.SetActive(false);
        }
    }
}

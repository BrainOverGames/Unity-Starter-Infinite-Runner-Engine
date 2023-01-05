using UnityEngine;

namespace BOG
{
    public class BulletLauncher : MonoBehaviour, ILauncher
    {
        public Transform firePoint;
        public Bullet bulletPrefab;
        public Vector3 launchDirection;
        public bool nestedUnderThis = true;

        public void Launch(Weapon weapon)
        {
            var bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation, nestedUnderThis ? transform: null);
            bullet.Launch(launchDirection);
        }
    }
}

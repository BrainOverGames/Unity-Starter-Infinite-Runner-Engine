using UnityEngine;

namespace BOG
{
    public class Weapon : MonoBehaviour
    {
        public float fireRefreshRate = 1f;
        protected ILauncher launcher;
        float nextFireTime;

        protected void Awake()
        {
            launcher = GetComponent<ILauncher>();
        }

        protected bool CanFire()
        {
            return Time.time >= nextFireTime;
        }

#if UNITY_EDITOR
        private void Update()
        {
            if(Input.GetKey(KeyCode.Space) && transform.parent.tag == "Player")
                FireWeapon();
        }
#endif
        internal void FireWeapon()
        {
            if (CanFire())
            {
                nextFireTime = Time.time + fireRefreshRate;
                launcher.Launch(this);
            }
        }
    }
}

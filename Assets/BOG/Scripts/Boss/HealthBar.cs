using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace BOG
{
    public class HealthBar : MonoBehaviour
    {
        public Image foregroundImg;
        public float updateSpeedSeconds = 0.5f;

        private void Awake()
        {
            Enemy.OnHealthChangedEvent += OnHealthChanged;
        }

        private void OnDestroy()
        {
            Enemy.OnHealthChangedEvent -= OnHealthChanged;
        }

        private void OnHealthChanged(float healthAmount)
        {
            StartCoroutine(ChangeToAmount(healthAmount));
        }

        IEnumerator ChangeToAmount(float amount)
        {
            float preChangePercent = foregroundImg.fillAmount;
            float elapsed = 0f;
            while (elapsed < amount)
            {
                elapsed += Time.deltaTime;
                foregroundImg.fillAmount = Mathf.Lerp(preChangePercent, amount, elapsed/ updateSpeedSeconds);
                yield return null;
            }
        }
    }
}

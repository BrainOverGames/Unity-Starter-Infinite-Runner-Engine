using System.Collections;
using System;
using UnityEngine;

namespace BOG
{
    public class CountdownTimer : MonoBehaviour
    {
        public CountdownTimerUI countdownTimerUI;
        public float secondsLeft = 120f;
        readonly float secondsToDeduct = 1f;

        public static Action OnCountdownTimerFinishedEvent;

        private void Start()
        {
            countdownTimerUI.SetTimerUI(secondsLeft);
            StartCoroutine(CountdownTimerCo());
        }

        IEnumerator CountdownTimerCo()
        {
            while (secondsLeft > 0)
            {
                yield return new WaitForSeconds(1f);
                secondsLeft -= secondsToDeduct;
                if(GameManager.Instance.Status == GameManager.GameStatus.GameInProgress)
                    countdownTimerUI.SetTimerUI(secondsLeft);
            }
            OnCountdownTimerFinishedEvent?.Invoke();
        }
    }
}

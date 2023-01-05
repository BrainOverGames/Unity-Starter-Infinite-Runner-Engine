using System;
using UnityEngine;
using UnityEngine.UI;

namespace BOG
{
    public class CountdownTimerUI : MonoBehaviour
    {
        public Text timerTxt;

        internal void SetTimerUI(float secondsLeft)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(secondsLeft);
            string timerStr = string.Format("{0:D2}m:{1:D2}s", timeSpan.Minutes, timeSpan.Seconds);
            timerTxt.text = timerStr;
        }
    }
}

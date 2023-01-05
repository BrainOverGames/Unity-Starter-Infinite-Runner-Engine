using UnityEngine;
using UnityEngine.UI;

namespace BOG
{
    public class UIManager : MonoBehaviour
    {
        public CountdownTimer countdownTimer;
        public HealthBar bossHealthBar;
        public Image winLossPanel;
        public Text winLossTxt;
        public Button restartGameBtn;

        private void Awake()
        {
            CountdownTimer.OnCountdownTimerFinishedEvent += OnCountdownTimerFinished;
            GameManager.OnGameStatusUpdateEvent += OnGameStatusUpdated;
        }

        private void OnDestroy()
        {
            CountdownTimer.OnCountdownTimerFinishedEvent -= OnCountdownTimerFinished;
            GameManager.OnGameStatusUpdateEvent -= OnGameStatusUpdated;
            restartGameBtn.onClick.RemoveListener(OnRestartBtnClicked);
        }

        private void OnCountdownTimerFinished()
        {
            countdownTimer.gameObject.SetActive(false);
            bossHealthBar.gameObject.SetActive(true);
        }

        private void OnGameStatusUpdated(GameManager.GameStatus gameStatus)
        {
            winLossPanel.gameObject.SetActive(gameStatus == GameManager.GameStatus.GoalReached || gameStatus == GameManager.GameStatus.GameOver);
            if (winLossPanel.enabled)
            {
                winLossTxt.text = gameStatus == GameManager.GameStatus.GoalReached ? "YOU WIN !!!" : "YOU LOSS";
                restartGameBtn.onClick.AddListener(OnRestartBtnClicked);
            }
        }

        private void OnRestartBtnClicked()
        {
			LevelManager.Instance.GameOverAction();
        }
    }
}

using UnityEngine;
using System;

namespace BOG
{
	/// <summary>
	/// The game manager is a persistent singleton that handles time
	/// </summary>
	public class GameManager : BogSingleton<GameManager>
	{
		public int TotalLives = 3;
		public int CurrentLives { get; protected set; }
		public float TimeScale = 1;
		public enum GameStatus { BeforeGameStart, GameInProgress, Paused, GameOver, LifeLost, GoalReached };
		public GameStatus Status;
		public static Action<GameStatus> OnGameStatusUpdateEvent;
		// storage
		protected float savedTimeScale;
		protected GameStatus statusBeforePause;

		protected void Start()
        {
            Init();
        }

        private void Init()
        {
            Application.targetFrameRate = 300;
            CurrentLives = TotalLives;
            savedTimeScale = TimeScale;
            Time.timeScale = TimeScale;
        }

        public void SetStatus(GameStatus newStatus)
		{
			Status = newStatus;
			//Debug.Log("GAME STATUS  :: " + Status);
			OnGameStatusUpdateEvent?.Invoke(Status);
		}

		public void Reset()
		{
			TimeScale = 1f;
			SetStatus(GameStatus.GameInProgress);
		}

		public void SetLives(int lives)
		{
			CurrentLives = lives;
		}

		public void LoseLives(int lives)
		{
			CurrentLives -= lives;
		}

		public void SetTimeScale(float newTimeScale)
		{
			savedTimeScale = Time.timeScale;
			Time.timeScale = newTimeScale;
		}

		public void ResetTimeScale()
		{
			Time.timeScale = savedTimeScale;
		}

		public void Pause()
		{
			if (Time.timeScale > 0.0f)
			{
				Instance.SetTimeScale(0.0f);
				statusBeforePause = Instance.Status;
				SetStatus(GameStatus.Paused);
			}
			else
			{
				UnPause();
			}
		}

		public void UnPause()
		{
			Instance.ResetTimeScale();
			SetStatus(statusBeforePause);
		}
	}
}
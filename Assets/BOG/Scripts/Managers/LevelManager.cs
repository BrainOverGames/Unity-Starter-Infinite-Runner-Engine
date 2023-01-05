using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BOG
{
    public class LevelManager : BogSingleton<LevelManager>
    {
		[Header("CountdownTimer Params")]
		public GameObject bossPlatform;
		public Boss boss;
		public GameObject multipleEnemySpawner;
		public GameObject multiPlatformSpawner;
		public float bossInvokeTime = 1f;
		public float multiPlatformInvokeTime = 3f;


		public enum Controls { SingleButton, LeftRight }

        public float Speed { get; protected set; }
        public float DistanceTraveled { get; protected set; }

		[Header("Prefabs")]
		public GameObject StartingPosition;
		public List<PlayableCharacter> PlayableCharacters;
		public List<PlayableCharacter> CurrentPlayableCharacters { get; set; }
		public float DistanceBetweenCharacters = 1f;
		public float RunningTime { get; protected set; }
		public float PointsPerSecond = 20;

		[Space(10)]
		[Header("Level Bounds")]
		public Bounds RecycleBounds;

		[Space(10)]
		public Bounds DeathBounds;

		[Space(10)]
		[Header("Speed")]
		public float InitialSpeed = 10f;
		public float MaximumSpeed = 50f;
		public float SpeedAcceleration = 1f;

		[Space(10)]
		[Header("Start")]
		public int StartCountdown;
		public string StartText;

		[Space(10)]
		[Header("Mobile Controls")]
		public Controls ControlScheme;

		[Space(10)]
		[Header("Life Lost")]
		public GameObject LifeLostExplosion;

		// protected stuff
		protected bool temporarySpeedFactorActive;
		protected float temporarySpeedFactorRemainingTime;
		protected float temporarySavedSpeed;
		protected float temporarySpeedFactor;
		protected Bounds tmpRecycleBounds;


		protected void Start()
        {
            BeforeCountdownTimerFinished();

            Speed = InitialSpeed;
            DistanceTraveled = 0;

            InstantiateCharacters();

            // storage
            GameManager.Instance.SetStatus(GameManager.GameStatus.BeforeGameStart);

            PrepareStart();
            CountdownTimer.OnCountdownTimerFinishedEvent += OnCountdownTimerFinished;
        }

        private void BeforeCountdownTimerFinished()
        {
            bossPlatform.SetActive(false);
            boss.gameObject.SetActive(false);
            multipleEnemySpawner.SetActive(true);
            multiPlatformSpawner.SetActive(true);
        }

        private void OnDestroy()
        {
			CountdownTimer.OnCountdownTimerFinishedEvent -= OnCountdownTimerFinished;
		}

		private void OnCountdownTimerFinished()
		{
			bossPlatform.SetActive(true);
			Invoke("SetBossStatus", bossInvokeTime);
			//multipleEnemySpawner.SetActive(false);
			Invoke("SetMultiPlatformStatus", multiPlatformInvokeTime);
		}

		void SetBossStatus()
        {
			boss.gameObject.SetActive(true);
		}

		void SetMultiPlatformStatus()
        {
			multiPlatformSpawner.SetActive(false);
		}

		/// <summary>
		/// Instantiates all the playable characters and feeds them to the gameManager
		/// </summary>
		void InstantiateCharacters()
		{
			CurrentPlayableCharacters = new List<PlayableCharacter>();

			if (PlayableCharacters == null)
			{
				return;
			}

			if (PlayableCharacters.Count == 0)
			{
				return;
			}

			for (int i = 0; i < PlayableCharacters.Count; i++)
			{
				PlayableCharacter instance = (PlayableCharacter)Instantiate(PlayableCharacters[i]);
				instance.transform.position = new Vector3(StartingPosition.transform.position.x + i * DistanceBetweenCharacters, StartingPosition.transform.position.y, StartingPosition.transform.position.z);
				instance.SetInitialPosition(instance.transform.position);
				CurrentPlayableCharacters.Add(instance);
			}
		}

		/// <summary>
		/// Handles everything before the actual start of the game.
		/// </summary>
		void PrepareStart()
		{
			if (StartCountdown > 0)
			{
				GameManager.Instance.SetStatus(GameManager.GameStatus.BeforeGameStart);
				StartCoroutine(PrepareStartCountdown());
			}
			else
			{
				LevelStart();
			}
		}

		IEnumerator PrepareStartCountdown()
		{
			int countdown = StartCountdown;
			while (countdown > 0)
			{
				countdown--;
				yield return new WaitForSeconds(1f);
			}

			// when the countdown reaches 0, and if we have a start message, we display it
			if ((countdown == 0) && (StartText != ""))
			{
				yield return new WaitForSeconds(1f);
			}
			LevelStart();
		}

		void LevelStart()
		{
			GameManager.Instance.SetStatus(GameManager.GameStatus.GameInProgress);
		}

		void Update()
		{
			DistanceTraveled = DistanceTraveled + Speed * Time.fixedDeltaTime;

			if (Speed < MaximumSpeed)
			{
				Speed += SpeedAcceleration * Time.deltaTime;
			}

			HandleSpeedFactor();

			RunningTime += Time.deltaTime;
		}

		void HandleSpeedFactor()
		{
			if (temporarySpeedFactorActive)
			{
				if (temporarySpeedFactorRemainingTime <= 0)
				{
					temporarySpeedFactorActive = false;
					Speed = temporarySavedSpeed;
				}
				else
				{
					temporarySpeedFactorRemainingTime -= Time.deltaTime;
				}
			}
		}

		/// <summary>
		/// Temporarily multiplies the level speed by the provided factor
		/// </summary>
		/// <param name="factor">The number of times you want to increase/decrease the speed by.</param>
		/// <param name="duration">The duration of the speed change, in seconds.</param>
		public void TemporarilyMultiplySpeed(float factor, float duration)
		{
			//Debug.Log("BEFORE  " + Speed);
			temporarySpeedFactor = factor;
			temporarySpeedFactorRemainingTime = duration;

			if (!temporarySpeedFactorActive)
			{
				temporarySavedSpeed = Speed;
			}

			Speed = temporarySavedSpeed * temporarySpeedFactor;
			//Debug.Log("AFTER  " + Speed);
			temporarySpeedFactorActive = true;
		}

		public bool CheckRecycleCondition(Bounds objectBounds, float destroyDistance)
		{
			tmpRecycleBounds = RecycleBounds;
			tmpRecycleBounds.extents += Vector3.one * destroyDistance;

			if (objectBounds.Intersects(tmpRecycleBounds))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public bool CheckDeathCondition(Bounds objectBounds)
		{
			if (objectBounds.Intersects(DeathBounds))
			{
				return false;
			}
			else
			{
				return true;
			}
		}

		public void GameOverAction()
		{
			GameManager.Instance.UnPause();
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public void LifeLostAction()
		{
			ResetLevel();
		}

		void ResetLevel()
		{
			InstantiateCharacters();
			PrepareStart();
		}

		public void KillCharacter(PlayableCharacter player)
		{
			StartCoroutine(KillCharacterCo(player));
		}

		IEnumerator KillCharacterCo(PlayableCharacter player)
		{
			CurrentPlayableCharacters.Remove(player);
			player.Die();
			yield return new WaitForSeconds(0f);

			if (Instance.CurrentPlayableCharacters.Count == 0)
			{
				AllCharactersAreDead();
			}
		}

		protected virtual void AllCharactersAreDead()
		{
			if (LifeLostExplosion != null)
			{
				GameObject explosion = Instantiate(LifeLostExplosion);
				explosion.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, 0);
			}

			GameManager.Instance.SetStatus(GameManager.GameStatus.LifeLost);
			GameManager.Instance.LoseLives(1);

			if (GameManager.Instance.CurrentLives <= 0)
			{
				GameManager.Instance.SetStatus(GameManager.GameStatus.GameOver);
			}
		}
	}
}

namespace BOG
{
    public class InputManager : BogSingleton<InputManager>
    {
		/// LEFT BUTTON ----------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Triggered once when the left button is pressed down
		/// </summary>
		public void LeftButtonDown()
		{
			if (LevelManager.Instance.ControlScheme == LevelManager.Controls.LeftRight)
			{
				if (GameManager.Instance.Status == GameManager.GameStatus.GameOver)
				{
					return;
				}
				if (GameManager.Instance.Status == GameManager.GameStatus.LifeLost)
				{
					LevelManager.Instance.LifeLostAction();
					return;
				}
			}

			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
			{
				LevelManager.Instance.CurrentPlayableCharacters[i].LeftStart();
			}
		}

		/// <summary>
		/// Triggered once when the left button is released
		/// </summary>
		public void LeftButtonUp()
		{
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
			{
				LevelManager.Instance.CurrentPlayableCharacters[i].LeftEnd();
			}
		}

		/// <summary>
		/// Triggered while the left button is being pressed
		/// </summary>
		public void LeftButtonPressed()
		{
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
			{
				LevelManager.Instance.CurrentPlayableCharacters[i].LeftOngoing();
			}
		}


		/// RIGHT BUTTON ----------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Triggered once when the right button is pressed down
		/// </summary>
		public void RightButtonDown()
		{
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
			{
				LevelManager.Instance.CurrentPlayableCharacters[i].RightStart();
			}
		}

		/// <summary>
		/// Triggered once when the right button is released
		/// </summary>
		public void RightButtonUp()
		{
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
			{
				LevelManager.Instance.CurrentPlayableCharacters[i].RightEnd();
			}
		}

		/// <summary>
		/// Triggered while the right button is being pressed
		/// </summary>
		public void RightButtonPressed()
		{
			for (int i = 0; i < LevelManager.Instance.CurrentPlayableCharacters.Count; ++i)
			{
				LevelManager.Instance.CurrentPlayableCharacters[i].RightOngoing();
			}
		}
	}
}

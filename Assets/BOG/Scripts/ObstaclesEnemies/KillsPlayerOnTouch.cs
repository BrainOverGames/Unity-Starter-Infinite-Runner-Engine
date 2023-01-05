using UnityEngine;

namespace BOG
{
	/// <summary>
	/// Add this class to a trigger boxCollider and it'll kill all playable characters that collide with it.
	/// </summary>
	public class KillsPlayerOnTouch : MonoBehaviour
	{
		void OnTriggerEnter2D(Collider2D other)
		{
			TriggerEnter(other.gameObject);
		}

		void OnTriggerEnter(Collider other)
		{
			TriggerEnter(other.gameObject);
		}

		void TriggerEnter(GameObject collidingObject)
		{
            if (collidingObject.tag != "Player")
            {
                return;
            }

            PlayableCharacter player = collidingObject.GetComponent<PlayableCharacter>();
			if (player == null)
			{
				return;
			}

			if (player.Invincible)
			{
				return;
			}

			LevelManager.Instance.KillCharacter(player);
		}
	}
}

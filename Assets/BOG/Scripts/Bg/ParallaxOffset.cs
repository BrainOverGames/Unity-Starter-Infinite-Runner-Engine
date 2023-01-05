using UnityEngine;
using UnityEngine.UI;

namespace BOG
{
	/// <summary>
	/// Add this class to an object and it'll move in parallax based on the level's speed.
	/// </summary>
	public class ParallaxOffset : MonoBehaviour
	{
		public float Speed = 0;
		public static ParallaxOffset CurrentParallaxOffset;

		RawImage rawImage;
		Renderer renderer;
		Vector2 newOffset;

		float _position = 0;
		float yOffset;

		void Start()
		{
			CurrentParallaxOffset = this;
			if (GetComponent<Renderer>() != null)
			{
				renderer = GetComponent<Renderer>();
			}

			if (renderer == null && GetComponent<RawImage>() != null)
			{
				rawImage = GetComponent<RawImage>();
			}

		}

		void Update()
		{
			if ((rawImage == null) && (renderer == null))
			{
				return;
			}
			if (LevelManager.Instance != null)
			{
				_position += (Speed / 300) * LevelManager.Instance.Speed * Time.deltaTime;
			}
			else
			{
				_position += (Speed / 300) * Time.deltaTime;
			}


			if (_position > 1.0f)
			{
				_position -= 1.0f;
			}

			newOffset.x = _position;
			newOffset.y = yOffset;

			if (renderer != null)
			{
				renderer.material.mainTextureOffset = newOffset;
			}
			if (rawImage != null)
			{
				rawImage.material.mainTextureOffset = newOffset;
			}
		}
	}
}
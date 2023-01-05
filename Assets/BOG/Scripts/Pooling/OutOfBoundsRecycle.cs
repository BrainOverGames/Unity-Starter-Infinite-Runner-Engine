using UnityEngine;

namespace BOG
{
	/// <summary>
	/// Adds this component to an object and it'll be automatically recycled for further use when it reaches a certain distance after the level bounds
	/// </summary>
	public class OutOfBoundsRecycle : MonoBehaviour
	{
		public float DestroyDistanceBehindBounds = 5f;

		void Update()
		{
			if (LevelManager.Instance.CheckRecycleCondition(GetComponent<BogPoolableObject>().GetBounds(), DestroyDistanceBehindBounds))
			{
				GetComponent<BogPoolableObject>().Destroy();
			}
		}
	}
}
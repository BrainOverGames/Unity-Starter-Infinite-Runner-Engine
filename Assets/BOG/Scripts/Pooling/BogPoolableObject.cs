using UnityEngine;

namespace BOG
{
	/// <summary>
	/// Add this class to an object that you expect to pool from an objectPooler. 
	/// </summary>
	[AddComponentMenu("BOG/Tools/Object Pool/BogPoolableObject")]
	public class BogPoolableObject : BogObjectBounds
	{
		public delegate void Events();
		public event Events OnSpawnComplete;

		[Header("Poolable Object")]
		/// The life time, in seconds, of the object. If set to 0 it'll live forever, if set to any positive value it'll be set inactive after that time.
		public float LifeTime = 0f;

		public virtual void Destroy()
		{
			gameObject.SetActive(false);
		}

		protected virtual void Update()
		{

		}

		protected virtual void OnEnable()
		{
			Size = GetBounds().extents * 2;
			if (LifeTime > 0f)
			{
				Invoke("Destroy", LifeTime);
			}
		}

		protected virtual void OnDisable()
		{
			CancelInvoke();
		}

		public virtual void TriggerOnSpawnComplete()
		{
			OnSpawnComplete?.Invoke();
		}
	}
}

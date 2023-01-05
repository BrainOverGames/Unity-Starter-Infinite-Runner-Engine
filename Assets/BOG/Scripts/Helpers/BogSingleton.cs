using UnityEngine;

namespace BOG
{
	public class BogSingleton<T> : MonoBehaviour where T : Component
	{
		protected static T _instance;
		public static bool HasInstance => _instance != null;
		public static T Current => _instance;

		/// <summary>
		/// Singleton design pattern
		/// </summary>
		/// <value>The instance.</value>
		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = FindObjectOfType<T>();
				}
				return _instance;
			}
		}

		protected virtual void Awake()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			_instance = this as T;
		}
	}
}

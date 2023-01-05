using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace BOG
{
	/// <summary>
	/// A base class, meant to be extended depending on the use (simple, multiple object pooler), and used as an interface by the spawners.
	/// Still handles common stuff like singleton and initialization on start().
	/// </summary>
	public abstract class BogObjectPooler : MonoBehaviour
	{
		public static BogObjectPooler Instance;
		public bool MutualizeWaitingPools = false;
		public bool NestWaitingPool = true;
		public bool NestUnderThis = false;

		protected GameObject waitingPool = null;
		protected BogObjectPool objectPool;

		protected virtual void Awake()
		{
			Instance = this;
			FillObjectPool();
		}

		protected virtual void CreateWaitingPool()
		{
			if (!MutualizeWaitingPools)
			{
				waitingPool = new GameObject(DetermineObjectPoolName());
				SceneManager.MoveGameObjectToScene(waitingPool, this.gameObject.scene);
				objectPool = waitingPool.AddComponent<BogObjectPool>();
				objectPool.PooledGameObjects = new List<GameObject>();
				ApplyNesting();
				return;
			}
			else
			{
				GameObject waitingPool = GameObject.Find(DetermineObjectPoolName());
				if (waitingPool != null)
				{
					this.waitingPool = waitingPool;
                    objectPool = this.waitingPool.GetComponent<BogObjectPool>();
				}
				else
				{
					this.waitingPool = new GameObject(DetermineObjectPoolName());
                    SceneManager.MoveGameObjectToScene(this.waitingPool, this.gameObject.scene);
                    objectPool = this.waitingPool.AddComponent<BogObjectPool>();
					objectPool.PooledGameObjects = new List<GameObject>();
					ApplyNesting();
				}
			}
		}

		/// <summary>
		/// If needed, nests the waiting pool under this object
		/// </summary>
		protected virtual void ApplyNesting()
		{
			if (NestWaitingPool && NestUnderThis)
			{
				waitingPool.transform.SetParent(this.transform);
			}
		}

		/// <summary>
		/// Determines the name of the object pool.
		/// </summary>
		/// <returns>The object pool name.</returns>
		protected virtual string DetermineObjectPoolName()
		{
			return ("[ObjectPooler] " + this.name);
		}

		public virtual void FillObjectPool()
		{
			return;
		}

		public virtual GameObject GetPooledGameObject()
		{
			return null;
		}

		public virtual void DestroyObjectPool()
		{
			if (waitingPool != null)
			{
				Destroy(waitingPool.gameObject);
			}
		}
	}
}
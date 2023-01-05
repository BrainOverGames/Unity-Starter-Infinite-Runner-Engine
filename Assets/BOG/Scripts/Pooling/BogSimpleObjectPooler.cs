using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace BOG
{
	/// <summary>
	/// A simple object pool outputting a single type of objects
	/// </summary>
	[AddComponentMenu("BOG/Tools/Object Pool/BogSimpleObjectPooler")]
	public class BogSimpleObjectPooler : BogObjectPooler
	{
		public GameObject GameObjectToPool;
		public int PoolSize = 20;
		public bool PoolCanExpand = true;

		protected List<GameObject> pooledGameObjects;

		public override void FillObjectPool()
		{
			if (GameObjectToPool == null)
			{
				return;
			}

			CreateWaitingPool();

			pooledGameObjects = new List<GameObject>();

			int objectsToSpawn = PoolSize;

			if (objectPool != null)
			{
				objectsToSpawn -= objectPool.PooledGameObjects.Count;
				pooledGameObjects = new List<GameObject>(objectPool.PooledGameObjects);
			}

			for (int i = 0; i < objectsToSpawn; i++)
			{
				AddOneObjectToThePool();
			}
		}

		protected override string DetermineObjectPoolName()
		{
			return ("[SimpleObjectPooler] " + this.name);
		}

		public override GameObject GetPooledGameObject()
		{
			for (int i = 0; i < pooledGameObjects.Count; i++)
			{
				if (!pooledGameObjects[i].gameObject.activeInHierarchy)
				{
					return pooledGameObjects[i];
				}
			}
			if (PoolCanExpand)
			{
				return AddOneObjectToThePool();
			}
			return null;
		}

		protected GameObject AddOneObjectToThePool()
		{
			if (GameObjectToPool == null)
			{
				Debug.LogWarning("The " + gameObject.name + " ObjectPooler doesn't have any GameObjectToPool defined.", gameObject);
				return null;
			}
			GameObject newGameObject = (GameObject)Instantiate(GameObjectToPool);
			SceneManager.MoveGameObjectToScene(newGameObject, this.gameObject.scene);
			newGameObject.gameObject.SetActive(false);
			if (NestWaitingPool)
			{
				newGameObject.transform.SetParent(waitingPool.transform);
			}
			newGameObject.name = GameObjectToPool.name + "-" + pooledGameObjects.Count;

			pooledGameObjects.Add(newGameObject);

			objectPool.PooledGameObjects.Add(newGameObject);

			return newGameObject;
		}
	}
}
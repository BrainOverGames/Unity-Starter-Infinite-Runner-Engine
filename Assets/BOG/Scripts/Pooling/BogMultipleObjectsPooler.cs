using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BOG
{
    [AddComponentMenu("BOG/Tools/Object Pool/BogMultipleObjectsPooler")]
    public class BogMultipleObjectsPooler : BogObjectPooler
    {
		public List<GameObject> GameObjectsToPool;
		public int PoolSize = 20;
		public bool PoolCanExpand = true;

		protected List<GameObject> pooledGameObjects;

		public override void FillObjectPool()
		{
			if (GameObjectsToPool == null)
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
				AddMultipleObjectsToThePool();
			}
		}

		protected override string DetermineObjectPoolName()
		{
			return ("[MultipleObjectsPooler] " + this.name);
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
				return AddMultipleObjectsToThePool();
			}
			return null;
		}

		GameObject AddMultipleObjectsToThePool()
		{
			if (GameObjectsToPool == null)
			{
				Debug.LogWarning("The " + gameObject.name + " ObjectPooler doesn't have any GameObjectToPool defined.", gameObject);
				return null;
			}
			GameObject gameobjectToPool = RandomPoolObjectFromList();
			GameObject newGameObject = (GameObject)Instantiate(gameobjectToPool);
			SceneManager.MoveGameObjectToScene(newGameObject, this.gameObject.scene);
			newGameObject.gameObject.SetActive(false);
			if (NestWaitingPool)
			{
				newGameObject.transform.SetParent(waitingPool.transform);
			}
			newGameObject.name = gameobjectToPool.name + "-" + pooledGameObjects.Count;

			pooledGameObjects.Add(newGameObject);

			objectPool.PooledGameObjects.Add(newGameObject);

			return newGameObject;
		}

		GameObject RandomPoolObjectFromList()
        {
			int randomObjectIndex =  Random.Range(0, GameObjectsToPool.Count - 1);
			//Debug.Log(randomObjectIndex);
			return GameObjectsToPool[randomObjectIndex];
        }
	}
}

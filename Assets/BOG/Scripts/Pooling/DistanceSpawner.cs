using UnityEngine;
using System;

namespace BOG
{
	/// <summary>
	/// Spawns and positions/resizes objects based on the distance traveled 
	/// </summary>
	public class DistanceSpawner : Spawner
	{
		public enum GapOrigins { Spawner, LastSpawnedObject }

		[Header("Gap between objects")]
		public GapOrigins GapOrigin = GapOrigins.Spawner;
		public Vector3 MinimumGap = new Vector3(1, 1, 1);
		public Vector3 MaximumGap = new Vector3(1, 1, 1);
		[Space(10)]
		[Header("Y Clamp")]
		public float MinimumYClamp;
		public float MaximumYClamp;
		[Header("Z Clamp")]
		public float MinimumZClamp;
		public float MaximumZClamp;
		[Space(10)]
		[Header("Spawn angle")]
		public bool SpawnRotatedToDirection = true;

		protected Transform lastSpawnedTransform;
		protected float nextSpawnDistance;
		protected Vector3 gap = Vector3.zero;


		protected void Start()
		{
			objectPooler = GetComponent<BogObjectPooler>();
		}

		protected void Update()
		{
			CheckSpawn();
		}

		protected void CheckSpawn()
		{
			if (OnlySpawnWhileGameInProgress)
			{
				if ((GameManager.Instance.Status != GameManager.GameStatus.GameInProgress) && (GameManager.Instance.Status != GameManager.GameStatus.Paused))
				{
					lastSpawnedTransform = null;
					return;
				}
			}

			if (lastSpawnedTransform == null)
			{
				DistanceSpawn(transform.position + BogMaths.RandomVector3(MinimumGap, MaximumGap));
				return;
			}
			else
			{
				if (!lastSpawnedTransform.gameObject.activeInHierarchy)
				{
					DistanceSpawn(transform.position + BogMaths.RandomVector3(MinimumGap, MaximumGap));
					return;
				}
			}

			if (transform.InverseTransformPoint(lastSpawnedTransform.position).x < -nextSpawnDistance)
			{
				Vector3 spawnPosition = transform.position;
				DistanceSpawn(spawnPosition);
			}
		}

		protected void DistanceSpawn(Vector3 spawnPosition)
		{
			GameObject spawnedObject = Spawn(spawnPosition, false);

			if (spawnedObject == null)
			{
				lastSpawnedTransform = null;
				nextSpawnDistance = UnityEngine.Random.Range(MinimumGap.x, MaximumGap.x);
				return;
			}

			if (spawnedObject.GetComponent<BogPoolableObject>() == null)
			{
				throw new Exception(gameObject.name + " is trying to spawn objects that don't have a PoolableObject component.");
			}

			if (SpawnRotatedToDirection)
			{
				spawnedObject.transform.rotation *= transform.rotation;
			}
			if (spawnedObject.GetComponent<MovingObject>() != null)
			{
				spawnedObject.GetComponent<MovingObject>().SetDirection(transform.rotation * Vector3.left);
			}

			if (lastSpawnedTransform != null)
			{
				spawnedObject.transform.position = transform.position;

				float xDistanceToLastSpawnedObject = transform.InverseTransformPoint(lastSpawnedTransform.position).x;

				spawnedObject.transform.position += transform.rotation
													* Vector3.right
													* (xDistanceToLastSpawnedObject
													+ lastSpawnedTransform.GetComponent<BogPoolableObject>().Size.x / 2
													+ spawnedObject.GetComponent<BogPoolableObject>().Size.x / 2);

				if (GapOrigin == GapOrigins.Spawner)
				{
					spawnedObject.transform.position += (transform.rotation * ClampedPosition(BogMaths.RandomVector3(MinimumGap, MaximumGap)));
				}
				else
				{
					gap.x = UnityEngine.Random.Range(MinimumGap.x, MaximumGap.x);
					gap.y = spawnedObject.transform.InverseTransformPoint(lastSpawnedTransform.position).y + UnityEngine.Random.Range(MinimumGap.y, MaximumGap.y);
					gap.z = spawnedObject.transform.InverseTransformPoint(lastSpawnedTransform.position).z + UnityEngine.Random.Range(MinimumGap.z, MaximumGap.z);

					spawnedObject.transform.Translate(gap);

					spawnedObject.transform.position = (transform.rotation * ClampedPositionRelative(spawnedObject.transform.position, transform.position));
				}
			}
			else
			{
				spawnedObject.transform.position = transform.position;
				spawnedObject.transform.position += (transform.rotation * ClampedPosition(BogMaths.RandomVector3(MinimumGap, MaximumGap)));
			}

			if (spawnedObject.GetComponent<MovingObject>() != null)
			{
				spawnedObject.GetComponent<MovingObject>().Move();
			}

			spawnedObject.GetComponent<BogPoolableObject>().TriggerOnSpawnComplete();
			foreach (Transform child in spawnedObject.transform)
			{
				if (child.gameObject.GetComponent<ReactivateOnSpawn>() != null)
				{
					child.gameObject.GetComponent<ReactivateOnSpawn>().Reactivate();
				}
			}

			nextSpawnDistance = spawnedObject.GetComponent<BogPoolableObject>().Size.x / 2;
			lastSpawnedTransform = spawnedObject.transform;

		}

		protected Vector3 ClampedPosition(Vector3 vectorToClamp)
		{
			vectorToClamp.y = Mathf.Clamp(vectorToClamp.y, MinimumYClamp, MaximumYClamp);
			vectorToClamp.z = Mathf.Clamp(vectorToClamp.z, MinimumZClamp, MaximumZClamp);
			return vectorToClamp;
		}

		protected Vector3 ClampedPositionRelative(Vector3 vectorToClamp, Vector3 clampOrigin)
		{
			vectorToClamp.y = Mathf.Clamp(vectorToClamp.y, MinimumYClamp + clampOrigin.y, MaximumYClamp + clampOrigin.y);
			vectorToClamp.z = Mathf.Clamp(vectorToClamp.z, MinimumZClamp + clampOrigin.z, MaximumZClamp + clampOrigin.z);
			return vectorToClamp;
		}
	}
}
using UnityEngine;

namespace BOG
{
	/// <summary>
	/// You should extend this class for all your playable characters
	/// </summary>
	public class PlayableCharacter : MonoBehaviour
	{
		public bool UseDefaultMecanim = true;
		public bool ShouldResetPosition = true;
		public float ResetPositionSpeed = 0.5f;
		public float DistanceToTheGround { get; protected set; }
		public float GroundDistanceTolerance = 0.05f;
		public float InitialInvincibilityDuration = 3f;

		public bool Invincible => (Time.time - awakeAt < InitialInvincibilityDuration);

		protected Vector3 initialPosition;
		protected bool grounded;
		protected BogRigidbodyInterface rigidbodyInterface;
		protected float distanceToTheGroundRaycastLength = 50f;
		protected GameObject ground;
		protected LayerMask collisionMaskSave;
		protected float awakeAt;

		protected Vector3 raycastLeftOrigin;
		protected Vector3 raycastRightOrigin;

		protected virtual void Awake()
		{
			Initialize();
		}

		protected virtual void Start()
		{

		}

		protected virtual void Initialize()
		{
			rigidbodyInterface = GetComponent<BogRigidbodyInterface>();
			DistanceToTheGround = -1;
			awakeAt = Time.time;
			if (rigidbodyInterface == null)
			{
				return;
			}
		}

		public virtual void SetInitialPosition(Vector3 initialPosition)
		{
			this.initialPosition = initialPosition;
		}

		protected virtual void Update()
		{
			ResetPosition();

			CheckDeathConditions();

			ComputeDistanceToTheGround();
		}


		protected virtual void ComputeDistanceToTheGround()
		{
			if (rigidbodyInterface == null)
			{
				return;
			}

			DistanceToTheGround = -1;

			if (rigidbodyInterface.Is2D)
			{
				raycastLeftOrigin = rigidbodyInterface.ColliderBounds.min;
				raycastRightOrigin = rigidbodyInterface.ColliderBounds.min;
				raycastRightOrigin.x = rigidbodyInterface.ColliderBounds.max.x;

				RaycastHit2D raycastLeft = BogDebug.RayCast(raycastLeftOrigin, Vector2.down, distanceToTheGroundRaycastLength, 1 << LayerMask.NameToLayer("Ground"), Color.gray, true);
				if (raycastLeft)
				{
					DistanceToTheGround = raycastLeft.distance;
					ground = raycastLeft.collider.gameObject;
				}
				RaycastHit2D raycastRight = BogDebug.RayCast(raycastRightOrigin, Vector2.down, distanceToTheGroundRaycastLength, 1 << LayerMask.NameToLayer("Ground"), Color.gray, true);
				if (raycastRight)
				{
					if (raycastLeft)
					{
						if (raycastRight.distance < DistanceToTheGround)
						{
							DistanceToTheGround = raycastRight.distance;
							ground = raycastRight.collider.gameObject;
						}
					}
					else
					{
						DistanceToTheGround = raycastRight.distance;
						ground = raycastRight.collider.gameObject;
					}
				}

				if (!raycastLeft && !raycastRight)
				{
					DistanceToTheGround = -1;
					ground = null;
				}
				grounded = DetermineIfGroudedConditionsAreMet();
			}
		}

		protected virtual bool DetermineIfGroudedConditionsAreMet()
		{
			if (DistanceToTheGround == -1)
			{
				return (false);
			}
			if (DistanceToTheGround - GetPlayableCharacterBounds().extents.y < GroundDistanceTolerance)
			{
				return (true);
			}
			else
			{
				return (false);
			}
		}

		protected virtual void CheckDeathConditions()
		{
			if (LevelManager.Instance.CheckDeathCondition(GetPlayableCharacterBounds()))
			{
				LevelManager.Instance.KillCharacter(this);
			}
		}

		protected virtual Bounds GetPlayableCharacterBounds()
		{
			if (GetComponent<Collider>() != null)
			{
				return GetComponent<Collider>().bounds;
			}

			if (GetComponent<Collider2D>() != null)
			{
				return GetComponent<Collider2D>().bounds;
			}

			return GetComponent<Renderer>().bounds;
		}

		protected virtual void ResetPosition()
		{
			if (ShouldResetPosition)
			{
				if (grounded)
				{
					rigidbodyInterface.Velocity = new Vector3((initialPosition.x - transform.position.x) * (ResetPositionSpeed), rigidbodyInterface.Velocity.y, rigidbodyInterface.Velocity.z);
				}
			}
		}

		public virtual void Disable()
		{
			gameObject.SetActive(false);
		}

		public virtual void Die()
		{
			Destroy(gameObject);
		}

		public virtual void DisableCollisions()
		{
			rigidbodyInterface.EnableBoxCollider(false);
		}

		public virtual void EnableCollisions()
		{
			rigidbodyInterface.EnableBoxCollider(true);
		}

		public virtual void MainActionStart() { }
		public virtual void MainActionEnd() { }
		public virtual void MainActionOngoing() { }

		public virtual void LeftStart() { }
		public virtual void LeftEnd() { }
		public virtual void LeftOngoing() { }

		public virtual void RightStart() { }
		public virtual void RightEnd() { }
		public virtual void RightOngoing() { }

		protected virtual void OnCollisionEnter2D(Collision2D collidingObject)
		{
			CollisionEnter(collidingObject.collider.gameObject);
		}

		protected virtual void OnCollisionExit2D(Collision2D collidingObject)
		{
			CollisionExit(collidingObject.collider.gameObject);
		}

		protected virtual void OnCollisionEnter(Collision collidingObject)
		{
			CollisionEnter(collidingObject.collider.gameObject);
		}

		protected virtual void OnCollisionExit(Collision collidingObject)
		{
			CollisionExit(collidingObject.collider.gameObject);
		}

		protected virtual void OnTriggerEnter2D(Collider2D collidingObject)
		{
			TriggerEnter(collidingObject.gameObject);
		}
		protected virtual void OnTriggerExit2D(Collider2D collidingObject)
		{
			TriggerExit(collidingObject.gameObject);
		}
		protected virtual void OnTriggerEnter(Collider collidingObject)
		{
			TriggerEnter(collidingObject.gameObject);
		}
		protected virtual void OnTriggerExit(Collider collidingObject)
		{
			TriggerExit(collidingObject.gameObject);
		}

		protected virtual void CollisionEnter(GameObject collidingObject)
		{

		}

		protected virtual void CollisionExit(GameObject collidingObject)
		{

		}

		protected virtual void TriggerEnter(GameObject collidingObject)
		{

		}

		protected virtual void TriggerExit(GameObject collidingObject)
		{

		}
	}
}
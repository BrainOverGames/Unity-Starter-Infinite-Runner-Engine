using UnityEngine;

namespace BOG
{
	/// <summary>
	/// Add this class to an object and it'll move according to the level's speed. 
	/// </summary>
	public class MovingObject : MonoBehaviour
	{
		[Header("Movement")]
		public float Speed = 0;
		public float Acceleration = 0;
		public Vector3 Direction = Vector3.left;

		[Header("Behaviour")]
		public bool DirectionCanBeChangedBySpawner = true;
		public Space MovementSpace = Space.World;

		public Vector3 Movement { get { return movement; } }

		protected Vector3 movement;
		protected float initialSpeed;

		protected void Awake()
		{
			initialSpeed = Speed;
		}

		protected void OnEnable()
		{
			Speed = initialSpeed;
		}

		protected void Update()
		{
			Move();
		}

		public virtual void Move()
		{
			if (LevelManager.Instance == null)
			{
				movement = Direction * (Speed / 10) * Time.deltaTime;
			}
			else
			{
				movement = Direction * (Speed / 10) * LevelManager.Instance.Speed * Time.deltaTime;
			}
			transform.Translate(movement, MovementSpace);
			Speed += Acceleration * Time.deltaTime;
		}

		public void SetDirection(Vector3 newDirection)
		{
			if (DirectionCanBeChangedBySpawner)
			{
				Direction = newDirection;
			}
		}
	}
}
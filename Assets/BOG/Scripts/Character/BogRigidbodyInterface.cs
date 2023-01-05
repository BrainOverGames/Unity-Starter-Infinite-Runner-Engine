using UnityEngine;

namespace BOG
{ 
	[AddComponentMenu("BOG/Tools/Rigidbody Interface/BogRigidbodyInterface")]
	/// <summary>
	/// Acts as a bridge between rigidbody component & playable character
	/// </summary>
	public class BogRigidbodyInterface : MonoBehaviour
	{
		protected string mode;
		protected Rigidbody2D rigidbody2D;
		protected Collider2D collider2D;
		protected Bounds colliderBounds;


		public Vector3 position
		{
			get
			{
				if (rigidbody2D != null)
				{
					return rigidbody2D.position;
				}
				return Vector3.zero;
			}
			set { }
		}

		public Rigidbody2D InternalRigidBody2D
		{
			get
			{
				return rigidbody2D;
			}
		}

		public Vector3 Velocity
		{
			get
			{
				if (mode == "2D")
				{
					return (rigidbody2D.velocity);
				}
				return new Vector3(0, 0, 0);

			}
			set
			{
				if (mode == "2D")
				{
					rigidbody2D.velocity = value;
				}
			}
		}

		public Bounds ColliderBounds
		{
			get
			{
				if (rigidbody2D != null)
				{
					return collider2D.bounds;
				}
				return new Bounds();
			}
		}

		public bool isKinematic
		{
			get
			{
				if (mode == "2D")
				{
					return (rigidbody2D.isKinematic);
				}
				return false;
			}
		}

		void Awake()
		{
			rigidbody2D = GetComponent<Rigidbody2D>();

            if (rigidbody2D != null)
			{
				mode = "2D";
				collider2D = GetComponent<Collider2D>();
			}
			if (rigidbody2D == null)
			{
				Debug.LogWarning("A RigidBodyInterface has been added to " + gameObject + " but there's no Rigidbody2D on it.", gameObject);
			}
		}

		public void AddForce(Vector3 force)
		{
			if (mode == "2D")
			{
				rigidbody2D.AddForce(force, ForceMode2D.Impulse);
			}
		}

		public void IsKinematic(bool status)
		{
			if (mode == "2D")
			{
				rigidbody2D.isKinematic = status;
			}
		}

		public void EnableBoxCollider(bool status)
		{
			if (mode == "2D")
			{
				GetComponent<Collider2D>().enabled = status;
			}
		}

		public bool Is2D
		{
			get
			{
				if (mode == "2D")
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
	}
}

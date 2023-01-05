using System.Collections;
using UnityEngine;

namespace BOG
{
    public class JumperPlayableChar : PlayableCharacter
    {
		public float JumpForce = 20f;
		public float JumpReleaseSpeed = 50f;
		public float CooldownBetweenJumps = 0f;
		public bool JumpsAllowedWhenGroundedOnly;
		public bool JumpProportionalToPress = true;
		public float UngroundedDurationAfterJump = 0.2f;
		public float MinimalDelayBetweenJumps = 0.02f;
		public int NumberOfJumpsAllowed = 2;
		public int numberOfJumpsLeft;
		public Weapon weapon;

		protected bool jumping = false;
		protected float lastJumpTime;

		protected override void Start()
        {
            lastJumpTime = Time.time;
            numberOfJumpsLeft = NumberOfJumpsAllowed;
        }

		protected override void Update()
		{
			ComputeDistanceToTheGround();
			ResetPosition();
			CheckDeathConditions();

			if (grounded)
			{
				if ((Time.time - lastJumpTime > MinimalDelayBetweenJumps)
					&& (Time.time - lastJumpTime > UngroundedDurationAfterJump))
				{
					jumping = false;
					numberOfJumpsLeft = NumberOfJumpsAllowed;
				}
			}
		}

		//LEFT BTN DOWN
        public override void LeftStart()
        {
			Jump();
		}

		void Jump()
		{
			if (!EvaluateJumpConditions())
			{
				return;
			}

			PerformJump();
		}

		bool EvaluateJumpConditions()
		{
			if (JumpsAllowedWhenGroundedOnly && !grounded)
			{
				return false;
			}

			if (numberOfJumpsLeft <= 0)
			{
				return false;
			}

			if ((Time.time - lastJumpTime < CooldownBetweenJumps) && (numberOfJumpsLeft != NumberOfJumpsAllowed))
			{
				return false;
			}
			return true;
		}

		void PerformJump()
		{
			lastJumpTime = Time.time;
			numberOfJumpsLeft--;

			if (rigidbodyInterface.Velocity.y < 0)
			{
				rigidbodyInterface.Velocity = Vector3.zero;
			}

			ApplyJumpForce();

			lastJumpTime = Time.time;
			jumping = true;
		}

		void ApplyJumpForce()
		{
			rigidbodyInterface.AddForce(Vector3.up * JumpForce);
		}

        //LEFT BTN UP
        public override void LeftEnd()
        {
			if (JumpProportionalToPress)
			{
				StartCoroutine(JumpSlow());
			}
		}

		/// <summary>
		/// Slows the player's jump
		/// </summary>
		/// <returns>The slow.</returns>
		IEnumerator JumpSlow()
		{
			while (rigidbodyInterface.Velocity.y > 0)
			{
				Vector3 newGravity = Vector3.up * (rigidbodyInterface.Velocity.y - JumpReleaseSpeed * Time.deltaTime);
				rigidbodyInterface.Velocity = new Vector3(rigidbodyInterface.Velocity.x, newGravity.y, rigidbodyInterface.Velocity.z);
				yield return 0;
			}
		}

		//RIGHT BTN DOWN
        public override void RightStart()
        {
			weapon.FireWeapon();
		}

		protected override void TriggerEnter(GameObject collidingObject)
		{
			SpeedManipulateObstacle speedManipulateObstacle = collidingObject.GetComponent<SpeedManipulateObstacle>();
			if (speedManipulateObstacle != null)
			{
				speedManipulateObstacle.ManipulateCharSpeed();
			}
		}
	}
}

using System;
using UnityEngine;

namespace Player
{
	/// <summary>
	/// Script is based on script from youtuber Alpharoah on a specific video: https://www.youtube.com/watch?v=AVjbCn5i_rk
	/// Has been addapted to better suit my preferences for how it should function but contains the same core functionality.
	/// </summary>
	public class PlayerMovement : MonoBehaviour
	{
		[SerializeField] private PlayerManager playerManager;
		[SerializeField] private float moveSpeed = 7.0f; // Ground move speed
		[SerializeField] private float sprintAcceleration = 14f; // Ground accel

		[SerializeField] private float groundCheckDistance = 0.4f;
		[SerializeField] private LayerMask groundMask;

		#region Variables i do not know what they do

		private float wishSpeed2;

		private float wishSpeed;

		#endregion

		private PlayerController playerController;

		public float RunDeacceleration = 10f; // Deacceleration that occurs when running on the ground
		public float AirAcceleration = 2.0f; // Air accel
		public float AirDeacceleration = 2.0f; // Deacceleration experienced when opposite strafing
		public float AirControl = 0.3f; // How precise air control is

		public float
			SideStrafeAcceleration =
				50f; // How fast acceleration occurs to get up to sideStrafeSpeed when side strafing

		public float SideStrafeSpeed = 1f; // What the max speed to generate when side strafing
		[Range(0,4)]public float JumpHeight = 4.0f;
		private float jumpHeightMultiplier = 250f;
		public float Friction = 6f;
		private float playerTopVelocity = 0;
		public float PlayerFriction = 0f;
		float addspeed;
		float accelspeed;
		float currentspeed;
		float zspeed;
		float speed;
		float dot;
		float k;
		float accel;
		float newspeed;
		float control;
		float drop;

		public bool JumpQueue = false;
		public bool WishJump = false;

		public Vector3 MoveDirection;
		public Vector3 MoveDirectionNorm;
		private Vector3 playerVelocity;
		Vector3 wishdir;
		Vector3 vec;

		public Transform PlayerView;

		public float X;
		public float Z;

		public bool IsGrounded;

		public Transform Player;
		Vector3 udp;

		private void OnValidate()
		{
			if (playerManager == null)
				playerManager = GetComponent<PlayerManager>();
		}

		private void Start()
		{
			playerController = playerManager.PlayerController;
		}

		private void FixedUpdate()
		{
			// Move the controller
			playerManager.Rigidbody.AddForce(playerVelocity);
		}

		public void HandleMovement()
		{
			IsGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

			QueueJump();

			/* Movement, here's the important part */
			if (IsGrounded)
				GroundMove();
			else
				AirMove();

			// Calculate top velocity
			udp = playerVelocity;
			udp.y = 0;
			if (udp.magnitude > playerTopVelocity)
				playerTopVelocity = udp.magnitude;
		}

		private void SetMovementDir()
		{
			X = playerController.Forward;
			Z = playerController.Left;
		}

		//Queues the next jump
		private void QueueJump()
		{
			if (playerController.JumpInput && IsGrounded)
			{
				WishJump = true;
				Debug.Log("Jumping");
			}

			if (!IsGrounded && playerController.JumpInput)
			{
				JumpQueue = true;
			}

			if (IsGrounded && JumpQueue)
			{
				WishJump = true;
				JumpQueue = false;
			}
		}

		//Calculates wish acceleration
		public void Accelerate(Vector3 wishdir, float wishspeed, float accel)
		{
			currentspeed = Vector3.Dot(playerVelocity, wishdir);
			addspeed = wishspeed - currentspeed;
			if (addspeed <= 0)
				return;
			accelspeed = accel * Time.deltaTime * wishspeed;
			if (accelspeed > addspeed)
				accelspeed = addspeed;

			playerVelocity.x += accelspeed * wishdir.x;
			playerVelocity.z += accelspeed * wishdir.z;
		}

		//Execs when the player is in the air
		public void AirMove()
		{
			SetMovementDir();

			wishdir = new Vector3(playerController.Left, 0, playerController.Forward);
			wishdir = transform.TransformDirection(wishdir);

			wishSpeed = wishdir.magnitude;

			wishSpeed *= 7f;

			wishdir.Normalize();
			MoveDirectionNorm = wishdir;

			// Aircontrol
			wishSpeed2 = wishSpeed;
			if (Vector3.Dot(playerVelocity, wishdir) < 0)
				accel = AirDeacceleration;
			else
				accel = AirAcceleration;

			// If the player is ONLY strafing left or right
			if (playerController.Forward == 0 && playerController.Left != 0)
			{
				if (wishSpeed > SideStrafeSpeed)
					wishSpeed = SideStrafeSpeed;
				accel = SideStrafeAcceleration;
			}

			Accelerate(wishdir, wishSpeed, accel);

			AirControl(wishdir, wishSpeed2);

			// !Aircontrol

			/**
				* Air control occurs when the player is in the air, it allows
				* players to move side to side much faster rather than being
				* 'sluggish' when it comes to cornering.
				*/

			void AirControl(Vector3 wishdir, float wishspeed)
			{
				// Can't control movement if not moving forward or backward
				if (playerController.Forward == 0 || wishspeed == 0)
					return;

				zspeed = playerVelocity.y;
				playerVelocity.y = 0;
				/* Next two lines are equivalent to idTech's VectorNormalize() */
				speed = playerVelocity.magnitude;
				playerVelocity.Normalize();

				dot = Vector3.Dot(playerVelocity, wishdir);
				k = 32;
				k *= this.AirControl * dot * dot * Time.deltaTime;

				// Change direction while slowing down
				if (dot > 0)
				{
					playerVelocity.x = playerVelocity.x * speed + wishdir.x * k;
					playerVelocity.y = playerVelocity.y * speed + wishdir.y * k;
					playerVelocity.z = playerVelocity.z * speed + wishdir.z * k;

					playerVelocity.Normalize();
					MoveDirectionNorm = playerVelocity;
				}

				playerVelocity.x *= speed;
				playerVelocity.y = zspeed; // Note this line
				playerVelocity.z *= speed;

			}
		}

		/**
			* Called every frame when the engine detects that the player is on the ground
			*/
		public void GroundMove()
		{
			// Do not apply friction if the player is queueing up the next jump
			if (!WishJump)
				ApplyFriction(1.0f);
			else
				ApplyFriction(0);

			SetMovementDir();

			wishdir = new Vector3(playerController.Left, 0, playerController.Forward);
			wishdir = transform.TransformDirection(wishdir);
			wishdir.Normalize();
			MoveDirectionNorm = wishdir;

			wishSpeed = wishdir.magnitude;
			wishSpeed *= moveSpeed;

			Accelerate(wishdir, wishSpeed, sprintAcceleration);

			// Reset the gravity velocity
			playerVelocity.y = 0;

			if (WishJump)
			{
				playerVelocity.y = JumpHeight * jumpHeightMultiplier;
				WishJump = false;
			}

			/**
				* Applies friction to the player, called in both the air and on the ground
				*/
			void ApplyFriction(float t)
			{
				vec = playerVelocity; // Equivalent to: VectorCopy();
				vec.y = 0f;
				speed = vec.magnitude;
				drop = 0f;

				/* Only if the player is on the ground then apply friction */
				if (IsGrounded)
				{
					control = speed < RunDeacceleration ? RunDeacceleration : speed;
					drop = control * Friction * Time.deltaTime * t;
				}

				newspeed = speed - drop;
				PlayerFriction = newspeed;
				if (newspeed < 0)
					newspeed = 0;
				if (speed > 0)
					newspeed /= speed;

				playerVelocity.x *= newspeed;
				// playerVelocity.y *= newspeed;
				playerVelocity.z *= newspeed;
			}
		}
	}
}

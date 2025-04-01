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

		public float runDeacceleration = 10f; // Deacceleration that occurs when running on the ground
		public float airAcceleration = 2.0f; // Air accel
		public float airDeacceleration = 2.0f; // Deacceleration experienced when opposite strafing
		public float airControl = 0.3f; // How precise air control is

		public float
			sideStrafeAcceleration =
				50f; // How fast acceleration occurs to get up to sideStrafeSpeed when side strafing

		public float sideStrafeSpeed = 1f; // What the max speed to generate when side strafing
		public float jumpSpeed = 8.0f;
		public float friction = 6f;
		private float playerTopVelocity = 0;
		public float playerFriction = 0f;
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
		public bool wishJump = false;

		public Vector3 moveDirection;
		public Vector3 moveDirectionNorm;
		private Vector3 playerVelocity;
		Vector3 wishdir;
		Vector3 vec;

		public Transform playerView;

		public float x;
		public float z;

		public bool IsGrounded;

		public Transform player;
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
		
		public void HandleMovement()
		{
			IsGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

			QueueJump();

			/* Movement, here's the important part */
			if (IsGrounded)
				GroundMove();
			else
				AirMove();

			// Move the controller
			playerManager.Rigidbody.AddForce(playerVelocity);
			

			// Calculate top velocity
			udp = playerVelocity;
			udp.y = 0;
			if (udp.magnitude > playerTopVelocity)
				playerTopVelocity = udp.magnitude;
		}

		private void SetMovementDir()
		{
			x = playerController.Forward;
			z = playerController.Left;
		}

		//Queues the next jump
		private void QueueJump()
		{
			if (playerController.JumpInput && IsGrounded)
			{
				wishJump = true;
				Debug.Log("Jumping");
			}

			if (!IsGrounded && playerController.JumpInput)
			{
				JumpQueue = true;
			}

			if (IsGrounded && JumpQueue)
			{
				wishJump = true;
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

			wishdir = new Vector3(playerController.Forward, 0, playerController.Left);
			wishdir = transform.TransformDirection(wishdir);

			wishSpeed = wishdir.magnitude;

			wishSpeed *= 7f;

			wishdir.Normalize();
			moveDirectionNorm = wishdir;

			// Aircontrol
			wishSpeed2 = wishSpeed;
			if (Vector3.Dot(playerVelocity, wishdir) < 0)
				accel = airDeacceleration;
			else
				accel = airAcceleration;

			// If the player is ONLY strafing left or right
			if (playerController.Forward == 0 && playerController.Left != 0)
			{
				if (wishSpeed > sideStrafeSpeed)
					wishSpeed = sideStrafeSpeed;
				accel = sideStrafeAcceleration;
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
				k *= airControl * dot * dot * Time.deltaTime;

				// Change direction while slowing down
				if (dot > 0)
				{
					playerVelocity.x = playerVelocity.x * speed + wishdir.x * k;
					playerVelocity.y = playerVelocity.y * speed + wishdir.y * k;
					playerVelocity.z = playerVelocity.z * speed + wishdir.z * k;

					playerVelocity.Normalize();
					moveDirectionNorm = playerVelocity;
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
			if (!wishJump)
				ApplyFriction(1.0f);
			else
				ApplyFriction(0);

			SetMovementDir();

			wishdir = new Vector3(playerController.Forward, 0, playerController.Left);
			wishdir = transform.TransformDirection(wishdir);
			wishdir.Normalize();
			moveDirectionNorm = wishdir;

			wishSpeed = wishdir.magnitude;
			wishSpeed *= moveSpeed;

			Accelerate(wishdir, wishSpeed, sprintAcceleration);

			// Reset the gravity velocity
			playerVelocity.y = 0;

			if (wishJump)
			{
				playerVelocity.y = jumpSpeed;
				wishJump = false;
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
					control = speed < runDeacceleration ? runDeacceleration : speed;
					drop = control * friction * Time.deltaTime * t;
				}

				newspeed = speed - drop;
				playerFriction = newspeed;
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

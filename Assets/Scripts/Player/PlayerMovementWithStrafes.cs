using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementWithStrafes : MonoBehaviour
{
	public CharacterController Controller;
	public Transform GroundCheck;
	public LayerMask GroundMask;

	private float wishspeed2;
	private float gravity = -20f;
	float wishspeed;

	public float GroundDistance = 0.4f;
	public float MoveSpeed = 7.0f;  // Ground move speed
	public float RunAcceleration = 14f;   // Ground accel
	public float RunDeacceleration = 10f;   // Deacceleration that occurs when running on the ground
	public float AirAcceleration = 2.0f;  // Air accel
	public float AirDeacceleration = 2.0f;    // Deacceleration experienced when opposite strafing
	public float AirControl = 0.3f;  // How precise air control is
	public float SideStrafeAcceleration = 50f;   // How fast acceleration occurs to get up to sideStrafeSpeed when side strafing
	public float SideStrafeSpeed = 1f;    // What the max speed to generate when side strafing
	public float JumpSpeed = 8.0f;
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

    //UI
	private Vector3 lastPos;
	private Vector3 moved;
	public Vector3 PlayerVel;
	public float ModulasSpeed;
	public float ZVelocity;
	public float XVelocity;
	//End UI

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


    private void Start()
    {
        //This is for UI, feel free to remove the Start() function.
		lastPos = Player.position;
	}

    // Update is called once per frame
    void Update()
	{
		#region //UI, Feel free to remove the region.

		moved = Player.position - lastPos;
		lastPos = Player.position;
		PlayerVel = moved / Time.fixedDeltaTime;

		ZVelocity = Mathf.Abs(PlayerVel.z);
		XVelocity = Mathf.Abs(PlayerVel.x);


		ModulasSpeed = Mathf.Sqrt(PlayerVel.z * PlayerVel.z + PlayerVel.x * PlayerVel.x);

		#endregion

		IsGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

		QueueJump();

		/* Movement, here's the important part */
		if (Controller.isGrounded)
			GroundMove();
		else if (!Controller.isGrounded)
			AirMove();

		// Move the controller
		Controller.Move(playerVelocity * Time.deltaTime);

		// Calculate top velocity
		udp = playerVelocity;
		udp.y = 0;
		if (udp.magnitude > playerTopVelocity)
			playerTopVelocity = udp.magnitude;
	}
	public void SetMovementDir()
	{
		X = Input.GetAxis("Horizontal");
		Z = Input.GetAxis("Vertical");
	}

	//Queues the next jump
	void QueueJump()
	{
		if (Input.GetButtonDown("Jump") && IsGrounded)
		{
			WishJump = true;
		}

		if (!IsGrounded && Input.GetButtonDown("Jump"))
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

		wishdir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		wishdir = transform.TransformDirection(wishdir);

		wishspeed = wishdir.magnitude;

		wishspeed *= 7f;

		wishdir.Normalize();
		MoveDirectionNorm = wishdir;

		// Aircontrol
		wishspeed2 = wishspeed;
		if (Vector3.Dot(playerVelocity, wishdir) < 0)
			accel = AirDeacceleration;
		else
			accel = AirAcceleration;

		// If the player is ONLY strafing left or right
		if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") != 0)
		{
			if (wishspeed > SideStrafeSpeed)
				wishspeed = SideStrafeSpeed;
			accel = SideStrafeAcceleration;
		}

		Accelerate(wishdir, wishspeed, accel);

		AirControl(wishdir, wishspeed2);

		// !Aircontrol

		// Apply gravity
		playerVelocity.y += gravity * Time.deltaTime;

		/**
			* Air control occurs when the player is in the air, it allows
			* players to move side to side much faster rather than being
			* 'sluggish' when it comes to cornering.
			*/

		void AirControl(Vector3 wishdir, float wishspeed)
		{
			// Can't control movement if not moving forward or backward
			if (Input.GetAxis("Horizontal") == 0 || wishspeed == 0)
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

		wishdir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		wishdir = transform.TransformDirection(wishdir);
		wishdir.Normalize();
		MoveDirectionNorm = wishdir;

		wishspeed = wishdir.magnitude;
		wishspeed *= MoveSpeed;

		Accelerate(wishdir, wishspeed, RunAcceleration);

		// Reset the gravity velocity
		playerVelocity.y = 0;

		if (WishJump)
		{
			playerVelocity.y = JumpSpeed;
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
			if (Controller.isGrounded)
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

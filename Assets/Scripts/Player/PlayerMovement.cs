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
		[SerializeField] private float sprintMultiplier = 2f; // Ground accel

		[SerializeField] private float groundCheckDistance = 0.4f;
		[SerializeField] private LayerMask groundMask;
		[SerializeField, Range(0,1)] private float icyness = 0.5f;
		
		private PlayerController playerController;
		private Rigidbody playerRigidbody;
		
		private float desiredSpeed;
		
		[Range(0,4)]public float JumpHeight = 1f;
		private const float jumpHeightMultiplier = 5;

		public bool JumpQueue;
		public bool WishJump;
		
		private Vector3 desiredDirection;
		private Vector3 playerMovement;

		public bool IsGrounded;

		private void OnValidate()
		{
			if (playerManager == null)
				playerManager = GetComponent<PlayerManager>();
		}

		private void Start()
		{
			playerController = playerManager.PlayerController;
			playerRigidbody = playerManager.Rigidbody;
		}

		private void FixedUpdate()
		{
			playerRigidbody.AddForce(-playerRigidbody.velocity*icyness + playerMovement);
			
			if (WishJump)
			{
				playerRigidbody.AddForce(Vector3.up * (JumpHeight * jumpHeightMultiplier), ForceMode.Impulse);
				WishJump = false;
			}
		}

		public void HandleMovement()
		{
			IsGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
			QueueJump();
			
			Movement();
		}
		
		private void Movement()
		{
			desiredDirection = new Vector3(playerController.Left, 0, playerController.Forward);
			desiredDirection = transform.TransformDirection(desiredDirection);

			playerMovement = desiredDirection * moveSpeed;

			if (playerController.SprintInput)
			{
				playerMovement *= sprintMultiplier;
			}
		}
		
		//Queues the next jump
		private void QueueJump()
		{
			if (playerController.JumpInput && IsGrounded)
			{
				WishJump = true;
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
	}
}

using System;
using Environment;
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
        [SerializeField, Range(0, 1)] private float icyness = 0.5f;
        [SerializeField] private float securityBreachSpeed = 30;

        private PlayerController playerController;
        private Rigidbody playerRigidbody;

        private float desiredSpeed;

        [Range(0, 4)] public float JumpHeight = 1f;
        private const float jumpHeightMultiplier = 5;

        public bool JumpQueue;
        public bool WishJump;

        private Vector3 desiredDirection;
        private Vector3 playerMovement;

        public bool IsGrounded;

        public delegate void CanBreakBlockades();

        public static CanBreakBlockades HighSpeedAchieved;
        public static CanBreakBlockades ReturnedToLowSpeed;
        private bool isInHighSpeed;

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
            if (playerMovement.x != 0 && playerMovement.z != 0)
            {
                Vector3 velocityWithoutGravity = playerRigidbody.velocity;
                velocityWithoutGravity.y = 0;
                playerRigidbody.AddForce(-velocityWithoutGravity * icyness + playerMovement);
            }

            if (WishJump)
            {
                Vector3 velocityPreJump = playerRigidbody.velocity;
                velocityPreJump.y = 0;
                playerRigidbody.velocity = velocityPreJump;
                playerRigidbody.AddForce(Vector3.up * (JumpHeight * jumpHeightMultiplier), ForceMode.Impulse);
                WishJump = false;
            }

            if (playerRigidbody.velocity.magnitude > securityBreachSpeed && !isInHighSpeed)
            {
                HighSpeedAchieved?.Invoke();
                isInHighSpeed = true;
            }
            else if (playerRigidbody.velocity.magnitude < securityBreachSpeed && isInHighSpeed)
            {
                ReturnedToLowSpeed?.Invoke();
                isInHighSpeed = false;
            }
        }

        public void HandleMovement()
        {
            IsGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);
            QueueJump();

            Movement();

            CheckForMovingPlatform();
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
                playerController.JumpInput = false;
                WishJump = true;
            }

            if (!IsGrounded && playerController.JumpInput)
            {
                playerController.JumpInput = false;
                JumpQueue = true;
            }

            if (IsGrounded && JumpQueue)
            {
                WishJump = true;
                JumpQueue = false;
            }
        }

        private void CheckForMovingPlatform()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, groundCheckDistance, groundMask) &&
                hit.transform.CompareTag("MovingPlatform") && hit.transform.GetComponent<MovingObject>())
            {
                transform.SetParent(hit.transform, true);
                Debug.Log("Parenting");
            }
            else
            {
                if (transform.parent != null)
                {
                    transform.SetParent(null, true);
                }
            }
        }
        
        public float GetSecurityBreachSpeed() => securityBreachSpeed;
    }
}

using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerMovement), typeof(PlayerController))]
    [RequireComponent(typeof(PlayerLook)),
     RequireComponent(typeof(PlayerDash))]
    public class PlayerManager : MonoBehaviour
    {
        public PlayerController PlayerController;
        public PlayerMovement PlayerMovement;
        public PlayerLook PlayerLook;
        public PlayerDash PlayerDash;

        public Rigidbody Rigidbody;

        private void OnValidate()
        {
            if (PlayerController == null)
                PlayerController = GetComponent<PlayerController>();

            if (PlayerMovement == null)
                PlayerMovement = GetComponent<PlayerMovement>();

            if (PlayerLook == null)
                PlayerLook = GetComponent<PlayerLook>();

            if (PlayerDash == null)
                PlayerDash = GetComponent<PlayerDash>();

            if (Rigidbody == null)
                Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            PlayerController.TickInput();
            PlayerMovement.HandleMovement();
            if (PlayerDash.enabled)
                PlayerDash.HandleDash();
        }

        private void LateUpdate()
        {
            PlayerController.ResetInputs();
        }
    }
}

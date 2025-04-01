using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerMovement), typeof(PlayerController))]
    [RequireComponent(typeof(PlayerLook))]
    public class PlayerManager : MonoBehaviour
    {
        public PlayerController PlayerController;
        public PlayerMovement PlayerMovement;
        public PlayerLook PlayerLook;
        public Rigidbody Rigidbody;

        private void OnValidate()
        {
            if (PlayerController == null)
                PlayerController = GetComponent<PlayerController>();
            
            if (PlayerMovement == null)
                PlayerMovement = GetComponent<PlayerMovement>();

            if (PlayerLook == null)
                PlayerLook = GetComponent<PlayerLook>();
            
            if (Rigidbody == null)
                Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            PlayerController.TickInput();
            PlayerMovement.HandleMovement();
        }

        private void LateUpdate()
        {
            PlayerController.ResetInputs();
        }
    }
}

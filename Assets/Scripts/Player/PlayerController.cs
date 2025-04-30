using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerControllerInput inputActions;

        public float Left, Forward;
        private Vector2 movementInput;
        public bool SprintInput, JumpInput, DashInput;

        private void Awake()
        {
            if (inputActions == null)
            {
                //if no input actions set, create one
                inputActions = new PlayerControllerInput();

                inputActions.Player.Enable();

                //check for inputs with input actions
                CheckInputs();
            }
        }

        internal void TickInput()
        {
            //Movement inputs  
            HandleMovementInput();
        }

        private void CheckInputs()
        {
            //Movement
            inputActions.Player.Move.performed += movementInputActions =>
                movementInput = movementInputActions.ReadValue<Vector2>();
            //Sprint
            inputActions.Player.Sprint.performed += i => SprintInput = true;
            inputActions.Player.Sprint.canceled += i => SprintInput = false;
            //Dodge
            inputActions.Player.Dash.performed += i => DashInput = true;
            inputActions.Player.Dash.canceled += i => DashInput = false;
            //Jump
            inputActions.Player.Jump.performed += i => JumpInput = true;
            inputActions.Player.Jump.canceled += i => JumpInput = false;
        }

        public void ToggleInputs(bool enable)
        {
            if (enable)
                inputActions.Player.Enable();
            else
                inputActions.Player.Disable();
        }
        
        public void ResetAllInputs()
        {
            movementInput = Vector2.zero;
            SprintInput = false;
            JumpInput = false;
            DashInput = false;
            Left = 0;
            Forward = 0;
        }

        #region Movement

        private void HandleMovementInput()
        {
            Left = movementInput.x;
            Forward = movementInput.y;
        }

        #endregion
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private PlayerControllerInput inputActions;

        public float Left, Forward, MoveAmount;
        public Vector2 LookInput;
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

        //Reset the input bools so that they do not queue up for animations
        internal void ResetInputs()
        {
            DashInput = false;
        }

        internal void ResetMovementValues()
        {
            Forward = 0;
            Left = 0;
            MoveAmount = 0;
            movementInput = Vector2.zero;
            LookInput = Vector2.zero;
        }

        private void CheckInputs()
        {
            //Look
            inputActions.Player.Look.performed +=
                lookInputActions => LookInput = lookInputActions.ReadValue<Vector2>();
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

        #region Movement

        private void HandleMovementInput()
        {
            Left = movementInput.x;
            Forward = movementInput.y;
            MoveAmount = Mathf.Clamp01(Mathf.Abs(Left) + Mathf.Abs(Forward));
        }

        #endregion
    }
    
}

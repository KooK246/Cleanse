using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        #region Keybinds
        public bool shiftInput;
        public bool interact_Input;
        public bool R_light_Input;
        public bool R_heavy_Input;
        public bool jump_Input;
        public bool up_Input;
        public bool down_Input;
        public bool left_Input;
        public bool right_Input;
        Vector2 movementInput;
        Vector2 cameraInput;
        #endregion

        #region Flags
        public bool rollFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public float rollInputTimer;
        #endregion

        #region Imports
        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        #endregion

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleAttackInput(delta);
            HandleQuickSlotsInput();
            HandleJumpInput();
            HandleInteractableInput();
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }
    
        private void HandleRollInput(float delta)
        {
            shiftInput = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (shiftInput)
            {
                rollInputTimer += delta;
                sprintFlag = true;
            }
            else
            {
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
                    rollFlag = true;
                }

                rollInputTimer = 0;
            }
        }

        private void HandleAttackInput(float delta)
        {
            inputActions.PlayerActions.RLight.performed += i => R_light_Input = true;
            inputActions.PlayerActions.RHeavy.performed += i => R_heavy_Input = true;

            // Right hand's light attack
            if(R_light_Input)
            {
                if(playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else
                {
                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                } 
            }
            
            if(R_heavy_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        private void HandleJumpInput()
        {
            inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
        }

        private void HandleQuickSlotsInput()
        {
            inputActions.PlayerQuickSlots.Right.performed += i => right_Input = true;
            inputActions.PlayerQuickSlots.Left.performed += i => left_Input = true;

            if (right_Input)
            {
                playerInventory.ChangeRightWeapon();
            }
            else if (left_Input)
            {
                playerInventory.ChangeLeftWeapon();
            }
        }

        private void HandleInteractableInput()
        {
            inputActions.PlayerActions.Interact.performed += i => interact_Input = true;
        }
    }
}
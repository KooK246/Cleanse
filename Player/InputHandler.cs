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
        public bool shift_Input;
        public bool interact_Input;
        public bool dual_Wield_Input;
        public bool R_light_Input;
        public bool R_heavy_Input;
        public bool block_Input;
        public bool jump_Input;
        public bool up_Input;
        public bool down_Input;
        public bool left_Input;
        public bool right_Input;
        public bool lock_On_Input;
        Vector2 movementInput;
        Vector2 cameraInput;
        #endregion

        #region Flags
        public bool rollFlag;
        public bool twoHandFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool lockOnFlag;
        public float rollInputTimer;
        #endregion

        #region Imports
        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        PlayerManager playerManager;
        CameraHandler cameraHandler;
        WeaponSlotManager weaponSlotManager;
        AnimatorHandler animatorHandler;
        #endregion

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.DualWield.performed += i => dual_Wield_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lock_On_Input = true;
                inputActions.PlayerActions.Block.performed += i => block_Input = true;
                inputActions.PlayerActions.Block.canceled += i => block_Input = false;
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
            HandleCombatInput(delta);
            HandleJumpInput();
            HandleQuickSlotsInput();
            HandleLockOnInput();
            HandleInteractableInput();
            HandleTwoHandInput(); 
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
            shift_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;

            if (shift_Input)
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

        private void HandleCombatInput(float delta)
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
                    if (playerManager.isInteracting)
                        return;
                    
                    if (playerManager.canDoCombo)
                        return;
                    
                    animatorHandler.anim.SetBool("isUsingRightHand", true);
                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                } 
            }
            
            if(R_heavy_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }

            if(block_Input)
            {
                playerAttacker.HandleQAction();
            }
            else
            {
                playerManager.isBlocking = false;
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

        private void HandleLockOnInput()
        {
            if (lock_On_Input && lockOnFlag == false)
            {   
                lock_On_Input = false;
                cameraHandler.HandleLockOn();
                if (cameraHandler.currentLockOnTarget == null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                }
            }
            else if (lock_On_Input && lockOnFlag == true)
            {
                lock_On_Input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }
        }

        private void HandleInteractableInput()
        {
            inputActions.PlayerActions.Interact.performed += i => interact_Input = true;
        }

        private void HandleTwoHandInput()
        {
            if (dual_Wield_Input)
            {
                dual_Wield_Input = false;
                twoHandFlag = !twoHandFlag;

                if (twoHandFlag)
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }
                else
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                }
            }
        }
    }
}
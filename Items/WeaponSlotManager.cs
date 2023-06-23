using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class WeaponSlotManager : MonoBehaviour
    {
        PlayerManager playerManager;
        QuickSlotsUI quickSlotsUI;
        Animator animator;
        InputHandler inputHandler;

        public WeaponItem attackingWeapon;

        WeaponHolderSlot leftHandSlot;
        WeaponHolderSlot rightHandSlot;

        DamageCollider leftHandDamageCollider;
        DamageCollider rightHandDamageCollider;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerManager = GetComponentInParent<PlayerManager>();
            quickSlotsUI = FindObjectOfType<QuickSlotsUI>();
            inputHandler = GetComponentInParent<InputHandler>();

            WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();
            foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
            {
                if(weaponSlot.isLeftHandSlot)
                {
                    leftHandSlot = weaponSlot;
                }
                else if(weaponSlot.isRightHandSlot)
                {
                    rightHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if(isLeft)
            {
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, true);

                #region Handle Left Weapon Idle
                if(weaponItem != null)
                {
                    animator.CrossFade(weaponItem.left_Hand_Idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }
                #endregion
            }
            else
            {
                if (inputHandler.twoHandFlag)
                {
                    // Move current left hand weapon to assigned slot
                    animator.CrossFade(weaponItem.th_Idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Both Arms Empty", 0.2f);
                    
                    if(weaponItem != null)
                    {
                        animator.CrossFade(weaponItem.right_Hand_Idle, 0.2f);
                    }
                    else
                    {
                        animator.CrossFade("Right Arm Empty", 0.2f);
                    }
                }

                rightHandSlot.LoadWeaponModel(weaponItem);
                LoadRightWeaponDamageCollider();
                quickSlotsUI.UpdateWeaponQuickSlotsUI(weaponItem, false);
            }
        }

        #region Handle Weapon Damage Collider
        
        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        }

        public void OpenDamageCollider()
        {
            if (playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if (playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            } 
        }

        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
            leftHandDamageCollider.DisableDamageCollider();
        }
        #endregion
    }
}


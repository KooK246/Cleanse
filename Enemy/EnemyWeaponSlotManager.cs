/* using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        WeaponHolderSlot rightHandSlot;
        WeaponHolderSlot leftHandSlot;

        DamagerCollider leftHandDamageCollider;
        DamagerCollider rightHandDamageCollider;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            playerManager = GetComponentInParent<PlayerManager>();

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

        public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weapon;
                leftHandSlot.LoadWeaponModel(weapon);
                LoadWeaponsDamageCollider(true);
            }
            else
            {
                rightHandSlot.currentWeapon = weapon;
                rightHandDamageCollider.LoadWeaponModel = weapon;
                LoadWeaponsDamageCollider(false);
            }
        }
        public void LoadWeaponsDamageCollider(bool isLeft)
        {
            if(isLeft)
            {
                leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamagerCollider>();
            }
            else
            {
                rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamagerCollider>();
            }
        }

        public void OpenDamageCollider()
        {
            rightHandDamageCollider.EnableDamageCollider();
        }

        public void ClsoeDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
        }
    }
}*/

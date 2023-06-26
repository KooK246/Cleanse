using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class EnemyStats : CharacterStats
    {
        Animator animator;
        InputHandler inputHandler;
        CameraHandler cameraHandler;

        public GameObject enemy;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            inputHandler = FindObjectOfType<InputHandler>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }
        
        public void TakeDamage(int damage)
        {
            if (isDead)
                return;
            
            currentHealth = currentHealth - damage;

            animator.Play("Damage_01");

            if(currentHealth <= 0)
            {
                // DEATH
                currentHealth = 0;
                animator.Play("Dead_01");
                isDead = true;
                Destroy(enemy, 1.2f);
                if (inputHandler.lockOnFlag != false)
                {
                    cameraHandler.ClearLockOnTargets();
                }
            }
        }
    }    
}

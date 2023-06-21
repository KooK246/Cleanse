using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class EnemyStats : CharacterStats
    {
        Animator animator;

        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
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
            currentHealth = currentHealth - damage;

            animator.Play("Damage_01");

            if(currentHealth <= 0)
            {
                // DEATH
                currentHealth = 0;
                animator.Play("Dead_01");
            }
        }
    }    
}

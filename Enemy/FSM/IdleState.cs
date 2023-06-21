using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OK
{
    public class IdleState : State
    {
        public PursueTargetState pursueTargetState;
        public LayerMask detectionLayer;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimatorManager enemyAnimatorManager)
        {
            #region Handle Enemy Target Detection
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();
                
                if (characterStats == null)
                    characterStats = colliders[i].transform.GetComponentInChildren<CharacterStats>();
                if (characterStats == null)
                    characterStats = colliders[i].transform.GetComponentInParent<CharacterStats>();

                if (characterStats != null)
                {
                    //Check for team ID
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    enemyManager.viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

                    if(enemyManager.viewableAngle > enemyManager.minimumDetectionAngle && enemyManager.viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                    }
                }
            }
            #endregion

            #region Handle Switching To Next State
            if(enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
            #endregion
        }
    }
}

using UnityEngine;

namespace stateproperty.fpshooter{


    public class EnemyAttack : MonoBehaviour{

        float timer;

        bool gameOver = false;
        private float attackDistance = 5;
        private int playerAttackDamage = 20;
        public float timeBetweenAttacks = 0.5f;
        
        Animator enemyAnim;
        NavMeshAgent navAgent;
        EnemyHealth enemyHealth;
        Transform enemyTransform;
        Vector3 enemyOldPosition;
        Transform _playerTransform;


        private float stuckTimer;
        private float stuckCheckTime = 3f;

        void OnEnable( ){
            GameManager.onGameStoppedEvent += HandleGameStoppedEvent;
            GameManager.onPlayerMovementEvent += HandlePlayerMovementEvent;
        }


        void Start( ){

            enemyAnim = GetComponent<Animator>();
            enemyHealth = GetComponent<EnemyHealth>();
            enemyTransform = transform;
            enemyOldPosition = enemyTransform.position;
            navAgent = GetComponent<NavMeshAgent>();
        }


        private void HandlePlayerMovementEvent( Transform transform ){
            _playerTransform = transform;
        }



        private void checkifStuck( ){
            stuckTimer += Time.deltaTime;
            
            if (stuckTimer >= stuckCheckTime) {
				bool isStuck = Vector3.Distance( enemyOldPosition, enemyTransform.position ) < 3;
                //bool isStuck = (enemyOldPosition.x - enemyTransform.position.x) < 3;
                if( isStuck ){
                    //enemyAnim.SetBool("isPursuing", false);
                    Debug.Log("GOT Stuck " + navAgent.isPathStale );
                  //  enemyTransform.position = Vector3.forward;
                  //  enemyAnim.SetBool("isPursuing", true);
                }

                stuckTimer = 0f;
            }
            
        }


        private void LateUpdate( ){

            if (_playerTransform == null) {
                return;
            }
                        
            if ( gameOver ){
               // navAgent.enabled = false;
                enemyAnim.SetBool("isPursuing", false);
                return;
            }
                        
            if( !enemyHealth.isEnemyAlive() ){
                //navAgent.enabled = false;
                enemyAnim.SetBool("isPursuing", false);
                return;
            }


            //checkifStuck( );
            Vector3 playerPosition = _playerTransform.position;
            navAgent.destination = playerPosition;
            enemyAnim.SetBool("isPursuing", true);
			navAgent.transform.LookAt( _playerTransform );

            timer += Time.deltaTime;
            
            float distance = Vector3.Distance(playerPosition, enemyTransform.position);
            bool inAttackRange = (distance <= attackDistance);
            //Debug.Log("Distance: " + distance + ", InRange: " + inAttackRange);

            if ( timer >= timeBetweenAttacks && inAttackRange ){
                Attack();
            }
    
        }
        
        void Attack( ){

            timer = 0f;
            enemyAnim.SetTrigger("Attack");
            GameManager.instance.sendPlayerHitEvent(playerAttackDamage);
                                
        }
        

        private void HandleGameStoppedEvent(bool playerWon) {
            gameOver = true;
        }


        void OnDisable() {
            GameManager.onGameStoppedEvent -= HandleGameStoppedEvent;
            GameManager.onPlayerMovementEvent -= HandlePlayerMovementEvent;
        }

    }


}

using UnityEngine;


namespace stateproperty.fpshooter{

    public class EnemySpawner : MonoBehaviour{
        
        public GameObject enemy;
        public Transform[] spawnPoints;
        public int maxEnemyCount = 1;
        public float spawnInterval = 5f;

        private int enemyCount;


        void OnEnable( ){
            GameManager.onGameStartedEvent += HandleGameStartedEvent;
            GameManager.onEnemyHitEvent    += HandleEnemyHitEvent;
            GameManager.onGameStoppedEvent += HandleGameStoppedEvent;
        }
                
        
        private void HandleGameStartedEvent( ){
            InvokeRepeating("Spawn", 1f, spawnInterval);
        }


        private void HandleGameStoppedEvent( bool playerWon ){
            CancelInvoke("Spawn");
        }
        

        private void HandleEnemyHitEvent( bool isDead ){
            if( isDead ){
                --enemyCount;
            }
        }


        void Spawn( ){
             
            if ( enemyCount >= maxEnemyCount ){
                return;
            }

            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnTransform = spawnPoints[spawnPointIndex];
            GameObject clone = Instantiate(enemy, spawnTransform.position, spawnTransform.rotation) as GameObject;
            ++enemyCount;
			GameManager.instance.sendEnemySpawnedEvent( clone );
            
            /*
            if ( enemyCount < 3) {                
                Instantiate(enemy, spawnTransform.position, spawnTransform.rotation);
            }else{
                Transform playerTransform = enemyManager.getPlayerTransform();
                float distance = Vector3.Distance(playerTransform.position, spawnTransform.position);
                bool playerClose = (distance <= 30f);
                Debug.Log("Enemy Spawned count: " + enemyCount + ", Distance: " + distance);

                if( playerClose ){
                    Instantiate(enemy, spawnTransform.position, spawnTransform.rotation);
                    
                }
            }

            ++enemyCount;
            */
        }


        void OnDisable( ){
            GameManager.onGameStartedEvent  -= HandleGameStartedEvent;
            GameManager.onEnemyHitEvent     -= HandleEnemyHitEvent;
            GameManager.onGameStoppedEvent  -= HandleGameStoppedEvent;
        }

    }


}


using UnityEngine;
using UnityEngine.SceneManagement;

namespace stateproperty.fpshooter{


    public class GameManager : MonoBehaviour {


        public delegate void GameStarted();
        public static event GameStarted onGameStartedEvent;

        //Player
		public delegate void TreasurePicked(TreasureType treasureType);
        public static event TreasurePicked onTreasurePickedEvent;
                                                
		public delegate void PlayerHit( int damage);
        public static event PlayerHit onPlayerHitEvent;

        public delegate void PlayerMovement( Transform transform );
        public static event PlayerMovement onPlayerMovementEvent;

		public delegate void BulletFired(PlayerWeapon weaponType, bool enemyHit, GameObject enemyObject);
        public static event BulletFired onBulletFiredEvent;

        //Enemy
        public delegate void EnemyHit( bool isDead);
        public static event EnemyHit onEnemyHitEvent;

		public delegate void EnemySpawned( GameObject enemyOject );
		public static event EnemySpawned onEnemySpawnedEvent;

        //Game
        public delegate void InstructionPanelToggled( );
        public static event InstructionPanelToggled onInstructionToggleEvent;

        public delegate void TreasureMessage(Color color, string message);
        public static event TreasureMessage onTreasureMessageEvent;

        public delegate void ScoreMessage(Color color, string message);
        public static event ScoreMessage onScoreMessageEvent;

        public delegate void AmmoMessage(Color color, string message);
        public static event AmmoMessage onAmmoMessageEvent;

        public delegate void HelpMessage(Color color2, string message);
        public static event HelpMessage onHelpMessageEvent;
        
        public delegate void GameStopped( bool playerWon );
        public static event GameStopped onGameStoppedEvent;
        

        //----------------------------------------------------------------

        public static GameManager instance = null;

        private bool gameRunning;
		
        
        void Awake( ){

            if (instance == null) {
                instance = this;
            } else if (instance != this) {
                Destroy(gameObject);
            }

          //  DontDestroyOnLoad(gameObject);
            Cursor.lockState = CursorLockMode.None;

            Invoke("startGame", 3);
            
        }

        private void startGame() {
            gameRunning = true;
            sendGameStartedEvent();
            Cursor.lockState = CursorLockMode.Locked;
        }


        void Update( ){
            bool rPressed = Input.GetKeyDown(KeyCode.R);
            if ( !gameRunning && rPressed) {
                SceneManager.LoadScene(0);
            }

            bool iPressed = Input.GetKeyDown(KeyCode.I);
            if( iPressed ){
                sendInstructionToggleEvent();
            }

        }
     
        
        public void sendGameStartedEvent() {
            if (onGameStartedEvent != null) {
                onGameStartedEvent();
            }
        }


        public void sendPlayerMovementEvent( Transform transform) {
            if( onPlayerMovementEvent != null) {
                onPlayerMovementEvent(transform);
            }
        }
        

        public void sendTreasureMessage(Color color, string message ){
            if (onTreasureMessageEvent != null) {
                onTreasureMessageEvent(color, message);
            }
        }


        public void sendScoreMessageEvent( Color color, string message ){
            if(onScoreMessageEvent != null ){
                onScoreMessageEvent( color, message );
            }
        }


        public void sendInstructionToggleEvent(){
            if (onInstructionToggleEvent != null) {
                onInstructionToggleEvent();
            }
        }


        public void sendEnemyHitEvent( bool isDead ){
            if (onEnemyHitEvent != null) {
                onEnemyHitEvent(isDead);
            }
        }


		public void sendEnemySpawnedEvent( GameObject enemyOject ){
			if (onEnemySpawnedEvent != null) {
				onEnemySpawnedEvent(enemyOject);
            }
        }

		
        public void sendPlayerHitEvent(int damage) {
            if (onPlayerHitEvent != null) {
                onPlayerHitEvent(damage);
            }
        }
            

        public void sendTreasurePickedEvent( TreasureType type ){
            if( onTreasurePickedEvent != null) {
                onTreasurePickedEvent(type);
            }
        }


        public void sendAmmoMessage( Color color, string message ){
            if (onAmmoMessageEvent != null) {
                onAmmoMessageEvent(color, message);
            }
        }

        
        public void sendHelpMessage(Color color, string message ){
            if (onHelpMessageEvent != null) {
                onHelpMessageEvent(color, message);
            }
        }


		public void sendBulletFiredEvent( PlayerWeapon weaponType, bool enemyHit, GameObject enemyObject) {
            if( onBulletFiredEvent != null ){
                onBulletFiredEvent(weaponType, enemyHit, enemyObject );
            }
        }


        
        public void sendGameStoppedEvent( bool playerWon ){
            gameRunning = false;

            if (onGameStoppedEvent != null) {
                onGameStoppedEvent(playerWon);
            }

            Cursor.lockState = CursorLockMode.Locked;

        }
        

    }

}

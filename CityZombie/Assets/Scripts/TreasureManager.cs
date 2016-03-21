
using UnityEngine;


namespace stateproperty.fpshooter{


    public class TreasureManager : MonoBehaviour{

        private int scoreCount;
        private int treasureCount;

        void OnEnable(){
            GameManager.onBulletFiredEvent += HandleBulletFiredEvent;
            GameManager.onTreasurePickedEvent += HandleTreasurePickedEvent;
        }


		void HandleBulletFiredEvent(PlayerWeapon weaponType, bool enemyHit, GameObject enemyObjectHit) {

            if (enemyHit) {
				int damageScore = weaponType.getDamage();
                scoreCount += damageScore;
				string scoreMessage = scoreCount + "";
				GameManager.instance.sendScoreMessageEvent(UIManager.DEFAULT_COLOR, scoreMessage);
            
            }

        }


		public int getScore(){
			return scoreCount;
		}


        private void HandleTreasurePickedEvent(TreasureType type) {

            
            if( TreasureType.TREASURE_KEY == type ){

                ++treasureCount;
                string treasureMessage = treasureCount + "";
				GameManager.instance.sendTreasureMessage(UIManager.DEFAULT_COLOR, treasureMessage);

				string helpMessage = "Awesome! You found it, now open the house door!";
                GameManager.instance.sendHelpMessage(UIManager.HELP_COLOR, helpMessage);

                /*
                bool allTreasuresPicked = (treasureCount == TOTAL_TREASURE);
                if( allTreasuresPicked ){
                    GameManager.instance.sendGameStoppedEvent(true);

                } else{ 
                    string helpMessage = "Awesome! You found a treasure.";
                    GameManager.instance.sendHelpMessage(UIManager.HELP_COLOR, helpMessage);
                }
                */              
                
            }

        }
        

        void OnDisable( ){
            GameManager.onBulletFiredEvent -= HandleBulletFiredEvent;
            GameManager.onTreasurePickedEvent -= HandleTreasurePickedEvent;
        }

    }


}

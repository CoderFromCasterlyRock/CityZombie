
using UnityEngine;
using UnityEngine.UI;


namespace stateproperty.fpshooter{


    public class UIManager : MonoBehaviour {

        public Text helpText;
        public Text scoreText;
        public Text bulletText;
        public Text treasureText;
        public Text objectiveText;
        public Text instructionText;

        private Canvas objectiveCanvas;
        private Canvas instructionCanvas;
        
        public static string GAME_OBJ  = "Search the terrain, find the key and escape by opening the door.";
        private static string GAME_WON  = "Well Done, You Escaped!\n\nPress R to try again.\nZombies are waiting.";
        private static string GAME_LOST = "You are dead meat!\n\nPress R to try again.\nZombies are waiting.";

        public static Color AMMO_LOW_COLOR      = Color.red;
        public static Color DEFAULT_COLOR       = Color.blue;//FFEB04
        public static Color HELP_COLOR          = Color.red;

        void OnEnable( ){
            GameManager.onGameStartedEvent      += HandleGameStartedEvent;
            GameManager.onInstructionToggleEvent+= HandleInstructionToggleEvent;

            GameManager.onScoreMessageEvent     += HandleScoreMessageEvent;
            GameManager.onTreasureMessageEvent  += HandleTreasureMessageEvent;

            GameManager.onAmmoMessageEvent      += HandleAmmoMessageEvent;
            GameManager.onHelpMessageEvent      += HandleHelpMessageEvent;
            GameManager.onGameStoppedEvent      += HandleGameStoppedEvent;

        }

        
        void Start( ){
            objectiveCanvas = objectiveText.GetComponentInParent<Canvas>();
            instructionCanvas = instructionText.GetComponentInParent<Canvas>();
        }

        
        private void HandleInstructionToggleEvent( ){
            instructionCanvas.enabled = !instructionCanvas.enabled;
        }


        private void HandleGameStartedEvent( ){
            objectiveCanvas.enabled = false;
            HandleHelpMessageEvent(HELP_COLOR, GAME_OBJ);
        }


        private void HandleScoreMessageEvent( Color color, string message ){

            if (message != null) {
                scoreText.text = message;
                scoreText.color = color;
            }
            
        }


        private void HandleTreasureMessageEvent(Color color, string message ) {

            if (message != null) {
                treasureText.text = message;
                treasureText.color = color;
            }
            
        }


        private void HandleAmmoMessageEvent(Color color, string message) {

            if (message != null) {
                bulletText.text = message;
                bulletText.color = color;
            }

        }

        private void HandleHelpMessageEvent( Color color, string message ){

            if (message != null) {
                helpText.text = message;
                helpText.color = color;
            }

        }

    
        private void HandleGameStoppedEvent( bool playerWon ){
            string message = (playerWon) ? GAME_WON : GAME_LOST;
            objectiveCanvas.enabled = true;
            objectiveText.text = "";
            objectiveText.text = message;

            helpText.text = "";
            
        }


        private string ColorToHex(Color32 color) {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }
        
        void OnDisable( ){
            GameManager.onGameStartedEvent -= HandleGameStartedEvent;
   
            GameManager.onScoreMessageEvent -= HandleScoreMessageEvent;
            GameManager.onTreasureMessageEvent -= HandleTreasureMessageEvent;
            GameManager.onInstructionToggleEvent -= HandleInstructionToggleEvent;
            GameManager.onAmmoMessageEvent -= HandleAmmoMessageEvent;
            GameManager.onHelpMessageEvent -= HandleHelpMessageEvent;
            GameManager.onGameStoppedEvent -= HandleGameStoppedEvent;

        }


    }

}


using UnityEngine;


namespace stateproperty.fpshooter {


    public class TreasurePicker : MonoBehaviour {

		public TreasureType type;
		public int SHOW_KEY_THRESHOLD;

		private bool triggered;
		private bool isTreasureKey;
		private int playerLayer; 
		private TreasureManager treasureManager;

		void Start( ){
			playerLayer 	= LayerMask.NameToLayer("Player");
			isTreasureKey	= (TreasureType.TREASURE_KEY == type);
			treasureManager = GetComponentInParent<TreasureManager>();

			if( isTreasureKey ){
				gameObject.GetComponent<Renderer>().enabled =false;
			}
		}


		void OnTriggerEnter( Collider other ){

            bool playerCollided = (other.gameObject.layer == playerLayer);
            if( !playerCollided ) return;

        	GameManager.instance.sendHelpMessage(UIManager.HELP_COLOR, "Press P to pickup " + type);
			triggered = true;
	    }


		void Update( ){

			onlyShowKey();

		    bool pPressed = Input.GetKeyDown(KeyCode.P);
			if( triggered && pPressed){
                GameManager.instance.sendTreasurePickedEvent(type);
                Destroy(gameObject);
            }

        }


        private void onlyShowKey( ){

			if( !isTreasureKey ) return;

			if( treasureManager.getScore() >= SHOW_KEY_THRESHOLD ){
				gameObject.GetComponent<Renderer>().enabled = true;
			}
        }

    }


}


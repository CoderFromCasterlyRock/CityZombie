
using UnityEngine;

namespace stateproperty.fpshooter{


    public class DoorGuard : MonoBehaviour {

        public Renderer scareObject;
        //private AudioSource scareSound;
		
        private int playerLayer; 
		private bool treasurePicked = false;

		void OnEnable( ){
            GameManager.onTreasurePickedEvent += HandleTreasurePickedEvent;
        }


        void Start() {
          //  scareSound = GetComponent<AudioSource>();
			playerLayer = LayerMask.NameToLayer("Player");
		    scareObject.enabled = false;
        }


		private void HandleTreasurePickedEvent(TreasureType type) {
		    if( TreasureType.TREASURE_KEY == type ){
				treasurePicked = true;
			}
		}


        private void OnTriggerEnter(Collider other){
            
            bool playerCollided = (other.gameObject.layer == playerLayer);
            if( !playerCollided ) return;

			//Treasure found and arrived ta door
			if( treasurePicked ){
				GameManager.instance.sendGameStoppedEvent(true);

			}else{
				
				scareObject.enabled = true;
				//scareSound.PlayOneShot(scareSound.clip);
				GameManager.instance.sendHelpMessage(UIManager.HELP_COLOR, "Mac says, find the key first playa!");
				Invoke("hideMacAfterWarning", 5f);

			}

        }


		private void hideMacAfterWarning( ){
			scareObject.enabled = false;
	    }


		void OnDisable(){
            GameManager.onTreasurePickedEvent -= HandleTreasurePickedEvent;
        }

    }

}

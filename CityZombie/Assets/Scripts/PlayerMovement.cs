
using UnityEngine;


namespace stateproperty.fpshooter {


    public class PlayerMovement : MonoBehaviour {

		public AudioSource lightSound;

		private Light spotLight;
		private bool isLightOn;
        private Vector3 oldPosition;
		private float playerMovedThreshold = 1;
        private float transformPublishInterval = 2f;
        
        
        void OnEnable() {
            GameManager.onGameStartedEvent += HandleGameStartedEvent;
            GameManager.onGameStoppedEvent += HandleGameStoppedEvent;
        }


        void Start( ){
			isLightOn		= true;
        	spotLight		= GetComponentInChildren<Light>();
            oldPosition     = transform.position;
        }


		void Update( ){

		    bool fPressed 	= Input.GetKeyDown(KeyCode.F);
		    if( fPressed){
		    	isLightOn	= !isLightOn;
                spotLight.enabled = isLightOn;

             }

        }



        private void HandleGameStartedEvent( ){
            InvokeRepeating("sendTransform", transformPublishInterval, transformPublishInterval);
        }


        private void sendTransform() {

			Transform newTransform = transform;
            float distance = Vector3.Distance(oldPosition, newTransform.position);
            bool playerMoved = (distance >= playerMovedThreshold);
			if (playerMoved) {
				GameManager.instance.sendPlayerMovementEvent(newTransform);
            }

			oldPosition = newTransform.position;
            
        }


        private void HandleGameStoppedEvent(bool playerWon) {
            CancelInvoke("sendTransform");
        }


        void OnDisable() {
            GameManager.onGameStartedEvent -= HandleGameStartedEvent;
            GameManager.onGameStoppedEvent -= HandleGameStoppedEvent;
        }



    }


}


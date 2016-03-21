
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace stateproperty.fpshooter{


	public class GameRadar : MonoBehaviour{

		public Texture playerTexture;
		public Texture enemyTexture;
		public Texture backgroundTexture;

		private Vector2 mapCenter;
		private float mapScale = 0.3f;
		private float radarSize=150f;
		private float maxDist = 300;
		private Rect radarRectangle;

		private Transform _playerTransform;
		private List<GameObject> enemyList = new List<GameObject>( 32 );


        void OnEnable( ){
			GameManager.onEnemySpawnedEvent += HandleEnemySpawnedEvent;
			GameManager.onPlayerMovementEvent += HandlePlayerMovementEvent;
        }


        void Start( ){
			_playerTransform = transform;

			radarRectangle =new Rect(Screen.width-radarSize, Screen.height - radarSize, radarSize-10, radarSize-10);
			mapCenter = new Vector2(Screen.width-radarSize/2, Screen.height - radarSize/2);
		}


		private void HandlePlayerMovementEvent( Transform transform ){
            _playerTransform = transform;
        }


		private void HandleEnemySpawnedEvent( GameObject enemySpawned ){
			if( enemySpawned != null ){
				enemyList.Add( enemySpawned );
			}
		}


		public void OnGUI( ){

			UnityEngine.GUI.DrawTexture(radarRectangle, backgroundTexture, ScaleMode.StretchToFill);

			drawBlip(_playerTransform, playerTexture);

			for( int i = enemyList.Count -1; i>= 0; i-- ){
				GameObject enemyObject = enemyList[i];
				if( enemyObject == null ){
					enemyList.RemoveAt(i);
				}else{
					drawBlip(enemyObject.transform, enemyTexture);
				}
			}

    	}


		private void drawBlip( Transform goTransform, Texture texture ){

			Vector3 centerPos = _playerTransform.position;
			Vector3 extPos = goTransform.position;
 
      		// first we need to get the distance of the enemy from the player
      		float dist = Vector3.Distance(centerPos, extPos);
			if (dist > maxDist) return;

      		float dx = centerPos.x - extPos.x; // how far to the side of the player is the enemy?
      		float dz = centerPos.z - extPos.z; // how far in front or behind the player is the enemy?
 
     		 // what's the angle to turn to face the enemy - compensating for the player's turning?
			float deltay = Mathf.Atan2(dx, dz) * Mathf.Rad2Deg - 270 - _playerTransform.eulerAngles.y;
 	
      		// just basic trigonometry to find the point x,y (enemy's location) given the angle deltay
      		float bX = dist * Mathf.Cos(deltay * Mathf.Deg2Rad);
      		float bY = dist * Mathf.Sin(deltay * Mathf.Deg2Rad);
 
      		bX = bX * mapScale; // scales down the x-coordinate by half so that the plot stays within our radar
      		bY = bY * mapScale; // scales down the y-coordinate by half so that the plot stays within our radar
 
      		// this is the diameter of our largest radar circle
			UnityEngine.GUI.DrawTexture(new Rect(mapCenter.x + bX, mapCenter.y + bY, 8, 8), texture);
      		
    	}


		void OnDisble( ){
			GameManager.onEnemySpawnedEvent -= HandleEnemySpawnedEvent;
			GameManager.onPlayerMovementEvent -= HandlePlayerMovementEvent;
        }

	}


}

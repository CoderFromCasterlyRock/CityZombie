
using UnityEngine;


namespace stateproperty.fpshooter{


    public class PlayerShooting : MonoBehaviour{
        
        float timer;
        Ray shootRay;
        RaycastHit shootHit;
        int shootableMask;
        ParticleSystem gunParticles;
        LineRenderer gunLine;
        AudioSource gunAudio;

		private bool gameOver;
        private int bulletCount;

        private float range = 75f;
        private float timeBetweenBullets = 0.10f;
        private float effectsDisplayTime = 0.2f;

        private int BULLET_LOW_COUNT = 50;
        private int BULLET_CRISIS_COUNT = 25;
        private int TOTAL_BULLET_COUNT = 100;

        private string ENEMY_HIT   = "Good shot! Enemy Hit";
        private string LOW_BULLET0 = "Be careful! Low on Bullets, find the ammo chest.";
        private string LOW_BULLET1 = "Dangerously low on Bullets, find the ammo chest.";

        private Color helpColor = UIManager.HELP_COLOR;
        private Color ammoColor = UIManager.DEFAULT_COLOR;
        
		public PlayerWeapon weaponType = PlayerWeapon.RUSSIAN_GUN;


        void OnEnable() {
            GameManager.onTreasurePickedEvent += HandleTreasurePickedEvent;
			GameManager.onGameStoppedEvent      += HandleGameStoppedEvent;
        }


        void Start( ){

            bulletCount = TOTAL_BULLET_COUNT;
            shootableMask = LayerMask.GetMask("Enemy");
       //     gunParticles = GetComponent<ParticleSystem>();
            gunLine = GetComponent<LineRenderer>();
            gunAudio = GetComponent<AudioSource>();
                        
            GameManager.instance.sendAmmoMessage(ammoColor, getBulletDisplayMsg() );

        }
        

        void Update() {
                        
            bool fireButtonPressed = Input.GetButtonDown("Fire1");

            timer += Time.deltaTime;

            if (fireButtonPressed && timer >= timeBetweenBullets && Time.timeScale != 0) {
                Shoot();
            }

            if (timer >= timeBetweenBullets * effectsDisplayTime) {
                gunLine.enabled = false;
            }
        }



        private string getBulletDisplayMsg( ){
            return bulletCount + "";
        }


		private void HandleGameStoppedEvent( bool playerWon ){
         	gameOver = true;
        }

        private void HandleTreasurePickedEvent(TreasureType type) {
            if (TreasureType.AMMO_PACK != type) return;

            if( bulletCount < TOTAL_BULLET_COUNT ){
                bulletCount = TOTAL_BULLET_COUNT;

                GameManager.instance.sendAmmoMessage(ammoColor, getBulletDisplayMsg());
                GameManager.instance.sendHelpMessage(helpColor, "Ammo pack picked up! Bullets replenished");
            }

        }


        void Shoot() {

			if( gameOver ) return;
			if( bulletCount == 0) return;


            timer = 0f;
            gunAudio.Play();
            
            gunLine.enabled = true;

            shootRay.origin = transform.position;
            shootRay.direction = transform.forward;

            gunLine.SetPosition(0, shootRay.origin);

            GameObject enemyObject = null;
            bool enemyHit = Physics.Raycast(shootRay, out shootHit, range, shootableMask);

            if( enemyHit ){
                enemyObject = shootHit.collider.gameObject;
                gunLine.SetPosition(1, shootHit.point);
                                
            }else{
                gunLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
            }
            
            gunShotFired(weaponType, enemyHit, enemyObject);

        }



        private void gunShotFired( PlayerWeapon weaponType, bool enemyHit, GameObject enemyObject) {

            --bulletCount;
            
            string message  = "";
            
            if ( enemyHit ){
                message = ENEMY_HIT;
                GameManager.instance.sendBulletFiredEvent(weaponType, enemyHit, enemyObject);

            } else{

                bool bulletFullCrisis = bulletCount < BULLET_CRISIS_COUNT;
                bool bulletMiniCrisis = bulletCount < BULLET_LOW_COUNT;
                
                ammoColor = (bulletFullCrisis) ? UIManager.AMMO_LOW_COLOR : ammoColor;
                message = (bulletFullCrisis) ? LOW_BULLET1 : (bulletMiniCrisis? LOW_BULLET0 : UIManager.GAME_OBJ);
            }

            GameManager.instance.sendAmmoMessage(ammoColor, getBulletDisplayMsg());
            GameManager.instance.sendHelpMessage(helpColor, message);
                        
        }


        void OnDisable() {
            GameManager.onTreasurePickedEvent -= HandleTreasurePickedEvent;
			GameManager.onGameStoppedEvent    -= HandleGameStoppedEvent;
        }

    }


}
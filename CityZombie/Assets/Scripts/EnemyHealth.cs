
using UnityEngine;


namespace stateproperty.fpshooter{

    public class EnemyHealth : MonoBehaviour {

        public int currentHealth;
        public float sinkSpeed;
        public AudioClip deathClip;
        
        Animator anim;
        AudioSource enemyAudio;
        SphereCollider capsuleCollider;
        Rigidbody rigidBody;
        
        bool isDead;
        bool isSinking;
        
        void OnEnable() {
            GameManager.onBulletFiredEvent += HandleBulletFiredEvent;
        }


        void Start() {

            anim = GetComponent<Animator>();
            enemyAudio = GetComponent<AudioSource>();
            capsuleCollider = GetComponent<SphereCollider>();
            rigidBody = GetComponent<Rigidbody>();
            
        }
                

        void Update( ){
                        
            if (isSinking) {
                transform.Translate(-Vector3.up * sinkSpeed * Time.deltaTime);
            }
        }
        

        public bool isEnemyAlive( ){
            return (currentHealth > 0);
        }


        public bool isEnemyDead() {
            return (currentHealth <= 0);
        }


        public int getEnemyHealth() {
            return currentHealth;
        }


		void HandleBulletFiredEvent(PlayerWeapon weaponType, bool enemyHit, GameObject enemyObjectHit ){

//            Debug.Log("Weapon: " + weaponType + ", EnemyHit: " + enemyHit + ", Val: " + ((int)weaponType) );

            if (isDead) return;
            if (!enemyHit ) return;
            if (gameObject != enemyObjectHit) return;

            enemyAudio.Play();

            int damage =  weaponType.getDamage();
            currentHealth -= damage;

            /*
            if (currentHealth <= 25 ){
                anim.SetTrigger("Struck");
                // rigidBody.AddForceAtPosition(transform.forward * 5, shootHitPoint);
            }
            */

            if (currentHealth <= 0) {
                Death();
            }

        }


        void Death( ){
            
            isDead = true;

            GameManager.instance.sendEnemyHitEvent( true );
                
            capsuleCollider.isTrigger = true;
            enemyAudio.clip = deathClip;
            enemyAudio.Play();
            StartSinking();

        }


        public void StartSinking( ){
            anim.SetTrigger("Death");
            GetComponent<NavMeshAgent>().enabled = false;
            rigidBody.isKinematic = true;
            isSinking = true;
            
            Destroy(gameObject, 2f);
        }



        void OnDisable() {
            GameManager.onBulletFiredEvent -= HandleBulletFiredEvent;
        }


    }

}
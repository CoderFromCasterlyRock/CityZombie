
using UnityEngine;
using UnityEngine.UI;


namespace stateproperty.fpshooter {

    public class PlayerHealth : MonoBehaviour {

        public int currentHealth = 100;
        public Slider healthSlider;
        public AudioClip deathClip;

        AudioSource playerAudio;
        PlayerShooting playerShooting;

		private bool isDead = false;

        void OnEnable() {
            GameManager.onPlayerHitEvent += HandlePlayerHitEvent;
        }


        void Awake() {
            
            playerAudio = GetComponent<AudioSource>();
            playerShooting = GetComponentInChildren<PlayerShooting>();
        }
        


        public bool isPlayerAlive( ){
            return (currentHealth > 0);
        }


        public int getHealth( ){
            return currentHealth;
        }

        public bool medicinePicked(Collider other) {
            bool needHealth = (currentHealth < 100);
            if( needHealth) {
                currentHealth = 100;
                healthSlider.value = currentHealth;
            }

            return needHealth;
        }



        private void HandlePlayerHitEvent(int amount) {

            currentHealth -= amount;
            healthSlider.value = currentHealth;
                        
            playerAudio.Play();

            if (currentHealth <= 0 && !isDead) {
                Death();
            }

        }


        void Death() {
            isDead = true;

            playerAudio.clip = deathClip;
            playerAudio.Play();
            
            playerShooting.enabled = false;
            GameManager.instance.sendGameStoppedEvent(false);

        }


        void OnDisable( ){
            GameManager.onPlayerHitEvent -= HandlePlayerHitEvent;
        }

    }


}

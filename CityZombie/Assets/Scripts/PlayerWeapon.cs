

namespace stateproperty.fpshooter{


    public class PlayerWeapon{

		private string type;        
		private int damage;

		private PlayerWeapon( string type, int damage ){
			this.type 	= type;
			this.damage	= damage;
		}

		public string getType( ){
			return type;
		}

		public int getDamage( ){
			return damage;
		}


		public static PlayerWeapon RUSSIAN_GUN = new PlayerWeapon( "RUSSIAN_GUN", 50);


	}

}

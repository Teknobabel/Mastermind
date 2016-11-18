using UnityEngine;
using System.Collections;

public class StartNewGameState : IGameState {

	public void EnterState (){

		Debug.Log ("Starting new game");

		Random.InitState(MainMenu.instance.randomSeed);

		Game newGame = Game.CreateInstance<Game> ();

		Director newDirector = GameManager.instance.m_directorBank [Random.Range (0, GameManager.instance.m_directorBank.Length)];
		newGame.AddDirectorToGame (newDirector);

		newGame.Initialize ();

		foreach (HenchmenData h in GameManager.instance.m_henchmenBank) {
			Henchmen newHenchman = Henchmen.CreateInstance<Henchmen> ();
			newHenchman.Initialize (h);
			newGame.AddHenchmanToGame (newHenchman);
		}

		foreach (RegionData r in GameManager.instance.m_regionBank) {
			Region newRegion = Region.CreateInstance<Region> ();
			newRegion.Initialize (r);
			newGame.AddRegionToGame (newRegion);
		}

		// initialize player

		Organization player = Organization.CreateInstance<Organization> ();
		player.Initialize (newDirector, newGame, MainMenu.instance.orgName);
		newGame.AddOrganizationToGame (player);

		GameManager.instance.game = newGame;

		GameManager.instance.PushMenuState(MenuState.State.TabMenu);

		GameManager.instance.ChangeGameState (GameManager.instance.beginTurn);
	}

	public void UpdateState () {
	
	}

	public void ExitState (){
		Debug.Log ("Exiting start new game phase");
	}
}

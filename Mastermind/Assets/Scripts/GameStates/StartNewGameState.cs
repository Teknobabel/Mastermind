using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

		foreach (HenchmenData h in GameManager.instance.m_agentBank) {
			Henchmen newHenchman = Henchmen.CreateInstance<Henchmen> ();
			newHenchman.Initialize (h);
			newGame.AddAgentToGame (newHenchman);
		}

		// initialize player

		Organization player = Organization.CreateInstance<Organization> ();
		player.Initialize (MainMenu.instance.orgName);
		newGame.AddOrganizationToGame (player);

		// initialize agent org

		AgentOrganization agentOrg = AgentOrganization.CreateInstance<AgentOrganization> ();
		newGame.AddAgentOrganizationToGame (agentOrg);
		agentOrg.Initialize ("Interpol");

		foreach (RegionData r in GameManager.instance.m_regionBank) {
			Region newRegion = Region.CreateInstance<Region> ();
			newRegion.Initialize (r);
			newGame.AddRegionToGame (newRegion);
		}

		if (newDirector.m_startingAgents > 0) {

			for (int i = 0; i < newDirector.m_startingAgents; i++) {

				agentOrg.SpawnAgentInWorld (null);
			}
		}


		// set up menu tabs

		Dictionary<int, MenuTab> menuTabs = new Dictionary<int, MenuTab> ();

		MenuTab henchTab = new MenuTab ();
		henchTab.m_name = "HENCHMEN";
		henchTab.m_menuState = MenuState.State.HenchmenMenu;
		henchTab.Initialize ();
		menuTabs.Add (henchTab.id, henchTab);

		MenuTab missionTab = new MenuTab ();
		missionTab.m_name = "MISSIONS";
		missionTab.m_menuState = MenuState.State.MissionMenu;
		missionTab.Initialize ();
		menuTabs.Add (missionTab.id, missionTab);

		MenuTab lairTab = new MenuTab ();
		lairTab.m_name = "LAIR";
		lairTab.m_menuState = MenuState.State.LairMenu;
		lairTab.Initialize ();
		menuTabs.Add (lairTab.id, lairTab);

		MenuTab worldTab = new MenuTab ();
		worldTab.m_name = "WORLD";
		worldTab.m_menuState = MenuState.State.WorldMenu;
		worldTab.Initialize ();
		menuTabs.Add (worldTab.id, worldTab);

		MenuTab activityTab = new MenuTab ();
		activityTab.m_name = "ACTIVITY";
		activityTab.m_menuState = MenuState.State.ActivityMenu;
		activityTab.Initialize ();
		menuTabs.Add (activityTab.id, activityTab);

		foreach (OmegaPlan op in player.omegaPlans) {

			MenuTab opTab = new MenuTab ();
			opTab.m_name = "OMEGA PLAN";
			opTab.objectID = op.id;
			opTab.m_menuState = MenuState.State.OmegaPlanMenu;
			opTab.Initialize ();
			menuTabs.Add (opTab.id, opTab);

			if (op.state == OmegaPlan.State.Hidden) {

				opTab.drawTab = false;
				op.AddObserver (opTab);
			}
		}

		MenuTab agentTab = new MenuTab ();
		agentTab.m_name = "AGENTS";
		agentTab.m_menuState = MenuState.State.AgentsMenu;
		agentTab.Initialize ();
		menuTabs.Add (agentTab.id, agentTab);

		// don't draw tab if there aren't currently any visible agents

		if (agentOrg.currentAgents.Count == 0) {

			agentTab.drawTab = false;

		} else {
			
			foreach (AgentWrapper aw in agentOrg.currentAgents) {

				if (aw.m_vizState == AgentWrapper.VisibilityState.Visible || aw.m_vizState == AgentWrapper.VisibilityState.Tracked) {
					break;
				}

				agentTab.drawTab = false;
			}
		}

		MenuTab databaseTab = new MenuTab ();
		databaseTab.m_name = "DATABASE";
		databaseTab.m_menuState = MenuState.State.DatabaseMenu;
		databaseTab.Initialize ();
		menuTabs.Add (databaseTab.id, databaseTab);

		TabMenu.instance.menuTabs = menuTabs;


//		GameManager.instance.game.SpawnIntel (); // debug

		GameManager.instance.PushMenuState(MenuState.State.TabMenu);

		GameManager.instance.ChangeGameState (GameManager.instance.beginTurn);
	}

	public void UpdateState () {
	
	}

	public void ExitState (){
		Debug.Log ("Exiting start new game phase");
	}
}

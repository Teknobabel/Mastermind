using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AgentOrganization : OrganizationBase, IObserver {

	private Dictionary<int, List<TurnResultsEntry>> m_turnResults = new Dictionary<int, List<TurnResultsEntry>> (); // by turn number
	private Dictionary<GameEvent, List<TurnResultsEntry>> m_turnResultsByType = new Dictionary<GameEvent, List<TurnResultsEntry>> ();

	private List<AgentWrapper> m_currentAgents = new List<AgentWrapper>();
	private Dictionary<IAgentAIState, List<AgentWrapper>> m_agentsByState = new Dictionary<IAgentAIState, List<AgentWrapper>>();

	private int m_agentsToSpawn = 0;

	private AgentAIState_Idle m_agentAIState_Idle;
	private AgentAIState_AttackBase m_agentAIState_AttackBase;
	private AgentAIState_AttackHenchmen m_agentAIState_AttackHenchmen;
	private AgentAIState_FreeRegion m_agentAIState_FreeRegion;
	private AgentAIState_CaptureIntel m_agentAIState_CaptureIntel;

	public override void Initialize (string orgName)
	{
		// spawn any starting agent as per director

		Director d = GameManager.instance.game.director;
		Game g = GameManager.instance.game;

		m_agentAIState_Idle = new AgentAIState_Idle ();
		m_agentAIState_AttackBase = new AgentAIState_AttackBase ();
		m_agentAIState_FreeRegion = new AgentAIState_FreeRegion ();
		m_agentAIState_CaptureIntel = new AgentAIState_CaptureIntel ();
		m_agentAIState_AttackHenchmen = new AgentAIState_AttackHenchmen ();

		if (GameManager.instance.game.director.m_startingAgentData.Length > 0) {

			for (int i=0; i < d.m_startingAgentData.Length; i++)
			{
				Henchmen h = g.GetAgent (d.m_startingAgentData [i]);

				SpawnAgentInWorld (h);
			}
		}

		AddObserver (TabMenu.instance);
	}

	public void RemoveAgentFromMissions (AgentWrapper aw)
	{
		aw.m_agent.ChangeState (Henchmen.state.Idle);

		List<MissionWrapper> cancelledMissions = new List<MissionWrapper> ();

		foreach (MissionWrapper mw in activeMissions) {

			if (mw != GameManager.instance.currentlyExecutingMission) {
				
				if (mw.m_agentInFocus != null && mw.m_agentInFocus.m_agent.id == aw.m_agent.id) {

					mw.m_agentInFocus = null;
				}

				if (mw.m_agents.Contains (aw)) {

					mw.m_agents.Remove (aw);
				}

				if (mw.m_henchmenInFocus != null && mw.m_henchmenInFocus.id == aw.m_agent.id) {

					mw.m_henchmenInFocus = null;
				}

				if (mw.m_henchmen.Contains (aw.m_agent)) {

					mw.m_henchmen.Remove (aw.m_agent);
				}

				if (mw.m_henchmenInFocus == null && mw.m_agentInFocus == null && mw.m_henchmen.Count == 0 && mw.m_agents.Count == 0) {

					cancelledMissions.Add (mw);
				}
			}
		}

		foreach (MissionWrapper mw in cancelledMissions) {

			if (activeMissions.Contains (mw)) {

				mw.m_mission.CancelMission (mw);
				activeMissions.Remove (mw);
			}
		}
	}

	public override void AddMission (MissionWrapper mw)
	{
		
		foreach (AgentWrapper aw in mw.m_agents) {
			if (aw.m_agent.currentState != Henchmen.state.OnMission) {
				aw.m_agent.ChangeState (Henchmen.state.OnMission);
			}
		}

		if (mw.m_agentInFocus != null && mw.m_agentInFocus.m_agent.currentState != Henchmen.state.OnMission) {

			mw.m_agentInFocus.m_agent.ChangeState(Henchmen.state.OnMission);
		}

		base.AddMission (mw);
	}

	public void PlayerWantedLevelIncreased ()
	{
		m_agentsToSpawn++;
	}

	public override void MissionCompleted (MissionWrapper a)
	{
		if (a.m_agentInFocus != null) {
			a.m_agentInFocus.m_agent.ChangeState (Henchmen.state.Idle);
		}

		foreach (AgentWrapper aw in a.m_agents) {
			aw.m_agent.ChangeState (Henchmen.state.Idle);
		}

		base.MissionCompleted (a);
	}

	public override void CancelMission (MissionWrapper mw)
	{
		base.CancelMission (mw);

		foreach (AgentWrapper a in mw.m_agents) {

			if (a.m_agent.currentState == Henchmen.state.OnMission) {

				a.m_agent.ChangeState (Henchmen.state.Idle);
			}
		}

		if (mw.m_agentInFocus != null && mw.m_agentInFocus.m_agent.currentState == Henchmen.state.OnMission) {

			mw.m_agentInFocus.m_agent.ChangeState (Henchmen.state.Idle);
		}
	}

	public void SpawnAgentInWorld (Henchmen henchmen)
	{
		Debug.Log ("Spawning Agent");

		List<Henchmen> agentBank = new List<Henchmen> ();
		List<Henchmen> currentHenchmen = new List<Henchmen>();

		// sort out currently active henchmen from bank list

		foreach (AgentWrapper aw in m_currentAgents) {
			currentHenchmen.Add (aw.m_agent);
		}

		foreach (Henchmen a in GameManager.instance.game.agents) {

			if (!currentHenchmen.Contains (a)) {

				agentBank.Add (a);
			}
		}

		if (agentBank.Count > 0) {

			// get a random empty region

			Region region = GameManager.instance.game.GetRandomRegion (true);

			if (region == null) {

				// if no empty regions exist, get a random region

				region = GameManager.instance.game.GetRandomRegion (false);
			}

			if (region != null) {

				Henchmen agent = null;

				if (henchmen == null) { // if a specific agent isn't supplied, choose a random agent

					// strength of valid agents is based on current wanted level

					List<Henchmen> validHenchmen = new List<Henchmen> ();
					Organization player = GameManager.instance.game.player;

					foreach (Henchmen a in agentBank) {

						if (player.currentWantedLevel <= 1 && a.rank == 1) {

							validHenchmen.Add (a);
						} else if (player.currentWantedLevel == GameManager.instance.game.director.m_maxWantedLevel && a.rank == 3) {

							validHenchmen.Add (a);
						} else if (player.currentWantedLevel < GameManager.instance.game.director.m_maxWantedLevel && a.rank < 3) {
							validHenchmen.Add (a);
						}
					}
						
					if (validHenchmen.Count > 0) {
						
						int r = Random.Range (0, validHenchmen.Count);
						agent = validHenchmen [r];

					} else {

						return;
					}

				} else {

					agent = henchmen;
				}

				// choose random slot in region
				List<Region.HenchmenSlot> validHSList = new List<Region.HenchmenSlot>();
				foreach (Region.HenchmenSlot hs in region.henchmenSlots) {

					if (hs.m_state == Region.HenchmenSlot.State.Empty) {

						validHSList.Add (hs);
					}
				}

				AgentWrapper aw = new AgentWrapper ();
				aw.m_agent = agent;
				aw.ChangeAIState (m_agentAIState_Idle);
				aw.ChangeVisibilityState (AgentWrapper.VisibilityState.Hidden);
				aw.AddObserver (this);
				m_currentAgents.Add (aw);

				if (validHSList.Count > 0) {
					region.ReserveSlot(aw.m_agent, validHSList[Random.Range(0, validHSList.Count)]);
				}

				region.AddAgent (aw);

				Debug.Log ("Agent: " + agent.henchmenName + " spawning in region: " + region.regionName);
			}
		}
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		
		switch (thisGameEvent) {

		case GameEvent.Agent_BecameVisible:
			
			Notify (subject, GameEvent.Agent_BecameVisible);
			break;
		}
	}

	public List<AgentWrapper> currentAgents {get{ return m_currentAgents;}}
	public int agentsToSpawn {get{ return m_agentsToSpawn;} set{m_agentsToSpawn = value;}}
	public Dictionary<IAgentAIState, List<AgentWrapper>> agentsByState {get{ return m_agentsByState; } set{ m_agentsByState = value; }}

	public AgentAIState_Idle agentAIState_Idle {get{return m_agentAIState_Idle;}}
	public AgentAIState_AttackBase agentAIState_AttackBase {get{return m_agentAIState_AttackBase;}}
	public AgentAIState_AttackHenchmen agentAIState_AttackHenchmen {get{return m_agentAIState_AttackHenchmen;}}
	public AgentAIState_FreeRegion agentAIState_FreeRegion {get{return m_agentAIState_FreeRegion;}}
	public AgentAIState_CaptureIntel agentAIState_CaptureIntel {get{return m_agentAIState_CaptureIntel;}}
}

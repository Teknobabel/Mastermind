using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
	public static GameManager instance;

	public bool m_forceMissionSuccess = false;

	public HenchmenData[] m_henchmenBank;
	public HenchmenData[] m_agentBank;
	public RegionData[] m_regionBank;
	public Director[] m_directorBank;
	public MissionBase[] m_missionBank;
	public MissionBase[] m_agentMissionBank;
	public MenuState[] m_menuStates;

	public RegionData m_lairRegion;
	public Asset m_lairAsset;
	public RegionData m_limboRegion;
	public TravelToRegion m_travelMission;
	public AssetToken m_intel;
	public Policy_War m_declareWar;

	public TraitData[] m_statusTraits;

	public TechTree m_techTree;

	private StartNewGameState m_startNewGame;
	private BeginTurnState m_beginTurn;
	private BeginPlayerPhaseState m_beginPlayerPhase;
	private PlayerPhaseState m_playerPhase;
	private MissionPhaseState m_missionPhase;
	private EndPlayerPhaseState m_endPlayerPhase;
	private RegionPhaseState m_regionPhase;
	private AgentPhaseState m_agentPhase;
	private EndTurnState m_endTurnPhase;

//	private AgentAIState_Idle m_agentState_Idle;

	private IGameState m_currentState = null;

	public Game m_game;

	private List<MenuState> m_menuStateStack = new List<MenuState>();
	private MenuState m_menuState = null;
	private MenuState.State m_targetMenuState = MenuState.State.None;
	private MenuTab m_currentTab = null;

	private int m_currentID = 0;

	private MissionWrapper m_currentMissionWrapper;
	private MissionWrapper m_currentlyExecutingMission;

	private string m_currentVersion = "VER 0.0.1";

	void Awake ()
	{
		Application.targetFrameRate = 60;

		if(!instance) {
			instance = this;
		} else {
			Destroy(gameObject);
		}

		m_startNewGame = new StartNewGameState ();
		m_beginTurn = new BeginTurnState ();
		m_beginPlayerPhase = new BeginPlayerPhaseState ();
		m_playerPhase = new PlayerPhaseState ();
		m_missionPhase = new MissionPhaseState ();
		m_endPlayerPhase = new EndPlayerPhaseState ();
		m_regionPhase = new RegionPhaseState ();
		m_agentPhase = new AgentPhaseState ();
		m_endTurnPhase = new EndTurnState ();

//		m_agentState_Idle = new AgentAIState_Idle ();

	}

	void Update ()
	{
		if (m_menuState != null) {
			m_menuState.OnUpdate ();
		}
	}

	void Start ()
	{
		PushMenuState(MenuState.State.MainMenu);
	}

	public void ChangeGameState (IGameState newState)
	{
		if (m_currentState != null) {
			m_currentState.ExitState ();
		}

		m_currentState = newState;

		newState.EnterState ();
	}

	public void ProcessMissionWrapper ()
	{
		if (m_currentMissionWrapper != null && m_currentMissionWrapper.m_mission != null) {
			
			Debug.Log ("Processing Mission Request: " + m_currentMissionWrapper.m_mission.m_name);

			if (m_currentMissionWrapper.m_organization != null) {

				m_currentMissionWrapper.m_organization.AddMission (m_currentMissionWrapper);

			} else {
				
				GameManager.instance.game.player.AddMission (m_currentMissionWrapper);
			}

			m_currentMissionWrapper = null;
		}
	}

	public void PushMenuState (MenuState.State newState)
	{
		Debug.Log ("Push State: " + newState);

		MenuState menuState = null;
		foreach (MenuState m in m_menuStates) {
			if (m.state == newState) {
				menuState = m;
				break;
			}
		}

		if (m_menuState != null)
		{
			m_menuState.OnHold();
		}

		m_menuState = menuState;

		if (m_menuState != null)
		{
			m_menuStateStack.Add (m_menuState);
			m_menuState.OnActivate(null);
		}
	}

	public void PushMenuState (MenuTab newTab)
	{
		Debug.Log ("Push State: " + newTab.m_menuState);

		m_currentTab = newTab;

//		MenuState menuState = this.GetComponentInChildren(newStateType) as MenuState;
		MenuState menuState = null;
		foreach (MenuState m in m_menuStates) {
			if (m.state == newTab.m_menuState) {
				menuState = m;
				break;
			}
		}

		if (m_menuState != null)
		{
			m_menuState.OnHold();
		}

		m_menuState = menuState;

		if (m_menuState != null)
		{
			m_menuStateStack.Add (m_menuState);
			m_menuState.OnActivate(newTab);
		}
	}

	public void PopMenuState ()
	{
		if (m_menuState)
		{
			Debug.Log("Pop State: " + m_menuState);
//			m_previousState = m_gameState.state;
			m_menuState.OnDeactivate();
		}

		if (m_menuStateStack.Count > 1) {
			m_menuStateStack.RemoveAt(m_menuStateStack.Count-1);
			m_menuState = m_menuStateStack[m_menuStateStack.Count-1];

			if (m_menuState != null)
			{
				Debug.Log("New State: " + m_menuState);
				m_menuState.OnReturn();
			}
		}
	}

	public StartNewGameState startNewGame {get{return m_startNewGame; }}
	public BeginTurnState beginTurn {get{return m_beginTurn; }}
	public BeginPlayerPhaseState beginPlayerPhase {get{return m_beginPlayerPhase; }}
	public PlayerPhaseState playerPhase {get{return m_playerPhase; }}
	public MissionPhaseState missionPhase {get{return m_missionPhase; }}
	public EndPlayerPhaseState endPlayerPhase {get{return m_endPlayerPhase; }}
	public RegionPhaseState regionPhase {get{return m_regionPhase; }}
	public AgentPhaseState agentPhase {get{return m_agentPhase; }}
	public EndTurnState endTurnPhase {get{return m_endTurnPhase; }}

//	public AgentAIState_Idle agentState_Idle {get{return m_agentState_Idle;}}

	public Game game {get{return m_game; } set {m_game = value; }}
	public MenuState.State currentMenuState {get{return m_menuState.m_state;}}
	public MenuState currentMenu {get{ return m_menuState;}}
	public int currentTabID {get{return m_currentTab.id;}}
	public string currentVersion {get{return m_currentVersion; }}
	public int newID {get{m_currentID++; return m_currentID;}}
	public MissionWrapper currentMissionWrapper {get{return m_currentMissionWrapper;}set{m_currentMissionWrapper = value;}}
	public MissionWrapper currentlyExecutingMission {get{return m_currentlyExecutingMission;}set{m_currentlyExecutingMission = value;}}
	public MenuState.State targetMenuState {get{ return m_targetMenuState; }set{m_targetMenuState = value; }}
}

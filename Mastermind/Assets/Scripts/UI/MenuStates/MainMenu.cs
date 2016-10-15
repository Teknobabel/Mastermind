using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MenuState {
	public static MainMenu instance;

	public GameObject m_mainMenu;

	public Text 
	m_versionText,
	m_orgNameText;

	private string m_orgName = "Null";

	void Awake ()
	{
		if(!instance) {
			instance = this;
		} else {
			Destroy(gameObject);
		}
	}

	public override void OnActivate(MenuTab newTab)
	{
		m_mainMenu.gameObject.SetActive (true);
		m_versionText.text = GameManager.instance.currentVersion;

		RandomizeOrgName ();
	}

	public override void OnHold()
	{
		m_mainMenu.gameObject.SetActive (false);
	}

	public override void OnReturn()
	{
		m_mainMenu.gameObject.SetActive (true);
	}

	public override void OnDeactivate()
	{
		m_mainMenu.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{

	}

	public void StartNewGameButton ()
	{
		Debug.Log ("Start New Game Button Pressed");
		GameManager.instance.ChangeGameState (GameManager.instance.startNewGame);
	}

	public void RandomizeOrgName ()
	{
		string s = "Null";

		List<string> names = new List<string> ();

		names.Add ("Bad Guys Club");
		names.Add ("Phantoms");
		names.Add ("The Ancient Ones");
		names.Add ("World Eaters");
		names.Add ("Evil Inc.");
		names.Add ("Facebook");
		names.Add ("The Olympians");
		names.Add ("The Collective");
		names.Add ("COBRA");
		names.Add ("The Triad");
		names.Add ("Archangels");
		names.Add ("Brimstone");
		names.Add ("The Invisibles");
		names.Add ("Neo Humanis");
		names.Add ("The Purge");
		names.Add ("Iron Claw");
		names.Add ("The Harlequin Club");
		names.Add ("The Silver Skulls");
		names.Add ("The Nexus");
		names.Add ("The Revenants");
		names.Add ("The Gate Keepers");
		names.Add ("The Key Holders");
		names.Add ("Masterminds Inc.");
		names.Add ("The Virus");
		names.Add ("ICARUS");
		names.Add ("Methuselah");
		names.Add ("Disaster Force");
		names.Add ("Alpha Wolf Squadron");
		names.Add ("The Silent Shadow");
		names.Add ("Carnage Corps");
		names.Add ("The Circle");
		names.Add ("UMBRA");
		names.Add ("LOCUST");
		names.Add ("The Swarm");
		names.Add ("The Amber Princes");
		names.Add ("Killgrave");
		names.Add ("The Cortex");
		names.Add ("The Usurpers");
		names.Add ("The Reavers");
		names.Add ("The Initiative");
		names.Add ("The Marduk Institute");
		names.Add ("Ghostnet");
		names.Add ("The Deadbutantes");
		names.Add ("Waveform Collapse");
		names.Add ("The Berkshire Group");
		names.Add ("Overwatch");
		names.Add ("BLK Manufacturing");
		names.Add ("The Orbis Initiative");
		names.Add ("Tenebris GmbH");
		names.Add ("Coversus Concern");
		names.Add ("Triskelion");
		names.Add ("Infernal Order");
		names.Add ("Kingmaker");
		names.Add ("Society of Scales");
		names.Add ("Babel");
		names.Add ("FURNACE");
		names.Add ("The Mandelbrot Circle");
		names.Add ("The Harbingers");
		names.Add ("The Static");
		names.Add ("The Sons of Ur");
		names.Add ("The Atunement");
		names.Add ("The Resonance");
		names.Add ("The Charybdis Collective");
		names.Add ("Encanto");

		s = names[Random.Range(0, names.Count)].ToUpper();

		m_orgName = s;
		m_orgNameText.text = s;
	}

	public string orgName {get{return m_orgName;}}
}

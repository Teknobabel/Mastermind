using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MenuState {

	public GameObject m_mainMenu;

	public Text m_versionText;

	public override void OnActivate()
	{
		m_mainMenu.gameObject.SetActive (true);
		m_versionText.text = GameManager.instance.currentVersion;
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
}

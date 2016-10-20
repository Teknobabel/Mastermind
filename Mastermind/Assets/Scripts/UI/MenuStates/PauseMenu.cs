using UnityEngine;
using System.Collections;

public class PauseMenu : MenuState {

	public GameObject m_pauseMenu;

	public override void OnActivate(MenuTab tabInfo)
	{
		Debug.Log ("Starting Pause Menu");
		m_pauseMenu.gameObject.SetActive (true);
	}

	public override void OnHold()
	{
	}

	public override void OnReturn()
	{
	}

	public override void OnDeactivate()
	{
		m_pauseMenu.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{

	}

	public void RestartButtonPressed ()
	{

	}

	public void ResumeButtonPressed ()
	{
		GameManager.instance.PopMenuState ();
	}
}

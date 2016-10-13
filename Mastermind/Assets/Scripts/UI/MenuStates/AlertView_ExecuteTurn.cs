using UnityEngine;
using System.Collections;

public class AlertView_ExecuteTurn : MenuState {

	public GameObject m_alertViewMenu;

	private float m_duration = 1.0f;
	private float t = 0.0f;

	public override void OnActivate()
	{
		Debug.Log ("Starting AlertView ExecuteTurn Menu");
		m_alertViewMenu.gameObject.SetActive (true);
	}

	public override void OnHold()
	{
	}

	public override void OnReturn()
	{
	}

	public override void OnDeactivate()
	{
		m_alertViewMenu.gameObject.SetActive (false);
	}

	public override void OnUpdate()
	{
		t += Time.deltaTime;

		if (t >= m_duration) {
			t = 0.0f;
			GameManager.instance.PopMenuState ();
		}
	}
}

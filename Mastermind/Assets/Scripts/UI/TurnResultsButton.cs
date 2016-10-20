using UnityEngine;
using System.Collections;
using TMPro;

public class TurnResultsButton : MonoBehaviour {

	public TextMeshProUGUI m_buttonText;

	private int m_turnNumber = -1;

	public void Initialize (int turnNumber)
	{
		m_turnNumber = turnNumber;
		m_buttonText.text = "TURN " + m_turnNumber.ToString ();
	}

	public void ButtonPressed ()
	{
		ActivityMenu.instance.UpdateActiveTurnView (m_turnNumber);
	}
}

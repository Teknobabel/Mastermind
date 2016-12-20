using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TurnResultsButton : MonoBehaviour, IObserver {

	public enum State
	{
		None,
		Inactive,
		Active,
	}

	public TextMeshProUGUI m_buttonText;
	public RawImage m_buttonImage;

	private int m_turnNumber = -1;

	private State m_state = State.Inactive;

//	private Color m_startingButtonColor;

	public void Initialize (int turnNumber)
	{
		m_turnNumber = turnNumber;
		m_buttonText.text = "TURN " + m_turnNumber.ToString ();
//		m_startingButtonColor = m_buttonImage.color;

		ActivityMenu.instance.AddObserver (this);
	}

	public void ButtonPressed ()
	{
		ActivityMenu.instance.TurnResultsButtonPressed (m_turnNumber);
	}

	public void OnNotify (ISubject subject, GameEvent thisGameEvent)
	{
		switch (thisGameEvent) {
		case GameEvent.ActivityMenu_TurnResultsChanged:
			if (ActivityMenu.instance.currentlyDisplayedTurnNumber == m_turnNumber && m_state != State.Active) {
				ChangeState (State.Active);
			} else if (ActivityMenu.instance.currentlyDisplayedTurnNumber != m_turnNumber && m_state == State.Active) {
				ChangeState (State.Inactive);
			}
			break;
		}
	}

	private void ChangeState (State newState)
	{
		m_state = newState;

		switch (m_state) {
		case State.Active:
//			m_buttonImage.color = Color.black;
//			m_buttonText.color = Color.white;
			m_buttonImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.TurnResultsButton_BG_Unselected);
			m_buttonText.color = ColorManager.instance.GetColor (ColorManager.UIElement.TurnResultsButton_BG_Unselected);
			break;
		case State.Inactive:
//			m_buttonImage.color = m_startingButtonColor;
//			m_buttonText.color = Color.black;
			m_buttonImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.TurnResultsButton_BG_Selected);
			m_buttonText.color = ColorManager.instance.GetColor (ColorManager.UIElement.TurnResultsButton_BG_Selected);
			break;
		}
	}

	public void Destroy ()
	{
		ActivityMenu.instance.RemoveObserver (this);
	}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SortModeButton : MonoBehaviour {

	public enum State
	{
		Selected,
		Unselected,
	}

	public Image m_image;

	public WorldMenu.SortType m_sortType = WorldMenu.SortType.None;

	private State m_state = State.Unselected;

	public void ChangeState (State newState)
	{
		m_state = newState;

		switch (newState) {

		case State.Selected:
			m_image.color = Color.grey;
			break;
		case State.Unselected:
			m_image.color = Color.white;
			break;
		}
	}

	public State state {get{return m_state;}}
}

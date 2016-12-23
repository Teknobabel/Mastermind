using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SortModeButton : MonoBehaviour {

	public enum State
	{
		Selected,
		Unselected,
	}

	public RawImage m_image;
	public TextMeshProUGUI m_text;

	public WorldMenu.SortType m_sortType = WorldMenu.SortType.None;

	private State m_state = State.Unselected;

	public void ChangeState (State newState)
	{
		m_state = newState;

		switch (newState) {

		case State.Selected:
			m_image.color = ColorManager.instance.GetColor (ColorManager.UIElement.ContentView_SortPanel_Button_BG_Selected);
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.ContentView_SortPanel_Button_Text_Selected);
			break;
		case State.Unselected:
			m_image.color = ColorManager.instance.GetColor (ColorManager.UIElement.ContentView_SortPanel_Button_BG_Unselected);
			m_text.color = ColorManager.instance.GetColor (ColorManager.UIElement.ContentView_SortPanel_Button_Text_Unselected);
			break;
		}
	}

	public State state {get{return m_state;}}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TabButton : MonoBehaviour {

	public enum State {
		None,
		Selected,
		Unselected,
	}

	public TextMeshProUGUI m_buttonText;
	public RawImage m_buttonImage;
	public Button m_button;
	private State m_state = State.None;
	private MenuTab m_menuTab;

//	private Color m_selectedColor = Color.white;
//	private Color m_unselectedColor = new Color(0.631f, 0.631f, 0.631f);

	public void Initialize (MenuTab t)
	{
		m_buttonText.text = t.GetTabName ();
		t.m_tabButton = this;
		m_menuTab = t;
	}
		
	public void Deactivate ()
	{
		gameObject.SetActive (false);
	}
	
	public void ChangeState (State newState)
	{
		ColorBlock cb = m_button.colors;

		switch (newState) {
		case State.Selected:

			Color selectedColor = ColorManager.instance.GetColor(ColorManager.UIElement.MainNav_Tab_BG_Selected);

//			cb.normalColor = selectedColor;
//			m_button.colors = cb;
			m_buttonImage.color = selectedColor;
			break;

		case State.Unselected:

			Color unselectedColor = ColorManager.instance.GetColor(ColorManager.UIElement.MainNav_Tab_BG_Unselected);

//			cb.normalColor = unselectedColor;
//			m_button.colors = cb;
			m_buttonImage.color = unselectedColor;
			break;
		}
	}

	public void TabButtonClicked ()
	{
		if (GameManager.instance.currentTabID != m_menuTab.id) {
			GameManager.instance.targetMenuState = MenuState.State.TabMenu;
			GameManager.instance.PopMenuState ();

			GameManager.instance.PushMenuState (m_menuTab);
		}
	}
}

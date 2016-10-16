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
	public Image m_buttonImage;
	public Button m_button;
	private State m_state = State.None;
	private MenuTab m_menuTab;

	private Color m_selectedColor = Color.white;
	private Color m_unselectedColor;

	// Use this for initialization
	void Start () {
		ColorBlock cb = m_button.colors;
		m_unselectedColor = cb.normalColor;
	}

	public void Initialize (MenuTab t)
	{
		m_buttonText.text = t.m_name;
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
//			m_buttonImage.color = Color.white;
			cb.normalColor = m_selectedColor;
			m_button.colors = cb;
			break;
		case State.Unselected:
//			m_buttonImage.color = Color.gray;
			cb.normalColor = m_unselectedColor;
			m_button.colors = cb;
			break;
		}
	}

	public void TabButtonClicked ()
	{
		if (GameManager.instance.currentTabID != m_menuTab.id) {
			GameManager.instance.PopMenuState ();

			GameManager.instance.PushMenuState (m_menuTab);
		}
	}
}

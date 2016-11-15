using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TokenButton : MonoBehaviour {

	public TextMeshProUGUI m_tokenText;
	public Image m_tokenImage;

	private Region.TokenSlot.State m_state = Region.TokenSlot.State.None;

	private Region.TokenSlot m_tokenSlot = null;

	// Use this for initialization
	void Start () {
	
	}

	public void Initialize (Region.TokenSlot t)
	{
		m_tokenSlot = t;

		ChangeState (t.m_state);
	}

	public void Deactivate ()
	{
		gameObject.SetActive (false);
	}

	public void ChangeState (Region.TokenSlot.State newState)
	{
		switch (newState) {
		case Region.TokenSlot.State.Hidden:
			
			m_tokenImage.fillCenter = true;

			switch (m_tokenSlot.m_type) {
			case Region.TokenSlot.TokenType.Policy:
				m_tokenText.text = "P";
				break;
			case Region.TokenSlot.TokenType.Asset:
				m_tokenText.text = "A";
				break;
			case Region.TokenSlot.TokenType.Control:
				m_tokenText.text = "C";
				break;
			}
			break;
		case Region.TokenSlot.State.None:
		case Region.TokenSlot.State.Revealed:

			m_tokenImage.fillCenter = false;
			
			TokenBase b = m_tokenSlot.GetBaseToken ();
			if (b != null) {
				string s = b.m_name.ToUpper ();

//				if (m_tokenSlot.m_status != Region.TokenSlot.Status.Normal) {
//					s += "\n<color=red>(" + m_tokenSlot.m_status.ToString ().ToUpper () + ")</color>";
//				}

				m_tokenText.text = s;
			} else {
				m_tokenText.text = "";
			}

			if (m_tokenSlot.m_owner == Region.TokenSlot.Owner.Player) {
				m_tokenImage.color = Color.green;
				m_tokenText.color = Color.green;
			} else if (m_tokenSlot.m_effects.Contains (Region.TokenSlot.Status.Protected)) {
				
				m_tokenImage.color = Color.blue;
				m_tokenText.color = Color.blue;

				if (m_tokenSlot.m_effects.Contains (Region.TokenSlot.Status.Vulnerable)) {
					m_tokenText.color = Color.red;
				}

			} else if (m_tokenSlot.m_effects.Contains (Region.TokenSlot.Status.Vulnerable) && !m_tokenSlot.m_effects.Contains (Region.TokenSlot.Status.Protected)) {
				m_tokenImage.color = Color.red;
				m_tokenText.color = Color.red;
			} else if (m_tokenSlot.m_effects.Count > 0 && !m_tokenSlot.m_effects.Contains (Region.TokenSlot.Status.Vulnerable) && !m_tokenSlot.m_effects.Contains (Region.TokenSlot.Status.Protected)) {
				m_tokenImage.color = Color.magenta;
				m_tokenText.color = Color.magenta;
			}
			break;

//		case Region.TokenSlot.State.None:
//
//			m_tokenImage.fillCenter = false;
//			m_tokenImage.color = Color.grey;
//			m_tokenText.text = "";
//
//			break;
		}

		m_state = newState;
	}
	
	public Region.TokenSlot tokenSlot {get{return m_tokenSlot;}}
}

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

//	public void InitializeAsset (Region.TokenSlot t)
//	{
//		Initialize (t.m_assetToken);
//
//		if (t.m_state == Region.TokenSlot.State.Hidden) {
//			m_tokenText.text = "A";
//		}
//	}
//
//	public void InitializePolicy (Region.TokenSlot t)
//	{
//		Initialize (t.m_policyToken);
//
//		if (t.m_state == Region.TokenSlot.State.Hidden) {
//			m_tokenText.text = "P";
//		}
//	}
//
//	public void InitializeControl (Region.TokenSlot t)
//	{
//		Initialize (t.m_controlToken);
//
//		if (t.m_state == Region.TokenSlot.State.Hidden) {
//			m_tokenText.text = "C";
//		}
//	}

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

		case Region.TokenSlot.State.Revealed:
				TokenBase b = m_tokenSlot.GetBaseToken ();
				if (b != null) {
					m_tokenText.text = b.m_name.ToUpper ();
				}
			break;
		}

		m_state = newState;
	}
	

}

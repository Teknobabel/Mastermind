using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TokenButton : MonoBehaviour {

	public TextMeshProUGUI m_tokenText;
	public Image m_tokenImage;

	private TokenBase.State m_state = TokenBase.State.None;

	// Use this for initialization
	void Start () {
	
	}

	private void Initialize (TokenBase tb)
	{
		m_tokenText.text = tb.m_name.ToUpper ();
		ChangeState (tb.state);
	}

	public void Deactivate ()
	{
		gameObject.SetActive (false);
	}

	public void Initialize (AssetToken a)
	{
		Initialize ((TokenBase)a);

		if (a.state == TokenBase.State.Hidden) {
			m_tokenText.text = "A";
		}
	}

	public void Initialize (PolicyToken p)
	{
		Initialize ((TokenBase)p);

		if (p.state == TokenBase.State.Hidden) {
			m_tokenText.text = "P";
		}
	}

	public void Initialize (ControlToken c)
	{
		Initialize ((TokenBase)c);

		if (c.state == TokenBase.State.Hidden) {
			m_tokenText.text = "C";
		}
	}

	public void ChangeState (TokenBase.State newState)
	{
		switch (newState) {
		case TokenBase.State.Hidden:
			m_tokenImage.fillCenter = true;
			break;
		case TokenBase.State.Revealed:

			break;
		}

		m_state = newState;
	}
	

}

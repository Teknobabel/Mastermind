using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TokenButton : MonoBehaviour {

	public TextMeshProUGUI m_tokenText;
	public RawImage 
	m_tokenImage,
	m_tokenTypeIndicator;

	private TokenSlot.State m_state = TokenSlot.State.None;

	private TokenSlot m_tokenSlot = null;

	// Use this for initialization
	void Start () {
	
	}

	public void Initialize (TokenSlot t)
	{
		m_tokenSlot = t;

		ChangeState (t.m_state);
	}

	public void Deactivate ()
	{
		gameObject.SetActive (false);
	}

	public void ChangeState (TokenSlot.State newState)
	{
		switch (m_tokenSlot.m_type) {
		case TokenSlot.TokenType.Policy:
			m_tokenTypeIndicator.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_Skill);
			break;
		case TokenSlot.TokenType.Asset:
			m_tokenTypeIndicator.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_Asset);
			break;
		case TokenSlot.TokenType.Control:
			m_tokenTypeIndicator.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_Resource);
			break;
		}

		switch (newState) {
		case TokenSlot.State.Hidden:
			
			m_tokenText.color = ColorManager.instance.GetColor(ColorManager.UIElement.Token_Text_Normal_Hidden);
			m_tokenImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.Token_BG_Normal_Hidden);

			switch (m_tokenSlot.m_type) {
			case TokenSlot.TokenType.Policy:
				m_tokenText.text = "P";
				break;
			case TokenSlot.TokenType.Asset:
				m_tokenText.text = "A";
				break;
			case TokenSlot.TokenType.Control:
				m_tokenText.text = "C";
				break;
			}
			break;
		case TokenSlot.State.None:
		case TokenSlot.State.Revealed:

//			m_tokenImage.fillCenter = false;
			
			TokenBase b = m_tokenSlot.GetBaseToken ();
			if (b != null) {
				string s = b.m_name.ToUpper ();

//				if (m_tokenSlot.m_status != TokenSlot.Status.Normal) {
//					s += "\n<color=red>(" + m_tokenSlot.m_status.ToString ().ToUpper () + ")</color>";
//				}

				m_tokenText.text = s;
			} else {
				m_tokenText.text = "";
			}

			if (m_tokenSlot.owner == Region.Owner.Player) {

				if (m_tokenSlot.m_effects.Contains (TokenSlot.Status.Protected)) {
					m_tokenImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_BG_Protected);
				} else if (m_tokenSlot.m_effects.Contains (TokenSlot.Status.Vulnerable)) {
					m_tokenImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_BG_Vulnerable);
				} else {
					m_tokenImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_BG_PlayerOwned);
				}

				m_tokenText.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_Text_PlayerOwned);
			} else if (m_tokenSlot.m_effects.Contains (TokenSlot.Status.Protected)) {

				m_tokenImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_BG_Protected);
				m_tokenText.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_Text_Protected);

				if (m_tokenSlot.m_effects.Contains (TokenSlot.Status.Vulnerable)) {
					m_tokenText.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_Text_Vulnerable);
				}

			} else if (m_tokenSlot.m_effects.Contains (TokenSlot.Status.Vulnerable) && !m_tokenSlot.m_effects.Contains (TokenSlot.Status.Protected)) {

				m_tokenImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_BG_Vulnerable);
				m_tokenText.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_Text_Vulnerable);

			} else if (m_tokenSlot.m_effects.Count > 0 && !m_tokenSlot.m_effects.Contains (TokenSlot.Status.Vulnerable) && !m_tokenSlot.m_effects.Contains (TokenSlot.Status.Protected)) {

				m_tokenImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_BG_StatusEffect);
				m_tokenText.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_Text_StatusEffect);
			} else {

				m_tokenImage.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_BG_Normal);
				m_tokenText.color = ColorManager.instance.GetColor (ColorManager.UIElement.Token_Text_Normal_Revealed);
			}
			break;

//		case TokenSlot.State.None:
//
//			m_tokenImage.fillCenter = false;
//			m_tokenImage.color = Color.grey;
//			m_tokenText.text = "";
//
//			break;
		}

		m_state = newState;
	}
	
	public TokenSlot tokenSlot {get{return m_tokenSlot;}}
}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TraitButton : MonoBehaviour {

	public TextMeshProUGUI m_traitName;
	public Image m_traitButtonImage;

	private Color 
	m_disabledColor = new Color (0.7f, 0.7f, 0.7f),
	m_skillColor = Color.yellow,
	m_resourceColor = Color.blue,
	m_giftColor = Color.green,
	m_flawColor = Color.red;

	public void Initialize (TraitData t, bool activeTrait)
	{
		m_traitName.text = t.m_name.ToUpper();

		Color c = Color.gray;

		if (activeTrait) {
			switch (t.m_class) {
			case TraitData.TraitClass.Skill:
				c = m_skillColor;
				break;
			case TraitData.TraitClass.Resource:
				c = m_resourceColor;
				break;
			case TraitData.TraitClass.Gift:
				c = m_giftColor;
				break;
			case TraitData.TraitClass.Flaw:
				c = m_flawColor;
				break;
			}
		}

		m_traitButtonImage.color = c;
		m_traitName.color = c;
	}

	public void Deactivate ()
	{
		m_traitButtonImage.fillCenter = true;
		m_traitButtonImage.color = m_disabledColor;
		m_traitName.gameObject.SetActive (false);
	}
}

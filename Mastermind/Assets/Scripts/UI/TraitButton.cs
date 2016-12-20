using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TraitButton : MonoBehaviour {

	public TextMeshProUGUI m_traitName;
	public RawImage m_traitButtonImage;

//	private Color 
//	m_disabledColor = Color.magenta,
//	m_skillColor = Color.magenta,
//	m_resourceColor = Color.magenta,
//	m_giftColor = Color.magenta,
//	m_flawColor = Color.magenta,
//	m_assetColor = Color.magenta;

	public void Initialize (TraitData t, bool activeTrait)
	{
		m_traitName.text = t.m_name.ToUpper();

		if (activeTrait) {
			switch (t.m_class) {
			case TraitData.TraitClass.Skill:
				m_traitButtonImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_Skill);
				m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Skill);
				break;
			case TraitData.TraitClass.Resource:
				m_traitButtonImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_Resource);
				m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Resource);
				break;
			case TraitData.TraitClass.Gift:
				m_traitButtonImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_Gift);
				m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Gift);
				break;
			case TraitData.TraitClass.Flaw:
				m_traitButtonImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_Flaw);
				m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Flaw);
				break;
			}
		}

//		m_traitName.color = c;
	}

	public void Initialize (Asset a, bool activeAsset)
	{
		m_traitName.text = a.m_name.ToUpper ();

//		Color c = Color.gray;

		if (activeAsset) {
			m_traitButtonImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_Asset);
			m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Asset);
		}
			
//		m_traitName.color = c;
	}

	public void Deactivate ()
	{
//		m_traitButtonImage.fillCenter = true;
		m_traitButtonImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_None);
		m_traitName.gameObject.SetActive (false);
	}
}

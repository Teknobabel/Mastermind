using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class TraitButton : MonoBehaviour {

	public TextMeshProUGUI m_traitName;
	public RawImage 
	m_traitButtonBGImage,
	m_traitTypeIndicator;

//	private Color 
//	m_disabledColor = Color.magenta,
//	m_skillColor = Color.magenta,
//	m_resourceColor = Color.magenta,
//	m_giftColor = Color.magenta,
//	m_flawColor = Color.magenta,
//	m_assetColor = Color.magenta;

	private int m_positionInList = -1;

	public void Initialize (TraitData t, bool activeTrait, int positionInList)
	{
		m_positionInList = positionInList;

		Initialize (t, activeTrait);
	}

	public void Initialize (TraitData t, bool activeTrait)
	{
		// start text crawl for trait name

		TextCrawl tc = (TextCrawl) m_traitName.transform.GetComponent<TextCrawl>();
		if (tc != null) {

			if (m_positionInList != -1) {

				float delay = 0.2f * ((float)(m_positionInList));
				tc.Initialize (t.GetName().ToUpper (), delay);
			} else {
				
				tc.Initialize (t.GetName().ToUpper ());
			}
		}



//		if (activeTrait) {
			switch (t.m_class) {

		case TraitData.TraitClass.Skill:
			
			m_traitTypeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_BG_Skill);

			if (activeTrait) {
				m_traitName.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_Text_Skill);
				m_traitButtonBGImage.color =  ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG);
			} else {
				m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Inactive);
				m_traitButtonBGImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_None);
			}
				break;

		case TraitData.TraitClass.Resource:
			
			m_traitTypeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_BG_Resource);

			if (activeTrait) {
				m_traitName.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_Text_Resource);
				m_traitButtonBGImage.color =  ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG);
			} else {
				m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Inactive);
				m_traitButtonBGImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_None);
			}

				break;

		case TraitData.TraitClass.Gift:
			
			m_traitTypeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_BG_Gift);

			if (activeTrait) {
				m_traitName.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_Text_Gift);
				m_traitButtonBGImage.color =  ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG);
			} else {
				m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Inactive);
				m_traitButtonBGImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_None);
			}

				break;

		case TraitData.TraitClass.Flaw:
			
			m_traitTypeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_BG_Flaw);

			if (activeTrait) {
				m_traitName.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_Text_Flaw);
				m_traitButtonBGImage.color =  ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG);
			} else {
				m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Inactive);
				m_traitButtonBGImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_None);
			}

				break;


		case TraitData.TraitClass.Status:
			
			m_traitTypeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_BG_Status);

			if (activeTrait) {
				m_traitName.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_Text_Status);
				m_traitButtonBGImage.color =  ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG);
			} else {
				m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Inactive);
				m_traitButtonBGImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_None);
			}

				break;

		case TraitData.TraitClass.Dynamic:

			DynamicTrait dt = (DynamicTrait)t;

			if (dt.m_linkType == DynamicTrait.LinkType.Rival || dt.m_linkType == DynamicTrait.LinkType.Wanted || dt.m_linkType == DynamicTrait.LinkType.Fear) {

				m_traitTypeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_BG_Flaw);

			} else {

				m_traitTypeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_BG_Gift);

			}



			if (activeTrait) {
				m_traitName.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_Text_Status);
				m_traitButtonBGImage.color =  ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG);
			} else {
				m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Inactive);
				m_traitButtonBGImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_None);
			}

			break;
			} 

//		} else {
//
//			m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Inactive);
//		}
	}

	public void Initialize (Asset a, bool activeAsset)
	{
//		m_traitName.text = a.m_name.ToUpper ();

		// start text crawl for asset name
		TextCrawl tc = (TextCrawl) m_traitName.transform.GetComponent<TextCrawl>();
		if (tc != null) {

			tc.Initialize(a.m_name.ToUpper());
		}

//		Color c = Color.gray;
		m_traitTypeIndicator.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_BG_Asset);


		if (activeAsset) {
			
			m_traitButtonBGImage.color =  ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG);
			m_traitName.color = ColorManager.instance.GetColor (ColorManager.UIElement.TraitButton_Text_Asset);

		} else {

			m_traitName.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_Text_Inactive);
			m_traitButtonBGImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_None);
		}
	}

	public void Deactivate ()
	{
//		m_traitButtonImage.fillCenter = true;
		m_traitButtonBGImage.color = ColorManager.instance.GetColor(ColorManager.UIElement.TraitButton_BG_None);
		m_traitTypeIndicator.gameObject.SetActive (false);
		m_traitName.gameObject.SetActive (false);
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Palettes/New Palette")]
public class Palette : ScriptableObject {

	[System.Serializable]
	public struct PaletteColor
	{
		public string m_paletteColorName;
		public Color m_color;
	}

	[System.Serializable]
	public struct UIElementLink
	{
		public string m_UIElementLinkName;
		public List<ColorManager.UIElement> m_linkedUIElements;
		public string m_colorName;
	}

	public string m_paletteName = "Palette Name";

	public PaletteColor[] m_paletteColors;

	public UIElementLink[] m_UIElementLinks;

	public Color GetColor (ColorManager.UIElement e)
	{
		Color c = Color.magenta;

		foreach (UIElementLink l in m_UIElementLinks) {

			if (l.m_linkedUIElements.Contains (e)) {

				foreach (PaletteColor p in m_paletteColors) {

					if (p.m_paletteColorName == l.m_colorName) {

						return p.m_color;
					}
				}
			}
		}

		Debug.Log ("Color for UI Element not found: " + e);

		return c;
	}
}

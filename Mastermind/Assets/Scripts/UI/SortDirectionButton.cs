using UnityEngine;
using System.Collections;
using TMPro;

public class SortDirectionButton : MonoBehaviour {

	public TextMeshProUGUI m_buttonText;

	public void UpdateState (WorldMenu.SortDirection dir)
	{
		switch (dir) {
		case WorldMenu.SortDirection.Normal:
			m_buttonText.text = "v";
			break;
		case WorldMenu.SortDirection.Reverse:
			m_buttonText.text = "^";
			break;
		}
	}
}

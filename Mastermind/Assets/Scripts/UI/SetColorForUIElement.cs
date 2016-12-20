using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class SetColorForUIElement : MonoBehaviour {

	public enum ImageType
	{
		None,
		Image,
		RawImage,
		Text,
	}

	public ColorManager.UIElement m_UIElement;
	public ImageType m_imageType = ImageType.Image;

	// Use this for initialization
	void Start () {
	
		Color c = ColorManager.instance.GetColor (m_UIElement);

		switch (m_imageType) {

		case ImageType.Image:

			Image i = this.gameObject.GetComponent<Image> ();
			if (i != null) {

				i.color = c;
			} else {
				Debug.Log ("No image component found");
			}
			break;

		case ImageType.RawImage:

			RawImage r = this.gameObject.GetComponent<RawImage> ();
			if (r != null) {

				r.color = c;
			} else {
				Debug.Log ("No raw image component found");
			}
			break;

		case ImageType.Text:

			TextMeshProUGUI t = this.gameObject.GetComponent<TextMeshProUGUI> ();
			if (t != null) {
				t.color = c;
			} else {
				Debug.Log ("No text mesh component found");
			}

			break;
		}
	}
	
//	// Update is called once per frame
//	void Update () {
//	
//	}
}

using UnityEngine;
using System.Collections;

public class RotateUIObject : MonoBehaviour {

	public float 
	m_minRotateSpeed = 0.0f,
	m_maxRotateSpeed = 1.0f;

	private RectTransform m_transform = null;
	private float m_rotateSpeed = 0.0f;

	// Use this for initialization
	void Start () {
	
		m_transform = (RectTransform)this.gameObject.GetComponent<RectTransform> ();

		if (m_maxRotateSpeed != m_minRotateSpeed) {

			m_rotateSpeed = Random.Range (m_minRotateSpeed, m_maxRotateSpeed);
		} else {

			m_rotateSpeed = m_maxRotateSpeed;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (m_transform != null) {

			Vector3 rot = m_transform.localRotation.eulerAngles;
			rot.z += m_rotateSpeed * Time.deltaTime;

			m_transform.rotation = Quaternion.Euler(rot);
		}
	
	}
}

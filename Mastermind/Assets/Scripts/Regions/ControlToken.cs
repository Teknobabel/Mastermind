using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class ControlToken : TokenBase, IToken {

	public enum ControlType
	{
		None,
		Military,
		Economic,
		Political,
		Random,
	}

	public ControlType m_controlType = ControlType.None;

	public void ClaimToken ()
	{

	}
}

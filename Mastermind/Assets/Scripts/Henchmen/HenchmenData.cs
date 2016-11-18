using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class HenchmenData : ScriptableObject {

	public string m_name = "Null";

	public int m_rank = 1;

	public int 
	m_hireCost = 1,
	m_costPerTurn = 1;

	public Sprite 
	m_portrait,
	m_portraitShort;

	public TraitData[] m_startingTraits;

	public TraitData m_startingStatusTrait;

	// Use this for initialization
	void OnEnable () {

	}
}

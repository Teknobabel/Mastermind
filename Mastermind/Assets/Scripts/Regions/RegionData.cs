using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class RegionData : ScriptableObject {

	public enum Rank
	{
		None,
		One,
		Two,
		Three,
	}

	public enum RegionGroup
	{
		None,
		NorthAmerica,
		SouthAmerica,
		Africa,
		Australia,
		EuropeanUnion,
		Asia,
	}

	public string m_name = "Null";

	public Rank m_rank = Rank.None;

	public int m_henchmenSlots = 1;

	public RegionGroup m_regionGroup = RegionGroup.None;

	public PolicyToken[] m_policyTokens;
	public AssetToken[] m_assetTokens;
	public ControlToken[] m_controlTokens;

}

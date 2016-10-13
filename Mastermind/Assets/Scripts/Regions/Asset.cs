using UnityEngine;
using System.Collections;

[CreateAssetMenu()]
public class Asset : ScriptableObject {

	public enum Rank
	{
		None,
		One,
		Two,
		Three,
		Four,
	}

	public enum AssetType
	{
		None,
		TradeGoods,
		Soldiers,
		Workers,
		Money,
		Scientists,
		Tanks,
		Data,
		Weapons,
		Drugs,
		Art,
		Missile,
		Jets,
		Blackmail,
		Satellite,
		Rocket,
		NuclearMissile,
		SpySatellite,
		Random,
	}

	public string m_name = "Null";
	public AssetType m_assetType = AssetType.None;
	public Rank m_rank = Rank.None;
}

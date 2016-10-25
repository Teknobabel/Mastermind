using UnityEngine;
using System.Collections;


public class OPGoalBase : ScriptableObject {

	protected string m_goalName = "Null";

	protected OmegaPlan m_omegaPlan;

	protected int m_id = -1;

	public virtual OPGoalBase GetObject (){return OPGoalBase.CreateInstance<OPGoalBase>();}

	public virtual void Initialize (OmegaPlan op, Organization o)
	{
		m_omegaPlan = op;
		m_id = GameManager.instance.newID;
	}

	public virtual string GetText (){return "Null";}
	public int id {get{ return m_id;}}
	public string goalName {get{return m_goalName;}}
}

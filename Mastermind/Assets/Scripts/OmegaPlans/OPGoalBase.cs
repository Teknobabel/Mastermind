using UnityEngine;
using System.Collections;


public class OPGoalBase : ScriptableObject {

	protected string m_goalName = "Null";

	public virtual OPGoalBase GetObject (){return OPGoalBase.CreateInstance<OPGoalBase>();}

	public virtual void Initialize (){}

	public virtual string GetText (){return "Null";}

	public string goalName {get{return m_goalName;}}
}

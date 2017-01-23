using UnityEngine;
using UnityEngine.UI;

public class SetCooldownTint : MonoBehaviour {

	public GMScript GM;
	public NumpadInput NI;
	public enum AttackType
	{
		Shark, Wave, Storm
	}
	public AttackType Type;
	
	void Update () {
		float div=1;
		switch (Type)
		{
		case AttackType.Shark:
			div=GM.GetNormalizedAttackCooldown (0);
		break;
		case AttackType.Wave:
			div=GM.GetNormalizedAttackCooldown (1);
		break;
		case AttackType.Storm:
			div=GM.GetNormalizedAttackCooldown (2);
		break;
		default:
		break;
		}
		transform.localScale = new Vector3 (div,1f,1f);
		if (NI.redAttackIcons [(int)Type])
			transform.parent.GetComponent<Image> ().color = Color.red;
		else
			transform.parent.GetComponent<Image> ().color = Color.white;
	}
}

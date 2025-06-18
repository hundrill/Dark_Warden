using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VideoPokerKit;

public class UI_One_Grade : MonoBehaviour
{
	public TextMeshProUGUI txt_Chips;
	public TextMeshProUGUI txt_Mult;
	public TextMeshProUGUI txt_Grade;

	public Wins winType;

	public Outline outline;

	public void OnGradeSelect(Wins type)
	{
		if (outline)
			outline.enabled = type == winType;
	}

	public void SetWinsType(Wins type)
	{
		winType = type;

		SetWinsName(CardDataManager.name_Wins[(int)type]);

		int[] paytableMultipliers = Paytable.the.GetMultipliers();

		SetChips(Paytable.the.GetChips(type));
		SetMult(Paytable.the.GetWinMultiplier(type));
	}

	public void SetChips(int chips)
	{
		if (txt_Chips != null)
			txt_Chips.text = string.Format("{0}", chips);
	}

	public void SetMult(int mult)
	{
		if (txt_Mult != null)
			txt_Mult.text = string.Format("{0}", mult);
	}

	public void SetWinsName(string name)
	{
		if (txt_Grade != null)
			txt_Grade.text = name;
	}
}

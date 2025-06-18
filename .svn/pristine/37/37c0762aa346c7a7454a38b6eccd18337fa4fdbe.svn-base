using UnityEngine;
using VideoPokerKit;

public class UI_Toggle : MonoBehaviour
{
	public GameObject toggle_Makr;
	public Wins type;

	public void Toggle()
	{
		if (toggle_Makr == null)
			return;

		if (toggle_Makr.activeSelf == true)
			if (CardDataManager.instance.list_User_Checked.Count <= 1)
				return;

		toggle_Makr.SetActive(!toggle_Makr.activeSelf);

		if (CardDataManager.instance)
			CardDataManager.instance.Reset_User_Check(type, toggle_Makr.activeSelf);
	}
}

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemReplace_Ui : MonoBehaviour
{
	public Image icon;
	public TextMeshProUGUI txt_Name;
	public TextMeshProUGUI txt_Ability_1;
	public TextMeshProUGUI txt_Ability_2;
	public TextMeshProUGUI txt_Ability_3;

	public TextMeshProUGUI txt_Value_1;
	public TextMeshProUGUI txt_Value_2;
	public TextMeshProUGUI txt_Value_3;

	public GameObject[] arrow_up;
	public GameObject[] arrow_down;

	public void SettingItem(string id)
	{
		if (id == "")
		{
			if (icon) icon.gameObject.SetActive(false);

			if (txt_Ability_1) txt_Ability_1.text = "";
			if (txt_Ability_2) txt_Ability_2.text = "";
			if (txt_Ability_3) txt_Ability_3.text = "";

			if (txt_Value_1) txt_Value_1.text = "";
			if (txt_Value_2) txt_Value_2.text = "";
			if (txt_Value_3) txt_Value_3.text = "";
		}
		else
		{
			if (icon) icon.gameObject.SetActive(true);

			if (txt_Name) txt_Name.text = (string)CsvManager.instance.GetTextData(id);

			if (txt_Ability_1) txt_Ability_1.text = "공격력";
			if (txt_Ability_2) txt_Ability_2.text = "방어력";
			if (txt_Ability_3) txt_Ability_3.text = "HP";

			string iconname = CsvManager.instance.GetItem(id, ITEM.IMAGE).ToString();
			icon.sprite = DataManager.instance.GetSprite(iconname);

			int n;
			int.TryParse(CsvManager.instance.GetItem(id, ITEM.ATK).ToString(), out n);
			if (txt_Value_1) txt_Value_1.text = string.Format("{0}", n);

			int.TryParse(CsvManager.instance.GetItem(id, ITEM.DEF).ToString(), out n);
			if (txt_Value_2) txt_Value_2.text = string.Format("{0}", n);

			int.TryParse(CsvManager.instance.GetItem(id, ITEM.HP).ToString(), out n);
			if (txt_Value_3) txt_Value_3.text = string.Format("{0}", n);
		}

	}

	public void ShowArrow(string nowid, string newid)
	{
		int[] val_now = new int[3];
		int[] val_new = new int[3];

		val_now[0] = DataManager.instance.GetItemValue(nowid, ITEM.ATK);
		val_now[1] = DataManager.instance.GetItemValue(nowid, ITEM.DEF);
		val_now[2] = DataManager.instance.GetItemValue(nowid, ITEM.HP);

		val_new[0] = DataManager.instance.GetItemValue(newid, ITEM.ATK);
		val_new[1] = DataManager.instance.GetItemValue(newid, ITEM.DEF);
		val_new[2] = DataManager.instance.GetItemValue(newid, ITEM.HP);

		for (int i = 0; i < 3; i++)
		{
			if (val_new[i] > val_now[i])
			{
				arrow_up[i].SetActive(true);
				arrow_down[i].SetActive(false);
			}
			else if (val_new[i] < val_now[i])
			{
				arrow_up[i].SetActive(false);
				arrow_down[i].SetActive(true);
			}
			else
			{
				arrow_up[i].SetActive(false);
				arrow_down[i].SetActive(false);
			}
		}
	}
}

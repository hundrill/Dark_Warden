using UnityEngine;

public class UI_ItemReplace : MonoBehaviour
{
    public ItemReplace_Ui item_now;
    public ItemReplace_Ui item_new;
	public string item_new_id;
    ITEMTYPE type;
	private void OnEnable()
	{
        type = EnumUtil<ITEMTYPE>.Parse(CsvManager.instance.GetItem(item_new_id, ITEM.TYPE).ToString().ToUpper());

        string item_now_id = DataManager.instance.Equip[(int)type];
        if (item_now_id != "")
        {
			item_now.gameObject.SetActive(true);

			item_now.SettingItem(item_now_id);            
        }
        else
        {
			item_now.gameObject.SetActive(false);
		}

		item_new.SettingItem(item_new_id);
        item_new.ShowArrow(item_now_id, item_new_id);	
	}

	public void Btn_Sell()
    {
        DataManager.instance.SetMyData(MYDATA.GOLD, 100);

		gameObject.SetActive(false);
    }


    public void Btn_Equip()
    {
        DataManager.instance.EquipItem(type, item_new_id);

		gameObject.SetActive(false);
    }
}

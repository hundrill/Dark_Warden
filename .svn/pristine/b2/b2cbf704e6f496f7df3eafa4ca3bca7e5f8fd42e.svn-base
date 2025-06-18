using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Description : MonoBehaviour
{
	public TextMeshProUGUI txt_Name;
	public TextMeshProUGUI txt_Desc;
	public TextMeshProUGUI txt_Level;
	public GameObject btn_Use;
	GameObject selectCard;

	public void SetDesc(GameObject select , string name , string desc , string level , bool use = false)
	{
		if (txt_Name)
			txt_Name.text = name;

		if (txt_Desc)
			txt_Desc.text = desc;

		if (txt_Level)
			txt_Level.text = level;

		btn_Use.SetActive(use);

		selectCard = select;
	}

	private void Awake()
	{
		if (btn_Use)
		{
			EventTrigger trigger = btn_Use.gameObject.GetComponent<EventTrigger>();
			if (trigger == null)
				trigger = btn_Use.gameObject.AddComponent<EventTrigger>();

			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener((data) => { OnPointerDown_UsePlanet(); });

			trigger.triggers.Add(entry);
		}
	}

	void OnPointerDown_UsePlanet()
	{
		if (selectCard && selectCard.GetComponent<Card_Planet>())
			selectCard.GetComponent<Card_Planet>().Btn_Use();
	}
}

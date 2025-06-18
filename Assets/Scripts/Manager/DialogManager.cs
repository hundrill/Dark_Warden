using UnityEngine;

public enum DIALOG
{
	OVER,
}

public class DialogManager : MonoBehaviour
{
	public GameObject[] list_Dialog;

	public static DialogManager instance;

	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);
	}

	private void Start()
	{
			
	}

	public void ShowDialog(DIALOG type)
	{
		int idx = (int)type;

		if (list_Dialog.Length <= idx)
			return;

		if (list_Dialog[idx])
			Instantiate(list_Dialog[idx]);
	}
}

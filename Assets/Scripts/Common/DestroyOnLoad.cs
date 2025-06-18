using UnityEngine;

public class DestroyOnLoad : MonoBehaviour
{
	private void Awake()
	{
		gameObject.SetActive(false);
	}

	private void Start()
	{
		gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		gameObject.SetActive(false);
	}
}

using UnityEngine;
using TMPro;

public class ui_Damage : MonoBehaviour
{
	public TextMeshProUGUI txt_Damage;

	public static ui_Damage instance;

	public bool is_Dlg_Open;

	public Animation anim;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else if (instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		if (txt_Damage)
			anim = txt_Damage.GetComponent<Animation>();

		if (anim == null || anim.clip == null)
		{
			Debug.LogWarning("Animation or clip not found on " + gameObject.name);
			return;
		}
	}

	public void OnMonsterDamage(int damage)
	{
		if (txt_Damage)
			txt_Damage.gameObject.SetActive(true);

		txt_Damage.text = string.Format("{0}", damage);

		// 애니메이션 재생
		anim.Play();

		// 클립 길이만큼 기다린 후 비활성화
		StartCoroutine(DisableAfterPlay(anim.clip.length));
	}

	private System.Collections.IEnumerator DisableAfterPlay(float delay)
	{
		yield return new WaitForSeconds(delay);
		txt_Damage.gameObject.SetActive(false);
	}
}

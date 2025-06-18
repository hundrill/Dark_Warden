using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static SpawnManager;

public class UI_BossHp : MonoBehaviour
{
    public Image fill;

	private void Start()
	{	
        if (fill)
            fill.fillAmount = 1;

        StartCoroutine(Setting());
    }

    IEnumerator Setting()
    {
        while (true)
        {
            if (SpawnManager.instance)
            {
                SpawnManager.instance.OnDistanceChange += OnDistanceChange;
				break;
            }

            yield return null;
        }
    }

    void OnDistanceChange(float pct)
    {
		if (fill)
			fill.fillAmount = pct;
	}
}

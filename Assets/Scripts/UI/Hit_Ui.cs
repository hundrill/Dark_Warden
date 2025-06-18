using UnityEngine;
using System.Collections;
using static Character;
using UnityEngine.UI;

public class Hit_Ui : MonoBehaviour
{
    public Image hit;

    private void OnEnable()
    {
        hit.gameObject.SetActive(true);        
        //StartCoroutine(Setting());
    }

    IEnumerator Setting()
    {
        while (true)
        {
            if (Character.instance)
            {
                Character.instance.OnHit+= OnHit;
                break;
            }
            yield return null;
        }

    }

    public void OnHit()
    {
        StartCoroutine(HitFrame());
    }

    IEnumerator HitFrame()
    {
        hit.gameObject.SetActive(true);
        float _time = 0;

        while (true)
        {
            _time += Time.deltaTime;

            if(_time > 1)
            {
                hit.gameObject.SetActive(false);
                break;
            }

            yield return null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public int idx_effect;
    public GameObject[] list_Effect;

    public static EffectManager instance { get; set; }

    public delegate void onEffectChanged(string name);
    public onEffectChanged OnEffectChanged;

    private float buttonSaver = 0f;
    private void Awake()
    {
        instance = this;
        idx_effect = 0;
    }

    // Use this for initialization
    void Start()
    {

    }

    public GameObject CreateEffect(int idx, Vector3 point, Transform parent = null)
    {
        /*idx = Random.Range(0, list_Effect.Length);
        idx = idx_effect;*/

        //point = point + new Vector3(0.4f, 0, 0);
        GameObject obj;

        if (parent == null)
            obj = Instantiate(list_Effect[idx], point, Quaternion.identity);
        else
            obj = Instantiate(list_Effect[idx], point, Quaternion.identity, parent);

        return obj;
        //obj.transform.localRotation = Quaternion.Euler(0, 0, 90);
        //Instantiate(effect_Ball[idx], point, Quaternion.Euler(-30,90,0));
    }
/*
    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKey(KeyCode.A) || Input.GetAxis("Horizontal") < 0) && buttonSaver >= 0.2f)// left button
        {
            buttonSaver = 0f;
            Counter(-1);
        }
        if ((Input.GetKey(KeyCode.D) || Input.GetAxis("Horizontal") > 0) && buttonSaver >= 0.2f)// right button
        {
            buttonSaver = 0f;
            Counter(+1);
        }
        buttonSaver += Time.deltaTime;
    }
*/
    void Counter(int count)
    {
        idx_effect += count;
        if (idx_effect > list_Effect.Length - 1)
        {
            idx_effect = 0;
        }
        else if (idx_effect < 0)
        {
            idx_effect = list_Effect.Length - 1;
        }

        if (list_Effect.Length > idx_effect && list_Effect[idx_effect])
            OnEffectChanged.Invoke(list_Effect[idx_effect].gameObject.name);
    }

}

//====================Copyright statement:AppsTools===================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShortRangeSkillsVFXEffectBundle3Demo : MonoBehaviour
{
    private bool isRotation = false;
    string[] strs = null;

    public int i = 0;
    public UnityEngine.UI.Text tex;
    public Transform ts;
    private GameObject currObj;
    public Transform hideParent;

    public void Awake()
    {

        strs = new string[hideParent.childCount];
        for (int j = 0; j < hideParent.childCount; j++)
        {
            strs[j] = hideParent.GetChild(j).gameObject.name;
        }

        currObj = GameObject.Instantiate(hideParent.transform.Find(strs[i]).gameObject);
        currObj.transform.SetParent(ts);
        currObj.transform.localPosition = Vector3.zero;
    }

    public List<T> RandomSortList<T>(List<T> ListT)
    {
        System.Random random = new System.Random();
        List<T> newList = new List<T>();
        foreach (T item in ListT)
        {
            newList.Insert(random.Next(newList.Count + 1), item);
        }
        return newList;
    }


    public void Update()
    {
        if (ts != null && isRotation)
        {
            ts.transform.Rotate(Vector3.up * Time.deltaTime * 90f);
        }

        if(Input.GetKeyDown(KeyCode.F2))
        {
            UnityEngine.GUIUtility.systemCopyBuffer = strs[i];
        }
    }

    public void R()
    {
        isRotation = true;
    }

    public void NotR()
    {
        isRotation = false;
    }

    public void RePlay()
    {
        if (currObj != null)
        {
            currObj.SetActive(false);
            currObj.SetActive(true);
        }
    }

    public void CopyName()
    {
        var s = strs[i].ToLower().Replace(".prefab", "");
        s = s.Substring(s.IndexOf("/") + 1);
        UnityEngine.GUIUtility.systemCopyBuffer = s;
    }

    public void OnLeftBtClick()
    {
        i--;
        if (i <= 0)
        {
            i = strs.Length - 1;
        }
        if (currObj != null)
        {
            GameObject.DestroyImmediate(currObj);
        }
        currObj = GameObject.Instantiate(hideParent.transform.Find(strs[i]).gameObject);
        currObj.transform.SetParent(ts);
        currObj.transform.localPosition = Vector3.zero;
 
    }

    public void OnRightBtClick()
    {
        i++;
        if (i >= strs.Length)
        {
            i = 0;
        }
        if (currObj != null)
        {
            GameObject.DestroyImmediate(currObj);
        }
        currObj = GameObject.Instantiate(hideParent.transform.Find(strs[i]).gameObject);
        currObj.transform.SetParent(ts);
        currObj.transform.localPosition = Vector3.zero;
    }
}
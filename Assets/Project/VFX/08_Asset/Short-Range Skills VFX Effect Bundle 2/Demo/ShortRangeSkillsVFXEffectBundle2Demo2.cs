//====================Copyright statement:AppsTools===================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShortRangeSkillsVFXEffectBundle2Demo2 : MonoBehaviour
{
    string ss = "VFX_Skill_Start_25_Purple&VFX_Skill_Hit_08_Purple&VFX_Skill_Boom_12_Purple&VFX_Skill_Start_03_Purple&VFX_Skill_Start_13_Purple&VFX_Skill_Boom_03_Purple&VFX_Skill_Start_16_Purple&VFX_Skill_Start_21_Purple&VFX_Skill_Fire_02_Purple&VFX_Skill_Start_33_Purple&VFX_Skill_Start_42_Purple&VFX_Skill_Hit_03_Purple&VFX_Skill_Start_19_Purple&VFX_Skill_Boom_17_Purple&VFX_Skill_Boom_01_Purple&VFX_Skill_Hit_11_Purple&VFX_Skill_Boom_11_Purple&VFX_Skill_Boom_07_Purple&VFX_Skill_Start_38_Purple&VFX_Skill_Start_22_Purple&VFX_Skill_Start_29_Purple&VFX_Skill_Boom_05_Purple&VFX_Skill_Start_36_Purple&VFX_Skill_Start_40_Purple&VFX_Skill_Boom_22_Purple&VFX_Skill_Start_18_Purple&VFX_Skill_Start_06_Purple&VFX_Skill_Start_27_Purple&VFX_Skill_Start_32_Purple&VFX_Skill_Boom_21_Purple&VFX_Skill_Start_28_Purple&VFX_Skill_Start_41_Purple&VFX_Skill_Hit_02_Purple&VFX_Skill_Start_12_Purple&VFX_Skill_Start_01_Purple&VFX_Skill_Boom_15_Purple&VFX_Skill_Start_26_Purple&VFX_Skill_Start_07_Purple&VFX_Skill_Hit_04_Purple&VFX_Skill_Boom_14_Purple&VFX_SKill_Start_04_Purple&VFX_Skill_Hit_09_Purple&VFX_Skill_Start_11_Purple&VFX_Skill_Start_20_Purple&VFX_Skill_Start_37_Purple&VFX_Skill_Start_08_Purple&VFX_Skill_Boom_13_Purple&VFX_Skill_Boom_19_Purple&VFX_Skill_Start_02_Purple&VFX_Skill_Start_31_Purple&VFX_Skill_Start_05_Purple&VFX_Skill_Start_35_Purple&VFX_Skill_Fire_01_Purple&VFX_Skill_Start_14_Purple&VFX_Skill_Boom_06_Purple&VFX_Skill_Boom_04_Purple&VFX_Skill_Boom_08_Purple&VFX_Skill_Start_15_Purple&VFX_Skill_Boom_09_Purple&VFX_Skill_Boom_18_Purple&VFX_Skill_Boom_10_Purple&VFX_Skill_Start_34_Purple&VFX_Skill_Hit_01_Purple&VFX_Skill_Boom_02_Purple&VFX_Skill_Start_39_Purple&VFX_Skill_Hit_05_Purple&VFX_Skill_Start_23_Purple&VFX_Skill_Start_24_Purple&VFX_Skill_Hit_07_Purple&VFX_Skill_Hit_10_Purple&VFX_Skill_Hit_06_Purple&VFX_Skill_Boom_16_Purple&VFX_Skill_Start_09_Purple";
    private bool r = false;
    string[] allArray = null;

    public int i = 0;
    public UnityEngine.UI.Text tex;
    public Transform ts;
    private GameObject currObj;
    public Transform hideParent;

    public void Awake()
    {
        allArray = ss.Split('&');
        currObj = GameObject.Instantiate(hideParent.transform.Find(allArray[i]).gameObject);
        currObj.transform.SetParent(ts);
        //currObj.transform.localPosition = Vector3.zero;
        tex.text = "Name: " + i + " 【" + allArray[i] + "】";
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
        if (ts != null && r)
        {
            ts.transform.Rotate(Vector3.up * Time.deltaTime * 90f);
        }
    }

    public void R()
    {
        r = true;
    }

    public void NotR()
    {
        r = false;
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
        var s = allArray[i].ToLower().Replace(".prefab", "");
        s = s.Substring(s.IndexOf("/") + 1);
        UnityEngine.GUIUtility.systemCopyBuffer = s;
    }

    public void OnLeftBtClick()
    {
        i--;
        if (i <= 0)
        {
            i = allArray.Length - 1;
        }
        if (currObj != null)
        {
            GameObject.DestroyImmediate(currObj);
        }
        currObj = GameObject.Instantiate(hideParent.transform.Find(allArray[i]).gameObject);
        currObj.transform.SetParent(ts);
        //currObj.transform.localPosition = Vector3.zero;
        tex.text = "Name: " + i + " 【" + allArray[i] + "】";
    }

    public void OnRightBtClick()
    {
        i++;
        if (i >= allArray.Length)
        {
            i = 0;
        }
        if (currObj != null)
        {
            GameObject.DestroyImmediate(currObj);
        }
        currObj = GameObject.Instantiate(hideParent.transform.Find(allArray[i]).gameObject);
        currObj.transform.SetParent(ts);
        //currObj.transform.localPosition = Vector3.zero;
        tex.text = "Name: " + i + " 【" + allArray[i] + "】";
    }
}
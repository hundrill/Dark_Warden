//====================Copyright statement:AppsTools===================//
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ShortRangeSkillsVFXEffectBundle2Demo1 : MonoBehaviour
{
    string ss = "VFX_Skill_Start_01_Red&VFX_Skill_Start_35_Red&VFX_Skill_Start_09_Red&VFX_Skill_Boom_17_Red&VFX_Skill_Hit_01_Red&VFX_Skill_Boom_13_Red&VFX_Skill_Hit_07_Red&VFX_Skill_Boom_Smoke_01&VFX_Skill_Start_03_Red&VFX_Skill_Boom_02_Red&VFX_Skill_Start_25_Red&VFX_Skill_Start_26_Red&VFX_Skill_Start_41_Red&VFX_Skill_Boom_19_Red&VFX_Skill_Start_40_Red&VFX_Skill_Start_05_Red&VFX_Skill_Start_39_Red&VFX_Skill_Start_18_Red&VFX_Skill_Start_42_Red&VFX_Skill_Boom_05_Red&VFX_Skill_Start_33_Red&VFX_Skill_Start_36_Red&VFX_Skill_Hit_03_Red&VFX_Skill_Start_15_Red&VFX_Skill_Fire_01_Red&VFX_Skill_Start_37_Red&VFX_Skill_Start_19_Red&VFX_Skill_Hit_09_Red&VFX_Skill_Hit_05_Red&VFX_Skill_Fire_02_Red&VFX_Skill_Start_24_Red&VFX_Skill_Start_22_Red&VFX_Skill_Start_17&VFX_Skill_Start_31_Red&VFX_Skill_Boom_07_Red&VFX_Skill_Start_28_Red&VFX_Skill_Hit_08_Red&VFX_Skill_Start_14_Red&VFX_Skill_Start_20_Red&VFX_Skill_Boom_11_Red&VFX_Skill_Boom_01_Red&VFX_Skill_Boom_18_Red&VFX_Skill_Boom_03_Red&VFX_Skill_Start_02_Red&VFX_Skill_Boom_22_Red&VFX_Skill_Hit_06_Red&VFX_Skill_Start_27_Red&VFX_Skill_Boom_10_Red&VFX_Skill_Start_16_Red&VFX_Skill_Start_38_Red&VFX_Skill_Start_34_Red&VFX_Skill_Start_11_Red&VFX_Skill_Hit_11_Red&VFX_Skill_Boom_08_Red&VFX_Skill_Start_21_Red&VFX_Skill_Start_13_Red&VFX_Skill_Start_06_Red&VFX_Skill_Hit_04_Red&VFX_Skill_Start_23_Red&VFX_SKill_Start_04_Red&VFX_Skill_Boom_04_Red&VFX_Skill_Start_29_Red&VFX_Skill_Hit_02_Red&VFX_Skill_Start_07_Red&VFX_Skill_Boom_15_Red&VFX_Skill_Boom_09_Red&VFX_Skill_Start_08_Red&VFX_Skill_Boom_14_Red&VFX_Skill_Hit_10_Red&VFX_Skill_Boom_21_Red&VFX_Skill_Start_10&VFX_Skill_Start_12_Red&VFX_Skill_Boom_16_Red&VFX_Skill_Boom_12_Red&VFX_Skill_Boom_06_Red&VFX_Skill_Start_32_Red";
    private bool r = false;
    string[] allArray = null;

    public int i = 0;
    public UnityEngine.UI.Text tex;
    public Transform ts;
    private GameObject currObj;
    public Transform hideParent;

    public void Awake()
    {

        /*string st2322r = "";
        var allFiles = Directory.GetFiles(Application.dataPath + "/Short-Range Skills VFX Effect Bundle 2/Prefabs", "*.prefab", SearchOption.AllDirectories);

        var newR = RandomSortList(new List<string>(allFiles));
        allFiles = newR.ToArray();

        for (int i = 0; i < allFiles.Length; i++)
        {
            var str = Application.dataPath + "/Short-Range Skills VFX Effect Bundle 2/Prefabs/";
            allFiles[i] = allFiles[i].Replace(@"\", "/").Replace(str.Replace(@"\", "/"), "").Replace(".prefab", "");
            if (allFiles[i].IndexOf("Purple") < 0)
            {
                st2322r += allFiles[i] + "&";
            }

        }

        Debug.LogError(st2322r);

        st2322r = "";
        for (int i = 0; i < allFiles.Length; i++)
        {
            var str = Application.dataPath + "/Short-Range Skills VFX Effect Bundle 2/Prefabs/";
            allFiles[i] = allFiles[i].Replace(@"\", "/").Replace(str.Replace(@"\", "/"), "").Replace(".prefab", "");
            if (allFiles[i].IndexOf("Purple") >= 0)
            {
                st2322r += allFiles[i] + "&";
            }

        }

        Debug.LogError(st2322r);

        return;*/


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
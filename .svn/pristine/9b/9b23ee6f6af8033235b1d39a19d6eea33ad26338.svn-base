using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

using System.Text.RegularExpressions;
public class TextGlobal : MonoBehaviour
{
	// Start is called before the first frame update	
	public string key;
	private void OnEnable()
	//private void Start()
	{
		Refresh();
	}

	public void Refresh()
	{
		string sentence = "";

		if (CsvManager.instance)
		{
			if (key.Contains("$"))
			{
				string[] arrs = key.Split('_');
				string name = arrs[arrs.Length - 1];

				/*if (EnumUtil<WEALTH>.TryParse(name))
					sentence = HeroDataLogic.instance.list_Wealth[(int)EnumUtil<WEALTH>.Parse(name)].ToString();*/
			}
			else
			{
				sentence = (string)CsvManager.instance.GetTextData(key, LANGUAGE.TEXT_KO, false);

				if (sentence == "")
					sentence = key;

				sentence = CheckConvert(sentence);
			}
		}
		
		if (GetComponent<TextMeshProUGUI>())
			GetComponent<TextMeshProUGUI>().text = sentence;
	}

	public static string CheckConvert(string sentence)
	{
		List<string> extractedSentences = ExtractStringsBetweenHashes(sentence);

		for (int i = 0; i < extractedSentences.Count; i++)
		{
			sentence = sentence.Replace("#" + extractedSentences[i] + "#", (ConvertShapString(extractedSentences[i])));
		}

		return sentence;
	}

	static string ConvertShapString(string origin)
	{
		string temp = "";

		object var;

		if (origin.Contains("Csv/"))
		{
			string[] name = origin.Replace("Csv/", "").Split('/');

			if (name[0].Contains("text"))
				return CsvManager.instance.GetTextData(name[1]);
		}
		else
		{
			switch (origin)
			{
			}
		}

		return origin;
	}

	static List<string> ExtractStringsBetweenHashes(string input)
	{
		List<string> extractedStrings = new List<string>();

		// 정규식 패턴을 사용하여 #으로 둘러싸인 문자열을 추출
		MatchCollection matches = Regex.Matches(input, @"#([^#]+)#");

		foreach (Match match in matches)
		{
			string matchedString = match.Groups[1].Value;
			extractedStrings.Add(matchedString);
		}

		return extractedStrings;
	}

}

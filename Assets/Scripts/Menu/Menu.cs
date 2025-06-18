using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class Menu : Editor
{
    [MenuItem("Menu/TestData")]
    public static void SelectMonsterHp()
    {
        // ScriptableObject 파일의 경로 설정
        string assetPath = "Assets/Resources/Scriptable/TESTDATA.asset"; // ScriptableObject 경로
        Object obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

        if (obj != null)
        {
            Selection.activeObject = obj; // 선택 상태로 변경
            EditorGUIUtility.PingObject(obj); // 프로젝트 창에서 강조 표시
        }
        else
        {
            Debug.Log($"Cannot find asset at path: {assetPath}");
        }
    }
}
#endif
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
public class Menu : Editor
{
    [MenuItem("Menu/TestData")]
    public static void SelectMonsterHp()
    {
        // ScriptableObject ������ ��� ����
        string assetPath = "Assets/Resources/Scriptable/TESTDATA.asset"; // ScriptableObject ���
        Object obj = AssetDatabase.LoadAssetAtPath<Object>(assetPath);

        if (obj != null)
        {
            Selection.activeObject = obj; // ���� ���·� ����
            EditorGUIUtility.PingObject(obj); // ������Ʈ â���� ���� ǥ��
        }
        else
        {
            Debug.Log($"Cannot find asset at path: {assetPath}");
        }
    }
}
#endif
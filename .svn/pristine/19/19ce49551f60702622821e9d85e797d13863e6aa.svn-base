using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(IndentAttribute))]
public class IndentDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Indent 수준 가져오기
        IndentAttribute indent = (IndentAttribute)attribute;
        int indentLevel = indent.Level;

        // Indent 수준 적용
        EditorGUI.indentLevel += indentLevel;
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.indentLevel -= indentLevel; // 원래 수준으로 복구
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(IndentAttribute))]
public class IndentDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Indent ���� ��������
        IndentAttribute indent = (IndentAttribute)attribute;
        int indentLevel = indent.Level;

        // Indent ���� ����
        EditorGUI.indentLevel += indentLevel;
        EditorGUI.PropertyField(position, property, label, true);
        EditorGUI.indentLevel -= indentLevel; // ���� �������� ����
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
}
#endif
#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(StringDropdown))]
public class StringDropdownDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Get the StringDropdown class
        SerializedProperty optionsProp = property.FindPropertyRelative("options");
        SerializedProperty selectedOptionProp = property.FindPropertyRelative("selectedOption");

        // Convert options to a string array
        string[] options = new string[optionsProp.arraySize];
        for (int i = 0; i < optionsProp.arraySize; i++)
        {
            options[i] = optionsProp.GetArrayElementAtIndex(i).stringValue;
        }

        // Find the current index of the selected option
        int currentIndex = Mathf.Max(0, System.Array.IndexOf(options, selectedOptionProp.stringValue));

        // Draw the dropdown
        int selectedIndex = EditorGUI.Popup(position, label.text, currentIndex, options);

        // Update the selectedOption if the value changed
        selectedOptionProp.stringValue = options[selectedIndex];

        EditorGUI.EndProperty();
    }
}

#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace NekoPunch.Raindrop
{
    [CustomEditor(typeof(CaptureNormal))]
    public class CatureNormalGUI : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("CreateMap"))
            {
                CaptureNormal script = target as CaptureNormal;
                script.Capture();
            }
        }
    }
}

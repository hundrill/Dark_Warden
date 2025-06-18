using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

namespace NekoPunch.Raindrop
{
    public class RaindropShaderGUI : ShaderGUI
    {
        public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
        {
            Material target = materialEditor.target as Material;

            EditorGUI.BeginChangeCheck();

            bool dynamicDropDir = EditorGUILayout.Toggle("Direction Lerp", target.IsKeywordEnabled("_DIRECTION_LERP"));
            bool raindotEnable = EditorGUILayout.Toggle("Rain Dot Enable", target.IsKeywordEnabled("_DOT_ENABLE"));
            bool raindripEnable = EditorGUILayout.Toggle("Rain Drip Enable", target.IsKeywordEnabled("_DRIP_ENABLE"));
            bool maskEnable = EditorGUILayout.Toggle("Additional Mask Enable", target.IsKeywordEnabled("_MASK_ENABLE"));

            if (EditorGUI.EndChangeCheck())
            {
                if (dynamicDropDir)
                {
                    target.EnableKeyword("_DIRECTION_LERP");
                }
                else
                {
                    target.DisableKeyword("_DIRECTION_LERP");
                }

                if (raindotEnable)
                    target.EnableKeyword("_DOT_ENABLE");
                else
                    target.DisableKeyword("_DOT_ENABLE");

                if (raindripEnable)
                    target.EnableKeyword("_DRIP_ENABLE");
                else
                    target.DisableKeyword("_DRIP_ENABLE");

                if (maskEnable)
                    target.EnableKeyword("_MASK_ENABLE");
                else
                    target.DisableKeyword("_MASK_ENABLE");
            }

            base.OnGUI(materialEditor, properties);
        }
    }
}
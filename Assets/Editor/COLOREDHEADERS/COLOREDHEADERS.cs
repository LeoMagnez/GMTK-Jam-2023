using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Hierarchy Window Group Header
/// http://diegogiacomelli.com.br/unitytips-hierarchy-window-group-header
/// 
/// 
/// 
/// 
/// F: Added some base setups for common categories.
/// </summary>
/// 
#if UNITY_EDITOR
[InitializeOnLoad]
public static class HierarchyWindowGroupHeader
{

    static COLOREDHEADERS_SO colorTitlesData;


    static HierarchyWindowGroupHeader()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;

        colorTitlesData = AssetDatabase.LoadAssetAtPath<COLOREDHEADERS_SO>("Assets/Editor/COLOREDHEADERS/ColoredHeadersData.asset");
            //Resources.Load<COLOREDHEADERS_SO>("ColoredHeadersData");
    }

    static void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    {
        GameObject gameObject = (GameObject)EditorUtility.InstanceIDToObject(instanceID);

        

        if (gameObject != null && gameObject.name.StartsWith("---", StringComparison.Ordinal))
        {
            bool _hasfound = false;

            foreach (COLOREDHEADERS_SO.ColoredTitle cT in colorTitlesData.coloredTitles)
            {

                if (gameObject.name.Contains(cT.name))
                {
                    EditorGUI.DrawRect(selectionRect, cT.color);

                    _hasfound = true;
                    break;
                }
            }
            
            if(_hasfound == false)
                EditorGUI.DrawRect(selectionRect, Color.black);

            EditorGUI.DropShadowLabel(selectionRect, "- " + gameObject.name.Replace("-", "") + " -"/*.ToUpperInvariant()*/);

           


        }
    }
}
#endif
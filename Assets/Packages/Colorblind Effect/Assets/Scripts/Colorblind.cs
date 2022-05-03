// Copyright (c) 2016 Jakub Boksansky, Adam Pospisil - All Rights Reserved
// Colorblind Effect Unity Plugin 1.0
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Wilberforce
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
	[HelpURL("https://projectwilberforce.github.io/colorblind/")]
    [AddComponentMenu("Image Effects/Color Adjustments/Colorblind")]
    public class Colorblind : MonoBehaviour
    {
        // public Parameters  
		public int Type = 0;
		// private Parameters
		public Material colorblindMaterial = Resources.Load("Assets/Packages/Colorblind Effect/Assets/Shaders/Colorblind.mat", typeof(Material)) as Material;

		public void Start()
		{
			 colorblindMaterial = Resources.Load("Assets/Packages/Colorblind Effect/Assets/Shaders/Colorblind.mat", typeof(Material)) as Material;
		}
	}

	// ensure unity editor is present - so it doesn't crash when running built project
	#if UNITY_EDITOR 

	// custom gui for inspector
	[CustomEditor(typeof(Colorblind))]
	public class ColorblindEditor : Editor
	{	
		// names appearing in the dropdown menu
		private readonly GUIContent[] typeTexts = new GUIContent[4] {
			new GUIContent("Normal Vision"),
			new GUIContent("Protanopia"),
			new GUIContent("Deuteranopia"),
			new GUIContent("Tritanopia")
		};
		// label and tooltip for the dropdown menu
		private readonly GUIContent typeLabelContent = new GUIContent("Type:", "Type of color blindness");

		// numbers passed to shader - indices of color-shifting matrices
		private readonly int[] typeInts = new int[4] { 0, 1, 2, 3 };

		// this method contains the custom gui for editor
		override public void OnInspectorGUI()
		{
			// bind the connected script to local variable
			var colorblindScript = target as Colorblind;

            // bind the 'Type' parameter of the Colorblind script to dropdown in GUI
            colorblindScript.Type = EditorGUILayout.IntPopup(typeLabelContent, colorblindScript.Type, typeTexts, typeInts);
			// if user made some changes (selected new value from the dropdown) we have to forward the notification
			if (GUI.changed)
			{
				// mark as dirty
				EditorUtility.SetDirty(target);
                colorblindScript.colorblindMaterial.SetInt("type", colorblindScript.Type);
			}
		}
	}
	#endif
}
/*
 *  Author: Calvin Soueid
 *  Date:   15/11/2021
 */

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Plant.Data;

[CustomEditor(typeof(HelpStep))]
public class HelpStepEditor : Editor
{

    private VisualElement root;

    private HelpStep helpStep;

    public void OnEnable()
    {
        root = new VisualElement();

        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/Editor/Plant Editor/HelpStep/HelpStep.uxml");
        visualTree.CloneTree(root);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/Editor/Plant Editor/HelpStep/HelpStep.uss");
        root.styleSheets.Add(styleSheet);

    }

    public override VisualElement CreateInspectorGUI()
    {
        helpStep = (HelpStep)target;
        

        VisualElement lockedVariables = root.Q("LockedVariables");


        UpdateLockedVariables();

        root.Add(new PropertyField(serializedObject.FindProperty("HintDialogue")));

        Button removeVar = (Button)lockedVariables.Q("RemoveVar");
        Button addVar = (Button)lockedVariables.Q("AddVar");

        removeVar.clickable.clicked += () => 
        { 
            if (helpStep.LockedVariables.Count > 0)
            {
                helpStep.LockedVariables.RemoveAt(helpStep.LockedVariables.Count - 1);
                EditorUtility.SetDirty(target);
                UpdateLockedVariables();
            }
        };

        addVar.clickable.clicked += () =>
        {
            helpStep.LockedVariables.Add(typeof(Plant.SoilDepth));
            EditorUtility.SetDirty(target);
            UpdateLockedVariables();
        };

        root.Add(new PropertyField(serializedObject.FindProperty("IsFailDialogueOverriden")));
        root.Add(new PropertyField(serializedObject.FindProperty("FailDialogue")));

        return root;
    }

    private void UpdateLockedVariables()
    {
        VisualElement lockedVarList = root.Q("LockedVariableList");

        lockedVarList.Clear();

        for (int i = 0; i < helpStep.LockedVariables.Count; i++)
        {
            VariableLock varLock = new VariableLock();
            int index = i;

            varLock.RegisterCallback<ChangeEvent<string>>((e) =>
            {
                helpStep.LockedVariables[index] = varLock.Value;
                EditorUtility.SetDirty(target);
            });
            varLock.Value = helpStep.LockedVariables[i].Value;

            lockedVarList.Add(varLock);
        }
    }
}
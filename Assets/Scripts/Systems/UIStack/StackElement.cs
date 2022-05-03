/*
 *  Author: James Greensill
 *  Date:   25.10.2021
 *  Folder Location: Assets/Scripts/UIStack/StackElement.cs
 */

using UnityEngine;

[System.Serializable]
public class StackElement
{

    /// <summary>
    /// MenuName is used to find the specified menu.
    /// </summary>
    [Header("Name for the Menu: Type String.")]
    [Tooltip("Use this name when using the Show(string name) method.")]
    [SerializeField] public string MenuName;

    /// <summary>
    /// Menu holds the Canvas Object.
    /// </summary>
    [Header("Menu GameObject: Type Canvas.")]
    [Tooltip("Put the GameObject prefab with the Canvas Component attached to it.")]
    [SerializeField] public GameObject Menu;
}
/*
 *  Author: James Greensill
 *  Date:   25.10.2021
 *  Folder Location: Assets/Scripts/UIStack/UserInterfaceStack.cs
 */

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR

using Debug = UnityEngine.Debug;

#endif

public class UserInterfaceStack : MonoBehaviour
{
    public static UserInterfaceStack Instance;

    [Header("Put list of menu prefabs here. (0 = shown menu).")]
    [SerializeField] public List<StackElement> UserInterfaceMenus;

    [SerializeField] private List<GameObject> m_MenuLayers;

    [Header("Methods listening to this event are called when a menu is shown.")]
    [SerializeField] public UnityEvent OnMenuShow;

    [Header("Methods listening to this event are called when a menu is hidden.")]
    [SerializeField] public UnityEvent OnMenuHide;

    [SerializeField] public bool OpenMenuOnAwake = false;
    [SerializeField] public string MenuToOpenOnAwake;

    /// <summary>
    /// Displays a menu based on Input Parameter.
    /// </summary>
    /// <param name="menuName">Input Parameter.</param>
    public void Show(string menuName, bool closeAll)
    {
        // Goes through the list and finds the first element that matches the <string menuName parameter> using lambda.
        var menu = UserInterfaceMenus.Find(stackElement => menuName == stackElement.MenuName);

        // Checks if the menu is null.
        if (menu == null)
        {
            return;
        }

        // Ensure the stack has objects.
        if (closeAll)
            HideAllMenus();

        // Instantiate the referenced menu.
        menu.Menu.gameObject.SetActive(true);
    }

    public void Show(string menuName) => Show(menuName, true);

    /// <summary>
    /// Destroys the topmost menu and shows the previous menu.
    /// </summary>
    public void Hide(string menuName)
    {
        // Set the topmost object to active.
        var menu = UserInterfaceMenus.Find(stackElement => menuName == stackElement.MenuName);

        Debug.Log($"Hiding {menuName}");

        if (menu != null)
            menu.Menu.SetActive(false);
    }

    public void AddLayer(string menuName)
    {
        var menu = UserInterfaceMenus.Find(stackElement => menuName == stackElement.MenuName);

        // Checks if the menu is null.
        if (menu == null)
        {
            return;
        }

        menu.Menu.SetActive(true);
    }

    public void HideAllMenus()
    {
        foreach (var layer in UserInterfaceMenus)
        {
            layer.Menu.gameObject.SetActive(false);
        }

        m_MenuLayers.Clear();
    }

    // Called before any frames.
    public void Awake()
    {
        InitializeFields();
    }

    private void Start()
    {
        //LockedPages.Pages["Ranunculus"] = false;
        //LockedPages.Pages["Waratah"] = false;
        //LockedPages.Pages["Banksia"] = false;

        if (OpenMenuOnAwake)
        {
            Show(MenuToOpenOnAwake, true);
        }
    }

    // Initializes all the variables.
    private void InitializeFields()
    {
        if (Instance != null)
        {
            throw new System.Exception($"Another instance of UserInterfaceStack already exists!");
        }

        Instance = this;

        // Checks if this event is null, if true it creates it as a new event.
        if (OnMenuShow == null)
        {
            OnMenuShow = new UnityEvent();
        }

        // Checks if this event is null, if true it creates it as a new event.
        if (OnMenuHide == null)
        {
            OnMenuHide = new UnityEvent();
        }

        // Creates a new stack based on the UserInterfaceMenus count.
        m_MenuLayers = new List<GameObject>(UserInterfaceMenus.Count);
    }
}
/*
 *  Author: James Greensill
 *  Date: 30/10/2021
 *  Folder Location: Assets/Tools
 *  
 *  Refactor: Calvin Soueid
 *  Date: 08/11/2021
 */

using UnityEngine;
using UnityEngine.Events;

using Interfaces;
using UnityEngine.EventSystems;

public class Interactable : MonoBehaviour, IDisableable
{
    [SerializeField]
    public UnityEvent OnClickDown = new UnityEvent();
    [SerializeField] 
    public UnityEvent OnInteracted = new UnityEvent();
    [SerializeField]
    public UnityEvent OnClickUp = new UnityEvent();

    private bool IsActive = true;

    public void SetActive(bool enabled)
    {
        IsActive = enabled;
    }

    protected virtual void OnMouseDown()
    {
        if (IsActive && !EventSystem.current.IsPointerOverGameObject())
        {
            OnClickDown.Invoke(); 
        }
    }

    protected virtual void OnMouseUp()
    {
        if (IsActive && !EventSystem.current.IsPointerOverGameObject())
        {
            OnClickUp.Invoke(); 
        }
    }

    protected virtual void OnMouseUpAsButton()
    {
        if (IsActive && !EventSystem.current.IsPointerOverGameObject())
        {
            OnInteracted.Invoke(); 
        }
    }


}
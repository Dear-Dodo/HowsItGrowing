/*
 *  Author: James Greensill
 *  Date:   29/10/2021
 *  Folder Location: Assets/Scripts/User Interface/Tooltip
 */

using UnityEngine.EventSystems;

public class TooltipTriggerScreenSpace : TooltipTrigger, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    /// <summary>
    /// Calls base.Hide() when the mouse enters this object via screen space.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        base.Show();
    }

    /// <summary>
    /// Calls base.Hide() when the mouse leaves this object via screen space.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        base.Hide();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        base.Hide();
    }

}
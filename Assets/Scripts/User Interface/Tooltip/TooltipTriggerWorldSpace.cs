/*
 *  Author: James Greensill
 *  Date:   29/10/2021
 *  Folder Location: Assets/Scripts/User Interface/Tooltip
 */

public class TooltipTriggerWorldSpace : TooltipTrigger
{
    /// <summary>
    /// Calls base.Hide() when the mouse enters this object via world space.
    /// </summary>
    private void OnMouseEnter()
    {
        base.Show();
    }

    /// <summary>
    /// Calls base.Hide() when the mouse enters this object via world space.
    /// </summary>
    private void OnMouseExit()
    {
        base.Hide();
    }

    private void OnMouseDown()
    {
        base.Hide();
    }
}
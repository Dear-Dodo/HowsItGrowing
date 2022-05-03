/*
 *  Author: James Greensill
 *  Date:   29/10/2021
 *  Folder Location: Assets/Scripts/User Interface/Tooltip
 */

using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
public class Tooltip : MonoBehaviour
{
    /// <summary>
    /// Header Field or Title Field held by the tooltip template.
    /// </summary>
    [SerializeField] public TextMeshProUGUI HeaderField;

    /// <summary>
    /// Content Field held by the tooltip template.
    /// </summary>
    [SerializeField] public TextMeshProUGUI ContentField;

    /// <summary>
    /// LayoutElement held by the tooltip template.
    /// </summary>
    [SerializeField] public LayoutElement LayoutElement;

    /// <summary>
    /// Character Wrap limit.
    /// </summary>
    [SerializeField] public int CharacterWrapLimit;

    /// <summary>
    /// Rect Transform of this object.
    /// </summary>
    [HideInInspector] private RectTransform m_RectTransform;

    /// <summary>
    /// Uses the tooltip data to set the elements held by the template.
    /// </summary>
    /// <param name="data">Data for the tooltip.</param>
    public void Set(TooltipData data)
    {
        // Sets the header/title.
        HeaderField.text = data.Header;
        // Sets the Content.
        ContentField.text = data.Content;

        // Sets the dimensions of the Tooltip to fit the Data.
        WrapTooltip();
    }

    /// <summary>
    /// Unity's Default Awake: Used for RectTransform Caching.
    /// </summary>
    private void Awake()
    {
        // Caches the RectTransform.
        m_RectTransform = GetComponent<RectTransform>();
    }

    /// <summary>
    /// Unity's Default FixedUpdate: Used to update the position of the Tooltip.
    /// </summary>
    private void Update()
    {
        WrapTooltip();
        PositionTooltip();
    }

    /// <summary>
    /// Sets the position of the tooltip to the Mouse Position and applies a pivot via screen space math.
    /// </summary>
    private void PositionTooltip()
    {
        // Gets the MousePosition.
        Vector2 position = Input.mousePosition;

        // Determines the X Pivot.
        var xPivot = position.x / Screen.width;
        // Determines the Y Pivot.
        var yPivot = position.y / Screen.height;

        // Sets the Pivot.

        Vector3 currentPivot = m_RectTransform.pivot;

        DOTween.To(() => (Vector2)currentPivot, x => currentPivot = x, new Vector2(Mathf.Round(xPivot), Mathf.Round(yPivot)), .1f).OnUpdate(() => { m_RectTransform.pivot = currentPivot; });

        // Sets the Position.
        transform.position = position;
    }

    /// <summary>
    /// Wraps the tooltip text.
    /// </summary>
    private void WrapTooltip()
    {
        var headerLength = HeaderField.text.Length;
        var contentLength = ContentField.text.Length;

        LayoutElement.enabled =
            (headerLength > CharacterWrapLimit || contentLength > CharacterWrapLimit);
    }
}
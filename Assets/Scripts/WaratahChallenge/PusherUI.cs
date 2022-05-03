/*
 * Author: Alex Manning
 * Date: 3/11/2021
 * Folder Location: Assets/Scripts/WaratahChallenge
 */

using UnityEngine;
using UnityEngine.UI;

public class PusherUI : MonoBehaviour
{
    public Canvas Canvas;   //HUD canvas
    public Pusher Pusher;   //Pusher object
    private Image image;    //Image component
    public Image Radius;    //Radius sprite

    private void Start()
    {
        //cache component
        image = gameObject.GetComponent<Image>();
        Cursor.visible = false;

        // Turn Object off when game ends. (James)
        Spawner.Instance.OnGameOver?.AddListener(() => { gameObject.SetActive(false); });
    }

    //project cursor position to canvas and draw radius
    private void Update()
    {
        Vector2 pos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(Canvas.transform as RectTransform, Input.mousePosition, Canvas.worldCamera, out pos);

        this.transform.position = Canvas.transform.TransformPoint(pos);
        image.fillAmount = Pusher.Timer / Pusher.Cooldown;

        Color fade = Radius.color;
        fade.a = (1 - Pusher.Timer / Pusher.Cooldown) * 0.5f;
        if (Pusher.HasObject)
        {
            fade.a += 0.5f;
        }

        Radius.color = fade;
    }
}
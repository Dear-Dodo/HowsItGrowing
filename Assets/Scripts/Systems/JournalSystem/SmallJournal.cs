using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace Journal
{
    public class SmallJournal : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        [SerializeField]
        private TextMeshProUGUI fieldNotes;
        [SerializeField]
        private Image plantImage;
        [SerializeField]
        private Image plantMap;

        [SerializeField]
        private float openXPosition;
        [SerializeField]
        private float closedXPosition;
        [SerializeField]
        private float openCloseTweenDuration = 0.5f;
        
        [SerializeField]
        private float hoverOffset = 50;
        [SerializeField]
        private float hoverTweenDuration = 0.5f;


        private RectTransform rect;

        private bool isOpen;
        private bool isTransitioning;
        private Tweener xPosTween;

        private void Start()
        {
            rect = GetComponent<RectTransform>();

            //xPosTween = DOTween.To(() => rect.anchoredPosition.x, x => rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y), closedXPosition, 0.5f);
        }

        /// <summary>
        /// Set xPosTween to a new x position tween.
        /// </summary>
        /// <param name="target"></param>
        /// <param name="duration"></param>
        private void BuildXTween(float target, float duration)
        {
            xPosTween?.Kill();
            xPosTween = DOTween.To(() => rect.anchoredPosition.x, x => rect.anchoredPosition = new Vector2(x, rect.anchoredPosition.y), target, duration);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            ToggleOpen();
        }

        /// <summary>
        /// Toggle the journal's status.
        /// </summary>
        public void ToggleOpen()
        {
            SetOpen(!isOpen);
        }

        /// <summary>
        /// Set the journal's status to 'status'.
        /// </summary>
        /// <param name="status"></param>
        public void SetOpen(bool status)
        {
            if (status)
            {
                BuildXTween(openXPosition, openCloseTweenDuration);
                isTransitioning = true;
                isOpen = true;
                xPosTween.OnComplete(() => isTransitioning = false);
            }
            else
            {
                BuildXTween(closedXPosition, openCloseTweenDuration);
                isTransitioning = true;
                isOpen = false;
                xPosTween.OnComplete(() => isTransitioning = false);
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!isOpen)
            {
                BuildXTween(closedXPosition + hoverOffset, hoverTweenDuration);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!isOpen)
            {
                BuildXTween(closedXPosition, hoverTweenDuration);
            }
        }

        private void OnEnable()
        {
            SetOpen(isOpen);
        }

        /// <summary>
        /// Set the field notes text value.
        /// </summary>
        /// <param name="plant"></param>
        public void SetPlant(JournalPage plant)
        {
            fieldNotes.text = IO.RichTextHandler.Parse(plant.FieldNotes);
            plantImage.sprite = plant.ScientificImage;
            plantMap.sprite = plant.GardenMap;
        }


    } 
}
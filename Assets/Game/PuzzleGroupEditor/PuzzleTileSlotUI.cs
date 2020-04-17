using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Assets.Game.PuzzleGroupEditor
{
    public class PuzzleTileSlotUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public PuzzleTileType type;

        CanvasGroup canvasGroup;

        [NonSerialized]
        public bool isClone = false;

        Vector2 origin;
        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isClone) return;

            origin = transform.localPosition;

            canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (isClone) return;

            transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isClone) return;

            transform.localPosition = origin;

            canvasGroup.blocksRaycasts = true;
        }

       
    }
}

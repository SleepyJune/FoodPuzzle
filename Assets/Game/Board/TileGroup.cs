using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.EventSystems;

using Assets.Game.Units.TileGroup;

namespace Assets.Game.Board
{
    public class TileGroup : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public List<Tile> tiles = new List<Tile>();

        CanvasGroup canvasGroup;

        Vector2 origin;
        Vector2 displacement;

        [NonSerialized]
        public Tile draggedTile;

        void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void AddTile(Tile tile)
        {            
            tiles.Add(tile);
            tile.parentGroup = this;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            origin = transform.localPosition;
            displacement = new Vector2(transform.position.x, transform.position.y) - eventData.position;
            canvasGroup.blocksRaycasts = false;
            
            if(eventData.pointerCurrentRaycast.gameObject != null)
            {
                draggedTile = eventData.pointerCurrentRaycast.gameObject.transform.GetComponent<Tile>();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position + displacement;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            //transform.localPosition = origin;
            canvasGroup.blocksRaycasts = true;
        }
    }
}

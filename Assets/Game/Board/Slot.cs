using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;

namespace Assets.Game.Board
{
    public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [NonSerialized]
        public Vector2Int position;

        public Image background;

        Tile tile;

        public HashSet<Slot> neighbours = new HashSet<Slot>();

        [NonSerialized]
        public bool recentlyAdded = false;

        public void SetTile(Tile droppedTile)
        {
            if(tile != null)
            {
                Destroy(tile.gameObject);
            }

            if(droppedTile != null)
            {
                var cloneTile = Instantiate(droppedTile, this.transform);
                cloneTile.transform.localPosition = new Vector2(0, 0);
                cloneTile.SetFood(droppedTile.foodObject);

                this.tile = cloneTile;

                recentlyAdded = true;
            }            
        }

        public void OnDrop(PointerEventData eventData)
        {
            var tile = GetDraggedTile(eventData);

            GameManager.instance.tileGroupManager.SetGroupToSlot(tile, this);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            var tile = GetDraggedTile(eventData);

            if(tile != null)
            {
                GameManager.instance.tileGroupManager.CheckTileGroupPositioning(tile, this);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            var tile = GetDraggedTile(eventData);
        }

        Tile GetDraggedTile(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                TileGroup tileGroup = eventData.pointerDrag.transform.GetComponent<TileGroup>();
                if (tileGroup != null && tileGroup.draggedTile != null)
                {
                    var tile = tileGroup.draggedTile;

                    return tile;
                }
            }

            return null;
        }

        void SetSlot(Tile tile)
        {
            this.tile = tile;
        }

        public FoodObject GetFood()
        {
            if(tile != null)
            {
                return tile.foodObject;
            }

            return null;
        }

        public bool isSameTileType(Slot other)
        {
            if(tile != null && other.tile != null)
            {
                if(tile.foodObject != null && tile.foodObject == other.tile.foodObject)
                {
                    return true;
                }
            }

            return false;
        }

        public Tile GetTile()
        {
            return tile;
        }

        public void DestroyTile()
        {
            if(tile != null)
            {
                Destroy(tile.gameObject);
            }
        }

        public bool isEmpty()
        {
            return tile == null;
        }
    }
}

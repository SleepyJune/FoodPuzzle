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
    public class Slot : MonoBehaviour, IDropHandler
    {
        [NonSerialized]
        public Vector2Int position;

        Tile tile;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag != null)
            {
                Tile tile = eventData.pointerDrag.GetComponent<Tile>();
                if (tile != null)
                {
                    SetSlot(tile);
                }
            }
        }
        void SetSlot(Tile tile)
        {
            this.tile = tile;
        }
    }
}

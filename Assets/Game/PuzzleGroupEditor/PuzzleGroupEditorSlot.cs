using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Assets.Game.Board;

namespace Assets.Game.PuzzleGroupEditor
{
    public class PuzzleGroupEditorSlot : Slot, IDropHandler
    {
        PuzzleTileSlotUI currentSlot;

        public PuzzleTileType GetTileType()
        {
            if(currentSlot != null)
            {
                return currentSlot.type;
            }

            return PuzzleTileType.None;
        }

        public void Clear()
        {
            if (currentSlot != null)
            {
                Destroy(currentSlot.gameObject);
                currentSlot = null;
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if(eventData.pointerDrag != null)
            {
                PuzzleTileSlotUI slot = eventData.pointerDrag.GetComponent<PuzzleTileSlotUI>();
                if(slot != null)
                {
                    SetSlot(slot);
                }
            }
        }

        public void SetSlot(PuzzleTileSlotUI slot)
        {
            if (currentSlot != null)
            {
                GameObject.Destroy(currentSlot.gameObject);
            }

            if (slot.type != PuzzleTileType.None)
            {
                var clone = Instantiate(slot, transform);
                clone.transform.position = transform.position;
                currentSlot = clone;
            }

            //Debug.Log(slot.type);
        }
    }
}

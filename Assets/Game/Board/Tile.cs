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
    public class Tile : MonoBehaviour, IPointerEnterHandler
    {
        [NonSerialized]
        public FoodObject foodObject;

        public Image icon;
        public Image background;

        public TileGroup parentGroup;

        [NonSerialized]
        public Vector2Int pos;

        [NonSerialized]
        public bool canFit = false;

        [NonSerialized]
        public Slot slot;

        public void OnPointerEnter(PointerEventData eventData)
        {

        }

        public void SetFood(FoodObject food)
        {
            this.foodObject = food;

            icon.sprite = foodObject.icon;
        }

        public void SetTempSlot(Slot slot)
        {
            if (slot != null)
            {
                this.slot = slot;
                slot.background.color = Color.green;
            }
            else
            {
                //slot.background.color = Color.red;
            }
        }
    }
}

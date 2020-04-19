using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Board
{
    public class BoardManager : MonoBehaviour
    {
        public Transform slotParent;
        public Slot slotPrefab;

        GridLayoutGroup slotParentGridLayoutGroup;

        public int boardWidth = 6;
        public int boardHeight = 6;

        [NonSerialized]
        public Dictionary<Vector2Int, Slot> slots;

        void Start()
        {
            slotParentGridLayoutGroup = slotParent.GetComponent<GridLayoutGroup>();

            InitializeBoard();
        }
        public void InitializeBoard()
        {
            slotParent.DeleteChildren();
            
            slotParentGridLayoutGroup.constraintCount = boardWidth;

            slots = new Dictionary<Vector2Int, Slot>();

            for (int y = boardHeight - 1; y >= 0; y--)
            {
                for(int x = 0;x < boardWidth; x++)
                {
                    var newSlot = Instantiate(slotPrefab, slotParent);
                    newSlot.position = new Vector2Int(x, y);

                    slots.Add(newSlot.position, newSlot);
                }
            }
        }

        public Slot GetSlot(Vector2Int pos)
        {
            Slot slot;
            if(slots.TryGetValue(pos, out slot))
            {
                return slot;
            }

            return null;
        }

        public Slot GetSlot(Slot slot, Vector2Int displacement)
        {
            if(slot != null)
            {
                var pos = slot.position + displacement;

                return GetSlot(pos);
            }

            return null;
        }


    }
}

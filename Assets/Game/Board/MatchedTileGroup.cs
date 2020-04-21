using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

namespace Assets.Game.Board
{
    public class MatchedTileGroup
    {
        public HashSet<Slot> slots;

        public int score;

        public FoodObject food;

        public MatchedTileGroup()
        {
            score = 0;
            //tiles = new HashSet<Tile>();
            slots = new HashSet<Slot>();
        }

        public void AddSlot(Slot slot)
        {
            if (!slots.Contains(slot))
            {
                slots.Add(slot);
            }
        }

        public void RemoveSlot(Slot slot)
        {
            slots.Remove(slot);
        }

        public void SetScore()
        {
            score = slots.Count * 100;
        }

        public bool isValidGroup()
        {
            return slots.Count >= 3 && slots.Count - slots.Count(s => s.recentlyAdded) >= 1;
        }

        public int GetTileCount()
        {
            return slots.Count;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using Assets.Game.PuzzleGroupEditor;

namespace Assets.Game.Units.TileGroup
{   
    [Serializable]
    public class TileGroupSlot
    {
        public Vector2Int pos;

        public PuzzleTileType slotType;

        public TileGroupSlot(Vector2Int pos, PuzzleTileType type)
        {
            this.pos = pos;
            this.slotType = type;
        }
    }
}

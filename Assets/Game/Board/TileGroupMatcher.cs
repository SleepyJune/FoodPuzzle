using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Game.Board
{
    public class TileGroupMatcher
    {
        BoardManager boardManager;

        HashSet<Slot> checkedSlots = new HashSet<Slot>();
        HashSet<Slot> matchedSlots = new HashSet<Slot>();

        //MatchTile[,] tiles;

        public TileGroupMatcher()
        {
            this.boardManager = GameManager.instance.boardManager;
        }

        public List<MatchedTileGroup> CheckMatch()
        {
            checkedSlots = new HashSet<Slot>();
            var matchedGroups = new List<MatchedTileGroup>();

            for (int y = 0; y < boardManager.boardHeight; y++)
            {
                for (int x = 0; x < boardManager.boardWidth; x++)
                {
                    var tile = boardManager.GetSlot(x, y);

                    if (tile != null && !checkedSlots.Contains(tile))
                    {
                        var group = GetSameNeighbours(tile);

                        if (group.isValidGroup())
                        {
                            TrimInvalidSlots(group);

                            if (group.isValidGroup())
                            {
                                group.SetScore();
                                matchedGroups.Add(group);
                            }
                        }

                        checkedSlots.Add(tile);
                    }
                }
            }

            return matchedGroups;
        }

        public MatchedTileGroup CheckMatch(Slot slot)
        {
            var group = GetSameNeighbours(slot);

            if (group.isValidGroup())
            {
                TrimInvalidSlots(group);

                if (group.isValidGroup())
                {
                    return group;
                }
            }

            return null;
        }

        MatchedTileGroup GetSameNeighbours(Slot slot)
        {
            MatchedTileGroup group = new MatchedTileGroup();

            HashSet<Slot> closedSet = new HashSet<Slot>();
            HashSet<Slot> openSet = new HashSet<Slot>();

            openSet.Add(slot);

            while (openSet.Count > 0)
            {
                var current = openSet.First();

                openSet.Remove(current);
                closedSet.Add(current);

                if (current.isEmpty())
                {
                    continue;
                }

                group.AddSlot(current);

                checkedSlots.Add(current);

                foreach (var neighbour in current.neighbours)
                {
                    if (neighbour != null)
                    {
                        if (!closedSet.Contains(neighbour) && neighbour.isSameTileType(current))
                        {
                            //UnityEngine.Debug.Log(neighbour.GetPositionString() + neighbour.tileType);

                            openSet.Add(neighbour);
                        }
                    }
                }
            }

            return group;
        }

        void TrimInvalidSlots(MatchedTileGroup group)
        {
            List<Slot> invalidSlots = new List<Slot>();

            foreach (var slot in group.slots)
            {
                if (!(CheckHorizontal(slot, group) || CheckVertical(slot, group)))
                {
                    invalidSlots.Add(slot);
                }
            }

            foreach (var slot in invalidSlots)
            {
                group.RemoveSlot(slot);
            }
        }

        bool CheckHorizontal(Slot slot, MatchedTileGroup group)
        {
            int numMatched = 1;

            //right
            for (int i = 1; i < 3; i++)
            {
                var testTile = boardManager.GetSlot(slot.position.x + i, slot.position.y);
                if (testTile != null && group.slots.Contains(testTile))
                {
                    numMatched += 1;

                    if (numMatched >= 3)
                    {
                        return true;
                    }
                }
            }


            //left
            for (int i = 1; i < 3; i++)
            {
                var testTile = boardManager.GetSlot(slot.position.x - i, slot.position.y);
                if (testTile != null && group.slots.Contains(testTile))
                {
                    numMatched += 1;

                    if (numMatched >= 3)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        bool CheckVertical(Slot slot, MatchedTileGroup group)
        {
            int numMatched = 1;

            //down
            for (int i = 1; i < 3; i++)
            {
                var testTile = boardManager.GetSlot(slot.position.x, slot.position.y + i);
                if (testTile != null && group.slots.Contains(testTile))
                {
                    numMatched += 1;

                    if (numMatched >= 3)
                    {
                        return true;
                    }
                }
            }


            //up
            for (int i = 1; i < 3; i++)
            {
                var testTile = boardManager.GetSlot(slot.position.x, slot.position.y - i);
                if (testTile != null && group.slots.Contains(testTile))
                {
                    numMatched += 1;

                    if (numMatched >= 3)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}

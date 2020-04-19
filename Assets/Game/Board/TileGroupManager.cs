using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using Assets.Game.Units.TileGroup;

namespace Assets.Game.Board
{
    public class TileGroupManager : MonoBehaviour
    {
        public TileGroup tileGroupPrefab;
        public Transform tileGroupParent;

        public Tile tilePrefab;

        const int slotSize = 140;

        BoardManager boardManager;

        List<TileGroupTemplate> templates = new List<TileGroupTemplate>();
        void Start()
        {
            boardManager = GameManager.instance.boardManager;

            Initialize();
        }

        void Initialize()
        {
            var database = GameManager.instance.databaseManager;

            tileGroupParent.DeleteChildren();

            foreach(var template in database.tileGroupTemplates.Values)
            {
                templates.Add(template);

                GenerateTileGroup(template);
            }
        }

        public void SetGroup(Tile draggedTile, Slot pointerSlot)
        {
            var tileGroup = draggedTile.parentGroup;

            CheckTileGroupPositioning(draggedTile, pointerSlot);

            foreach (var tile in tileGroup.tiles)
            {
                if(tile.slot != null)
                {
                    tile.slot.SetTile(tile);
                }
            }

            Destroy(tileGroup.gameObject);
        }
        public bool CheckTileGroupPositioning(Tile draggedTile, Slot pointerSlot)
        {            
            foreach(var slot in boardManager.slots.Values)
            {
                slot.background.color = Color.white;
            }

            var tileGroup = draggedTile.parentGroup;

            bool canFitGroup = true;

            foreach (var tile in tileGroup.tiles)
            {
                var displacement = tile.pos - draggedTile.pos;
                var slot = boardManager.GetSlot(pointerSlot, displacement);

                tile.slot = null;

                if (slot != null)
                {
                    if (!slot.isEmpty())
                    {
                        slot.background.color = Color.red;
                        slot = null;
                    }
                }

                tile.SetTempSlot(slot);

                if(slot == null)
                {
                    canFitGroup = false;
                }
            }

            return canFitGroup;
        }

        TileGroup GenerateTileGroup(TileGroupTemplate template)
        {           
            TileGroup newTileGroup = Instantiate(tileGroupPrefab, tileGroupParent);

            Dictionary<int, FoodObject> foodObjects = new Dictionary<int, FoodObject>();

            foreach(var slot in template.slots)
            {
                var newTile = Instantiate(tilePrefab, newTileGroup.transform);
                newTile.transform.localPosition = new Vector2(slotSize * slot.pos.x, slotSize * slot.pos.y);
                newTile.pos = slot.pos;
                
                newTileGroup.AddTile(newTile);
                
                int foodIndex = (int)slot.slotType - 2;

                FoodObject food;

                if (foodIndex >= 0)
                {
                    if (!foodObjects.TryGetValue(foodIndex, out food))
                    {
                        food = GameManager.instance.foodManager.GenerateRandomFood();
                        foodObjects.Add(foodObjects.Count, food);
                    }
                }
                else
                {
                    food = GameManager.instance.foodManager.GenerateRandomFood();
                }

                newTile.SetFood(food);
            }

            return null;
        }
    }
}

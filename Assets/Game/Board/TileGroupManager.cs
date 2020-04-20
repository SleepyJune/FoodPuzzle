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

                //GenerateTileGroup(template);
            }

            GenerateRandomTileGroup();
        }

        public void SetGroupToSlot(Tile draggedTile, Slot pointerSlot)
        {
            if (pointerSlot != null && draggedTile != null)
            {
                var tileGroup = draggedTile.parentGroup;

                var canFit = CheckTileGroupPositioning(draggedTile, pointerSlot);

                if (canFit)
                {
                    foreach (var tile in tileGroup.tiles)
                    {
                        if (tile.slot != null)
                        {
                            tile.slot.SetTile(tile);
                        }
                    }

                    Destroy(tileGroup.gameObject);

                    GenerateRandomTileGroup();
                }
                else
                {
                    //doesn't fit
                }
            }

            ResetSlotColors();
        }

        public void ResetSlotColors()
        {
            foreach (var slot in boardManager.slots.Values)
            {
                slot.background.color = Color.white;
            }
        }

        public bool CheckTileGroupPositioning(Tile draggedTile, Slot pointerSlot)
        {
            ResetSlotColors();

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
                        if(slot.GetFood() != tile.foodObject)
                        {
                            slot.background.color = Color.red;
                            slot = null;
                        }                        
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

        void GenerateRandomTileGroup()
        {
            var random = templates.GenerateRandomElement();

            GenerateTileGroup(random);
        }

        TileGroup GenerateTileGroup(TileGroupTemplate template)
        {           
            TileGroup newTileGroup = Instantiate(tileGroupPrefab, tileGroupParent);

            Dictionary<int, FoodObject> foodObjects = new Dictionary<int, FoodObject>();

            float min_x = 9999, max_x = 0, min_y = 9999, max_y = 0;

            foreach(var slot in template.slots)
            {
                var newTile = Instantiate(tilePrefab, newTileGroup.tileParent);
                newTile.transform.localPosition = new Vector2(slotSize * slot.pos.x, slotSize * slot.pos.y);
                newTile.pos = slot.pos;
                
                min_x = Math.Min(min_x, newTile.transform.localPosition.x);
                min_y = Math.Min(min_y, newTile.transform.localPosition.y);
                max_x = Math.Max(max_x, newTile.transform.localPosition.x);
                max_y = Math.Max(max_y, newTile.transform.localPosition.y);

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

            var tileParentDisp = new Vector2(-min_x - (max_x - min_x)/2f, -min_y - (max_y - min_y)/2f);

            Debug.Log(tileParentDisp);
            newTileGroup.tileParent.localPosition = tileParentDisp;

            return null;
        }
    }
}

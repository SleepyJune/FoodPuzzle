using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using UnityEditor;

using Assets.Game.Units.TileGroup;
using System.IO;

namespace Assets.Game.PuzzleGroupEditor
{
    public enum PuzzleTileType
    {
        None = 0,
        Random = 1,
        Group1 = 2,
        Group2 = 3,
        Group3 = 4,
    }
    public class PuzzleGroupEditor : MonoBehaviour
    {
        public Transform slotTypeParent;

        public Transform slotParent;
        public PuzzleGroupEditorSlot slotPrefab;

        public LoadButtonController loadButtonController;

        public InputField saveButtonInput;

        GridLayoutGroup slotParentGridLayoutGroup;

        public int boardWidth = 6;
        public int boardHeight = 6;

        [NonSerialized]
        public Dictionary<Vector2Int, PuzzleGroupEditorSlot> slots;

        [NonSerialized]
        public Dictionary<PuzzleTileType, PuzzleTileSlotUI> slotTypes = new Dictionary<PuzzleTileType, PuzzleTileSlotUI>();

        void Start()
        {
            slotParentGridLayoutGroup = slotParent.GetComponent<GridLayoutGroup>();

            InitializeSlotTypes();

            InitializeBoard();
        }

        void InitializeSlotTypes()
        {
            foreach(Transform child in slotTypeParent)
            {
                var slot = child.GetComponent<PuzzleTileSlotUI>();
                if(slot != null)
                {
                    slotTypes.Add(slot.type, slot);
                }
            }
        }

        public void OnSaveButtonPressed()
        {
            TileGroupTemplate newTemplate = new TileGroupTemplate();

            string name = saveButtonInput.text;

            if(name != "")
            {
                newTemplate.name = saveButtonInput.text;
            }
            else
            {
                newTemplate.name = "template";
            }

            List<TileGroupSlot> slotList = new List<TileGroupSlot>();
            foreach (var slot in slots.Values)
            {
                var newSlot = new TileGroupSlot(slot.position, slot.GetTileType());

                if(newSlot.slotType != PuzzleTileType.None)
                {
                    slotList.Add(newSlot);
                }
            }

            newTemplate.slots = slotList.ToArray();

            //AssetDatabase.CreateAsset(newTemplate, "Prefabs/GameDataObjects/" + newTemplate.name + ".asset");

            string jsonData = JsonUtility.ToJson(newTemplate);

            var path = Application.dataPath + "\\Resources\\TileGroupTemplates\\";

            //Debug.Log(Application.dataPath);
            
            File.WriteAllText(path + newTemplate.name + ".json", jsonData);


        }

        public void Load(TileGroupTemplate template)
        {
            if (template == null) return;

            Clear();

            foreach (var slot in template.slots) 
            {
                PuzzleGroupEditorSlot boardSlot;
                if(slots.TryGetValue(slot.pos, out boardSlot))
                {
                    boardSlot.SetSlot(slotTypes[slot.slotType]);
                }
            }

        }

        public void Clear()
        {
            foreach(var slot in slots.Values)
            {
                slot.Clear();
            }
        }

        public void OnLoadButtonPressed()
        {
            loadButtonController.gameObject.SetActive(true);
        }
        void InitializeBoard()
        {
            slotParent.DeleteChildren();

            slotParentGridLayoutGroup.constraintCount = boardWidth;

            slots = new Dictionary<Vector2Int, PuzzleGroupEditorSlot>();

            for (int y = 0; y < boardHeight; y++)
            {
                for (int x = 0; x < boardWidth; x++)
                {
                    var newSlot = Instantiate(slotPrefab, slotParent);
                    newSlot.position = new Vector2Int(x, y);

                    slots.Add(newSlot.position, newSlot);
                }
            }
        }
    }
}

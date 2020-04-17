using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

using UnityEngine;
using UnityEditor;

using System.Linq;

using Assets.Game.Units.TileGroup;

class PuzzleTileGroupTemplateImporter : AssetPostprocessor
{
    static GameDatabase database;

    class AssetInfo
    {
        public string[] importedAssets;
        public string[] deletedAssets;
        public string[] movedAssets;
        public string[] movedFromAssetPaths;

        public HashSet<string> importedSet = new HashSet<string>();

        public void MakeDictionary()
        {
            importedAssets.ToList().ForEach(x => importedSet.Add(x));
        }
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        if (database == null)
        {
            database = (GameDatabase)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameDataObjects/GameDatabase.asset", typeof(GameDatabase));
        }

        var info = new AssetInfo()
        {
            importedAssets = importedAssets,
            deletedAssets = deletedAssets,
            movedAssets = movedAssets,
            movedFromAssetPaths = movedFromAssetPaths,
        };

        info.MakeDictionary();

        AddTemplate("/Resources/TileGroupTemplates/", ref database.allObjects, ".json", info);

    }

    static void AddTemplate<T>(string path, ref T[] collection, string searchPattern, AssetInfo info) where T : UnityEngine.Object, IGameDataObject
    {
        foreach (string str in info.importedAssets)
        {            
            if (str.StartsWith("Assets" + path) && str.EndsWith(searchPattern))
            {
                TileGroupTemplate newTemplate = ScriptableObject.CreateInstance<TileGroupTemplate>();

                var loadPath = str.Replace("Assets/Resources/", "").Replace(searchPattern,"");
                //Debug.Log(loadPath);

                var textAsset = Resources.Load<TextAsset>(loadPath);

                if (textAsset != null)
                {
                    JsonUtility.FromJsonOverwrite(textAsset.text, newTemplate);

                    var assetPath = "Assets/Prefabs/GameDataObjects/TileGroupTemplates/" + newTemplate.name + ".asset";

                    AssetDatabase.CreateAsset(newTemplate, assetPath);

                    //Debug.Log("Imported: " + str);
                }
            }

        }
    }
}
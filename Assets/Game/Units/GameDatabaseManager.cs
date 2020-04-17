using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using System.Linq;

using Assets.Game.Units.TileGroup;

public class GameDatabaseManager : MonoBehaviour
{
    public GameDatabase database;

    public Dictionary<int, GameDataObject> allObjects = new Dictionary<int, GameDataObject>();
    public Dictionary<int, GameDataPrefab> allPrefabs = new Dictionary<int, GameDataPrefab>();

    public Dictionary<int, FoodObject> foodObjects = new Dictionary<int, FoodObject>();
    public Dictionary<int, FoodRecipeObject> recipeObjects = new Dictionary<int, FoodRecipeObject>();

    public Dictionary<int, TileGroupTemplate> tileGroupTemplates = new Dictionary<int, TileGroupTemplate>();

    private void Awake()
    {
        InitializeObjectDictionary();
        InitializePrefabDictionary();

        if(GameManager.instance != null)
        {
            GameManager.instance.Initialize();
        }
    }

    void InitializePrefabDictionary()
    {
        foreach (var prefab in database.allPrefabs)
        {
            allPrefabs.Add(prefab.id, prefab);
        }
    }

    void InitializeObjectDictionary()
    {
        foreach (var obj in database.allObjects)
        {
            if (obj is FoodObject)
            {
                foodObjects.Add(obj.id, obj as FoodObject);
            }

            if (obj is FoodRecipeObject)
            {
                recipeObjects.Add(obj.id, obj as FoodRecipeObject);
            }

            if (obj is TileGroupTemplate)
            {
                tileGroupTemplates.Add(obj.id, obj as TileGroupTemplate);
            }

            allObjects.Add(obj.id, obj);
        }
    }

    public List<T> GetAllObjects<T>(int id) where T : GameDataObject
    {        
        return allObjects.Values.OfType<T>().ToList();
    }

    public T GetPrefab<T>(int id) where T : GameDataPrefab
    {
        GameDataPrefab prefab;
        if (allPrefabs.TryGetValue(id, out prefab))
        {
            var ret = prefab as T;

            if(ret != null)
            {
                return ret;
            }
        }

        return null;
    }

    public T GetObject<T>(int id) where T : GameDataObject
    {
        GameDataObject obj;
        if (allObjects.TryGetValue(id, out obj))
        {
            var ret = obj as T;

            if (ret != null)
            {
                return ret;
            }
        }

        return null;
    }
}
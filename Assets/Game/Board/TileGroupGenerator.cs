using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using Assets.Game.Units.TileGroup;

namespace Assets.Game.Board
{
    public class TileGroupGenerator : MonoBehaviour
    {
        List<TileGroupTemplate> templates = new List<TileGroupTemplate>();

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            var database = GameManager.instance.databaseManager;
            
            foreach(var template in database.tileGroupTemplates.Values)
            {
                templates.Add(template);
            }
        }

        void GenerateTileGroup()
        {

        }
    }
}

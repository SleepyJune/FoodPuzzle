using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using Assets.Game.Units.TileGroup;

namespace Assets.Game.PuzzleGroupEditor
{
    public class LoadButtonController : MonoBehaviour
    {
        public LoadButton loadButtonPrefab;
        public Transform buttonParent;

        public GameDatabaseManager databaseManager;

        public PuzzleGroupEditor editorManager;

        void Start()
        {
            Initialize();
        }

        void Initialize()
        {
            buttonParent.DeleteChildren();

            var texts = Resources.LoadAll("TileGroupTemplates", typeof(TextAsset));

            foreach(var text in texts)
            {
                TextAsset textAsset = text as TextAsset;

                TileGroupTemplate template = new TileGroupTemplate();

                //Debug.Log(textAsset.text);

                JsonUtility.FromJsonOverwrite(textAsset.text, template);

                if(template != null)
                {
                    var newButton = Instantiate(loadButtonPrefab, buttonParent);
                    newButton.Initialize(template, this);
                }
            }

            /*foreach(var group in databaseManager.tileGroupTemplates.Values)
            {
                var newButton = Instantiate(loadButtonPrefab, buttonParent);
                newButton.Initialize(group);
            }*/
        }

        public void OnTemplateButtonPressed(TileGroupTemplate template)
        {
            editorManager.Load(template);
            this.gameObject.SetActive(false);
        }

        public void OnCloseButtonPressed()
        {
            this.gameObject.SetActive(false);
        }
    }
}

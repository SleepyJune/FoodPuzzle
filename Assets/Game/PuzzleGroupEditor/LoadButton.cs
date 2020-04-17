using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

using Assets.Game.Units.TileGroup;

namespace Assets.Game.PuzzleGroupEditor
{
    public class LoadButton : MonoBehaviour
    {
        public Text text;

        TileGroupTemplate template;

        LoadButtonController controller;

        public void Initialize(TileGroupTemplate group, LoadButtonController controller)
        {
            text.text = group.name;
            this.template = group;
            this.controller = controller;
        }

        public void OnButtonPress()
        {
            controller.OnTemplateButtonPressed(template);
        }
    }
}

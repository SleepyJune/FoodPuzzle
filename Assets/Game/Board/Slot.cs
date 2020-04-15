﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.UI;

namespace Assets.Game.Board
{
    public class Slot : MonoBehaviour
    {
        [NonSerialized]
        public Vector2 position;

        [NonSerialized]
        public FoodObject foodObject;

        public Image icon;
    }
}

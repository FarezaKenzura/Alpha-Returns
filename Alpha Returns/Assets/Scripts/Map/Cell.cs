using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlphaReturn.Map
{
    public class Cell
    {
        public bool isWater;
        public bool hasObstacle;

        public Cell(bool isWater)
        {
            this.isWater = isWater;
            this.hasObstacle = false;
        }
    }
}
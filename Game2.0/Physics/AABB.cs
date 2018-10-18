using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game1.Physics
{
    class AABB
    {
        public Vector2 min { get; set; }
        public Vector2 max { get; set; }

        public AABB()
        {
            min = new Vector2(0, 0);
            max = new Vector2(0, 0);
        }
    }
}

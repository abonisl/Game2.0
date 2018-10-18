using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game1.Physics
{
    class Box
    {
        AABB aabb;

        public Box()
        {
            aabb = new AABB();
        }

        public void UpdateCollisionBox(Vector2 maxAABB, Vector2 minAABB)
        {    
            aabb.max = maxAABB;
            aabb.min = minAABB;
        }

        public Vector2 GetAABBmin()
        {
            return this.aabb.min;
        }

        public Vector2 GetAABBmax()
        {
            return this.aabb.max;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Game1.Physics
{
    static class World
    {
        public static Vector2 Gravity = new Vector2(0.0f, 9.0f);
        public static float friction = 0.5f;

        // takes object force and return it with friciton
        public static Vector2 ApplyFriction(Vector2 objectForce, float mass)
        {
            if (objectForce.X > 0)
            {
                objectForce.X -= friction * mass * Gravity.Y;
                if (objectForce.X < 0)
                    objectForce.X = 0;
            }
            else if (objectForce.X < 0)
            {
                objectForce.X += friction * mass * Gravity.Y;
                if (objectForce.X > 0)
                    objectForce.X = 0;
            }

            return objectForce;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class StaticSprite : Sprite
    {
        // non animated sprite
        public StaticSprite(Texture2D textureImage, Vector2 position, Point frameSize)
            : base(textureImage, position, frameSize)
        {
            CollisionBox.UpdateCollisionBox(
                new Vector2(position.X, position.Y), // top left vertex (AABB max)
                new Vector2(position.X + frameSize.X, position.Y + frameSize.Y)); // bottom right vertex (AABB min)
        }

        // animated sprite
        public StaticSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize)
            : base(textureImage, position, frameSize, currentFrame, sheetSize)
        {
            CollisionBox.UpdateCollisionBox(
    new Vector2(position.X, position.Y), // top left vertex (AABB max)
    new Vector2(position.X + frameSize.X, position.Y + frameSize.Y)); // bottom right vertex (AABB min)
        }

        public StaticSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, currentFrame, sheetSize, millisecondsPerFrame)
        {
            CollisionBox.UpdateCollisionBox(
    new Vector2(position.X, position.Y), // top left vertex (AABB max)
    new Vector2(position.X + frameSize.X, position.Y + frameSize.Y)); // bottom right vertex (AABB min)
        }

        public override Vector2 Direction
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Point GetFrameSize
        {
            get
            {
                return frameSize;
            }
        }
    }
}

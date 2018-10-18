using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2._0
{
    class GroundTile
    {
        Point position;
        Texture2D textureImage;
        public Rectangle collisionBox;

        public GroundTile(Point position, Point size,Texture2D textureImage)
        {
            this.position = position;
            this.textureImage = textureImage;
            collisionBox = new Rectangle(position, size);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            spriteBatch.Draw(textureImage, new Vector2(position.X, position.Y), Color.White);
            spriteBatch.End();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Game1.Physics;

namespace Game1
{
    abstract class Sprite
    {
        protected Point frameSize;

        Texture2D textureImage;
        Point currentFrame;
        Point sheetSize;
        public Vector2 position;
        public float speed;
        public abstract Vector2 Direction { get; }
        public abstract Point GetFrameSize { get; }
        int timeSinceLastFrame = 0;
        const int defaultMillisecondsPerFrame = 16;
        int millisecondsPerFrame;
        public Box CollisionBox { get; }

        //_______________ Static Sprites ____________________

        // non animated sprite
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            CollisionBox = new Box();
        }


        // animated sprite
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.millisecondsPerFrame = defaultMillisecondsPerFrame;
            CollisionBox = new Box();
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.millisecondsPerFrame = millisecondsPerFrame;
            CollisionBox = new Box();
        }

        //_______________ Dynamic Sprites ___________________

        // non animated sprite
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, float speed)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.speed = speed;
            CollisionBox = new Box();
        }


        // animated sprite
        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, float speed)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = defaultMillisecondsPerFrame;
            CollisionBox = new Box();
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, float speed, int millisecondsPerFrame)
        {
            this.textureImage = textureImage;
            this.position = position;
            this.frameSize = frameSize;
            this.currentFrame = currentFrame;
            this.sheetSize = sheetSize;
            this.speed = speed;
            this.millisecondsPerFrame = millisecondsPerFrame;
            CollisionBox = new Box();
        }

        public Sprite(Texture2D textureImage, Vector2 position, Point frameSize, float speed, Point sheetSize) : this(textureImage, position, frameSize, speed)
        {
            this.sheetSize = sheetSize;
        }

        public virtual void Update(GameTime gameTime, Rectangle clientBounds)
        {
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;

            // animation logic
            if( timeSinceLastFrame > millisecondsPerFrame)
            {
                timeSinceLastFrame = 0;
                ++currentFrame.X;

                if(currentFrame.X >= sheetSize.X)
                {
                    currentFrame.X = 0;
                    ++currentFrame.Y;

                    if(currentFrame.Y >= sheetSize.Y)
                    {
                        currentFrame.Y = 0;
                    }
                }
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(textureImage, position, new Rectangle(
                (currentFrame.X * frameSize.X), 
                (currentFrame.Y * frameSize.Y), 
                frameSize.X, frameSize.Y), 
                Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);
        }
    }
}

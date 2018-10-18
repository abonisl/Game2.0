using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1.Physics;

namespace Game2._0
{
    class Player
    {
        // Collision
        Rectangle collisionBox;
        // Movement
        Vector2 position;
        float moveSpeed;
        // Texture
        Texture2D textureImage;
        // Animation
        private int rows = 4;
        private int columns = 4;
        private int currentFrame = 0;
        private int totalFrames;
        // Animation speed control
        private int timeSinceLastFrame = 0;
        private int msPerFrame = 50;
        public Player(Vector2 startingPosition, Texture2D textureImage, float moveSpeed)
        {
            this.position = startingPosition;
            collisionBox = new Rectangle((int)position.X, (int)position.Y, 100, 100);
            this.textureImage = textureImage;
            this.moveSpeed = moveSpeed;
            totalFrames = rows * columns;
        }

        public void Move(Vector2 Direction)
        {
            position += Direction * moveSpeed;
        }

        public void DrawUpdate(SpriteBatch spriteBatch)
        {
            int Width = textureImage.Width / columns;
            int Heigth = textureImage.Height / rows;
            int row = (int)((float)currentFrame / (float)columns);
            int column = currentFrame % columns;

            Rectangle sourceRectangle = new Rectangle(Width * column, Heigth * row, Width, Heigth);
            Rectangle destinationRectangle = new Rectangle((int)position.X,(int)position.Y,Width,Heigth);

            spriteBatch.Begin();
            spriteBatch.Draw(textureImage,destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }

        public void PlayerUpdate(GameTime gameTime)
        {
            // Collision box position update
            collisionBox.Location = new Point((int)position.X, (int)position.Y);
            // Movement and enviroment effects
            position += World.Gravity;
            foreach(GroundTile g in World.groundTilesList)
            {
                // if true deal with collision
                if(collisionBox.Intersects(g.collisionBox))
                {
                    //player is colliding with top side
                    if (collisionBox.Bottom > g.collisionBox.Top && collisionBox.Bottom < g.collisionBox.Bottom)
                    {
                        position.Y = g.collisionBox.Top - 50;
                        return;
                    }
                    //// player is colliding with down side
                    //if (collisionBox.Top < g.collisionBox.Bottom && collisionBox.Top > g.collisionBox.Top)
                    //{
                    //    position.Y = collidingSprite.position.Y + GetFrameSize.Y;
                    //    return;
                    //}
                    //// player is colliding with right side
                    //if (collisionBox.Left > g.collisionBox.Right && collisionBox.Left < g.collisionBox.Left)
                    //{
                    //    position.X = collidingSprite.position.X - GetFrameSize.X;
                    //    return;
                    //}
                    //// player is colliding with left side
                    //if (collisionBox.Right < g.collisionBox.Left && collisionBox.Right > g.collisionBox.Right)
                    //{
                    //    position.X = collidingSprite.position.X + collidingSprite.GetFrameSize.X;
                    //    return;
                    //}
                }
            }

            // Animation Update
            timeSinceLastFrame += gameTime.ElapsedGameTime.Milliseconds;
            if(timeSinceLastFrame > msPerFrame)
            {
                timeSinceLastFrame -= msPerFrame;

                currentFrame++;
                timeSinceLastFrame = 0;
                if(currentFrame == totalFrames)
                {
                    currentFrame = 0;
                }
            }
        }
    }
}

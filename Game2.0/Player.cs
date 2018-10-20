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
        List<GroundTile> collisionList;
        // Movement
        Vector2 position;
        float moveSpeed;
        // Falling / jumping
        public bool isFalling = true;
        public bool isJumping = false;
        int jumpSpeed = 0;
        int jumpHeight = 0;
        int jumpPossibleHeight = 100;
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
            collisionList = new List<GroundTile>();
            this.position = startingPosition;
            this.collisionBox = new Rectangle((int)position.X, (int)position.Y, 100, 100);
            this.textureImage = textureImage;
            this.moveSpeed = moveSpeed;
            this.totalFrames = rows * columns;
        }

        public void Move(Vector2 Direction)
        {
            position += Direction * moveSpeed;
        }

        public void Jump(int jumpSpeed, int jumpHeight)
        {
            jumpPossibleHeight = (int)position.Y - jumpHeight;
            this.jumpSpeed = jumpSpeed;
            isJumping = true;
            isFalling = false;
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
            if(isFalling && !isJumping)
            {
                position += World.Gravity;
            }

            if(!isFalling && isJumping)
            {
                position -= new Vector2(0, 1) * jumpSpeed;
                if(jumpPossibleHeight > position.Y) // player is above max jump height
                {
                    Console.WriteLine("Jump stop trigger");
                    isJumping = false;
                    isFalling = true;
                }
            }

            foreach(GroundTile g in World.groundTilesList)
            {
                // if true deal with collision
                if(collisionBox.Intersects(g.collisionBox))
                {
                    //player is colliding with top side
                    if (collisionBox.Bottom > g.collisionBox.Top && collisionBox.Bottom < g.collisionBox.Bottom)
                    {
                        isFalling = false;
                        collisionList.Add(g);
                    }
                    // player is colliding with down side
                    if (collisionBox.Top < g.collisionBox.Bottom && collisionBox.Top > g.collisionBox.Top)
                    {
                        collisionList.Add(g);
                    }
                    // player is colliding with right side
                    if (collisionBox.Left > g.collisionBox.Right && collisionBox.Left < g.collisionBox.Left)
                    {
                        collisionList.Add(g);
                    }
                    // player is colliding with left side
                    if (collisionBox.Right < g.collisionBox.Left && collisionBox.Right > g.collisionBox.Right)
                    {
                        collisionList.Add(g);
                    }
                }
                else if(collisionList.Contains(g)) // collision is over remove it from the list
                {
                    collisionList.Remove(g);
                }
                else if(!isJumping && collisionList.Count == 0) // no collision, fall down
                {
                        isFalling = true;
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

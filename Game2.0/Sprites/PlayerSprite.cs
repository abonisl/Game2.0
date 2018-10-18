using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class PlayerSprite : Sprite
    {
        float lastPosition = 0;
        bool isFalling = true;
        bool isJumping = false;
        Vector2 force = new Vector2();
        float jump = -20;
        public Vector2 velocity = new Vector2();
        public static Vector2 playerPosition = Vector2.Zero;

        // non animated sprite
        public PlayerSprite(Texture2D textureImage, Vector2 position, Point frameSize, float speed)
            :base(textureImage,position,frameSize,speed)
        {
            
        }

        // animated sprite
        public PlayerSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, float speed)
            :base(textureImage,position,frameSize,currentFrame,sheetSize,speed)
        {
            
        }

        public PlayerSprite(Texture2D textureImage, Vector2 position, Point frameSize, Point currentFrame, Point sheetSize, float speed, int millisecondsPerFrame)
            :base(textureImage,position,frameSize,currentFrame,sheetSize,speed,millisecondsPerFrame)
        {
            
        }

        public override Vector2 Direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                KeyboardState keyboard = Keyboard.GetState();

                if (keyboard.IsKeyDown(Keys.A)) // player go left
                {
                    inputDirection += new Vector2(-1, 0);
                }

                if (keyboard.IsKeyDown(Keys.D)) // player go right
                {
                    inputDirection += new Vector2(1, 0);
                }

                if (keyboard.IsKeyDown(Keys.W)) // player jump
                {
                    isJumping = true;
                }
                else
                {
                    isJumping = false;
                }

                return inputDirection;
            }
        }

        // not used
        public Vector2 Force
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;
                KeyboardState keyboard = Keyboard.GetState();

                if(keyboard.IsKeyDown(Keys.A)) // player go left
                {
                    force += new Vector2(-400.0f,0f);
                }

                if(keyboard.IsKeyDown(Keys.D)) // player go right
                {
                    force += new Vector2(400.0f, 0f);
                }
                
                if(keyboard.IsKeyDown(Keys.W)) // player jump
                {
                    if(isFalling)
                    {
                        isJumping = false;
                    }
                    else
                    {
                        inputDirection += new Vector2(0, -1);
                        //isJumping = true;
                    }
                }

                return force;
            }
        }

        public override Point GetFrameSize
        {
            get
            {
                return frameSize;
            }
        }

        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            KeyboardState keyboard = Keyboard.GetState();

            if (lastPosition >= position.Y)
            {
                isFalling = false;
            }
            else
            {
                isFalling = true;
                isJumping = false;
            }
            lastPosition = position.Y;

            if(isJumping && !isFalling)
            {
                velocity = speed * Direction + new Vector2(0, jump) + Physics.World.Gravity;
            }
            else
            {
                velocity = speed * Direction + Physics.World.Gravity;
            }

            position += velocity;                                                       // update position
            playerPosition = position;

            // Keep sprite inside the game screen
            if(position.X < 0)
            {
                position.X = 0;
            }

            if(position.Y < 0)
            {
                position.Y = 0;
            }

            if(position.X > clientBounds.Width - frameSize.X)
            {
                position.X = clientBounds.Width - frameSize.X;
            }

            if(position.Y > clientBounds.Height - frameSize.Y)
            {
                position.Y = clientBounds.Height - frameSize.Y;
            }

            // Update collision box to current position
            CollisionBox.UpdateCollisionBox(
                new Vector2(position.X, position.Y), // player top left vertex (AABB max)
                new Vector2(position.X + frameSize.X, position.Y + frameSize.Y)); // player bottom right vertex (AABB min)

            base.Update(gameTime, clientBounds);
        }

        // AABB test
        public bool IsOverlaping(Sprite spriteToTest)
        {
            // Monogame uses different coordinates system so AABB test will be slightly different than usual
            if (CollisionBox.GetAABBmax().X > spriteToTest.CollisionBox.GetAABBmin().X || CollisionBox.GetAABBmin().X < spriteToTest.CollisionBox.GetAABBmax().X)
                return false;
            
            if (CollisionBox.GetAABBmax().Y > spriteToTest.CollisionBox.GetAABBmin().Y || CollisionBox.GetAABBmin().Y < spriteToTest.CollisionBox.GetAABBmax().Y)
                return false;
            return true;
        }

        public void ResolveCollision(Sprite collidingSprite)
        {
            //player is colliding with top side
            if(CollisionBox.GetAABBmin().Y > collidingSprite.CollisionBox.GetAABBmax().Y && CollisionBox.GetAABBmin().Y < collidingSprite.CollisionBox.GetAABBmin().Y)
            {
                position.Y = collidingSprite.position.Y - GetFrameSize.Y;
                return;
            }
            // player is colliding with down side
            if(CollisionBox.GetAABBmax().Y < collidingSprite.CollisionBox.GetAABBmin().Y && CollisionBox.GetAABBmax().Y > collidingSprite.CollisionBox.GetAABBmax().Y)
            {
                position.Y = collidingSprite.position.Y + GetFrameSize.Y;
                return;
            }
            //// player is colliding with right side
            if (CollisionBox.GetAABBmin().X > collidingSprite.CollisionBox.GetAABBmax().X && CollisionBox.GetAABBmin().X < collidingSprite.CollisionBox.GetAABBmin().X)
            {
                position.X = collidingSprite.position.X - GetFrameSize.X;
                return;
            }
            // player is colliding with left side
            if(CollisionBox.GetAABBmax().X < collidingSprite.CollisionBox.GetAABBmin().X && CollisionBox.GetAABBmax().X > collidingSprite.CollisionBox.GetAABBmax().X)
            {
                position.X = collidingSprite.position.X + collidingSprite.GetFrameSize.X;
                return;
            }
        }
    }
}

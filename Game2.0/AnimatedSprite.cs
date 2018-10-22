using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Lidgren.Network;

namespace Game2._0
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private float currentFrame;
        private int totalFrames;
        public float speed;
        private int last_time_run;
        public bool but_d;
        public Vector2 currentPos;
        public NetConnection con;

        public AnimatedSprite(Texture2D texture, int rows, int columns, Vector2 location, NetConnection conn)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            currentPos = location;
            con = conn;
            last_time_run = 0;
            but_d = false;
        }

        public void Move(int speed)
        {
            this.speed = speed;
            currentPos.X += speed;
        }

        public void Update()
        {
            if (speed > 0)
            {
                currentFrame += speed;
                if (currentFrame == totalFrames)
                    currentFrame = 0;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = ((int)currentFrame) % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)currentPos.X, (int)currentPos.Y, width, height);

            spriteBatch.Begin();
            spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            spriteBatch.End();
        }
    }
}
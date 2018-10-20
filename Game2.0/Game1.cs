using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game1.Physics;

namespace Game2._0
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public Player player1;
        //GroundTile ground;

        private SpriteFont font;
        private int gameTimer = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            World.groundTilesList = new System.Collections.Generic.List<GroundTile>();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>("Score");
            Texture2D playerTexture = Content.Load<Texture2D>("SmileyWalk");
            Texture2D groundTile = Content.Load<Texture2D>("groundTile");
            player1 = new Player(new Vector2(100,100),playerTexture,5);
            for(int i = 0; i < 20; ++i)
            {
                GroundTile ground = new GroundTile(new Point(50*i, 300), new Point(50, 50), groundTile);
                World.groundTilesList.Add(ground);
            }
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (animatedSprite1.CheckFinish() || animatedSprite1.CheckFinish())
            gameTimer++;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                player1.Move(new Vector2(1,0));
            }
            if (Keyboard.GetState().IsKeyDown(Keys.W) && !player1.isJumping && !player1.isFalling)
            {
                player1.Jump(10, 50);
            }

            // TODO: Add your update logic here

            player1.PlayerUpdate(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Time: " + gameTimer, new Vector2(50, 50), Color.Black);
            spriteBatch.End();

            player1.DrawUpdate(spriteBatch);
            foreach(GroundTile g in World.groundTilesList)
            {
                g.Draw(spriteBatch);
            }
            //animatedSprite1.Draw(spriteBatch);
            //animatedSprite2.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}

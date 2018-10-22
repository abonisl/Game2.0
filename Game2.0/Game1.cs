using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Lidgren.Network;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Game2._0
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private AnimatedSprite animatedSprite1;

        public int gamestate = 0;
        public bool host = false;

        private Texture2D HostGameButton;
        private Texture2D JoinGameButton;
        private Rectangle recHostGameButton;
        private Rectangle recJoinGameButton;
        private SpriteFont font;
        private Texture2D texture;
        private int score = 1000;
        private NetPeerConfiguration config;
        private NetServer server;
        private NetClient client;

        private List<AnimatedSprite> players;
        private int connected_players = 0;
        private int my_player = 0;
        MouseState mouseState;
        Rectangle Cursor;

        NetIncomingMessage message;

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

            players = new List<AnimatedSprite>();
            IsMouseVisible = true;
            recHostGameButton = new Rectangle(200, 100, 400, 100);
            recJoinGameButton = new Rectangle(200, 300, 400, 100);

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

            HostGameButton = Content.Load<Texture2D>("HostGame");
            JoinGameButton = Content.Load<Texture2D>("JoinGame");

            font = Content.Load<SpriteFont>("Score");
            texture = Content.Load<Texture2D>("runner2");
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
        private void UpdateCursorPosition()
        {
            /* Update Cursor position by Mouse */
            mouseState = Mouse.GetState();
            Cursor.X = mouseState.X; Cursor.Y = mouseState.Y;
        }

        private void ButtonsEvents()
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if ((recHostGameButton.Intersects(Cursor)))
                {
                    host = true;
                    gamestate = 1;

                    config = new NetPeerConfiguration("Game20")
                    { Port = 12345 };
                    server = new NetServer(config);
                    server.Start();

                    //animatedSprite1 = new AnimatedSprite(texture, 4, 4, new Vector2(50, 300), null);
                    AnimatedSprite animatedSprite = new AnimatedSprite(texture, 2, 7, new Vector2(50, 400), null);
                    players.Add(animatedSprite);
                }
                else if ((recJoinGameButton.Intersects(Cursor)))
                {
                    host = false;
                    gamestate = 1;

                    config = new NetPeerConfiguration("Game20");
                    client = new NetClient(config);
                    client.Start();
                    client.Connect(host: "127.0.0.1", port: 12345);
                }
            }
        }

        private void UpdateNetwork()
        {
            if (host)
            {
                while ((message = server.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.StatusChanged:
                            // handle connection status messages
                            switch (message.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:
                                    AnimatedSprite animatedSprite = new AnimatedSprite(texture, 2, 7, new Vector2(50, 330 - connected_players*70), message.SenderConnection);
                                    players.Add(animatedSprite);
                                    connected_players++;
                                    if (server.Connections.Count > 0)
                                    {
                                        NetOutgoingMessage sendMsg = server.CreateMessage();
                                        sendMsg.Write("N" + connected_players);
                                        server.SendMessage(sendMsg, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                                    }
                                    break;

                                case NetConnectionStatus.Disconnected:
                                    //players.Remove(message.SenderConnection.RemoteUniqueIdentifier);
                                    connected_players--;
                                    if (server.Connections.Count > 0)
                                    {
                                        NetOutgoingMessage sendMsg = server.CreateMessage();
                                        sendMsg.Write("N" + connected_players);
                                        server.SendMessage(sendMsg, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                                    }
                                    break;
                            }
                            break;
                        case NetIncomingMessageType.Data:
                            // handle custom messages
                            string[] data = message.ReadString().Split(' ');
                            if(data[0] == "POS")
                            {
                                players[int.Parse(data[1])].currentPos.X = int.Parse(data[2]);
                            }
                            break;

                        /* .. */
                        default:
                            Console.WriteLine("unhandled message with type: "
                                + message.MessageType);
                            break;
                    }
                }
                if (server.Connections.Count > 0 && gamestate > 1)
                {
                    NetOutgoingMessage sendMsg = server.CreateMessage();
                    string all_pos = "POS ";
                    foreach (AnimatedSprite player in players)
                    {
                        all_pos += player.currentPos.X + " ";
                    }
                    sendMsg.Write(all_pos);
                    server.SendMessage(sendMsg, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                }
            }
            else
            {
                while ((message = client.ReadMessage()) != null)
                {
                    switch (message.MessageType)
                    {
                        case NetIncomingMessageType.StatusChanged:
                            // handle connection status messages
                            switch (message.SenderConnection.Status)
                            {
                                case NetConnectionStatus.Connected:
                                    break;

                                case NetConnectionStatus.Disconnected:
                                    break;
                            }
                            break;

                        case NetIncomingMessageType.Data:
                            // handle custom messages
                            var data = message.ReadString();
                            if (data == "G2")
                            {
                                foreach (AnimatedSprite player in players)
                                {
                                    player.currentPos.X = 50;
                                }
                                score = 1000;
                                gamestate = 2;
                            }
                            else if (data == "G4")
                                gamestate = 4;
                            else if (data[0].ToString() == "N")
                            {
                                connected_players = int.Parse(Regex.Replace(data, @"\D", ""));

                                if (my_player == 0)
                                {
                                    for (int i = 0; i <= connected_players; i++)
                                    {
                                        AnimatedSprite animatedSprite = new AnimatedSprite(texture, 2, 7, new Vector2(50, 400 - i * 70), null);
                                        players.Add(animatedSprite);
                                    }
                                    my_player = connected_players;
                                }
                                else
                                {
                                    AnimatedSprite animatedSprite = new AnimatedSprite(texture, 2, 7, new Vector2(50, 400 - connected_players * 70), null);
                                    players.Add(animatedSprite);
                                }
                            }
                            else if (data[0].ToString() == "P")
                            {
                                string[] data2 = data.Split(' ');
                                var j = 0;
                                foreach (string dat in data2)
                                {
                                    if (dat == "POS" || dat == "")
                                        continue;
                                    else
                                    {
                                        if (j != my_player)
                                            players[j].currentPos.X = int.Parse(dat);
                                        j++;
                                    }
                                }
                            }
                            break;

                        /* .. */
                        default:
                            Console.WriteLine("unhandled message with type: "
                                + message.MessageType);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (gamestate > 0)
            {
                UpdateNetwork();
                if (gamestate == 1)
                {
                    if (host && Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        if (server.Connections.Count > 0)
                        {
                            NetOutgoingMessage sendMsg = server.CreateMessage();
                            sendMsg.Write("G2");
                            server.SendMessage(sendMsg, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                        }
                        gamestate = 2;
                        //wyslac wiadomosc do graczy
                    }
                }
                else if (gamestate == 2)
                {
                    score--;
                    if (score == 0)
                        gamestate = 3;
                }
                else if (gamestate == 3)
                {
                    //if (animatedSprite1.CheckFinish() || animatedSprite1.CheckFinish())
                    score++;

                    if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Exit();

                    if (Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.A))
                    {
                        if(players[my_player].but_d && Keyboard.GetState().IsKeyDown(Keys.A))
                        {
                            players[my_player].but_d = false;
                            players[my_player].Move(2);
                        }
                        else if (!players[my_player].but_d && Keyboard.GetState().IsKeyDown(Keys.D))
                        {
                            players[my_player].but_d = true;
                            players[my_player].Move(2);
                        }
                        else
                        {
                            players[my_player].Move(0);
                        }

                  
                        //animatedSprite1.Update();
                        if (!host)
                        {
                            NetOutgoingMessage sendMsg = client.CreateMessage();
                            sendMsg.Write("POS " + my_player + " " + players[my_player].currentPos.X);
                            client.SendMessage(sendMsg, NetDeliveryMethod.ReliableOrdered);
                        }
                    }

                    foreach (AnimatedSprite player in players)
                    {
                        player.Update();
                        if (host && player.currentPos.X > 740)
                        {
                            gamestate = 4;
                            if (server.Connections.Count > 0)
                            {
                                NetOutgoingMessage sendMsg = server.CreateMessage();
                                sendMsg.Write("G4");
                                server.SendMessage(sendMsg, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                            }
                        }
                    }
                }
                else if (gamestate == 4)
                {
                    if (host && Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        foreach (AnimatedSprite player in players)
                        {
                            player.currentPos.X = 50;
                        }

                        score = 1000;
                        if (server.Connections.Count > 0)
                        {
                            NetOutgoingMessage sendMsg = server.CreateMessage();
                            sendMsg.Write("G2");
                            server.SendMessage(sendMsg, server.Connections, NetDeliveryMethod.ReliableOrdered, 0);
                        }
                        gamestate = 2;
                        //wyslac wiadomosc do graczy
                    }
                }
            }
            else
            {
                UpdateCursorPosition();
                ButtonsEvents();
            }
            // TODO: Add your update logic here
            
            

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
            if (gamestate > 1)
            {
                spriteBatch.Begin();
                if (gamestate == 2)
                {
                    spriteBatch.DrawString(font, "Start in: " + score, new Vector2(50, 50), Color.Black);
                }
                else
                {
                    spriteBatch.DrawString(font, "Time: " + score, new Vector2(50, 50), Color.Black);
                }
                spriteBatch.End();

                //animatedSprite1.Draw(spriteBatch);
                foreach (AnimatedSprite player in players)
                {
                    player.Draw(spriteBatch);
                }
            }
            else if (gamestate == 0)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(HostGameButton, recHostGameButton, Color.White);
                spriteBatch.Draw(JoinGameButton, recJoinGameButton, Color.White);
                spriteBatch.End();
            }
            else if (gamestate == 1)
            {
                spriteBatch.Begin();
                spriteBatch.DrawString(font, "Connected players: " + connected_players, new Vector2(50, 50), Color.Black);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}

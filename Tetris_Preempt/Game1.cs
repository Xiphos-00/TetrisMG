using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace Tetris_Preempt
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        TetrisGameB mainGame;
        bool running = true;
        #region
        Texture2D texture1, texture2;
        Vector2 pos1, pos2;
        Vector2 vel1, vel2;
        Vector2 acc1, acc2;
        Rectangle rct1, rct2;
        Character Player1, Player2;
        public static int maxX, minX, maxY, minY;
        Input input = new Input();
        float speed = 0.01f;
        public static List<Keys> controls;
        #endregion

        public Game1()
        {

            this.IsMouseVisible = true;
            controls = new List<Keys>{
                Keys.W,
                Keys.A,
                Keys.S,
                Keys.D,
                Keys.Up,
                Keys.Down,
                Keys.Left,
                Keys.Right
            };

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";





            input.Initialise();



            maxX = 800; 
            maxY = 480; 
            minY = 0;
            minX = 0;

            pos1 = new Vector2(0, 0);
            pos2 = new Vector2(700, 380);
            vel1 = new Vector2(0, 0);
            vel2 = new Vector2(0, 0);
            acc1 = new Vector2(0, 0);
            acc2 = new Vector2(0, 0);


            this.IsFixedTimeStep = true;
            this.graphics.SynchronizeWithVerticalRetrace = true;
            this.TargetElapsedTime = new System.TimeSpan(0,0,0,0,500); //1s/0.033s = 30fps
            TetrisGameB.OnGameOver += this.NewGame;
            
            /*
            this.Deactivated += this.gamePaused;
            this.Activated += this.gameUnPaused;
            Character.OnCharacterCollision += this.charCollision;
            Character.OnWindowCollision += this.windowCollision;
            Input.OnInputKB += this.MoveBox;
            Input.OnClick += this.SetBox;
            Input.OnUnclick += this.ChangeCol;*/

            
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>

        /*private void charCollision(Character sender, Character.CharacterCollisionEventArgs e)
        {
            sender.Velocity *= -1;
            sender.Acceleration *= -1;
        }
        private void windowCollision(Character sender, Character.WindowCollisionEventArgs e)
        {
            if(e.SidesCollidedWith.Contains("Bottom Side"))
            {
                Vector2 posAdd = new Vector2(0, maxY - (sender.Position.Y + sender.Body.Height));          
                Vector2 accelRef = new Vector2(1, -1);
                Vector2 velRef = new Vector2(1, -1);
                sender.Position += posAdd;
                sender.Acceleration *= accelRef;
                sender.Velocity *= velRef;
            }
            if(e.SidesCollidedWith.Contains("Left Side"))
            {
                Vector2 posAdd = new Vector2(minX - sender.Position.X, 0);
                Vector2 accelRef = new Vector2(-1, 1);
                Vector2 velRef = new Vector2(-1, 1);
                sender.Position += posAdd;
                sender.Acceleration *= accelRef;
                sender.Velocity *= velRef;
            }
            if(e.SidesCollidedWith.Contains("Right Side"))
            {
                Vector2 posAdd = new Vector2(maxX - (sender.Position.X + sender.Body.Width), 0);
                Vector2 accelRef = new Vector2(-1, 1);
                Vector2 velRef = new Vector2(-1, 1);
                sender.Position += posAdd;
                sender.Acceleration *= accelRef;
                sender.Velocity *= velRef;
            }
            if (e.SidesCollidedWith.Contains("Top Side"))
            {
                Vector2 posAdd = new Vector2(0, minY - sender.Position.Y);
                Vector2 accelRef = new Vector2(1, -1);
                Vector2 velRef = new Vector2(1, -1);
                sender.Position += posAdd;
                sender.Acceleration *= accelRef;
                sender.Velocity *= velRef;
            }

        }
        private void SetBox(object sender, Input.InputEventArgs e)
        {
            /*if(e.Clicked.Contains("Left Click"))
            {
                Color[] colorData = new Color[100 * 100];
                for (int i = 0; i < colorData.Length; i++)
                {
                    colorData[i] = Color.Black;
                }
                texture.SetData<Color>(colorData);
                Point p = e.MouseLocation;
                pos = new Vector2(p.X, p.Y);
            }
        }
        private void MoveBox(object sender, Input.InputEventArgs e)
        {
            Vector2 a1 = new Vector2(0, 0);
            if (e.KeysDown.Contains(Keys.W))
            {
                a1.Y -= speed;
            }
            if (e.KeysDown.Contains(Keys.A))
            {
                a1.X -= speed;
            }
            if (e.KeysDown.Contains(Keys.S))
            {
                a1.Y += speed;
            }
            if (e.KeysDown.Contains(Keys.D))
            {
                a1.X += speed;
            }
            Player1.Accelerate(a1);
            Vector2 a2 = new Vector2(0, 0);
            if (e.KeysDown.Contains(Keys.Up))
            {
                a2.Y -= speed;
            }
            if (e.KeysDown.Contains(Keys.Left))
            {
                a2.X -= speed;
            }
            if (e.KeysDown.Contains(Keys.Down))
            {
                a2.Y += speed;
            }
            if (e.KeysDown.Contains(Keys.Right))
            {
                a2.X += speed;
            }
            Player2.Accelerate(a2);
        }

        private void gamePaused(object sender, System.EventArgs args)
        {
            this.Window.Title = "[PAUSED]";
        }
        private void gameUnPaused(object sender, System.EventArgs args)
        {
            this.Window.Title = "Tetris";
        }
        private void ChangeCol(object sender, Input.InputEventArgs e)
        {
            /*if(e.UnClicked.Contains("Left Click"))
            {

                Color[] colorData = new Color[100 * 100];
                for (int i = 0; i < colorData.Length; i++)
                {
                    colorData[i] = Color.Red;
                }
                texture.SetData<Color>(colorData);
            }
        }*/

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            Input.OnInputKB += this.MoveBlock;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        private void MoveBlock(object sender, Input.InputEventArgs e)
        {
            if (running)
            {
                if (e.KeysDown.Contains(Keys.Left))
                {
                    mainGame.MoveLeft();
                }
            }
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Random rnd = new Random();
            mainGame = new TetrisGameB(new Point(0, 0), 10, 20, 16, 3 , GraphicsDevice);

            mainGame.ClearBoard();

            // TODO: use this.Content to load your game content here
        }
        private void NewGame(object sender, TetrisGameB.GameOverEventArgs e)
        {
            mainGame = new TetrisGameB(e.gLoc, e.gWidth, e.gHeight, e.gTileSize, e.gSeed, e.gGr);
            
            running = true;
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
            //input.checkUpdate();
            if (IsActive && running)
            {
                if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                    Exit();



                // TODO: Add your update logic here
                //Move down and then move elsewhere
                bool b = mainGame.Update();
                if (!b)
                {
                    running = false;
                    return;
                }
                KeyboardState kbS = Keyboard.GetState();
                if (kbS.IsKeyDown(Keys.Left))
                {
                    mainGame.MoveLeft();
                }

                base.Update(gameTime);
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            if(running)
                mainGame.Draw();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}

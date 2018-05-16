using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tetris_Preempt
{
   
    class TetrisGameB : Game1
    {
        public delegate void GameOverEventDel(object sender, GameOverEventArgs e);
        public static event GameOverEventDel OnGameOver;

        public Dictionary<string, Block> BlockTypes = new Dictionary<string, Block>();
        public int XPos, YPos;
        public int Width, Height;
        public int TileSize;
        public Block CurrentBlock;
        public Dictionary<Block, Point> GamePieces = new Dictionary<Block, Point>();
        public int[,] Board = new int[10,20];
        public Queue<Block> UpcomingPieces;
        private SpriteBatch Sprite;
        public GraphicsDevice Gr;
        bool running;
        
        public TetrisGameB(Point gameLocation, int width, int height, int tilesize, int seed, GraphicsDevice g)
        {
            this.GetGraphics(g);
            running = true;
            XPos = gameLocation.X;
            YPos = gameLocation.Y;
            Width = width;
            Height = height;
            TileSize = tilesize;

            //Initialize the block types
            BlockTypes = new Dictionary<string, Block> {
            {"I", new Block(Tilesets.I, Tilesets.TileColors[0],this)},
            {"O", new Block(Tilesets.O, Tilesets.TileColors[1],this)},
            {"T", new Block(Tilesets.T, Tilesets.TileColors[2],this)},
            {"J", new Block(Tilesets.J, Tilesets.TileColors[3],this)},
            {"L", new Block(Tilesets.L, Tilesets.TileColors[4],this)},
            {"S", new Block(Tilesets.S, Tilesets.TileColors[5],this)},
            {"Z", new Block(Tilesets.Z, Tilesets.TileColors[6],this)},
            };

            //Initialize the board
            ClearBoard();

            //Enqueue
            UpcomingPieces = new Queue<Block>(5);
            for(int i = 0; i < 4; i++)
            {
                UpcomingPieces.Enqueue(NextRandomBlock((i*52)+32));
            }

            //Get the first block
            CurrentBlock = NextRandomBlock(seed);



        }
        public void GetGraphics(GraphicsDevice g)
        {
            Gr = g;
            Sprite = new SpriteBatch(g);
        }
        public bool Update()
        {
            //Try to move it down. If it returns false, the current block just stopped
            bool canStillFall = CurrentBlock.MoveDown(this.Board);
            if (!canStillFall)
            {            
                GamePieces.Add(CurrentBlock, CurrentBlock.GetLocation());
                if(GamePieces[CurrentBlock].Y <= 0)
                {
                    Restart();
                    running = false;
                    return false;
                }
                UpcomingPieces.Enqueue(NextRandomBlock(0));
                Block newBlock = UpcomingPieces.Dequeue();
                CurrentBlock = newBlock;
            }

            //Now update the Board[,]
            ClearBoard();
            UpdateBoard();
            return true;
        }
        public void Restart()
        {
            //Triggers the gameover event
            GamePieces.Clear();
            ClearBoard();
            UpcomingPieces.Clear();
            Random r = new Random();
            GameOverEventArgs e = new GameOverEventArgs(new Point(XPos, YPos), Width, Height, TileSize, this.Gr, r.Next());
            OnGameOver?.Invoke(this, e);
        }
        public class GameOverEventArgs
        {
            public Point gLoc;
            public int gWidth, gHeight;
            public int gTileSize;
            public GraphicsDevice gGr;
            public int gSeed;
            public GameOverEventArgs(Point l, int w, int h, int t, GraphicsDevice g, int s)
            {
                gLoc = l;
                gWidth = w;
                gHeight = h;
                gTileSize = t;
                gGr = g;
                gSeed = s;
            }
        }
        public void ClearBoard()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    Board[x, y] = 0;
                }
            }
        }
        public void Draw()
        {
            if (running)
            {
                InitialBG();
                try
                {
                    foreach (Block b in GamePieces.Keys)
                    {
                        b.Draw(Gr);
                    }
                    CurrentBlock.Draw(Gr);
                }
                catch
                {

                }
            }

        }
        public void InitialBG()
        {
            Texture2D Background = new Texture2D(Gr, Width * TileSize, Height * TileSize);
            Rectangle BGBoard = new Rectangle(XPos, YPos, Width * TileSize, Height * TileSize);
            Color[] data = new Color[(Width * TileSize) * (Height * TileSize)];
            for (int i = 0; i < data.Length; i++) data[i] = Color.Black;
            Background.SetData(data);
            Sprite.Begin();
            Sprite.Draw(Background, BGBoard, Color.Black);
            Sprite.End();
        }
        private Block NextRandomBlock(int a)
        {
            Random rnd;
            if(a!= 0){
                rnd = new Random(a);
            }
            else
            {
                rnd = new Random();
            }
            int x = rnd.Next(0,6);
            string key = Tilesets.TileNames[x];
            Block b = new Block(BlockTypes[key],this);
            b.SetLocation(new Point((this.Width / 2)-2, 0));
            return b;
        }
        private void UpdateBoard()
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    //See if there is a block that starts on this position, if so add it in

                    if (GamePieces.Values.Any(a => a.X == x && a.Y == y))
                    {
                        //Current block
                        Block cBlock = (GamePieces.First(a => a.Value == new Point(x, y))).Key;

                        //Tileset x and tileset y
                        for (int tsX = 0; tsX < 4; tsX++)
                        {
                            for (int tsY = 0; tsY < 4; tsY++)
                            {
                                if (x + tsX >= (this.Width - 1) || x + tsX < 0)
                                {
                                    continue;
                                }
                                if (y + tsY >= (this.Height - 1) || y + tsY < 0)
                                {
                                    continue;
                                }
                                if (cBlock.Tiles[tsX, tsY] == 1)
                                {
                                    Board[x + tsX, y + tsY] = 1;
                                }
                            }
                        }
                    }
                }
            }
        }
        public void MoveLeft()
        {
            CurrentBlock.MoveLeft(this.Board);
            UpdateBoard();
        }
    }
    class Block
    {
        private Point Location = new Point();
        public Point DrawingLocation = new Point();
        private TetrisGameB Game;
        private SpriteBatch Sprite;
        public int[,] Tiles = new int[4, 4];
        public Color BlockColor;
        public bool Falling;

        public Block(Block b, TetrisGameB g)
        {
            this.Game = g;
            this.SetLocation(b.Location);
            this.DrawingLocation = b.DrawingLocation;

            this.Sprite = b.Sprite;
            this.Tiles = b.Tiles;
            this.BlockColor = b.BlockColor;
            this.Falling = true;
        }
        public Block(Point loc, int[,] tileset, Color col, TetrisGameB g)
        {
            Game = g;
            this.SetLocation(loc);
            Tiles = tileset;
            BlockColor = col;

            Falling = true;
            Sprite = new SpriteBatch(g.Gr);
        }
        public Block(int[,] tileset, Color col, TetrisGameB g)
        {
            Game = g;
            this.SetLocation(new Point((Game.Width / 2) - 2, 0));
            Tiles = tileset;
            BlockColor = col;

            Falling = true;
            Sprite = new SpriteBatch(g.Gr);
        }
        public void SetLocation(Point p)
        {
            Location = new Point(p.X, p.Y);
            DrawingLocation = new Point(Game.XPos + (Location.X * Game.TileSize), Game.YPos + (Location.Y * Game.TileSize));
        }
        public Point GetLocation()
        {
            return Location;
        }
        public bool MoveDown(int[,] Board)
        {
            //A return value of false means that this object can no longer move down
            if (this.Falling)
            {
                //Check collisions
                for(int x = 0; x < 4; x++)
                {
                    for (int y = 3; y >= 0; y--)
                    {
                        int cTile = this.Tiles[x, y];
                        //Get the tile and the one below it

                        //If the tile below is past the game border, and there is a tile at the bottom of the given tileset, it is over.
                        if ((Location.Y + y + 1 > (Game.Height - 1) || Location.Y < 0) && cTile == 1)
                        {
                            Falling = false;
                            return false;
                        }
                        if (Location.Y + y + 1 > (Game.Height - 1) || Location.Y < 0)
                        {
                            continue;
                        }

                        int cBelow = Board[Location.X + x, (Location.Y) + y + 1]; 

                        //Check for collisions
                        if(cTile == 1 && cBelow == 1)
                        {
                            Falling = false;
                            return false;
                        }
                    }
                }

                //No collisions occured, move down
                this.SetLocation(new Point(this.Location.X, this.Location.Y + 1));
                return true;
            }
            return false;
           
        }
        public bool MoveLeft(int[,] Board)
        {
         
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4 ; y++)
                {
                    int cTile = this.Tiles[x, y];
              
                    //If we are at the left side of the screen and there is a filled tile, dont move
                    if ((Location.X) < 0)
                    {
                        return false;
                    }
                    //We cannot move left
                    if((Location.X + x == 0) && cTile == 1)
                    {
                        return false;
                    }
                    //If we are on the very left side but there is a blank tile here, skip
                    if(Location.X + x == 0)
                    {
                        continue;
                    }
                    if(Location.Y + y > Game.Height - 1)
                    {
                        return false;
                    }

                    int cLeft = Board[(Location.X + x)-1, (Location.Y) + y];

                    //Check for collisions
                    if (cTile == 1 && cLeft == 1)
                    {
                        return false;
                    }
                }
            }

            //No collisions occured, move down
            this.SetLocation(new Point(this.Location.X-1, this.Location.Y));
            return true;
        }
        public void Draw(GraphicsDevice Gr)
        {
            Rectangle rec = new Rectangle(DrawingLocation, new Point(Game.TileSize));
            Texture2D cText = new Texture2D(Gr, Game.TileSize, Game.TileSize);
            Color[] data = new Color[Game.TileSize * Game.TileSize];

            Sprite.Begin();
            for (int i = 0; i<4; i++)
            {
                for(int j = 0; j<4; j++)
                {
                    if(Tiles[i,j] == 1)
                    {
                        Point newLocation = new Point(Game.XPos + ((Location.X + i) * Game.TileSize), Game.YPos + ((Location.Y + j) * Game.TileSize));
                        Rectangle cRect = new Rectangle(newLocation, new Point(Game.TileSize));
                        for (int q = 0; q < data.Length; q++) data[q] = BlockColor;
                        cText.SetData(data);
                        Sprite.Draw(cText, cRect, BlockColor);
                    }
                }
            }          
            Sprite.End();
        }
    }

}

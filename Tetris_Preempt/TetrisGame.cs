using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

/*namespace Tetris_Preempt
{
    
    class TetrisGame : Game1
    {
        public Tile[,] Board;
        public int Width;
        public int Height;
        public int XPos;
        public int YPos;
        public int TileSize;
        private int drawTimes = 0;
        public Block CurrentBlock;
        public Dictionary<Block, Location> GamePieces = new Dictionary<Block, Location>();
        public Queue<Block> UpcomingPieces;
        public Tilesets Tiles = new Tilesets();
        private SpriteBatch Sprite;
        private GraphicsDevice Gr;

        public TetrisGame(int width, int height, int xPos, int yPos, int tileSize, SpriteBatch sprite, GraphicsDevice g)
        {
            Width = width;
            Height = height;

            //Initialise board
            Board = new Tile[width,height];
            for(int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Board[i,j] = new Tile(0,Color.Black);
                }
            }

            //Enqueue the next pieces
            UpcomingPieces = new Queue<Block>(5);
            for (int i = 0; i < 4; i++)
            {
                UpcomingPieces.Enqueue(NextRandomBlock());
            }
           
            XPos = xPos;
            YPos = yPos;
            TileSize = tileSize;
            Sprite = sprite;
            Gr = g;

            CurrentBlock = NextRandomBlock();
            GamePieces.Add(CurrentBlock, CurrentBlock.Location);
        }

        public void Update()
        {
            CurrentBlock.MoveDown();
            GamePieces[CurrentBlock] = CurrentBlock.Location;
            if (CurrentBlock.Falling)
            {
                GamePieces[CurrentBlock] = CurrentBlock.Location;
            }
            //If the current block has stopped falling, generate the new block
            else
            {
                CurrentBlock = UpcomingPieces.Dequeue();
                GamePieces.Add(CurrentBlock, CurrentBlock.Location);
                UpcomingPieces.Enqueue(NextRandomBlock());
            }
            //Now that the pieces have moved, update the board
            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    //If there is a block that starts at this location
                    //GamePieces.Values.Contains(new Location(x, y))
                    if (GamePieces.Values.Any(a => a.X == x && a.Y == y))
                    {
                        
                        Block cBlock = (GamePieces.First(a => a.Value == new Location(x, y))).Key;
                        for(int tileSetX = 0; tileSetX < 4; tileSetX++)
                        {
                            for(int tileSetY = 0; tileSetY < 4; tileSetY++)
                            {
                                if (cBlock.Tiles[tileSetX,tileSetY] == 1)
                                {
                                    this.Board[x+tileSetX,y+tileSetY]= new Tile(cBlock.Tiles[tileSetX,tileSetY], cBlock.Type);
                                }
                                else
                                {
                                    this.Board[x + tileSetX, y + tileSetY] = new Tile(0, Color.Black);
                                }
                            }
                        }
                    }
                    else
                    {
                        this.Board[x,y] = new Tile(0, Color.Black);
                    }
                }
            }
        }
        public void Draw()
        {
            for(int x = 0; x<this.Width; x++)
            {
                for(int y = 0; y < this.Height; y++)
                {
                    Tile cTile = this.Board[x,y];
                    Vector2 cLocation = new Vector2(XPos + (x * TileSize), YPos + (y * TileSize));
                    Rectangle rec = new Rectangle(new Point(XPos + (x * TileSize), YPos + (y * TileSize)), new Point(TileSize));
                    Texture2D cText = new Texture2D(Gr, TileSize, TileSize);
                    Color[] data = new Color[TileSize * TileSize];
                    for (int i = 0; i < data.Length; i++) data[i] = cTile.Shading;

                    cText.SetData(data);
                    Sprite.Begin();
                    Sprite.Draw(cText, rec, cTile.Shading);
                    Sprite.End();
                }
            }
            drawTimes++;

        }

        public Block NextRandomBlock()
        {
            //Get the next block randomly (through the Tilesets.cs framework)
            #region
            Random rnd = new Random();
            int iRnd = rnd.Next(0, 6);
            string TileName = Tiles.TileNames[iRnd];
            #endregion

            Block next = new Block(Width / 2, 0, Tiles.TileSets[TileName], this, Tiles.TileColors[iRnd]);
            return next;
        }
    }
    class Block
    {
        #region
        public int X;
        public int Y;
        public Location Location;
        public int Top;
        public int Bottom;
        public int Left;
        public int Right;
        public Color Type;
        public int[,] Tiles;
        public bool Falling;
        public TetrisGame Game;
        #endregion
        public Block(int x, int y, int[,] tileset, TetrisGame g, Color type)
        {
            X = x;
            Y = y;
            Location = new Location(X, Y);
            Right = x + 3;
            Left = x;
            Top = y;
            Bottom = y + 3;

            Tiles = tileset;
            Falling = true;
            Game = g;

            Type = type;
        }
        public void MoveDown()
        {
            if (Falling)
            {
                //Check below this to make sure no collisions will occur
                for(int x = 0; x< 4; x++)
                {
                    for (int y = 3; y >=0; y--)
                    {
                        int cTile = this.Tiles[x, y];
                        int cBelow = Game.Board[this.X + x, y].Value;

                        if (cTile == 1)
                        {
                            if (cBelow == 1)
                            {
                                //Collision occured below
                                this.Falling = false;
                                return;
                            }
                        }
                        if (this.Y + 3 == Game.Height - 1)
                        {
                            this.Falling = false;
                            return;
                        }
                    }
                }

                //If no collisions occured, move this down
                this.Y++;
                this.Location.Y++;
                Game.GamePieces[this] = this.Location;
            }
        }
    }
    
    class Tile
    {
        public int Value;
        public Color Shading;
        public Tile(int val, Color shade)
        {
            Value = val;
            Shading = shade;
        }
    }
    class Location
    {
        public int X = new int();
        public int Y = new int();
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }
        public static bool operator ==(Location me, Location other)
        {
            if(me.X == other.X && me.Y == other.Y)
            {
                return true;
            }
            return false;
        }
        public static bool operator !=(Location me,  Location other)
        {
            if (me.X == other.X && me.Y == other.Y)
            {
                return false ;
            }
            return true;
        }


    }
}
*/
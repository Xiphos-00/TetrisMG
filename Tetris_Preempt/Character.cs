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
    public class Character
    {
        public delegate void CharacterCollisionEvents(Character sender, CharacterCollisionEventArgs e);
        public delegate void WindowCollisionEvents(Character sender, WindowCollisionEventArgs e);
        public static event CharacterCollisionEvents OnCharacterCollision;
        public static event WindowCollisionEvents OnWindowCollision;
        private Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Acceleration { get; set; }
        private Color Color { get; set; }
        private SpriteBatch Sprite { get; set; }
        public Rectangle Size;
        public Rectangle Body { get; set; }
        private int minX = Game1.minX;
        private int minY = Game1.minY;
        private int maxX = Game1.maxX;
        private int maxY = Game1.maxY;

        public Character(Texture2D texture, Vector2 position, Vector2 velocity, Vector2 acceleration, Color color, SpriteBatch sprite, Rectangle size)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Acceleration = acceleration;
            Sprite = sprite;
            Body= size;
        }
        public void Update()
        {
            Position += Velocity;
            Velocity += Acceleration;
            this.WindowCollision();
            this.Draw();
        }
        public void Draw()
        {
            Sprite.Begin();
            Sprite.Draw(Texture, Position, sourceRectangle:Body);
            Sprite.End();
        }
        public void Accelerate(Vector2 aAdd)
        {
            Acceleration = aAdd;
        }
        public void CheckCollisions(Character c)
        {
            if (this.CollidedWith(c))
            {
                CharacterCollisionEventArgs e = new CharacterCollisionEventArgs
                {
                    CollidedWith = c
                };
                OnCollisionC(e); //Fire a collision event
            }
        }
        public bool CollidedWith(Character other)
        {
            Rectangle LocationA = new Rectangle(new Point((int)this.Position.X, (int)this.Position.Y), new Point(this.Body.Width, this.Body.Height));
            Rectangle LocationB = new Rectangle(new Point((int)other.Position.X, (int)other.Position.Y), new Point(other.Body.Width, other.Body.Height));

            if (LocationA.Intersects(LocationB))
            {
                return true;
            }
            return false;
        }
        public bool WindowCollision()
        {
            List<string> collidedWith = new List<string>();
            bool a = (this.Position.X + this.Body.Width) > maxX;
            bool b = (this.Position.X) < minX;
            bool c = (this.Position.Y + this.Body.Height) > maxY;
            bool d = (this.Position.Y) < minY;
            if (a)
                collidedWith.Add("Right Side");
            if (b)
                collidedWith.Add("Left Side");
            if (c)
                collidedWith.Add("Bottom Side");
            if (d)
                collidedWith.Add("Top Side");

            if(collidedWith.Count > 0)
            {
                WindowCollisionEventArgs e = new WindowCollisionEventArgs(collidedWith);
                OnCollisionW(e);
                return true;
            }
            return false;
        }
        public class CharacterCollisionEventArgs
        {
            public Character CollidedWith;
        }
        public class WindowCollisionEventArgs
        {
            public List<string> SidesCollidedWith;
            public WindowCollisionEventArgs(List<string> collided)
            {
                SidesCollidedWith = collided;
            }
        }

        private void OnCollisionC(CharacterCollisionEventArgs e)
        {
            OnCharacterCollision?.Invoke(this, e);
        }
        private void OnCollisionW(WindowCollisionEventArgs e)
        {
            OnWindowCollision?.Invoke(this, e);
        }
    }
}

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
    class Input
    {
        //Handles Keyboard/Mouse Input
        public delegate void InputEvents(object sender, InputEventArgs e);
        public static event InputEvents OnInputKB;
        public static event InputEvents OnClick;
        public static event InputEvents OnUnclick;
        public static event InputEvents OnMouseMove;

        public KeyboardState prevState_kb;
        public MouseState prevState_m;
        public List<Keys> keys;
        public Dictionary<Keys, bool> keyPressedState = new Dictionary<Keys, bool>();
        public List<Buttons> btns;
        public Dictionary<string, bool> mouseButtonState = new Dictionary<string, bool>();

        private void OnInput(InputEventArgs e)
        {
            OnInputKB?.Invoke(this,e);
        }
        private void OnClickM(InputEventArgs e)
        {
            OnClick?.Invoke(this, e);
        }
        private void OnUnclickM(InputEventArgs e)
        {
            OnUnclick?.Invoke(this, e);
        }
        private void OnMouseMoveM(InputEventArgs e)
        {
            OnMouseMove?.Invoke(this, e);
        }
        public class InputEventArgs
        {
            public List<Keys> NewKeysDown;
            public List<Keys> NewKeysUp;
            public List<Keys> KeysDown = new List<Keys>();
            public List<Keys> KeysUp = new List<Keys>();
            public Point MouseLocation = new Point();
            public List<string> UnClicked= new List<string>();
            public List<string> Clicked = new List<string>();
            public InputEventArgs(List<Keys> down, List<Keys> up, Dictionary<Keys,bool> keyPressedState)
            {
                NewKeysDown = down;
                NewKeysUp = up;
                foreach(Keys k in keyPressedState.Keys)
                {
                    if (keyPressedState[k])
                    {
                        KeysDown.Add(k);
                    }
                    else
                    {
                        KeysUp.Add(k);
                    }
                }
            }
            public InputEventArgs(Point p, List<string> clicked, List<string> unclicked)
            {
                MouseLocation = p;
                Clicked = clicked;
                UnClicked = unclicked;
            }
            public InputEventArgs(Point p)
            {
                MouseLocation = p;
            }

        }
        public void Initialise()
        {
            //Initialise keyboard input
            prevState_kb = Keyboard.GetState();
            keys = Game1.controls;
            foreach(Keys k in keys)
            {
                if (prevState_kb.IsKeyDown(k))
                {
                    keyPressedState.Add(k, true);
                }
                else
                {
                    keyPressedState.Add(k, false);
                }
            }

            //Initialise mouse input
            prevState_m = Mouse.GetState();
            if(prevState_m.LeftButton == ButtonState.Pressed)
            {
                mouseButtonState.Add("Left Click", true);
            }
            else if(prevState_m.LeftButton == ButtonState.Released)
            {
                mouseButtonState.Add("Left Click", false);
            }
            if(prevState_m.RightButton == ButtonState.Pressed)
            {
                mouseButtonState.Add("Right Click", true);
            }
            else if(prevState_m.RightButton == ButtonState.Released)
            {
                mouseButtonState.Add("Right Click", false);
            }
        }
        public void checkUpdate()
        {
            //Get the updates for keyboard
            KeyboardState newState_kb = Keyboard.GetState();
            if(newState_kb.GetPressedKeys() != prevState_kb.GetPressedKeys())
            {
                List<Keys> Pressed = new List<Keys>();
                List<Keys> UnPressed = new List<Keys>();
                foreach(Keys k in keys)
                {
                    bool prevPressed = prevState_kb.IsKeyDown(k);
                    bool nowPressed = newState_kb.IsKeyDown(k);
                    if (nowPressed == true && prevPressed == false)
                    {
                        keyPressedState[k] = true;
                        Pressed.Add(k);
                    }
                    else if(nowPressed == false && prevPressed == true)
                    {
                        keyPressedState[k] = false;
                        UnPressed.Add(k);
                    }
                    InputEventArgs e = new InputEventArgs(Pressed, UnPressed,keyPressedState);
                    OnInput(e);
                }
                prevState_kb = newState_kb;
            }

            MouseState newState_m = Mouse.GetState();
            if(newState_m.LeftButton != prevState_m.LeftButton || newState_m.RightButton != prevState_m.RightButton)
            {
                List<string> justClicked = new List<string>();
                List<string> justUnClicked = new List<string>();
                Point clickLocation = Mouse.GetState().Position;
                if(prevState_m.Position != clickLocation)
                {
                    InputEventArgs eMouseMoved = new InputEventArgs(clickLocation);
                    OnMouseMoveM(eMouseMoved);
                }
                foreach (string b in mouseButtonState.Keys)
                {
                    
                    if (b == "Left Click")
                    {
                        //They just left clicked
                        if(prevState_m.LeftButton == ButtonState.Released && newState_m.LeftButton == ButtonState.Pressed)
                        {
                            justClicked.Add("Left Click");
                        }
                        //They just un-leftclicked
                        if(prevState_m.LeftButton == ButtonState.Pressed && newState_m.LeftButton == ButtonState.Released)
                        {
                            justUnClicked.Add("Left Click");
                        }
                    }
                    if(b == "Right Click")
                    {
                        //They just left clicked
                        if (prevState_m.RightButton == ButtonState.Released && newState_m.RightButton == ButtonState.Pressed)
                        {
                            justClicked.Add("Right Click");
                        }
                        //They just un-leftclicked
                        if (prevState_m.RightButton == ButtonState.Pressed && newState_m.RightButton == ButtonState.Released)
                        {
                            justUnClicked.Add("Right Click");
                        }
                    }
                }
                InputEventArgs e = new InputEventArgs(clickLocation, justClicked, justUnClicked);

                if(justClicked.Count > 0)
                {
                    OnClickM(e);
                }
                if(justUnClicked.Count > 0)
                {
                    OnUnclickM(e);
                }
                prevState_m = newState_m;
            }
        }
    }
}

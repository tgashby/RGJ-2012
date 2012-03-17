using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace RGJgame
{
    class Input
    {
        public static bool X(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.X == ButtonState.Pressed;
        }
        public static bool Y(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.Y == ButtonState.Pressed;
        }
        public static bool A(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.A == ButtonState.Pressed;
        }
        public static bool B(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.B == ButtonState.Pressed;
        }
        public static bool BACK(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.Back == ButtonState.Pressed;
        }
        public static bool START(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.Start == ButtonState.Pressed;
        }
        public static bool LB(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.LeftShoulder == ButtonState.Pressed;
        }
        public static bool RB(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Buttons.RightShoulder == ButtonState.Pressed;
        }
        public static bool DLEFT(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).DPad.Left == ButtonState.Pressed;
        }
        public static bool DUP(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).DPad.Up == ButtonState.Pressed;
        }
        public static bool DDOWN(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).DPad.Down == ButtonState.Pressed;
        }
        public static bool DRIGHT(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).DPad.Right == ButtonState.Pressed;
        }
        public static float LSTICKX(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Left.X;
        }
        public static float LSTICKY(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Left.Y;
        }
        public static float RSTICKX(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Right.X;
        }
        public static float RSTICKY(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Right.Y;
        }
        public static bool LSTICKX(int playerNumber, float bound)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Left.X >= bound;
        }
        public static bool LSTICKY(int playerNumber, float bound)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Left.Y >= bound;
        }
        public static bool RSTICKX(int playerNumber, float bound)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Right.X >= bound;
        }
        public static bool RSTICKY(int playerNumber, float bound)
        {
            return GamePad.GetState(translate(playerNumber)).ThumbSticks.Right.Y >= bound;
        }
        public static float LT(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Triggers.Left;
        }
        public static float RT(int playerNumber)
        {
            return GamePad.GetState(translate(playerNumber)).Triggers.Right;
        }
        public static bool LT(int playerNumber, float bound)
        {
            return GamePad.GetState(translate(playerNumber)).Triggers.Left >= bound;
        }
        public static bool RT(int playerNumber, float bound)
        {
            return GamePad.GetState(translate(playerNumber)).Triggers.Right >= bound;
        }

        public static PlayerIndex translate(int num)
        {
            if (num == 1) return PlayerIndex.One;
            if (num == 2) return PlayerIndex.Two;
            if (num == 3) return PlayerIndex.Three;
            if (num == 4) return PlayerIndex.Four;
            return 0;
        }

    }
}

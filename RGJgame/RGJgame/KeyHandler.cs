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
    static class KeyHandler
    {
        private static Dictionary<Keys, bool> keystates = new Dictionary<Keys,bool>();

        public static void Update()
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (Keyboard.GetState().IsKeyDown(key))
                {
                    keystates[key] = true;
                }
                else if (Keyboard.GetState().IsKeyUp(key))
                {
                    keystates[key] = false;
                }
            }
        }

        public static bool keyDown(Keys key)
        {
            return keystates[key];
        }
    }
}

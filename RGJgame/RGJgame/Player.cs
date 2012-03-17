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
    class Player
    {
        public float health;
        public float detection;
        private bool jumping;

        public Vector2 position, velocity, acceleration;

        public Player(Vector2 pos)
        {
            health = 1.0f;
            detection = 1.0f;

            position = pos;
        }

        public void update(float dtime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {

            }
        }

    }
}

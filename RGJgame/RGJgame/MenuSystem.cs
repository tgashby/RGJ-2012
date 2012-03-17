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
    class MenuSystem : GameComponent
    {
        public static int PAUSE = 0, MAIN = 1, MAP = 2, GAMEOVER = 3,
            LEVELCOMPLETE = 4, OPTIONS = 5;

        public bool menusBusy;
        private int currentMenu = -1;
        private List<Menu> menus = new List<Menu>();
        private SpriteFont titleFont, unselectedFont, selectedFont;
        private Game1 toMod;
        private int wait = 0;

        public MenuSystem(Game game)
            : base(game)
        {
            menusBusy = false;
            LoadContent();
            toMod = (Game1)game;
        }

        public void Update(float gameTime)
        {
            menus[currentMenu].Update(gameTime);

            if (wait == 0)
            {
                if (Input.A(1) || Input.A(2) || Input.A(3) || Input.A(4) || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    DecodeMenuSelection(currentMenu, menus[currentMenu].selection);
                    wait = 15;
                }
                if (Input.B(1) || Input.B(2) || Input.B(3) || Input.B(4) || Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    DecodeAltSelection(currentMenu);
                    wait = 15;
                }
            }
            else
            {
                wait--;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menus[currentMenu].Draw(spriteBatch, new SpriteFont[] { titleFont, selectedFont, unselectedFont });
        }

        public void turnOn(int menu)
        {
            currentMenu = menu;
            menus[currentMenu].ReadyUp();
            menusBusy = true;
            wait = 0;
        }

        /*
         * Rest should be private      
         */

        private void LoadContent()
        {
            titleFont = Game.Content.Load<SpriteFont>(@"Titles");
            selectedFont = Game.Content.Load<SpriteFont>(@"Selected");
            unselectedFont = Game.Content.Load<SpriteFont>(@"Unselected");

            ActivateMenus();
        }

        private void ActivateMenus()
        {
            menus.Add(new Menu("Paused", null));
            menus[PAUSE].addOption("Continue");
            menus[PAUSE].addOption("Select Level");
            menus[PAUSE].addOption("Options");
            menus[PAUSE].addOption("Exit");

            menus.Add(new Menu("Main Menu", null));
            menus[MAIN].addOption("Continue Game");
            menus[MAIN].addOption("Select Level");
            menus[MAIN].addOption("Options");
            menus[MAIN].addOption("Exit");

            menus.Add(new Menu("Level Select", null));
            menus[MAP].addOption("Select Level");
            menus[MAP].addOption("Back To Main Menu");

            menus.Add(new Menu("Game Over", null));
            menus[GAMEOVER].addOption("Select Level");
            menus[GAMEOVER].addOption("Back To Main Menu");
            menus[GAMEOVER].addOption("Exit");

            menus.Add(new Menu("Level Complete!", null));
            menus[LEVELCOMPLETE].addOption("Select Level");
            menus[LEVELCOMPLETE].addOption("Back To Main Menu");
            menus[LEVELCOMPLETE].addOption("Exit");

            menus.Add(new Menu("Options", null));
            menus[OPTIONS].addOption("Toggle Full Screen");
            menus[OPTIONS].addOption("Save & Return");
        }

        private class Menu
        {
            private Texture2D image;
            private String title;
            public int selection;
            private int wait = 0;
            private List<String> options;

            public Menu(String title, Texture2D bg)
            {
                image = bg;
                this.title = title;

                selection = 0;
                options = new List<String>();
            }

            public void addOption(String option)
            {
                options.Add(option);
            }

            public void ReadyUp()
            {
                wait = 0;
            }

            public void Update(float gameTime)
            {
                if (wait != 0)
                    wait--;

                float bound = 0.2f;
                if ((Input.DDOWN(1) || Input.DDOWN(2) || Input.DDOWN(3) || Input.DDOWN(4) ||
                    !Input.LSTICKY(1, -bound) || !Input.LSTICKY(2, -bound) || 
                    !Input.LSTICKY(3, -bound) || !Input.LSTICKY(4, -bound) ||
                    Keyboard.GetState().IsKeyDown(Keys.Down)) && wait == 0 )
                {
                    selection++;
                    if (selection == options.Count)
                        selection = 0;
                    wait = 10;
                }
                if ((Input.DUP(1) || Input.DUP(2) || Input.DUP(3) || Input.DUP(4) ||
                    Input.LSTICKY(1, bound) || Input.LSTICKY(2, bound) ||
                    Input.LSTICKY(3, bound) || Input.LSTICKY(4, bound) ||
                    Keyboard.GetState().IsKeyDown(Keys.Up)) && wait == 0)
                {
                    selection--;
                    if (selection < 0)
                        selection = options.Count - 1;
                    wait = 10;
                }
            }

            public void Draw(SpriteBatch spriteBatch, SpriteFont[] fonts)
            {
                if (image != null) 
                    spriteBatch.Draw(image, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);

                spriteBatch.DrawString(fonts[0], title, new Vector2(40, 40), Color.Orange, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                for (int i = 0; i < options.Count; i++)
                {
                    if (i == selection)
                    {
                        spriteBatch.DrawString(fonts[1], options[i], new Vector2(50, 100 + i * 30), Color.Blue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }
                    else
                    {
                        spriteBatch.DrawString(fonts[2], options[i], new Vector2(50, 100 + i * 30), Color.DarkBlue, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
                    }
                }
            }
        }


        private void DecodeMenuSelection(int menu, int selection)
        {
            if (menu == PAUSE)
            {
                switch (selection)
                {
                    case 0: menusBusy = false; break;
                    case 1: currentMenu = MAP; break;
                    case 2: currentMenu = OPTIONS; break;
                    case 3: toMod.EndGame(); break;
                }
            }
            else if (menu == MAIN)
            {
                switch (selection)
                {
                    case 0: currentMenu = MAP; break;
                    case 1: currentMenu = MAP; break;
                    case 2: currentMenu = OPTIONS; break;
                    case 3: toMod.EndGame(); break;
                }
            }
            else if (menu == MAP)
            {
                switch (selection)
                {
                    case 0: menusBusy = false; toMod.NewGame(); break;
                    case 1: currentMenu = MAIN; break;
                }
            }
            else if (menu == GAMEOVER)
            {
                switch (selection)
                {
                    case 0: currentMenu = MAP; break;
                    case 1: currentMenu = MAIN; break;
                    case 2: toMod.EndGame(); break;
                }

            }
            else if (menu == LEVELCOMPLETE)
            {
                switch (selection)
                {
                    case 0: currentMenu = MAP; break;
                    case 1: currentMenu = MAIN; break;
                    case 2: toMod.EndGame(); break;
                }
            }
            else if (menu == OPTIONS)
            {
                switch (selection)
                {
                    case 0: toMod.toggleFullScreen(); break;
                    case 1: currentMenu = MAIN; break;
                }
            }

        }

        private void DecodeAltSelection(int menu)
        {
            if (menu == PAUSE)
            {

            }
            else if (menu == MAIN)
            {

            }
            else if (menu == MAP)
            {
                
            }
            else if (menu == GAMEOVER)
            {
                
            }
            else if (menu == LEVELCOMPLETE)
            {
                
            }
            else if (menu == OPTIONS)
            {

            }
        }
    }
}

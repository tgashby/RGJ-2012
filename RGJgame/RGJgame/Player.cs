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
    public class Player
    {
        public float JUMP = -1.1f, MOVEMENTSPEED = 0.28f, GRAVITY = 0.08f;
        public static Vector2 PLAYERDRAWPOS = new Vector2(300, 300);
        public const int RUNCYCLE = 20, DETECTIONCYCLE = 50;
        public const float RECOVERY = 0.007f;
        public const float VBOUND = 5f;

        public float health;
        public float detection;
        private float hitTimer;
        public bool hack = false, jump = false, facingLeft = false;
        public bool shielding = false;
        private bool justHit = false;
        private Texture2D standing, running1, running2, jumping, hacking, jumphacking, shield, shield1, shield2, shield3;
        private int runtimer, detectiontimer = DETECTIONCYCLE;

        public PlayerPower powers;

        public Vector2 position, velocity;

        public Player(Vector2 pos)
        {
            health = 1.0f;
            detection = 1.0f;
            hitTimer = 0.0f;

            position = pos;
        }

        public void LoadContent(Game game)
        {
            standing = game.Content.Load<Texture2D>(@"images/guy_standing");
            running1 = game.Content.Load<Texture2D>(@"images/guy_walking1");
            running2 = game.Content.Load<Texture2D>(@"images/guy_walking2");
            jumping = game.Content.Load<Texture2D>(@"images/guy_jumping");
            hacking = game.Content.Load<Texture2D>(@"images/guy_hacking_standing");
            jumphacking = game.Content.Load<Texture2D>(@"images/guy_hacking_jumping");
            shield = game.Content.Load<Texture2D>(@"images/guy_shield");
            shield1 = game.Content.Load<Texture2D>(@"images/playershield1");
            shield2 = game.Content.Load<Texture2D>(@"images/playershield2");
            shield3 = game.Content.Load<Texture2D>(@"images/playershield3");

            powers = new PlayerPower();
            powers.initiate();
            powers.initializeAvail();
        }

        public void update(float dtime)
        {
            if (justHit)
            {
                hitTimer += dtime;

                if (hitTimer >= 1000.0f)
                {
                    justHit = false;
                    hitTimer = 0;
                }
            }

            if (jump)
                velocity.Y += GRAVITY;

            if (!jump)
                shielding = KeyHandler.keyDown(Keys.S);
            else
                shielding = false;

            if (!shielding)
            {
                if (KeyHandler.keyDown(Keys.W))
                {
                    if (!jump)
                    {
                        jump = true;
                        velocity.Y = JUMP;
                        if (powers.check(PlayerPower.SUPER_JUMP)) detection -= 0.05f;
                        if (powers.check(PlayerPower.NORMAL_JUMP)) detection -= 0.0f;
                        if (powers.check(PlayerPower.WEAK_JUMP)) detection -= 0.001f;
                    }
                }
                if (KeyHandler.keyDown(Keys.A))
                {
                    velocity.X += -MOVEMENTSPEED;
                    if (velocity.X < -MOVEMENTSPEED)
                        velocity.X = -MOVEMENTSPEED;
                    runtimer++;

                    if (velocity.X <= 0)
                    {
                        facingLeft = true;
                    }
                }
                if (KeyHandler.keyDown(Keys.D))
                {
                    velocity.X += MOVEMENTSPEED;
                    if (velocity.X > MOVEMENTSPEED)
                        velocity.X = MOVEMENTSPEED;
                    runtimer++;

                    if (velocity.X >= 0)
                    {
                        facingLeft = false;
                    }
                }

                if (!KeyHandler.keyDown(Keys.D) && !KeyHandler.keyDown(Keys.A))
                {
                    velocity.X = 0;
                    runtimer = 0;
                }

                if (Math.Abs(velocity.X) > VBOUND)
                {
                    velocity.X = VBOUND * Math.Sign(velocity.X);
                }
                if (Math.Abs(velocity.Y) > VBOUND)
                {
                    velocity.Y = VBOUND * Math.Sign(velocity.Y);
                }


                position += velocity * dtime;
            }

            updateDetection();
        }

        public void usePower(String str)
        {
            powers.set(str, true);
        }
        public void disablePower(String str)
        {
            powers.set(str, false);
        }


        public void updateDetection()
        {
            if (powers.check(PlayerPower.GRAVITY_OFF)) detection -= 0.01f;
            if (powers.check(PlayerPower.REV_GRAVITY)) detection -= 0.015f;
            if (powers.check(PlayerPower.LOW_GRAV)) detection -= 0.005f;
            if (powers.check(PlayerPower.MASSIVE_GRAV)) detection -= 0.005f;
            if (powers.check(PlayerPower.GRAVITY_NORMAL)) detection -= 0.0f;
            if (powers.check(PlayerPower.MOVEMENT_SLOW)) detection += 0.0008f;
            if (powers.check(PlayerPower.MOVEMENT_NORMAL)) detection -= 0.0f;
            if (powers.check(PlayerPower.MOVEMENT_FAST)) detection -= 0.002f;

            if (powers.check(PlayerPower.OVERCLOCK4)) detection -= 0.006f;
            if (powers.check(PlayerPower.OVERCLOCK2)) detection -= 0.003f;
            if (powers.check(PlayerPower.CLOCK1)) detection -= 0.0f;
            if (powers.check(PlayerPower.UNDERCLOCK2)) detection -= 0.004f;
            if (powers.check(PlayerPower.UNDERCLOCK4)) detection -= 0.008f;

            //recovery will be faster if you are close to being detected, but not if you've been detected...
            if (detectiontimer == DETECTIONCYCLE)
                detection += RECOVERY * (1.0f - detection);

            if (detection > 1)
                detection = 1;
            if (detection < 0)
                detection = 0;

            if (detection == 0)
            {
                detectiontimer--;

                if (detectiontimer == 0)
                {
                    hardReset();
                    detection = 1.0f;
                    detectiontimer = DETECTIONCYCLE;
                }
                else
                {
                    char[] msg = new char[31];
                    Random r = new Random();
                    for (int i = 0; i < 31; i++)
                    {
                        msg[i] = (char)(r.Next(2) + '0');
                    }
                    msg[30] = '\n';

                    LogState.instance.catIntoLog(new String(msg));
                }
            }
        }

        public void hardReset()
        {
            Dictionary<String, Boolean> tmpAvailable = new Dictionary<String, Boolean>(powers.available);

            powers = new PlayerPower();
            powers.initiate();
            powers.available = new Dictionary<String, Boolean>(tmpAvailable);
            LogState.instance.catIntoLog("Resetting...\n");
            Bullets.instance.removeAll(this);
        }

        public Vector2 imageDimension()
        {
            return new Vector2(running1.Width, running1.Height);
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Texture2D toDraw;
            if (jump)
            {
                if (hack)
                    toDraw = jumphacking;
                else
                    toDraw = jumping;
            }
            else if (hack)
            {
                toDraw = hacking;
            }
            else if (shielding)
            {
                toDraw = shield;
            }
            else
            {
                if (velocity.X == 0)
                    toDraw = standing;
                else if (runtimer % RUNCYCLE < RUNCYCLE / 2)
                    toDraw = running1;
                else
                    toDraw = running2;
            }

            SpriteEffects playerDir = new SpriteEffects();

            if (velocity.X < 0)
                facingLeft = true;
            if (velocity.X > 0)
                facingLeft = false;

            if (facingLeft)
            {
                playerDir = SpriteEffects.FlipHorizontally;
            }
    
            spriteBatch.Draw(toDraw, PLAYERDRAWPOS, null, Color.White, 0f, new Vector2(toDraw.Width/2, toDraw.Height/2), 1f, playerDir, 0.9f);

            if (shielding)
            {
                Random r = new Random();
                Texture2D shieldIM = null;
                switch (r.Next(3))
                {
                    case 0: shieldIM = shield1; break;
                    case 1: shieldIM = shield2; break;
                    case 2: shieldIM = shield3; break;
                }
                spriteBatch.Draw(shieldIM, PLAYERDRAWPOS, null, Color.White, 0f, new Vector2(shieldIM.Width / 2, shieldIM.Height / 2), 1f, playerDir, 0.95f);
            }
        }

        public void doCollision(Entity ent)
        {
            if (ent.GetType() == typeof(FlyingEnemy) || 
                ent.GetType() == typeof(SpawnerEnemy) || 
                ent.GetType() == typeof(GuardEnemy))
            {
                if (!justHit)
                {
                    health -= 1.0f;
                    justHit = true;
                }
            }

            if (ent.GetType() == typeof(InfoPad))
            {
                String newPower = ((InfoPad)ent).power;

                if (!powers.isAvailable(newPower))
                {
                    powers.makeAvailable(newPower);
                    LogState.instance.catIntoLog("Discovered: " + newPower + "\n");
                    LogState.instance.catIntoAvailable(newPower);
                    LogState.instance.clearInput();
                }
                else
                {
                    LogState.instance.catIntoLog("Already know " + newPower + "\n");
                }
            }
        }

        public bool isDead()
        {
            return health <= 0;
        }
    }
    

    public sealed class PlayerPower
    {
        public PlayerPower() {}

        Dictionary<String, Boolean> active = new Dictionary<String, Boolean>();
        public Dictionary<String, Boolean> available = new Dictionary<String, Boolean>();

        public void initiate()
        {
            active.Add(GRAVITY_OFF, false);
            active.Add(GRAVITY_NORMAL, true);
            GameState.player.GRAVITY = 0.08f;
            active.Add(MASSIVE_GRAV, false);
            active.Add(REV_GRAVITY, false);
            active.Add(LOW_GRAV, false);
            active.Add(SUPER_JUMP, false);
            active.Add(NORMAL_JUMP, true);
            GameState.player.JUMP = -1.1f;
            active.Add(WEAK_JUMP, false);
            active.Add(MOVEMENT_FAST, false);
            active.Add(MOVEMENT_NORMAL, true);
            GameState.player.MOVEMENTSPEED = 0.28f;
            active.Add(MOVEMENT_SLOW, false);
            active.Add(OVERCLOCK4, false);
            active.Add(OVERCLOCK2, false);
            active.Add(CLOCK1, true);
            Game1.CLOCKSPEED = 1;
            active.Add(UNDERCLOCK2, false);
            active.Add(UNDERCLOCK4, false);
            active.Add(BULLET1, false);
            active.Add(BULLET2, false);
            active.Add(BULLET_SPREAD, false);
            active.Add(BULLET_DIAGONAL, false);
            active.Add(BULLET_TRIPLE, false);
            active.Add(STRONG_BULLETS, false);
            active.Add(FREEZE_ENEMIES, false);
            active.Add(BURN_ENEMIES, false);
            active.Add(THROW_ENEMY, false);
            active.Add(SUPERLAZER, false);
            active.Add(TELEPORT, false);
            active.Add(LAZER, false);
            active.Add(DECREASE_ENEMY_SPEED, false);
            active.Add(GET_ENEMY_ID, false);
            active.Add(KILL_ID, false);
            active.Add(ROOT_PRIV, false);
            active.Add(RESET, true);
        }

        public void initializeAvail()
        {
            available.Add(GRAVITY_OFF, false);
            available.Add(GRAVITY_NORMAL, true);
            available.Add(MASSIVE_GRAV, false);
            available.Add(REV_GRAVITY, false);
            available.Add(LOW_GRAV, false);
            available.Add(SUPER_JUMP, false);
            available.Add(NORMAL_JUMP, true);
            available.Add(WEAK_JUMP, false);
            available.Add(MOVEMENT_FAST, false);
            available.Add(MOVEMENT_NORMAL, true);
            available.Add(MOVEMENT_SLOW, false);
            available.Add(OVERCLOCK4, false);
            available.Add(OVERCLOCK2, false);
            available.Add(CLOCK1, true);
            available.Add(UNDERCLOCK2, false);
            available.Add(UNDERCLOCK4, false);
            available.Add(BULLET1, false);
            available.Add(BULLET2, false);
            available.Add(BULLET_SPREAD, false);
            available.Add(BULLET_DIAGONAL, false);
            available.Add(BULLET_TRIPLE, false);
            available.Add(STRONG_BULLETS, false);
            available.Add(FREEZE_ENEMIES, false);
            available.Add(BURN_ENEMIES, false);
            available.Add(THROW_ENEMY, false);
            available.Add(SUPERLAZER, false);
            available.Add(TELEPORT, false);
            available.Add(LAZER, false);
            available.Add(DECREASE_ENEMY_SPEED, false);
            available.Add(GET_ENEMY_ID, false);
            available.Add(KILL_ID, false);
            available.Add(ROOT_PRIV, false);
            available.Add(RESET, true);
        }

        public bool check(String power)
        {
            return available[power] && active[power];
        }

        public void set(String power, bool b)
        {
            // If setting it false, no need to check if it's available
            if (!b)
                active[power] = b;
            else if (available[power])
                active[power] = b;
        }

        public bool isAvailable(String power)
        {
            try
            {
                return available[power];
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void makeAvailable(String power)
        {
            available[power] = true;
        }

        // Add powers and their string values down here, then go to LogState and add them in parseInput
        public const string LOW_GRAV = "GRAVITY WEAK";
        public const string MASSIVE_GRAV = "GRAVITY STRONG";
        public const string GRAVITY_NORMAL = "GRAVITY NORMAL";
        public const string GRAVITY_OFF = "GRAVITY OFF";
        public const string REV_GRAVITY = "GRAVITY FLIP";
        public const string WEAK_JUMP = "JUMP WEAK";
        public const string SUPER_JUMP = "JUMP STRONG";
        public const string NORMAL_JUMP = "JUMP NORMAL";
        public const string MOVEMENT_FAST = "MOVEMENT FAST";
        public const string MOVEMENT_SLOW = "MOVEMENT SLOW";
        public const string MOVEMENT_NORMAL = "MOVEMENT NORMAL";
        public const string BULLET1 = "SHOOT BULLET";
        public const string BULLET2 = "SHOOT";
        public const string BULLET_SPREAD = "SHOOT SPREAD";
        public const string BULLET_DIAGONAL = "SHOOT DIAGONAL";
        public const string BULLET_TRIPLE = "SHOOT TRIPLE";
        public const string STRONG_BULLETS = "STRONG BULLETS";
        public const string FREEZE_ENEMIES = "FREEZE";
        public const string BURN_ENEMIES = "FLAMES";
        public const string THROW_ENEMY = "THROW NEAREST";
        public const string SUPERLAZER = "WE ARE DOOMED";
        public const string TELEPORT = "TELEPORT"; // Special Case
        public const string LAZER = "LAZAR";
        public const string DECREASE_ENEMY_SPEED = "SLOW EM DOWN";
        public const string GET_ENEMY_ID = "PID NEAREST";
        public const string KILL_ID = "KILL"; // Special Case
        public const string ROOT_PRIV = "LOLZROOT";
        public const string OVERCLOCK4 = "OVERCLOCK 4.0";
        public const string OVERCLOCK2 = "OVERCLOCK 2.0";
        public const string CLOCK1 = "OVERCLOCK 1.0";
        public const string UNDERCLOCK2 = "OVERCLOCK 0.5";
        public const string UNDERCLOCK4 = "OVERCLOCK 0.25";
        public const string RESET = "RESET";
    }
}

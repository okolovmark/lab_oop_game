using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace lab_oop_box_game
{
    /// <summary>
    ///     This is the main type for your game.
    /// </summary>
    public class MyGame : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Gameobject _cannon;
        private Gameobject[] _cannonballs;
        private Gameobject[] _enemies;
        private gameobjser _mycannon;
        private gameobjser[] _mycannonballs;
        private gameobjser[] _myenemies;
        private const int MaxCannonBalls = 2;
        private savegame mysave;
        private Rectangle _viewportRect;
        private KeyboardState _prev = Keyboard.GetState();
        private const int MaxEnimies = 3;
        private const float MaxEnemyHeight = 0.1f;
        private const float MinEnemyHeight = 0.5f;
        private const float MaxEnemyVelocity = 5.0f;
        private const float MinEnemyVelocity = 1.0f;
        
        private bool _gamepause;
        private bool _canaddhealth = true;
        private bool _saverecord = false;
        private bool _savegame = false;
        private readonly Random _random = new Random();
        private int _record;
        private int _score;
        private int _health = 10;
        private SpriteFont _font;
        private Texture2D _backgroundTexture;
        private Texture2D _menuTexture;
        private Texture2D _endTexture;
        private Vector2 _scoreDrawPoint = new Vector2(0.1f, 0.1f);
        private Vector2 _healthDrawPoint = new Vector2(0.82f, 0.01f);
       
        private enum GameState
        {
            MainMenu,
            Gameplay,
            EndOfGame
        }

        private enum GameComplexity
        {
            Easy = 1,
            Normal,
            Hard
        }

        private GameComplexity _complexity = GameComplexity.Easy;
        private GameState _state = GameState.MainMenu;

        public MyGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            //  TargetElapsedTime = new System.TimeSpan(0, 0, 0, 0, 50);
        }

        /// <summary>
        ///     LoadContent will be called once per game and is the place to load
        ///     all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _cannon = new Gameobject(Content.Load<Texture2D>("cannon"));
            _cannon.Position = new Vector2(80, _graphics.GraphicsDevice.Viewport.Height - 80);
            _cannon.Center = new Vector2(0, _cannon.Sprite.Height / 2);
            _viewportRect = new Rectangle(0, 0, _graphics.GraphicsDevice.Viewport.Width,
                                         _graphics.GraphicsDevice.Viewport.Height);
            _cannonballs = new Gameobject[MaxCannonBalls];
            for (var i = 0; i < MaxCannonBalls; i++)
                _cannonballs[i] = new Gameobject(Content.Load<Texture2D>("cannonball"));
            _enemies = new Gameobject[MaxEnimies];
            for (var i = 0; i < MaxEnimies; i++)
                _enemies[i] = new Gameobject(Content.Load<Texture2D>("enemy"));
            _backgroundTexture = Content.Load<Texture2D>("background");
            _menuTexture = Content.Load<Texture2D>("background_menu");
            _endTexture = Content.Load<Texture2D>("background_end");
            _font = Content.Load<SpriteFont>("score");
        }

        /// <summary>
        ///     UnloadContent will be called once per game and is the place to unload
        ///     game-specific content.
        /// </summary>
        protected override void UnloadContent(){}

        /// <summary>
        ///     Allows the game to run logic such as updating the world,
        ///     checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (_state)
            {
                case GameState.MainMenu:
                    UpdateMainMenu();
                    break;
                case GameState.Gameplay:
                    UpdateGameplay(gameTime);
                    break;
                case GameState.EndOfGame:
                    UpdateEndOfGame();
                    break;
            }
        }

        private void UpdateEndOfGame()
        {
            if (_prev.IsKeyUp(Keys.Enter) &&
                Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                _health = 10;
                _score = 0;
                _state = GameState.Gameplay;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!_saverecord)
            {
                _saverecord = true;
                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream fs = new FileStream("record.dat", FileMode.OpenOrCreate))
                {
                    Record myRecord = new Record(_score);
                    try
                    {
                        Record oldRecord = (Record)formatter.Deserialize(fs);
                        if (oldRecord.record < myRecord.record)
                        {
                            formatter.Serialize(fs, myRecord);
                            _record = myRecord.record;
                        }
                        else
                        {
                            _record = oldRecord.record;
                        }
                    }
                    catch (Exception e)
                    {
                        formatter.Serialize(fs, myRecord);
                        _record = myRecord.record;
                        Debug.WriteLine(e);
                    }
                    
                    
                }
               
                
                
            }
            _prev = Keyboard.GetState();
        }

        private void UpdateMainMenu()
        {
            if (_prev.IsKeyUp(Keys.Enter) &&
                Keyboard.GetState().IsKeyDown(Keys.Enter))
                _state = GameState.Gameplay;
            _prev = Keyboard.GetState();
        }

        public void UpdateGameplay(GameTime gameTime)
        {
            if (_gamepause)
            {
                if (_prev.IsKeyUp(Keys.Escape) &&
                    Keyboard.GetState().IsKeyDown(Keys.Escape))
                    _gamepause = false;

                _prev = Keyboard.GetState();
            }

            if (!_gamepause)
            {
                if (_saverecord)
                {
                    _saverecord = false;
                }

                


                if (_prev.IsKeyUp(Keys.Escape) &&
                    Keyboard.GetState().IsKeyDown(Keys.Escape))
                    _gamepause = true;

                if (_prev.IsKeyUp(Keys.Tab) &&
                    Keyboard.GetState().IsKeyDown(Keys.Tab))
                {
                    switch (_complexity)
                    {
                        case GameComplexity.Easy:
                            _complexity = GameComplexity.Normal;
                            break;
                        case GameComplexity.Normal:
                            _complexity = GameComplexity.Hard;
                            break;
                        case GameComplexity.Hard:
                            _complexity = GameComplexity.Easy;
                            break;

                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                    _cannon.Rotation -= 0.1f;
                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                    _cannon.Rotation += 0.1f;
                _cannon.Rotation = MathHelper.Clamp(_cannon.Rotation, -MathHelper.PiOver2, 0);
                if (_prev.IsKeyUp(Keys.Up) && Keyboard.GetState().IsKeyDown(Keys.Up))
                    foreach (var x in _cannonballs)
                        if (!x.Alive)
                        {
                            x.Alive = true;
                            x.Position = _cannon.Position +
                                         new Vector2(_cannon.Sprite.Width * (float) Math.Cos(_cannon.Rotation),
                                                     _cannon.Sprite.Width * (float) Math.Sin(_cannon.Rotation));
                            x.Rotation = _cannon.Rotation;
                            x.Velocity = 5.5f * (int)_complexity *
                                         new Vector2((float) Math.Cos(_cannon.Rotation),
                                                     (float) Math.Sin(_cannon.Rotation));
                            break;
                        }


                UpdateBalls();
                UpdateEnimies();

                _prev = Keyboard.GetState();
                base.Update(gameTime);
                if (_health == 0)
                    _state = GameState.EndOfGame;
                if (_score % 100 == 0 && _canaddhealth && _score != 0)
                {
                    _health++;
                    _canaddhealth = false;
                }
                if (_score % 100 != 0 && !_canaddhealth)
                    _canaddhealth = true;

                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    _savegame = true;
                }
                if (_savegame)
                {
                    SaveGame();
                    _savegame = false;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                {
                   
                    LoadGame();

                    
                }
            }
        }

        private void SaveGame()
        {
            _gamepause = true;

            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("savegame.dat", FileMode.Create))
            {
                try
                {
                    _mycannon = new gameobjser(_cannon.Position.X, _cannon.Position.Y, _cannon.Center.X,
                                               _cannon.Center.Y, _cannon.Velocity.X, _cannon.Velocity.Y,
                                               _cannon.Rotation, _cannon.Alive);
                    _mycannonballs = new gameobjser[MaxCannonBalls];
                    for (int i = 0; i < _mycannonballs.Length; i++)
                    {
                        _mycannonballs[i] = new gameobjser(_cannonballs[i].Position.X, _cannonballs[i].Position.Y, _cannonballs[i].Center.X,
                                               _cannonballs[i].Center.Y, _cannonballs[i].Velocity.X, _cannonballs[i].Velocity.Y,
                                               _cannonballs[i].Rotation, _cannonballs[i].Alive);
                    }
                    _myenemies = new gameobjser[MaxEnimies];
                    for (int i = 0; i < _myenemies.Length; i++)
                    {
                        _myenemies[i] = new gameobjser(_enemies[i].Position.X, _enemies[i].Position.Y, _enemies[i].Center.X,
                                               _enemies[i].Center.Y, _enemies[i].Velocity.X, _enemies[i].Velocity.Y,
                                               _enemies[i].Rotation, _enemies[i].Alive);
                    }
                    mysave = new savegame(_mycannon, _myenemies, _mycannonballs, _score, _health);
                    formatter.Serialize(fs, mysave);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }


            }

            _gamepause = false;
        }

        private void LoadGame()
        {
            _gamepause = true;
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream fs = new FileStream("savegame.dat", FileMode.Open))
            {
                try
                {
                    mysave = (savegame)formatter.Deserialize(fs);
                    _score = mysave._myscore;
                    _health = mysave._myhealth;
                    _cannon.Center = new Vector2(mysave._mycannon.Centerx, mysave._mycannon.Centery);
                    _cannon.Alive = mysave._mycannon.Alive;
                    _cannon.Position = new Vector2(mysave._mycannon.Positionx, mysave._mycannon.Positiony);
                    _cannon.Rotation = mysave._mycannon.Rotation;
                    _cannon.Velocity = new Vector2(mysave._mycannon.Velocityx, mysave._mycannon.Velocityy);
                    try
                    {
                        for (int i = 0; i < MaxCannonBalls; i++)
                        {
                            _cannonballs[i].Center = new Vector2(mysave._mycannonballs[i].Centerx,
                                                                 mysave._mycannonballs[i].Centery);
                            _cannonballs[i].Alive = mysave._mycannonballs[i].Alive;
                            _cannonballs[i].Position = new Vector2(mysave._mycannonballs[i].Positionx,
                                                                   mysave._mycannonballs[i].Positiony);
                            _cannonballs[i].Rotation = mysave._mycannonballs[i].Rotation;
                            _cannonballs[i].Velocity = new Vector2(mysave._mycannonballs[i].Velocityx,
                                                                   mysave._mycannonballs[i].Velocityy);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                    try
                    {
                        for (int i = 0; i < MaxEnimies; i++)
                        {
                            _enemies[i].Center = new Vector2(mysave._myenemies[i].Centerx,
                                                             mysave._myenemies[i].Centery);
                            _enemies[i].Alive = mysave._myenemies[i].Alive;
                            _enemies[i].Position = new Vector2(mysave._myenemies[i].Positionx,
                                                               mysave._myenemies[i].Positiony);
                            _enemies[i].Rotation = mysave._myenemies[i].Rotation;
                            _enemies[i].Velocity = new Vector2(mysave._myenemies[i].Velocityx,
                                                               mysave._myenemies[i].Velocityy);
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }


            }
            _gamepause = false;
        }

        private void UpdateBalls()
        {
            foreach (var x in _cannonballs)
                if (x.Alive)
                {
                    x.Position += x.Velocity;
                    if (!_viewportRect.Contains(new Point((int) x.Position.X, (int) x.Position.Y)))
                        x.Alive = false;
                    var r = new Rectangle((int) x.Position.X - x.Sprite.Width / 2,
                                          (int) x.Position.Y - x.Sprite.Height / 2, x.Sprite.Width, x.Sprite.Height);
                    foreach (var z in _enemies)
                    {
                        var q = new Rectangle((int) z.Position.X - z.Sprite.Width / 2 + 4,
                                              (int) z.Position.Y - z.Sprite.Height / 2, z.Sprite.Width-5, z.Sprite.Height-5);
                        if (q.Intersects(r))
                        {
                            z.Alive = false;
                            x.Alive = false;
                            _score = _score + 1 * (int)_complexity;
                        }
                    }
                }
        }

        private void UpdateEnimies()
        {
            foreach (var enemy in _enemies)
                if (enemy.Alive)
                {
                    enemy.Position += enemy.Velocity;
                    if (
                        !_viewportRect.Contains(new Point((int) enemy.Position.X - enemy.Sprite.Width / 2,
                                                         (int) enemy.Position.Y)))
                    {
                        enemy.Alive = false;
                        _health--;
                    }
                }
                else
                {
                    enemy.Alive = true;
                    enemy.Position = new Vector2(_viewportRect.Right,
                                                 MathHelper.Lerp(_viewportRect.Height * MinEnemyHeight,
                                                                 _viewportRect.Height * MaxEnemyHeight,
                                                                 (float) _random.NextDouble()));
                    enemy.Velocity =
                        new Vector2(
                                    MathHelper.Lerp(-MinEnemyVelocity * (int) _complexity,
                                                    -MaxEnemyVelocity * (int) _complexity, (float) _random.NextDouble()),
                                    0);
                }
        }

        /// <summary>
        ///     This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (_state)
            {
                case GameState.MainMenu:
                    DrawMainMenu();
                    break;
                case GameState.Gameplay:
                    DrawGameplay();
                    break;
                case GameState.EndOfGame:
                    DrawEndOfGame();
                    break;
            }
            base.Draw(gameTime);
        }

        private void DrawEndOfGame()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            _spriteBatch.Draw(_endTexture, Vector2.Zero, null, Color.White, 0, _cannon.Center, 0.735f,
                             SpriteEffects.None, 1f);
            _spriteBatch.DrawString(_font, "you lose!\nyour score: " + _score + "\nrecord: " + _record,
                                   new Vector2(0.4f * _viewportRect.Width,
                                               0.4f * _viewportRect.Height), Color.Yellow);
            _spriteBatch.End();
        }

        private void DrawGameplay()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            //background
            _spriteBatch.Draw(_backgroundTexture, Vector2.Zero, null, Color.White, 0, _cannon.Center, 0.735f,
                             SpriteEffects.None, 1f);

            //cannon
            _spriteBatch.Draw(_cannon.Sprite, _cannon.Position, null, Color.White, _cannon.Rotation, _cannon.Center, 1.0f,
                             SpriteEffects.None, 0.9f);

            //ball
            foreach (var ball in _cannonballs)
                if (ball.Alive)
                    _spriteBatch.Draw(ball.Sprite, ball.Position, null, Color.White, ball.Rotation, ball.Center, 1.0f,
                                     SpriteEffects.None, 0.5f);

            //enemy
            foreach (var enemy in _enemies)
                if (enemy.Alive)
                    _spriteBatch.Draw(enemy.Sprite, enemy.Position, null, Color.White, enemy.Rotation, enemy.Center, 1.0f,
                                     SpriteEffects.None, 0.9f);

            //score
            _spriteBatch.DrawString(_font, "score: " + _score,
                                   new Vector2(_scoreDrawPoint.X * _viewportRect.Width,
                                               _scoreDrawPoint.Y * _viewportRect.Height), Color.Yellow);

            //health
            _spriteBatch.DrawString(_font, "health: " + _health,
                                   new Vector2(_healthDrawPoint.X * _viewportRect.Width,
                                               _healthDrawPoint.Y * _viewportRect.Height), Color.Red);
            _spriteBatch.End();
        }

        private void DrawMainMenu()
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            _spriteBatch.Draw(_menuTexture, Vector2.Zero, null, Color.White, 0, _cannon.Center, 0.735f,
                             SpriteEffects.None, 1f);
            _spriteBatch.End();
        }
    }
}

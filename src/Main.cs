using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GameOfLive.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOfLive
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        private  GraphicsDeviceManager _graphicsDeviceManager;
        private  SpriteBatch _spriteBatch;
        private Texture2D _cellTexture;
        private Texture2D _bacteriaTexture;


        private int _cellSize = 16;

        private int _xCellCount = 110;
        private int _yCellCount = 60;

        private Random _Randomizer = new Random();

        private bool _isRunning = false;

        private List<Bacteria> _bacterias = new List<Bacteria>();

        public Main()
        {
            _graphicsDeviceManager = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();

            _graphicsDeviceManager.PreferredBackBufferHeight = _yCellCount * _cellSize + 1;
            _graphicsDeviceManager.PreferredBackBufferWidth = _xCellCount * _cellSize + 1;
            _graphicsDeviceManager.ApplyChanges();
            IsMouseVisible = true;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _cellTexture = Content.Load<Texture2D>("Images/cell");
            _bacteriaTexture = Content.Load<Texture2D>("Images/bacteria");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private double _generationUpdateTimer = 0.0;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            _generationUpdateTimer = _generationUpdateTimer + gameTime.ElapsedGameTime.TotalMilliseconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (_isRunning)
            {
                if (_generationUpdateTimer > 100)
                {
                    NextGeneration();
                    _generationUpdateTimer = 0;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _isRunning = !_isRunning;
                if (_isRunning)
                {
                    GenerateBacterias();
                }
                else
                    _bacterias.Clear();
                
                Thread.Sleep(500);
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        //Generates the list of bacterias.
        private void GenerateBacterias()
        {
            for (int y = 0; y < _xCellCount; y++)
            {
                for (int x = 0; x < _xCellCount; x++)
                {
                    var newBacteria = new Bacteria()
                    {
                        X = x,
                        Y = y,
                        IsAlive = GenerateBacteriaAliveStatus(),
                        Neighbours = new List<Bacteria>()
                    };
                    SetNeighboursOfBacteriaByPosition(newBacteria);
                    _bacterias.Add(newBacteria);
                }
            }
        }

        private bool GenerateBacteriaAliveStatus()
        {
            int aliveStatusIndex = _Randomizer.Next(0, 2);
            return aliveStatusIndex == 1;
        }

        private void SetNeighboursOfBacteriaByPosition(Bacteria bacteria)
        {
            for (int yPos = bacteria.Y - 1; yPos <= bacteria.Y + 1; yPos++)
            {
                if (yPos >= 0 && yPos <= _yCellCount -1)
                {
                    for (int xPos = bacteria.X - 1; xPos <= bacteria.X + 1; xPos++)
                    {
                        if (xPos >= 0 && xPos <= _xCellCount - 1)
                        {
                            var neighbourBacteria = _bacterias.FirstOrDefault(x => x.X == xPos && x.Y == yPos);
                            if (neighbourBacteria != null)
                            {
                                bacteria.Neighbours.Add(neighbourBacteria);
                                neighbourBacteria.Neighbours.Add(bacteria);
                            }
                        }
                    }
                }
            }
        }

        private void NextGeneration()
        {
            _bacterias.ForEach(x => x.WasAlive = x.IsAlive);

            foreach (var bacteria in _bacterias)
            {
                var neighbourCount = bacteria.Neighbours.Where(x=>x.WasAlive).Count();
                if (bacteria.IsAlive)
                {
                    if (neighbourCount < 2)
                        bacteria.IsAlive = false;
                    if (neighbourCount == 2 || neighbourCount == 3)
                        bacteria.IsAlive = true;
                    if (neighbourCount > 3)
                        bacteria.IsAlive = false;
                }
                else
                {
                    if (neighbourCount == 3)
                        bacteria.IsAlive = true;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            //Draws the bacterias
            foreach (var bacteria in _bacterias)
            {
                if (bacteria.IsAlive)
                    _spriteBatch.Draw(_bacteriaTexture, new Rectangle(bacteria.X * _cellSize, bacteria.Y * _cellSize, _cellSize+1, _cellSize+1), Color.White);
            }

            //Draws the grid
            for (int y = 0; y < _xCellCount; y++)
            {
                for (int x = 0; x < _xCellCount; x++)
                {
                    _spriteBatch.Draw(_cellTexture, new Rectangle(x * _cellSize, y * _cellSize, _cellSize+1, _cellSize+1),Color.White);
                }
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

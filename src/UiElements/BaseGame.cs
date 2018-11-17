using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using GameOfLive.Bacteria;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOfLive.UiElements
{
    public class BaseGame : IUiElement
    {
        private GraphicsDevice _graphicsDevice;
        private ContentManager _content;
        private SpriteBatch _spriteBatch;

        //Textures
        private Texture2D _cellTexture;

        private Texture2D _bacteriaTexture;

        //Cell informations
        public int CellSize { get; } = 16;
        public int CellCountX { get; } = 50;
        public int CellCountY { get; } = 50;

        /// <summary>
        /// Update rate in milliseconds
        /// </summary>
        public int UpdateRate { get; } = 100;

        private double _updateRateTimer;
        public bool IsRunning;

        public BacteriaManager BacteriaManager;
        private List<Bacteria.Bacteria> _bacterias = new List<Bacteria.Bacteria>();

        public void Initialize(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
            _content = content ?? throw new ArgumentNullException(nameof(content));
            _spriteBatch = new SpriteBatch(_graphicsDevice);
            BacteriaManager = new BacteriaManager();
        }

        public void LoadContent()
        {
            _cellTexture = _content.Load<Texture2D>("Images/cell");
            _bacteriaTexture = _content.Load<Texture2D>("Images/bacteria");
        }

        public void UnloadContent()
        {
            _cellTexture.Dispose();
            _bacteriaTexture.Dispose();
            _content.Unload();
        }

        public void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            var mouseState = Mouse.GetState();

            _updateRateTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (IsRunning)
            {
                if (!(_updateRateTimer > UpdateRate)) return;

                BacteriaManager.GenerateNextGeneration(_bacterias);
                _updateRateTimer = 0;
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                var bacteria =
                    _bacterias.FirstOrDefault(x =>
                        x.X == mouseState.Position.X / CellSize && x.Y == mouseState.Position.Y / CellSize);
                if (bacteria != null)
                    bacteria.IsAlive = !bacteria.IsAlive;
            }
        }

        public void Start()
        {
            IsRunning = true;
            _bacterias = BacteriaManager.GenerateBacterias(CellCountX, CellCountY);
        }

        public void Stop()
        {
            IsRunning = false;
            _bacterias.Clear();
        }

        public void Draw(GameTime gameTime)
        {
            if (_spriteBatch == null) return;

            _spriteBatch.Begin();

            //Draws the bacterias
            foreach (var bacteria in _bacterias)
            {
                if (bacteria.IsAlive)
                    _spriteBatch.Draw(_bacteriaTexture,
                        new Rectangle(bacteria.X * CellSize, bacteria.Y * CellSize, CellSize + 1, CellSize + 1),
                        Color.White);
            }

            //Draws the grid
            for (int y = 0; y < CellCountY; y++)
            {
                for (int x = 0; x < CellCountX; x++)
                {
                    _spriteBatch.Draw(_cellTexture,
                        new Rectangle(x * CellSize, y * CellSize, CellSize + 1, CellSize + 1), Color.White);
                }
            }
            _spriteBatch.End();
        }
    }
}
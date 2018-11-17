using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOfLive.UiElements
{
    public class SideMenu :IUiElement
    {
        private GraphicsDevice _graphicsDevice;
        private ContentManager _content;
        private SpriteBatch _spriteBatch;
        private BaseGame _baseGame;

        private double _mouseClickTimer;
        private Rectangle _btnStartStopRectangle;

        //Content
        private SpriteFont _mainFont;
        private Texture2D _customColor;
        private Texture2D _startBtn;
        private Texture2D _stopBtn;

        public void Initialize(GraphicsDevice graphicsDevice, ContentManager content)
        {
            _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
            _content = content ?? throw new ArgumentNullException(nameof(content));
            _spriteBatch = new SpriteBatch(_graphicsDevice);
        }

        public void Initialize(GraphicsDevice graphicsDevice, ContentManager content, BaseGame baseGame)
        {
            _graphicsDevice = graphicsDevice ?? throw new ArgumentNullException(nameof(graphicsDevice));
            _content = content ?? throw new ArgumentNullException(nameof(content));
            _baseGame = baseGame ?? throw new ArgumentNullException(nameof(baseGame));

            _spriteBatch = new SpriteBatch(_graphicsDevice);
            _btnStartStopRectangle = new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 25,
                _baseGame.CellCountY * _baseGame.CellSize + 1 - 50, 208, 42);
        }
        public void LoadContent()
        {
            _mainFont = _content.Load<SpriteFont>("MainFont");
            _customColor = _content.Load<Texture2D>("Images/CustomColor");
            _startBtn = _content.Load<Texture2D>("Images/start_btn");
            _stopBtn = _content.Load<Texture2D>("Images/stop_btn");
        }

        public void UnloadContent()
        {
        }


        public void Update(GameTime gameTime)
        {
            _mouseClickTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_mouseClickTimer > 200)
                {
                    if (new Rectangle(mouseState.X, mouseState.Y, 1, 1).Intersects(_btnStartStopRectangle))
                    {
                        if (_baseGame.IsRunning)
                            _baseGame.Stop();
                        else
                            _baseGame.Start();
                        _mouseClickTimer = 0;
                    }
                }
            }
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            if (_baseGame != null)
            {
                _spriteBatch.Draw(_customColor, new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1, 5, 1, _baseGame.CellCountX * _baseGame.CellSize -10), new Color(72,72,72));

                if (_baseGame.IsRunning)
                {
                    _spriteBatch.Draw(_stopBtn, 
                        new Vector2(_btnStartStopRectangle.X,
                            _btnStartStopRectangle.Y), Color.White);
                    _spriteBatch.DrawString(_mainFont, "Status: running",
                        new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 10), Color.Lime);
                }
                else
                {
                    _spriteBatch.Draw(_startBtn,
                        new Vector2(_btnStartStopRectangle.X,
                            _btnStartStopRectangle.Y), Color.White);
                    _spriteBatch.DrawString(_mainFont, "Status: not running", 
                        new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 10), Color.Red);
                }
                _spriteBatch.DrawString(_mainFont, "Current generation: 0", new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 30), Color.White);
                _spriteBatch.DrawString(_mainFont, $"X-axis cell count: {_baseGame.CellCountX}", new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 50), Color.White);
                _spriteBatch.DrawString(_mainFont, $"Y-axis cell count: {_baseGame.CellCountY}", new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 70), Color.White);
                _spriteBatch.DrawString(_mainFont, $"Update rate: {_baseGame.UpdateRate}", new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 90), Color.White);
                _spriteBatch.DrawString(_mainFont, $"Birth rate: {_baseGame.BacteriaManager.IsAliveChance.ToString("0.00")}%", new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 110), Color.White);

            }

            _spriteBatch.End();
        }
    }
}

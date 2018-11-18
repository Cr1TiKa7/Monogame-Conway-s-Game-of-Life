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

        //Rectangles for the buttons
        private Rectangle _btnStartStopRectangle;
        private Rectangle _btnPlusXAxisButtonRectangle;
        private Rectangle _btnMinusXAxisButtonRectangle;
        private Rectangle _btnPlusYAxisButtonRectangle;
        private Rectangle _btnMinusYAxisButtonRectangle;
        private Rectangle _btnPlusUpdateRateButtonRectangle;
        private Rectangle _btnMinusUpdateRateButtonRectangle;
        private Rectangle _btnPlusBirthRateButtonRectangle;
        private Rectangle _btnMinusBirthRateButtonRectangle;

        //Content
        private SpriteFont _mainFont;
        private Texture2D _customColor;
        private Texture2D _startBtn;
        private Texture2D _stopBtn;
        private Texture2D _plusBtn;
        private Texture2D _minusBtn;

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

            //Setting up the rectangles for the buttons
            _btnPlusXAxisButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 160, 50, 16, 16);
            _btnMinusXAxisButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 180, 50, 16, 16);
            _btnPlusYAxisButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 160, 70, 16, 16);
            _btnMinusYAxisButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 180, 70, 16, 16);
            _btnPlusUpdateRateButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 160, 90, 16, 16);
            _btnMinusUpdateRateButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 180, 90, 16, 16);
            _btnPlusBirthRateButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 160, 110, 16, 16);
            _btnMinusBirthRateButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 180, 110, 16, 16);

            _btnStartStopRectangle = new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 25,
                _baseGame.CellCountY * _baseGame.CellSize + 1 - 50, 208, 42);
        }
        public void LoadContent()
        {
            _mainFont = _content.Load<SpriteFont>("MainFont");
            _customColor = _content.Load<Texture2D>("Images/CustomColor");
            _startBtn = _content.Load<Texture2D>("Images/start_btn");
            _stopBtn = _content.Load<Texture2D>("Images/stop_btn");
            _plusBtn = _content.Load<Texture2D>("Images/plus_btn");
            _minusBtn = _content.Load<Texture2D>("Images/minus_btn");
        }

        public void UnloadContent()
        {
            _mainFont = null;
            _customColor.Dispose();
            _startBtn.Dispose();
            _stopBtn.Dispose();
            _plusBtn.Dispose();
            _minusBtn.Dispose();
            _content.Unload();
        }


        public void Update(GameTime gameTime)
        {
            _mouseClickTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            MouseState mouseState = Mouse.GetState();

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                var mouseRect = new Rectangle(mouseState.X, mouseState.Y, 1, 1);
                if (_mouseClickTimer > 200)
                {
                    if (mouseRect.Intersects(_btnStartStopRectangle))
                    {
                        if (_baseGame.IsRunning)
                            _baseGame.Stop();
                        else
                            _baseGame.Start();
                        _mouseClickTimer = 0;
                    }
                    if (!_baseGame.IsRunning)
                    {
                        if (mouseRect.Intersects(_btnPlusXAxisButtonRectangle))
                        {
                            _baseGame.CellCountX = _baseGame.CellCountX + 1;
                            _mouseClickTimer = 0;
                        }
                        else if (mouseRect.Intersects(_btnMinusXAxisButtonRectangle))
                        {
                            _baseGame.CellCountX = _baseGame.CellCountX - 1;
                            _mouseClickTimer = 0;
                        }
                        else if (mouseRect.Intersects(_btnPlusYAxisButtonRectangle))
                        {
                            _baseGame.CellCountY = _baseGame.CellCountY + 1;
                            _mouseClickTimer = 0;
                        }
                        else if (mouseRect.Intersects(_btnMinusYAxisButtonRectangle))
                        {
                            _baseGame.CellCountY = _baseGame.CellCountY - 1;
                            _mouseClickTimer = 0;
                        }
                        else if (mouseRect.Intersects(_btnPlusBirthRateButtonRectangle))
                        {
                            _baseGame.BacteriaManager.SetBirthRate(_baseGame.BacteriaManager.MaxNumber - 1);
                            _mouseClickTimer = 0;
                        }
                        else if (mouseRect.Intersects(_btnMinusBirthRateButtonRectangle))
                        {
                            _baseGame.BacteriaManager.SetBirthRate(_baseGame.BacteriaManager.MaxNumber + 1);
                            _mouseClickTimer = 0;
                        }
                        UpdateRectangles();
                    }
                    if (mouseRect.Intersects(_btnPlusUpdateRateButtonRectangle))
                    {
                        _baseGame.UpdateRate = _baseGame.UpdateRate + 10;
                        _mouseClickTimer = 0;
                    }
                    else if (mouseRect.Intersects(_btnMinusUpdateRateButtonRectangle))
                    {
                        _baseGame.UpdateRate = _baseGame.UpdateRate - 10;
                        _mouseClickTimer = 0;
                    }
                }
            }
        }

        private void UpdateRectangles()
        {
            _btnPlusXAxisButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 160, 50, 16, 16);
            _btnMinusXAxisButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 180, 50, 16, 16);
            _btnPlusYAxisButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 160, 70, 16, 16);
            _btnMinusYAxisButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 180, 70, 16, 16);
            _btnPlusUpdateRateButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 160, 90, 16, 16);
            _btnMinusUpdateRateButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 180, 90, 16, 16);
            _btnPlusBirthRateButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 160, 110, 16, 16);
            _btnMinusBirthRateButtonRectangle =
                new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 180, 110, 16, 16);
            _btnStartStopRectangle = new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1 + 25,
                _baseGame.CellCountY * _baseGame.CellSize + 1 - 50, 208, 42);
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            if (_baseGame != null)
            {
                _spriteBatch.Draw(_customColor, new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1, 5, 1, _baseGame.CellCountX * _baseGame.CellSize -10), new Color(72,72,72));

                //Plus and minus button for the x-axis cell count
                _spriteBatch.Draw(_plusBtn,_btnPlusXAxisButtonRectangle,Color.White);
                _spriteBatch.Draw(_minusBtn, _btnMinusXAxisButtonRectangle, Color.White);
                //Plus and minus button for the y-axis cell count
                _spriteBatch.Draw(_plusBtn,_btnPlusYAxisButtonRectangle,Color.White);
                _spriteBatch.Draw(_minusBtn,_btnMinusYAxisButtonRectangle,Color.White);
                //Plus and minus button for the update rate
                _spriteBatch.Draw(_plusBtn, _btnPlusUpdateRateButtonRectangle,Color.White);
                _spriteBatch.Draw(_minusBtn,_btnMinusUpdateRateButtonRectangle,Color.White);
                //Plus and minus button for the birth rate
                _spriteBatch.Draw(_plusBtn, _btnPlusBirthRateButtonRectangle,Color.White);
                _spriteBatch.Draw(_minusBtn, _btnMinusBirthRateButtonRectangle,Color.White);

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
                _spriteBatch.DrawString(_mainFont, $"Current generation: {_baseGame.BacteriaManager.Generation}", new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 30), Color.White);
                _spriteBatch.DrawString(_mainFont, $"X-axis cell count: {_baseGame.CellCountX}", new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 50), Color.White);
                _spriteBatch.DrawString(_mainFont, $"Y-axis cell count: {_baseGame.CellCountY}", new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 70), Color.White);
                _spriteBatch.DrawString(_mainFont, $"Update rate: {_baseGame.UpdateRate}", new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 90), Color.White);
                _spriteBatch.DrawString(_mainFont, $"Birth rate: {_baseGame.BacteriaManager.IsAliveChance.ToString("0.00")}%", new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 110), Color.White);

            }

            _spriteBatch.End();
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameOfLive.UiElements
{
    public class SideMenu :IUiElement
    {
        private GraphicsDevice _graphicsDevice;
        private ContentManager _content;
        private SpriteBatch _spriteBatch;
        private BaseGame _baseGame;

        //Content
        private SpriteFont _mainFont;
        private Texture2D _customColor;

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
        }
        public void LoadContent()
        {
            _mainFont = _content.Load<SpriteFont>("MainFont");
            _customColor = _content.Load<Texture2D>("Images/CustomColor");
        }

        public void UnloadContent()
        {
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin();

            if (_baseGame != null)
            {
                _spriteBatch.Draw(_customColor, new Rectangle(_baseGame.CellCountX * _baseGame.CellSize + 1, 5, 1, _baseGame.CellCountX * _baseGame.CellSize -10), new Color(72,72,72));

                if (_baseGame.IsRunning)
                    _spriteBatch.DrawString(_mainFont, "Status: running", new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 10), Color.Lime);
                else
                    _spriteBatch.DrawString(_mainFont, "Status: not running", new Vector2(_baseGame.CellCountX * _baseGame.CellSize + 1 + 10, 10), Color.Red);
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

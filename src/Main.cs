using System.Collections.Generic;
using System.Linq;
using GameOfLive.UiElements;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameOfLive
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        private readonly List<IUiElement> _uiElements = new List<IUiElement>();
        private readonly GraphicsDeviceManager _graphicsDeviceManager;
        private SpriteBatch _spriteBatch;

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
            //Initialize Uielements
            var newBaseGame = new BaseGame();
            var newSideMenu = new SideMenu();
            newBaseGame.Initialize(GraphicsDevice, new ContentManager(Content.ServiceProvider, Content.RootDirectory));
            newSideMenu.Initialize(GraphicsDevice, new ContentManager(Content.ServiceProvider, Content.RootDirectory),
                newBaseGame);

            _uiElements.Add(newBaseGame);
            _uiElements.Add(newSideMenu);

            //Set width and Height of application
            _graphicsDeviceManager.PreferredBackBufferHeight = newBaseGame.CellCountY * newBaseGame.CellSize + 1;
            _graphicsDeviceManager.PreferredBackBufferWidth = newBaseGame.CellCountX * newBaseGame.CellSize + 1 + 250;
            _graphicsDeviceManager.ApplyChanges();

            //Set mouse visibility to true
            IsMouseVisible = true;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (var uiElement in _uiElements)
                uiElement.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            foreach (var uiElement in _uiElements)
                uiElement.UnloadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (_uiElements?.Any() == true)
            {
                _graphicsDeviceManager.PreferredBackBufferHeight =
                    (_uiElements[0] as BaseGame).CellCountY * (_uiElements[0] as BaseGame).CellSize + 1;
                _graphicsDeviceManager.PreferredBackBufferWidth =
                    (_uiElements[0] as BaseGame).CellCountX * (_uiElements[0] as BaseGame).CellSize + 1 + 250;
                _graphicsDeviceManager.ApplyChanges();

            }
            foreach (var uiElement in _uiElements)
                uiElement.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(40, 40, 40));

            foreach (var uiElement in _uiElements)
                uiElement.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
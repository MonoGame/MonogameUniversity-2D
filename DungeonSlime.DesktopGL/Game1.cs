using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace DungeonSlime;

public class Game1 : Core
{
    private Texture2D _logo;
    private Vector2 _logoCenter;
    private Vector2 screenCenter;

    public Game1() : base("Dungeon Slime", 1280, 720, false)
    {
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
    }

    protected override void LoadContent()
    {
        screenCenter = new Vector2(Window.ClientBounds.Width, Window.ClientBounds.Height) * 0.5f;

        // TODO: use this.Content to load your game content here
        _logo = Content.Load<Texture2D>("Textures/logo");
        _logoCenter = new Vector2(_logo.Width, _logo.Height) * 0.5f;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        SpriteBatch.Begin();
        Console.WriteLine($"");
        // Draw the logo texture.
        SpriteBatch.Draw(
            _logo,          // texture
            new Vector2(    // position
                (Window.ClientBounds.Width * 0.5f) - (_logo.Width * 0.5f),
                (Window.ClientBounds.Height * 0.5f) - (_logo.Height * 0.5f)),
            Color.White     // color
        );

        SpriteBatch.Draw(
            _logo,              // texture
            new Vector2(        // position
                (screenCenter.X) - (_logoCenter.X),
                (screenCenter.Y) - (_logoCenter.Y)),
            Color.White   // color
        );
        Console.WriteLine($"Screen - Width - {(Window.ClientBounds.Width * 0.5f)}/ {screenCenter.X} - Height - {(Window.ClientBounds.Height * 0.5f)}/ {screenCenter.Y}");
        Console.WriteLine($"Logo - Width - {(_logo.Width * 0.5f)}/ {_logoCenter.X} - Height - {(_logo.Height * 0.5f)}/ {_logoCenter.Y}");

        SpriteBatch.End();

        base.Draw(gameTime);
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameLibrary.Input;

/// <summary>
/// Keys
/// DPAD move
/// WASD Move
/// Button A speed
/// Space Speed
/// P / Button Y = Pause/Play Music
/// </summary>
public class InputManager
{
    /// <summary>
    /// Gets the state information of keyboard input.
    /// </summary>
    public KeyboardInfo Keyboard { get; private set; }

    /// <summary>
    /// Gets the state information of mouse input.
    /// </summary>
    public MouseInfo Mouse { get; private set; }

    /// <summary>
    /// Gets the state information of a gamepad.
    /// </summary>
    public GamePadInfo[] GamePads { get; private set; }

    /// <summary>
    /// Creates a new InputManager.
    /// </summary>
    public InputManager()
    {
        Keyboard = new KeyboardInfo();
        Mouse = new MouseInfo();

        GamePads = new GamePadInfo[4];
        for (int i = 0; i < 4; i++)
        {
            GamePads[i] = new GamePadInfo((PlayerIndex)i);
        }
    }

    /// <summary>
    /// Updates the state information for the keyboard, mouse, and gamepad inputs.
    /// </summary>
    /// <param name="gameTime">A snapshot of the timing values for the current frame.</param>
    public void Update(GameTime gameTime)
    {
        Keyboard.Update(gameTime);
        Mouse.Update(gameTime);

        for (int i = 0; i < 4; i++)
        {
            GamePads[i].Update(gameTime);
        }
    }

    public bool IncreaseSpeed()
    {
        var gamePadOne = GamePads[(int)PlayerIndex.One];

        return Keyboard.IsKeyDown(Keys.Space) || gamePadOne.IsButtonDown(Buttons.A);
    }

    public bool MoveLeft()
    {
        var gamePadOne = GamePads[(int)PlayerIndex.One];

        return Keyboard.WasKeyJustPressed(Keys.A) ||
               Keyboard.WasKeyJustPressed(Keys.Left) ||
               gamePadOne.WasButtonJustPressed(Buttons.DPadUp) ||
               gamePadOne.WasButtonJustPressed(Buttons.LeftThumbstickLeft);
    }

    public bool MoveRight()
    {
        var gamePadOne = GamePads[(int)PlayerIndex.One];

        return Keyboard.WasKeyJustPressed(Keys.D) || 
               Keyboard.WasKeyJustPressed(Keys.Right) ||
               gamePadOne.WasButtonJustPressed(Buttons.DPadRight) ||
               gamePadOne.WasButtonJustPressed(Buttons.LeftThumbstickRight);
    }

    public bool MoveUp()
    {
        var gamePadOne = GamePads[(int)PlayerIndex.One];

        return Keyboard.WasKeyJustPressed(Keys.W) ||
               Keyboard.WasKeyJustPressed(Keys.Up) ||
               gamePadOne.WasButtonJustPressed(Buttons.DPadUp) ||
               gamePadOne.WasButtonJustPressed(Buttons.DPadUp);
    }

    public bool MoveDown()
    {
        var gamePadOne = GamePads[(int)PlayerIndex.One];

        return Keyboard.WasKeyJustPressed(Keys.S) ||
               Keyboard.WasKeyJustPressed(Keys.Down) ||
               gamePadOne.WasButtonJustPressed(Buttons.DPadDown) ||
               gamePadOne.WasButtonJustPressed(Buttons.DPadDown);
    }

    public bool PausePlayMusic()
    {
        var gamePadOne = GamePads[(int)PlayerIndex.One];

        return Keyboard.WasKeyJustPressed(Keys.P) || gamePadOne.IsButtonDown(Buttons.Start);
    }

    public bool AudioUp()
    {
        var gamePadOne = GamePads[(int)PlayerIndex.One];

        return Keyboard.WasKeyJustPressed(Keys.OemPlus) || gamePadOne.IsButtonDown(Buttons.DPadUp);
    }

    public bool AudioDown()
    {
        var gamePadOne = GamePads[(int)PlayerIndex.One];
        // Don't to this"!
        return Keyboard.WasKeyJustPressed(Keys.OemMinus) || gamePadOne.IsButtonDown(Buttons.DPadDown);
    }

    public bool PausedPressed()
    {
        var gamePadOne = GamePads[(int)PlayerIndex.One];

        return Core.Input.Keyboard.WasKeyJustPressed(Keys.Escape) || gamePadOne.IsButtonDown(Buttons.Start);
    }

    /// <summary>
    /// Returns true if the player has triggered the "action" button,
    /// typically used for menu confirmation.
    /// </summary>
    public bool Action()
    {
        var gamePadOne = GamePads[(int)PlayerIndex.One];

        return Core.Input.Keyboard.WasKeyJustPressed(Keys.Enter) ||
               gamePadOne.WasButtonJustPressed(Buttons.A);
    }    
}


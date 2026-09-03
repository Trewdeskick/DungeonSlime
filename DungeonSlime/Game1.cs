using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System;

namespace DungeonSlime;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;

    // Effects
    private SpriteBatch _spriteBatch;

    private Song _bgm;
    private Color previousColor;
    private Color targetColor = Color.CornflowerBlue;
    private Color backgroundColor;

    // Frame Stuff
    private double _elapsedTime = 0;
    private int _frameRateTracker = 0;
    private int frameCounter = 0;
    private const int targetFrames = 30;

    // Bouncing Ball Info
    private Vector2 _ballPosition;
    private Vector2 _ballVelocity;
    private Texture2D _logo;


    // make it like galaga and add sprites that look like: >:(
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        //set target to 60 fps
        TargetElapsedTime = TimeSpan.FromSeconds(1.0 / 60.0);

        IsFixedTimeStep = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        Window.AllowUserResizing = true;

        _graphics.PreferredBackBufferWidth = 1600;
        _graphics.PreferredBackBufferHeight = 1000;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _logo = Content.Load<Texture2D>(@"images\download");

        _bgm = Content.Load<Song>("Soulja Boy Tell'em - Turn My Swag On (Official Video)");

        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.5f;
        MediaPlayer.Play(_bgm);

        // Position the ball in the center of the screen
        _ballPosition.X = GraphicsDevice.Viewport.Width / 2;
        _ballPosition.Y = GraphicsDevice.Viewport.Height / 2;

        // Give the ball a random velocity
        System.Random rand = new();
        _ballVelocity.X = (float)rand.NextDouble();
        _ballVelocity.Y = (float)rand.NextDouble();
        _ballVelocity.Normalize();
        _ballVelocity *= 1000;

        Console.WriteLine(_ballVelocity.X);
        Console.WriteLine(_ballVelocity.Y);

        // TODO: use this.Content to load your game content here
    }

    // protected void playerMovement()
    // {
        
    //     KeyboardState pressedKey = Keyboard.GetState();

    //     if (velocity.Y > 0+_logo.Height && (pressedKey.IsKeyDown(Keys.W) || pressedKey.IsKeyDown(Keys.Up)))
    //     {
    //         Console.WriteLine("Player Jumps");
    //         velocity.Y -= 100;
    //     }
    //     if (velocity.Y < Window.ClientBounds.Height - _logo.Height && (pressedKey.IsKeyDown(Keys.S) || pressedKey.IsKeyDown(Keys.Down)))
    //     {
    //         Console.WriteLine("Player Falls");
    //         velocity.Y += 100;
    //     }
    //     if (velocity.X > 0+_logo.Width && (pressedKey.IsKeyDown(Keys.A) || pressedKey.IsKeyDown(Keys.Left)))
    //     {
    //         Console.WriteLine("Player Moves Left");
    //         velocity.X -= 100;
    //     }
    //     if (velocity.X < Window.ClientBounds.Width - _logo.Width && (pressedKey.IsKeyDown(Keys.D) || pressedKey.IsKeyDown(Keys.Right)))
    //     {
    //         Console.WriteLine("Player Moves Right");
    //         velocity.X += 100;
    //     }
    

    // }


    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _ballPosition += _ballVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

        if(_ballPosition.X < GraphicsDevice.Viewport.X || _ballPosition.X > GraphicsDevice.Viewport.Width - _logo.Bounds.Width)
        {
            _ballVelocity.X *= -1;
        }
        if (_ballPosition.Y < GraphicsDevice.Viewport.Y || _ballPosition.Y > GraphicsDevice.Viewport.Height - _logo.Bounds.Height)
        {
            _ballVelocity.Y *= -1;
        }

        _elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
        _frameRateTracker++;

        if(_elapsedTime >= 1.0)
        {
            Window.Title = $"FPS: {_frameRateTracker}";
            _frameRateTracker = 0;
            _elapsedTime -= 1.0;
        }

        // Console.WriteLine($"Position: {_ballPosition} Velocity: {_ballVelocity}");
        // Console.WriteLine($"Logo Width: {_logo.Bounds.Width}, Logo Height: {_logo.Bounds.Height}");

        //playerMovement();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        // TODO: Add your drawing code here

        frameCounter++;

        // goes off at 120 frames
        if (frameCounter >= targetFrames)
        {
            updateColor();
            frameCounter = 0;
        }

        float amount = (float)frameCounter / targetFrames;

        backgroundColor = Color.Lerp(previousColor, targetColor, amount);

        GraphicsDevice.Clear(backgroundColor);

        // Begin the sprite batch to prepare for rendering.
        _spriteBatch.Begin();

        //Draw the texture
        _spriteBatch.Draw(_logo, _ballPosition, Color.White);

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    protected void updateColor()
    {
        previousColor = targetColor;

        Random rand = new();

        int r = rand.Next(0,256);
        int g = rand.Next(0,256);
        int b = rand.Next(0,256);

        targetColor = new Color(r,g,b);

    }
}

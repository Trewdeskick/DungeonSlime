using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

using System;

namespace DungeonSlime;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private readonly Random rand;

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

    // Bouncing 'Ball' Info
    private Texture2D _logo;
    private Physics[] _kittys = new Physics[5];

    public struct Physics
    {
        public Vector2 _ballPosition;
        public Vector2 _ballVelocity;

        public Physics(Vector2 position, Vector2 velocity)
        {
            _ballPosition = position;
            _ballVelocity = velocity;
        }
    }


    // make it like galaga and add sprites that look like: >:(
    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        rand = new Random();
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

        _graphics.PreferredBackBufferWidth = 1920;
        _graphics.PreferredBackBufferHeight = 1080;
        _graphics.ApplyChanges();

        int _screenWidth = GraphicsDevice.Viewport.Width;
        int _screenHeight = GraphicsDevice.Viewport.Height;

        for (int i = 0; i < _kittys.Length; i++)
        {

            // Position the ball in the center of the screen
            Vector2 _initialPosition = new Vector2(
                rand.Next(0, _screenWidth / 2),
                rand.Next(0, _screenHeight / 2)
            );

            // Give the ball a random velocity
            Vector2 _initialVelocity = new Vector2 (
                (float)rand.NextDouble(),
                (float)rand.NextDouble()
            );

            _initialVelocity.Normalize();
            _initialVelocity *= 1000f;

            _kittys[i] = new Physics(_initialPosition, _initialVelocity);
        }

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


        for (int i = 0; i < _kittys.Length; i++)
        {
            _kittys[i]._ballPosition += _kittys[i]._ballVelocity *
                (float)gameTime.ElapsedGameTime.TotalSeconds;

         if(_kittys[i]._ballPosition.X < GraphicsDevice.Viewport.X ||
            _kittys[i]._ballPosition.X > GraphicsDevice.Viewport.Width - _logo.Bounds.Width)
            {
                _kittys[i]._ballVelocity.X *= -1;
            }
            if (_kittys[i]._ballPosition.Y < GraphicsDevice.Viewport.Y ||
                _kittys[i]._ballPosition.Y > GraphicsDevice.Viewport.Height - _logo.Bounds.Height)
            {
               _kittys[i]._ballVelocity.Y *= -1;
            }
        }

        ResolveKittyCollisions();

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

    // AI GENERATED CODE
    private void ResolveKittyCollisions()
    {
        Vector2 spriteSize = new(_logo.Width, _logo.Height);
        Vector2 halfSpriteSize = spriteSize / 2f;

        for (int first = 0; first < _kittys.Length - 1; first++)
        {
            for (int second = first + 1; second < _kittys.Length; second++)
            {
                Vector2 firstCenter = _kittys[first]._ballPosition + spriteSize / 2f;
                Vector2 secondCenter = _kittys[second]._ballPosition + spriteSize / 2f;
                Vector2 separation = secondCenter - firstCenter;
                float overlapX = (halfSpriteSize.X * 2f) - MathF.Abs(separation.X);
                float overlapY = (halfSpriteSize.Y * 2f) - MathF.Abs(separation.Y);

                if (overlapX <= 0f || overlapY <= 0f)
                {
                    continue;
                }

                Vector2 collisionNormal;
                float overlap;

                if (overlapX < overlapY)
                {
                    collisionNormal = new Vector2(separation.X < 0f ? -1f : 1f, 0f);
                    overlap = overlapX;
                }
                else
                {
                    collisionNormal = new Vector2(0f, separation.Y < 0f ? -1f : 1f);
                    overlap = overlapY;
                }

                _kittys[first]._ballPosition -= collisionNormal * (overlap / 2f);
                _kittys[second]._ballPosition += collisionNormal * (overlap / 2f);

                float relativeSpeed = Vector2.Dot(
                    _kittys[second]._ballVelocity - _kittys[first]._ballVelocity,
                    collisionNormal);

                if (relativeSpeed < 0f)
                {
                    float firstNormalSpeed = Vector2.Dot(_kittys[first]._ballVelocity, collisionNormal);
                    float secondNormalSpeed = Vector2.Dot(_kittys[second]._ballVelocity, collisionNormal);

                    _kittys[first]._ballVelocity += (secondNormalSpeed - firstNormalSpeed) * collisionNormal;
                    _kittys[second]._ballVelocity += (firstNormalSpeed - secondNormalSpeed) * collisionNormal;
                }
            }
        }
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

        for (int i = 0; i < _kittys.Length; i++) {
            //Draw the texture
            _spriteBatch.Draw(_logo, _kittys[i]._ballPosition, Color.White);
        }

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

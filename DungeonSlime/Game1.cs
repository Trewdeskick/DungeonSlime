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
    private Texture2D _logo;
    private Song _bgm;
    private Color previousColor;
    private Color targetColor = Color.CornflowerBlue;
    private Color backgroundColor;

    // Frame Stuff
    private int frameCounter = 0;
    private const int targetFrames = 30;

    // Gravity and Movement
    private const int gravity = 0;

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

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _logo = Content.Load<Texture2D>(@"images\download");

        _bgm = Content.Load<Song>("arpmedia-club-party-dance-music-569466");

        MediaPlayer.IsRepeating = true;
        MediaPlayer.Volume = 0.5f;
        MediaPlayer.Play(_bgm);
        // TODO: use this.Content to load your game content here
    }

    protected void PlayerMovement()
    {
        
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();


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
        _spriteBatch.Draw(_logo, new Vector2(Window.ClientBounds.Width - _logo.Width,
                                             Window.ClientBounds.Height - _logo.Height) 
                                             * .5f, Color.White);

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

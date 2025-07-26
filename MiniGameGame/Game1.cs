using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;

namespace MiniGameGame;


public class Game1 : Core
{
    Point[] gridCoords;
    Point desiredGrid;
    double sizeMultiplyer;
    double gridGap;
    Point lastWindowSize;

    // all the textures
    private Texture2D clickPixelBlack;
    private Texture2D clickPixelGray;
    private Texture2D clickPixelWhite;
    private Texture2D clickPixelTest;
    private Texture2D layout;

    public Game1() : base("MiniGameGame", 960, 960, false)
    {
        lastWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
    }

    protected override void Initialize()
    {
        desiredGrid = new Point(16,16);
        sizeMultiplyer = 0.8;
        

        base.Initialize();
    }

    protected override void LoadContent()
    {
        clickPixelBlack = Content.Load<Texture2D>("clickPixel/clickPixel");
        clickPixelGray = Content.Load<Texture2D>("clickPixel/clickPixelgray");
        clickPixelWhite = Content.Load<Texture2D>("clickPixel/clickPixelwhite");
        clickPixelTest = Content.Load<Texture2D>("clickPixel/clickPixeltest");
        layout = Content.Load<Texture2D>("layout");
        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        Point currentSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
        if (currentSize != lastWindowSize)
        {
            gridCoords = Grid.Coords(currentSize.X, currentSize.Y, desiredGrid, sizeMultiplyer, out gridGap);
            lastWindowSize = currentSize;
        }
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        double pixelDimentions = gridGap / clickPixelBlack.Width;
        GraphicsDevice.Clear(Color.CornflowerBlue);
        SpriteBatch.Begin();
        //SpriteBatch.Draw(layout, Vector2.Zero, Color.White);
        foreach (var element in gridCoords)
        {
            SpriteBatch.Draw(clickPixelBlack, new Vector2(element.X, element.Y), null , Color.White, 0f, Vector2.Zero, (float)pixelDimentions, SpriteEffects.None, 0.0f);
        }
        SpriteBatch.End(); 
        base.Draw(gameTime);
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MiniGameLogic;

namespace MiniGameGame;


public class Game1 : Core
{
    Point[] gridCoords;
    Point desiredGrid;
    double sizeMultiplyer;
    double gridGap;

    (int, int, int)[] ticTacToeClickable;
    (int, int, int)[] ticTacToeUI;
    (int, int, int)[] ticTacToePlaced;

    Point lastWindowSize;
    int gameSelector;
    bool isSinglePlr;


    MouseState mouseState;
    // all the textures
    private Texture2D clickPixelBlack;
    private Texture2D clickPixelGray;
    private Texture2D clickPixelWhite;
    private Texture2D clickPixelTest;
    private Texture2D layout;

    public Game1() : base("MiniGameGame", 960, 960, false)
    {
        lastWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
        mouseState = Mouse.GetState();
    }

    protected override void Initialize()
    {
        desiredGrid = new Point(16,16);
        sizeMultiplyer = 0.8;
        gameSelector = 1; // => tic tac toe
        isSinglePlr = false;

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
        Point currentWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
        if (currentWindowSize != lastWindowSize)
        {
            gridCoords = Grid.Coords(currentWindowSize, desiredGrid, sizeMultiplyer);
            gridGap = Grid.gridGap;
            lastWindowSize = currentWindowSize;
            switch (gameSelector)
            {
            case 1:
                ticTacToeUI = Grid.TicTacToeCoords(desiredGrid, out ticTacToeClickable);
                break;
            case 2:
                // Code für Wert2
                break;          
            }
            
        }
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
             switch (gameSelector)
            {
                case 1:
                    ticTacToePlaced = TicTacToe.Logic(ticTacToeClickable, isSinglePlr);
                    break;
                case 2:
                    // Code für Wert2
                    break;

            }
        }
       
            
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
            SpriteBatch.Draw(clickPixelBlack, new Vector2(element.X, element.Y), null , Color.White, 0f,
                             Vector2.Zero, (float)pixelDimentions, SpriteEffects.None, 0.0f);
        }       
        switch (gameSelector)
        {
            case 1:
                foreach (var element in ticTacToeUI)
                {
                    if (element.Item3 == 2)
                        SpriteBatch.Draw(clickPixelGray, new Vector2(element.Item1, element.Item2), null, Color.White, 0f,
                                         Vector2.Zero, (float)pixelDimentions, SpriteEffects.None, 0.0f);
                }
                if(ticTacToePlaced != null)
                foreach (var element in ticTacToePlaced)
                {
                    SpriteBatch.Draw(clickPixelWhite, new Vector2(element.Item1, element.Item2), null, Color.White, 0f,
                                        Vector2.Zero, (float)pixelDimentions, SpriteEffects.None, 0.0f);
                }
                break;
            case 2:
                // Code für Wert2
                break;

        }
            SpriteBatch.End();
            base.Draw(gameTime);
    }
}

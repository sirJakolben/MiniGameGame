using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MiniGameLogic;
using System.Linq;

namespace MiniGameGame;


public class Game1 : Core
{
    int gameSelector;
    bool isSinglePlr;
    Point desiredGrid;
    double sizeMultiplyer;

    (int x, int y, int gameId)[] clickableCoords;
    (int x, int y, int gameId) clickedPos;
    // GAME ID LIST:
    //---------------------------------
    // TicTacToe:
    //  0-8 = the 9 Squares of the 3x3 playing field: top left -> bottom right
    //---------------------------------

    (int x, int y, int drawId)[] gameUI;
    (int x, int y, int drawId)[] gamePixels;
    // DRAW ID LIST:
    //---------------------------------
    //  0 = Pixelblack
    //  1 = Pixelgray
    //  2 = Pixelwhite
    //  3 = clickPixelBlack
    //  4 = clickPixelGray
    //  5 = clickPixelWhite
    //---------------------------------

    double gridGap;
    Point lastWindowSize;
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
    }

    protected override void Initialize()
    {
        desiredGrid = new Point(17,17);
        sizeMultiplyer = 0.8;
        gameSelector = 1; // => tic tac toe
        isSinglePlr = false;

        TicTacToe.Clear();

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
        if (mouseState.LeftButton == ButtonState.Pressed && Grid.MouseCollision(clickableCoords) != null)
        {
            switch (gameSelector)
            {
                case 1:
                    gamePixels = TicTacToe.Logic(clickedPos, isSinglePlr);
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
        float pixelDimentions = (float)(gridGap / clickPixelBlack.Width);
        float pixel2Dimentions = (float)(gridGap / clickPixelGray.Width);
        GraphicsDevice.Clear(Color.CornflowerBlue);
        SpriteBatch.Begin();
        Point currentWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
        if (currentWindowSize != lastWindowSize)
        {
            switch (gameSelector)
            {
                case 1:
                    (int x, int y)[] emptyGrid = Grid.NewEmpty(currentWindowSize, gameSelector, sizeMultiplyer);
                    gameUI = Grid.TicTacToeUI(emptyGrid);
                    break;
                case 2:

                    break;
            } 
        }
        var renderArray1 = gameUI.Concat(gamePixels).ToArray();
        foreach (var element in renderArray1)
        {
            switch (element.drawId)
            {
                case 0:
                    SpriteBatch.Draw(pixelBlack, new Vector2(element.x, element.y), null , Color.White, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break;
                case 1:
                    SpriteBatch.Draw(pixelGray, new Vector2(element.x, element.y), null , Color.White, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break;
                case 2:
                    SpriteBatch.Draw(pixelWhite, new Vector2(element.x, element.y), null , Color.White, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break;
                case 3:
                    SpriteBatch.Draw(clickPixelBlack, new Vector2(element.x, element.y), null , Color.White, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break;
                case 4:
                    SpriteBatch.Draw(clickPixelGray, new Vector2(element.x, element.y), null , Color.White, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break;
                case 5:
                    SpriteBatch.Draw(clickPixelWhite, new Vector2(element.x, element.y), null , Color.White, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break; 
            }
        }                    
            SpriteBatch.End();
            base.Draw(gameTime);
    }
}

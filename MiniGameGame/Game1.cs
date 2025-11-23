using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MiniGameLogic;
using System.Linq;
using System.Diagnostics;

namespace MiniGameGame;


public class Game1 : Core
{
    public static int gameSelector;
    bool isSinglePlr;
    double sizeMultiplyer;
    double buttonSizeMultiplyer;

    // GAME ID LIST:
    //---------------------------------
    // TicTacToe:
    //  0-8 = the 9 Squares of the 3x3 playing field: top left -> bottom right
    //---------------------------------
    // Vier Gewinnt:
    //  0-6 = the 7 rows of the Playing field
    //---------------------------------

    (int x, int y, int drawId)[] gameUI;
    (int x, int y, int drawId)[] gamePixels;
    (int x, int y, int drawId)[] buttons;
    // DRAW ID LIST:
    //---------------------------------
    // -3 = Multiplayer Button
    // -2 = Singleplayer Button 
    // -1 = Replay Button
    //  0 = Pixelblack
    //  1 = Pixelgray
    //  2 = Pixelwhite
    //  3 = clickPixelBlack
    //  4 = clickPixelGray
    //  5 = clickPixelWhite
    //---------------------------------
    Point lastWindowSize;
    MouseState mouseState;
    int counter;
    bool lockInput;
    // all the textures
    private Texture2D pixelBlack;
    private Texture2D pixelGray;
    private Texture2D pixelWhite;
    private Texture2D clickPixelBlack;
    private Texture2D clickPixelGray;
    private Texture2D clickPixelWhite;

    private Texture2D highlightPixel;
    private Texture2D Background;

    public Game1() : base("MiniGameGame", 1920, 1080, false)
    {
        lastWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);
    }

    protected override void Initialize()
    {
        sizeMultiplyer = 0.8;
        buttonSizeMultiplyer = 0.8;
        gameSelector = 2;
        isSinglePlr = true;

        counter = 0;
        lockInput = false;
        TicTacToe.Clear();
        VierGewinnt.Clear();
        base.Initialize();
    }

    protected override void LoadContent()
    {
        clickPixelBlack = Content.Load<Texture2D>("clickPixel/clickPixel");
        clickPixelGray = Content.Load<Texture2D>("clickPixel/clickPixelgray");
        clickPixelWhite = Content.Load<Texture2D>("clickPixel/clickPixelwhite");
        pixelBlack = Content.Load<Texture2D>("pixel/pixelBlack");
        pixelGray = Content.Load<Texture2D>("pixel/pixelGray");
        pixelWhite = Content.Load<Texture2D>("pixel/pixelWhite");
        highlightPixel = Content.Load<Texture2D>("clickPixel/highlightPixel");

        Background = Content.Load<Texture2D>("miniGameGame");

        base.LoadContent();
    }

    protected override void Update(GameTime gameTime)
    {
        int inputCount = 0;
        if (Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            if (!lockInput)
            {
            counter++;
            lockInput = true;
            }
            inputCount++;
        } 
        mouseState = Mouse.GetState();
        if (mouseState.LeftButton == ButtonState.Pressed)
        {
            if (!lockInput && Grid.MouseCollision() != null)
            {
                var clickedPos = Grid.MouseCollision().Value;
                bool clearGame = false;
                switch (clickedPos.gameId)
                {
                    case -3:
                        if (!isSinglePlr)
                        {
                            isSinglePlr = !isSinglePlr;
                            clearGame = true;
                        }
                        break;
                    case -2:
                        if (isSinglePlr)
                        {
                            isSinglePlr = !isSinglePlr;
                            clearGame = true;
                        }
                        break;
                    case -1:
                        clearGame = true;
                        break;
                }
                switch (gameSelector)
                {
                    case 1:
                        if (clearGame)
                        {
                            TicTacToe.Clear();
                            gamePixels = TicTacToe.ConvertToPixels(TicTacToe.moveList, TicTacToe.winList);
                        }
                        else
                            gamePixels = TicTacToe.Logic(clickedPos, isSinglePlr);
                        break;
                    case 2:
                        if (clearGame)
                        {
                            VierGewinnt.Clear();
                            gamePixels = VierGewinnt.ConvertToPixels(TicTacToe.moveList, TicTacToe.winList);
                        }
                        else
                            gamePixels = VierGewinnt.Logic(clickedPos, isSinglePlr);
                        break;            
                }          
                lockInput = true;
            }
            inputCount++;
        }
        if (inputCount == 0)
        {
            lockInput = false;
        }
          
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        float pixelDimentions = (float)Grid.pixelGap / (float)clickPixelBlack.Width;
        float buttonDimentions = (float)Grid.smallOffset / (float)clickPixelBlack.Width;
        Point currentWindowSize = new Point(Window.ClientBounds.Width, Window.ClientBounds.Height);

        GraphicsDevice.Clear(Color.CornflowerBlue);
        SpriteBatch.Begin();
        if (currentWindowSize != lastWindowSize)
        {
            (int x, int y)[] emptyGrid = Grid.NewEmpty(currentWindowSize, gameSelector, sizeMultiplyer);
            switch (gameSelector)
            {
                case 1:
                    gameUI = Grid.TicTacToeUI(emptyGrid);
                    gamePixels = TicTacToe.ConvertToPixels(TicTacToe.moveList, TicTacToe.winList);
                    break;
                case 2:
                    gameUI = Grid.VierGewinntUI(emptyGrid);
                    gamePixels = VierGewinnt.ConvertToPixels(VierGewinnt.moveList, VierGewinnt.winList);
                    break;
            }
            buttons = Grid.Buttons(currentWindowSize, gameSelector, buttonSizeMultiplyer);
            lastWindowSize = currentWindowSize;
        }

        var renderArray1 = gameUI.Concat(buttons).ToArray();
        if (gamePixels != null)
        {
            renderArray1 = renderArray1.Concat(gamePixels).ToArray();
        }
        foreach (var element in renderArray1)
        {
            switch (element.drawId)
            {
                case -3:
                    if(!isSinglePlr)
                    SpriteBatch.Draw(pixelBlack, new Vector2(element.x, element.y)+ Grid.offset, null , Color.White, 0f,
                    Vector2.Zero, buttonDimentions, SpriteEffects.None, 0.0f);
                    else
                    SpriteBatch.Draw(pixelGray, new Vector2(element.x, element.y)+ Grid.offset, null , Color.White, 0f,
                    Vector2.Zero, buttonDimentions, SpriteEffects.None, 0.0f);
                    break;
                case -2:
                    if(isSinglePlr)
                    SpriteBatch.Draw(pixelBlack, new Vector2(element.x, element.y)+ Grid.offset, null , Color.White, 0f,
                    Vector2.Zero, buttonDimentions, SpriteEffects.None, 0.0f);
                    else
                    SpriteBatch.Draw(pixelGray, new Vector2(element.x, element.y)+ Grid.offset, null , Color.White, 0f,
                    Vector2.Zero, buttonDimentions, SpriteEffects.None, 0.0f);
                    break;
                case -1:
                    SpriteBatch.Draw(pixelBlack, new Vector2(element.x, element.y)+ Grid.offset, null , Color.White, 0f,
                    Vector2.Zero, buttonDimentions, SpriteEffects.None, 0.0f);
                    break;
                case 0:
                    SpriteBatch.Draw(pixelBlack, new Vector2(element.x, element.y)+ Grid.offset, null , Color.White, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break;
                case 1:
                    SpriteBatch.Draw(pixelGray, new Vector2(element.x, element.y)+ Grid.offset, null , Color.White, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break;
                case 2:
                    SpriteBatch.Draw(pixelWhite, new Vector2(element.x, element.y)+ Grid.offset, null , Color.White, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break;
                case 3:
                    SpriteBatch.Draw(clickPixelBlack, new Vector2(element.x, element.y)+ Grid.offset, null, Color.White, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break;
                case 4:
                    SpriteBatch.Draw(clickPixelGray, new Vector2(element.x, element.y)+ Grid.offset, null, Color.White, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break;
                case 5:
                    SpriteBatch.Draw(clickPixelWhite, new Vector2(element.x, element.y)+ Grid.offset, null, Color.White, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break;
            }
        }
        if (Grid.MouseCollision() is var collision && collision.HasValue)
        {
            var mousePos = collision.Value;
            switch (mousePos.gameId)
            {
                case -3:
                    if (!isSinglePlr)
                    SpriteBatch.Draw(highlightPixel, new Vector2(mousePos.x, mousePos.y) + Grid.offset, null, Color.White * 0.5f, 0f, 
                    Vector2.Zero, buttonDimentions, SpriteEffects.None, 0.0f);
                    break;
                case -2:
                    if (isSinglePlr)
                    SpriteBatch.Draw(highlightPixel, new Vector2(mousePos.x, mousePos.y) + Grid.offset, null, Color.White * 0.5f, 0f, 
                    Vector2.Zero, buttonDimentions, SpriteEffects.None, 0.0f);
                    break;
                case -1:
                    SpriteBatch.Draw(highlightPixel, new Vector2(mousePos.x, mousePos.y)+ Grid.offset, null, Color.White * 0.5f, 0f,
                    Vector2.Zero, buttonDimentions, SpriteEffects.None, 0.0f);
                    break;
                default:
                    SpriteBatch.Draw(highlightPixel, new Vector2(mousePos.x, mousePos.y)+ Grid.offset, null, Color.White * 0.5f, 0f,
                    Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
                    break;
            }

        }               
        // var clickable = Grid.clickable;
        // foreach (var element in clickable)
        // {
        //     if (element.gameId  == counter)
        //         SpriteBatch.Draw(clickPixelWhite, new Vector2(element.x, element.y), null, Color.White, 0f,
        //         Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
        //     else
        //     SpriteBatch.Draw(clickPixelGray, new Vector2(element.x, element.y), null, Color.White, 0f,
        //         Vector2.Zero, pixelDimentions, SpriteEffects.None, 0.0f);
        // }
        
            SpriteBatch.End();
            base.Draw(gameTime);
    }
}

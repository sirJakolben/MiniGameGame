using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace MonoGameLibrary;

public class Grid
{
    public static double gridGap;
    public static (int x, int y) gridDimentions;
    public static (int x, int y) gridOffset;
    public static (int x, int y, int gameId)[] clickableCoords;
    static MouseState mouse;
    static Point mousePos;
    public static (int x, int y)[] NewEmpty(Point windowSize, int gameSelector, double sizeMultiplyer)
    {
        switch (gameSelector)
        {
            case 1: // tic tac toe
                gridDimentions = (17, 17);
            break;
        }
        if (windowSize.X / gridDimentions.x > windowSize.Y / gridDimentions.y)
        {
            gridGap = (int)((float)(windowSize.Y * sizeMultiplyer) / gridDimentions.y);
            gridOffset = ((int)(0.5f * (windowSize.Y - sizeMultiplyer * windowSize.Y)),
                          (int)(0.5f * (windowSize.Y - sizeMultiplyer * windowSize.Y)));
        }
        else
        {
            gridGap = (int)((float)(windowSize.X * sizeMultiplyer) / gridDimentions.x);
            gridOffset = ((int)(0.5f * (windowSize.X - sizeMultiplyer * windowSize.X)),
                          (int)(0.5f * (windowSize.X - sizeMultiplyer * windowSize.X)));
        }
        (int x, int y)[] pixelCoords = new (int,int)[gridDimentions.x * gridDimentions.y];
        int i = 0;
        for (int x = 0; x < gridDimentions.x; x++)
        {
            for (int y = 0; y < gridDimentions.y; y++)
            {
                pixelCoords[i] = ((int)(gridGap * x + gridOffset.x), (int)(gridGap * y + gridOffset.y));
                i++;
            }
        }
        return pixelCoords; // => this Point[] is a (int,int) array with all the coordinates of the top left pixel corner 
    }
    public static (int x, int y, int id)[] TicTacToeUI((int x, int y)[] emptyGrid) // generates the tic tac toe UI => the four lines
    {
        List<(int x, int y, int drawId)> UIList = new();
        List<(int x, int y, int gameId)> clickableList = new();
        int i = 0;
        for (int y = 0; y < gridDimentions.y; y++)
        {
            for (int x = 0; x < gridDimentions.x; x++)
            {
                if (x == (int)(1.0 / 3 * gridDimentions.x) || x == (int)(2.0 / 3 * gridDimentions.x) || // vertical lines
                    y == (int)(1.0 / 3 * gridDimentions.y) || y == (int)(2.0 / 3 * gridDimentions.y)) // horizontal lines
                {
                    UIList.Add((emptyGrid[i].x, emptyGrid[i].y, 1));
                }
                else
                {
                    UIList.Add((emptyGrid[i].x, emptyGrid[i].y, 3));
                    if (x < 1.0 / 3 * gridDimentions.x && y < 1.0 / 3 * gridDimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 0)); // section 1 / 9 is clickable
                    }
                    else if (x < 2.0 / 3 * gridDimentions.x && y < 1.0 / 3 * gridDimentions.y &&
                            x > 1.0 / 3 * gridDimentions.x)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 1)); // section 2 / 9 is clickable
                    }
                    else if (x > 2.0 / 3 * gridDimentions.x && y < 1.0 / 3 * gridDimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 2)); // section 3 / 9 is clickable
                    }
                    else if (x < 1.0 / 3 * gridDimentions.x && y < 2.0 / 3 * gridDimentions.y &&
                                                            y > 1.0 / 3 * gridDimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 3)); // section 4 / 9 is clickable
                    }
                    else if (x < 2.0 / 3 * gridDimentions.x && y < 2.0 / 3 * gridDimentions.y &&
                            x > 1.0 / 3 * gridDimentions.x && y > 1.0 / 3 * gridDimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 4)); // section 5 / 9 is clickable
                    }
                    else if (x > 2.0 / 3 * gridDimentions.x && y < 2.0 / 3 * gridDimentions.y &&
                                                            y > 1.0 / 3 * gridDimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 5)); // section 6 / 9 is clickable
                    }
                    else if (x < 1.0 / 3 * gridDimentions.x && y > 2.0 / 3 * gridDimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 6)); // section 7 / 9 is clickable
                    }
                    else if (x < 2.0 / 3 * gridDimentions.x && y > 2.0 / 3 * gridDimentions.y &&
                            x > 1.0 / 3 * gridDimentions.x)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 7)); // section 8 / 9 is clickable
                    }
                    else if (x > 2.0 / 3 * gridDimentions.x && y > 2.0 / 3 * gridDimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 8)); // section 9 / 9 is clickable
                    }
                }
                i++;
            }
        }
        clickableCoords = clickableList.ToArray(); // this contains the subset of Tic Tac Toe UI, that is clickable => squares 1-9
        (int x, int y, int drawId)[] UICoords = UIList.ToArray();
        return UICoords; // this contains the Tic Tac Toe UI => 4 lines + squares 1-9
    }
    public static (int x, int y, int gameId)? MouseCollision((int x, int y, int gameId)[] clickableTargets)
    {
        mouse = Mouse.GetState();
        mousePos = new Point(mouse.X, mouse.Y);
        foreach (var element in clickableTargets)
        {
            if (mousePos.X >= element.Item1 && mousePos.X <= (element.Item1 + gridGap) &&
                mousePos.Y >= element.Item2 && mousePos.Y <= (element.Item2 + gridGap))
            {
                return element;
            }
        }
        return null;
    }
}
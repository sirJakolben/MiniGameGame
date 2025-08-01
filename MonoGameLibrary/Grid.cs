using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace MonoGameLibrary;

public class Grid
{
    public static double pixelGap;
    public static (int x, int y) dimentions;
    public static (int x, int y) offset;
    public static (int x, int y, int gameId)[] clickable;
    static MouseState mouse;
    static Point mousePos;
    public static (int x, int y)[] NewEmpty(Point windowSize, int gameSelector, double sizeMultiplyer)
    {
        switch (gameSelector)
        {
            case 1: // tic tac toe
                dimentions = (17, 17);
            break;
        }
        if (windowSize.X / dimentions.x > windowSize.Y / dimentions.y)
        {
            pixelGap = (int)((float)(windowSize.Y * sizeMultiplyer) / dimentions.y);
            offset = ((int)(0.5f * (windowSize.Y - sizeMultiplyer * windowSize.Y)),
                          (int)(0.5f * (windowSize.Y - sizeMultiplyer * windowSize.Y)));
        }
        else
        {
            pixelGap = (int)((float)(windowSize.X * sizeMultiplyer) / dimentions.x);
            offset = ((int)(0.5f * (windowSize.X - sizeMultiplyer * windowSize.X)),
                          (int)(0.5f * (windowSize.X - sizeMultiplyer * windowSize.X)));
        }
        (int x, int y)[] pixelCoords = new (int,int)[dimentions.x * dimentions.y];
        int i = 0;
        for (int x = 0; x < dimentions.x; x++)
        {
            for (int y = 0; y < dimentions.y; y++)
            {
                pixelCoords[i] = ((int)(pixelGap * x + offset.x), (int)(pixelGap * y + offset.y));
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
        for (int y = 0; y < dimentions.y; y++)
        {
            for (int x = 0; x < dimentions.x; x++)
            {
                if (x == (int)(1.0 / 3 * dimentions.x) || x == (int)(2.0 / 3 * dimentions.x) || // vertical lines
                    y == (int)(1.0 / 3 * dimentions.y) || y == (int)(2.0 / 3 * dimentions.y)) // horizontal lines
                {
                    UIList.Add((emptyGrid[i].x, emptyGrid[i].y, 1));
                }
                else
                {
                    UIList.Add((emptyGrid[i].x, emptyGrid[i].y, 3));
                    if (x < 1.0 / 3 * dimentions.x && y < 1.0 / 3 * dimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 0)); // section 1 / 9 is clickable
                    }
                    else if (x < 2.0 / 3 * dimentions.x && y < 1.0 / 3 * dimentions.y &&
                            x > 1.0 / 3 * dimentions.x)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 1)); // section 2 / 9 is clickable
                    }
                    else if (x > 2.0 / 3 * dimentions.x && y < 1.0 / 3 * dimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 2)); // section 3 / 9 is clickable
                    }
                    else if (x < 1.0 / 3 * dimentions.x && y < 2.0 / 3 * dimentions.y &&
                                                            y > 1.0 / 3 * dimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 3)); // section 4 / 9 is clickable
                    }
                    else if (x < 2.0 / 3 * dimentions.x && y < 2.0 / 3 * dimentions.y &&
                            x > 1.0 / 3 * dimentions.x && y > 1.0 / 3 * dimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 4)); // section 5 / 9 is clickable
                    }
                    else if (x > 2.0 / 3 * dimentions.x && y < 2.0 / 3 * dimentions.y &&
                                                            y > 1.0 / 3 * dimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 5)); // section 6 / 9 is clickable
                    }
                    else if (x < 1.0 / 3 * dimentions.x && y > 2.0 / 3 * dimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 6)); // section 7 / 9 is clickable
                    }
                    else if (x < 2.0 / 3 * dimentions.x && y > 2.0 / 3 * dimentions.y &&
                            x > 1.0 / 3 * dimentions.x)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 7)); // section 8 / 9 is clickable
                    }
                    else if (x > 2.0 / 3 * dimentions.x && y > 2.0 / 3 * dimentions.y)
                    {
                        clickableList.Add((emptyGrid[i].x, emptyGrid[i].y, 8)); // section 9 / 9 is clickable
                    }
                }
                i++;
            }
        }
        clickable = clickableList.ToArray(); // this contains the subset of Tic Tac Toe UI, that is clickable => squares 1-9
        (int x, int y, int drawId)[] UICoords = UIList.ToArray();
        return UICoords; // this contains the Tic Tac Toe UI => 4 lines + squares 1-9
    }
    public static (int x, int y, int gameId)? MouseCollision()
    {
        mouse = Mouse.GetState();
        mousePos = new Point(mouse.X, mouse.Y);
        foreach (var element in clickable)
        {
            if (mousePos.X >= element.Item1 && mousePos.X <= (element.Item1 + pixelGap) &&
                mousePos.Y >= element.Item2 && mousePos.Y <= (element.Item2 + pixelGap))
            {
                return element;
            }
        }
        return null;
    }
}
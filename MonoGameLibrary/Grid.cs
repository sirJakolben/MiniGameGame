using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
namespace MonoGameLibrary;

public class Grid
{
    public static double gridGap;
    static MouseState mouse;
    static Point mousePos;
    static Point[] nodeCords; //generates an empty grid with the window size, the desired grid dimentions and the relative grid size on the screen
    public static Point[] Coords(Point windowSize, Point playGridSize, double sizeMultiplyer)
    {
        int offset;
        if (windowSize.X / playGridSize.X > windowSize.Y / playGridSize.Y)
        {
            gridGap = windowSize.Y * sizeMultiplyer / playGridSize.Y;
            offset = (int)(0.5f * (windowSize.Y - sizeMultiplyer * windowSize.Y));
        }
        else
        {
            gridGap = windowSize.X * sizeMultiplyer / playGridSize.X;
            offset = (int)(0.5f * (windowSize.X - sizeMultiplyer * windowSize.X));
        }
        nodeCords = new Point[playGridSize.X * playGridSize.Y];
        int i = 0;
        for (int x = 0; x < playGridSize.X; x++)
        {
            for (int y = 0; y < playGridSize.Y; y++)
            {
                nodeCords[i] = new Point((int)(gridGap * x + offset), (int)(gridGap * y + offset));
                i++;
            }
        }
        return nodeCords; // => this Point[] is a (int,int) array with all the coordinates of the top left pixel corner 
    }
    public static (int, int, int)[] TicTacToeCoords(Point playGridSize, out (int, int, int)[] clickableCoords) // generates the tic tac toe UI => the four lines
    {
        List<(int, int, int)> ticTacToeList = new();
        List<(int, int, int)> clickableList = new();
        int i = 0;
        for (int y = 0; y < playGridSize.Y; y++)
        {
            for (int x = 0; x < playGridSize.X; x++)
            {
                if (x == (int)(1.0 / 3 * playGridSize.X) || x == (int)(2.0 / 3 * playGridSize.X) || // vertical lines
                    y == (int)(1.0 / 3 * playGridSize.Y) || y == (int)(2.0 / 3 * playGridSize.Y)) // horizontal lines
                {
                    ticTacToeList.Add((nodeCords[i].X, nodeCords[i].Y, 2));
                }
                else if (x < 1.0 / 3 * playGridSize.X && y < 1.0 / 3 * playGridSize.Y)
                {
                    clickableList.Add((nodeCords[i].X, nodeCords[i].Y, 1)); // section 1 / 9 is clickable
                }
                else if (x < 2.0 / 3 * playGridSize.X && y < 1.0 / 3 * playGridSize.Y &&
                         x > 1.0 / 3 * playGridSize.X)
                {
                    clickableList.Add((nodeCords[i].X, nodeCords[i].Y, 2)); // section 2 / 9 is clickable
                }
                else if (x > 2.0 / 3 * playGridSize.X && y < 1.0 / 3 * playGridSize.Y)
                {
                    clickableList.Add((nodeCords[i].X, nodeCords[i].Y, 3)); // section 3 / 9 is clickable
                }
                else if (x < 1.0 / 3 * playGridSize.X && y < 2.0 / 3 * playGridSize.Y &&
                                                         y > 1.0 / 3 * playGridSize.X)
                {
                    clickableList.Add((nodeCords[i].X, nodeCords[i].Y, 4)); // section 4 / 9 is clickable
                }
                else if (x < 2.0 / 3 * playGridSize.X && y < 2.0 / 3 * playGridSize.Y &&
                         x > 1.0 / 3 * playGridSize.X && y > 1.0 / 3 * playGridSize.X)
                {
                    clickableList.Add((nodeCords[i].X, nodeCords[i].Y, 5)); // section 5 / 9 is clickable
                }
                else if (x > 2.0 / 3 * playGridSize.X && y < 2.0 / 3 * playGridSize.Y &&
                                                         y > 1.0 / 3 * playGridSize.X)
                {
                    clickableList.Add((nodeCords[i].X, nodeCords[i].Y, 6)); // section 6 / 9 is clickable
                }
                else if (x < 1.0 / 3 * playGridSize.X && y > 2.0 / 3 * playGridSize.Y)
                {
                    clickableList.Add((nodeCords[i].X, nodeCords[i].Y, 7)); // section 7 / 9 is clickable
                }
                else if (x < 2.0 / 3 * playGridSize.X && y > 2.0 / 3 * playGridSize.Y &&
                         x > 1.0 / 3 * playGridSize.X)
                {
                    clickableList.Add((nodeCords[i].X, nodeCords[i].Y, 8)); // section 8 / 9 is clickable
                }
                else if (x > 2.0 / 3 * playGridSize.X && y > 2.0 / 3 * playGridSize.Y)
                {
                    clickableList.Add((nodeCords[i].X, nodeCords[i].Y, 9)); // section 9 / 9 is clickable
                }
                i++;
            }
        }
        clickableCoords = clickableList.ToArray();
        (int, int, int)[] ticTacToeCoords = ticTacToeList.ToArray();
        return ticTacToeCoords; // this (int,int,int) array contains the coordinates of the devider lines and the vallue (1) for the gray pixel
    }
    public static (int, int, int)[] MouseCollisions((int, int, int)[] clickableTargets)
    {
        mouse = Mouse.GetState();
        mousePos = mouse.Position;

        List<(int, int, int)> collisionList = new();
        foreach (var element in clickableTargets)
        {
            if (mousePos.X > element.Item1 && mousePos.X < (element.Item1 + gridGap) &&
                mousePos.Y > element.Item2 && mousePos.Y < (element.Item2 + gridGap))
            {
                collisionList.Add(element);
            }
        }
        (int, int, int)[] collisionCoords = collisionList.ToArray();
        return collisionCoords;
    }
}
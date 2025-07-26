using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame;
using MonoGameLibrary;

namespace MiniGameLogic;

public class TicTacToe
{
    static List<(int, int)> plr1MoveList;
    static List<(int, int)> plr2MoveList;
    static List<(int, int, int)> moveList;
    static (int, int, int)[] coords;
    static bool isPlrOne;
    public TicTacToe()
    {
        isPlrOne = false;
        plr1MoveList = new();
        plr2MoveList = new();
        moveList = new();
    }
    public static (int, int, int)[] Logic((int, int, int)[] clickableCoords, bool isSinglePlr)
    {
        coords = clickableCoords;
        moveList.Clear();
        isPlrOne = !isPlrOne;
        (int, int, int)[] collision = Grid.MouseCollisions(clickableCoords);
        foreach (var element in plr1MoveList)
        {
            if (element.Item1 + 3 * element.Item2 == collision[0].Item3)
            {
                return null;
            }
        }
        foreach (var element in plr2MoveList)
        {
            if (element.Item1 + 3 * element.Item2 == collision[0].Item3)
            {
                return null;
            }
        }
        int y = collision[0].Item3 / 3;
        int x = collision[0].Item3 - y;
        if (isPlrOne) plr1MoveList.Add((x, y));
        if (!isPlrOne && !isSinglePlr) plr2MoveList.Add((x, y));
        foreach (var element in ConvertToPixels(x, y))
        {
            moveList.Add(element);
        }
        if (!isPlrOne && isSinglePlr)
        {
            foreach (var element in ConvertToPixels(BotMove().Item1, BotMove().Item1))
            {
                moveList.Add(element);
            }

        }
        (int, int, int)[] moveCoords = moveList.ToArray();
        return moveCoords;
    }

    static (int, int) BotMove()
    {
        throw new NotImplementedException();
    }
    static (int, int, int)[] ConvertToPixels(int x, int y)
    {
        List<(int, int, int)> pixelList = new();
        List<(int, int)> clickableList = new();
        foreach (var element in coords)
        {
            if (element.Item3 == x + 3 * y)
            {
                clickableList.Add((element.Item1, element.Item2));
            }
        }
        (int, int) firstPixel = clickableList[0];
        (int, int) lastPixel = clickableList[clickableList.Count - 1];
        if (!isPlrOne)
        {
            foreach (var element in clickableList)
            {
                if (element.Item1 == firstPixel.Item1 || element.Item1 == lastPixel.Item1 ||
                   element.Item2 == firstPixel.Item2 || element.Item2 == lastPixel.Item2)
                {
                    pixelList.Add((element.Item1, element.Item2, 3));
                }
            }
        }
        else
        {
            pixelList.Add((firstPixel.Item1, firstPixel.Item2, 3));
            pixelList.Add((lastPixel.Item1, lastPixel.Item2, 3));
            pixelList.Add((firstPixel.Item1, lastPixel.Item2, 3));
            pixelList.Add((lastPixel.Item1, firstPixel.Item2, 3));
            (int, int) middlePixel = ((firstPixel.Item1 + lastPixel.Item1) / 2, (firstPixel.Item2 + lastPixel.Item2) / 2);
            pixelList.Add((middlePixel.Item1, middlePixel.Item2, 3));
            pixelList.Add(((middlePixel.Item1 + lastPixel.Item1) / 2, (firstPixel.Item2 + middlePixel.Item2) / 2, 3));
            pixelList.Add(((middlePixel.Item1 + firstPixel.Item1) / 2, (lastPixel.Item2 + middlePixel.Item2) / 2, 3));
            pixelList.Add(((middlePixel.Item1 + lastPixel.Item1) / 2, (lastPixel.Item2 + middlePixel.Item2) / 2, 3));
            pixelList.Add(((middlePixel.Item1 + firstPixel.Item1) / 2, (firstPixel.Item2 + middlePixel.Item2) / 2, 3));
        }

        (int, int, int)[] pixelCoords = pixelList.ToArray();
        return pixelCoords;
    }
}
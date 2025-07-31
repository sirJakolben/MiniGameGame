using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MonoGameLibrary;

namespace MiniGameLogic;

public class TicTacToe
{
    static List<(int, int, bool)> moveList;
    static (int, int, int)[] pixelCoords;
    static bool isPlrOne;
    public static void Clear()
    {
        isPlrOne = false;
        moveList = new();
    }
    public static (int, int, int)[] Logic((int, int, int) clickedPos, bool isSinglePlr)
    {
        isPlrOne = !isPlrOne;
        
        foreach (var element in moveList)
        {
            if (element.Item1 + 3 * element.Item2 == clickedPos.Item3)
            {
                return null;
            }
        }
        int y = (int)(clickedPos.Item3 / 3.0);
        int x = clickedPos.Item3 - y;
        moveList.Add((x, y, isPlrOne));
        List<(int, int, int)> pixelList = ConvertToPixels(x, y, clickedPos).ToList(); ;
        if (!isPlrOne && isSinglePlr)
        {
            pixelList.Concat(ConvertToPixels(BotMove().Item1, BotMove().Item2, clickedPos)).ToList();
            isPlrOne = !isPlrOne;
        }
        (int, int, int)[] pixelCoords = pixelList.ToArray();
        return pixelCoords;
    }

    static (int, int) BotMove()
    {
        throw new NotImplementedException();
    }
    static (int, int, int)[] ConvertToPixels(int x, int y, (int,int,int) clickedPos)
    {
        List<(int, int, int)> pixelList = new();
        List<(int, int)> clickableList = new();
        foreach (var element in Grid.clickableCoords)
        {
            if (element.Item3 == x + 3 * y)
            {
                clickableList.Add(((int)element.Item1, (int)element.Item2));
            }
        }
        int gridGap = clickableList[1].Item2 - clickableList[0].Item2;
        (int, int) firstPixel = clickableList[0];
        (int, int) lastPixel = clickableList[clickableList.Count - 1];
        (int, int) middlePixel = ((firstPixel.Item1 + lastPixel.Item1) / 2, (firstPixel.Item2 + lastPixel.Item2) / 2);
        int smallAxis = (lastPixel.Item1 - firstPixel.Item1) / gridGap;
        if((lastPixel.Item2 - firstPixel.Item2) / gridGap < smallAxis)
            smallAxis = (lastPixel.Item2 - firstPixel.Item2) / gridGap;
        if (!isPlrOne)
        {
            foreach (var element in clickableList)
            {
                if (Convert.ToInt16(Math.Sqrt(Math.Pow(middlePixel.Item1 - element.Item1, 2) +
                                         Math.Pow(middlePixel.Item2 - element.Item2, 2)) / gridGap) == (int)(smallAxis / 2))
                {
                    pixelList.Add((element.Item1, element.Item1, 3));
                }
            }
        }
        else
        {
            for (int i = (int)(-1.0 / 2 * smallAxis); i < (int)(1.0 / 2 * smallAxis + 1); i++)
            {
                for (int j = -1; j < 2; j += 2)
                {
                    pixelList.Add((Convert.ToInt16(middlePixel.Item1 + i* gridGap), 
                                  Convert.ToInt16(middlePixel.Item2 + i * gridGap * j), 3));
                }
            }
        }

        (int, int, int)[] pixelCoords = pixelList.ToArray();
        return pixelCoords;
    }
    static (int, int)[] CheckWin()
    {
        throw new NotImplementedException();
    }
}
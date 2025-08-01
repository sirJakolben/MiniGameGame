using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MonoGameLibrary;

namespace MiniGameLogic;

public class TicTacToe
{
    static List<(int, int, bool isPlrOne)> moveList;
    static (int, int, int drawId)[] gamePixels;
    static bool isPlrOne;
    public static void Clear()
    {
        isPlrOne = false;
        moveList = new();
    }
    public static (int, int, int drawId)[] Logic((int, int, int gameId) clickedPos, bool isSinglePlr)
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
        if (!isPlrOne && isSinglePlr)
        {
            moveList.Add((BotMove().Item1, BotMove().Item2, isPlrOne));
            isPlrOne = !isPlrOne;
        }
        return ConvertToPixels(moveList);
    }

    static (int, int) BotMove()
    {
        throw new NotImplementedException();
    }
    static (int, int, int)[] ConvertToPixels(List<(int, int, bool isPlrOne)> moveList)
    {
        List<(int, int, int)> pixelList = new();
        foreach (var element in moveList)
        {
            var movePixels = Grid.clickable.Where(x => x.Item3 == element.Item1 + 3 * element.Item2).ToArray();
            var firstPixel = movePixels[0];
            var lastPixel = movePixels[movePixels.Length - 1];
            var middlePixel = ((firstPixel.Item1 + lastPixel.Item1) / 2, (firstPixel.Item2 + lastPixel.Item2) / 2);
            int smallAxis = (int)((lastPixel.Item1 - firstPixel.Item1) / Grid.pixelGap);
            if ((lastPixel.Item2 - firstPixel.Item2) / Grid.pixelGap < smallAxis)
                smallAxis = (int)((lastPixel.Item2 - firstPixel.Item2) / Grid.pixelGap);
            if (!isPlrOne)
            {
                foreach (var pixel in movePixels)
                {
                    if (Convert.ToInt16(Math.Sqrt(Math.Pow(middlePixel.Item1 - pixel.Item1, 2) +
                                             Math.Pow(middlePixel.Item2 - pixel.Item2, 2)) / Grid.pixelGap) == (int)(smallAxis / 2))
                    {
                        pixelList.Add((pixel.Item1, pixel.Item1, 3));
                    }
                }
            }
            else
            {
                for (int i = (int)(-1.0 / 2 * smallAxis); i < (int)(1.0 / 2 * smallAxis + 1); i++)
                {
                    for (int j = -1; j < 2; j += 2)
                    {
                        pixelList.Add((Convert.ToInt16(middlePixel.Item1 + i * Grid.pixelGap),
                                      Convert.ToInt16(middlePixel.Item2 + i * Grid.pixelGap * j), 3));
                    }
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
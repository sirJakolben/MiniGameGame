using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using MonoGameLibrary;

namespace MiniGameLogic;

public class TicTacToe
{
    public static List<(int, int, bool isPlrOne)> moveList;
    static bool isPlrOne;
    static Random random;
    public static void Clear()
    {
        random = new Random();
        isPlrOne = true;
        moveList = new();
    }
    public static (int, int, int drawId)[] Logic((int, int, int gameId) clickedPos, bool isSinglePlr)
    {
        foreach (var element in moveList)
        {
            if (element.Item1 + 3 * element.Item2 == clickedPos.Item3)
            {
                return ConvertToPixels(moveList);
            }
        }
        int y = (int)(clickedPos.Item3 / 3.0);
        int x = clickedPos.Item3 % 3;
        moveList.Add((x, y, isPlrOne));
        isPlrOne = !isPlrOne;
        if (!isPlrOne && isSinglePlr)
        {
            moveList.Add((BotMove().Item1, BotMove().Item2, isPlrOne));
            isPlrOne = !isPlrOne;
        }
        return ConvertToPixels(moveList);
    }

    static (int, int) BotMove()
    {
        bool invalidMove = true;
        int rndmMove = 0;
        var botMoveList = moveList.Where(x => x.isPlrOne == false).ToArray();
        var plrMoveList = moveList.Where(x => x.isPlrOne == true).ToArray();
        if (PossibleWins(botMoveList, plrMoveList) != null)
        {
            return PossibleWins(botMoveList, plrMoveList).Value;
        }
        else if (PossibleWins(plrMoveList, botMoveList) != null)
        {
            return PossibleWins(plrMoveList, botMoveList).Value;
        }
        else
        {
            while (invalidMove)
            {
                invalidMove = false;
                rndmMove = random.Next(0, 8);
                foreach (var element in moveList)
                {
                    if (element.Item1 + 3 * element.Item2 == rndmMove)
                    {
                        invalidMove = true;
                    }
                }
            }
            return (rndmMove % 3, (int)(rndmMove / 3));
        }
    }
    static (int, int)? PossibleWins((int, int, bool)[] plrMoveList, (int, int, bool)[] notPlrMoveList)
    {
        foreach (var element in plrMoveList)
        {
            foreach (var position in plrMoveList)
            {
                if (position == element) continue;
                if (element.Item1 == position.Item1 && !notPlrMoveList.Any(x => x.Item1 == element.Item1))
                {
                    return (element.Item1, 3 - element.Item2 - position.Item2);
                }
                else if (element.Item2 == position.Item2 && !notPlrMoveList.Any(x => x.Item2 == element.Item2))
                {
                    return (3 - element.Item1 - position.Item1, element.Item2);
                }
                else if ((element.Item1 + element.Item2) % 2 == 0 && (position.Item1 + position.Item2) % 2 == 0 &&
                          !notPlrMoveList.Any(x => x.Item1 == 3 - element.Item1 - position.Item1 &&
                                                    x.Item2 == 3 - element.Item2 - position.Item2))
                {
                    return (3 - element.Item1 - position.Item1, 3 - element.Item2 - position.Item2);
                }
            }
        }
        return null;
    }
    static List<(int, int, bool isPlrOne)> CheckWin()
    {
        if (moveList.Count < 5) return new();
        var botMoveList = moveList.Where(x => x.isPlrOne == false).ToArray();
        var plrMoveList = moveList.Where(x => x.isPlrOne == true).ToArray();
        for (int i = 0; i < 3; i++)
        {
            if (botMoveList.Where(x => x.Item1 == i).ToArray().Count() == 3)
                return botMoveList.Where(x => x.Item1 == i).ToList();
            if (botMoveList.Where(x => x.Item2 == i).ToArray().Count() == 3)
                return botMoveList.Where(x => x.Item1 == i).ToList();
            if (plrMoveList.Where(x => x.Item1 == i).ToArray().Count() == 3)
                return plrMoveList.Where(x => x.Item1 == i).ToList();
            if (plrMoveList.Where(x => x.Item2 == i).ToArray().Count() == 3)
                return plrMoveList.Where(x => x.Item1 == i).ToList();
        }
    var mainDiag = new (int, int)[] { (0, 0), (1, 1), (2, 2) };
    var antiDiag = new (int, int)[] { (0, 2), (1, 1), (2, 0) };

    if (mainDiag.All(p => botMoveList.Any(x => x.Item1 == p.Item1 && x.Item2 == p.Item2)))
        return botMoveList.Where(x => mainDiag.Any(p => p.Item1 == x.Item1 && p.Item2 == x.Item2)).ToList();
    if (mainDiag.All(p => plrMoveList.Any(x => x.Item1 == p.Item1 && x.Item2 == p.Item2)))
        return plrMoveList.Where(x => mainDiag.Any(p => p.Item1 == x.Item1 && p.Item2 == x.Item2)).ToList();

    if (antiDiag.All(p => botMoveList.Any(x => x.Item1 == p.Item1 && x.Item2 == p.Item2)))
        return botMoveList.Where(x => antiDiag.Any(p => p.Item1 == x.Item1 && p.Item2 == x.Item2)).ToList();
    if (antiDiag.All(p => plrMoveList.Any(x => x.Item1 == p.Item1 && x.Item2 == p.Item2)))
        return plrMoveList.Where(x => antiDiag.Any(p => p.Item1 == x.Item1 && p.Item2 == x.Item2)).ToList();
        return new();
    }
    public static (int, int, int)[] ConvertToPixels(List<(int, int, bool)> moveList)
    {
        List<(int, int, int)> pixelList = new();
        foreach (var element in moveList)
        {
            var movePixels = Grid.clickable.Where(x => x.Item3 == element.Item1 + 3 * element.Item2).ToArray();
            if (movePixels.Length == 0)
                continue;
            var firstPixel = movePixels[0];
            var lastPixel = movePixels[movePixels.Length - 1];
            var middlePixel = ((firstPixel.Item1 + lastPixel.Item1) / 2, (firstPixel.Item2 + lastPixel.Item2) / 2);
            int smallAxis = (int)((lastPixel.Item1 - firstPixel.Item1) / Grid.pixelGap);
            if ((lastPixel.Item2 - firstPixel.Item2) / Grid.pixelGap < smallAxis)
                smallAxis = (int)((lastPixel.Item2 - firstPixel.Item2) / Grid.pixelGap);
            if (!element.Item3)
            {
                foreach (var pixel in movePixels)
                {
                    if (Convert.ToInt16(Math.Sqrt(Math.Pow(middlePixel.Item1 - pixel.Item1, 2) +
                                             Math.Pow(middlePixel.Item2 - pixel.Item2, 2)) / Grid.pixelGap) == (int)(smallAxis / 2))
                    {
                        pixelList.Add((pixel.Item1, pixel.Item2, 5));
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
                                      Convert.ToInt16(middlePixel.Item2 + i * Grid.pixelGap * j), 5));
                    }
                }
            }
        }
        (int, int, int)[] pixelCoords = pixelList.ToArray();
        return pixelCoords;
    }
}
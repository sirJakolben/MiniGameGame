using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using MonoGameLibrary;

namespace MiniGameLogic;

public class TicTacToe
{
    public static List<(int, int, bool isPlrOne)> moveList;
    public static List<(int, int, bool isPlrOne)> winList;
    static bool isPlrOne;
    static Random random;
    public static void Clear()
    {
        random = new Random();
        isPlrOne = true;
        moveList = new();
        winList = new();
    }
    public static (int, int, int drawId)[] Logic((int, int, int gameId) clickedPos, bool isSinglePlr)
    {
        var botMoveList = moveList.Where(x => x.isPlrOne == false).ToArray();
        var plrMoveList = moveList.Where(x => x.isPlrOne == true).ToArray();
        var checkPlrWin = CheckWin(plrMoveList, botMoveList);
        var checkBotWin = CheckWin(botMoveList, plrMoveList);
        foreach (var element in moveList)
        {
            if (element.Item1 + 3 * element.Item2 == clickedPos.Item3 || winList.Count() == 3)
            {
                return ConvertToPixels(moveList, winList);
            }
        }
        if (!(moveList.Count() == 0) || !(clickedPos.gameId == -3))
        {
           int y = (int)(clickedPos.Item3 / 3.0);
           int x = clickedPos.Item3 % 3;
           moveList.Add((x, y, isPlrOne)); 
        }
        isPlrOne = !isPlrOne;
        plrMoveList = moveList.Where(x => x.isPlrOne == true).ToArray();
        checkPlrWin = CheckWin(plrMoveList, botMoveList);
        if (checkPlrWin.Count() == 3)
        {
            winList = checkPlrWin;
        }
        else if (!isPlrOne && isSinglePlr && moveList.Count() < 9)
        {
            var botMove = BotMove();
            moveList.Add((botMove.Item1, botMove.Item2, isPlrOne));
            isPlrOne = !isPlrOne;
        }
        botMoveList = moveList.Where(x => x.isPlrOne == false).ToArray();
        checkBotWin = CheckWin(botMoveList, plrMoveList);  
        if (checkBotWin.Count() == 3)
        {
            winList = checkBotWin;
        }  
        return ConvertToPixels(moveList, winList);
    }

    static (int, int) BotMove()
    {
        bool invalidMove = true;
        int rndmMove = 0;
        var botMoveList = moveList.Where(x => x.isPlrOne == false).ToArray();
        var plrMoveList = moveList.Where(x => x.isPlrOne == true).ToArray();
        var zwischenSpeicher = CheckWin(botMoveList, plrMoveList); // checks if bot has a wining move
        if (zwischenSpeicher.Count() == 1)
        {
            return (zwischenSpeicher[0].Item1, zwischenSpeicher[0].Item2);
        }
        zwischenSpeicher = CheckWin(plrMoveList, botMoveList); // checks if player has a wining move
        if (zwischenSpeicher.Count() == 1)
        {
            return (zwischenSpeicher[0].Item1, zwischenSpeicher[0].Item2);
        }         
        while (invalidMove) // places randomly
        {
            invalidMove = false;
            rndmMove = random.Next(0, 9);
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
    static List<(int, int, bool isPlrOne)> CheckWin((int, int, bool)[] plrMoveList, (int, int, bool)[] notPlrMoveList) // returns either list with 3 coords (if win) or list with one coord (if possible win)
    {
        if (plrMoveList.Length < 2) return new();
        List<(int, int, bool isPlrOne)> zwischenSpeicher = new();
        bool isMovePlr = plrMoveList[0].Item3;
        for (int i = 0; i < 3; i++) // checks for vertical and horizontal wins / possible wins 
        {
            zwischenSpeicher = plrMoveList.Where(x => x.Item1 == i).ToList();
            if (zwischenSpeicher.Count() == 3)
                return zwischenSpeicher;
            else if (zwischenSpeicher.Count() == 2 && !notPlrMoveList.Any(x => x.Item1 == i))
            {
                return new List<(int, int, bool isPlrOne)> {
                    (zwischenSpeicher[0].Item1, 3 - zwischenSpeicher[0].Item2 - zwischenSpeicher[1].Item2, isMovePlr) };
            }
            zwischenSpeicher = plrMoveList.Where(x => x.Item2 == i).ToList();
            if (zwischenSpeicher.Count() == 3)
                return zwischenSpeicher;
            else if (zwischenSpeicher.Count() == 2 && !notPlrMoveList.Any(x => x.Item2 == i))
            {
                return new List<(int, int, bool isPlrOne)> {
                    (3 - zwischenSpeicher[0].Item1 - zwischenSpeicher[1].Item1, zwischenSpeicher[0].Item2, isMovePlr)};
            }
        }
        var mainDiag = new List<(int, int, bool)> { (0, 0, isMovePlr), (1, 1, isMovePlr), (2, 2, isMovePlr) }; // checks for diagonal wins / possible wins
        var antiDiag = new List<(int, int, bool)> { (0, 2, isMovePlr), (1, 1, isMovePlr), (2, 0, isMovePlr) };
        zwischenSpeicher = plrMoveList.Intersect(mainDiag).ToList();
        if (zwischenSpeicher.Count() == 0) return new();
        else if (zwischenSpeicher.Count() == 3)
            return zwischenSpeicher;
        else if (zwischenSpeicher.Count() == 2 && !notPlrMoveList.Any(m => mainDiag.Any(d => d.Item1 == m.Item1 && d.Item2 == m.Item2)))
        {
            return mainDiag.Except(zwischenSpeicher).ToList();
        }
        zwischenSpeicher = plrMoveList.Intersect(antiDiag).ToList();
        if (zwischenSpeicher.Count() == 3)
            return zwischenSpeicher;
        else if (zwischenSpeicher.Count() == 2 && !notPlrMoveList.Any(m => antiDiag.Any(d => d.Item1 == m.Item1 && d.Item2 == m.Item2)))
        {
            return antiDiag.Except(zwischenSpeicher).ToList();
        }
        return new();
    }
    public static (int, int, int)[] ConvertToPixels(List<(int, int, bool)> moveList, List<(int, int, bool)> winList)
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
            if (!element.Item3) // draws a Circle for player 2
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
            else // draws a X for player 1
            {
                for (int i = 0; i < (int)(1.0 / 2 * (smallAxis - 2) + 1); i++)
                {
                    for (int x = -1; x < 2; x += 2)
                    {
                        for (int y = -1; y < 2; y += 2)
                        {
                            (double, double) offset = (0, 0);
                            if (!Grid.clickable.Any(Coord => Coord.x == middlePixel.Item1))
                                offset.Item1 = 0.5;
                            if (!Grid.clickable.Any(Coord => Coord.y == middlePixel.Item2))
                                offset.Item2 = 0.5;
                            pixelList.Add((Convert.ToInt16(middlePixel.Item1 + Grid.pixelGap * x * (i + offset.Item1)),
                                           Convert.ToInt16(middlePixel.Item2 + Grid.pixelGap * y * (i + offset.Item2)), 5));                                                 
                        }
                    }
                }
            }
        }
        foreach (var element in winList)
        {
            var movePixels = Grid.clickable.Where(x => x.Item3 == element.Item1 + 3 * element.Item2).ToArray();
            var firstPixel = movePixels[0];
            var lastPixel = movePixels[movePixels.Length - 1];
            foreach (var pixel in movePixels)
            {
                if (pixel.x == firstPixel.x || pixel.x == lastPixel.x || pixel.y == firstPixel.y || pixel.y == lastPixel.y)
                {
                    pixelList.Add((pixel.x, pixel.y, 4));
                }
            }
        }
        (int, int, int)[] pixelCoords = pixelList.ToArray();
        return pixelCoords;
    }
}
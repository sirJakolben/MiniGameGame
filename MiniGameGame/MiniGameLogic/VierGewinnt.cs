using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using MonoGameLibrary;

namespace MiniGameLogic;

public class VierGewinnt
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
        if (!(moveList.Count() == 0) || !(clickedPos.gameId == -3))
        {
            int y = (int)(5 - moveList.Where(x => x.Item1 == clickedPos.gameId).Count());
            if (y < 0)
                return ConvertToPixels(moveList, winList);
            Debug.Print("y =" + y);
            int x = clickedPos.Item3;
            moveList.Add((x, y, isPlrOne));
        }
        if (winList.Count == 4)
            return ConvertToPixels(moveList, winList);
        isPlrOne = !isPlrOne;
        var botMoveList = moveList.Where(x => x.isPlrOne == false).ToArray();
        var plrMoveList = moveList.Where(x => x.isPlrOne == true).ToArray();
        var checkPlrWin = CheckWin(plrMoveList, botMoveList);
        if (checkPlrWin.Count() == 4)
        {
            winList = checkPlrWin;
        }
        else if (!isPlrOne && isSinglePlr && moveList.Count() < 42)
        {
            var botMove = BotMove();
            moveList.Add((botMove.Item1, botMove.Item2, isPlrOne));
            isPlrOne = !isPlrOne;

            botMoveList = moveList.Where(x => x.isPlrOne == false).ToArray();
            var checkBotWin = CheckWin(botMoveList, plrMoveList);
            if (checkBotWin.Count() == 4)
            {
                winList = checkBotWin;
            }  
        } 
        return ConvertToPixels(moveList, winList);
    }
    static (int, int) BotMove()
    {
        bool invalidMove = true;
        int rndmX = 0;
        int rndmY = 0;
        var botMoveList = moveList.Where(x => x.isPlrOne == false).ToArray();
        var plrMoveList = moveList.Where(x => x.isPlrOne == true).ToArray();
        if (botMoveList.Count() > 1)
        {
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
        }
        while (invalidMove) // places randomly
        {
            invalidMove = false;
            rndmX = random.Next(0, 7);
            rndmY = (int)(5 - moveList.Where(x => x.Item1 == rndmX).Count());
            if (rndmY < 0)
                invalidMove = true;
        }
        return (rndmX, rndmY);
    }
    public static (int, int, int)[] ConvertToPixels(List<(int, int, bool)> moveList, List<(int, int, bool)> winList)
    {
        List<(int, int, int)> pixelList = new();
        foreach (var element in moveList)
        {
            var movePixels = Grid.clickable.Where(x => x.Item3 == element.Item1 && ((int)(x.y / (Grid.pixelGap * 5)) == element.Item2)).ToArray();
            if (!element.Item3) // draws plate for player 2
            {
                foreach (var pixel in movePixels)
                {
                    pixelList.Add((pixel.Item1, pixel.Item2, 4));
                }
            }
            else // draws a plate for player 1
            {
                foreach (var pixel in movePixels)
                {
                    pixelList.Add((pixel.Item1, pixel.Item2, 5));
                }
            }
        }
        foreach (var element in winList)
        {
            var movePixels = Grid.clickable.Where(x => x.Item3 == element.Item1 && 6 - (x.y / 6 * Grid.pixelGap) == element.Item2).ToArray();
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
    static List<(int, int, bool isPlrOne)> CheckWin((int, int, bool)[] plrMoveList, (int, int, bool)[] notPlrMoveList) // returns either list with 3 coords (if win) or list with one coord (if possible win)
    {
        List<(int, int, bool isPlrOne)> zwischenSpeicher = new();
        bool isMovePlr = plrMoveList[0].Item3;
        for (int i = 0; i < 7; i++) // checks for vertical and horizontal wins / possible wins 
        {
            zwischenSpeicher = plrMoveList.Where(x => x.Item1 == i).ToList();
            int previousPosition = 69;
            int counter = 0;
            int index = 0;
            foreach (var position in zwischenSpeicher)
            {
                if (position.Item2 == previousPosition + 1)
                    counter++;
                else
                    counter = 0;
                if (counter == 3)
                    return new List<(int, int, bool isPlrOne)>
                    {
                        position,
                        (position.Item1, position.Item2 -1, position.isPlrOne),
                        (position.Item1, position.Item2 -2, position.isPlrOne),
                        (position.Item1, position.Item2 -3, position.isPlrOne)
                    };
                else if (counter == 2)
                {
                    if (!moveList.Any(x => x.Item1 == position.Item1 && x.Item2 == position.Item2 + 1))
                    {
                        return new List<(int, int, bool isPlrOne)>
                        {
                            (position.Item1, position.Item2 + 1, position.isPlrOne),
                        };
                    }
                    else if (!moveList.Any(x => x.Item1 == position.Item1 && x.Item2 == position.Item2 - 3))
                    {
                        return new List<(int, int, bool isPlrOne)>
                        {
                            (position.Item1, position.Item2 -3, position.isPlrOne),
                        };
                    }
                }
                previousPosition = position.Item2;
                bool isLast = index == zwischenSpeicher.Count - 1;
                index++;
                if (isLast)
                {
                    if (counter == 2)
                        return new List<(int, int, bool isPlrOne)>
                    {
                        (position.Item1, position.Item2 +1, position.isPlrOne)
                    };
                }
            }
            if (i != 7)
            {
                zwischenSpeicher = plrMoveList.Where(x => x.Item2 == i).ToList();
                previousPosition = 69;
                counter = 0;
                index = 0;
                foreach (var position in zwischenSpeicher)
                {
                if (position.Item1 == previousPosition + 1)
                    counter++;
                else
                    counter = 0;
                if (counter == 3)
                    return new List<(int, int, bool isPlrOne)>
                    {
                        position,
                        (position.Item1 -1, position.Item2, position.isPlrOne),
                        (position.Item1 -2, position.Item2, position.isPlrOne),
                        (position.Item1 -3, position.Item2, position.isPlrOne)
                    };
                else if (counter == 2)
                {
                    if (!moveList.Any(x => x.Item1 == position.Item1 + 1 && x.Item2 == position.Item2))
                    {
                        return new List<(int, int, bool isPlrOne)>
                        {
                            (position.Item1 +1, position.Item2, position.isPlrOne),
                        };
                    }
                    else if (!moveList.Any(x => x.Item1 == position.Item1 -3 && x.Item2 == position.Item2))
                    {
                        return new List<(int, int, bool isPlrOne)>
                        {
                            (position.Item1 -3, position.Item2, position.isPlrOne),
                        };
                    }
                }
                    previousPosition = position.Item1;
                    bool isLast = index == zwischenSpeicher.Count - 1;
                    index++;
                    if (isLast)
                    {
                        if (counter == 2)
                            return new List<(int, int, bool isPlrOne)>
                        {
                            (position.Item1 +1, position.Item2, position.isPlrOne)
                        };
                    }
                }
            }         
        }

        return new();
    }
}
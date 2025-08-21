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
            if ((y < 0))
                return ConvertToPixels(moveList, winList);
            Debug.Print("y =" + y);
           int x = clickedPos.Item3;
           moveList.Add((x, y, isPlrOne)); 
        }
        if (winList.Count == 4)
            return ConvertToPixels(moveList, winList);
        isPlrOne = !isPlrOne;
        // var botMoveList = moveList.Where(x => x.isPlrOne == false).ToArray();
        // var plrMoveList = moveList.Where(x => x.isPlrOne == true).ToArray();
        // var checkPlrWin = CheckWin(plrMoveList, botMoveList);
        // if (checkPlrWin.Count() == 4)
        // {
        //     winList = checkPlrWin;
        // }
        // else if (!isPlrOne && isSinglePlr && moveList.Count() < 42)
        // {
        //     var botMove = BotMove();
        //     moveList.Add((botMove.Item1, botMove.Item2, isPlrOne));
        //     isPlrOne = !isPlrOne;

        //     botMoveList = moveList.Where(x => x.isPlrOne == false).ToArray();
        //     var checkBotWin = CheckWin(botMoveList, plrMoveList);
        //     if (checkBotWin.Count() == 4)
        //     {
        //         winList = checkBotWin;
        //     }  
        // }  
        return ConvertToPixels(moveList, winList);
    }
   
    // static (int, int) BotMove()
    // {
        
    // }
    // static List<(int, int, bool isPlrOne)> CheckWin((int, int, bool)[] plrMoveList, (int, int, bool)[] notPlrMoveList)
    // {
        
    // }
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
}
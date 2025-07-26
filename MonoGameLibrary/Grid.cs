using Microsoft.Xna.Framework;
namespace MonoGameLibrary;
public class Grid
{
    public static Point[] Coords(int width, int height, Point playGridSize, double sizeMultiplyer, out double gridGap)
    {
        Point[] nodeCords;
        int offset;
        if (width / playGridSize.X > height / playGridSize.Y)
        {
            gridGap = height * sizeMultiplyer / playGridSize.Y ;
            offset = (int)(0.5f * (height - sizeMultiplyer * height));
        }
        else
        {
            gridGap = width * sizeMultiplyer / playGridSize.X;
            offset = (int)(0.5f * (width - sizeMultiplyer * width));
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
        return nodeCords;
    }
}
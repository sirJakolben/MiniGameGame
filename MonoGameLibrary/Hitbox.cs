using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

public class Hitbox
{
    static Color[] hitboxes;
    public static void Clear((int,int) startScreen)
    {
        hitboxes = new Color[startScreen.Item1 * startScreen.Item2];
        for(int i = 0; i < hitboxes.Length; i++)
        {
            hitboxes[i] = Color.Transparent;
        }
    }
    public static Color[] Maker((int,int) startScreen, Vector2 screenSizeMult)
    {
        MouseState mouseState = Mouse.GetState();

        if(mouseState.LeftButton == ButtonState.Pressed)
        {
            hitboxes[(int)(mouseState.Y * startScreen.Item1 * screenSizeMult.X * 1/screenSizeMult.Y + mouseState.X * 1/screenSizeMult.X)] = Color.Red;
        }
        return hitboxes;
    }
}
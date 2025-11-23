using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGameLibrary;
using MiniGameLogic;
using System.Linq;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;

namespace MiniGameGame;


public class MainClass : Core
{
    private Texture2D mainScreen;
    private Texture2D HitboxTex;
    public static (int,int) startScreen = (1920,1080);
    public static Vector2 screenSizeMult;
	public MainClass() : base("MiniGameGame", startScreen.Item1, startScreen.Item2, false)
    {
        
    }
    protected override void Initialize()
    {
        Hitbox.Clear(startScreen);
        base.Initialize();
    }
    protected override void LoadContent()
    {
        mainScreen = Content.Load<Texture2D>("MainScreen");
        HitboxTex = new Texture2D(GraphicsDevice, startScreen.Item1, startScreen.Item2);
        base.LoadContent();
    }
    protected override void Update(GameTime gameTime)
    {
        HitboxTex.SetData(Hitbox.Maker(startScreen, screenSizeMult));
    }
    protected override void Draw(GameTime gameTime)
    {
        (int,int) currentWindowSize = (Window.ClientBounds.Width, Window.ClientBounds.Height);
        screenSizeMult = new Vector2((float)currentWindowSize.Item1 / (float)startScreen.Item1, (float)currentWindowSize.Item2 / (float)startScreen.Item2);
        SpriteBatch.Begin();

        SpriteBatch.Draw(mainScreen, Vector2.Zero, null, Color.White, 0f,
                            Vector2.Zero, screenSizeMult, SpriteEffects.None, 0.0f);
        SpriteBatch.Draw(HitboxTex, Vector2.Zero, null, Color.White, 0f,
                            Vector2.Zero, screenSizeMult, SpriteEffects.None, 0.0f);

        SpriteBatch.End();
    }
}
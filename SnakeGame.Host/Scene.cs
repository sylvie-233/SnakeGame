using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Host;

public abstract class Scene
{
    protected SnakeGame Game { get; }
    protected SpriteBatch SpriteBatch { get; }
    protected SpriteFont Font { get; }
    protected GraphicsDevice GraphicsDevice { get; }

    public Scene(SnakeGame game, SpriteBatch spriteBatch, SpriteFont font)
    {
        Game = game;
        SpriteBatch = spriteBatch;
        Font = font;
        GraphicsDevice = game.GraphicsDevice;
    }

    public abstract void Initialize();
    public abstract void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState previousKeyboardState);
    public abstract void Draw(GameTime gameTime);
}

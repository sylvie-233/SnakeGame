using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Host;

public enum SceneType
{
    Start,
    Game,
    GameOver
}

public class SnakeGame : Game
{
    public const int WindowWidth = 720;
    public const int WindowHeight = 640;
    
    public GraphicsDeviceManager GraphicsManager { get; private set; }
    public int LastScore { get; set; }
    public int HighScore { get; set; }
    
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private SpriteFont _font;
    private Scene _currentScene;
    private StartScene _startScene;
    private GameScene _gameScene;
    private GameOverScene _gameOverScene;
    private KeyboardState _previousKeyboardState;

    public SnakeGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        GraphicsManager = _graphics;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = WindowWidth;
        _graphics.PreferredBackBufferHeight = WindowHeight;
        _graphics.ApplyChanges();

        HighScore = 0;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _font = Content.Load<SpriteFont>("Font/Arial");

        _startScene = new StartScene(this, _spriteBatch, _font);
        _gameScene = new GameScene(this, _spriteBatch, _font);
        _gameOverScene = new GameOverScene(this, _spriteBatch, _font);

        ChangeScene(SceneType.Start);
    }

    protected override void Update(GameTime gameTime)
    {
        KeyboardState currentKeyboardState = Keyboard.GetState();

        _currentScene?.Update(gameTime, currentKeyboardState, _previousKeyboardState);

        _previousKeyboardState = currentKeyboardState;

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        _currentScene?.Draw(gameTime);

        base.Draw(gameTime);
    }

    public void ChangeScene(SceneType sceneType)
    {
        _currentScene = sceneType switch
        {
            SceneType.Start => _startScene,
            SceneType.Game => _gameScene,
            SceneType.GameOver => _gameOverScene,
            _ => _currentScene
        };

        _currentScene?.Initialize();
    }
}

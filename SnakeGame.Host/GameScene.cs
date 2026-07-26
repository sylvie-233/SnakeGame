using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SnakeGame.Core;

namespace SnakeGame.Host;

public class GameScene : Scene
{
    private GameEngine _gameEngine;
    private float _updateTimer;
    private const float UpdateInterval = 0.15f;
    private int _cellSize = 22;
    private bool _gameOverTriggered;
    private float _foodPulseTimer;
    
    private const int GridWidth = 22;
    private const int GridHeight = 22;
    private const int PanelWidth = 200;
    private const int Padding = 16;
    
    private int GridOffsetX => Padding;
    private int GridOffsetY => Padding;
    private int GridPixelWidth => GridWidth * _cellSize;
    private int GridPixelHeight => GridHeight * _cellSize;
    private int PanelOffsetX => GridOffsetX + GridPixelWidth + Padding;

    public GameScene(SnakeGame game, SpriteBatch spriteBatch, SpriteFont font) 
        : base(game, spriteBatch, font)
    {
    }

    public override void Initialize()
    {
        _gameEngine = new GameEngine(GridWidth, GridHeight);
        _updateTimer = 0;
        _gameOverTriggered = false;
        _foodPulseTimer = 0;
    }

    public override void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState previousKeyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
        {
            Game.ChangeScene(SceneType.Start);
        }

        HandleInput(keyboardState, previousKeyboardState);

        _updateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_updateTimer >= UpdateInterval)
        {
            _gameEngine.Update();
            _updateTimer = 0;
        }

        _foodPulseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_gameEngine.State == GameState.GameOver && !_gameOverTriggered)
        {
            _gameOverTriggered = true;
            Game.LastScore = _gameEngine.Score;
            if (_gameEngine.Score > Game.HighScore)
            {
                Game.HighScore = _gameEngine.Score;
            }
            Game.ChangeScene(SceneType.GameOver);
        }
    }

    private void HandleInput(KeyboardState keyboardState, KeyboardState previousKeyboardState)
    {
        if (keyboardState.IsKeyDown(Keys.W) && !previousKeyboardState.IsKeyDown(Keys.W))
            _gameEngine.ChangeDirection(Direction.Up);
        if (keyboardState.IsKeyDown(Keys.S) && !previousKeyboardState.IsKeyDown(Keys.S))
            _gameEngine.ChangeDirection(Direction.Down);
        if (keyboardState.IsKeyDown(Keys.A) && !previousKeyboardState.IsKeyDown(Keys.A))
            _gameEngine.ChangeDirection(Direction.Left);
        if (keyboardState.IsKeyDown(Keys.D) && !previousKeyboardState.IsKeyDown(Keys.D))
            _gameEngine.ChangeDirection(Direction.Right);

        if (keyboardState.IsKeyDown(Keys.Up) && !previousKeyboardState.IsKeyDown(Keys.Up))
            _gameEngine.ChangeDirection(Direction.Up);
        if (keyboardState.IsKeyDown(Keys.Down) && !previousKeyboardState.IsKeyDown(Keys.Down))
            _gameEngine.ChangeDirection(Direction.Down);
        if (keyboardState.IsKeyDown(Keys.Left) && !previousKeyboardState.IsKeyDown(Keys.Left))
            _gameEngine.ChangeDirection(Direction.Left);
        if (keyboardState.IsKeyDown(Keys.Right) && !previousKeyboardState.IsKeyDown(Keys.Right))
            _gameEngine.ChangeDirection(Direction.Right);

        if (keyboardState.IsKeyDown(Keys.P) && !previousKeyboardState.IsKeyDown(Keys.P))
            _gameEngine.TogglePause();

        if (keyboardState.IsKeyDown(Keys.R) && !previousKeyboardState.IsKeyDown(Keys.R))
            _gameEngine.Restart();
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ColorPalette.Background);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        DrawBackgroundGradient();
        DrawGrid();
        DrawSnake();
        DrawFood();
        DrawHud();
        DrawGameState();

        SpriteBatch.End();
    }

    private void DrawBackgroundGradient()
    {
        UiHelper.DrawGradient(SpriteBatch, 
            new Rectangle(0, 0, SnakeGame.WindowWidth, SnakeGame.WindowHeight),
            ColorPalette.Background, ColorPalette.BackgroundLight);
    }

    private void DrawGrid()
    {
        Rectangle gridRect = new Rectangle(GridOffsetX - 2, GridOffsetY - 2, GridPixelWidth + 4, GridPixelHeight + 4);
        
        UiHelper.DrawRoundedRectangle(SpriteBatch, gridRect, 8, ColorPalette.Panel);
        UiHelper.DrawRoundedRectangleBorder(SpriteBatch, gridRect, 8, ColorPalette.Border);

        for (int x = 0; x <= GridWidth; x++)
        {
            int px = GridOffsetX + x * _cellSize;
            SpriteBatch.Draw(UiHelper.CreateColorTexture(GraphicsDevice, ColorPalette.GridLine),
                new Rectangle(px, GridOffsetY, 1, GridPixelHeight), Color.White);
        }

        for (int y = 0; y <= GridHeight; y++)
        {
            int py = GridOffsetY + y * _cellSize;
            SpriteBatch.Draw(UiHelper.CreateColorTexture(GraphicsDevice, ColorPalette.GridLine),
                new Rectangle(GridOffsetX, py, GridPixelWidth, 1), Color.White);
        }

        for (int x = 0; x < GridWidth; x++)
        {
            for (int y = 0; y < GridHeight; y++)
            {
                Rectangle cell = new Rectangle(GridOffsetX + x * _cellSize + 1, 
                    GridOffsetY + y * _cellSize + 1, _cellSize - 1, _cellSize - 1);
                Color color = (x + y) % 2 == 0 ? ColorPalette.GridCell : ColorPalette.Background;
                SpriteBatch.Draw(UiHelper.CreateColorTexture(GraphicsDevice, color), cell, Color.White);
            }
        }
    }

    private void DrawSnake()
    {
        for (int i = 0; i < _gameEngine.Snake.Segments.Count; i++)
        {
            Position segment = _gameEngine.Snake.Segments[i];
            int x = GridOffsetX + segment.X * _cellSize + 2;
            int y = GridOffsetY + segment.Y * _cellSize + 2;
            int size = _cellSize - 4;

            Rectangle rect = new Rectangle(x, y, size, size);
            Color color = i == 0 ? ColorPalette.SnakeHead : ColorPalette.SnakeBody;

            UiHelper.DrawRoundedRectangle(SpriteBatch, rect, size / 3, color);

            if (i == 0)
            {
                int eyeSize = size / 5;
                int eyeOffset = size / 4;
                Color eyeColor = Color.Black;

                SpriteBatch.Draw(UiHelper.CreateColorTexture(GraphicsDevice, eyeColor),
                    new Rectangle(x + eyeOffset, y + eyeOffset, eyeSize, eyeSize), Color.White);
                SpriteBatch.Draw(UiHelper.CreateColorTexture(GraphicsDevice, eyeColor),
                    new Rectangle(x + size - eyeOffset - eyeSize, y + eyeOffset, eyeSize, eyeSize), Color.White);
            }
        }
    }

    private void DrawFood()
    {
        Position foodPosition = _gameEngine.Food.Position;
        int x = GridOffsetX + foodPosition.X * _cellSize + 3;
        int y = GridOffsetY + foodPosition.Y * _cellSize + 3;
        int size = _cellSize - 6;

        float pulseScale = 1 + (float)System.Math.Sin(_foodPulseTimer * 8) * 0.15f;
        int scaledSize = (int)(size * pulseScale);
        int offset = (size - scaledSize) / 2;

        Rectangle glowRect = new Rectangle(x - 2 + offset, y - 2 + offset, scaledSize + 4, scaledSize + 4);
        Color glowColor = Color.Lerp(ColorPalette.FoodGlow, Color.Transparent, 0.6f);
        UiHelper.DrawRoundedRectangle(SpriteBatch, glowRect, (scaledSize + 4) / 2, glowColor);

        Rectangle rect = new Rectangle(x + offset, y + offset, scaledSize, scaledSize);
        UiHelper.DrawRoundedRectangle(SpriteBatch, rect, scaledSize / 2, ColorPalette.Food);
    }

    private void DrawHud()
    {
        Rectangle panelRect = new Rectangle(PanelOffsetX, Padding, PanelWidth, SnakeGame.WindowHeight - Padding * 2);
        UiHelper.DrawRoundedRectangle(SpriteBatch, panelRect, 12, ColorPalette.Panel);
        UiHelper.DrawRoundedRectangleBorder(SpriteBatch, panelRect, 12, ColorPalette.Border);

        int y = Padding + 20;

        string title = "SCORE";
        UiHelper.DrawCenteredText(SpriteBatch, Font, title, 
            new Rectangle(PanelOffsetX, y, PanelWidth, 30), ColorPalette.TextSecondary);
        y += 35;

        string score = _gameEngine.Score.ToString();
        UiHelper.DrawCenteredTextShadow(SpriteBatch, Font, score, 
            new Rectangle(PanelOffsetX, y, PanelWidth, 60), ColorPalette.ScoreColor, Color.Black);
        y += 70;

        string highScoreLabel = "HIGH SCORE";
        UiHelper.DrawCenteredText(SpriteBatch, Font, highScoreLabel, 
            new Rectangle(PanelOffsetX, y, PanelWidth, 25), ColorPalette.TextDim);
        y += 30;

        string highScore = Game.HighScore.ToString();
        UiHelper.DrawCenteredTextShadow(SpriteBatch, Font, highScore, 
            new Rectangle(PanelOffsetX, y, PanelWidth, 40), ColorPalette.HighScoreColor, Color.Black);
        y += 50;

        Rectangle divider = new Rectangle(PanelOffsetX + 20, y, PanelWidth - 40, 2);
        SpriteBatch.Draw(UiHelper.CreateColorTexture(GraphicsDevice, ColorPalette.Border), divider, Color.White);
        y += 30;

        string controlsLabel = "CONTROLS";
        UiHelper.DrawCenteredText(SpriteBatch, Font, controlsLabel, 
            new Rectangle(PanelOffsetX, y, PanelWidth, 25), ColorPalette.TextSecondary);
        y += 35;

        string[] controls = {
            "W / UP     Up",
            "S / DOWN   Down",
            "A / LEFT   Left",
            "D / RIGHT  Right",
            "",
            "P          Pause",
            "R          Restart",
            "ESC        Menu"
        };

        foreach (string control in controls)
        {
            Vector2 textSize = Font.MeasureString(control);
            float x = PanelOffsetX + (PanelWidth - textSize.X) / 2;
            SpriteBatch.DrawString(Font, control, new Vector2(x, y), 
                string.IsNullOrWhiteSpace(control) ? Color.Transparent : ColorPalette.TextDim);
            y += 24;
        }

        y = SnakeGame.WindowHeight - Padding - 80;

        string hint = "Eat red food to grow!";
        UiHelper.DrawCenteredText(SpriteBatch, Font, hint, 
            new Rectangle(PanelOffsetX, y, PanelWidth, 30), ColorPalette.TextSecondary);
    }

    private void DrawGameState()
    {
        if (_gameEngine.State == GameState.Paused)
        {
            Rectangle overlay = new Rectangle(GridOffsetX - 2, GridOffsetY - 2, GridPixelWidth + 4, GridPixelHeight + 4);
            Color overlayColor = new Color(0, 0, 0, 150);
            UiHelper.DrawRoundedRectangle(SpriteBatch, overlay, 8, overlayColor);

            string text = "PAUSED";
            Vector2 textSize = Font.MeasureString(text);
            Vector2 position = new Vector2(
                GridOffsetX + (GridPixelWidth - textSize.X) / 2,
                GridOffsetY + (GridPixelHeight - textSize.Y) / 2 - 20);
            UiHelper.DrawTextShadow(SpriteBatch, Font, text, position, ColorPalette.Warning, Color.Black);

            string subText = "Press P to resume";
            Vector2 subTextSize = Font.MeasureString(subText);
            Vector2 subPosition = new Vector2(
                GridOffsetX + (GridPixelWidth - subTextSize.X) / 2,
                GridOffsetY + (GridPixelHeight - subTextSize.Y) / 2 + 20);
            SpriteBatch.DrawString(Font, subText, subPosition, ColorPalette.TextSecondary);
        }
    }
}

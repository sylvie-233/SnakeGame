using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Host;

public class StartScene : Scene
{
    private Rectangle _startButton;
    private Rectangle _highScoreButton;
    private bool _isStartHovered;
    private bool _isHighScoreHovered;
    private float _titlePulseTimer;

    public StartScene(SnakeGame game, SpriteBatch spriteBatch, SpriteFont font) 
        : base(game, spriteBatch, font)
    {
    }

    public override void Initialize()
    {
        int buttonWidth = 240;
        int buttonHeight = 56;
        int x = (SnakeGame.WindowWidth - buttonWidth) / 2;
        
        _startButton = new Rectangle(x, SnakeGame.WindowHeight / 2 - 20, buttonWidth, buttonHeight);
        _highScoreButton = new Rectangle(x, SnakeGame.WindowHeight / 2 + 50, buttonWidth, buttonHeight);
        
        _isStartHovered = false;
        _isHighScoreHovered = false;
        _titlePulseTimer = 0;
    }

    public override void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState previousKeyboardState)
    {
        MouseState mouseState = Mouse.GetState();
        _isStartHovered = _startButton.Contains(mouseState.X, mouseState.Y);
        _isHighScoreHovered = _highScoreButton.Contains(mouseState.X, mouseState.Y);

        _titlePulseTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

        if (_isStartHovered && mouseState.LeftButton == ButtonState.Pressed)
        {
            Game.ChangeScene(SceneType.Game);
        }

        if (_isHighScoreHovered && mouseState.LeftButton == ButtonState.Pressed)
        {
        }

        if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
        {
            Game.ChangeScene(SceneType.Game);
        }

        if (keyboardState.IsKeyDown(Keys.Escape))
        {
            Game.Exit();
        }
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ColorPalette.Background);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        DrawBackgroundGradient();
        DrawTitle();
        DrawButtons();
        DrawHighScore();
        DrawFooter();

        SpriteBatch.End();
    }

    private void DrawBackgroundGradient()
    {
        UiHelper.DrawGradient(SpriteBatch, 
            new Rectangle(0, 0, SnakeGame.WindowWidth, SnakeGame.WindowHeight),
            ColorPalette.Background, ColorPalette.BackgroundLight);
    }

    private void DrawTitle()
    {
        float pulseScale = 1 + (float)System.Math.Sin(_titlePulseTimer * 2) * 0.03f;
        
        string title = "SNAKE";
        Vector2 titleSize = Font.MeasureString(title);
        Vector2 titlePosition = new Vector2(
            (SnakeGame.WindowWidth - titleSize.X) / 2,
            SnakeGame.WindowHeight / 2 - 180);

        UiHelper.DrawTextShadow(SpriteBatch, Font, title, titlePosition * pulseScale, ColorPalette.SnakeHead, Color.Black, 3);

        string subtitle = "Classic Arcade Game";
        Vector2 subtitleSize = Font.MeasureString(subtitle);
        Vector2 subtitlePosition = new Vector2(
            (SnakeGame.WindowWidth - subtitleSize.X) / 2,
            titlePosition.Y + titleSize.Y + 20);
        SpriteBatch.DrawString(Font, subtitle, subtitlePosition, ColorPalette.TextSecondary);
    }

    private void DrawButtons()
    {
        DrawButton(_startButton, _isStartHovered, "START GAME");
        DrawButton(_highScoreButton, _isHighScoreHovered, "HIGH SCORE");
    }

    private void DrawButton(Rectangle rect, bool isHovered, string text)
    {
        float scale = isHovered ? 1.05f : 1f;
        int scaledWidth = (int)(rect.Width * scale);
        int scaledHeight = (int)(rect.Height * scale);
        int offsetX = (rect.Width - scaledWidth) / 2;
        int offsetY = (rect.Height - scaledHeight) / 2;

        Rectangle scaledRect = new Rectangle(rect.X + offsetX, rect.Y + offsetY, scaledWidth, scaledHeight);

        UiHelper.DrawRoundedRectangle(SpriteBatch, scaledRect, 12, 
            isHovered ? ColorPalette.ButtonHover : ColorPalette.ButtonPrimary);
        UiHelper.DrawRoundedRectangleBorder(SpriteBatch, scaledRect, 12, 
            isHovered ? ColorPalette.ButtonBorder : ColorPalette.ButtonPrimary, 2);

        Vector2 textSize = Font.MeasureString(text);
        Vector2 textPosition = new Vector2(
            scaledRect.X + (scaledRect.Width - textSize.X) / 2,
            scaledRect.Y + (scaledRect.Height - textSize.Y) / 2);
        
        UiHelper.DrawTextShadow(SpriteBatch, Font, text, textPosition, Color.White, Color.Black);
    }

    private void DrawHighScore()
    {
        if (Game.HighScore > 0)
        {
            string highScoreText = $"Best Score: {Game.HighScore}";
            Vector2 textSize = Font.MeasureString(highScoreText);
            Vector2 textPosition = new Vector2(
                (SnakeGame.WindowWidth - textSize.X) / 2,
                _highScoreButton.Y + _highScoreButton.Height + 30);
            UiHelper.DrawTextShadow(SpriteBatch, Font, highScoreText, textPosition, ColorPalette.HighScoreColor, Color.Black);
        }
    }

    private void DrawFooter()
    {
        string footer = "Use Arrow Keys or WASD to control | ESC to exit";
        Vector2 footerSize = Font.MeasureString(footer);
        Vector2 footerPosition = new Vector2(
            (SnakeGame.WindowWidth - footerSize.X) / 2,
            SnakeGame.WindowHeight - 40);
        SpriteBatch.DrawString(Font, footer, footerPosition, ColorPalette.TextDim);
    }
}

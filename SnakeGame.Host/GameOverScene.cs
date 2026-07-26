using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SnakeGame.Host;

public class GameOverScene : Scene
{
    private Rectangle _restartButton;
    private Rectangle _menuButton;
    private bool _isRestartHovered;
    private bool _isMenuHovered;
    private bool _isNewHighScore;

    public GameOverScene(SnakeGame game, SpriteBatch spriteBatch, SpriteFont font) 
        : base(game, spriteBatch, font)
    {
    }

    public override void Initialize()
    {
        int buttonWidth = 200;
        int buttonHeight = 50;
        int x = (SnakeGame.WindowWidth - buttonWidth) / 2;
        
        _restartButton = new Rectangle(x, SnakeGame.WindowHeight / 2 + 40, buttonWidth, buttonHeight);
        _menuButton = new Rectangle(x, SnakeGame.WindowHeight / 2 + 110, buttonWidth, buttonHeight);
        
        _isRestartHovered = false;
        _isMenuHovered = false;
        _isNewHighScore = Game.LastScore >= Game.HighScore && Game.LastScore > 0;
    }

    public override void Update(GameTime gameTime, KeyboardState keyboardState, KeyboardState previousKeyboardState)
    {
        MouseState mouseState = Mouse.GetState();
        _isRestartHovered = _restartButton.Contains(mouseState.X, mouseState.Y);
        _isMenuHovered = _menuButton.Contains(mouseState.X, mouseState.Y);

        if (_isRestartHovered && mouseState.LeftButton == ButtonState.Pressed)
        {
            Game.ChangeScene(SceneType.Game);
        }

        if (_isMenuHovered && mouseState.LeftButton == ButtonState.Pressed)
        {
            Game.ChangeScene(SceneType.Start);
        }

        if (keyboardState.IsKeyDown(Keys.Enter) && !previousKeyboardState.IsKeyDown(Keys.Enter))
        {
            Game.ChangeScene(SceneType.Game);
        }

        if (keyboardState.IsKeyDown(Keys.Escape) && !previousKeyboardState.IsKeyDown(Keys.Escape))
        {
            Game.ChangeScene(SceneType.Start);
        }
    }

    public override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ColorPalette.Background);

        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        DrawBackgroundGradient();
        DrawGameOverTitle();
        DrawScore();
        DrawButtons();
        DrawFooter();

        SpriteBatch.End();
    }

    private void DrawBackgroundGradient()
    {
        UiHelper.DrawGradient(SpriteBatch, 
            new Rectangle(0, 0, SnakeGame.WindowWidth, SnakeGame.WindowHeight),
            ColorPalette.Background, ColorPalette.BackgroundLight);
    }

    private void DrawGameOverTitle()
    {
        string title = "GAME OVER";
        Vector2 titleSize = Font.MeasureString(title);
        Vector2 titlePosition = new Vector2(
            (SnakeGame.WindowWidth - titleSize.X) / 2,
            SnakeGame.WindowHeight / 2 - 150);

        UiHelper.DrawTextShadow(SpriteBatch, Font, title, titlePosition, ColorPalette.Food, Color.Black, 3);

        if (_isNewHighScore)
        {
            string newHighScoreText = "NEW HIGH SCORE!";
            Vector2 newHighScoreSize = Font.MeasureString(newHighScoreText);
            Vector2 newHighScorePosition = new Vector2(
                (SnakeGame.WindowWidth - newHighScoreSize.X) / 2,
                titlePosition.Y + titleSize.Y + 15);
            UiHelper.DrawTextShadow(SpriteBatch, Font, newHighScoreText, newHighScorePosition, ColorPalette.HighScoreColor, Color.Black);
        }
    }

    private void DrawScore()
    {
        int panelWidth = 300;
        int panelHeight = 120;
        int panelX = (SnakeGame.WindowWidth - panelWidth) / 2;
        int panelY = SnakeGame.WindowHeight / 2 - 80;

        Rectangle panelRect = new Rectangle(panelX, panelY, panelWidth, panelHeight);
        UiHelper.DrawRoundedRectangle(SpriteBatch, panelRect, 12, ColorPalette.Panel);
        UiHelper.DrawRoundedRectangleBorder(SpriteBatch, panelRect, 12, ColorPalette.Border);

        string scoreLabel = "SCORE";
        UiHelper.DrawCenteredText(SpriteBatch, Font, scoreLabel, 
            new Rectangle(panelX, panelY + 10, panelWidth, 25), ColorPalette.TextSecondary);

        string score = Game.LastScore.ToString();
        UiHelper.DrawCenteredTextShadow(SpriteBatch, Font, score, 
            new Rectangle(panelX, panelY + 30, panelWidth, 50), ColorPalette.ScoreColor, Color.Black);

        string highScoreLabel = $"HIGH SCORE: {Game.HighScore}";
        UiHelper.DrawCenteredText(SpriteBatch, Font, highScoreLabel, 
            new Rectangle(panelX, panelY + 85, panelWidth, 25), ColorPalette.HighScoreColor);
    }

    private void DrawButtons()
    {
        DrawButton(_restartButton, _isRestartHovered, "RESTART");
        DrawButton(_menuButton, _isMenuHovered, "MAIN MENU");
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

    private void DrawFooter()
    {
        string footer = "Enter to restart | ESC to return to menu";
        Vector2 footerSize = Font.MeasureString(footer);
        Vector2 footerPosition = new Vector2(
            (SnakeGame.WindowWidth - footerSize.X) / 2,
            SnakeGame.WindowHeight - 40);
        SpriteBatch.DrawString(Font, footer, footerPosition, ColorPalette.TextDim);
    }
}

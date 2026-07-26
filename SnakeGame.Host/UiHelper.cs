using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SnakeGame.Host;

public static class UiHelper
{
    public static void DrawRoundedRectangle(SpriteBatch spriteBatch, Rectangle rect, int cornerRadius, Color color)
    {
        int w = rect.Width;
        int h = rect.Height;
        int r = cornerRadius;

        spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, color), new Rectangle(rect.X + r, rect.Y, w - r * 2, r), Color.White);
        spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, color), new Rectangle(rect.X + r, rect.Y + h - r, w - r * 2, r), Color.White);
        spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, color), new Rectangle(rect.X, rect.Y + r, r, h - r * 2), Color.White);
        spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, color), new Rectangle(rect.X + w - r, rect.Y + r, r, h - r * 2), Color.White);

        spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, color), new Rectangle(rect.X, rect.Y, r, r), Color.White);
        spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, color), new Rectangle(rect.X + w - r, rect.Y, r, r), Color.White);
        spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, color), new Rectangle(rect.X, rect.Y + h - r, r, r), Color.White);
        spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, color), new Rectangle(rect.X + w - r, rect.Y + h - r, r, r), Color.White);
    }

    public static void DrawRoundedRectangleBorder(SpriteBatch spriteBatch, Rectangle rect, int cornerRadius, Color borderColor, int borderWidth = 2)
    {
        int w = rect.Width;
        int h = rect.Height;
        int r = cornerRadius;
        int bw = borderWidth;

        spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, borderColor), new Rectangle(rect.X + r, rect.Y, w - r * 2, bw), Color.White);
        spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, borderColor), new Rectangle(rect.X + r, rect.Y + h - bw, w - r * 2, bw), Color.White);
        spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, borderColor), new Rectangle(rect.X, rect.Y + r, bw, h - r * 2), Color.White);
        spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, borderColor), new Rectangle(rect.X + w - bw, rect.Y + r, bw, h - r * 2), Color.White);

        for (int i = 0; i < bw; i++)
        {
            spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, borderColor), new Rectangle(rect.X + i, rect.Y + i, r - i, bw), Color.White);
            spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, borderColor), new Rectangle(rect.X + w - r - i, rect.Y + i, r - i, bw), Color.White);
            spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, borderColor), new Rectangle(rect.X + i, rect.Y + h - bw - i, r - i, bw), Color.White);
            spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, borderColor), new Rectangle(rect.X + w - r - i, rect.Y + h - bw - i, r - i, bw), Color.White);

            spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, borderColor), new Rectangle(rect.X + i, rect.Y + i, bw, r - i), Color.White);
            spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, borderColor), new Rectangle(rect.X + w - bw - i, rect.Y + i, bw, r - i), Color.White);
            spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, borderColor), new Rectangle(rect.X + i, rect.Y + h - r - i, bw, r - i), Color.White);
            spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, borderColor), new Rectangle(rect.X + w - bw - i, rect.Y + h - r - i, bw, r - i), Color.White);
        }
    }

    public static void DrawGradient(SpriteBatch spriteBatch, Rectangle rect, Color startColor, Color endColor, bool vertical = true)
    {
        int steps = rect.Height;
        if (!vertical) steps = rect.Width;

        for (int i = 0; i < steps; i++)
        {
            float t = (float)i / steps;
            Color color = Color.Lerp(startColor, endColor, t);
            
            if (vertical)
            {
                spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, color), 
                    new Rectangle(rect.X, rect.Y + i, rect.Width, 1), Color.White);
            }
            else
            {
                spriteBatch.Draw(CreateColorTexture(spriteBatch.GraphicsDevice, color), 
                    new Rectangle(rect.X + i, rect.Y, 1, rect.Height), Color.White);
            }
        }
    }

    public static void DrawTextShadow(SpriteBatch spriteBatch, SpriteFont font, string text, Vector2 position, Color textColor, Color shadowColor, int offset = 2)
    {
        spriteBatch.DrawString(font, text, position + new Vector2(offset, offset), shadowColor);
        spriteBatch.DrawString(font, text, position, textColor);
    }

    public static void DrawCenteredText(SpriteBatch spriteBatch, SpriteFont font, string text, Rectangle rect, Color color)
    {
        Vector2 textSize = font.MeasureString(text);
        Vector2 position = new Vector2(
            rect.X + (rect.Width - textSize.X) / 2,
            rect.Y + (rect.Height - textSize.Y) / 2);
        spriteBatch.DrawString(font, text, position, color);
    }

    public static void DrawCenteredTextShadow(SpriteBatch spriteBatch, SpriteFont font, string text, Rectangle rect, Color textColor, Color shadowColor)
    {
        Vector2 textSize = font.MeasureString(text);
        Vector2 position = new Vector2(
            rect.X + (rect.Width - textSize.X) / 2,
            rect.Y + (rect.Height - textSize.Y) / 2);
        DrawTextShadow(spriteBatch, font, text, position, textColor, shadowColor);
    }

    public static Texture2D CreateColorTexture(GraphicsDevice graphicsDevice, Color color)
    {
        Texture2D texture = new Texture2D(graphicsDevice, 1, 1);
        texture.SetData(new[] { color });
        return texture;
    }

    public static Texture2D CreateGradientTexture(GraphicsDevice graphicsDevice, int width, int height, Color startColor, Color endColor, bool vertical = true)
    {
        Texture2D texture = new Texture2D(graphicsDevice, width, height);
        Color[] colors = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float t = vertical ? (float)y / height : (float)x / width;
                colors[y * width + x] = Color.Lerp(startColor, endColor, t);
            }
        }

        texture.SetData(colors);
        return texture;
    }
}

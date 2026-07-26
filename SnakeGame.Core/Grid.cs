namespace SnakeGame.Core;

public class Grid
{
    public int Width { get; }
    public int Height { get; }

    public Grid(int width, int height)
    {
        Width = width;
        Height = height;
    }

    public bool IsInside(Position position)
    {
        return position.X >= 0 && position.X < Width &&
               position.Y >= 0 && position.Y < Height;
    }

    public Position RandomPosition()
    {
        var random = new Random();
        return new Position(random.Next(Width), random.Next(Height));
    }

    public Position RandomPositionExcept(IEnumerable<Position> excludedPositions)
    {
        var excluded = new HashSet<Position>(excludedPositions);
        var availablePositions = new List<Position>();

        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                var pos = new Position(x, y);
                if (!excluded.Contains(pos))
                {
                    availablePositions.Add(pos);
                }
            }
        }

        if (availablePositions.Count == 0)
            return new Position(0, 0);

        var random = new Random();
        return availablePositions[random.Next(availablePositions.Count)];
    }
}

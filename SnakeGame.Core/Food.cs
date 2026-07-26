namespace SnakeGame.Core;

public class Food
{
    public Position Position { get; private set; }

    public Food(Position position)
    {
        Position = position;
    }

    public void Spawn(Position position)
    {
        Position = position;
    }
}

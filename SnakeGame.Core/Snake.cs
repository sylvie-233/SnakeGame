namespace SnakeGame.Core;

public class Snake
{
    private readonly List<Position> _segments;
    private Direction _direction;

    public IReadOnlyList<Position> Segments => _segments;
    public Direction Direction => _direction;

    public Snake(int initialLength = 3, int startX = 10, int startY = 10)
    {
        _segments = new List<Position>();
        for (int i = 0; i < initialLength; i++)
        {
            _segments.Add(new Position(startX - i, startY));
        }
        _direction = Direction.Right;
    }

    public void SetDirection(Direction newDirection)
    {
        if ((_direction == Direction.Up && newDirection == Direction.Down) ||
            (_direction == Direction.Down && newDirection == Direction.Up) ||
            (_direction == Direction.Left && newDirection == Direction.Right) ||
            (_direction == Direction.Right && newDirection == Direction.Left))
        {
            return;
        }
        _direction = newDirection;
    }

    public void Move()
    {
        Position head = _segments[0];
        Position newHead = _direction switch
        {
            Direction.Up => new Position(head.X, head.Y - 1),
            Direction.Down => new Position(head.X, head.Y + 1),
            Direction.Left => new Position(head.X - 1, head.Y),
            Direction.Right => new Position(head.X + 1, head.Y),
            _ => head
        };
        _segments.Insert(0, newHead);
        _segments.RemoveAt(_segments.Count - 1);
    }

    public void Grow()
    {
        Position tail = _segments[^1];
        _segments.Add(tail);
    }

    public bool CollidesWith(Position position) => _segments.Any(s => s == position);

    public bool CollidesWithSelf() => _segments.Skip(1).Any(s => s == _segments[0]);

    public Position HeadPosition => _segments[0];

    public void Reset(int startX, int startY, int initialLength = 3)
    {
        _segments.Clear();
        for (int i = 0; i < initialLength; i++)
        {
            _segments.Add(new Position(startX - i, startY));
        }
        _direction = Direction.Right;
    }
}

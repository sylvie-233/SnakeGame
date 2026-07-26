namespace SnakeGame.Core;

public class GameEngine
{
    public Snake Snake { get; }
    public Food Food { get; }
    public Grid Grid { get; }
    public GameState State { get; private set; }
    public int Score { get; private set; }

    public event Action? ScoreChanged;
    public event Action? GameOver;

    public GameEngine(int gridWidth = 20, int gridHeight = 20)
    {
        Grid = new Grid(gridWidth, gridHeight);
        Snake = new Snake(3, gridWidth / 2, gridHeight / 2);
        Food = new Food(Grid.RandomPositionExcept(Snake.Segments));
        State = GameState.Playing;
        Score = 0;
    }

    public void Update()
    {
        if (State != GameState.Playing)
            return;

        Snake.Move();

        if (!Grid.IsInside(Snake.HeadPosition))
        {
            GameOverGame();
            return;
        }

        if (Snake.CollidesWithSelf())
        {
            GameOverGame();
            return;
        }

        if (Snake.HeadPosition == Food.Position)
        {
            Snake.Grow();
            Score++;
            ScoreChanged?.Invoke();
            Food.Spawn(Grid.RandomPositionExcept(Snake.Segments));
        }
    }

    public void ChangeDirection(Direction direction)
    {
        Snake.SetDirection(direction);
    }

    public void TogglePause()
    {
        if (State == GameState.Playing)
            State = GameState.Paused;
        else if (State == GameState.Paused)
            State = GameState.Playing;
    }

    public void Restart()
    {
        Snake.Reset(Grid.Width / 2, Grid.Height / 2, 3);
        State = GameState.Playing;
        Score = 0;
        ScoreChanged?.Invoke();
        Food.Spawn(Grid.RandomPositionExcept(Snake.Segments));
    }

    private void GameOverGame()
    {
        State = GameState.GameOver;
        GameOver?.Invoke();
    }
}

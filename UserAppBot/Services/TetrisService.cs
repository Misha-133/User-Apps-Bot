using System.Collections.Concurrent;
using System.Text;

namespace UserAppBot.Services;

public enum TileType
{
    Empty = 0,
    Blue = 1,
    Green = 2,
    Red = 3,
    Yellow = 4
}

public enum FigureType
{
    Straight = 1,
    S = 2,
    InverseS = 3,
    Cube = 4,
    Bent = 5,
    InverseBent = 6
}

public enum Movement
{
    Down = 1,
    Left = 2,
    Right = 3,
    Rotate = 4
}

public class Figure
{
    public int X;
    public int Y;

    public int Width;
    public int Height;

    public TileType[,] Tiles;

    public Figure(FigureType type)
    {
        switch (type)
        {
            case FigureType.Cube:
            {
                Width = 2;
                Height = 2;
                Tiles = new TileType[,] {
                    { TileType.Red, TileType.Red },
                    { TileType.Red, TileType.Red }
                };
                break;
            }

            case FigureType.Straight:
            {
                Width = 4;
                Height = 4;
                Tiles = new TileType[,]
                {
                    { TileType.Green, TileType.Empty, TileType.Empty, TileType.Empty },
                    { TileType.Green, TileType.Empty, TileType.Empty, TileType.Empty },
                    { TileType.Green, TileType.Empty, TileType.Empty, TileType.Empty },
                    { TileType.Green, TileType.Empty, TileType.Empty, TileType.Empty }
                };
                break;
            }

            case FigureType.S:
            {
                Width = 3;
                Height = 3;
                Tiles = new TileType[,] {
                    { TileType.Empty, TileType.Blue, TileType.Blue },
                    { TileType.Blue, TileType.Blue, TileType.Empty },
                    { TileType.Empty, TileType.Empty, TileType.Empty }
                };
                break;
            }

            case FigureType.InverseS:
            {
                Width = 3;
                Height = 3;
                Tiles = new TileType[,] {
                    { TileType.Yellow, TileType.Yellow, TileType.Empty},
                    { TileType.Empty, TileType.Yellow, TileType.Yellow },
                    { TileType.Empty, TileType.Empty, TileType.Empty },
                };
                break;
            }

            case FigureType.Bent:
            {
                Width = 3;
                Height = 3;
                Tiles = new TileType[,] {
                    { TileType.Yellow, TileType.Yellow, TileType.Empty},
                    { TileType.Yellow, TileType.Empty, TileType.Empty },
                    { TileType.Yellow, TileType.Empty, TileType.Empty },
                };
                break;
            }

            case FigureType.InverseBent:
            {
                Width = 3;
                Height = 3;
                Tiles = new TileType[,] {
                    { TileType.Red, TileType.Empty, TileType.Empty},
                    { TileType.Red, TileType.Empty, TileType.Empty },
                    { TileType.Red, TileType.Red, TileType.Empty },
                };
                break;
            }


            default:
                throw new ArgumentOutOfRangeException(nameof(type));
        }
    }

    public bool CanMove(Movement movement, TileType[,] field)
    {
        switch (movement)
        {
            case Movement.Down:
            {
                for (var y = Tiles.GetLength(1) - 1; y >= 0; y--)
                {
                    for (var x = 0; x < Tiles.GetLength(0); x++)
                    {
                        var checkX = X + x;
                        var checkY = Y + y + 1;

                        if (Tiles[x, y] is not TileType.Empty && checkY >= field.GetLength(1))
                            return false;

                        if (checkX < 0 || checkY < 0 || checkX >= field.GetLength(0) || checkY >= field.GetLength(1))
                            continue;

                        if (Tiles[x, y] is not TileType.Empty && field[checkX, checkY] is not TileType.Empty)
                            return false;
                    }
                }
                return true;
            }

            case Movement.Left:
            {
                for (var y = Tiles.GetLength(1) - 1; y >= 0; y--)
                {
                    for (var x = 0; x < Tiles.GetLength(0); x++)
                    {
                        var checkX = X + x - 1;
                        var checkY = Y + y;

                        if (Tiles[x, y] is not TileType.Empty && checkX < 0)
                            return false;

                        if (checkX < 0 || checkY < 0 || checkX >= field.GetLength(0) || checkY >= field.GetLength(1))
                            continue;

                        if (Tiles[x, y] is not TileType.Empty && field[checkX, checkY] is not TileType.Empty)
                            return false;
                    }
                }
                return true;
            }

            case Movement.Right:
            {
                for (var y = Tiles.GetLength(1) - 1; y >= 0; y--)
                {
                    for (var x = 0; x < Tiles.GetLength(0); x++)
                    {
                        var checkX = X + x + 1;
                        var checkY = Y + y;

                        if (Tiles[x, y] is not TileType.Empty && checkX >= field.GetLength(0))
                            return false;

                        if (checkX < 0 || checkY < 0 || checkX >= field.GetLength(0) || checkY >= field.GetLength(1))
                            continue;

                        if (Tiles[x, y] is not TileType.Empty && field[checkX, checkY] is not TileType.Empty)
                            return false;
                    }
                }
                return true;
            }

            case Movement.Rotate:
            {
                for (var y = Tiles.GetLength(1) - 1; y >= 0; y--)
                {
                    for (var x = 0; x < Tiles.GetLength(0); x++)
                    {
                        var checkY = X + x;
                        var checkX = Y + y;

                        if (checkX < 0 || checkY < 0 || checkX >= field.GetLength(0) || checkY >= field.GetLength(1))
                            continue;

                        if (Tiles[x, y] is not TileType.Empty && field[checkX, checkY] is not TileType.Empty)
                            return false;
                    }
                }
                return true;
            }
        }

        return false;
    }

    public void Move(Movement movement)
    {
        switch (movement)
        {
            case Movement.Down:
            {
                Y++;

                break;
            }

            case Movement.Left:
            {
                X--;

                break;
            }

            case Movement.Right:
            {
                X++;
                break;
            }

            case Movement.Rotate:
            {
                var oldTiles = (TileType[,])Tiles.Clone();

                for (var x = 0; x < oldTiles.GetLength(0); x++)
                    for (var y = 0; y < oldTiles.GetLength(1); y++)
                        Tiles[x, y] = oldTiles[y, oldTiles.GetLength(0) - x - 1];

                var afterFigure = false;
                for (var moveFromX = Tiles.GetLength(0) - 1; moveFromX >= 0; moveFromX--)
                {
                    var empty = true;
                    for (var moveFromY = 0; moveFromY < oldTiles.GetLength(1); moveFromY++)
                        if (Tiles[moveFromX, moveFromY] is not TileType.Empty)
                        {
                            empty = false;
                            afterFigure = true;
                        }

                    if (empty && afterFigure)
                        ShiftLeft();
                }

                afterFigure = false;
                for (var moveFromY = Tiles.GetLength(1) - 1; moveFromY >= 0; moveFromY--)
                {
                    var empty = true;
                    for (var moveFromX = 0; moveFromX < oldTiles.GetLength(1); moveFromX++)
                        if (Tiles[moveFromX, moveFromY] is not TileType.Empty)
                        {
                            empty = false;
                            afterFigure = true;
                        }

                    if (empty && afterFigure)
                        ShiftUp();
                }

                break;
            }
        }
    }
    private void ShiftUp()
    {
        for (var x = 0; x < Tiles.GetLength(0); x++)
            for (var y = 0; y < Tiles.GetLength(1) - 1; y++)
            {
                Tiles[x, y] = Tiles[x, y + 1];
                Tiles[x, y + 1] = TileType.Empty;
            }
    }

    private void ShiftLeft()
    {
        for (var x = 0; x < Tiles.GetLength(0) - 1; x++)
            for (var y = 0; y < Tiles.GetLength(1); y++)
            {
                Tiles[x, y] = Tiles[x + 1, y];
                Tiles[x + 1, y] = TileType.Empty;
            }
    }

    public void Draw(TileType[,] field)
    {
        for (var x = 0; x < Tiles.GetLength(0); x++)
            for (var y = 0; y < Tiles.GetLength(0); y++)
                if (Tiles[x, y] is not TileType.Empty)
                    field[X + x, Y + y] = Tiles[x, y];
    }

    public static Figure CreateRandom()
        => new((FigureType)Random.Shared.Next(1, 6));
}

public class TetrisGame
{
    public const int TickDelayMs = 1000;
    public const int Width = 10;
    public const int Height = 14;

    public ulong UserId { get; }

    public Guid Id { get; }

    public TileType[,] Tiles { get; } = new TileType[Width, Height];

    public int Points { get; private set; }

    private IDiscordInteraction _interaction;
    private Task _autoUpdateTask;
    private Figure _currentFigure;
    private bool _gameOver = false;
    private bool _skipAutoUpdate = false;

    public TetrisGame(IDiscordInteraction interaction)
    {
        _interaction = interaction;
        Id = Guid.CreateVersion7();
        UserId = interaction.User.Id;

        _currentFigure = Figure.CreateRandom();
        _currentFigure.X = Width / 2 - _currentFigure.Width / 2;

        _autoUpdateTask = Task.Run(async () =>
        {
            var timer = new PeriodicTimer(TimeSpan.FromMilliseconds(TickDelayMs));
            while (await timer.WaitForNextTickAsync() && !_gameOver)
            {
                if (_skipAutoUpdate)
                {
                    _skipAutoUpdate = false;
                    continue;
                }

                TickGame();

                await interaction.ModifyOriginalResponseAsync(x => x.Components = GetField().Build());
            }
        });
    }

    public void TickGame()
    {
        for (var y = Height - 1; y > 0; y--)
        {
            var rowFull = true;
            for (var x = 0; x < Width; x++)
                if (Tiles[x, y] is TileType.Empty)
                    rowFull = false;

            if (rowFull)
            {
                ShiftAllDown();
                Points += 100;

                y++;
            }
        }

        if (_currentFigure.CanMove(Movement.Down, Tiles))
        {
            _currentFigure.Move(Movement.Down);
        }
        else
        {
            _currentFigure.Draw(Tiles);
            _currentFigure = Figure.CreateRandom();
            _currentFigure.X = Width / 2 - _currentFigure.Width / 2;
            if (!_currentFigure.CanMove(Movement.Down, Tiles))
                _gameOver = true;
        }
    }

    private void ShiftAllDown()
    {
        for (var y = Height - 1; y > 0; y--)
        {
            for (var x = 0; x < Width; x++)
            {
                Tiles[x, y] = Tiles[x, y - 1];
                Tiles[x, y - 1] = TileType.Empty;
            }
        }
    }

    private string GetEmojiForTile(TileType tile)
        => tile switch
        {
            TileType.Empty => "<:zTile:889102325141090346>",
            TileType.Yellow => "<:yTile:889099534939066368>",
            TileType.Red => "<:rTile:889099534876151809>",
            TileType.Green => "<:gTile:889099534863593492>",
            TileType.Blue => "<:bTile:889099534972616764>",
            _ => "L"
        };

    public ComponentBuilderV2 GetField()
    {
        var fieldBuilder = new StringBuilder();
        for (var y = 0; y < Height; y++)
        {
            for (var x = 0; x < Width; x++)
            {
                if (x >= _currentFigure.X && x < _currentFigure.X + _currentFigure.Width &&
                    y >= _currentFigure.Y && y < _currentFigure.Y + _currentFigure.Height)
                {
                    if (Tiles[x, y] is not TileType.Empty)
                        fieldBuilder.Append(GetEmojiForTile(Tiles[x, y]));
                    else
                        fieldBuilder.Append(GetEmojiForTile(_currentFigure.Tiles[x - _currentFigure.X, y - _currentFigure.Y]));
                }
                else
                    fieldBuilder.Append(GetEmojiForTile(Tiles[x, y]));
            }

            fieldBuilder.AppendLine();
        }

        var builder = new ComponentBuilderV2()
            .WithContainer(new ContainerBuilder()
                .WithTextDisplay($"""
                                 -# Player: <@{UserId}>
                                 ## Tetris
                                 """)
                .WithSeparator()
                .WithTextDisplay(fieldBuilder.ToString())
                .WithSeparator()
                .WithTextDisplay($"Your points: `{Points}`{(_gameOver ? " | Game OVER!" : string.Empty)}")
                .WithActionRow([
                    new ButtonBuilder(" ", $"tetris-stop-{Id}", ButtonStyle.Danger, emote: Emoji.Parse(":octagonal_sign:"), isDisabled: _gameOver),
                    new ButtonBuilder(" ", $"tetris-rotate-{Id}", ButtonStyle.Primary, emote: Emoji.Parse(":arrows_counterclockwise:"), isDisabled: !_currentFigure.CanMove(Movement.Rotate, Tiles) || _gameOver),
                    ])
                .WithActionRow([
                    new ButtonBuilder(" ", $"tetris-move-left-{Id}", ButtonStyle.Success, emote: Emoji.Parse(":arrow_backward:"), isDisabled: !_currentFigure.CanMove(Movement.Left, Tiles) || _gameOver),
                    new ButtonBuilder(" ", $"tetris-down-{Id}", ButtonStyle.Primary, emote: Emoji.Parse(":arrow_double_down:"), isDisabled: !_currentFigure.CanMove(Movement.Down, Tiles) || _gameOver),
                    new ButtonBuilder(" ", $"tetris-move-right-{Id}", ButtonStyle.Success, emote: Emoji.Parse(":arrow_forward:"), isDisabled: !_currentFigure.CanMove(Movement.Right, Tiles) || _gameOver),
                ]));

        return builder;
    }

    public void MoveFigure(Movement movement)
    {
        _currentFigure.Move(movement);
    }

    public bool CanMoveFigure(Movement movement)
    {
        return _currentFigure.CanMove(movement, Tiles);
    }

    public async Task UpdateInteraction(IComponentInteraction newInteraction)
    {
        _skipAutoUpdate = true;
        _interaction = newInteraction;

        await newInteraction.UpdateAsync(x => x.Components = GetField().Build());
    }

    public void EndGame()
    {
        _gameOver = true;
    }
}

public class TetrisService
{
    public ConcurrentDictionary<Guid, TetrisGame> Games = new();

    public ComponentBuilderV2 StartGame(IDiscordInteraction interaction)
    {
        var game = new TetrisGame(interaction);

        Games.TryAdd(game.Id, game);

        return game.GetField();
    }

    public TetrisGame? GetGame(Guid gameId)
        => Games.GetValueOrDefault(gameId);
}

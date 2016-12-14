using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SampleWebBotClient.Models.TankBlaster;

namespace SampleWebBotClient.Helpers
{
    public class TankBlasterComplexAiService
    {
        private BotArenaInfo _arenaInfo;

        private readonly List<MoveDirection> _allMoveDirections = Enum.GetValues(typeof(MoveDirection)).Cast<MoveDirection>().ToList();

        private static readonly Random Rand = new Random(DateTime.Now.Millisecond);

        public TankBlasterComplexAiService()
        {
        }

        public BotMove CalculateNextMove(BotArenaInfo arenaInfo)
        {
            _arenaInfo = arenaInfo;
            var result = new BotMove();


            if (_arenaInfo.MissileAvailableIn == 0)
            {
                var fireDirections = _allMoveDirections;

                if (fireDirections.Any())
                {
                    result.Action = BotAction.FireMissile;
                    result.FireDirection = fireDirections.ElementAt(Rand.Next(fireDirections.Count()));
                }
            }

            if (IsInDangerZone(_arenaInfo.BotLocation))
            {
                result.Action = BotAction.None;
            }

            var directions =
                _allMoveDirections.Where(
                    direction => IsLocationValid(AddDirectionMove(_arenaInfo.BotLocation, direction))).ToList();

            if (!directions.Any())
            {
                result.Direction = null;
                result.Action = BotAction.None;
                return result;
            }

            var safeDirections =
                directions.Where(d => !IsInDangerZone(AddDirectionMove(_arenaInfo.BotLocation, d))).ToList();
            result.Direction = safeDirections.Any()
                ? safeDirections[Rand.Next(safeDirections.Count)]
                : (MoveDirection?) null;

            if (_arenaInfo.MissileAvailableIn != 0)
            {
                result.Action = BotAction.None;
                return result;
            }

            var validCombos = new List<ValidMoveWithFire>();

            foreach (var direction in safeDirections)
            {
                var nextPosition = AddDirectionMove(_arenaInfo.BotLocation, direction);
                var validFireDirections =
                    _allMoveDirections.Where(d =>
                    {
                        var tempLocation = nextPosition;
                        for (int i = 0; i < CurrentMissileBlastRadius + 1; i++)
                        {
                            tempLocation = AddDirectionMove(tempLocation, direction);
                            if (!IsLocationValid(tempLocation))
                            {
                                return false;
                            }
                        }
                        return true;
                    }).ToList();
                var possibleCombos =
                    validFireDirections.Select(
                        f => new ValidMoveWithFire() {MoveDirection = direction, FireDirection = f});
                validCombos.AddRange(possibleCombos);
                Console.WriteLine(possibleCombos.Aggregate(string.Empty, (acc, el) => acc + " " + el.MoveDirection + " " + el.FireDirection ));
            }

            if (validCombos.Any())
            {
                var choosenCombo = validCombos[Rand.Next(validCombos.Count)];
                result.Direction = choosenCombo.MoveDirection;
                result.FireDirection = choosenCombo.FireDirection;
                result.Action = BotAction.FireMissile;
            }
            else
            {
                result.Action = BotAction.None;
            }

            return result;
        }

        private int CurrentMissileBlastRadius
        {
            get
            {
                return _arenaInfo.GameConfig.MissileBlastRadius +
                       (_arenaInfo.GameConfig.RoundsBeforeIncreasingBlastRadius == 0 ? 0 : (_arenaInfo.RoundNumber / _arenaInfo.GameConfig.RoundsBeforeIncreasingBlastRadius));
            }
        }

        private int CurrentBombBlastRadius
        {
            get
            {
                return _arenaInfo.GameConfig.BombBlastRadius +
                       (_arenaInfo.GameConfig.RoundsBeforeIncreasingBlastRadius == 0 ? 0 : (_arenaInfo.RoundNumber / _arenaInfo.GameConfig.RoundsBeforeIncreasingBlastRadius));
            }
        }

        private bool IsInDangerZone(Point location)
        {
            return !IsLocationValid(location) || _arenaInfo.Bombs.SelectMany(bomb => GetDangerZone(bomb.Location, CurrentBombBlastRadius)).Contains(location) ||
                   _arenaInfo.Missiles.SelectMany(missile => GetDangerZone(missile.Location, CurrentMissileBlastRadius)).Contains(location);
        }

        private List<Point> GetDangerZone(Point centerLocation, int explosionRadius)
        {
            var result = GetSurroundingPoints(centerLocation, explosionRadius).ToList();
            result.Add(centerLocation);

            return result;
        }

        private IEnumerable<Point> GetSurroundingPoints(Point centerLocation, int radius)
        {
            for (int i = 1; i <= radius; i++)
            {
                var locations = new[]
                {
                    new Point(centerLocation.X, centerLocation.Y + i),
                    new Point(centerLocation.X, centerLocation.Y - i),
                    new Point(centerLocation.X + i, centerLocation.Y),
                    new Point(centerLocation.X - i, centerLocation.Y)
                };

                foreach (var point in locations.Where(IsLocationValid))
                {
                    yield return point;
                }
            }
        }

        private bool IsLocationValid(Point location)
        {
            return location.X >= 0
                   && location.X < _arenaInfo.Board.GetLength(0)
                   && location.Y >= 0
                   && location.Y < _arenaInfo.Board.GetLength(1)
                   && _arenaInfo.Board[location.X, location.Y] == BoardTile.Empty;
        }

        private Point AddDirectionMove(Point location, MoveDirection direction)
        {
            var result = new Point(location.X, location.Y);
            switch (direction)
            {
                case MoveDirection.Up:
                    result.Y--;
                    break;
                case MoveDirection.Down:
                    result.Y++;
                    break;
                case MoveDirection.Right:
                    result.X++;
                    break;
                case MoveDirection.Left:
                    result.X--;
                    break;
            }

            return result;
        }
    }

    struct ValidMoveWithFire
    {
        public MoveDirection MoveDirection { get; set; }
        public MoveDirection FireDirection { get; set; }
        
    }
}
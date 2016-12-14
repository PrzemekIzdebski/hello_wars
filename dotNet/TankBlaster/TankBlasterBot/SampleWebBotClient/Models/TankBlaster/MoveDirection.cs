using System;

namespace SampleWebBotClient.Models.TankBlaster
{
    public enum MoveDirection
    {
        Up,
        Down,
        Right,
        Left
    }

    public static class MoveDirectionExtensions
    {
        public static bool IsOpposite(this MoveDirection direction, MoveDirection other)
        {
            switch (direction)
            {
                case MoveDirection.Down:
                    return other == MoveDirection.Up;
                case MoveDirection.Up:
                    return other == MoveDirection.Down;
                case MoveDirection.Left:
                    return other == MoveDirection.Right;
                case MoveDirection.Right:
                    return other == MoveDirection.Left;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}

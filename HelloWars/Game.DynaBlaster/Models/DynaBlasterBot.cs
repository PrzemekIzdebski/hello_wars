﻿using System;
using System.Drawing;
using System.Windows.Media;
using Common.Interfaces;
using Common.Models;

namespace Game.DynaBlaster.Models
{
    public class DynaBlasterBot : BotClientBase<BotArenaInfo, BotMove>
    {
        public DynaBlasterBot(ICompetitor competitor) : base(competitor)
        {
            LastMissileFiredRound = Int32.MinValue;
        }

        public ImageSource Image { get; set; }
        public Point Location { get; set; }
        public bool IsDead { get; set; }
        public int LastMissileFiredRound { get; set; }
        public MoveDirection LastDirection { get; set; }
    }
}
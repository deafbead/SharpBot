using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.Commands.Dice;
using TelegramBot.Bot.Replies;
using TelegramBot.Util;

namespace TelegramBot.Bot.Commands.Impl.Dice
{
    class DiceMidGameState : ICommandState
    {
        private readonly DiceGameCommand _command;

        private string Lie = "lie";
        private string Truth = "truth";
        private string Raise = "raise";

        private DicePlayer CurrentPlayer { get; set; }

        public DiceMidGameState(DiceGameCommand command)
        {
            _command = command;
            ResetDices();

        }

        private void ResetDices()
        {
            foreach (var player in _command.Players)
            {
                player.Dices.Clear();
                for (int i = 0; i < _command.InitialDiceCount; i++)
                {
                    player.Dices.Add(new Games.Dices.Dice());
                }
            }
        }

        public IEnumerable<IReply> BeginGame()
        {
            _command.CurrentBet = null;
            foreach (var player in _command.Players)
            {
                player.Roll();
            }
            CurrentPlayer = _command.Players.PickRandom();

            var results = new List<IReply>();
            foreach (var player in _command.Players)
            {
                results.Add(player.Message("Вам выпало: " + string.Join(" ", player.Dices.Select(t => t.Value))));
            }
            results.AddRange(_command.ReplyAll($"Первым ходит игрок: {CurrentPlayer.Treat}"));

            return results;
        }

        public bool ShouldInvoke(TelegramMessageEventArgs input)
        {
            return input.From.Equals(CurrentPlayer.User) && (input.MessageEquals(Lie, Truth) || input.MessageMatches(@"\d +\d"));
        }

        public Task<IEnumerable<IReply>> Invoke(TelegramMessageEventArgs input)
        {
            if (input.MessageEquals(Lie))
            {
                return ProcessLie().AsTaskResult();
            }

            if (input.MessageEquals(Truth))
            {
                return ProcessTruth().AsTaskResult();
            }

            return ProcessRaise(input).AsTaskResult();
        }

        private IEnumerable<IReply> ProcessLie()
        {
            DicePlayer winner = _command.CurrentBet.IsAtLeast(_command.Players) ? _command.CurrentBet.Player : CurrentPlayer;
            return EndGame(winner);
        }

        private IEnumerable<IReply> ProcessTruth()
        {
            DicePlayer winner = _command.CurrentBet.IsExact(_command.Players) ? CurrentPlayer : _command.CurrentBet.Player;
            return EndGame(winner);
        }

        private IEnumerable<IReply> EndGame(DicePlayer winner)
        {
            var results = new List<IReply>();
            results.AddRange(_command.ReplyAll("Кубики игроков:\n\n" +
                                               string.Join("\n",
                                                   _command.Players.Select(
                                                       p =>
                                                           $"{p.Treat}: {string.Join(" ", p.Dices.Select(d => d.Value))}"))));
            results.AddRange(_command.ReplyAll($"Побеждает {winner.Treat}"));
            results.AddRange(PunishLosers(winner));

            if (_command.Players.Count == 1)
            {
                results.AddRange(_command.ReplyAll($"Игра окончена! Победил: {winner.Treat}"));
                _command.CurrentState = new DiceBeginGameState(_command);
                return results;
            }

            results.AddRange(BeginGame());
            return results;
        }

        private IEnumerable<IReply> PunishLosers(DicePlayer winner)
        {
            var losers = new List<DicePlayer>();
            foreach (var player in _command.Players.Where(p => p != winner))
            {
                player.Dices.RemoveAt(0);
                foreach (var r in _command.ReplyAll($"Игрок {player.Treat} теряет 1 кубик. Осталось: {player.Dices.Count}"))
                {
                    yield return r;
                }
                if (player.Dices.Count == 0)
                {
                    foreach (var r in _command.ReplyAll($"Игрок {player.Treat} лишился всех кубиков и выбывает из игры"))
                    {
                        yield return r;
                    }
                    losers.Add(player);
                }
            }

            _command.Players.RemoveAll(p => losers.Contains(p));
        }

        private IEnumerable<IReply> ProcessRaise(TelegramMessageEventArgs input)
        {
            var results = new List<IReply>();
            var split = input.Message.Text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (split.Length < 2 || !int.TryParse(split[0], out int count) || !int.TryParse(split[1], out int value)
                || count < 1 || count > 6 || value < 1 || value > 6)
            {
                results.Add(input.TextReply($"Некорректный ввод (ожидалось \"количество номинал\")"));
                return results;
            }

            if (_command.CurrentBet != null && (_command.CurrentBet.Count > count || (_command.CurrentBet.Count == count && _command.CurrentBet.Value < value)))
            {
                results.Add(input.TextReply(
                    "Вы должны поставить количество не меньше противника, причем при равных количествах ваше значение должно быть больше"));
                return results;
            }

            _command.CurrentBet = new DiceBet(CurrentPlayer, count, value);
            results.AddRange(_command.ReplyAll($"Ставка от игрока {CurrentPlayer.Treat}: {count} {value}"));
            CurrentPlayer = NextPlayer();
            return results;
        }

        private DicePlayer NextPlayer()
        {
            int index = _command.Players.IndexOf(CurrentPlayer);
            if (index == _command.Players.Count - 1)
            {
                return _command.Players[0];
            }
            return _command.Players[index + 1];
        }
    }
}

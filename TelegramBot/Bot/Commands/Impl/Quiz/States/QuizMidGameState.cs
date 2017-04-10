using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TelegramBot.Bot.Args;
using TelegramBot.Bot.Commands.Quiz.Questions;
using TelegramBot.Bot.Replies;
using TelegramBot.Util;

namespace TelegramBot.Bot.Commands.Quiz.States
{
    class QuizMidGameState : ICommandState
    {
        private readonly QuizCommand _command;
        private DateTime _questionPostedAt = DateTime.MinValue;

        public QuizMidGameState(QuizCommand command)
        {
            _command = command;
            _questions = Resources.Quiz.Questions.Split(new[] { "\n" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(q => q.Split('|'))
                .Where(q => q.Length == 2)
                .Select(q => new QuizQuestion
                {
                    Text = q[0],
                    Answer = q[1]
                })
                .ToList();
        }



        public bool ShouldInvoke(TelegramMessageEventArgs input)
        {
            return true;
        }

        private const int PointsPerCorrectAnswer = 5;

        public Task<IEnumerable<IReply>> Invoke(TelegramMessageEventArgs input)
        {
            if (input.MessageEquals("всё", "все", "конец", "завершить", "end"))
            {
                _command.CurrentState = new QuizBeginGameState(_command);
                return new TextReply("Викторина завершена").AsResult();
            }

            if (input.MessageEquals("не знаю", "хз"))
            {
                var timeSinceQuestionAsked = DateTime.Now - _questionPostedAt;
                var questionLastsAtLeast = TimeSpan.FromSeconds(15);
                if (timeSinceQuestionAsked < questionLastsAtLeast)
                {
                    return new TextReply($"Следующий вопрос будет доступен через {(questionLastsAtLeast - timeSinceQuestionAsked).ToString(@"hh\:mm\:ss")}").AsResult();
                }

                string answer = _currentQuestion.Answer;
                _currentQuestion = _questions.PickRandom();
                _questionPostedAt = DateTime.Now;
                return Task.FromResult((IEnumerable<IReply>)new IReply[]
                {
                    new TextReply($"Правильный ответ: {answer}"),
                    new TextReply($"Следующий вопрос: {_currentQuestion.Text}")
                });

            }

            if (input.MessageEquals("счет", "счёт", "score"))
            {
                int score = _command.GetScore(input.From);
                var rank = _command.GetRank(input.From);
                string output = $"Счет игрока {_command.Treat(input.From)}: {score}.";
                if (rank.NextRank != null)
                {
                    output += $" До следующего ранга: {rank.NextRank.PointsRequired - score}";
                }
                return new TextReply(output).AsResult();
            }

            if (input.MessageEquals("топ"))
            {
                int count = 10;

                string top = _command.RecordsTable.Data(QuizCommand.GameName).OrderByDescending(t => t.Value)
                    .Select((t, i) => $"{i + 1}. {_command.Treat(t.Key).StartWithUppercase()} - {t.Value}")
                    .Take(count)
                    .StringJoin("\r\n");

                return new TextReply($"Топ-{count} игроков:\r\n\r\n" + top).AsResult();
            }

            if (input.MessageEquals(_currentQuestion.Answer))
            {
                string answer = _currentQuestion.Answer;
                _currentQuestion = _questions.PickRandom();
                var originalRank = _command.GetRank(input.From);
                _command.AddPoints(input.From, PointsPerCorrectAnswer);
                var newRank = _command.GetRank(input.From);
                _questionPostedAt = DateTime.Now;

                string correctMessage = $"Правильно, {_command.Treat(input.From)} ты получаешь {PointsPerCorrectAnswer} очков";
                int? pointsToNextRank = newRank.NextRank?.PointsRequired - _command.GetScore(input.From);

                if (newRank.Name != originalRank.Name)
                {
                    correctMessage += $" и новый ранг: {newRank.Name.StartWithUppercase()}";
                }

                if (pointsToNextRank != null)
                {
                    correctMessage += $" ({pointsToNextRank} до следующего ранга)";
                }
                correctMessage += $"!  Правильный ответ: {answer}.";
                return Task.FromResult((IEnumerable<IReply>)new IReply[]
                {
                    new TextReply(correctMessage),
                    new TextReply($"Следующий вопрос: {_currentQuestion.Text}")
                });
            }

            return Task.FromResult(Enumerable.Empty<IReply>());
        }

        public Task<IEnumerable<IReply>> BeginGame()
        {
            _currentQuestion = _questions.PickRandom();
            _questionPostedAt = DateTime.Now;
            return Task.FromResult((IEnumerable<IReply>)new IReply[]
            {
                new TextReply($"Викторина начинается!"),
                new TextReply($"Первый вопрос: {_currentQuestion.Text}")
            });
        }

        private QuizQuestion _currentQuestion;
        private IList<QuizQuestion> _questions;
    }
}

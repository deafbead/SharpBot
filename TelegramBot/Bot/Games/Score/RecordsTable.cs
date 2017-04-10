using System.Collections.Generic;
using System.Linq;
using TelegramBot.API.Models;
using TelegramBot.Persistence;

namespace TelegramBot.Bot.Games.Score
{
    class RecordsTable : IRecordsTable
    {
        private readonly IRepository<UserDTO> _usersRepository;

        public RecordsTable(IRepository<UserDTO> usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public IDictionary<User, int> Data(string gameId)
        {
            var users = _usersRepository.GetAll().Where(t => t.Scores.Any(sc => sc.GameId == gameId));
            return users.ToDictionary(u => new User
            {
                Id = u.Id,
                Username = u.Username,
                FirstName = u.FirstName,
                LastName = u.LastName
            }, u => u.Scores.Single(t => t.GameId == gameId).Points);
        }

        public int GetPoints(User user, string gameId)
        {
            var persistUser = _usersRepository.Get(user.Id);
            var score = persistUser?.Scores.SingleOrDefault(t => t.GameId == gameId);
            if (score == null) return 0;
            return score.Points;
        }

        public void AddPoints(User user, int points, string gameId)
        {
            var persistUser = _usersRepository.Get(user.Id) ;

            if (persistUser == null)
            {
                persistUser = new UserDTO()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Username = user.Username,
                    Scores = new List<ScoreDTO>()
                };
                _usersRepository.Add(persistUser);
            }

            var dto = persistUser.Scores.SingleOrDefault(t => t.GameId == gameId);
            if (dto != null)
            {
                dto.Points += points;
                _usersRepository.Update(persistUser);
                return;
            }

            dto = new ScoreDTO()
            {
                GameId = gameId,
                Points = points,
                User = persistUser
            };
            persistUser.Scores.Add(dto);
            _usersRepository.Update(persistUser);
        }
    }
}

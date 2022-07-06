using Demo_ASP_MVC_Modele.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_ASP_MVC_Modele.DAL.Interfaces
{
    public interface IGameRepository : IRepository<int, GameEntity>
    {
        // Pour ajouter des méthodes pour le type d'entity (GameEntity)

        IEnumerable<GameEntity> GetByMemberId(int id);
        bool AddFavoriteGame(int memberId, int gameId);
    }
}

using Demo_ASP_MVC_Modele.DAL.Entities;
using Demo_ASP_MVC_Modele.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_ASP_MVC_Modele.DAL.Repositories
{
    public class GameRepository : IGameRepository
    {
        private IDbConnection _Connection;

        public GameRepository(IDbConnection connection)
        {
            _Connection = connection;
        }

        private void AddParameter(IDbCommand cmd, string name, object data)
        {
            IDbDataParameter param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = data ?? DBNull.Value;
            cmd.Parameters.Add(param);
        }

        #region CRUD
        public IEnumerable<GameEntity> GetAll()
        {
            throw new NotImplementedException();
        }

        public GameEntity GetById(int id)
        {
            throw new NotImplementedException();
        }

        public int Insert(GameEntity entity)
        {
            // Créer la commande
            IDbCommand cmd = _Connection.CreateCommand();

            // On défini la requete SQL
            cmd.CommandText = "INSERT INTO Game([Name], [Description], [Nb_Player_Min], [Nb_Player_Max], [Age], [Coop])" +
                " OUTPUT inserted.[Id]" +
                " VALUES (@Name, @Desc, @NbPlayerMin, @NbPlayerMax, @Age, @Coop)";

            // On ajoute les parametres SQL
            AddParameter(cmd, "@Name", entity.Name);
            AddParameter(cmd, "@Desc", entity.Description);
            AddParameter(cmd, "@NbPlayerMin", entity.NbPlayerMin);
            AddParameter(cmd, "@NbPlayerMax", entity.NbPlayerMax);
            AddParameter(cmd, "@Age", entity.Age);
            AddParameter(cmd, "@Coop", entity.IsCoop);

            _Connection.Open();
            int id = (int)cmd.ExecuteScalar();
            _Connection.Close();

            return id;
        }

        public bool Update(GameEntity entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

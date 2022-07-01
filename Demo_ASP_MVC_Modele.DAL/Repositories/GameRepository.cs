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

        protected GameEntity Convert(IDataRecord record)
        {
            return new GameEntity
            {
                Id = (int)record["Id"],
                Name = (string)record["Name"],
                Description = record["Description"] is DBNull ? null :  (string)record["Description"],
                IsCoop = (bool)record["Coop"],
                NbPlayerMin = (int)record["Nb_player_min"],
                NbPlayerMax = (int)record["Nb_player_max"],
                Age = record["Age"] is DBNull ? null : (int)record["Age"]

            };
        }

        #region CRUD
        public IEnumerable<GameEntity> GetAll()
        {
            using (IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Game";

                _Connection.Open();
                using (IDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return Convert(reader);
                    }
                }
                _Connection.Close();

            }
        }

        public GameEntity GetById(int id)
        {
            using (IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = "SELECT * FROM Game WHERE Id = @id";

                AddParameter(cmd, "@id", id);

                _Connection.Open();

                using (IDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) return Convert(reader);
                    throw new ArgumentNullException("Jeu inexistant");
                }
            }
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
            using (IDbCommand cmd = _Connection.CreateCommand())
            {
               
                cmd.CommandText = "UPDATE Game SET Name = @Name, " +
                    "Description = @Desc, " +
                    "Nb_player_min = @NbPlayerMin, " +
                    "Nb_player_max = @NbPlayerMax, " +
                    "Age = @Age, " +
                    "Coop = @Coop " +
                    "WHERE Id = @id";

                AddParameter(cmd, "@id", entity.Id);
                AddParameter(cmd, "@Name", entity.Name);
                AddParameter(cmd, "@Desc", entity.Description);
                AddParameter(cmd, "@NbPlayerMin", entity.NbPlayerMin);
                AddParameter(cmd, "@NbPlayerMax", entity.NbPlayerMax);
                AddParameter(cmd, "@Age", entity.Age);
                AddParameter(cmd, "@Coop", entity.IsCoop);

                _Connection.Open();
                return cmd.ExecuteNonQuery() >= 1;
            }
        }

        public bool Delete(int id)
        {
            using(IDbCommand cmd = _Connection.CreateCommand())
            {
                cmd.CommandText = "DELETE FROM Game WHERE Id = @id";
                AddParameter(cmd, "@id", id);

                _Connection.Open();
                return cmd.ExecuteNonQuery() == 1;

            }
        }
        #endregion
    }
}

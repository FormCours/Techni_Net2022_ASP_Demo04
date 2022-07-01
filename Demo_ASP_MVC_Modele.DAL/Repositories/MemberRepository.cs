using Demo_ASP_MVC_Modele.DAL.Entities;
using Demo_ASP_MVC_Modele.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_ASP_MVC_Modele.DAL.Repositories
{
    public class MemberRepository : RepositoryBase<int, MemberEntity>, IMemberRepository
    {
        public MemberRepository(IDbConnection connection) 
            : base(connection, "Member", "Id")
        { }
        protected override MemberEntity Convert(IDataRecord record)
        {
            return new MemberEntity
            {
                Id = (int)record["Id"],
                Pseudo = (string)record["Pseudo"],
                Email = (string)record["Email"],
                PwdHash = (string)record["Pwd_Hash"],
            };
        }

        public override int Insert(MemberEntity entity)
        {
            IDbCommand cmd = _Connection.CreateCommand();

            cmd.CommandText= "INSERT INTO [Member]([Pseudo], [Email], [Pwd_Hash])"+
                " OUTPUT inserted.[Id]" +
                " VALUES (@Pseudo, @Email, @PwdHash)";

            // On ajoute les parametres SQL
            AddParameter(cmd, "@Pseudo", entity.Pseudo);
            AddParameter(cmd, "@Email", entity.Email);
            AddParameter(cmd, "@PwdHash", entity.PwdHash);

            _Connection.Open();
            int id = (int)cmd.ExecuteScalar();
            _Connection.Close();
            return id;
        }

        public override bool Update(MemberEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}

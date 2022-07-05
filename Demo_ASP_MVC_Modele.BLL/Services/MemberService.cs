using Demo_ASP_MVC_Modele.BLL.Entities;
using Demo_ASP_MVC_Modele.BLL.Interfaces;
using Demo_ASP_MVC_Modele.BLL.Tools;
using Demo_ASP_MVC_Modele.DAL.Entities;
using Demo_ASP_MVC_Modele.DAL.Interfaces;
using Demo_ASP_MVC_Modele.DAL.Repositories;
using Isopoh.Cryptography.Argon2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_ASP_MVC_Modele.BLL.Services
{
    public class MemberService : IMemberService
    {
        IMemberRepository _MemberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _MemberRepository = memberRepository;
        }

        public MemberModel Register(MemberModel member)
        {
            if(_MemberRepository.CheckUserExists(member.Email, member.Pseudo))
            {
                throw new ArgumentException("Email ou pseudo déjà éxistant");
            }

            // Hashage du mot de passe
            string pwdHash = Argon2.Hash(member.Pwd);

            // Ajout dans la DB
            MemberEntity mEntity = member.ToDAL();
            mEntity.PwdHash = pwdHash;

            int id = _MemberRepository.Insert(mEntity);

            // Recuperation du member
            return _MemberRepository.GetById(id).ToBll();
        }

        public MemberModel Login(string pseudo, string password)
        {
            // Récuperation le Hash lier au compte
            string hash = _MemberRepository.GetHashByPseudo(pseudo);

            if(string.IsNullOrWhiteSpace(hash))
            {
                return null; // Ou Exception
            }

            // Validation du hash avec le password
            if(Argon2.Verify(hash, password))
            {
                return _MemberRepository.GetByPseudo(pseudo).ToBll();
            }
            else
            {
                throw new Exception("Mot de passe incorrect");
            }
        }
    }
}

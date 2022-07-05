using Demo_ASP_MVC_Modele.BLL.Entities;

namespace Demo_ASP_MVC_Modele.WebApp.Models.Mappers
{
    public static class MemberMapper
    {
        public static MemberModel ToBll(this MemberRegForm form)
        {
            return new MemberModel
            {
                Email = form.Email,
                Pwd = form.Pwd,
                Pseudo = form.Pseudo
            };
        }

        public static Member ToWeb(this MemberModel m)
        {
            return new Member
            {
                Id = m.Id,
                Email = m.Email,
                Pseudo = m.Pseudo
            };
        }
    }
}

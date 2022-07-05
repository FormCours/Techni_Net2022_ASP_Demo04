using Demo_ASP_MVC_Modele.WebApp.Models;
using System.Text.Json;

namespace Demo_ASP_MVC_Modele.WebApp.Infrastructure
{
    public class SessionManager
    {
        private ISession _session;

        public SessionManager(IHttpContextAccessor accessor)
        {
            _session = accessor.HttpContext.Session;
        }

        public Member CurrentUser
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_session.GetString("currentUser"))) return null;

                return JsonSerializer.Deserialize<Member>(_session.GetString("currentUser"));
            }

            set
            {
                _session.SetString("currentUser", JsonSerializer.Serialize(value));
            }
        }

        public void Logout()
        {
            CurrentUser = null;
        }


    }
}

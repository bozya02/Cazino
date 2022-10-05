using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cazino.DB
{
    public static class DataAccess
    {
        public static List<User> GetUsers()
        {
            return CazinoEntities.GetContext().Users.ToList();
        }

        public static User GetUser(string login, string password)
        {
            return GetUsers().FirstOrDefault(user => user.Login == login && user.Password == password);
        }

        public static bool RegistartionUser(string login, string password)
        {
            User user = new User
            {
                Login = login,
                Password = password,
                Points = 100
            };

            CazinoEntities.GetContext().Users.Add(user);
            return Convert.ToBoolean(CazinoEntities.GetContext().SaveChanges());
        }

        public static bool CheckLogin(string login)
        {
            return GetUsers().Where(user => user.Login == login).Count() == 0;
        }

        
    }
}

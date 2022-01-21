using System.Linq;
using Vanct.Dal.Entities;

namespace Vanct.Dal.Dao
{
    public class BaseUserDao : BaseDao<BaseUser, VanctEntities>
    {
        public BaseUser Login(string username, string password)
        {
            using (var db = new VanctEntities())
            {
                try
                {
                    return db.BaseUsers.Single(i => i.Username.Equals(username) && i.Password.Equals(password));
                }
                catch
                {
                    return null;
                }
            }
        }

        public void ChangePassword(string newPass)
        {
            using (var db = new VanctEntities())
            {
                BaseUser o = db.BaseUsers.Single(i => i.Username.Equals("admin"));
                o.Password = newPass;
                db.SaveChanges();
            }
        }
    }
}
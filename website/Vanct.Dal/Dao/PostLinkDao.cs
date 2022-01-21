using System.Collections.Generic;
using System.Linq;
using Vanct.Dal.Entities;

namespace Vanct.Dal.Dao
{
    public class PostLinkDao : BaseDao<PostLink, VanctEntities>
    {
        public IList<PostLinkType> GetHomeTypes()
        {
            using (var db = new VanctEntities())
            {
                return (from i in db.PostLinkTypes
                    where i.IsHomeShowed && i.IsActive
                    orderby i.Position descending
                    select i)
                    .ToList();
            }
        }

        public PostLinkType GetTypeById(string id)
        {
            using (var db = new VanctEntities())
            {
                return db.PostLinkTypes.SingleOrDefault(i => i.Id.Equals(id));
            }
        }

        public void UpdateType(PostLinkType type)
        {
            using (var db = new VanctEntities())
            {
                var update = db.PostLinkTypes.SingleOrDefault(i => i.Id.Equals(type.Id));
                if(update == null) return;
                update.IsHomeShowed = type.IsHomeShowed;
                update.Position = type.Position;
                db.SaveChanges();
            }
        }
    }
}
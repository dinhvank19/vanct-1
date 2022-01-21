using System;
using System.Collections.Generic;
using System.Linq;
using POS.Dal.Entities;
using POS.Dal.Enums;
using POS.Shared;

namespace POS.Dal
{
    public class RecordUser
    {
        #region Properties

        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }
        public string ValidStatus { get; set; }

        public RecordSession Session { get; set; }

        #endregion

        #region Statics

        /// <summary>
        /// Logins the specified username.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">LoginFailed</exception>
        public static RecordUser Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
                throw new Exception("MissingUsername");

            if (string.IsNullOrEmpty(password))
                throw new Exception("MissingPassword");

            using (var db = new POSEntities())
            {
                password = password.ToMd5();
                var record = db.Users.SingleOrDefault(i =>
                    i.Username.Equals(username)
                    && i.Password.Equals(password)
                    && i.ValidStatus.Equals(Enums.ValidStatus.Active.ToString()));
                if (record == null)
                    throw new Exception("LoginFailed");

                return record.Clone(new RecordUser());
            }
        }

        /// <summary>
        /// Gets the specified record identifier.
        /// </summary>
        /// <param name="recordId">The record identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">NotFoundData</exception>
        public static RecordUser Get(int recordId)
        {
            using (var db = new POSEntities())
            {
                var record = db.Users.SingleOrDefault(i => i.Id == recordId);
                if (record == null)
                    throw new Exception("NotFoundData");

                return record.Clone(new RecordUser());
            }
        }

        /// <summary>
        /// Alls this instance.
        /// </summary>
        /// <returns></returns>
        public static IList<RecordUser> All(ValidStatus valid = Enums.ValidStatus.None)
        {
            using (var db = new POSEntities())
            {
                return db.Users
                    .Where(i => valid == Enums.ValidStatus.None || i.ValidStatus.Equals(valid.ToString()))
                    .Select(i => new RecordUser
                    {
                        UserType = i.UserType,
                        Name = i.Name,
                        Id = i.Id,
                        Username = i.Username,
                        ValidStatus = i.ValidStatus
                    })
                    .ToList();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Updates this instance.
        /// </summary>
        /// <exception cref="System.Exception">NotFoundData</exception>
        public void Update()
        {
            using (var db = new POSEntities())
            {
                var record = db.Users.SingleOrDefault(i => i.Id == Id);
                if (record == null)
                    throw new Exception("NotFoundData");

                record.ValidStatus = ValidStatus;
                record.Name = Name;
                record.UserType = UserType;

                if (record.Password != Password)
                    record.Password = Password.ToMd5();

                db.SaveChanges();
            }
        }

        /// <summary>
        /// Inserts this instance.
        /// </summary>
        /// <exception cref="System.Exception">NotFoundData</exception>
        public void Insert()
        {
            using (var db = new POSEntities())
            {
                var record = new User
                {
                    Password = Password.ToMd5(),
                    UserType = UserType,
                    Name = Name,
                    Username = Username,
                    ValidStatus = ValidStatus
                };
                db.Users.Add(record);
                db.SaveChanges();

                Id = record.Id;
            }
        }

        public void ChangedPassword(string newPassword)
        {
            using (var db = new POSEntities())
            {
                var record = db.Users.SingleOrDefault(i => i.Id == Id);
                if (record == null)
                    throw new Exception("NotFoundData");

                record.Password = newPassword.ToMd5();
                db.SaveChanges();
            }
        }

        #endregion
    }
}
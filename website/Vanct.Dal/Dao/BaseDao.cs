using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Hulk.Shared.Log;

namespace Vanct.Dal.Dao
{
    public abstract class BaseDao<TBaseEntity, TBaseDb>
        where TBaseEntity : class, new()
        where TBaseDb : DbContext, new()
    {
        #region Base Functions

        /// <summary>
        ///     Counts the specified query.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public int Count(Expression<Func<TBaseEntity, bool>> query)
        {
            using (var db = new TBaseDb())
            {
                try
                {
                    return db.Set<TBaseEntity>().Count(query);
                }
                catch (Exception ex)
                {
                    LoggingFactory.GetLogger().Log(ex.ToString());
                    return 0;
                }
            }
        }

        /// <summary>
        ///     Get one record from database
        /// </summary>
        /// <param name="query">This is condition for select top 1. Example : i=>i.Id==1 or i=>i.Name.Equals("Another string") ...</param>
        /// <returns></returns>
        public TBaseEntity Get(Expression<Func<TBaseEntity, bool>> query)
        {
            using (var db = new TBaseDb())
            {
                try
                {
                    return db.Set<TBaseEntity>().SingleOrDefault(query);
                }
                catch (Exception ex)
                {
                    LoggingFactory.GetLogger().Log(ex.ToString());
                    return null;
                }
            }
        }

        /// <summary>
        ///     Get list entity from database
        /// </summary>
        /// <param name="query">This is condition for select. Example : i=>i.Id==1 or i=>i.Name.Equals("Another string") ...</param>
        /// <returns>IList</returns>
        public IList<TBaseEntity> Gets(Expression<Func<TBaseEntity, bool>> query)
        {
            using (var db = new TBaseDb())
            {
                try
                {
                    return db.Set<TBaseEntity>().Where(query).ToList();
                }
                catch (Exception ex)
                {
                    LoggingFactory.GetLogger().Log(ex.ToString());
                    return new List<TBaseEntity>();
                }
            }
        }

        /// <summary>
        ///     Getses this instance.
        /// </summary>
        /// <returns></returns>
        public IList<TBaseEntity> Gets()
        {
            using (var db = new TBaseDb())
            {
                try
                {
                    return db.Set<TBaseEntity>().Select(i => i).ToList();
                }
                catch (Exception ex)
                {
                    LoggingFactory.GetLogger().Log(ex.ToString());
                    return new List<TBaseEntity>();
                }
            }
        }

        /// <summary>
        ///     Create new one record to database
        /// </summary>
        /// <param name="o">This is entity</param>
        /// <returns></returns>
        public TBaseEntity Create(TBaseEntity o)
        {
            using (var db = new TBaseDb())
            {
                try
                {
                    db.Set<TBaseEntity>().Add(o);
                    db.SaveChanges();
                    return o;
                }
                catch (Exception ex)
                {
                    LoggingFactory.GetLogger().Log(ex.ToString());
                    return null;
                }
            }
        }


        /// <summary>
        ///     Delete record in database
        /// </summary>
        /// <param name="query">This is condition for delete. Example : i=>i.Id==1 or i=>i.Name.Equals("Another string") ...</param>
        /// <returns></returns>
        public bool Delete(Expression<Func<TBaseEntity, bool>> query)
        {
            using (var db = new TBaseDb())
            {
                try
                {
                    db.Set<TBaseEntity>().Remove(db.Set<TBaseEntity>().Single(query));
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    LoggingFactory.GetLogger().Log(ex.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        ///     Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public bool Delete(TBaseEntity entity)
        {
            using (var db = new TBaseDb())
            {
                try
                {
                    db.Set<TBaseEntity>().Remove(entity);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    LoggingFactory.GetLogger().Log(ex.ToString());
                    return false;
                }
            }
        }

        /// <summary>
        ///     Update record to database
        /// </summary>
        /// <param name="entity">This is entity</param>
        /// <param name="query">This is condition for update. Example : i=>i.Id==1 or i=>i.Name.Equals("Another string") ...</param>
        /// <returns></returns>
        public bool Edit(TBaseEntity entity, Expression<Func<TBaseEntity, bool>> query)
        {
            using (var db = new TBaseDb())
            {
                try
                {
                    TBaseEntity entityFromDb = db.Set<TBaseEntity>().Single(query);
                    if (null == entityFromDb) return false;

                    PropertyInfo[] properties = entityFromDb.GetType().GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        object propertyValue = null;
                        if (null == property.GetSetMethod()) continue;

                        PropertyInfo entityProperty = entity.GetType().GetProperty(property.Name);
                        string _namespace = property.PropertyType.Namespace;

                        if (_namespace == null ||
                            (_namespace.Equals(db.GetType().Namespace) || _namespace.Equals("System.Data"))) continue;

                        if (entityProperty.PropertyType.BaseType == Type.GetType("System.ValueType")
                            || entityProperty.PropertyType == Type.GetType("System.String"))
                            propertyValue = entity.GetType().GetProperty(property.Name).GetValue(entity, null);

                        property.SetValue(entityFromDb, propertyValue, null);
                    }
                    db.SaveChanges();

                    return true;
                }
                catch (Exception ex)
                {
                    LoggingFactory.GetLogger().Log(ex.ToString());
                    return false;
                }
            }
        }

        #endregion

        #region SqlHelper

        public void AddParameter(DbCommand command, DbType type, string name, object value)
        {
            DbParameter parameter = command.CreateParameter();
            parameter.DbType = type;
            parameter.ParameterName = name;
            parameter.Value = value;
            command.Parameters.Add(parameter);
        }

        public bool VerifyColumn(DataRow row, string columnName)
        {
            return !string.IsNullOrEmpty(columnName) && row != null && row.Table.Columns.Contains(columnName);
        }

        public string FieldString(DataRow row, string columnName)
        {
            if (!VerifyColumn(row, columnName)) return null;
            return row.IsNull(columnName) ? string.Empty : row.Field<string>(columnName);
        }

        public TimeSpan? FieldTimeSpan(DataRow row, string columnName)
        {
            if (!VerifyColumn(row, columnName)) return null;
            return row.IsNull(columnName) ? (TimeSpan?) null : row.Field<TimeSpan>(columnName);
        }

        public DateTime? FieldDateTime(DataRow row, string columnName)
        {
            if (!VerifyColumn(row, columnName)) return null;
            return row.IsNull(columnName) ? (DateTime?) null : row.Field<DateTime>(columnName);
        }

        public Int16? FieldInt16(DataRow row, string columnName)
        {
            if (!VerifyColumn(row, columnName)) return null;
            return row.IsNull(columnName) ? (Int16?) null : row.Field<Int16>(columnName);
        }

        public Int32? FieldInt32(DataRow row, string columnName)
        {
            if (!VerifyColumn(row, columnName)) return null;
            return row.IsNull(columnName) ? (Int32?) null : row.Field<Int32>(columnName);
        }

        public Int64? FieldInt64(DataRow row, string columnName)
        {
            if (!VerifyColumn(row, columnName)) return null;
            return row.IsNull(columnName) ? (Int64?) null : row.Field<Int64>(columnName);
        }

        public bool? FieldBool(DataRow row, string columnName)
        {
            if (!VerifyColumn(row, columnName)) return null;
            return row.IsNull(columnName) ? (bool?) null : row.Field<bool>(columnName);
        }

        #endregion
    }

    internal static class QueryableExtensions
    {
        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, string propertyName)
        {
            if (source == null) throw new ArgumentNullException("source");
            propertyName = propertyName.Replace(" ASC", string.Empty);
            int descIndex = propertyName.IndexOf(" DESC", StringComparison.Ordinal);
            if (descIndex >= 0) propertyName = propertyName.Substring(0, descIndex).Trim();
            if (String.IsNullOrEmpty(propertyName)) return source;
            ParameterExpression parameter = Expression.Parameter(source.ElementType, String.Empty);
            MemberExpression property = Expression.Property(parameter, propertyName);
            LambdaExpression lambda = Expression.Lambda(property, parameter);
            string methodName = (descIndex < 0) ? "OrderBy" : "OrderByDescending";
            Expression methodCallExpression = Expression.Call(typeof (Queryable), methodName,
                new[] {source.ElementType, property.Type},
                source.Expression, Expression.Quote(lambda));
            return source.Provider.CreateQuery<T>(methodCallExpression);
        }
    }
}
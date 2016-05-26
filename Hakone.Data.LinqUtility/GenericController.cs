using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Linq.Dynamic;
using System.Reflection;
using System.Web;
using FluentData;

namespace Hakone.Data.LinqUtility
{
    [DataObject]
    public abstract class GenericController<TEntity, TDataContext>
        where TDataContext : DataContext
        where TEntity : class
    {
        private static TDataContext m_dataContext;
        private static PropertyInfo[] m_columns;
        private static PropertyInfo m_primaryKey;
        private static string m_primaryKeyDBColumnName;

        #region Properties

        protected static TDataContext DataContext
        {
            get
            {
                if (HttpContext.Current != null)
                {
                    TDataContext dataContext = (TDataContext)HttpContext.Current.Items["dataContext"];

                    if (dataContext == null)
                    {
                        dataContext = Activator.CreateInstance<TDataContext>();
                        HttpContext.Current.Items.Add("dataContext", dataContext);
                    }

                    return dataContext;
                }
                else
                {
                    if (m_dataContext == null)
                    {
                        m_dataContext = Activator.CreateInstance<TDataContext>();
                    }

                    return m_dataContext;
                }

            }
        }
        protected static Table<TEntity> EntityTable
        {
            get { return DataContext.GetTable<TEntity>(); }
        }

        protected static string EntityName
        {
            get { return EntityType.Name; }
        }

        protected static Type EntityType
        {
            get { return typeof(TEntity); }
        }

        protected static string TableName
        {
            get
            {
                var att = EntityType.GetCustomAttributes(typeof(TableAttribute), false).FirstOrDefault();

                return att != null ? ((TableAttribute)att).Name : "";
            }
        }

        public static DataLoadOptions LoadOptions
        {
            get { return DataContext.LoadOptions; }
            set { DataContext.LoadOptions = value; }
        }


        protected static PropertyInfo[] Columns
        {
            get
            {
                if (m_columns == null)
                {
                    m_columns = (from p in EntityType.GetProperties()
                                 where p.GetIndexParameters().Length == 0 &
                                 p.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault() != null
                                 select p).ToArray<PropertyInfo>();
                }

                return m_columns;
            }
        }

        protected static PropertyInfo PrimaryKey
        {
            get
            {
                if (m_primaryKey == null)
                {
                    foreach (PropertyInfo pi in Columns)
                    {
                        foreach (ColumnAttribute col in pi.GetCustomAttributes(typeof(ColumnAttribute), false))
                        {
                            if (col.IsPrimaryKey)
                            {
                                m_primaryKey = pi;
                            }
                        }
                    }
                }

                return m_primaryKey;
            }
        }

        protected static string PrimaryKeyDBColumnName
        {
            get
            {
                if (m_primaryKeyDBColumnName == null)
                {
                    m_primaryKeyDBColumnName = GetDBColumnName(PrimaryKey);
                }

                return m_primaryKeyDBColumnName;
            }
        }

        protected static string GetDBColumnName(PropertyInfo p)
        {
            var att = p.GetCustomAttributes(typeof(ColumnAttribute), false).FirstOrDefault();

            if (att == null | ((ColumnAttribute)att).Name == null)
            {
                return p.Name;
            }

            return ((ColumnAttribute)att).Name;
        }

        #endregion

        #region Helper methods

        protected static object GetPrimaryKeyValue(TEntity entity)
        {
            #region Validation
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            #endregion

            return PrimaryKey.GetValue(entity, null);
        }

        protected static TEntity GetEntity(TEntity entity)
        {
            #region Validation
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            #endregion

            return GetEntity(GetPrimaryKeyValue(entity));
        }

        protected static void UpdateOriginalFromChanged(ref TEntity destination, TEntity source)
        {
            #region Validation
            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            #endregion

            foreach (PropertyInfo pi in Columns)
            {
                pi.SetValue(destination, pi.GetValue(source, null), null);
            }
        }

        #endregion

        #region Generic CRUD methods
        //---------------------Selects----------------------------------

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IQueryable<TEntity> SelectAll()
        {
            return EntityTable;
        }       

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IQueryable<TEntity> SelectAll(int maximumRows, int startRowIndex)
        {
            #region Validation
            if (maximumRows <= 0)
            {
                throw new ArgumentOutOfRangeException("maximumRows");
            }

            if (startRowIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startRowIndex");
            }
            #endregion

            return EntityTable.Skip(startRowIndex).Take(maximumRows);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IQueryable<TEntity> SelectAll(string sortExpression)
        {
            if (String.IsNullOrEmpty(sortExpression))
            {
                return SelectAll();
            }

            return EntityTable.OrderBy(sortExpression);
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static IQueryable<TEntity> SelectAll(string sortExpression, int maximumRows, int startRowIndex)
        {
            #region Validation
            if (maximumRows <= 0)
            {
                throw new ArgumentOutOfRangeException("maximumRows");
            }

            if (startRowIndex < 0)
            {
                throw new ArgumentOutOfRangeException("startRowIndex");
            }
            #endregion

            if (String.IsNullOrEmpty(sortExpression))
            {
                return SelectAll(maximumRows, startRowIndex);
            }

            return SelectAll(sortExpression).Skip(startRowIndex).Take(maximumRows);
        }

        public static int Count()
        {
            return EntityTable.Count();
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static List<TEntity> SelectAllAsList()
        {
            return EntityTable.ToList();
        }

        [DataObjectMethod(DataObjectMethodType.Select)]
        public static TEntity GetEntity(object id)
        {
            #region Validation
            if (id == null)
            {
                throw new ArgumentNullException("id");
            }
            #endregion

            string query = String.Format("Select * from {0} where {1} = {2}", new object[] { TableName, PrimaryKeyDBColumnName, id });

            return DataContext.ExecuteQuery<TEntity>(query).FirstOrDefault();
        }

        //----------------------Insert------------------------------------
        [DataObjectMethod(DataObjectMethodType.Insert)]
        public static void Insert(TEntity entity)
        {
            #region Validation
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            #endregion

            Insert(entity, true);
        }

        public static void Insert(TEntity entity, bool submitChanges)
        {
            #region Validation
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            #endregion

            EntityTable.InsertOnSubmit(entity);

            if (submitChanges)
            {
                DataContext.SubmitChanges();
            }
        }

        //-----------------------Update-----------------------------------------
        [DataObjectMethod(DataObjectMethodType.Update)]
        public static void Update(TEntity entity)
        {
            #region Validation
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            #endregion

            Update(entity, true);
        }

        public static void Update(TEntity entity, bool submitChanges)
        {
            #region Validation
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            #endregion

            TEntity original = GetEntity(entity);

            UpdateOriginalFromChanged(ref original, entity);

            if (submitChanges)
            {
                DataContext.SubmitChanges();
            }
        }

        //----------------------Delete-------------------------------------------
        [DataObjectMethod(DataObjectMethodType.Delete)]
        public static void Delete(TEntity entity)
        {
            #region Validation
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            #endregion

            Delete(entity, true);
        }

        public static void Delete(TEntity entity, bool submitChanges)
        {
            #region Validation
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            #endregion

            TEntity delete = GetEntity(entity);

            EntityTable.DeleteOnSubmit(delete);

            if (submitChanges)
            {
                DataContext.SubmitChanges();
            }
        }

        #endregion

        public static void SubmitChanges()
        {
            DataContext.SubmitChanges();
        }

        public IDbContext DefaultContext
        {
            get
            {
                IDbContext context = new DbContext().ConnectionStringName(@"Hakone.Domain.Properties.Settings.haodian8ConnectionString", new FluentData.SqlServerProvider());
                context.IsolationLevel(IsolationLevel.ReadCommitted);

                return context;
            }
        }
    }
}

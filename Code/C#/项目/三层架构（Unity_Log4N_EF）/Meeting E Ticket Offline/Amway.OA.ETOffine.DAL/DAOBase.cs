using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data.SQLite;
using System.Data;
using Amway.OA.ETOffine.Entities;
using System.Data.Entity;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq.Expressions;

namespace Amway.OA.ETOffine.DAL
{
    public class DAOBase<TEntity> :IDAO<TEntity> where TEntity : EntitieBase, new()
    {
        //private SQLiteHelper _Db;
        //protected DAOBase()
        //{
        //    this._Db = new SQLiteHelper();
        //}

        //private int NewID()
        //{
        //    return 0;
        //}

        //public void Add(TEntity entity)
        //{
        //    //if (entity.IsPropertyChange("ID"))
        //    //{
        //    //    entity.ID = NewID();
        //    //}

        //    var builder = new StringBuilder();
        //    var builderValue = new StringBuilder();

        //    // SQLite给主键ID赋NULL时，自动生成自增长值
        //    builder.AppendFormat("insert into {0} (ID", entity.TableName);
        //    builderValue.AppendFormat(" values({0}", "NULL");

        //    var paramList = new List<SQLiteParameter>();
        //    PropertyInfo[] properties = entity.GetType().GetProperties();

        //    foreach (var propertyInfo in properties)
        //    {
        //        if (propertyInfo.Name == "ID" || propertyInfo.Name == "TableName")
        //        {
        //            continue;
        //        }

        //        builder.AppendFormat(",{0}", entity.GetColumnName(propertyInfo.Name));
        //        builderValue.AppendFormat(",@{0}", propertyInfo.Name);

        //        var p = new SQLiteParameter("@" + propertyInfo.Name);
        //        //p.DbType = propertyInfo.PropertyType.Equals(typeof(int)) ? DbType.Int32 : DbType.String;
        //        p.Value = propertyInfo.GetValue(entity, null);

        //        paramList.Add(p);
        //    }
        //    builder.Append(") ");
        //    builderValue.Append("); ");

        //    _Db.ExecuteNonQuery(builder.ToString() + builderValue.ToString(), paramList.ToArray());
        //}

        //public void Update(TEntity entity)
        //{
        //    if (entity.IsPropertyChange())
        //    {
        //        var builder = new StringBuilder();
        //        builder.AppendFormat("update {0} set ", entity.TableName);

        //        var paramList = new List<SQLiteParameter>();
        //        PropertyInfo[] properties = entity.GetType().GetProperties();

        //        foreach (var propertyInfo in properties)
        //        {
        //            if (!entity.IsPropertyChange(propertyInfo.Name) || propertyInfo.Name == "ID" || propertyInfo.Name == "TableName")
        //            {
        //                continue;
        //            }

        //            builder.AppendFormat("{0}=@{1},", entity.GetColumnName(propertyInfo.Name), propertyInfo.Name);

        //            var p = new SQLiteParameter("@" + propertyInfo.Name);
        //            //p.DbType = propertyInfo.PropertyType.Equals(typeof(int)) ? DbType.Int32 : DbType.String;
        //            p.Value = propertyInfo.GetValue(entity, null);

        //            paramList.Add(p);
        //        }
        //        builder.Remove(builder.Length - 1, 1);
        //        builder.AppendFormat(" where ID={0}; ", entity.ID);

        //        _Db.ExecuteNonQuery(builder.ToString(), paramList.ToArray());
        //    }
        //}

        //public void Delete(int id)
        //{
        //    var entity = new TEntity();
        //    _Db.ExecuteNonQuery(string.Format("delete from {0} where ID={1}; ", entity.TableName, id));
        //}

        //public TEntity GetByID(int id)
        //{
        //    string strSQL = this.Select() + " where ID=" + id;
        //    var list = this.GetEntityList(strSQL);
        //    return list.Count > 0 ? list[0] : null;
        //}

        //public int GetRecordCount(string strWhere, params SQLiteParameter[] param)
        //{
        //    string strSQL = string.Format(" select count(*) {0} where 1=1 {1} ", new TEntity().TableName, strWhere);
        //    return (int)_Db.ExecuteScalar(strSQL, param); 
        //}

        //public List<TEntity> GetFilterList(string strWhere, string orderBy = "", bool isAsc = true, params SQLiteParameter[] param)
        //{
        //    return this.GetFilterPagerList(strWhere, 0, 0, orderBy, isAsc, param);
        //}

        //public List<TEntity> GetFilterPagerList(string strWhere, int startIndex, int endIndex, string orderBy = "", bool isAsc = true, params SQLiteParameter[] param)
        //{
        //    var strSQL = this.Select();
        //    if (!string.IsNullOrWhiteSpace(strWhere))
        //    {
        //        strSQL += " where 1=1 " + strWhere;
        //    }

        //    if (!string.IsNullOrWhiteSpace(orderBy))
        //    {
        //        strSQL += " order by " + orderBy + (isAsc ? "asc" : "desc");
        //    }

        //    if (startIndex > 0 && endIndex > 0 && endIndex > startIndex)
        //    {
        //        strSQL += string.Format(" limit {0} offset {1}", endIndex - startIndex, startIndex);
        //    }

        //    return this.GetEntityList(strSQL, param);
        //}

        //public DataTable GetFilterDataTable(string strWhere, string orderBy = "", bool isAsc = true, params SQLiteParameter[] param)
        //{
        //    return this.GetFilterPagerDataTable(strWhere, 0, 0, orderBy, isAsc, param);
        //}

        //public DataTable GetFilterPagerDataTable(string strWhere, int startIndex, int endIndex, string orderBy = "", bool isAsc = true, params SQLiteParameter[] param)
        //{
        //    var strSQL = this.Select();
        //    if (!string.IsNullOrWhiteSpace(strWhere))
        //    {
        //        strSQL += " where 1=1 " + strWhere;
        //    }

        //    if (!string.IsNullOrWhiteSpace(orderBy))
        //    {
        //        strSQL += " order by " + orderBy + (isAsc ? "asc" : "desc");
        //    }

        //    if (startIndex > 0 && endIndex > 0 && endIndex > startIndex)
        //    {
        //        strSQL += string.Format(" limit {0} offset {1}", endIndex - startIndex, startIndex);
        //    }

        //    return _Db.ExecuteDataTable(strSQL, param);
        //}

        //private string Select()
        //{
        //    var entity = new TEntity();
        //    return string.Format("select {0} from {1} ", entity.GetAllColumnsName(), entity.TableName);
        //}

        //private List<TEntity> GetEntityList(string strSQL, params SQLiteParameter[] param)
        //{
        //    var list = new List<TEntity>();
        //    PropertyInfo[] properties = typeof(TEntity).GetType().GetProperties();

        //    using (var reader = _Db.ExecuteDataReader(strSQL, System.Data.CommandBehavior.CloseConnection, param))
        //    {
        //        var entity = new TEntity();
        //        foreach (var p in properties)
        //        {
        //            var dbValue = reader[entity.GetColumnName(p.Name)];
        //            if (dbValue != DBNull.Value)
        //            {
        //                p.SetValue(entity, dbValue, null);
        //            }
        //        }
        //        list.Add(entity);
        //    }

        //    return list;
        //}

        #region 构造函数
        
        /// <summary>
        /// 构造函数
        /// </summary>
        public DAOBase()
        {
            this.dataContext = new ETOfflineContext();
        }

        /// <summary>
        /// 构造函数


        /// </summary>
        /// <param name="context">数据库上下文</param>
        public DAOBase(DbContext context)
        {
            this.dataContext = context;
        }

        #endregion

        protected DbContext dataContext;

        /// <summary>
        /// 获取数据库DbContext 对应的ObjectContext
        /// </summary>
        public ObjectContext ObjectContext
        {
            get
            {
                ObjectContext context = ((IObjectContextAdapter)dataContext).ObjectContext;
                return context;
            }
        }

        /// <summary>
        /// 数据库上下文
        /// </summary>
        public DbContext DataContext
        {
            get { return dataContext; }
            //set { dataContext = value; }
        }

        /// <summary>
        /// 获得当前数据库上下文
        /// </summary>
        /// <returns></returns>
        public DbContext GetDataContext()
        {
            return dataContext;
        }

        public void Add(TEntity entity)
        {
            entity.ID = null;
            dataContext.Set<TEntity>().Add(entity);
            dataContext.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            if (null == entity)
            {
                throw new ArgumentException("输入的实体是null值");
            }
            dataContext.Set<TEntity>().Remove(entity);
            dataContext.SaveChanges();
        }

        public void Delete(List<TEntity> entitys)
        {
            DbSet<TEntity> objectSet = dataContext.Set<TEntity>();

            foreach (var item in entitys)
            {
                objectSet.Remove(item);
            }
            dataContext.SaveChanges();
        }

        public void DeleteById(Int64 Id)
        {
            DbSet<TEntity> objectSet = dataContext.Set<TEntity>();
            TEntity obj = objectSet.SingleOrDefault(o => o.ID == Id);
            if (null != obj)
            {
                objectSet.Remove(obj);
            }
            dataContext.SaveChanges();
        }

        public void DeleteById(List<Int64> Ids)
        {
            DbSet<TEntity> objectSet = dataContext.Set<TEntity>();
            foreach (var item in Ids)
            {
                TEntity obj = GetById(item);
                objectSet.Remove(obj);
            }
            dataContext.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            //检查输入变量
            if (entity == null)
                throw new ArgumentException("entity不能为空", "entity");

            //获得ObjectSet
            DbSet<TEntity> objectSet = dataContext.Set<TEntity>();

            //取得数据库中的值
            var oldValue = GetById(entity.ID.Value);

            //更新为传入的值
            entity.UpdateDBEntity(oldValue);

            //保存到数据库
            this.DataContext.SaveChanges();
        }

        public TEntity GetById(Int64 Id)
        {
            //检查查询变量
            if (Id <= 0)
                throw new ArgumentException("Id不能为空", "Id");

            //获得ObjectSet
            DbSet<TEntity> objectSet = dataContext.Set<TEntity>();

            //根据ID返回单条记录
            return objectSet.SingleOrDefault(o => o.ID == Id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            DbSet<TEntity> objectSet = this.DataContext.Set<TEntity>();
            return objectSet;
        }

        public IEnumerable<TEntity> GetFilteredElements<S>(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, System.Linq.Expressions.Expression<Func<TEntity, S>> orderByExpression = null, bool ascending = true)
        {
            if (filter == (Expression<Func<TEntity, bool>>)null)
                filter = o => 1 == 1;

            //获得ObjectSet
            DbSet<TEntity> objectSet = this.DataContext.Set<TEntity>();

            if (orderByExpression == (Expression<Func<TEntity, S>>)null)
            {
                return objectSet.Where(filter);
            }
            else
            {
                return (ascending)
                    ?
                        objectSet
                            .Where(filter)
                            .OrderBy(orderByExpression)
                    :
                        objectSet
                            .Where(filter)
                            .OrderByDescending(orderByExpression);
            }                
        }

        public List<TEntity> GetFilteredList<S>(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, System.Linq.Expressions.Expression<Func<TEntity, S>> orderByExpression, bool ascending = true)
        {
            //获得ObjectSet
            DbSet<TEntity> objectSet = this.DataContext.Set<TEntity>();

            //条件是否为空
            if (null == filter)
            {
                filter = o => 1 == 1;
            }

            //查询数据
            List<TEntity> result = (ascending)
                                ?
                                    objectSet
                                     .Where(filter)
                                     .OrderBy(orderByExpression)
                                     .ToList()
                                :
                                    objectSet
                                     .Where(filter)
                                     .OrderByDescending(orderByExpression)
                                     .ToList();
            //返回
            return result;
        }

        public List<TEntity> GetFilteredList(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            //获得ObjectSet
            DbSet<TEntity> objectSet = this.DataContext.Set<TEntity>();

            //条件是否为空
            if (null == filter)
            {
                filter = o => 1 == 1;
            }

            //查询数据
            List<TEntity> result = objectSet
                                     .Where(filter)
                                     .ToList();

            //返回
            return result;
        }

        public List<TEntity> GetFilteredList(string strWhere, string strSort)
        {
            //List<TEntity> list = new List<TEntity>();

            //if (strWhere == "")
            //{
            //    strWhere = " (it.Status=1 or it.Status=0) ";
            //}
            //else
            //{
            //    strWhere += " and (it.Status=1 or it.Status=0) ";
            //}

            //string queryStr = string.Format(@"SELECT value it FROM AmwayFrameworkEntities.{0}s as it WHERE {1}", this.EntityName, strWhere);
            //list = this.ObjectContext.CreateQuery<TEntity>(queryStr).OrderBy(strSort).ToList();
            //return list;
            return new List<TEntity>();
        }

        public TEntity GetSingle(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            //获得ObjectSet
            DbSet<TEntity> objectSet = this.DataContext.Set<TEntity>();

            //条件是否为空
            if (null == filter)
            {
                filter = o => 1 == 1;
            }

            return objectSet.Where(filter).FirstOrDefault();
        }

        public void ExportFilteredElements<S>(System.IO.Stream exportStream, int ExportPageSize, System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, System.Linq.Expressions.Expression<Func<TEntity, S>> orderByExpression, bool ascending)
        {
        //    StreamWriter sw = new StreamWriter(exportStream);
        //    HtmlTextWriter hw = new HtmlTextWriter(sw);

        //    List<string> columnNames = new List<string>();
        //    //<html>
        //    hw.RenderBeginTag(HtmlTextWriterTag.Html);

        //    //<head>
        //    hw.RenderBeginTag(HtmlTextWriterTag.Head);

        //    //<meta http-equiv='content-type' content='application/ms-excel; charset=GB2312' />
        //    hw.AddAttribute("http-equiv", "content-type");
        //    hw.AddAttribute("content", "application/ms-excel");
        //    hw.AddAttribute("charset", "utf-8");
        //    hw.RenderBeginTag(HtmlTextWriterTag.Meta);
        //    hw.RenderEndTag();

        //    //<style type="text/css">
        //    //table{font-size:12px; border:1px; border-color:#969696 }
        //    //th{ background:#CCCCCC}
        //    //</style>
        //    hw.AddAttribute("type", "text/css");
        //    hw.RenderBeginTag(HtmlTextWriterTag.Style);
        //    hw.Write(@"table{font-size:12px; border:1px; border-color:#969696 }");
        //    hw.Write(@"th{ background:#CCCCCC}");
        //    hw.RenderEndTag();

        //    //</head>
        //    hw.RenderEndTag();

        //    //<body>
        //    hw.RenderBeginTag(HtmlTextWriterTag.Body);

        //    //<table>
        //    hw.RenderBeginTag(HtmlTextWriterTag.Table);


        //    PagerInfo pageInfo = new PagerInfo();
        //    pageInfo.PageSize = ExportPageSize;
        //    pageInfo.CurrentPage = 1;

        //    TEntity t = new TEntity();
        //    PropertyInfo[] ps = t.GetType().GetProperties();

        //    hw.RenderBeginTag(HtmlTextWriterTag.Tr);

        //    foreach (PropertyInfo p in ps)
        //    {
        //        if (!(p.PropertyType.Name.Contains("ICollection")))
        //        {
        //            hw.RenderBeginTag(HtmlTextWriterTag.Th);
        //            hw.Write(p.Name);
        //            hw.RenderEndTag();
        //            columnNames.Add(p.Name);
        //        }
        //    }
        //    hw.RenderEndTag();


        //    while (true)
        //    {
        //        PagerResult<TEntity> rt = GetFilteredElements(pageInfo, filter, orderByExpression, ascending);
        //        if (rt.Result.Count() <= 0)
        //        {
        //            break;
        //        }
        //        foreach (TEntity o in rt.Result)
        //        {
        //            hw.RenderBeginTag(HtmlTextWriterTag.Tr);
        //            foreach (string columnName in columnNames)
        //            {
        //                string value = string.Empty;
        //                object col = o.GetPropertyValue(columnName);
        //                if (null != col)
        //                {
        //                    value = col.ToString();
        //                }
        //                hw.RenderBeginTag(HtmlTextWriterTag.Td);
        //                value = HttpUtility.HtmlEncode(value);
        //                hw.Write(value);
        //                hw.RenderEndTag();
        //            }
        //            hw.RenderEndTag();
        //            hw.Flush();
        //            exportStream.Flush();
        //        }
        //        pageInfo.CurrentPage = pageInfo.CurrentPage + 1;
        //    }

        //    //</table>
        //    hw.RenderEndTag();

        //    //</body>
        //    hw.RenderEndTag();

        //    //</html>
        //    hw.RenderEndTag();

        //    hw.Flush();
        //    exportStream.Flush();
        }

        public int ExcuteSQL(string sql)
        {
            return dataContext.Database.ExecuteSqlCommand(sql);
        }

        public List<TEntity> GetPagedFilteredList(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, int startRecord, int maxRecords)
        {
            return GetPagedFilteredList(filter, o => o.ID, true, startRecord, maxRecords);
        }

        public List<TEntity> GetPagedFilteredList<S>(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, System.Linq.Expressions.Expression<Func<TEntity, S>> orderByExpression, bool ascending, int startRecord, int maxRecords)
        {
            //获得ObjectSet
            DbSet<TEntity> objectSet = this.DataContext.Set<TEntity>();

            //条件是否为空
            if (null == filter)
            {
                filter = o => 1 == 1;
            }

            //查询数据
            List<TEntity> result = (ascending)
                                ?
                                    objectSet
                                     .Where(filter)
                                     .OrderBy(orderByExpression).Skip<TEntity>(startRecord).Take<TEntity>(maxRecords)
                                     .ToList()
                                :
                                    objectSet
                                     .Where(filter)
                                     .OrderByDescending(orderByExpression).Skip<TEntity>(startRecord).Take<TEntity>(maxRecords)
                                     .ToList();
            //返回
            return result;
        }

        public int GetRecordCount(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            //获得ObjectSet
            DbSet<TEntity> objectSet = this.DataContext.Set<TEntity>();

            //条件是否为空
            if (null == filter)
            {
                filter = o => 1 == 1;
            }
            return objectSet.Count(filter);
        }

        public void SaveChanges()
        {
            this.DataContext.SaveChanges();
        }
    }
}

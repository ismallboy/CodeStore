using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Amway.OA.ETOffine.Entities;
using System.IO;
using Amway.OA.ETOffine.DAL;
using System.Runtime.CompilerServices;

namespace Amway.OA.ETOffine.BLL
{
    public class BOBase<TEntity, TIDAO> : IBO<TEntity>
        where TEntity : EntitieBase, new()
        where TIDAO : IDAO<TEntity>
    {
        /// <summary>
        /// DAO接口
        /// </summary>
        private TIDAO dao;

        /// <summary>
        /// DAO接口
        /// 由IOC框架通过属性注入方式注入DAO
        /// </summary>
        public TIDAO Dao
        {
            get {
                if (dao == null)
                {
                    dao = ServiceLocator.GetService<TIDAO>();
                }
                return dao;             
            }
            set { dao = value; }
        }

        public int ExcuteSQL(string sql)
        {
            return Dao.ExcuteSQL(sql);
        }

        public void Add(TEntity entity)
        {
            Dao.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            Dao.Delete(entity);
        }

        public void Delete(List<TEntity> entitys)
        {
            Dao.Delete(entitys);
        }

        public void DeleteById(Int64 Id)
        {
            Dao.DeleteById(Id);
        }

        public void DeleteById(List<Int64> Ids)
        {
            Dao.DeleteById(Ids);
        }

        public void Update(TEntity entity)
        {
            Dao.Update(entity);
        }

        public TEntity GetById(Int64 Id)
        {
            return Dao.GetById(Id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Dao.GetAll();
        }

        public IEnumerable<TEntity> GetFilteredElements<S>(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, System.Linq.Expressions.Expression<Func<TEntity, S>> orderByExpression = null, bool ascending = true)
        {
            return Dao.GetFilteredElements<S>(filter, orderByExpression, ascending);
        }

        public List<TEntity> GetFilteredList<S>(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, System.Linq.Expressions.Expression<Func<TEntity, S>> orderByExpression, bool ascending = true)
        {
            return Dao.GetFilteredList<S>(filter, orderByExpression, ascending);
        }

        public List<TEntity> GetFilteredList(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return Dao.GetFilteredList(filter);
        }

        public List<TEntity> GetFilteredList(string strWhere, string strSort)
        {
            return Dao.GetFilteredList(strWhere, strSort);
        }

        public TEntity GetSingle(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return Dao.GetSingle(filter);
        }

        public void ExportFilteredElements<S>(Stream exportStream, int ExportPageSize, System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, System.Linq.Expressions.Expression<Func<TEntity, S>> orderByExpression, bool ascending)
        {
            Dao.ExportFilteredElements(exportStream, ExportPageSize, filter, orderByExpression, ascending);
        }

        public List<TEntity> GetPagedFilteredList(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, int startRecord, int maxRecords)
        {
            return Dao.GetPagedFilteredList(filter, startRecord, maxRecords);
        }

        public List<TEntity> GetPagedFilteredList<S>(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter, System.Linq.Expressions.Expression<Func<TEntity, S>> orderByExpression, bool ascending, int startRecord, int maxRecords)
        {
            return Dao.GetPagedFilteredList<S>(filter, orderByExpression, ascending, startRecord, maxRecords);
        }

        public int GetRecordCount(System.Linq.Expressions.Expression<Func<TEntity, bool>> filter)
        {
            return Dao.GetRecordCount(filter);
        }

        public void SaveChanges()
        {
            Dao.SaveChanges();
        }
    }
}

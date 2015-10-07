using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using Amway.OA.ETOffine.Entities;
using System.IO;

namespace Amway.OA.ETOffine.BLL
{
    public interface IBO<TEntity>  where TEntity : EntitieBase
    {
        //void Add(TEntity entity);

        //void Update(TEntity entity);

        //void Delete(int id);

        //TEntity GetByID(int id);

        //int GetRecordCount(string strWhere, params SQLiteParameter[] param);

        //List<TEntity> GetFilterList(string strWhere, string orderBy = "", bool isAsc = true, params SQLiteParameter[] param);

        //List<TEntity> GetFilterPagerList(string strWhere, int startIndex, int endIndex, string orderBy = "", bool isAsc = true, params SQLiteParameter[] param);

        //DataTable GetFilterDataTable(string strWhere, string orderBy = "", bool isAsc = true, params SQLiteParameter[] param);

        //DataTable GetFilterPagerDataTable(string strWhere, int startIndex, int endIndex, string orderBy = "", bool isAsc = true, params SQLiteParameter[] param);
        
        /// <summary>
        /// 执行SQL
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        int ExcuteSQL(string sql);

        /// <summary>
        /// 增加实体对象
        /// </summary>
        /// <param name="entity">需要新增的实体对象实例</param>
        void Add(TEntity entity);

        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="entity">需要删除的实体对象实例</param>
        void Delete(TEntity entity);

        /// <summary>
        ///  删除实体对象
        /// </summary>
        /// <param name="entitys">删除实体对象集合</param>
        void Delete(List<TEntity> entitys);

        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="Id">需要删除的实体对象的Id</param>
        void DeleteById(Int64 Id);

        /// <summary>
        /// 删除实体对象
        /// </summary>
        /// <param name="Id">需要删除的实体对象的Id集合</param>
        void DeleteById(List<Int64> Ids);

        /// <summary>
        /// 更新实体对象
        /// </summary>
        /// <param name="entity">需要更新的实体对象</param>
        void Update(TEntity entity);

        /// <summary>
        /// 根据ID返回实体对象
        /// </summary>
        /// <param name="Id">实体ID</param>
        /// <returns>实体对象</returns>
        TEntity GetById(Int64 Id);

        /// <summary>
        /// 返回所有的实体对象
        /// </summary>
        /// <returns>实体对象集合</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// 返回符合查询条件的实体对象
        /// </summary>
        /// <typeparam name="S">排序字段的类型，可以不指定</typeparam>
        /// <param name="filter">查询条件</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="ascending">升降序</param>
        /// <returns>返回通用分页结果集，类型为<see cref="PagerResult"/></returns>
        IEnumerable<TEntity> GetFilteredElements<S>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, S>> orderByExpression = null, bool ascending = true);

        /// <summary>
        /// 返回符合条件的列表

        /// </summary>
        /// <typeparam name="S">排序字段的类型，可以不指定</typeparam>
        /// <param name="filter">查询条件</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="ascending">升降序</param>
        /// <returns>返回符合条件的列表</returns>
        List<TEntity> GetFilteredList<S>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, S>> orderByExpression, bool ascending = true);

        /// <summary>
        /// 返回符合条件的列表

        /// </summary>
        /// <typeparam name="S">排序字段的类型，可以不指定</typeparam>
        /// <param name="filter">查询条件</param>
        /// <returns>返回符合条件的列表</returns>
        List<TEntity> GetFilteredList(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 根据条件获取排序的列表

        /// </summary>
        /// <param name="strWhere">ObjectContext的查询表达式,对象别名为it</param>
        /// <param name="strSort">排序表达式</param>
        /// <returns>List对象</returns>
        List<TEntity> GetFilteredList(string strWhere, string strSort);

        /// <summary>
        /// 返回符合条件的单条记录

        /// </summary>
        /// <param name="filter">查询条件</param>
        /// <returns>返回符合条件的单条记录</returns>
        TEntity GetSingle(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 导出符合查询条件的实体对象
        /// 此方法因不能分页导出，如果数据量大不建议使用此方法
        /// </summary>
        /// <typeparam name="S">排序字段的类型，可以不指定</typeparam>
        /// <param name="exportStream">导出流</param>
        /// <param name="ExportPageSize">导出时每页的大小</param>
        /// <param name="filter">查询条件</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="ascending">升降序</param>
        void ExportFilteredElements<S>(Stream exportStream, int ExportPageSize, Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, S>> orderByExpression, bool ascending);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="filter">条件</param>
        /// <param name="startRecord">开始记录</param>
        /// <param name="maxRecords">最大返回记录</param>
        /// <returns>返回列表</returns>
        List<TEntity> GetPagedFilteredList(Expression<Func<TEntity, bool>> filter, int startRecord, int maxRecords);

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="S">排序类型</typeparam>
        /// <param name="filter">条件</param>
        /// <param name="orderByExpression">排序表达式</param>
        /// <param name="ascending">升降序</param>
        /// <param name="startRecord">开始记录</param>
        /// <param name="maxRecords">最大返回记录</param>
        /// <returns>返回列表</returns>
        List<TEntity> GetPagedFilteredList<S>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, S>> orderByExpression, bool ascending, int startRecord, int maxRecords);

        /// <summary>
        /// 返回符合条件的记录条数
        /// </summary>
        /// <param name="filter">条件</param>
        /// <returns></returns>
        int GetRecordCount(Expression<Func<TEntity, bool>> filter);

        /// <summary>
        /// 保存所有修改
        /// </summary>
        void SaveChanges();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Amway.OA.ETOffine.Entities
{
    public class EntitieBase
    {
        protected Dictionary<string, string> _MapDictionary = new Dictionary<string, string>();

        protected Dictionary<string, bool> _IsChargeDictionary = new Dictionary<string, bool>();

        //protected string _TableName = string.Empty;
        //public string TableName { get { return _TableName; } }

        public EntitieBase()
        {
        }

        private Nullable<Int64> _ID;
        virtual public Nullable<Int64> ID
        {
            get { return _ID; }
            set
            {
                this._ID = value;
                this.MarkPropertyChange("ID");
            }
        }

        private Nullable<System.DateTime> _syncDate;
        virtual public Nullable<System.DateTime> SyncDate
        {
            get { return _syncDate; }
            set
            {
                this._syncDate = value;
                this.MarkPropertyChange("SyncDate");
            }
        }

        private int _syncStatus;
        virtual public int SyncStatus
        {
            get { return _syncStatus; }
            set
            {
                this._syncStatus = value;
                this.MarkPropertyChange("SyncStatus");
            }
        }
        /// <summary>
        /// 标记该属性已经修改
        /// </summary>
        /// <param name="propertyName">属性名</param>
        protected void MarkPropertyChange(string propertyName)
        {
            this._IsChargeDictionary[propertyName] = true;
        }

        /// <summary>
        /// 判断是否有更新的属性
        /// </summary>
        /// <returns></returns>
        public bool IsPropertyChange()
        {
            foreach (var item in this._IsChargeDictionary)
            {
                if (item.Key != "ID"  && item.Value == true)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 通过名称属性获取列名
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        public string GetColumnName(string propertyName)
        {
            if (this._MapDictionary.ContainsKey(propertyName))
            {
                return this._MapDictionary[propertyName];
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 获取当前表的所有列名（格式：XX,YY）
        /// </summary>
        /// <returns></returns>
        public string GetAllColumnsName()
        {
            return string.Join(",", this._MapDictionary.Select(o => o.Value));
        }

        /// <summary>
        /// 检查该属性是否已经修改
        /// </summary>
        /// <param name="propertyName">属性名</param>
        /// <returns>是否已经修改</returns>
        public bool IsPropertyChange(string propertyName)
        {
            if (_IsChargeDictionary.ContainsKey(propertyName))
            {
                return _IsChargeDictionary[propertyName];
            }
            else
            {
                return false;
            }
        }

        public void UpdateDBEntity(EntitieBase oldValue)
        {
            if (this.IsPropertyChange())
            {
                PropertyInfo[] properties = this.GetType().GetProperties();

                foreach (var propertyInfo in properties)
                {
                    //不能直接更改ID字段
                    if (IsPropertyChange(propertyInfo.Name) && (propertyInfo.Name != "ID"))
                    {
                        propertyInfo.SetValue(oldValue, propertyInfo.GetValue(this, null), null);
                    }
                }
            }
        }
    }
}

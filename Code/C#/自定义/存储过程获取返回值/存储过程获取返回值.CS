        /// <summary>
        /// 把相关数据写入活动手机助手发布数据表
        /// </summary>
        /// <param name="meetingId">会议ID</param>
        /// <returns>0失败，1成功/// </returns>
        public int WriteIPublishTable(string recordId)
        {
            string sql = "execute SP_MSAT_WritePublishInterfaceTable @recordID, @returnValue output";
            var parList = new List<SqlParameter>();
            var par = new SqlParameter("@recordID", recordId);
            var returnValue = new SqlParameter("@returnValue", SqlDbType.Int);
            returnValue.Direction = ParameterDirection.Output;
            parList.Add(par);
            parList.Add(returnValue);

            DB.ExecuteNonQuery(sql, parList.ToArray());
            return returnValue.Value.ToInt(0);
        }
/// <summary>
/// 下载文件
/// </summary>
/// <param name="fileName">导出文件本地Path</param>
/// <param name="outputName">输出文件名，包含扩展名,传入前最好Server.UrlEncode</param>
public static void DownloadFile(string fileName,string outputName)
{
	byte[] data;
	System.Threading.Thread.Sleep(100);
	using (FileStream fs = File.Open(fileName, FileMode.Open, FileAccess.Read))
	{
		data = new byte[fs.Length];
		fs.Read(data, 0, data.Length);
	}

	HttpContext.Current.Response.Clear();
	HttpContext.Current.Response.Charset = "UTF-8";
	HttpContext.Current.Response.AddHeader("Content-Disposition", "attachment;filename=" + outputName);
	HttpContext.Current.Response.AddHeader("Content-Length", data.Length.ToString());
	HttpContext.Current.Response.ContentType = "application/download";
	HttpContext.Current.Response.BinaryWrite(data);
	HttpContext.Current.Response.Flush();
	HttpContext.Current.Response.Close();
	//HttpContext.Current.Response.End();   //Alan 2015-05-06 注析 原因：Try catch 会把response.end()语句作为语句中断的错误来执行
}

/// <summary>
/// 导入座位信息
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnImport_Click(object sender, EventArgs e)
{
	//判断是否exel文件
	if (string.IsNullOrEmpty(fudSeatTable.FileName))
	{
		ShowMessage("请选择要导入的文件");
		return;
	}
	if (Path.GetExtension(fudSeatTable.PostedFile.FileName).ToLower() != ".xls")
	{
		ShowMessage("请选择.xls格式的文件");
		return;
	} 

	//校验数据
	Stream importStream = fudSeatTable.PostedFile.InputStream;
	//读取excel文件数据
	var listExcel = ExcelHelper.ReadExcelData(importStream);
	string msg = string.Empty;
	if (listExcel != null && listExcel.Count > 0)
	{
		var listSeatChart = new List<MsetSeatingChart>();
		if (CheckData(listExcel, out listSeatChart, out msg))
		{
			//加长事务时间 10分钟
			using (TransactionScope tran = new TransactionScope( TransactionScopeOption.Required, new TimeSpan(0,10,0)))
			{
				var msetSeatChartBO = GetService<IMsetSeatingChartBO>();
				//先删除原来的座位表信息，再添加当前导入的座位表信息
				var oldSeatTable = msetSeatChartBO.GetFilteredList(o => o.RecordID == this.RecordID);
				foreach (var item in oldSeatTable)
				{
					msetSeatChartBO.Delete(item);
				}
				foreach (var item in listSeatChart)
				{
					msetSeatChartBO.Add(item);
				}
				tran.Complete();
				ShowMessage("导入成功;");
				//关闭弹出层，刷新父页面
				this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "MsetUnblockUI", "$.unblockUI();$('#ifrmImportSeatTable').contents().find('#btnSearch').click();", true);
			}
		}
		else
		{
			ShowMessage("导入座位表信息失败:\\n" + msg);
			Logger.Debug("导出座位表信息错误,详细:" + msg);
			return;
		}
	}
	//解除弹出层
	this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "MsetUnblockUI", "$.unblockUI();", true);
}

/// <summary>
/// 模板下载
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void ibtnDownload_Click(object sender, EventArgs e)
{
	try
	{
		//根据门票数据设置生成excel模板提供下载
		var msetSeatTableChartBO = GetService<IMsetSeatingChartBO>();
		string fileFolder = Server.MapPath("~/Template/");
		var fileName = "MsetSeatTableTemplate_" + Guid.NewGuid().ToString() + ".xls";
		string fileFullName = fileFolder + fileName;
		if (!msetSeatTableChartBO.CreateSeatTableTemplate(this.RecordID, fileFullName))
		{
			ShowMessage("生成模板失败");
			return;
		}

		var mapPath = Request.MapPath("~/Template/" + fileName);

		var exportName = Server.UrlEncode("导入座位信息模板.xls");
		MsetUtility.DownloadFile(mapPath, exportName);
		//删除临时文件
		if (File.Exists(mapPath))
		{
			File.Delete(mapPath);
		}
	}
	catch (Exception ex)
	{
		Logger.Error("下载座位信息模板出错，文件路径：详细" + ex.Message + ex.StackTrace);
		ShowMessage("下载座位信息模板失败，请刷新重试");
	}
}
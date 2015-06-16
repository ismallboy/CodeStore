/// <summary>
/// �����ļ�
/// </summary>
/// <param name="fileName">�����ļ�����Path</param>
/// <param name="outputName">����ļ�����������չ��,����ǰ���Server.UrlEncode</param>
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
	//HttpContext.Current.Response.End();   //Alan 2015-05-06 ע�� ԭ��Try catch ���response.end()�����Ϊ����жϵĴ�����ִ��
}

/// <summary>
/// ������λ��Ϣ
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void btnImport_Click(object sender, EventArgs e)
{
	//�ж��Ƿ�exel�ļ�
	if (string.IsNullOrEmpty(fudSeatTable.FileName))
	{
		ShowMessage("��ѡ��Ҫ������ļ�");
		return;
	}
	if (Path.GetExtension(fudSeatTable.PostedFile.FileName).ToLower() != ".xls")
	{
		ShowMessage("��ѡ��.xls��ʽ���ļ�");
		return;
	} 

	//У������
	Stream importStream = fudSeatTable.PostedFile.InputStream;
	//��ȡexcel�ļ�����
	var listExcel = ExcelHelper.ReadExcelData(importStream);
	string msg = string.Empty;
	if (listExcel != null && listExcel.Count > 0)
	{
		var listSeatChart = new List<MsetSeatingChart>();
		if (CheckData(listExcel, out listSeatChart, out msg))
		{
			//�ӳ�����ʱ�� 10����
			using (TransactionScope tran = new TransactionScope( TransactionScopeOption.Required, new TimeSpan(0,10,0)))
			{
				var msetSeatChartBO = GetService<IMsetSeatingChartBO>();
				//��ɾ��ԭ������λ����Ϣ������ӵ�ǰ�������λ����Ϣ
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
				ShowMessage("����ɹ�;");
				//�رյ����㣬ˢ�¸�ҳ��
				this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "MsetUnblockUI", "$.unblockUI();$('#ifrmImportSeatTable').contents().find('#btnSearch').click();", true);
			}
		}
		else
		{
			ShowMessage("������λ����Ϣʧ��:\\n" + msg);
			Logger.Debug("������λ����Ϣ����,��ϸ:" + msg);
			return;
		}
	}
	//���������
	this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "MsetUnblockUI", "$.unblockUI();", true);
}

/// <summary>
/// ģ������
/// </summary>
/// <param name="sender"></param>
/// <param name="e"></param>
protected void ibtnDownload_Click(object sender, EventArgs e)
{
	try
	{
		//������Ʊ������������excelģ���ṩ����
		var msetSeatTableChartBO = GetService<IMsetSeatingChartBO>();
		string fileFolder = Server.MapPath("~/Template/");
		var fileName = "MsetSeatTableTemplate_" + Guid.NewGuid().ToString() + ".xls";
		string fileFullName = fileFolder + fileName;
		if (!msetSeatTableChartBO.CreateSeatTableTemplate(this.RecordID, fileFullName))
		{
			ShowMessage("����ģ��ʧ��");
			return;
		}

		var mapPath = Request.MapPath("~/Template/" + fileName);

		var exportName = Server.UrlEncode("������λ��Ϣģ��.xls");
		MsetUtility.DownloadFile(mapPath, exportName);
		//ɾ����ʱ�ļ�
		if (File.Exists(mapPath))
		{
			File.Delete(mapPath);
		}
	}
	catch (Exception ex)
	{
		Logger.Error("������λ��Ϣģ������ļ�·������ϸ" + ex.Message + ex.StackTrace);
		ShowMessage("������λ��Ϣģ��ʧ�ܣ���ˢ������");
	}
}
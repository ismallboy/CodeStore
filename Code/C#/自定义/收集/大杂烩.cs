1.��̨ˢ�½��棺
	��//��ȡ�ɹ���رմ��ڲ�ˢ�¸�����
	string myScript = "<script>window.opener.location.href = window.opener.location.href;  if (window.opener.progressWindow) { window.opener.progressWindow.close(); }  window.close();</script>";
	Page.ClientScript.RegisterStartupScript(typeof(string), "closeWindowAndRefresh", myScript);
	��response.redirect("");
2.WebService�������session
	[WebMethod(EnableSession=true)]
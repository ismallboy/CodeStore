1.   Linq��ѯ��ֵ�Ƿ��ظ�        
	//��λ��Ψһ
	var groupList = (from s in listSeatChart group s by new {SeatName = s.SeatName } into newGroup where newGroup.Count() > 1 select newGroup.Key.SeatName).ToArray();
	if (groupList != null && groupList.Length > 0)
	{ 
			msg += "��λ��{0}�����ظ�ֵ;\\n".FormatWith(string.Join(",",groupList));
	}
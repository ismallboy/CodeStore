1.   Linq查询列值是否重复        
	//座位号唯一
	var groupList = (from s in listSeatChart group s by new {SeatName = s.SeatName } into newGroup where newGroup.Count() > 1 select newGroup.Key.SeatName).ToArray();
	if (groupList != null && groupList.Length > 0)
	{ 
			msg += "座位号{0}存在重复值;\\n".FormatWith(string.Join(",",groupList));
	}
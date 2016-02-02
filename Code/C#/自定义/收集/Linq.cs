1.   Linq查询列值是否重复        
	//座位号唯一
	var groupList = (from s in listSeatChart group s by new {SeatName = s.SeatName } into newGroup where newGroup.Count() > 1 select newGroup.Key.SeatName).ToArray();
	if (groupList != null && groupList.Length > 0)
	{ 
			msg += "座位号{0}存在重复值;\\n".FormatWith(string.Join(",",groupList));
	}
2.左连接
	var q = from a in qCategory
		join b in qUsed on a.ID equals b.CategoryID into usedNullList
		from b in usedNullList.DefaultIfEmpty()
		join c in qUseup on a.ID equals c.CategoryID into useupNullList
		from c in useupNullList.DefaultIfEmpty()
		select new { CategoryName = a.TicketCategory, TotalQuantity = a.Quantity, UsedQuantity = b == null ? 0 : b.UsedQuantity, UseupQuantity = c == null ? 0 : c.UseupQuantity };
3.

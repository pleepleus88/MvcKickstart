﻿using System;

namespace MvcKickstart.ViewModels.Mail
{
	public abstract class EmailBase
	{
		public string To { get; set; }
		public string From { get; set; }
		public Guid TrackingId { get; set; }
	}
}
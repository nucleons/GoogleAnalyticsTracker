﻿using System.Web.Http;

namespace GoogleAnalyticsTracker.WebAPI2.v1 
{
	public static class ConfigurationHelper
    {
		public static void RegisterTracker(this HttpConfiguration configuration, string trackingAccount, string trackingDomain = null)
        {
			configuration.Filters.Add(new ActionTrackingAttribute(trackingAccount, trackingDomain));
		}

		public static void RegisterTracker(this HttpConfiguration configuration, Tracker tracker)
        {
			configuration.Filters.Add(new ActionTrackingAttribute(tracker));
		}
	}
}

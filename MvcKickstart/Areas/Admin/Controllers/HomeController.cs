﻿using System.Data;
using System.Web.Mvc;
using AttributeRouting;
using AttributeRouting.Web.Mvc;
using CacheStack;
using Google.GData.Client;
using MvcKickstart.Areas.Admin.ViewModels.Home;
using MvcKickstart.Infrastructure;
using MvcKickstart.Infrastructure.Attributes;
using MvcKickstart.Infrastructure.Extensions;
using MvcKickstart.Models;
using MvcKickstart.Services;
using ServiceStack.CacheAccess;
using Spruce;

namespace MvcKickstart.Areas.Admin.Controllers
{
	[RouteArea("admin")]
	public class HomeController : BaseController
    {
		private readonly ISiteSettingsService _siteSettingsService;
		public HomeController(IDbConnection db, IMetricTracker metrics, ICacheClient cache, ISiteSettingsService siteSettingsService) : base(db, metrics, cache)
		{
			_siteSettingsService = siteSettingsService;
		}

		[GET("", RouteName = "Admin_Home_Index")]
		[Restricted(RequireAdmin = true)]
		public ActionResult Index()
		{
			var settings = _siteSettingsService.GetSettings();
			var model = new Index
				{
					HasAnalyticsConfigured = !string.IsNullOrEmpty(settings.AnalyticsToken)
				};
			return View(model);
		}

		[GET("authResponse", RouteName = "Admin_Home_AuthResponse")]
		[Restricted(RequireAdmin = true)]
		public ActionResult AuthResponse(string token)
		{
			var sessionToken = AuthSubUtil.exchangeForSessionToken(token, null);

			var settings = _siteSettingsService.GetSettings();
			settings.AnalyticsToken = sessionToken;
			Db.Save(settings);
			Cache.Trigger(TriggerFor.Id<SiteSettings>(settings.Id));

			return RedirectToAction("Index");
		}

		#region Partials

		[Route("__partial__Menu")]
		public ActionResult Menu()
		{
			if (User.IsAdmin)
			{
				return PartialView("_Menu");
			}
			return new EmptyResult();
		}

		#endregion
	}
}

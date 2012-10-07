﻿using FizzWare.NBuilder;
using MvcKickstart.Areas.Admin;
using MvcKickstart.Areas.Admin.Controllers;
using MvcKickstart.Models.Users;
using MvcKickstart.Tests.Utilities;
using NUnit.Framework;

namespace MvcKickstart.Tests.Controllers.Admin.Home
{
	public abstract class ControllerTestBase : TestBase
	{
		protected static User User { get; private set; }
		protected HomeController Controller { get; set; }

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			AdminAreaRegistration.CreateMappings();
		}

		[SetUp]
		public override void SetUp()
		{
			base.SetUp();

			User = Builder<User>.CreateNew()
				.With(x => x.Id = null)
				.With(x => x.Username = "admin")
				.With(x => x.IsAdmin = true)
				.Build();
			Session.Store(User);
			Session.SaveChanges();

			Controller = new HomeController(Session, Metrics);
			ControllerUtilities.SetupControllerContext(Controller, User);
		}
	}
}
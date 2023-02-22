using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace LogicTests
{
	public class RecursionTest
	{
		public static Authorization AuthorizationCases => new Authorization
		{
			AuthorizationType = AuthorizationType.ClientGuid,
			Value = "JJ Keller",
			IsActive = true,
			Children = new List<Authorization>
				 {
					 new Authorization
					 {
						  AuthorizationType = AuthorizationType.ServiceGuid,
						   Value = "ApplicantManagement",
							IsActive = true,
							 Children = new List<Authorization>
							 {
								 new Authorization
								 {
									  AuthorizationType = AuthorizationType.CustomFormEdit,
									   Value = "1",
									    IsActive = true,
										 Children = new List<Authorization>(),
								 },

								 new Authorization
								 {
									  AuthorizationType = AuthorizationType.JobView,
									   Value = "1",
										IsActive = true,
										 Children = new List<Authorization>
										 {
											 new Authorization
											 {
												  AuthorizationType = AuthorizationType.JobSettingsView,
												   Value = "1",
													IsActive = true,
													 Children = new List<Authorization>()
													 {
														  new Authorization
														  {
															   AuthorizationType = AuthorizationType.DepthLevel3,
															    Value = "1",
																 IsActive = true,
																  Children = new List<Authorization>
																  {
																	   new Authorization
																	   {
																		    AuthorizationType = AuthorizationType.DepthLevel4,
																			 Value = "1",
																			  IsActive = true,
																			   Children = new List<Authorization>
																			   {
																				    new Authorization
																					{
																						 AuthorizationType = AuthorizationType.DepthLevel5,
																						  Value = "1",
																						   IsActive = true,
																						    Children = new List<Authorization>(),
																					}
																			   }
																	   },

																	   new Authorization
																	   {
																		    AuthorizationType = AuthorizationType.DepthLevel6,
																			 Value = "1",
																			  IsActive = true,
																			   Children = new List<Authorization>(),
																	   },
																  }
														  }
													 }
											 }
										 }
								 },

								 new Authorization
								 {
									  AuthorizationType = AuthorizationType.JobEdit,
									   Value = "1",
									    IsActive = true,
										 Children = new List<Authorization>
										 {
											 new Authorization
											 {
												  AuthorizationType = AuthorizationType.JobSettingsEdit,
												   Value = "1",
												    IsActive = true,
													 Children = new List<Authorization>(),
											 }
										 }
								 },
							 }
					 }
				 }
		};

		[Fact]
		public void Recursion_Test()
		{ 	
			// Not available in list
			Assert.False(AuthorizationCases.HasAuthorization(AuthorizationType.NotFound));

			// All depth levels
			Assert.True(AuthorizationCases.HasAuthorization(AuthorizationType.JobEdit, "1"));
            Assert.True(AuthorizationCases.HasAuthorization(AuthorizationType.JobSettingsView, "1"));
            Assert.True(AuthorizationCases.HasAuthorization(AuthorizationType.JobView, "1"));
            Assert.True(AuthorizationCases.HasAuthorization(AuthorizationType.JobSettingsEdit, "1"));
            Assert.True(AuthorizationCases.HasAuthorization(AuthorizationType.DepthLevel4, "1"));
            Assert.True(AuthorizationCases.HasAuthorization(AuthorizationType.DepthLevel3, "1"));
            Assert.True(AuthorizationCases.HasAuthorization(AuthorizationType.DepthLevel5, "1"));
            Assert.True(AuthorizationCases.HasAuthorization(AuthorizationType.DepthLevel6, "1"));

			Assert.True(AuthorizationCases.HasAuthorization(AuthorizationType.ClientGuid, "Jj Keller"));
            Assert.False(AuthorizationCases.HasAuthorization(AuthorizationType.ClientGuid, "TF"));

            Assert.True(AuthorizationCases.HasAuthorization(AuthorizationType.ServiceGuid, "ApplicantManagement"));

        }

		
	}

	public enum AuthorizationType
	{
		ClientGuid = 1,
		ServiceGuid = 2,
		JobView = 3,
		JobEdit = 4,
		JobSettingsView = 5,
		JobSettingsEdit = 6,
		CustomFormEdit = 7,
		NotFound = 8,
		DepthLevel3 = 9,
		DepthLevel4 = 10,
		DepthLevel5 = 11,
		DepthLevel6 = 12,
		DepthLevel7 = 13,
	}

	public class Authorization
	{
		public AuthorizationType AuthorizationType { get; set; }
		public string Value { get; set; }
		public bool IsActive { get; set; }
		public List<Authorization> Children { get; set; }

		public bool HasAuthorization(AuthorizationType authorizationType, string value = null)
		{
			if(this.AuthorizationType == authorizationType && this.IsActive && value == null)
			{
				return true;
			}

			if (this.AuthorizationType == authorizationType && this.IsActive && this.Value.Equals(value, StringComparison.InvariantCultureIgnoreCase))
			{
				return true;
			}
			
			return this.Children.Any(x => x.HasAuthorization(authorizationType, value));
		}
	}

}


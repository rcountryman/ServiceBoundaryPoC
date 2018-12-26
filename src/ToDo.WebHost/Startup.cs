using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Common.Api.Mappings;
using Common.Database;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using SequentialGuid;
using ToDo.Api.Mappings;
using ToDo.Api.Validators;

namespace ToDo.WebHost
{
	public class Startup : IStartup
	{
		private readonly IConfiguration _config;
		private readonly IHostingEnvironment _env;

		public Startup(IConfiguration config, IHostingEnvironment env)
		{
			_config = config;
			_env = env;
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
		public IServiceProvider ConfigureServices(IServiceCollection services)
		{

			services
				.AddAuthentication(
					/*o =>
					{
						o.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
						o.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
					}*/)
				.AddCookie(o =>
				{
					o.Events.OnRedirectToAccessDenied = c =>
					{
						// Redirect them to a 403 instead since this is API stuff
						c.Response.StatusCode = StatusCodes.Status403Forbidden;
						return Task.CompletedTask;
					};
					o.Events.OnRedirectToLogin = c =>
					{
						// Redirect them to a 401 instead since this is API stuff
						c.Response.StatusCode = StatusCodes.Status401Unauthorized;
						return Task.CompletedTask;
					};
				})
				.AddJwtBearer();
			services
				.AddMongoServices()
				.Configure<MongoSettings>(_config.GetSection("Mongo"))
				.AddAutoMapper(c =>
				{
					c.AddProfile<SharedMappingProfile>();
					c.AddProfile<ToDoTaskMappingProfile>();
				});

			services
				.AddMvcCore()
				.AddApiExplorer()
				.AddAuthorization()
				.AddJsonFormatters()
				.AddFluentValidation(fv =>
				{
					// TODO: Wire up Assembly Reflection
					fv.RegisterValidatorsFromAssemblyContaining<
						ToDoTaskPutValidator>();
				})
				.SetCompatibilityVersion(CompatibilityVersion.Latest);

			return services.BuildServiceProvider();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			if (_env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			app
				.UseAuthentication()
				.UseMvc()
				.UseSpa(spa =>
				{
					spa.Options.DefaultPageStaticFileOptions =
						new StaticFileOptions
						{
							OnPrepareResponse = async c =>
							{
								c.Context.Response.GetTypedHeaders()
									.CacheControl = new CacheControlHeaderValue
									{
										MustRevalidate = true,
										NoCache = true,
										NoStore = true
									};
								if (c.Context.User.Identity.IsAuthenticated)
									return;
								await c.Context.SignInAsync(
									CookieAuthenticationDefaults
										.AuthenticationScheme,
									new ClaimsPrincipal(new ClaimsIdentity(
										new List<Claim>
										{
											new Claim(ClaimTypes.Anonymous,
												"true"),
											new Claim(ClaimTypes.IsPersistent,
												"true"),
											new Claim(ClaimTypes.NameIdentifier,
												SequentialGuidGenerator.Instance
													.NewGuid().ToString())
										},
										CookieAuthenticationDefaults
											.AuthenticationScheme)),
									new AuthenticationProperties
									{
										ExpiresUtc = DateTimeOffset.MaxValue,
										IsPersistent = true,
										IssuedUtc = DateTimeOffset.UtcNow
									});
							}
						};
				});
		}
	}
}

using System;
using AutoMapper;
using Common.Api.Mappings;
using Common.Database;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
				.AddMongoServices()
				.Configure<MongoSettings>(_config.GetSection("Mongo"))
				.AddAutoMapper(c =>
				{
					c.AddProfile<SharedMappingProfile>();
					c.AddProfile<ToDoTaskMappingProfile>();
				})
				.AddMvcCore()
				.AddApiExplorer()
				.AddJsonFormatters()
				.SetCompatibilityVersion(CompatibilityVersion.Latest)
				.AddFluentValidation(fv =>
					{
						// TODO: Wire up Assembly Reflection
						fv.RegisterValidatorsFromAssemblyContaining<
							ToDoTaskPutValidator>();
					});
			return services.BuildServiceProvider();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app)
		{
			if (_env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			app
				.UseStaticFiles(new StaticFileOptions
				{
					OnPrepareResponse = async c =>
					{
						/*
						if (!c.Context.User.Identity.IsAuthenticated)
						{
							await c.Context.SignInAsync();
						}
						*/
					}
				})
				.UseMvc();
		}
	}
}

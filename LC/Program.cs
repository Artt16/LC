using LC;
using LC.Controllers;
using LC.Data.Model;
using LC.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Builder;
using static System.Collections.Specialized.BitVector32;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddMvc();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
app.UseWhen(

    context => context.Request.Path != "/{controller}/{action}",
app =>
    {
        app.Run(context => {
            context.Response.Redirect(new RedirectController()
                .RedirectToFullLink(context.Request.Path.ToString()), true); 
            return Task.CompletedTask;
        });
    });

app.MapRazorPages();

app.Run();
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
//var options = new RewriteOptions();
//options.AddRedirect(@"\w{9}$", new RedirectController().RedirectToFullLink().ToString());
//app.UseRewriter(options);

if (!app.Environment.IsDevelopment())
{
	app.UseDeveloperExceptionPage();
}
else
{ 
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.    
    app.UseHsts(); //adds the Strict-Transport-Security header.    
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
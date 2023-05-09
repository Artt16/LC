﻿using LC.Controllers;
using LC.Data;
using LC.Data.Model;
using LC.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using System;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LCDbContext>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();


app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();

    endpoints.MapControllers();
    endpoints.MapControllerRoute(
        name: "RedirectShortLink",
        pattern: "{shortLink}",
        defaults: new { controller = typeof(RedirectController).Name, action = nameof(RedirectController.RedirectToLongLink) });
});

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<LCDbContext>();
    db.Database.EnsureCreated();
}
app.UseStaticFiles();
app.Run();

// See https://aka.ms/new-console-template for more information
using System.Collections.Generic;
using gp_unisis.Database;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, Efe!");

var db = new ApplicationDbContext();
db.Database.EnsureCreated();
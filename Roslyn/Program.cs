using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Roslyn
{
	class Program
	{

		//Ref.:https://www.strathweb.com/2018/01/easy-way-to-create-a-c-lambda-expression-from-a-string-with-roslyn/
		//===================================================
		//https://stackoverflow.com/questions/38114761/asp-net-core-configuration-for-net-core-console-application
		//===================================================
		//https://docs.microsoft.com/hu-hu/aspnet/core/fundamentals/configuration/index?tabs=basicconfiguration#simple-configuration

		private static IConfigurationRoot Configuration { get; set; }

		static async Task Main(string[] args)
		{
			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json");

			Configuration = builder.Build();
			//String value1 = Configuration["myKey1"];
			//String value2 = Configuration.GetSection("filter").Value;
			//Console.WriteLine(value1);
			//Console.WriteLine(value2);

			Album a = new Album();
			a.Init();

			// ================ Normal way ============================
			var parameter = Expression.Parameter(typeof(Album), "album");
			var comparison = Expression.GreaterThan(Expression.Property(parameter,
				Type.GetType("Roslyn.Album").GetProperty("Quantity")),
				Expression.Constant(100));
			Func<Album, Boolean> discountFilterExpression = Expression.Lambda<Func<Album, Boolean>>(comparison, parameter).Compile();

			//var discountedAlbums = a.albums.Where(discountFilterExpression);
			List<Album> discountedAlbums = new List<Album>();
			discountedAlbums = a.albums.Where(discountFilterExpression).ToList();

			Console.WriteLine(discountedAlbums.Count());
			// ========================================================

			//Task.Run(async () => p.test(a));

			//var discountFilter = "album => album.Quantity > 100";
			var filter1 = Configuration["albumFilter"];
			var filter2 = Configuration["albumFilter2"];

			await FilterList(filter1, a.albums);
			Console.WriteLine("\nFilter2\n");
			await FilterList(filter2, a.albums);

			Console.ReadKey();

		}

		static async Task FilterList(String filter, List<Album> albums)
		{
			var options = ScriptOptions.Default.AddReferences(typeof(Album).Assembly);

			Func<Album, Boolean> filterExpression = await CSharpScript.EvaluateAsync<Func<Album, Boolean>>(filter, options);

			var filteredAlbums = albums.Where(filterExpression).ToList();

			WriteInfo(filteredAlbums);
		}

		static void WriteInfo(List<Album> albums)
		{
			Console.WriteLine("Filtered albums count: {0}", albums.Count());
			foreach (var album in albums)
			{
				Console.WriteLine("Title: {0}, | Quantity: {1}, | Artist: {2}",
					album.Title,
					album.Quantity,
					album.Artist);
				Console.WriteLine("=======================================");
			}
		}

	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Roslyn
{
	public class Album
	{
		public List<Album> albums;

		public int Quantity { get; set; }
		public string Title { get; set; }
		public string Artist { get; set; }

		public void Init()
		{
			albums = new List<Album>
			{
				new Album { Quantity = 10, Artist = "Betontod", Title = "Revolution" },
				new Album { Quantity = 50, Artist = "The Dangerous Summer", Title = "The Dangerous Summer" },
				new Album { Quantity = 60, Artist = "Depeche Mode", Title = "Spirit" },
				new Album { Quantity = 80, Artist = "Depeche Mode", Title = "Construction Time Again" },
				new Album { Quantity = 100, Artist = "Depeche Mode", Title = "Songs of Faith and Devotion" },
				new Album { Quantity = 200, Artist = "Queen", Title = "A Night at the Opera" },
				new Album { Quantity = 300, Artist = "Queen", Title = "A Kind of Magic" },
			};
		}

	}
}

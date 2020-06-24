			var a =
				from x in new List<List<int>>(1000)
				from g in new List<Random>(1000)
				group x by g.Next()
				into randGroup
				from k in randGroup
				orderby k ascending
				where randGroup.Select(s=>s.Average()).Sum() / (double)randGroup.Count() <k.Average()
				select k;


			foreach(var i in a) foreach(var j in i) Console.WriteLine(j);

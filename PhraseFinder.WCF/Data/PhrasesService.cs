using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;

namespace PhraseFinder.WCF.Data
{
	internal class Phrase
	{
		public int PhraseId { get; set; }
		public string Value { get; set; }
	}


	internal class PhrasesService
	{
		private readonly string _dbPath;

		public PhrasesService()
		{
			const Environment.SpecialFolder localAppDataFolder = Environment.SpecialFolder.ApplicationData;
			var dbDirectory = Path.Combine(Environment.GetFolderPath(localAppDataFolder), "PhraseFinder");
			Directory.CreateDirectory(dbDirectory);
			_dbPath = Path.Combine(dbDirectory, "expresiones-y-locuciones.accdb");
		}

		public IEnumerable<Phrase> GetPhrases()
		{
			using (var dbConnection = new OleDbConnection($"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={_dbPath}"))
			{
				dbConnection.Open();
				var command = new OleDbCommand("SELECT * FROM [Expresiones y locuciones]", dbConnection);
				using (var reader = command.ExecuteReader())
				{
					while (reader.Read())
					{

						yield return new Phrase
						{
							PhraseId = (int)reader[0],
							Value = reader[1] as string ?? string.Empty
						};
					}
				}
				dbConnection.Close();
			}
		}
	}
}
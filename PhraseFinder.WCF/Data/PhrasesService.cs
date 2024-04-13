using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.IO;
using Dapper;

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
				return dbConnection.Query<Phrase>(
					"SELECT [ID_Locucion] as [PhraseId], [Locucion] as [Value] FROM Locuciones_y_expresiones;");

			}
		}
	}
}
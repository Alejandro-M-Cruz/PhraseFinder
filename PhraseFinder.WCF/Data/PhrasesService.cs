﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Dapper;

namespace PhraseFinder.WCF.Data
{
	internal class Phrase
	{
		public int PhraseId { get; set; }
		public string Value { get; set; }
	}

    internal class PhraseDefinitionExample
    {
        public int PhraseId { get; set; }
        public string Definition { get; set; }
        public string Example { get; set; }
    }

	internal class PhrasesService : IDisposable
	{
		private static readonly string ConnectionString;
        private readonly IDbConnection _dbConnection;
        private IDictionary<int, PhraseDefinition[]> PhraseDefinitions { get; set; }

        static PhrasesService()
        {
            const Environment.SpecialFolder localAppDataFolder = Environment.SpecialFolder.ApplicationData;
            var dbDirectory = Path.Combine(Environment.GetFolderPath(localAppDataFolder), "PhraseFinder");
            Directory.CreateDirectory(dbDirectory);
            var dbPath = Path.Combine(dbDirectory, "expresiones-y-locuciones.accdb");
            ConnectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath}";
        }

        public PhrasesService()
        {
            _dbConnection = new OleDbConnection(ConnectionString);
            _dbConnection.Open();
        }

        public IEnumerable<Phrase> GetPhrases()
		{
			return _dbConnection.Query<Phrase>(
				"select [ID_Locucion] as [PhraseId], [Locucion] as [Value] from Locuciones_y_expresiones;");
		}

        public void LoadPhraseDefinitions(IEnumerable<int> phraseIds)
        {
            var phraseDefinitionExamples = _dbConnection.Query<PhraseDefinitionExample>(
                "select D.[ID_Locucion] as [PhraseId], D.[Definicion] as [Definition], E.[Ejemplo] as [Example] " +
                "from Definiciones as D " +
                "left join Ejemplos as E on D.[ID_Definicion] = E.[ID_Definicion] " +
                "where D.ID_Locucion in @PhraseIds",
                new { PhraseIds = phraseIds });
            PhraseDefinitions = phraseDefinitionExamples
                .GroupBy(pde => pde.PhraseId)
                .ToDictionary(
                    g => g.Key,
                    grouping => grouping
                        .GroupBy(pde => pde.Definition)
                        .Select(g => new PhraseDefinition
                        { 
                            Definition = g.Key, 
                            Examples = g.Select(pde => pde.Example).Where(e => e != null).ToArray()
                        }).ToArray());
        }

        public PhraseDefinition[] GetPhraseDefinitions(int phraseId)
        {
            return PhraseDefinitions[phraseId];
        }

        public void Dispose()
        {
            _dbConnection?.Dispose();
        }
    }
}
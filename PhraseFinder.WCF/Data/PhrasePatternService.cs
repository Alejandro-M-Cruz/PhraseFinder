using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using Dapper;
using PhraseFinder.WCF.Contracts;
using PhraseFinder.WCF.Models;

namespace PhraseFinder.WCF.Data
{
    internal class PhrasePatternService : IDisposable
	{
        private const string PhrasePatternQuery = @"
select 
    [Locucion_o_expresion] as [Phrase],
    [Patron] as [Pattern], 
    [Palabra_base] as [BaseWord], 
    [ID_Locucion] as [PhraseId] 
from Patrones;";

        private const string PhraseDefinitionExampleQuery = @"
select 
    D.[ID_Locucion] as [PhraseId], 
    D.[Definicion] as [Definition], 
    E.[Ejemplo] as [Example] 
from 
    Definiciones as D left join Ejemplos as E on D.[ID_Definicion] = E.[ID_Definicion] 
where 
    D.ID_Locucion in @PhraseIds;";

        private struct PhraseDefinitionExample
        {
            public int PhraseId { get; set; }
            public string Value { get; set; }
            public string Definition { get; set; } 
            public string Example { get; set; }
        }

        private static readonly string ConnectionString = 
            ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString;
        private readonly IDbConnection _dbConnection;
        private IDictionary<int, PhraseDefinition[]> PhraseIdToDefinitions { get; set; } =
            new Dictionary<int, PhraseDefinition[]>();

        public PhrasePatternService()
        {
            _dbConnection = new OleDbConnection(ConnectionString);
            _dbConnection.Open();
        }

        public IEnumerable<PhrasePattern> GetPhrasePatterns()
		{
			return _dbConnection.Query<PhrasePattern>(PhrasePatternQuery);
		}

        public void LoadPhraseDefinitions(IReadOnlyCollection<int> phraseIds)
        {
            if (phraseIds.Count == 0)
            {
                PhraseIdToDefinitions.Clear();
                return;
            }

            var phraseDefinitionExamples = _dbConnection.Query<PhraseDefinitionExample>(
                PhraseDefinitionExampleQuery,
                new { PhraseIds = phraseIds });
            PhraseIdToDefinitions = phraseDefinitionExamples
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
            var hasDefinitions = PhraseIdToDefinitions.TryGetValue(phraseId, out var definitions);
            return hasDefinitions ? definitions : Array.Empty<PhraseDefinition>();
        }

        public void Dispose()
        {
            _dbConnection.Close();
            _dbConnection.Dispose();
        }
    }
}
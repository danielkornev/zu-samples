using System.Collections.Generic;
using ZU.Core;
using ZU.Samples.Text.Extraction.KeyphraseExtractionProvider.Analysis;
using ZU.Semantic.Keyphrases;

namespace ZU.Samples.Text.Extraction.KeyphraseExtractionProvider.Extraction
{
	public static class KeyphrasesExtractor
	{
		private static int defaultNumTerms = 100;
		private static int maxResults = 100;

		public static int DefaultNumTerms
		{
			get
			{ return defaultNumTerms; }
			set
			{ defaultNumTerms = value; }
		}

		public static bool firstLoad = true;

		internal static bool TryGetKeyphrases(IEntity entity, string fullText, int numberOfKeyphrases, out List<IKeyphrase> keyphrases)
		{
			keyphrases = new List<IKeyphrase>();
			bool result = false;

			if (string.IsNullOrEmpty(fullText))
				return false;

			if (numberOfKeyphrases < 0)
				numberOfKeyphrases = DefaultNumTerms;

			KeyphraseAnalyzer ka = new KeyphraseAnalyzer(entity.ModelContext.SIM);
			int truncateAt = 0;
			var keyphrases1 = ka.Extract(fullText, 50, 20, out truncateAt);
			if (keyphrases1.Count > 0)
			{
				int counter = 0;
				foreach (var k in keyphrases1)
				{
					if (counter > numberOfKeyphrases)
						break;

					// we add only first N keyphrases
					keyphrases.Add(k);
					counter++;
				}

				result = true;
			}

			return result;
		}
	} // class
} // namespace

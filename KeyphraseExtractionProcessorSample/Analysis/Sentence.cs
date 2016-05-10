using System.Collections.Generic;

namespace ZU.Samples.Text.Extraction.KeyphraseExtractionProvider.Analysis
{
	internal class Sentence
	{
		internal List<Word> Words { get; set; }

		internal Sentence() { Words = new List<Word>(); }
	} // class
} // namespace

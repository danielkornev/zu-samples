using System.Collections.Generic;

namespace ZU.Samples.Text.Extraction.KeyphraseExtractionProvider.Analysis
{
	internal class Paragraph
	{
		internal List<Sentence> Sentences { get; set; }

		internal Paragraph() { Sentences = new List<Sentence>(); }
	} // class
} // namespace

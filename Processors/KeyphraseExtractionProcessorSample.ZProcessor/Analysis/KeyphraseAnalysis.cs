using System.Collections.Generic;
using ZU.Semantic.Keyphrases;

namespace ZU.Samples.Text.Extraction.KeyphraseExtractionProvider.Analysis
{
	internal class KeyphraseAnalysis
	{
		internal string Content { get; set; }
		internal int WordCount { get; set; }
		internal IEnumerable<IKeyphrase> Keyphrases { get; set; }
		internal List<Paragraph> Paragraphs { get; set; }
		internal IEnumerable<Title> Titles { get; set; }
	} // class
} // namespace
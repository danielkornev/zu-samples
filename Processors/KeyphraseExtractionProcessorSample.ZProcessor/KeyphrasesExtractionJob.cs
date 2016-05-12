using System;
using System.Collections.Generic;
using ZU.Core;
using ZU.Samples.Text.Extraction.KeyphraseExtractionProvider.Extraction;
using ZU.Semantic.Keyphrases;

namespace ZU.Samples.Text.Extraction.KeyphraseExtractionProvider
{
	public class KeyphrasesExtractionJob
	{
		public Action<IEntity, List<IKeyphrase>, bool> OnKeyphraseExtractionJobCompleted { get; internal set; }
		public IEntity Entity { get; internal set; }
		private int NumOfTerms;

		public KeyphrasesExtractionJob(IEntity entity, int numOfTerms)
		{
			this.Entity = entity;
			this.NumOfTerms = numOfTerms;
		}



		internal void Start()
		{
			if (Entity != null && Entity.ModelContext != null)
			{
				string fullText = Entity.FullText;
				// we need to start extracting keyphrases
				var keyphrases = new List<IKeyphrase>();

				// actual work
				bool succeeded = KeyphrasesExtractor.TryGetKeyphrases(this.Entity, fullText, this.NumOfTerms, out keyphrases);

				// reporting back to the Central
				if (this.OnKeyphraseExtractionJobCompleted != null)
				{
					this.OnKeyphraseExtractionJobCompleted(this.Entity, keyphrases, succeeded);
				}
			}

			IsEnded = true;
		}

		public DateTime StartTime = DateTime.Now;
		public int MaxSecs = 120;

		public bool IsEnded = false;
		public bool IsOvertime
		{
			get
			{
				return (DateTime.Now - StartTime).TotalSeconds > MaxSecs;
			}
		}

		public void Stop()
		{
			// we can't really cancel this job, right?
			IsEnded = true;
		}
	} // class
} // namespace
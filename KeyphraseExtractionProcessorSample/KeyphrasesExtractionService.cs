using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZU.Core;
using ZU.Semantic.Keyphrases;

namespace ZU.Samples.Text.Extraction.KeyphraseExtractionProvider
{
	public static class KeyphrasesExtractionService
	{
		public static Action<IEntity, List<IKeyphrase>, bool> OnKeyphraseExtractionCompleted;

		#region Singleton pattern

		#endregion

		#region Service pattern
		public static void Start()
		{
			stopped = false;
			// Create and start Client
			Thread thread = new Thread(new ThreadStart(Loop));
			thread.Priority = ThreadPriority.Lowest; // Below Normal

			//Thread thread = new Thread(new ParameterizedThreadStart(Loop));
			// we send our local FT_Index path to the Loop Thread
			thread.Start();
		}

		#region Public methods
		public static void Add(params KeyphrasesExtractionJob[] jobs)
		{
			if (jobs != null)
				foreach (var j in jobs)
					//ToAdd.Add(j);
					ToAdd.Enqueue(j); // we add it to the queue
		}
		#endregion

		public static void Stop()
		{
			stopped = true;
			Thread.Sleep(250);
		}
		#endregion


		#region Public fields
		public static int Count = 0;
		#endregion

		#region Implementation
		private static bool stopped = false;
		// we use Queue instead of List to get FIFO (First-In/First-Out) effect
		public static Queue<KeyphrasesExtractionJob> ToAdd = new Queue<KeyphrasesExtractionJob>();
		static List<KeyphrasesExtractionJob> Jobs = new List<KeyphrasesExtractionJob>();

		private static void Loop()
		{
			// internal lists

			List<int> toDel = new List<int>();

			// Main loop
			while (!stopped)
			{
				// Update jobs
				lock (ToAdd)
				{
					// Add new jobs only if current list of jobs is zero
					while (ToAdd.Count > 0 && Jobs.Count == 0)
					{
						//var j = ToAdd[ToAdd.Count - 1];

						// obtaining first available element
						var j = ToAdd.Dequeue();

						if (j != null)
						{
							j.OnKeyphraseExtractionJobCompleted += j_OnKeyphraseExtractionJobCompleted;

							Jobs.Add(j);
							Log.Message(Log.Level.Info, "+ [KeyphrasesExtractionService] Job added \"{0}\"", j.Entity.Id);

							j.Start(); // this happens synchronously, right?
						}

						if (ToAdd.Count == 0)
						{
							Log.Message(Log.Level.Info, "= [KeyphrasesExtractionService] Indexing finished!");
						}
					}
				}
				// Stop ended/overtime jobs
				for (int i = 0; i < Jobs.Count; i++)
				{
					var j = Jobs[i];
					if ((j != null) && (j.IsEnded || j.IsOvertime))
					{
						var isOver = j.IsOvertime;
						j.Stop();
						toDel.Add(i);
						if (isOver)
						{
							Log.Message(Log.Level.Info, "- [KeyphrasesExtractionService] Job stopped \"{0}\"", j.Entity.Id);
						}
						else
						{
							Log.Message(Log.Level.Info, "= [KeyphrasesExtractionService] Job ended \"{0}\"", j.Entity.Id);
						}

						Log.Info("---------------------------------------------------------------------------");
					}
				}
				// Delete ended jobs
				while (toDel.Count > 0)
				{
					var i = toDel[toDel.Count - 1];
					Jobs.RemoveAt(i);
					toDel.RemoveAt(toDel.Count - 1);
				}
				// Clear toDel
				toDel.Clear();
				// Update Count
				Count = Jobs.Count;
				//
				Thread.Sleep(100);
			}
			Thread.Sleep(100);
			// nothing to shutdown, right?

		}//loop()

		static void j_OnKeyphraseExtractionJobCompleted(ZU.Core.IEntity entity, List<IKeyphrase> keyphrases, bool succeeded)
		{
			// notifying the central
			OnKeyphraseExtractionCompleted?.Invoke(entity, keyphrases, succeeded);
		}
		#endregion

		public static bool IsBusy
		{
			get
			{
				if (Jobs.Count > 0)
					return true;
				return false;
			}
		}

		public static bool IsStopped
		{
			get
			{
				return stopped;
			}
		}
	} // class
} // namespace
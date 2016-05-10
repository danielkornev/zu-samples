using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAL.Flatbed;
using ZU.Core;
using ZU.Plugins;
using ZU.Semantic.Processors;
using ZU.Semantic.Keyphrases;

namespace ZU.Samples.Text.Extraction.KeyphraseExtractionProvider
{
	public partial class KeyphraseExtractionSampleProcessor : IZetProcessor
	{
		#region Members
		private ISemanticPipelineProcessor processor;
		internal IZetHost ZetHost;

		internal bool subscribed;
		#endregion

		public IHost Host
		{
			get;set;
		}

		public string Name
		{
			get;set;
		}

		public string Status
		{
			get
			{
				if (subscribed == false)
					return "Disabled";

				if (!KeyphrasesExtractionService.IsStopped)
				{
					if (KeyphrasesExtractionService.IsBusy)
						return "Extracting Keyphrases";
					else
						return "Idle";
				}
				return "Stopped";
			}
		}

		public object Invoke(string message, params object[] args)
		{
			return null;
		}

		public bool OnConnection(ConnectMode mode)
		{
			if (this.Host == null) throw new NotImplementedException("Host is not available");

			this.ZetHost = this.Host as IZetHost;

			if (this.ZetHost == null) throw new PlatformNotSupportedException("This build of Zet Universe platform is not supported. Host doesn't implement ZU.Plugins.IZetHost interface");

			// obtaining ISemanticPipelineProcessor
			var processor = this.ZetHost.SIM.SemanticPipelineProcessor;

			if (processor == null) throw new PlatformNotSupportedException("This build of Zet Universe platform is not supported. Host doesn't provide access to a functioning Semantic Processor");

			// Cache the interface for later use
			this.processor = processor;

			this.Name = "Sample Keyphrase Extraction Processor";

			// KeyphraseExtractionService
			KeyphrasesExtractionService.OnKeyphraseExtractionCompleted = OnJobCompleted;
			KeyphrasesExtractionService.Start();

			this.processor.RegisterProcessor(this);

			subscribed = processor.Subscribe(new OnPublishedDelegate(OnFullTextExtracted), Constants.Topics.Properties.FullText, this.Name);


			return true;
		}

		/// <summary>
		/// This callback is called by the host when full text has been extracted from the given entity
		/// </summary>
		/// <param name="Entity"></param>
		private void OnFullTextExtracted(IEntity Entity)
		{
			// TO DO:
		}

		private void OnJobCompleted(IEntity entity, List<IKeyphrase> keyphrases, bool succeeded)
		{
			if (succeeded)
			{
				// setting new keyphrases; Semantic Processor in the Platform will intelligently process the newly obtained keyphrases on its own
				entity.NewKeyphrases = keyphrases;

				// publishing back
				processor.Publish(entity, Constants.Topics.Properties.Keyphrases);
				processor.ReportSuccessfulProcessing(entity, Constants.Topics.Properties.FullText);
			}
			else
			{
				// he's dead, Jim
				processor.ReportFailedProcessing(entity, Constants.Topics.Properties.FullText);
			}
		}

		/// <summary>
		/// Not used for now
		/// </summary>
		/// <param name="mode"></param>
		/// <returns></returns>
		public bool OnDisconnection(DisconnectMode mode)
		{
			this.Shutdown();

			return true;
		}

		public void Shutdown()
		{
			KeyphrasesExtractionService.Stop();
		}
	} // class
} // namespace

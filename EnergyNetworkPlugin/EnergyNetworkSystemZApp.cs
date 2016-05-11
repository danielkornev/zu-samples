using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAL.Flatbed;
using ZU.Configuration.Settings;
using ZU.Core.Apps;
using ZU.Plugins;
using ZU.Core;
using System.Windows;
using System.Windows.Media.Imaging;
using ZU;

namespace EnergyNetworkSystem
{
	public partial class EnergyNetworkSystemZApp : IZetApp
	{
		#region Members
		internal ZU.Configuration.Settings.IAppSettings _settings;
		internal ISystemInformationModel _SIM;
		private bool _IsInitialized;
		private string _Status;
		internal IZetHost ZetHost;
		private AppPaneBase _appPane;
		private IAppButtonInfo _appButtonInfo;
		private IAppManager _appManager;

		public EnergyNetworkSystemZApp Instance { get; private set; }

		internal delegate void AppLoadedEventHandler(object sender, RoutedEventArgs e);

		//internal static StickyNotesApp Instance;
		#endregion

		#region Events

		public event AppAccountAddedEventHandler Added;

		internal event AppLoadedEventHandler Loaded;

		public event OnInformationStructureChangedEventHandler OnInformationStructureChanged;
	
		#endregion

		
		


		private void Initialize(IAppSettings settings, ISystemInformationModel sim, IAppManager appManager)
		{
			this._settings = settings;
			this._SIM = sim;
			this._appManager = appManager;

			// Singleton for poors
			Instance = this;

			this._appButtonInfo = appManager.CreateAppButtonInfo("M30,42L27.5917949676514,42.2402381896973 25.3359375,42.9609375 21.5390625,45.5390625 18.9609375,49.359375 18.240234375,51.62109375 18,54 18.240234375,56.408203125 18.9609375,58.6640625 21.5390625,62.4609375 25.3359375,65.0390625 27.5917949676514,65.759765625 30,66 32.37890625,65.759765625 34.640625,65.0390625 38.4609375,62.4609375 41.0390625,58.6640625 41.759765625,56.408203125 42,54 41.759765625,51.62109375 41.0390625,49.359375 38.4609375,45.5390625 34.640625,42.9609375 32.37890625,42.2402381896973 30,42z M66,5.99999952316284L63.591796875,6.240234375 61.3359375,6.96093702316284 57.5390625,9.53906345367432 54.9609375,13.359375 54.2402305603027,15.62109375 54,18 54.2402305603027,20.408203125 54.9609375,22.6640625 57.5390625,26.4609375 61.3359375,29.0390625 63.591796875,29.759765625 66,30 68.37890625,29.759765625 70.640625,29.0390625 74.4609375,26.4609375 77.0390625,22.6640625 77.759765625,20.408203125 78,18 77.759765625,15.62109375 77.0390625,13.359375 74.4609375,9.53906345367432 70.640625,6.96093702316284 68.37890625,6.240234375 66,5.99999952316284z M66,0L69.59765625,0.357421815395355 72.984375,1.42968714237213 76.0546875,3.111328125 78.703125,5.29687452316284 80.888671875,7.9453125 82.5703125,11.015625 83.642578125,14.40234375 84,18 83.513671875,22.16015625 82.0546875,26.109375 79.740234375,29.625 76.6875,32.484375 82.1953125,36.3046875 86.390625,41.3671875 87.92578125,44.2617225646973 89.0625,47.3671875 89.765625,50.630859375 90,54 84,54 83.642578125,50.40234375 82.5703125,47.015625 80.888671875,43.9453163146973 78.703125,41.296875 76.0546875,39.1113319396973 72.984375,37.4296875 69.59765625,36.3574256896973 66,36 62.4023399353027,36.3574256896973 59.015625,37.4296875 55.9453086853027,39.1113319396973 53.296875,41.296875 51.1113243103027,43.9453163146973 49.4296875,47.015625 48.357421875,50.40234375 48,54 47.513671875,58.16015625 46.0546875,62.109375 43.740234375,65.625 40.6875,68.484375 46.1953125,72.3046875 50.390625,77.3671875 51.9257774353027,80.26171875 53.0625,83.3671875 53.7656211853027,86.630859375 54,90 48,90 47.642578125,86.40234375 46.5703125,83.015625 44.888671875,79.9453125 42.703125,77.296875 40.0546875,75.111328125 36.984375,73.4296875 33.59765625,72.357421875 30,72 26.4023418426514,72.357421875 23.015625,73.4296875 19.9453125,75.111328125 17.296875,77.296875 15.1113271713257,79.9453125 13.4296875,83.015625 12.357421875,86.40234375 11.9999990463257,90 6,90 6.234375,86.630859375 6.93750047683716,83.3671875 8.07421875,80.26171875 9.609375,77.3671875 13.8046875,72.3046875 19.3125,68.484375 16.259765625,65.625 13.9453134536743,62.109375 12.486328125,58.16015625 11.9999990463257,54 12.357421875,50.40234375 13.4296875,47.015625 15.1113271713257,43.9453163146973 17.296875,41.296875 19.9453125,39.1113319396973 23.015625,37.4296875 26.4023418426514,36.3574256896973 30,36 34.16015625,36.4863319396973 38.109375,37.9453125 41.625,40.2597694396973 44.484375,43.3125 46.51171875,39.9843788146973 49.03125,37.03125 51.9843711853027,34.5117225646973 55.3125,32.484375 52.2597618103027,29.625 49.9453125,26.109375 48.486328125,22.16015625 48,18 48.357421875,14.40234375 49.4296875,11.015625 51.1113243103027,7.9453125 53.296875,5.29687452316284 55.9453086853027,3.111328125 59.015625,1.42968714237213 62.4023399353027,0.357421815395355 66,0z", @"Show/Hide People and Groups pane");

			if (this.Accounts == null)
			{
				this.Accounts = new List<IAppAccount>();

				CreateDefaultAccount();
			}
			else
			{
				if (this.Accounts.Count == 0)
				{
					CreateDefaultAccount();
				}
				else if (this.Accounts.Count == 1)
				{
					var account = this.Accounts.FirstOrDefault();

					account.App = this;
				}
			}

			// creating default app pane
			//this._appPane = new AppPane(this.Accounts.FirstOrDefault());

			RegisterWithAppManager();



			if (this.Loaded != null)
				this.Loaded(this, new RoutedEventArgs());

			this._IsInitialized = true;
		}

		private void RegisterWithAppManager()
		{
			_appManager.RegisterApp(this as IZetApp);

			RegisterKinds();

			RegisterRelationships();
		}

		private void RegisterRelationships()
		{

			// ConnectedWith
			var connectedWith = new AppRelationshipDefinition();
			connectedWith.AppId = this.Id.ToString();
			connectedWith.DescriptionFrom = "connects with";
			connectedWith.Description = "connects with";
			connectedWith.Relation = "IsConnectedWith";

			connectedWith.AllowedParticipatingKinds.Add(new Tuple<string, string>(EnergyNetworkSystemConstants.Kinds.TechnicalLocation, EnergyNetworkSystemConstants.Kinds.TechnicalLocation));

			_appManager.RegisterRelationship(connectedWith);

			// Member of
			var memberOfEnergyNetworkItem = new AppRelationshipDefinition();
			memberOfEnergyNetworkItem.AppId = this.Id.ToString();
			memberOfEnergyNetworkItem.DescriptionFrom = "has member";
			memberOfEnergyNetworkItem.Description = "member of";
			memberOfEnergyNetworkItem.Relation = "IsMemberOf";

			memberOfEnergyNetworkItem.AllowedParticipatingKinds.Add(new Tuple<string, string>(EnergyNetworkSystemConstants.Kinds.TechnicalLocation, EnergyNetworkSystemConstants.Kinds.EquipmentItem));
			memberOfEnergyNetworkItem.AllowedParticipatingKinds.Add(new Tuple<string, string>(EnergyNetworkSystemConstants.Kinds.EquipmentItem, EnergyNetworkSystemConstants.Kinds.EquipmentItem));
			memberOfEnergyNetworkItem.AllowedParticipatingKinds.Add(new Tuple<string, string>(EnergyNetworkSystemConstants.Kinds.TechnicalLocation, EnergyNetworkSystemConstants.Kinds.TechnicalLocation));

			_appManager.RegisterRelationship(memberOfEnergyNetworkItem);

			// Topological Member of
			var topologicalMemberOf = new AppRelationshipDefinition();
			topologicalMemberOf.AppId = this.Id.ToString();
			topologicalMemberOf.DescriptionFrom = "has topological member";
			topologicalMemberOf.Description = "topological member of";
			topologicalMemberOf.Relation = "IsTopologicalMemberOf";

			topologicalMemberOf.AllowedParticipatingKinds.Add(new Tuple<string, string>(EnergyNetworkSystemConstants.Kinds.TechnicalLocation, EnergyNetworkSystemConstants.Kinds.TechnicalLocation));

			_appManager.RegisterRelationship(topologicalMemberOf);

			// Described by
			var describedBy = new AppRelationshipDefinition();
			describedBy.AppId = this.Id.ToString();
			describedBy.Description = "described by";
			describedBy.DescriptionFrom = "describes";
			describedBy.Relation = "IsDescribedBy";

			describedBy.AllowedParticipatingKinds.Add(new Tuple<string, string>(EnergyNetworkSystemConstants.Kinds.TechnicalLocation, EnergyNetworkSystemConstants.Kinds.EquipmentItem));

			_appManager.RegisterRelationship(describedBy);


			//var managerOfAgent = new AppRelationshipDefinition();
			//managerOfAgent.AppId = this.Id.ToString();
			//managerOfAgent.DescriptionFrom = "manages";
			//managerOfAgent.Description = "managed by";
			//managerOfAgent.Relation = "IsManagerOf";

			//managerOfAgent.AllowedParticipatingKinds.Add(new Tuple<string, string>(Constants.Kinds.Organization, Constants.Kinds.Organization));
			//managerOfAgent.AllowedParticipatingKinds.Add(new Tuple<string, string>(Constants.Kinds.PeopleGroup, Constants.Kinds.Organization));
			//managerOfAgent.AllowedParticipatingKinds.Add(new Tuple<string, string>(Constants.Kinds.Person, Constants.Kinds.Organization));
			//managerOfAgent.AllowedParticipatingKinds.Add(new Tuple<string, string>(Constants.Kinds.Person, Constants.Kinds.PeopleGroup));

			//_appManager.RegisterRelationship(managerOfAgent);
		}


		private void RegisterKinds()
		{
			// Energy Network Item: Root Kind
			AppEntityKindDefinition energyNetworkItem = new AppEntityKindDefinition();

			energyNetworkItem.AppId = this.Id;
			energyNetworkItem.AppTitle = this.Title;
			energyNetworkItem.DisplayNamePlural = "energy network items";
			energyNetworkItem.DisplayNameSingular = "energy network item";
			energyNetworkItem.FullTextAliases = "Energy Network Item Items";
			energyNetworkItem.Kind = EnergyNetworkSystemConstants.Kinds.EnergyNetworkItem;
			energyNetworkItem.ParentKind = Constants.Kinds.Kind;
			energyNetworkItem.CanBePartOfVisualCluster = true;
			energyNetworkItem.CanParticipateInTopographicClusterVisualization = true;
			//dropboxFolderAppKind.Template = dt;
			energyNetworkItem.DefaultThumbnail = new BitmapImage(new Uri(@"pack://application:,,,/EnergyNetworkSystem.ZApp;component/Images/TechnicalLocation.png"));


			// we also need to register our app: it should be able to open needed entities
			_appManager.RegisterKind(OpenEnergyNetworkItemEntity, energyNetworkItem);


			// Technical Location
			AppEntityKindDefinition technicalLocation = new AppEntityKindDefinition();

			technicalLocation.AppId = this.Id;
			technicalLocation.AppTitle = this.Title;
			technicalLocation.DisplayNamePlural = "technical locations";
			technicalLocation.DisplayNameSingular = "technical location";
			technicalLocation.FullTextAliases = "technical location locations";
			technicalLocation.Kind = EnergyNetworkSystemConstants.Kinds.TechnicalLocation;
			technicalLocation.ParentKind = EnergyNetworkSystemConstants.Kinds.EnergyNetworkItem;
			technicalLocation.CanBePartOfVisualCluster = true;
			technicalLocation.CanParticipateInTopographicClusterVisualization = true;
			//dropboxFolderAppKind.Template = dt;
			technicalLocation.DefaultThumbnail = new BitmapImage(new Uri(@"pack://application:,,,/EnergyNetworkSystem.ZApp;component/Images/TechnicalLocation.png"));


			// we also need to register our app: it should be able to open needed entities
			_appManager.RegisterKind(OpenTechnicalLocationEntity, technicalLocation);


			// Equipment Item
			AppEntityKindDefinition equipmentItem = new AppEntityKindDefinition();

			equipmentItem.AppId = this.Id;
			equipmentItem.AppTitle = this.Title;
			equipmentItem.DisplayNamePlural = "equipment items";
			equipmentItem.DisplayNameSingular = "equipment item";
			equipmentItem.FullTextAliases = "equipment item items";
			equipmentItem.Kind = EnergyNetworkSystemConstants.Kinds.EquipmentItem;
			equipmentItem.ParentKind = EnergyNetworkSystemConstants.Kinds.EnergyNetworkItem;
			equipmentItem.CanBePartOfVisualCluster = true;
			equipmentItem.CanParticipateInTopographicClusterVisualization = true;
			//dropboxFolderAppKind.Template = dt;
			equipmentItem.DefaultThumbnail = new BitmapImage(new Uri(@"pack://application:,,,/EnergyNetworkSystem.ZApp;component/Images/EquipmentItem.png"));


			// we also need to register our app: it should be able to open needed entities
			_appManager.RegisterKind(OpenEquipmentItem, equipmentItem);
		}

		private void OpenTechnicalLocationEntity(IEntity entity)
		{
			MessageBox.Show("Not Yet Implemented", this.Title);
		}

		private void OpenEquipmentItem(IEntity entity)
		{
			MessageBox.Show("Not Yet Implemented", this.Title);
		}

		private void OpenEnergyNetworkItemEntity(IEntity entity)
		{
			MessageBox.Show("Not Yet Implemented", this.Title);
		}

		private void CreateDefaultAccount()
		{
			IAppAccount account = _appManager.CreateAppAccount
			(
				this.Id.ToString().ToLowerInvariant(),
				"Energy Network System",
				this,
				Microsoft.Win32.ShellUserProfileHelper.GetUserMicrosoftAccount(),
				string.Empty,
				string.Empty,
				System.Environment.UserName,
				Microsoft.Win32.ShellUserProfileHelper.GetUserDisplayName(),
				string.Empty,
				CurrentModelContext.UID,
				CurrentModelContext.UID,
				DateTime.UtcNow,
				DateTime.MaxValue
			);

			this._settings.Accounts.Add(account);

			this.SaveSettings();
		}

		private void SaveSettings()
		{
			_appManager.TrySaveAppSettings(this._settings);
		}
	} // class
} // namespace

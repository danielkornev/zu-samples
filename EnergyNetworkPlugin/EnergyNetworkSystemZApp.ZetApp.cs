using SAL.Flatbed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZU.Configuration.Settings;
using ZU.Core.Apps;
using ZU.Plugins;

namespace EnergyNetworkSystem
{
	public partial class EnergyNetworkSystemZApp
	{
		public object Invoke(string message, params object[] args)
		{
			return null;
		}

		public bool OnConnection(ConnectMode mode)
		{
			if (this.Host == null) throw new NotImplementedException("Host is not available");

			this.ZetHost = this.Host as IZetHost;

			if (this.ZetHost == null) throw new PlatformNotSupportedException("This build of Zet Universe platform is not supported. Host doesn't implement ZU.Plugins.IZetHost interface");

			IAppSettings settings = this.ZetHost.AppManager.GetAppSettings(this).Result;

			// we init using old Initialize method
			this.Initialize(settings, this.ZetHost.SIM, this.ZetHost.AppManager);

			return true;
		}

		public bool OnDisconnection(DisconnectMode mode)
		{
			return true;
		}


		public void Shutdown()
		{

		}


		public void ProcessGenericNotification()
		{

		}

		public bool ShowAddDialog()
		{
			//
			MessageBox.Show("Not Implemented Yet", "Energy Network System: Add");
			return false;
		}

		public void ShowSettingsDialog()
		{
			//
			MessageBox.Show("Not Implemented Yet", "Energy Network System: Settings");
		}


		public bool TryRemove()
		{
			return false;
		}


		// for now, no app pane for Energy Network System
		public AppPaneBase GetAppPane(IAppAccount account)
		{
			return null; //  _appPane;
		}

		public IAppSettings GetCurrentAppSettings()
		{
			return this._settings;
		}


	} // class
} // namespace

using SAL.Flatbed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZU.Configuration.Settings;
using ZU.Core.Apps;

namespace EnergyNetworkSystem
{
	public partial class EnergyNetworkSystemZApp
	{
		public List<IAppAccount> Accounts
		{
			get
			{
				return this._settings.Accounts;
			}
			set
			{
				this._settings.Accounts = value;
			}
		}

		public IAppButtonInfo AppButtonInfo
		{
			get
			{
				return this._appButtonInfo;
			}
		}

		public bool AreMultipleAccountsAllowed
		{
			get
			{
				return false;
			}
		}

		public IHost Host
		{
			get; set;
		}

		public Guid Id
		{
			get
			{
				return Guid.Parse("79FF6A8D-C619-4D66-B167-8347F9E5E1DA");
			}
		}

		public bool IsAdded
		{
			get
			{
				return true;
			}
		}

		public bool IsDataImportSupported
		{
			get
			{
				return false;
			}
		}

		public bool IsSignedIn
		{
			get
			{
				return true;
			}
		}

		public string Publisher
		{
			get
			{
				Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
				AssemblyCompanyAttribute companyAttribute = AssemblyCompanyAttribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute)) as AssemblyCompanyAttribute;
				if (companyAttribute != null)
				{
					string companyName = companyAttribute.Company;
					// Do something

					return companyName;
				}

				return string.Empty;
			}
		}

		public string Status
		{
			get
			{
				return "Added";
			}
		}

		public string Title
		{
			get
			{
				return "Energy Network System";
			}
		}

		public Version Version
		{
			get
			{
				return System.Reflection.Assembly.GetExecutingAssembly()
						   .GetName()
						   .Version;
			}
		}
	} // class
} // namespace

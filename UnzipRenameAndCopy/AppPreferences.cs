using Foundation;

namespace UnzipRenameAndCopy
{
	public class AppPreferences : NSObject
	{
		public AppPreferences()
		{
		}

		[Export("NumberOfEmail")]
		public string NumberOfEmail
		{
			get
			{
				var value = Load("NumberOfEmail", "");
				return value;
			}
			set
			{
				WillChangeValue("NumberOfEmail");
				Save("NumberOfEmail", value);
				DidChangeValue("NumberOfEmail");
			}
		}

		[Export("Email")]
		public string Email
		{
			get
			{
				var value = Load("Email", "");
				return value;
			}
			set
			{
				WillChangeValue("Email");
				Save("Email", value);
				DidChangeValue("Email");
			}
		}

		[Export("Password")]
		public string Password
		{
			get
			{
				var value = Load("Password", "");
				return value;
			}
			set
			{
				WillChangeValue("Password");
				Save("Password", value);
				DidChangeValue("Password");
			}
		}

		[Export("Keyword")]
		public string Keyword
		{
			get
			{
				var value = Load("Keyword", "");
				return value;
			}
			set
			{
				WillChangeValue("Keyword");
				Save("Keyword", value);
				DidChangeValue("Keyword");
			}
		}

		[Export("Pop")]
		public string Pop
		{
			get
			{
				var value = Load("Pop", "");
				return value;
			}
			set
			{
				WillChangeValue("Pop");
				Save("Pop", value);
				DidChangeValue("Pop");
			}
		}

		[Export("Port")]
		public string Port
		{
			get
			{
				var value = Load("Port", "");
				return value;
			}
			set
			{
				WillChangeValue("Port");
				Save("Port", value);
				DidChangeValue("Port");
			}
		}

		[Export("Destination")]
		public string Destination
		{
			get
			{
				var value = Load("Destination", "");
				return value;
			}
			set
			{
				WillChangeValue("Destination");
				Save("Destination", value);
				DidChangeValue("Destination");
			}
		}

		[Export("DestinationCsv")]
		public string DestinationCsv
		{
			get
			{
				var value = Load("DestinationCsv", "");
				return value;
			}
			set
			{
				WillChangeValue("DestinationCsv");
				Save("DestinationCsv", value);
				DidChangeValue("DestinationCsv");
			}
		}

		public string Load(string key, string defaultValue)
		{
			// Attempt to read preference
			var preference = NSUserDefaults.StandardUserDefaults.StringForKey(key);

			// Take action based on value
			if (preference == null)
			{
				return defaultValue;
			}
			else
			{
				return preference;
			}
		}

		public void Save(string key, string value)
		{
			NSUserDefaults.StandardUserDefaults.SetString(value, key);
		}
	}
}

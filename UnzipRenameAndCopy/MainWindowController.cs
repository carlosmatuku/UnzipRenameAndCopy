using System;

using Foundation;
using AppKit;
using System.Text.RegularExpressions;
using MailKit;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO.Compression;
using System.IO;
using System.Text;
using System.Net.Sockets;
using MailKit.Security;
using MailKit.Net.Pop3;
using System.Net;

namespace UnzipRenameAndCopy
{
	public partial class MainWindowController : NSWindowController
	{
		public MainWindowController(IntPtr handle) : base(handle)
		{
		}

		[Export("initWithCoder:")]
		public MainWindowController(NSCoder coder) : base(coder)
		{
		}

		public MainWindowController() : base("MainWindow")
		{
		}

		public static AppDelegate App
		{
			get { return (AppDelegate)NSApplication.SharedApplication.Delegate; }
		}

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();
			LoadParameters();
		}

		public new MainWindow Window
		{
			get { return (MainWindow)base.Window; }
		}

		private void SaveParameters()
		{
			var preferences = App.Preferences;
			preferences.Destination = Destination.StringValue;
			preferences.DestinationCsv = DestinationCsv.StringValue;
			preferences.Email = UserEmail.StringValue;
			preferences.Pop = Pop.StringValue;
			preferences.Password = UserPassword.StringValue;
			preferences.Port = Port.StringValue;
			preferences.NumberOfEmail = NumberOfMails.StringValue;
		}

		private void LoadParameters()
		{
			Destination.StringValue = App.Preferences.Destination;
			DestinationCsv.StringValue = App.Preferences.DestinationCsv;
			Port.StringValue = App.Preferences.Port;
			UserEmail.StringValue = App.Preferences.Email;
			Pop.StringValue = App.Preferences.Pop;
			UserPassword.StringValue = App.Preferences.Password;
			if (string.IsNullOrEmpty(App.Preferences.NumberOfEmail))
			{
				NumberOfMails.StringValue = "10";
			}
			else
				NumberOfMails.StringValue = App.Preferences.NumberOfEmail;
		}


		private bool CanGetMails()
		{
			if (string.IsNullOrEmpty(UserEmail.StringValue))
			{
				ShowAlert("Identifiants incorrects!", "L'adresse Email est obligatoire...");
				return false;
			}

			if (string.IsNullOrEmpty(UserPassword.StringValue))
			{
				ShowAlert("Identifiants incorrects!", "Le mot de passe est obligatoire...");
				return false;
			}

			if (string.IsNullOrEmpty(NumberOfMails.StringValue))
			{
				ShowAlert("Paramètres incorrects!", "Veuillez renseigner le nombre de mails maximum à remonter (Recommandé : 10)");
				return false;
			}

			var regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
			if (!regex.IsMatch(UserEmail.StringValue))
			{
				ShowAlert("Identifiants incorrects!", "Le format de l'adresse mail est incorrect!...");
				return false;
			}

			if (string.IsNullOrEmpty(Pop.StringValue))
			{
				ShowAlert("Paramètres incorrects!", "L'adresse IMAP est obligatoire...");
				return false;
			}

			if (string.IsNullOrEmpty(Port.StringValue))
			{
				ShowAlert("Paramètres incorrects!", "Le port est obligatoire...");
				return false;
			}

			regex = new Regex("^[0-9]+$");
			if (!regex.IsMatch(Port.StringValue))
			{
				ShowAlert("Paramètres incorrects!", "Le format du port est incorrect...");
				return false;
			}

			if (!regex.IsMatch(NumberOfMails.StringValue))
			{
				ShowAlert("Paramètres incorrects!", "Le nombre de mails est incorrect...");
				return false;
			}

			SaveParameters();

			return true;
		}

		private void ShowAlert(string title, string content)
		{
			var alert = new NSAlert
			{
				AlertStyle = NSAlertStyle.Critical,
				InformativeText = content,
				MessageText = title
			};
			alert.RunModal();
		}

		async partial void DoWork(NSObject sender)
		{
			if (MailsTable.SelectedRows.Count == 0)
			{
				ShowAlert("Attention", "Aucun élément sélectionné!");
				return;
			}

			LoadingText.StringValue = "Traitement en cours...";
			IsLoading.StartAnimation(sender);
			try
			{
				var dataSource = (CommandeDataSource)MailsTable.DataSource;
				var destination = Destination.StringValue;
				var destinationCsv = DestinationCsv.StringValue;

					foreach (var item in MailsTable.SelectedRows)
					{
						var commande = dataSource.Commandes[(int)item];
						if (Directory.Exists(Path.Combine(destination, commande.CommandNumber)))
						{
							ShowAlert("Erreur!", "Le dossier " + commande.CommandNumber + " existe déjà...");
							return;
						}
						await Task.Run(() =>
						{
							using (var client = new WebClient())
							{
								var content = client.DownloadData(commande.AttachmentUrl);
								using (var stream = new MemoryStream(content))
								{
									var archive = new ZipArchive(stream);
									var destinationPath = Directory.CreateDirectory(Path.Combine(destination, commande.CommandNumber));
									foreach (var entry in archive.Entries)
									{
										entry.ExtractToFile(Path.Combine(destinationPath.FullName, entry.Name));
										if (entry.Name.ToLower().Contains(".csv"))
										{
											var destinationCsvPath = Directory.CreateDirectory(Path.Combine(destinationCsv, commande.CommandNumber));
											var lines = File.ReadLines(Path.Combine(destinationPath.FullName, entry.Name), Encoding.Default).Take(2);
											var stringBuilder = new StringBuilder();
											foreach (var line in lines)
											{
												stringBuilder.AppendLine(line);
											}
											File.WriteAllText(Path.Combine(destinationCsvPath.FullName, entry.Name), stringBuilder.ToString(), Encoding.Default);
										}
									}
								}
							}
						});
				}
				ShowAlert("Information!", "Traitement réussi!");
			}
			catch (Exception ex)
			{
				ShowAlert("Erreur!", ex.ToString());
			}
			finally
			{ 
				LoadingText.StringValue = string.Empty;
				IsLoading.StopAnimation(sender);
			}
		}

		partial void OpenDestinationDialog(NSObject sender)
		{
			var dlg = NSOpenPanel.OpenPanel;
			dlg.CanChooseFiles = false;
			dlg.CanChooseDirectories = true;

			if (dlg.RunModal() == 1)
			{
				// Nab the first file
				var url = dlg.Urls[0];

				if (url != null)
				{
					Destination.StringValue = url.Path;
				}
			}
		}

		partial void OpenDestinationCsvDialog(NSObject sender)
		{
			var dlg = NSOpenPanel.OpenPanel;
			dlg.CanChooseFiles = false;
			dlg.CanChooseDirectories = true;

			if (dlg.RunModal() == 1)
			{
				// Nab the first file
				var url = dlg.Urls[0];

				if (url != null)
				{
					DestinationCsv.StringValue = url.Path;
				}
			}
		}

		private async Task<List<Commande>> GetMailsAsync()
		{
			var commandes = new List<Commande>();
			try
			{
				using (var client = new Pop3Client())
				{ 
					LoadingText.StringValue = "Connexion en cours...";
					await client.ConnectAsync(Pop.StringValue, Convert.ToInt32(Port.StringValue), true);
					client.AuthenticationMechanisms.Remove("XOAUTH2");
					await client.AuthenticateAsync(UserEmail.StringValue, UserPassword.StringValue);
					LoadingText.StringValue = "Connexion réussie. Récupération des commandes...";

					var totalMails = client.Count;
					LoadingText.StringValue = "Connexion réussie. Récupération des commandes... (" + totalMails + " mails détectés)";

					var nbMails = Convert.ToInt32(NumberOfMails.StringValue);
					if (nbMails > totalMails)
						nbMails = totalMails;

					var messages = await client.GetMessagesAsync(totalMails - nbMails, nbMails);
					var commandList = (from m in messages
									   where m.TextBody != null
					                   where m.Subject != null
									   where m.Subject.ToLower().Contains("safti")
					                   orderby m.Date.DateTime descending
									   select m);
					var regexHttpLink = new Regex(@"(ftp|https?):[^\s]+.zip");
					foreach (var commande in commandList)
					{
						var matches = regexHttpLink.Matches(commande.TextBody);
						foreach (var match in matches)
						{ 
							commandes.Add(new Commande(commande.From[0].Name, commande.Date.DateTime, match.ToString()));
						}
					}

					//commandes = (from message in messages
					//				   where message.Attachments != null
					//				   where !message.Subject.Contains("bulk")
					//				   where !message.Subject.Contains("spam")
					//				   where !message.Subject.Contains("junk")
					//				   from attachment in message.Attachments
					//				   where attachment.IsAttachment
					//             where attachment.ContentDisposition.FileName.Contains(".zip")
					//             orderby message.Date.DateTime descending
					//             select new Commande(message.From[0].Name, message.Date.DateTime, (MimePart)attachment)).ToList();
				}
				//using (var client = new ImapClient())
				//{
				//	LoadingText.StringValue = "Connexion en cours...";
				//	await client.ConnectAsync(Imap.StringValue, Convert.ToInt32(Port.StringValue), true);
				//	// Note: since we don't have an OAuth2 token, disable
				//	// the XOAUTH2 authentication mechanism.
				//	client.AuthenticationMechanisms.Remove("XOAUTH2");

				//	await client.AuthenticateAsync(UserEmail.StringValue, UserPassword.StringValue);
				//	LoadingText.StringValue = "Connexion réussie. Récupération des commandes...";

				//	var inbox = client.Inbox;
				//	await inbox.OpenAsync(FolderAccess.ReadOnly);

				//	var query = SearchQuery.SubjectContains(Keyword.StringValue).And(SearchQuery.SentAfter(DateTime.Today.AddDays(-10)));
				//	var uids = await inbox.SearchAsync(query);
				//	if (uids.Count > 0)
				//	{
				//		var messages = await inbox.FetchAsync(uids, MessageSummaryItems.Envelope | MessageSummaryItems.BodyStructure);

				//		var items = (from message in messages
				//					 where message.Attachments != null
				//					 from attachment in message.Attachments
				//					 where attachment.IsAttachment
				//					 where attachment.FileName.Contains(".zip")
				//					 select new { Message = message, Attachment = attachment }).ToList();

				//		var counter = 1;
				//		var total = items.Count;

					//	foreach (var item in items)
					//	{
					//		LoadingText.StringValue = "Récupération des commandes (" + counter + " sur " + total + ")...";
					//		var mime = (MimePart)(await client.Inbox.GetBodyPartAsync(item.Message.UniqueId, item.Attachment));
					//		commandes.Add(new Commande(item.Message.UniqueId, item.Message.Date.DateTime, item.Message.Envelope.From.First().Name, mime));
					//		counter++;
					//	}

					//	client.Disconnect(true);
					//}
				//}
			}
			catch (SocketException)
			{
				ShowAlert("Erreur!", "L'adresse IMAP ou le port renseigné est incorrect!");
			}
			catch (ServiceNotConnectedException)
			{
				ShowAlert("Erreur!", "Impossible de se connecter au serveur!");
			}
			catch (AuthenticationException)
			{
				ShowAlert("Erreur!", "L'adresse mail ou le mot de passe renseigné est incorrect!");
			}
			catch (Exception ex)
			{
				ShowAlert("Erreur!", ex.ToString());
				LoadingText.StringValue = string.Empty;
				commandes = null;
			}
			finally
			{
				LoadingText.StringValue = string.Empty;
			}
			return commandes;
		}


		async partial void GetMails(NSObject sender)
		{
			if (CanGetMails())
			{
				var dataSource = new CommandeDataSource();
				IsLoading.StartAnimation(sender);
				var commandes = await GetMailsAsync();
				dataSource.Commandes.AddRange(commandes);
				MailsTable.DataSource = dataSource;
				MailsTable.Delegate = new MailsTableDelegate(dataSource);
				IsLoading.StopAnimation(sender);

			}
		}
	}
}

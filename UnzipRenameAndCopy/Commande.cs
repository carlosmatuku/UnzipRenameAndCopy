using System;
using System.Linq;
using System.IO;
using MimeKit;

namespace UnzipRenameAndCopy
{
	public class Commande
	{
		public Commande()
		{
		}

		public string Date { get; set; } = "";
		public string Sender { get; set; } = "";
		public string FileName { get; set; } = "";
		public string CommandNumber { get; set; } = "";
		public string Attachment { get; set; }
		public string AttachmentUrl { get; set; }

		public Commande(string sender, DateTime date, string attachmentUrl)
		{
			if (date.Date == DateTime.Today)
			{
				this.Date = date.ToString("HH':'mm");
			}
			else { 
				this.Date = date.ToString("dd/MM/yyyy");
			}

			this.Sender = sender;
			this.AttachmentUrl = attachmentUrl;
			var myUri = new Uri(attachmentUrl);
			this.FileName = Path.GetFileNameWithoutExtension(myUri.Segments.Last());

			var parts = this.FileName.Split('_');
			this.CommandNumber = parts.Last();
		}


	}
}

using System;
using AppKit;

namespace UnzipRenameAndCopy
{
	public class MailsTableDelegate : NSTableViewDelegate
	{
		public MailsTableDelegate()
		{
		}

		#region Constants 
		private const string CellIdentifier = "ProdCell";
		#endregion

		#region Private Variables
		private CommandeDataSource DataSource;
		#endregion

		#region Constructors
		public MailsTableDelegate(CommandeDataSource datasource)
		{
			this.DataSource = datasource;
		}
		#endregion

		#region Override Methods
		public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
		{
			// This pattern allows you reuse existing views when they are no-longer in use.
			// If the returned view is null, you instance up a new view
			// If a non-null view is returned, you modify it enough to reflect the new data
			NSTextField view = (NSTextField)tableView.MakeView(CellIdentifier, this);
			if (view == null)
			{
				view = new NSTextField();
				view.Identifier = CellIdentifier;
				view.BackgroundColor = NSColor.Clear;
				view.Bordered = false;
				view.Selectable = false;
				view.Editable = false;
			}

			// Setup view based on the column selected
			switch (tableColumn.Title)
			{
				case "Date":
					view.StringValue = DataSource.Commandes[(int)row].Date;
					break;
				case "Expéditeur":
					view.StringValue = DataSource.Commandes[(int)row].Sender;
					break;
				case "Pièce jointe":
					view.StringValue = DataSource.Commandes[(int)row].FileName;
					break;
				case "Commande":
					view.Editable = true;
					view.EditingEnded += (sender, e) =>
							{
								DataSource.Commandes[(int)view.Tag].CommandNumber = view.StringValue;
							};
					view.StringValue = DataSource.Commandes[(int)row].CommandNumber;
					break;
			}

			return view;
		}

		public override bool ShouldSelectRow(NSTableView tableView, nint row)
		{
			return true;
		}

		//public override NSView GetViewForItem(NSTableView tableView, NSTableColumn tableColumn, nint row)
		//{
		//	// This pattern allows you reuse existing views when they are no-longer in use.
		//	// If the returned view is null, you instance up a new view
		//	// If a non-null view is returned, you modify it enough to reflect the new data
		//	NSTextField view = (NSTextField)tableView.MakeView(tableColumn.Title, this);
		//	if (view == null)
		//	{
		//		view = new NSTextField();
		//		view.Identifier = tableColumn.Title;
		//		view.BackgroundColor = NSColor.Clear;
		//		view.Bordered = false;
		//		view.Selectable = false;
		//		view.Editable = true;

		//		view.EditingEnded += (sender, e) =>
		//		{

		//			// Take action based on type
		//			switch (view.Identifier)
		//			{
		//				case "Product":
		//					DataSource.Products[(int)view.Tag].Title = view.StringValue;
		//					break;
		//				case "Details":
		//					DataSource.Products[(int)view.Tag].Description = view.StringValue;
		//					break;
		//			}
		//		};
		//	}

		//	// Tag view
		//	view.Tag = row;

		//	// Setup view based on the column selected
		//	switch (tableColumn.Title)
		//	{
		//		case "Product":
		//			view.StringValue = DataSource.Products[(int)row].Title;
		//			break;
		//		case "Details":
		//			view.StringValue = DataSource.Products[(int)row].Description;
		//			break;
		//	}

		//	return view;
		//}
		#endregion
	}
}

﻿using System;
using AppKit;
using System.Collections.Generic;

namespace UnzipRenameAndCopy
{
	public class CommandeDataSource: NSTableViewDataSource
	{
		#region Public Variables
		public List<Commande> Commandes = new List<Commande>();
		#endregion

		public CommandeDataSource()
		{
		}

		#region Override Methods
		public override nint GetRowCount(NSTableView tableView)
		{
			return Commandes.Count;
		}

		//public void Sort(string key, bool ascending)
		//{

		//	// Take action based on key
		//	switch (key)
		//	{
		//		case "Date":
		//			if (ascending)
		//			{
		//				Products.Sort((x, y) => x.Title.CompareTo(y.Title));
		//			}
		//			else {
		//				Products.Sort((x, y) => -1 * x.Title.CompareTo(y.Title));
		//			}
		//			break;
		//		case "Description":
		//			if (ascending)
		//			{
		//				Products.Sort((x, y) => x.Description.CompareTo(y.Description));
		//			}
		//			else {
		//				Products.Sort((x, y) => -1 * x.Description.CompareTo(y.Description));
		//			}
		//			break;
		//	}

		//}

		//public override void SortDescriptorsChanged(NSTableView tableView, NSSortDescriptor[] oldDescriptors)
		//{
		//	// Sort the data
		//	if (oldDescriptors.Length > 0)
		//	{
		//		// Update sort
		//		Sort(oldDescriptors[0].Key, oldDescriptors[0].Ascending);
		//	}
		//	else {
		//		// Grab current descriptors and update sort
		//		NSSortDescriptor[] tbSort = tableView.SortDescriptors;
		//		Sort(tbSort[0].Key, tbSort[0].Ascending);
		//	}

		//	// Refresh table
		//	tableView.ReloadData();
		//}
		#endregion
	}
}

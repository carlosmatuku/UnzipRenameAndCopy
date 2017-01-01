// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace UnzipRenameAndCopy
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		AppKit.NSTableColumn AttachmentColumn { get; set; }

		[Outlet]
		AppKit.NSTableColumn CommandColumn { get; set; }

		[Outlet]
		AppKit.NSTableColumn DateColumn { get; set; }

		[Outlet]
		AppKit.NSTextField Destination { get; set; }

		[Outlet]
		AppKit.NSTextField DestinationCsv { get; set; }

		[Outlet]
		AppKit.NSTableColumn ExpediteurColumn { get; set; }

		[Outlet]
		AppKit.NSProgressIndicator IsLoading { get; set; }

		[Outlet]
		AppKit.NSTextField Keyword { get; set; }

		[Outlet]
		AppKit.NSTextField LoadingText { get; set; }

		[Outlet]
		AppKit.NSTableView MailsTable { get; set; }

		[Outlet]
		AppKit.NSTextField NumberOfMails { get; set; }

		[Outlet]
		AppKit.NSTextField Pop { get; set; }

		[Outlet]
		AppKit.NSTextField Port { get; set; }

		[Outlet]
		AppKit.NSTextField UserEmail { get; set; }

		[Outlet]
		AppKit.NSSecureTextField UserPassword { get; set; }

		[Action ("DoWork:")]
		partial void DoWork (Foundation.NSObject sender);

		[Action ("GetMails:")]
		partial void GetMails (Foundation.NSObject sender);

		[Action ("OpenDestinationCsvDialog:")]
		partial void OpenDestinationCsvDialog (Foundation.NSObject sender);

		[Action ("OpenDestinationDialog:")]
		partial void OpenDestinationDialog (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (AttachmentColumn != null) {
				AttachmentColumn.Dispose ();
				AttachmentColumn = null;
			}

			if (CommandColumn != null) {
				CommandColumn.Dispose ();
				CommandColumn = null;
			}

			if (DateColumn != null) {
				DateColumn.Dispose ();
				DateColumn = null;
			}

			if (Destination != null) {
				Destination.Dispose ();
				Destination = null;
			}

			if (DestinationCsv != null) {
				DestinationCsv.Dispose ();
				DestinationCsv = null;
			}

			if (ExpediteurColumn != null) {
				ExpediteurColumn.Dispose ();
				ExpediteurColumn = null;
			}

			if (Pop != null) {
				Pop.Dispose ();
				Pop = null;
			}

			if (IsLoading != null) {
				IsLoading.Dispose ();
				IsLoading = null;
			}

			if (Keyword != null) {
				Keyword.Dispose ();
				Keyword = null;
			}

			if (LoadingText != null) {
				LoadingText.Dispose ();
				LoadingText = null;
			}

			if (MailsTable != null) {
				MailsTable.Dispose ();
				MailsTable = null;
			}

			if (Port != null) {
				Port.Dispose ();
				Port = null;
			}

			if (UserEmail != null) {
				UserEmail.Dispose ();
				UserEmail = null;
			}

			if (NumberOfMails != null) {
				NumberOfMails.Dispose ();
				NumberOfMails = null;
			}

			if (UserPassword != null) {
				UserPassword.Dispose ();
				UserPassword = null;
			}
		}
	}
}

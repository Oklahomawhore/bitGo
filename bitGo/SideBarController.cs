using System;

using UIKit;
using JASidePanels; // JASidePanels  https://github.com/gotosleep/JASidePanels
using CoreGraphics;
using System.Drawing;
using Contacts;
using System.Threading.Tasks;
using Foundation;
using System.Collections.Generic;
using System.Linq;

namespace bitGo
{
	public partial class SideBarController : UIViewController
	{
		public SideBarController () :base()
		{
			

		}

		public static UITableView table;
		public static NSMutableArray arrContact;
		public static CNContactStore contactStore;
		public static UISearchBar searchBar;

		public static Dictionary<string, List<string>> tableSectionData;
		private static List<string> nameData;
		public static string[] keysArray;

		public static Dictionary<string,List<string>>  searchResults;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();


			// Perform any additional setup after loading the view, typically from a nib.
			View.BackgroundColor = UIColor.LightGray;
			CGRect ss = UIScreen.MainScreen.Bounds;
			//var searchbar = new UISearchBar (new CGRect(ss.Width - this.GetSidePanelController().RightVisibleWidth,20,this.GetSidePanelController().RightVisibleWidth,30));
			CGRect bb = new CGRect(ss.Width - this.GetSidePanelController().RightVisibleWidth,20,this.GetSidePanelController().RightVisibleWidth, ss.Height - 20);


			//View.AddSubview (searchbar);
			//var contacts = store.GetUnifiedContacts (predicate, fetchkeys , out error);


			table = new UITableView (new CGRect(bb.X,bb.Y + 30, bb.Width, bb.Height - 30));
			searchBar = new UISearchBar (new CGRect (bb.X, bb.Y, bb.Width, 30));
			Add (table);
			Add (searchBar);
			//TableSource newSource = new TableSource (contacts);

			//table.Source = newSource;
			#region color section
			table.SectionIndexTrackingBackgroundColor = UIColor.DarkGray;
			table.BackgroundColor = UIColor.DarkGray;

			table.SectionIndexBackgroundColor = UIColor.FromRGB (51,50,69);
			table.SectionIndexColor = UIColor.White;
			table.TintColor = UIColor.White;

			#endregion


			//fetchrequest
			//fetchContacts();

			#region Create Table Source Data
			tableSectionData = createSection ();

			TableSource arr = new TableSource (tableSectionData, this);

			table.Source = arr;
			#endregion

			#region search implement

			var srchUpdator = new SearchResultUpdator();
			srchUpdator.updateSearchResults += search;

			searchDelegate srchD = new searchDelegate();
			searchBar.Delegate = srchD;

			#endregion



		}

		public class searchDelegate: UISearchBarDelegate
		{
			public searchDelegate():base()
			{


			}
			public override void TextChanged (UISearchBar searchBar, string searchText)
			{
				search (searchText);
			}

			public override void SearchButtonClicked (UISearchBar searchBar)
			{
				searchBar.ResignFirstResponder ();
			}

			public override void CancelButtonClicked (UISearchBar searchBar)
			{
				searchBar.ResignFirstResponder ();
			}
		}

		public class SearchResultUpdator: UISearchResultsUpdating{

			public event Action<string> updateSearchResults = delegate{};

			public override void UpdateSearchResultsForSearchController (UISearchController searchController)
			{
				this.updateSearchResults (searchController.SearchBar.Text);
			}


		}


		private Dictionary<string,List<string>> createSection(){
			#region declaration
			NSError error;

			contactStore = new CNContactStore ();

			if (tableSectionData == null) {

				tableSectionData = new Dictionary<string, List<string>> ();
			} else
				tableSectionData.Clear ();
			
			if (nameData == null)
				nameData = new List<string> ();
			
			bool isLetter = true;
			#endregion
			//var predicate = CNContact.GetPredicateForContactsInContainer (containerID);
			#region fetchContacts
			var keys = new NSString[] {
				CNContactKey.FamilyName, CNContactKey.GivenName, 
				CNContactKey.PhoneticFamilyName, CNContactKey.PhoneticGivenName, 
				CNContactKey.PhoneNumbers
			};



			CNContactFetchRequest feq = new CNContactFetchRequest (keys);

			feq.MutableObjects = false;
			feq.UnifyResults = true;
			
			contactStore.EnumerateContacts (feq, out error, (CNContact contact, bool stop) => {
				if (contact.PhoneNumbers.Length > 0 && (contact.GivenName != "" || contact.FamilyName != "" )){

					string name;

					if (contact.GivenName == "")
						name = contact.FamilyName;
					else if(contact.FamilyName == "")
						name = contact.GivenName;
					else
					name = contact.GivenName + " " + contact.FamilyName;

					string key = "";

					foreach (var v in contact.GivenName.ToCharArray()){

						if (!IsbasicLetter(v))
							isLetter = false;

					}
					foreach (var v in contact.FamilyName.ToCharArray()){

						if (!IsbasicLetter(v))
							isLetter = false;

					}
					if (!isLetter){

					if(contact.GivenName != "")
						key = GetPinyin(contact.GivenName).ToCharArray()[0].ToString();
					else key = GetPinyin(contact.FamilyName).ToCharArray()[0].ToString();
					}

					key = key.ToUpper();
				if (tableSectionData.ContainsKey (key))
					tableSectionData [key].Add (name);
				else {
					List<string> list = new List<string> () { name };

					tableSectionData.Add (key, list);
				}

					nameData.Add (name);}
			});
			#endregion
			/*try{
				contacts = contactStore.GetUnifiedContacts (predicate, keys, out error);
			}
			catch (Exception ex){

				Console.WriteLine (ex.Message);
			}*/


			//Create Keys
			keysArray = new string[tableSectionData.Keys.Count];
			tableSectionData.Keys.CopyTo(keysArray, 0);


			return tableSectionData;
		}

		public static string GetPinyin(string input){

			var output = new NSMutableString ();
			output.SetString (new NSString (input));
			var dumy = new NSRange ();
			output.ApplyTransform (NSStringTransform.MandarinToLatin, false, new NSRange (0, output.Length), out dumy);
			return output.ToString ();


		}

		public static bool IsbasicLetter(char c){
			return (c >= 'a' && c <= 'z' || c >= 'A' && c <= 'Z');
		}
		/*
		public static CNContactStore fetchContacts()
		{
			arrContact.RemoveAllObjects ();

			NSError error;

			var fetchkeys = new NSString[] { CNContactKey.PhoneNumbers, CNContactKey.FamilyName, CNContactKey.GivenName };


			CNContactFetchRequest fetchReq = new CNContactFetchRequest(CNContactKey.GivenName, CNContactKey.FamilyName, CNContactKey.PhoneNumbers);
			fetchReq.SortOrder = CNContactSortOrder.UserDefault;

			contactStore.EnumerateContacts (fetchReq,out error, (CNContact contact, bool stop) => {

				arrContact.Add(contact);

				table.ReloadSections( NSIndexSet.FromIndex(0), withRowAnimation: UITableViewRowAnimation.Fade);


			});

			createSectionData ();
			}
			*/

			/*
		private static void createSectionData(){


			if (tableSectionData == null)
				tableSectionData = new NSMutableDictionary ();
			else
				tableSectionData.Clear ();
			foreach (CNContact modal in arrContact) {

				NSString sectionKey = "";

				sectionKey = modal.GivenName.Substring (1);

				sectionKey = sectionKey.LocalizedUppercaseString;

				NSMutableArray sectionArray = tableSectionData.ObjectForKey (sectionKey);

				if (sectionArray == null) {
					allKeysArray.Add (sectionKey);
					sectionArray = new NSMutableArray ();
					tableSectionData.LowlevelSetObject (sectionArray, sectionKey);

				}

				sectionArray.Add (modal);

				NSMutableArray sortKeys = allKeysArray.Sort((obj1, obj2) => {return NSComparator(obj1,obj2);});

				keysArray.AddObjects (sortKeys);
					}
			}
		*/
		
		public class TableSource:UITableViewSource
		{

			Dictionary<string,List<string>> TableItems;
			string CellIdentifier = "TableCell";
			UIViewController pop;

			string[] keyslist;

			public TableSource (Dictionary<string, List<string>> ab) :base ()
			{
				TableItems = ab;

				var list  = ab.Keys.ToList();
				list.Sort();
				keyslist = list.ToArray();

				pop = new UIViewController();
			}
			
			public TableSource(Dictionary<string,List<string>> ab, UIViewController pop ): base ()
			{
				TableItems = ab;
				this.pop = pop;

				var list  = ab.Keys.ToList();
				list.Sort();
				keyslist = list.ToArray();
			}



			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				var arr = TableItems [keyslist[indexPath.Section]];
				UIAlertController okAlertController = UIAlertController.Create ("Row Selected", arr[indexPath.Row], UIAlertControllerStyle.Alert);
				okAlertController.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
				
				pop.PresentViewController (okAlertController, true, null);
				
				
				tableView.DeselectRow (indexPath, true);
			
				Console.WriteLine ("{0} selected", tableView.CellAt (indexPath).TextLabel.Text);


			}
				


			public override nint NumberOfSections (UITableView tableView)
			{
				
				return keyslist.Length;
			}
			public override nint RowsInSection (UITableView tableview, nint section)
			{
				
				return TableItems[keyslist[section]].Count;
			}
			public override string[] SectionIndexTitles (UITableView tableView)
			{
				return keyslist;
			}

			public override string TitleForHeader (UITableView tableView, nint section)
			{
				return keyslist [section];
			}


			public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell (CellIdentifier);

				if (cell == null) {

					cell = new UITableViewCell (UITableViewCellStyle.Default, CellIdentifier);
				}


				cell.BackgroundColor = UIColor.DarkGray;

				var arr = TableItems [keyslist[indexPath.Section]];

				var tempContact = arr [indexPath.Row];
				cell.TextLabel.Text = tempContact;

				cell.TextLabel.TextColor = UIColor.White;
				cell.TintColor = UIColor.White;

				return cell;

					
			}


		}

		public static void search(string forsearchString){
			if (forsearchString != "") {
				if (searchResults == null)
					searchResults = new Dictionary<string, List<string>> ();
				else
					searchResults.Clear ();

				/*
				foreach (string m in tableSectionData.Keys) {



					foreach (string k in tableSectionData[m]) {
						foreach (char b in forsearchString.ToCharArray ()) {
							if (k.Contains (b)) {
								if (searchResults.ContainsKey (m))
									searchResults [m].Add (k);
								else {
									List<string> temp = new List<string> ();
									temp.Add (k);
									searchResults.Add (m, temp);
										
								}
							}
						}
					}
				
				}*/

				//Dictionary find method

				foreach (string key in tableSectionData.Keys) {

					var tempList = tableSectionData [key].ToArray ();
					tempList = tempList.Where (t => t.Contains (forsearchString)).ToArray ();

					if (tempList.Length > 0)
						searchResults.Add (key, tempList.ToList ());
				}


				table.Source = new TableSource (searchResults);
			} else {
				
				table.Source = new TableSource (tableSectionData);
			}
			table.ReloadData ();
		}



		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}



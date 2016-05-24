using System;

using UIKit;
using System.Collections.Generic;
using System.Linq;
using CoreGraphics;

namespace bitGo
{
	public partial class searchResultController : UITableViewController
	{
		Dictionary<string,List<string>> tableData;
		static readonly string searchItemId = "searchItemId";

		public List<string> searchResults { get ; set;}


		public searchResultController(Dictionary<string,List<string>> tableData, CGRect bounds): base (){

			this.tableData = tableData;

			searchResults = new List<string> ();

		}

		public override nint RowsInSection (UITableView tableView, nint section)
		{
			return searchResults.Count;
		}

		public override UITableViewCell GetCell (UITableView tableView, Foundation.NSIndexPath indexPath)
		{
			var cell = tableView.DequeueReusableCell (searchItemId);

			if (cell == null)
				cell = new UITableViewCell ();
			
			cell.TextLabel.Text = searchResults [indexPath.Row];

			return cell;
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.


		}



		public void Search(string forsearchString){

			searchResults.Clear ();

			foreach(var key in tableData.Keys){
				

				foreach (string name in tableData [key]) {

					foreach (char b in forsearchString.ToCharArray ()) {
						if (name.Contains(b))
							searchResults.Add (name);

					}
				}

			}

			this.TableView.ReloadData ();
		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}


	}
}



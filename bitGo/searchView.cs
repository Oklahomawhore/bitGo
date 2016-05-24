using System;

using UIKit;
using CoreGraphics;

namespace bitGo
{
	public partial class searchView : UISearchController
	{
		

		public searchView (UIViewController controller):base (controller)
		{


		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.


			this.View.BackgroundColor = UIColor.DarkGray;


		}



		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}



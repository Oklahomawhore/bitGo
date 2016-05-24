using System;

using UIKit;
using JASidePanels;
using CoreGraphics;

namespace bitGo
{
	public partial class RootViewController : UIViewController
	{
		public RootViewController () : base ("RootViewController", null)
		{
			
		}

		UIBarButtonItem customButton;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			// Perform any additional setup after loading the view, typically from a nib.

			#region Nav controller color
			this.NavigationController.ToolbarHidden = false;
			//nav bar color
			this.NavigationController.NavigationBar.TintColor = UIColor.White;

			this.NavigationController.NavigationBar.BarTintColor = UIColor.FromRGB (255,160,20);

			//toolbar color
			this.NavigationController.Toolbar.TintColor = UIColor.White;

			this.NavigationController.Toolbar.BarTintColor = UIColor.FromRGB (255,160,20);

			#endregion



			View.BackgroundColor = UIColor.FromRGB(242,242,242);

			UINavigationBar.Appearance.SetTitleTextAttributes (new UITextAttributes (){ TextColor = UIColor.White });

			customButton = new UIBarButtonItem (UIImage.FromFile ("icon2.png"), UIBarButtonItemStyle.Plain, (sender, e) => {
				System.Diagnostics.Debug.WriteLine ("button tapped");
				this.GetSidePanelController().ToggleRightPanel(customButton);
			});

			NavigationItem.RightBarButtonItem = customButton;

			//Title View Settings
			var text = new UITextView (new CGRect (0,0,220,44));

			text.Text = "Send";
			text.BackgroundColor = null;
			text.Font = UIFont.PreferredTitle1;
			text.TextColor = UIColor.White;
			text.TextAlignment = UITextAlignment.Center;
			NavigationItem.TitleView = text;

			//Left Navigation Item

			//bottom toolbar
			this.SetToolbarItems (new UIBarButtonItem[] {new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace),
				new UIBarButtonItem("Next", UIBarButtonItemStyle.Plain, ((sender, e) => {

					Console.WriteLine("Next button pressed");
				})), new UIBarButtonItem(UIBarButtonSystemItem.FlexibleSpace)
			},true);


		}

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}



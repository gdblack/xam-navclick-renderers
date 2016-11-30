using System.Drawing;
using System.Threading.Tasks;
using CoreGraphics;
using DoneDone.Controls;
using DoneDone.Domain;
using DoneDone.Domain.Extensions;
using DoneDone.iOS.Controls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationPageRenderer))]
namespace DoneDone.iOS.Controls
{
    public class CustomNavigationPageRenderer : NavigationRenderer
    {
        private string NavTitle { get; set; }
        private Label NavTitleLabel { get; set; }
        private bool IsFirstViewed { get; set; }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                var navPage = (CustomNavigationPage)Element;

                NavTitle = navPage.CurrentPage.Title;
                NavTitleLabel = navPage.TitleLabel;
            }
        }


        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (NavigationBar.TopItem != null && !IsFirstViewed && App.TitleChanged)
            {
                var lbl = new UILabel();
                lbl.Text = App.IsIssuePage ? App.FilterTitleValue.FixNavTitle(PageType.issue) : App.FilterTitleValue.FixNavTitle(PageType.activity);
                lbl.Font = UIFont.FromName("GothamSSm-Book", 15);

                NavTitleLabel.FontFamily = "GothamSSm-Book";
                NavTitleLabel.FontSize = 15;

                var size = lbl.Text.StringSize(lbl.Font);
                NavTitleLabel.Text = App.IsIssuePage ? App.FilterTitleValue.FixNavTitle(PageType.issue) : App.FilterTitleValue.FixNavTitle(PageType.activity);
                var label = ConvertFormsToNative(NavTitleLabel, new RectangleF(0, 0, (float)size.Width, (float)size.Height));
                NavigationBar.TopItem.TitleView = null;
                NavigationBar.TopItem.TitleView = label;

            }
            IsFirstViewed = false;
        }

        //Fires when view is first loaded.
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            IsFirstViewed = true;
            this.NavigationBar.TintColor = UIColor.White;
            this.NavigationBar.BarTintColor = new UIColor(red: 0.95f, green: 0.44f, blue: 0.13f, alpha: 1.0f);

            if (NavTitleLabel != null)
            {
                UILabel lbl = new UILabel();
                lbl.Text = NavTitleLabel.Text;
                lbl.Font = UIFont.FromName("GothamSSm-Book", 15);

                NavTitleLabel.FontFamily = "GothamSSm-Book";
                NavTitleLabel.FontSize = 15;

                var size = lbl.Text.StringSize(lbl.Font);
                var label = ConvertFormsToNative(NavTitleLabel, new RectangleF(0, 0, (float)size.Width, (float)size.Height));

                this.NavigationBar.TopItem.TitleView = label;
            }
        }

        //http://www.michaelridland.com/xamarin/creating-native-view-xamarin-forms-viewpage/
        private static UIView ConvertFormsToNative(Xamarin.Forms.View view, CGRect size)
        {
            var renderer = Platform.CreateRenderer(view);

            renderer.NativeView.Frame = size;

            renderer.NativeView.AutoresizingMask = UIViewAutoresizing.All;
            renderer.NativeView.ContentMode = UIViewContentMode.ScaleAspectFit;
            
            renderer.Element.Layout(size.ToRectangle());

            var nativeView = renderer.NativeView;

            nativeView.SetNeedsLayout();

            return nativeView;
        }
    }
}
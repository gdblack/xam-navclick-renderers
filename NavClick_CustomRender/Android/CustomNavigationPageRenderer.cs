using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using DoneDone.Controls;
using DoneDone.Droid.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Color = Xamarin.Forms.Color;
using View = Android.Views.View;

[assembly: ExportRenderer(typeof(CustomNavigationPage), typeof(CustomNavigationPageRenderer))]
namespace DoneDone.Droid.Controls
{
    public class CustomNavigationPageRenderer : NavigationRenderer
    {
        private string NavTitle { get; set; }
        private Label NavTitleLabel { get; set; }
        private bool IsFirstViewed { get; set; }

        protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
        {
            base.OnElementChanged(e);

            if (Element != null)
            {
                var navPage = (CustomNavigationPage)Element;

                NavTitle = navPage.CurrentPage.Title;
                NavTitleLabel = navPage.TitleLabel;
            }
        }


        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            base.OnLayout(changed, l, t, r, b);

            var actionBar = ((Activity)Context).ActionBar;
            if (actionBar != null)
            {
                NavTitleLabel.FontFamily = "GothamSSm-Book";
                NavTitleLabel.FontSize = 30;
                NavTitleLabel.VerticalTextAlignment = Xamarin.Forms.TextAlignment.Center;
                NavTitleLabel.HorizontalTextAlignment = Xamarin.Forms.TextAlignment.Center;
                
                NavTitleLabel.TextColor = Color.Black;
                NavTitleLabel.Text = App.TitleChanged ? App.FilterTitleValue : NavTitleLabel.Text;

                actionBar.SetDisplayShowTitleEnabled(false);
                actionBar.SetDisplayShowCustomEnabled(true);

                var customeTitle = ConvertFormsToNative(NavTitleLabel, NavTitleLabel.Bounds);
                actionBar.CustomView = customeTitle;
            }

        }


        //http://www.michaelridland.com/xamarin/creating-native-view-xamarin-forms-viewpage/
        private ViewGroup ConvertFormsToNative(Xamarin.Forms.View view, Rectangle size)
        {
            var vRenderer = Platform.CreateRenderer(view);
            var viewGroup = vRenderer.ViewGroup;
            vRenderer.Tracker.UpdateLayout();
            var layoutParams = new ViewGroup.LayoutParams((int)size.Width, (int)size.Height);
            viewGroup.LayoutParameters = layoutParams;
            view.Layout(size);
            viewGroup.Layout(0, 0, (int)view.WidthRequest, (int)view.HeightRequest);
            return viewGroup;
        }

    }
}
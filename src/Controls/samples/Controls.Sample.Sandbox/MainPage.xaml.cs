using System;
using Microsoft.Maui.Controls;

namespace Maui.Controls.Sample
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
			InitializeComponent();
		}

		private void myButton_Click(object sender, EventArgs e)
		{
			(sender as Button).Text = DateTime.Now.ToString();
		}

		private void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			thisLabel.Text = e.CurrentSelection[0].ToString();
		}

		private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			thisLabel.Text = e.SelectedItem.ToString();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1
{
    public class Stand
    {
        public string Text { get; set; }
        public string Location { get; set; }
        public string Pass { get; private set; }
        public bool Visited;
        public int Points { get; set; }

        public Stand(string text, string location, string pass, int points = 1)
        {
            Text = text;
            Pass = pass;
            Visited = false;
            Points = points;
            Location = location;
        }

        public StackLayout GetStackLayout()
        {
            Color color = Color.Default;
            if (Visited)
                color = Color.Green;
            StackLayout stackLine = new StackLayout() { Orientation = StackOrientation.Horizontal };
            stackLine.Children.Add(new Label { Text = this.Location, HorizontalOptions = LayoutOptions.Center, TextColor = color });
            stackLine.Children.Add(new Label { Text = "Počet bodů: " + Points, HorizontalOptions = LayoutOptions.Center, TextColor = color });
            stackLine.Children.Add(new Label { Text = this.Text, HorizontalOptions = LayoutOptions.CenterAndExpand, TextColor = color });

            return stackLine;
        }

        public bool Visit(string pass)
        {
            if (pass.ToLower() == Pass)
            {
                this.Visited = true;
                return true;
            }
            return false;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace App1
{
    public class Stand
    {
        public string Text { get; set; }
        public string Pass { get; private set; }
        public bool Visited;
        public int Points { get; set; }

        public Stand(string text, string pass, int points = 1)
        {
            Text = text;
            Pass = pass;
            Visited = false;
            Points = points;
        }

        public StackLayout GetStackLayout()
        {
            Color color = Color.Default;
            if (Visited)
                color = Color.Green;

            

            StackLayout stack = new StackLayout() { Orientation = StackOrientation.Horizontal };
            stack.Children.Add(new Label { Text = this.Text, HorizontalOptions = LayoutOptions.Start, HorizontalTextAlignment = TextAlignment.Start, TextColor = color});
            stack.Children.Add(new Label { Text = "Počet bodů: " + Points, HorizontalOptions = LayoutOptions.Center,HorizontalTextAlignment= TextAlignment.Start, TextColor = color});
            stack.Children.Add(new Label { Text = "\u2713", HorizontalOptions = LayoutOptions.Start, HorizontalTextAlignment = TextAlignment.Start, TextColor = color, IsVisible = Visited });


            return stack;
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

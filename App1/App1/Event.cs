using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace App1
{
    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Stand> Stands = new List<Stand>();
        public DateTime From;
        public DateTime To;

        public Event(XmlNode node)
        {
            Name = node["Name"].InnerText;
            From = DateTime.Parse(node["From"].InnerText);
            To = DateTime.Parse(node["To"].InnerText);
            foreach (XmlNode stand in node["Stands"].ChildNodes)
                Stands.Add(Stand.NewStand(stand));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BinManager.Models
{
    public enum MenuItemType
    {
        MainMenu,
        NewBin,
        Map
    }
    public class HomeMenuItem
    {
        public MenuItemType Id { get; set; }

        public string Title { get; set; }
    }
}

using System;
using Xamarin.Forms;

namespace BinManager.Utilities.Extensions
{
    public static class ActivityIndicatorExtensions
    {
        public static void Off(this ActivityIndicator ai)
        {
            ai.IsRunning = false;
            ai.IsVisible = false;
        }

        public static void On(this ActivityIndicator ai)
        {
            ai.IsVisible = true;
            ai.IsRunning = true;
        }
    }
}

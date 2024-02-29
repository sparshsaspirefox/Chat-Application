using Android.Content;
using Android.Views.InputMethods;
using Android.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application = Android.App.Application;

namespace ChatHubApp.Platforms.Android.Keyboard
{
    public class KeyboardService_Android : IKeyboardService
    {
        public void ShowKeyboard()
        {
            var context = Application.Context;
            var inputMethodManager = context.GetSystemService(Context.InputMethodService) as InputMethodManager;
            var currentFocus = (context as Activity).CurrentFocus;

            if (currentFocus != null)
                inputMethodManager.ShowSoftInput(currentFocus, ShowFlags.Forced);
        }
    }
}

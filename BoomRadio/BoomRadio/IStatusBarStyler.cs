using System;
using System.Collections.Generic;
using System.Text;

namespace BoomRadio
{
    public interface IStatusBarStyler
    {
        void SetLightTheme();
        void SetDarkTheme();
        bool IsDarkTheme();
    }
}

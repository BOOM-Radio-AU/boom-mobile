using System;
using System.Collections.Generic;
using System.Text;

namespace BoomRadio
{
    public interface IUpdatableUI
    {
        void UpdateUI();
        void SetHorizontalDisplay();
        void SetVerticalDisplay();
    }
}

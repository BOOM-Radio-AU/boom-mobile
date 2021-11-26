using System;
using System.Collections.Generic;
using System.Text;

namespace BoomRadio
{
    /// <summary>
    /// A view that can update it's UI, per the Model-View-Controller pattern
    /// </summary>
    public interface IUpdatableUI
    {
        /// <summary>
        /// Updates the view from its model(s)
        /// </summary>
        void UpdateUI();
        /// <summary>
        /// Updates ther view to display horizontally
        /// </summary>
        void SetHorizontalDisplay();
        /// <summary>
        /// Updates ther view to display vertically
        /// </summary>
        void SetVerticalDisplay();
    }
}

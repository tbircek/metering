﻿using System.Windows;

namespace metering
{
    /// <summary>
    /// The view model for main window
    /// </summary>
    public class WindowViewModel: BaseViewModel
    {
        #region Private Member

        /// <summary>
        /// The window this view model controls
        /// </summary>
        private Window thisWindow;
        #endregion

        #region Public Properties


        #endregion
        
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public WindowViewModel(Window window)
        {
            thisWindow = window;
        }

        #endregion

    }
}

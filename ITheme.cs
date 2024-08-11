
namespace FlatPanelTest
{
    // Copyright (c) Smart PC Utilities, Ltd.
    // All rights reserved.

    public interface ITheme
    {

        #region Properties

        /// <summary>
        /// Get or set the control style
        /// </summary>
        FlatPanelTest.UITheme Theme { get; set; }

        /// <summary>
        /// Get or set whether to allow the control to inherit the parent control style
        /// </summary>
        bool ParentTheme { get; set; }

        #endregion

    }
}
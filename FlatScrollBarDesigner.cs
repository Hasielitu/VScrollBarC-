using System.Collections;
// Copyright (c) Smart PC Utilities, Ltd.
// All rights reserved.

#region References

using System.ComponentModel;
using System.Windows.Forms.Design;
using Microsoft.VisualBasic.CompilerServices;

#endregion

namespace FlatPanelTest.Controls
{

    internal class FlatScrollBarDesigner : ControlDesigner
    {

        #region Overridden Properties

        public override SelectionRules SelectionRules
        {
            get
            {

                var propDescriptor = TypeDescriptor.GetProperties(Component)["Orientation"];

                if (propDescriptor is not null)
                {

                    Controls.ScrollBarOrientation orientation = (Controls.ScrollBarOrientation)Conversions.ToInteger(propDescriptor.GetValue(Component));

                    return orientation == Controls.ScrollBarOrientation.Vertical ? SelectionRules.Visible | SelectionRules.Moveable | SelectionRules.BottomSizeable | SelectionRules.TopSizeable : SelectionRules.Visible | SelectionRules.Moveable | SelectionRules.LeftSizeable | SelectionRules.RightSizeable;

                }

                return base.SelectionRules;

            }
        }

        #endregion

        #region Overridden Methods

        protected override void PreFilterProperties(IDictionary properties)
        {

            properties.Remove("Text");
            properties.Remove("BackgroundImage");
            properties.Remove("ForeColor");
            properties.Remove("ImeMode");
            properties.Remove("Padding");
            properties.Remove("BackgroundImageLayout");
            properties.Remove("BackColor");
            properties.Remove("Font");
            properties.Remove("RightToLeft");

            base.PreFilterProperties(properties);

        }

        #endregion

    }

}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    public abstract class MenuItem : Drawable
    {
        #region Properties

        public bool Selected
        {
            get { return m_selected; }
            set { m_selected = value; }
        }

        private bool m_selected = false;

        public Color Selected_Tint
        {
            get { return m_selected_tint; }
            set { m_selected_tint = value; }
        }

        private Color m_selected_tint = Color.White;

        //public abstract int Width { get; }
        //public abstract int Height { get; }

        #endregion 

        #region Events

        /// <summary>
        /// Event raised when the menu entry is selected.
        /// </summary>
        public event EventHandler<PlayerIndexEventArgs> m_selected_events;

        /// <summary>
        /// Method for raising the Selected event.
        /// </summary>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex) // Took away internal keyword
        {
            if (m_selected_events != null)
                m_selected_events(this, new PlayerIndexEventArgs(playerIndex));
        }

        #endregion
    }
}

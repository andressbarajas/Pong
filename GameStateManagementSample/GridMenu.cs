using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace PongaThemes
{
    public delegate void Selected();

    public class GridMenu
    {
        #region Properties

        class SelectionItem {
            public bool m_selected;
            public Selected m_fcn_ptr;
            public Drawable m_sprite;
            public Rectangle[] m_border;
            public string m_music_name;
        }

        class GridItem
        {
            public bool m_selectable;
            public int m_list_index;
        }

        public int X_Pos {
            get { return m_xpos; }
            set { m_xpos = value; }
        }

        private int m_xpos = 0;

        public int Y_Pos {
            get { return m_ypos; }
            set { m_ypos = value; }
        }

        private int m_ypos = 0;

        public int Spacing {
            get { return m_spacing; }
            set { m_spacing = value; }
        }

        private int m_spacing = 15;

        public int BorderWidth
        {
            set { m_border_width = value; }
        }

        private int m_border_width = 5;

        public Color BorderTint
        {
            get { return m_brdr_tint; }
            set { m_brdr_tint = value; }
        }

        private Color m_brdr_tint = Color.White;

        public Color SelectedTint
        {
            get { return m_sel_tint; }
            set { m_sel_tint = value; }
        }

        private Color m_sel_tint = Color.Aqua;

        #endregion 

        #region Fields

        int m_x;
        int m_y;
        int m_rows;
        int m_columns;
        int m_num_items;
        int m_selected_x;
        int m_selected_y;
        int m_old_selected_x;
        int m_old_selected_y;

        GridItem[,] m_grid;

        Cue m_cue;
        SoundBank m_soundbank;

        Texture2D m_texture;
        List<SelectionItem> m_list;

        #endregion

        #region Initialization

        public GridMenu(int xloc, int yloc, int cols, Texture2D texture, SoundBank soundbank)
        {  
            m_xpos = xloc;
            m_ypos = yloc;
            m_columns = cols;
            m_texture = texture;
            m_soundbank = soundbank;

            Console.WriteLine("Starting Drawing at [" + xloc + ", " + yloc + "]");

            m_x = -1;
            m_y = 0;
            m_rows = 1;
            m_num_items = 0;
            m_selected_x = -1;
            m_selected_y = -1;
            
            m_list = new List<SelectionItem>();
            m_grid = new GridItem[m_rows, m_columns];

            for (int i = 0; i < m_columns; i++)
            {
                m_grid[m_rows-1, i] = new GridItem();
                m_grid[m_rows-1, i].m_selectable = false;
            }
        }

        #endregion

        #region Update and Draw

        public void Update()
        {
            for (int i = 0; i < m_list.Count; i++)
            {
                m_list[i].m_sprite.Update();
                /*
                for (int y = 0; y < m_list[i].m_border.Length; y++)
                {
                    
                }*/
            }
        }

        public void Draw(SpriteBatch spritebatch)
        {
            for (int i = 0; i < m_list.Count; i++)
            {
                m_list[i].m_sprite.Draw(spritebatch);

                for (int y = 0; y < m_list[i].m_border.Length; y++)
                {
                    if (m_list[i].m_selected)
                    {
                        spritebatch.Draw(m_texture, m_list[i].m_border[y], m_sel_tint);
                    }
                    else
                    {
                        spritebatch.Draw(m_texture, m_list[i].m_border[y], m_brdr_tint);
                    }
                }
            }
        }

        #endregion

        #region Public Methods

        public void HandleInput(InputState input) 
        {
            PlayerIndex playerIndex;

            m_old_selected_x = m_selected_x;
            m_old_selected_y = m_selected_y;
            m_list[m_grid[m_selected_y, m_selected_x].m_list_index].m_selected = false;

            if (m_cue.IsPrepared && !m_cue.IsPlaying)
            {
                m_cue.Play();
            }

            if (input.IsMenuRight(null))
            {
                m_selected_x++;

                if ( (m_selected_x == m_columns) || (m_grid[m_selected_y, m_selected_x].m_selectable != true) )
                {
                    m_selected_x = 0;
                }
                UpdateWav();

                //Console.WriteLine("Grid[" + m_selected_y + ", " + m_selected_x + "] Selected");
            }
            else if (input.IsMenuLeft(null))
            {
                m_selected_x--;

                if (m_selected_x < 0) 
                {
                    m_selected_x = m_columns - 1;

                    while (m_grid[m_selected_y, m_selected_x].m_selectable != true)
                    {
                        m_selected_x--;
                    }
                }
                UpdateWav();
                //Console.WriteLine("Grid[" + m_selected_y + ", " + m_selected_x + "] Selected");
            }
            else if (input.IsMenuUp(null))
            {
                m_selected_y--;
                if (m_selected_y < 0) 
                {
                    m_selected_y = m_rows - 1;

                    while (m_grid[m_selected_y, m_selected_x].m_selectable != true)
                    {
                        m_selected_y--;
                    }
                }
                UpdateWav();
                //Console.WriteLine("Grid[" + m_selected_y + ", " + m_selected_x + "] Selected");
            }
            else if (input.IsMenuDown(null))
            {
                m_selected_y++;
                if ( (m_selected_y == m_rows) || (m_grid[m_selected_y, m_selected_x].m_selectable != true) )
                {
                    m_selected_y = 0;
                }
                UpdateWav();
                //Console.WriteLine("Grid[" + m_selected_y + ", " + m_selected_x + "] Selected");
            }
            else if (input.IsMenuSelect(null, out playerIndex))
            {
                m_cue.Stop(AudioStopOptions.Immediate);
                m_list[m_grid[m_selected_y, m_selected_x].m_list_index].m_fcn_ptr();
            }

            m_list[m_grid[m_selected_y, m_selected_x].m_list_index].m_selected = true;
        }

        public void AddItem(Drawable item, Selected fcn_ptr, string music_name)
        {
            SelectionItem temp = new SelectionItem();
            temp.m_sprite = item;

            m_num_items++;
                
            if (++m_x == m_columns)
            {
                m_x = 0;
            }

            temp.m_sprite.X_Pos = m_xpos + (m_x * item.Width) + (m_x * m_spacing);
            temp.m_sprite.Y_Pos = m_ypos + (m_y * item.Height) + (m_y * m_spacing);
            // Make the item selectable
            m_grid[m_y, m_x].m_selectable = true;
            m_grid[m_y, m_x].m_list_index = m_num_items - 1;

            Console.WriteLine("Grid[" + m_y + ", " + m_x + "] Set to true");

            // Update rows
            if (m_y != (int)((m_num_items/ m_columns)))
            {
                m_y++;
                m_rows++; 
                m_grid = HelperUtils.ResizeArray<GridItem>(m_grid, m_rows, m_columns);

                // Set new constructed row to false
                for (int i = 0; i < m_columns; i++)
                {
                    m_grid[m_rows - 1, i] = new GridItem();
                    m_grid[m_rows - 1, i].m_selectable = false;
                }
            }
            temp.m_music_name = music_name; 
            temp.m_fcn_ptr = fcn_ptr;
            temp.m_border = HelperUtils.BuildBorder(new Rectangle((int)temp.m_sprite.X_Pos, (int)temp.m_sprite.Y_Pos, temp.m_sprite.Width, temp.m_sprite.Height), m_border_width);
            
            if (m_list.Count == 0)
            {
                temp.m_selected = true;
                m_selected_x = 0;
                m_selected_y = 0;
                m_cue = m_soundbank.GetCue(music_name);
            }
            else
            {
                temp.m_selected = false;
            }

            m_list.Add(temp);
        }

        #endregion

        #region Private Methods

        private void UpdateWav() {
            // Only if we are selecting an new item are we changing the music
            if (m_cue.IsPrepared || (m_selected_x != m_old_selected_x || m_selected_y != m_old_selected_y))
            {
                //Console.WriteLine("TRUE");
                m_cue.Stop(AudioStopOptions.Immediate);
                // if(m_soundbank.HasValue == true) {
                SoundBank temp = m_soundbank;//.Value;
                m_cue = temp.GetCue(m_list[m_grid[m_selected_y, m_selected_x].m_list_index].m_music_name);
                m_cue.Play();
                // }
            }
        }

        #endregion
    }
}

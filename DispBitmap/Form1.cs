/* BitmapToBinary - generate binary files to store bitmaps
 * Copyright (C) 2014 Cristiano Griletti
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License v2
 * as published by the Free Software Foundation.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DispBitmap
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Items.AddRange(Directory.GetFiles("bmp\\"));
            listBox2.Items.AddRange(File.ReadAllLines("list.txt"));
            SetIm("bmp\\n_bitmap336.bmp");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listBox2.Items.Add(listBox1.SelectedItem.ToString());
            UpdateFile();
            listBox1.SelectedIndex++;
        }

        void UpdateFile()
        {
            string[] gg = new string[listBox2.Items.Count];
            listBox2.Items.CopyTo(gg, 0);
            File.WriteAllLines("list.txt", gg);
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           SetIm(listBox1.SelectedItem.ToString());
        }

        void SetIm(string s)
        {
            try
            {
                Bitmap p = new Bitmap(Bitmap.FromFile(s));

                pictureBox1.Width = p.Width * 2;
                pictureBox1.Height = p.Height * 2;
                pictureBox1.Image = p;
                label1.Text = "W= " + p.Width.ToString() + " H= " + p.Height.ToString();
            }
            catch { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null) return;
            listBox2.Items.Remove(listBox2.SelectedItem);
            UpdateFile();
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedItem == null) return;
            SetIm(listBox2.SelectedItem.ToString());
        }

        byte[] buff = new byte[0x20000];
        int ind;
        private void button3_Click(object sender, EventArgs e)
        {
            ind = 0;
            foreach(string li in listBox2.Items)
            {
                string p = li.ToString();
                ImgToArr(p);
            }
            File.WriteAllBytes("ee.bin", buff);
        }

        void ImgToArr(string fn)
        {
            Bitmap p = new Bitmap(Bitmap.FromFile(fn));
            byte[]  o = new byte[(p.Width * p.Height) / 8];
            byte b;

            for (int y = 0; y < (p.Height / 8); y++ )
            {
                for(int x = 0; x < p.Width; x++)
                {
                    b = 0;
                    for(int i = ((y+1)*8)-1;i>= (y*8); i--)
                    {
                        //byte gg = (byte)(p.GetPixel(x, i).ToArgb());
                        b <<= 1;
                        b += (byte)(p.GetPixel(x, i).ToArgb() + 1);
                    }
                    buff[ind++] = b;
                }
            }

        }
    }
}

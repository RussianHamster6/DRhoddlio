﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DRhoddlio
{
    public partial class infoForm : Form
    {
        public infoForm()
        {
            InitializeComponent();
        }

        //Closes form on click
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

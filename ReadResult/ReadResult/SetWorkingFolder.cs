﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReadResult
{
    public partial class SetWorkingFolder : Form
    {
        public SetWorkingFolder()
        {
            Application.EnableVisualStyles();
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new System.Windows.Forms.FolderBrowserDialog();
            fd.ShowNewFolderButton = false;
            fd.Description = "选择工作目录";
            var result = fd.ShowDialog(this);
            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            txtWorkingFolder.Text = fd.SelectedPath;
            btnConfirm.Focus();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            string sPath = txtWorkingFolder.Text;
            if (sPath == "")
            {
                SetInfo("请先选择工作目录！");
                return;
            }
            if (!Directory.Exists(sPath))
            {
                SetInfo("目录不存在，请重新选择！");
                return;
            }
            GlobalVars.Instance.WorkingFolder = sPath;
            this.Close();
        }

        private void SetInfo(string s)
        {
            txtHint.Text = s;
            txtHint.ForeColor = Color.Red;
        }
    }
}
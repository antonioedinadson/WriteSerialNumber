﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using WriteSerialNumber.Controllers;

namespace WriteSerialNumber
{
    public partial class WriteSerialNumber : Form
    {
        public WriteSerialNumber()
        {
            InitializeComponent();
        }

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );

        private void WriteSerialNumber_Load(object sender, EventArgs e)
        {
            lbBIOS.Text = DMIController.GetBiosManufacturer();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 15, 15));
        }

        private Boolean CheckSizeSerialNumber(string serialNumber)
        {

            try
            {


                if (serialNumber.Length < 15 || serialNumber.Length > 15)
                {
                    return false;
                }

            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }

            return true;
        }

        private Boolean isNumeric(string serialNumber)
        {
            if (serialNumber.Where(c => char.IsLetter(c)).Count() > 0)
            {
                return false;
            }

            return true;
        }

        private void txtSerialNumber_KeyDown(object sender, KeyEventArgs e)
        {
            string serialnumber = txtSerialNumber.Text;

            try
            {
                if (e.KeyValue == 13)
                {
                    
                    lbError.Text = "";

                    if (string.IsNullOrEmpty(serialnumber) || !CheckSizeSerialNumber(serialnumber) || !isNumeric(serialnumber))
                    {
                        throw new Exception("Serial Number is invalid.");
                    }

                    WriteSerialController.WriteSerialNumber(serialnumber, "");
                    lbError.ForeColor = Color.LimeGreen;
                    lbError.Text = "Serial number saved successfully.";
                    lbError.Visible = true;
                    txtSerialNumber.Text = "";

                    MessageBox.Show($"{serialnumber}, saved successfully.");
                }
            }
            catch (Exception ex)
            {
                lbError.Text = ex.Message;
                lbError.Visible = true;
            }
        }
    }
}

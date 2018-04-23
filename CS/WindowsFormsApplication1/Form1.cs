﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraPivotGrid;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'nwindDataSet.Invoices' table. You can move, or remove it, as needed.
            this.invoicesTableAdapter.Fill(this.nwindDataSet.Invoices);
            pivotGridControl1.BestFitColumnArea();

        }

        private void pivotGridControl1_CellDoubleClick(object sender, DevExpress.XtraPivotGrid.PivotCellEventArgs e)
        {
            if (e.RowField == null) return; //Grand Total cell
            PivotGridField[] columnFields = e.GetColumnFields();
            object[] columnValues = new object[columnFields.Length];
            for (int i = 0; i < columnFields.Length; i++)
                columnValues[i] = e.GetFieldValue(columnFields[i]);
            PivotGridField[] rowFields = e.GetRowFields();
            object[] rowValues = new object[rowFields.Length ];
            for (int i = 0; i < rowFields.Length -1; i++)
                rowValues[i] = e.GetFieldValue(rowFields[i]);

            DataTable dt = new DataTable();
            foreach (PivotGridField field in columnFields)
                dt.Columns.Add(field.ToString(), field.DataType);
            foreach (PivotGridField field in rowFields)
                dt.Columns.Add(field.ToString(), field.DataType);
            dt.Columns.Add(e.DataField.ToString(), e.DataField.DataType);

            foreach (object lastLevelValue in e.RowField.GetVisibleValues() )
            {
                rowValues[rowFields.Length-1] = lastLevelValue;
                object cellValue = e.GetCellValue(columnValues, rowValues, e.DataField);
                if (cellValue == null) continue;
                DataRow row = dt.NewRow();

                for (int i = 0; i < columnFields.Length; i++)
                    row[columnFields[i].ToString()] = columnValues[i];
                for (int i = 0; i < rowFields.Length; i++)
                    row[rowFields[i].ToString()] = rowValues[i];
                row[e.DataField.ToString()] = cellValue;
                dt.Rows.Add(row);
                
            }
            dataGridView1.DataSource = dt;

        }
    }
}
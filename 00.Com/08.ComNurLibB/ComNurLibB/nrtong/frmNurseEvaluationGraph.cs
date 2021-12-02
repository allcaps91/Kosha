using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

namespace ComNurLibB
{
    public partial class frmNurseEvaluationGraph : Form
    {
        Chart chart = null;

        double[] yValues1;
        double[] yValues2;
        double[] yValues3;
        string strTitle = "";

        private PrintDocument printDocument1 = new PrintDocument();

        public frmNurseEvaluationGraph()
        {
            InitializeComponent();
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
        }

        public frmNurseEvaluationGraph(double[] yVal1, double[] yVal2, double[] yVal3, string argTitle )
        {
            InitializeComponent();
            yValues1 = yVal1;
            yValues2 = yVal2;
            yValues3 = yVal3;
            strTitle = argTitle;
            printDocument1.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
        }


        private void frmNurseEvaluationGraph_Load(object sender, EventArgs e)
        {
            eGraph();
        }


        private void eGraph()
        {
            string[] xValues1 = { "V/S", "I/O", "감시", "산소포화", "기관흡입", "정맥투약", "기타투약", "배액관", "억제대", "전문치료", "체위변경", "이동", "식사섭취", "배변" };
            //double[] yValues1 = { 150, 130, 250, 130, 270, 250, 270, 130, 130, 270, 170, 100,  30, 150 };
            //double[] yValues2 = { 250, 270, 150, 270, 130, 150, 130, 270, 270, 130, 200, 200, 200, 200 };
            //double[] yValues3 = {   0,   0,   0,   0,   0,   0,   0,   0,   0,   0,  50, 100, 170,  50 };

            lbTitle.Text = "간호간병통합서비스 병동 중증도 간호필요도(" + strTitle + ")";

            chart = this.chart1;

            chart.Series.Clear();

            Series s1 = new Series("0");
            Series s2 = new Series("1");
            Series s3 = new Series("2");

            chart.Series.Add(s1);
            chart.Series.Add(s2);
            chart.Series.Add(s3);

            s1.Points.DataBindXY(xValues1, yValues1);
            //s1.Points.DataBindXY(yValues1, xValues1);
            s2.Points.DataBindY(yValues2);
            s3.Points.DataBindY(yValues3);


            s1.ChartType = SeriesChartType.Column;

            Axis ax = chart.ChartAreas[0].AxisX;
            ax.Interval = 1;

            SS1_Sheet1.Cells[1, 1].Text = string.Format("{0:0}", yValues1[0]);
            SS1_Sheet1.Cells[2, 1].Text = string.Format("{0:0}", yValues1[1]);
            SS1_Sheet1.Cells[3, 1].Text = string.Format("{0:0}", yValues1[2]);
            SS1_Sheet1.Cells[4, 1].Text = string.Format("{0:0}", yValues1[3]);
            SS1_Sheet1.Cells[5, 1].Text = string.Format("{0:0}", yValues1[4]);
            SS1_Sheet1.Cells[6, 1].Text = string.Format("{0:0}", yValues1[5]);
            SS1_Sheet1.Cells[7, 1].Text = string.Format("{0:0}", yValues1[6]);
            SS1_Sheet1.Cells[8, 1].Text = string.Format("{0:0}", yValues1[7]);
            SS1_Sheet1.Cells[9, 1].Text = string.Format("{0:0}", yValues1[8]);
            SS1_Sheet1.Cells[10, 1].Text = string.Format("{0:0}", yValues1[9]);
            SS1_Sheet1.Cells[12, 1].Text = string.Format("{0:0}", yValues1[10]);
            SS1_Sheet1.Cells[13, 1].Text = string.Format("{0:0}", yValues1[11]);
            SS1_Sheet1.Cells[14, 1].Text = string.Format("{0:0}", yValues1[12]);
            SS1_Sheet1.Cells[15, 1].Text = string.Format("{0:0}", yValues1[13]);

            SS1_Sheet1.Cells[1, 2].Text = string.Format("{0:0}", yValues2[0]);
            SS1_Sheet1.Cells[2, 2].Text = string.Format("{0:0}", yValues2[1]);
            SS1_Sheet1.Cells[3, 2].Text = string.Format("{0:0}", yValues2[2]);
            SS1_Sheet1.Cells[4, 2].Text = string.Format("{0:0}", yValues2[3]);
            SS1_Sheet1.Cells[5, 2].Text = string.Format("{0:0}", yValues2[4]);
            SS1_Sheet1.Cells[6, 2].Text = string.Format("{0:0}", yValues2[5]);
            SS1_Sheet1.Cells[7, 2].Text = string.Format("{0:0}", yValues2[6]);
            SS1_Sheet1.Cells[8, 2].Text = string.Format("{0:0}", yValues2[7]);
            SS1_Sheet1.Cells[9, 2].Text = string.Format("{0:0}", yValues2[8]);
            SS1_Sheet1.Cells[10, 2].Text = string.Format("{0:0}", yValues2[9]);

            SS1_Sheet1.Cells[12, 2].Text = string.Format("{0:0}", yValues2[10]);
            SS1_Sheet1.Cells[13, 2].Text = string.Format("{0:0}", yValues2[11]);
            SS1_Sheet1.Cells[14, 2].Text = string.Format("{0:0}", yValues2[12]);
            SS1_Sheet1.Cells[15, 2].Text = string.Format("{0:0}", yValues2[13]);

            SS1_Sheet1.Cells[12, 3].Text = string.Format("{0:0}", yValues3[10]);
            SS1_Sheet1.Cells[13, 3].Text = string.Format("{0:0}", yValues3[11]);
            SS1_Sheet1.Cells[14, 3].Text = string.Format("{0:0}", yValues3[12]);
            SS1_Sheet1.Cells[15, 3].Text = string.Format("{0:0}", yValues3[13]);

            //chart.Legends[0].Enabled = true;

            //s1.BorderWidth = 12;
            //s1.Color = Color.OrangeRed;

            //s1.IsValueShownAsLabel = true;

            //s1.BackGradientStyle = GradientStyle.VerticalCenter;
            //s2.BackGradientStyle = GradientStyle.Center;

            s1.XAxisType = AxisType.Primary;
            s2.XAxisType = AxisType.Secondary;
            s3.XAxisType = AxisType.Secondary;

            s1.YAxisType = AxisType.Primary;
            s2.YAxisType = AxisType.Secondary;
            s3.YAxisType = AxisType.Secondary;

            //int i = 0;

            

            //for (i =0; i < 14; i++)
            //{
            //    chart.Series[i].Label = xValues1[i];
            //}

            //chart.BorderlineColor = Color.Red;
            //chart.BorderlineDashStyle = ChartDashStyle.Solid;
            //chart.BorderlineWidth = 1;

            //chart.ChartAreas[0].BorderColor = Color.Blue;
            //chart.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
            //chart.ChartAreas[0].BorderWidth = 1;

            //chart.ChartAreas[0].AxisX.LineColor = Color.Blue;
            //chart.ChartAreas[0].AxisY.LineColor = Color.Blue;
            //chart.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            //chart.ChartAreas[0].AxisY.Enabled = AxisEnabled.False;

            //chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Blue;
            //chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Blue;
            //chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            //chart.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;

            //chart.ChartAreas[0].AxisX.MajorTickMark.LineColor = Color.Red;
            //chart.ChartAreas[0].AxisY.MajorTickMark.LineColor = Color.Red;
            //chart.ChartAreas[0].AxisX.MajorTickMark.Enabled = false;
            //chart.ChartAreas[0].AxisY.MajorTickMark.Enabled = false;

            //s1.Points[4].Color = Color.Blue;

        }

        Bitmap memoryImage;
        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            //Size s = this.Size;
            Size s = new Size(1150, 722);

            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X + 10, this.Location.Y + 30, 0, 0, s);

        }

        private void printDocument1_PrintPage(System.Object sender,
               System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            btnExit.Visible = false;
            btnPrint.Visible = false;
            this.Refresh();

            CaptureScreen();

            
            printDocument1.DefaultPageSettings.Landscape = true;
            printDocument1.Print();

            btnExit.Visible = true;
            btnPrint.Visible = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

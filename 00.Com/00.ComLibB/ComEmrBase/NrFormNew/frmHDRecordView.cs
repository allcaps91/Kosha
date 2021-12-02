using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ComBase;
using ComBase.Controls;
using Oracle.DataAccess.Client;

namespace ComEmrBase
{
    public partial class frmHDRecordView : Form
    {
        EmrPatient pAcp = null;

        #region GV
        class GV
        {
            public string Code = "";
            public string Y = "";
            public double X = 0;
        }

        private double WheelValue = 0;
        #endregion

        public frmHDRecordView(EmrPatient AcpEmr)
        {
            pAcp = AcpEmr;
            InitializeComponent();
        }


        #region 폼 이벤트
        private void frmHDRecordView_Load(object sender, EventArgs e)
        {
            ssList1_Sheet1.RowCount = 0;
            ssList2_Sheet1.RowCount = 0;

            dtpSDate1.Value = dtpEDate1.Value.AddYears(-1);
            dtpEDate1.Value = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();

            dtpSDate2.Value = dtpSDate1.Value;
            dtpEDate2.Value = dtpEDate1.Value;

            chartVital.MouseWheel += chartVital_MouseWheel;
        }
        #endregion



        #region 함수

        /// <summary>
        /// 인공신장실 v/s 가져오기!!!
        /// </summary>
        private void GetVitalData()
        {
            #region 변수
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            OracleDataReader reader = null;

            List<GV> GVLsit = new List<GV>();
            List<string> XList = new List<string>();
            #endregion

            ssList1_Sheet1.RowCount = 0;


            try
            {
                #region 쿼리
                SQL.AppendLine("SELECT CHARTDATE, CHARTTIME, R.ITEMVALUE");
                SQL.AppendLine("  FROM KOSMOS_EMR.AEMRCHARTMST A");
                SQL.AppendLine("    INNER JOIN KOSMOS_EMR.AEMRCHARTROW R");
                SQL.AppendLine("       ON A.EMRNO    = R.EMRNO");
                SQL.AppendLine("      AND A.EMRNOHIS = R.EMRNOHIS");
                SQL.AppendLine("      AND R.ITEMCD = 'I0000011061' -- AF(ml/min)");
                SQL.AppendLine("      AND R.ITEMVALUE IS NOT NULL  -- 데이터 있는 항목만");
                SQL.AppendLine(" WHERE CHARTDATE >= '" + dtpSDate1.Value.ToString("yyyyMMdd") + "'");
                SQL.AppendLine("   AND CHARTDATE <= '" + dtpEDate1.Value.ToString("yyyyMMdd") + "'");
                SQL.AppendLine("   AND PTNO = '" + pAcp.ptNo + "'");
                SQL.AppendLine("   AND FORMNO = 2201");
                #endregion


                SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr, "조회 에러");
                    return;
                }

                if (reader.HasRows)
                {
                    while(reader.Read())
                    {
                        ssList1_Sheet1.RowCount += 1;
                        ssList1_Sheet1.Cells[ssList1_Sheet1.RowCount - 1, 0].Text = DateTime.ParseExact(reader.GetValue(0).ToString(), "yyyyMMdd", null).ToString("yyyy-MM-dd");
                        ssList1_Sheet1.Cells[ssList1_Sheet1.RowCount - 1, 1].Text = DateTime.ParseExact(reader.GetValue(1).ToString(), "HHmmss", null).ToString("HH:mm");
                        ssList1_Sheet1.Cells[ssList1_Sheet1.RowCount - 1, 2].Text = reader.GetValue(2).ToString();

                        string strDateTime = ssList1_Sheet1.Cells[ssList1_Sheet1.RowCount - 1, 0].Text.Substring(5) + "\r\n" + ssList1_Sheet1.Cells[ssList1_Sheet1.RowCount - 1, 1].Text;
                        if (XList.IndexOf(strDateTime) == -1)
                        {
                            XList.Add(strDateTime);
                        }

                        GVLsit.Add(new GV()
                        {
                            Code = "AF"
                             ,
                            X = VB.Val(reader.GetValue(2).ToString())
                             ,
                            Y = strDateTime
                        }
                         );
                    }

                    ssList1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }

                reader.Dispose();


                if (GVLsit.Count == 0)
                    return;

                #region 그래프

                chartVital.Series.Clear();
                chartVital.Titles.Clear();
                chartVital.ChartAreas.Clear();

                chartVital.ChartAreas.Add("Default");
                chartVital.Titles.Add("AF");
                chartVital.Titles[0].Font = new Font("굴림", 16F, FontStyle.Bold);

                chartVital.ChartAreas["Default"].Position = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(0, 5, 85, 90);
                chartVital.ChartAreas["Default"].InnerPlotPosition = new System.Windows.Forms.DataVisualization.Charting.ElementPosition(13, 5, 90, 90);

                chartVital.Series.Add("AF");
                chartVital.Series["AF"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
                chartVital.Series["AF"].BorderWidth = 2;
                chartVital.Series["AF"].Color = Color.Red;
                chartVital.Series["AF"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;
                chartVital.Series["AF"].MarkerColor = Color.Red;
                chartVital.Series["AF"].MarkerSize = 6;

                // X축 그리기

                XList.Sort();

                foreach (string DateTiem in XList)
                {
                    if (GVLsit.Where(d => d.Y == DateTiem).Any())
                    {
                        List<GV> list = GVLsit.Where(d => d.Y == DateTiem).ToList();

                        foreach (System.Windows.Forms.DataVisualization.Charting.Series series in chartVital.Series)
                        {
                            if (list.Where(d => d.Code == series.Name).Any())
                            {
                                series.Points.AddY(list.Where(d => d.Code == series.Name).First().X);
                            }
                            else
                            {
                                series.Points.AddY(double.NaN);
                                series.Points[series.Points.Count - 1].IsEmpty = true;
                            }
                        }

                        chartVital.Series[0].Points[chartVital.Series[0].Points.Count - 1].AxisLabel = DateTiem;

                    }
                }

     

                chartVital.ChartAreas["Default"].AxisX.Interval = 1;
                chartVital.ChartAreas["Default"].AxisY.Interval = 0;
                chartVital.ChartAreas["Default"].AxisY.IntervalOffset = 10;
                chartVital.ChartAreas["Default"].AxisY.Minimum = 0; //250
                chartVital.ChartAreas["Default"].AxisY.Maximum = 3500; //250
                chartVital.ChartAreas["Default"].Position.X = 12;
                chartVital.ChartAreas["Default"].InnerPlotPosition.X = 2;
                chartVital.ChartAreas["Default"].AxisY.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
                chartVital.ChartAreas["Default"].AxisX.LabelAutoFitStyle = System.Windows.Forms.DataVisualization.Charting.LabelAutoFitStyles.None;
                chartVital.ChartAreas["Default"].AxisX.ScrollBar.Enabled = true;
                chartVital.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;

                int PositionX = 0; // Y축범위 가로 간격
                if (chartVital.Width <= 970)
                {
                    PositionX = 4;
                }
                else
                {
                    PositionX = 3;
                }

                chartVital.ChartAreas["Default"].Position.X = PositionX * 1;
                chartVital.ChartAreas["Default"].AxisY.LineWidth = 2;

                double WheelValue = 21;

                chartVital.ChartAreas["Default"].AxisX.ScaleView.Zoom(0, WheelValue);

                if (chartVital.Series[0].Points.Count > WheelValue)
                {
                    chartVital.ChartAreas["Default"].AxisX.ScaleView.Scroll(chartVital.Series[0].Points.Count - WheelValue);
                }

                foreach (System.Windows.Forms.DataVisualization.Charting.ChartArea item in chartVital.ChartAreas)
                {
                    if (item == chartVital.ChartAreas["Default"])
                        continue;

                    item.AxisX.ScrollBar.Enabled = true;
                    item.AxisX.ScaleView.Zoomable = true;
                    item.AxisX.ScrollBar.ButtonStyle = System.Windows.Forms.DataVisualization.Charting.ScrollBarButtonStyles.None;
                    item.AxisX.ScrollBar.ChartArea.BorderDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
                    item.AxisX.ScrollBar.LineColor = Color.Transparent;
                    item.AxisX.ScaleView.Zoom(0, WheelValue);

                    if (chartVital.Series[0].Points.Count > WheelValue)
                    {
                        item.AxisX.ScaleView.Scroll(chartVital.Series[0].Points.Count - WheelValue);
                    }

                }

                #endregion
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "GetVitalData 에러", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message, "조회 에러");
                return;
            }
        }


        private void chartVital_MouseWheel(object sender, MouseEventArgs e)
        {



            if (e.Delta < 0)
            {
                if (chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMaximum < WheelValue + 1)
                {
                    WheelValue = chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMaximum;
                }
                else
                {
                    WheelValue = WheelValue + 1;
                }


            }

            if (e.Delta > 0)
            {
                if (chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMinimum > WheelValue)
                {
                    WheelValue = chartVital.ChartAreas["Default"].AxisX.ScaleView.ViewMinimum;
                }
                else
                {
                    WheelValue = WheelValue - 1;
                }


            }

            chartVital.ChartAreas["Default"].AxisX.ScaleView.Zoom(0, WheelValue);

            foreach (System.Windows.Forms.DataVisualization.Charting.ChartArea item in chartVital.ChartAreas)
            {
                if (item == chartVital.ChartAreas["Default"])
                    continue;

                item.AxisX.ScaleView.Zoom(0, WheelValue);
            }

        }

        /// <summary>
        /// 혈액 투석 정보 가져오기
        /// </summary>
        private void GetBloodData()
        {
            #region 변수
            StringBuilder SQL = new StringBuilder();
            string SqlErr = string.Empty;
            OracleDataReader reader = null;
            #endregion

            ssList2_Sheet1.RowCount = 0;
            try
            {
                #region 쿼리
                SQL.AppendLine("SELECT CHARTDATE, CHARTTIME, R.ITEMVALUE");
                SQL.AppendLine("  FROM KOSMOS_EMR.AEMRCHARTMST A");
                SQL.AppendLine("    INNER JOIN KOSMOS_EMR.AEMRCHARTROW R");
                SQL.AppendLine("       ON A.EMRNO    = R.EMRNO");
                SQL.AppendLine("      AND A.EMRNOHIS = R.EMRNOHIS");
                SQL.AppendLine("      AND R.ITEMCD = 'I0000030990' -- Spkt/V");
                SQL.AppendLine("      AND R.ITEMVALUE <> '미해당'   -- 수치 값 적었을경우만!~");
                SQL.AppendLine(" WHERE CHARTDATE >= '" + dtpSDate2.Value.ToString("yyyyMMdd") + "'");
                SQL.AppendLine("   AND CHARTDATE <= '" + dtpEDate2.Value.ToString("yyyyMMdd") + "'");
                SQL.AppendLine("   AND PTNO = '" + pAcp.ptNo + "'");
                SQL.AppendLine("   AND FORMNO = 1577");
                #endregion


                SqlErr = clsDB.GetAdoRs(ref reader, SQL.ToString().Trim(), clsDB.DbCon);
                if (string.IsNullOrWhiteSpace(SqlErr) == false)
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL.ToString().Trim(), clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, SqlErr, "조회 에러");
                    return;
                }

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ssList2_Sheet1.RowCount += 1;
                        ssList2_Sheet1.Cells[ssList2_Sheet1.RowCount - 1, 0].Text = DateTime.ParseExact(reader.GetValue(0).ToString(), "yyyyMMdd", null).ToString("yyyy-MM-dd");
                        ssList2_Sheet1.Cells[ssList2_Sheet1.RowCount - 1, 1].Text = DateTime.ParseExact(reader.GetValue(1).ToString(), "HHmmss", null).ToString("HH:mm");
                        ssList2_Sheet1.Cells[ssList2_Sheet1.RowCount - 1, 2].Text = reader.GetValue(2).ToString();
                    }

                    ssList2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }

                reader.Dispose();
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, "GetBloodData 에러", clsDB.DbCon);
                ComFunc.MsgBoxEx(this, ex.Message, "조회 에러");
                return;
            }
        }
        #endregion

        #region 버튼 이벤트
        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetVitalData();
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            GetBloodData();
        }
        #endregion

        
    }
}

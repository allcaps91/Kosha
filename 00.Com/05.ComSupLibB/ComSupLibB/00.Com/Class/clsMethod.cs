using ComBase; //기본 클래스
using ComDbB; //DB연결
using ComSupLibB.SupIjrm;
using DevComponents.DotNetBar.Charts;
using DevComponents.DotNetBar.Charts.Style;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Management;
using System.Text;
using System.Windows.Forms;

namespace ComSupLibB.Com
{
    /// <summary>
    /// Class Name      : ComSupLibB.Com
    /// File Name       : clsMethod.cs
    /// Description     : 진료지원 공통 함수들 모음
    /// Author          : 김홍록
    /// Create Date     : 2017-08-18
    /// Update History  : 
    /// </summary>
    /// <history>       
    /// </history>
    /// <seealso cref= "신규" />
    public class clsMethod : clsParam
    {
        public bool isPrinterOffLine(string pStrPrinterNm)
        {
            bool b = true;

            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

                string printerName = "";
                foreach (ManagementObject printer in searcher.Get())
                {
                    printerName = printer["Name"].ToString().ToLower();
                    if (printerName.ToString().ToUpper().IndexOf(pStrPrinterNm) >= 0)
                    {
                        if (printer["WorkOffline"].ToString().ToLower().Equals("true"))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }

                        break;
                    }
                }
            }
            catch
            {
                
                return false;
            }

            return false;
        }

        public DataTable getDT_GROUPBY(string i_sGroupByColumn, string i_sAggregateColumn, DataTable i_dSourceTable)
        {

            DataView dv = new DataView(i_dSourceTable);

            DataTable dtGroup = dv.ToTable(true, new string[] { i_sGroupByColumn });

            dtGroup.Columns.Add("Count", typeof(int));

            foreach (DataRow dr in dtGroup.Rows)
            {
                dr["Count"] = i_dSourceTable.Compute("Count(" + i_sAggregateColumn + ")", i_sGroupByColumn + " = '" + dr[i_sGroupByColumn] + "'");
            }

            return dtGroup;
        }

        public DialogResult InputBox(string title, string content, ref string value, Image img = null)
        {
            Form form = new Form();
            PictureBox picture = new PictureBox();
            Label label = new Label();
            TextBox textBox = new TextBox();
            Button buttonOk = new Button();
            Button buttonCancel = new Button();

            form.ClientSize = new Size(400, 100);
            form.Controls.AddRange(new Control[] { label, picture, textBox, buttonOk, buttonCancel });
            form.FormBorderStyle = FormBorderStyle.FixedDialog;
            form.StartPosition = FormStartPosition.CenterScreen;
            form.MaximizeBox = false;
            form.MinimizeBox = false;
            form.AcceptButton = buttonOk;
            form.CancelButton = buttonCancel;

            form.Text = title;
            picture.Image = img;
            picture.SizeMode = PictureBoxSizeMode.StretchImage;
            label.Text = content;
            textBox.Text = value;
            buttonOk.Text = "확인";
            buttonCancel.Text = "취소";

            buttonOk.DialogResult = DialogResult.OK;
            buttonCancel.DialogResult = DialogResult.Cancel;

            picture.SetBounds(10, 10, 50, 50);
            label.SetBounds(65, 17, 300, 20);
            textBox.SetBounds(65, 40, 220, 20);
            buttonOk.SetBounds(135, 70, 70, 20);
            buttonCancel.SetBounds(215, 70, 70, 20);

            DialogResult dialogResult = form.ShowDialog();

            value = textBox.Text;
            return dialogResult;
        }

        public bool IsNumeric(string o) // 삽입된 문자열이 숫치형인지 아는지를 비교
        {

            if (o != null && o.ToString().Trim().Length > 0)
            {
                foreach (char ch in o)
                {
                    if (ch == '.' || ch == ',')
                    {
                    }
                    else
                    { 
                        if (!char.IsNumber(ch))
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void delDirctoryFile(string strDirPath)
        {
            DirectoryInfo dir = new DirectoryInfo(strDirPath);

            chkDir(strDirPath);

            FileInfo[] files = dir.GetFiles("*.*",SearchOption.AllDirectories);

            foreach (System.IO.FileInfo file in files)
            {
                file.Attributes = FileAttributes.Normal;
            }

            Directory.Delete(strDirPath, true);

            chkDir(strDirPath);
        }

        public void runImag(string strFilePath)
        {
            ProcessStartInfo f = new
            ProcessStartInfo("C:\\windows\\system32\\rundll32.exe", "C:\\windows\\system32\\shimgvw.dll,ImageView_Fullscreen " + strFilePath);
            try
            {
                Process.Start(f);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        public void killOrderEXE(string strEXEName)
        {
            foreach (Process p in Process.GetProcesses())
            {
                if (p.ProcessName.StartsWith(strEXEName))
                {
                    p.Kill();
                }

            }

        }

        public void setImage(string saveFilePath, DataTable dt, int row, int col )
        {
            Bitmap bitmap = null;


            try
            {
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    byte[] img = (byte[])dt.Rows[row][col];
                    MemoryStream ms = new MemoryStream(img);
                    bitmap = new Bitmap(ms);
                    bitmap.Save(saveFilePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                else
                {
                    return ;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                return ;
            }
        }

        /// <summary>BLOB형 데이터 처리</summary>
        /// <param name="filePath">해당파일경로</param>
        /// <returns></returns>
        public byte[] getImage(string filePath)
        {
            byte[] bReturn = null;

            try
            {
                FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                BinaryReader reader = new BinaryReader(stream);

                bReturn = reader.ReadBytes((int)stream.Length);

                reader.Close();
                stream.Close();
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.ToString());
                return null;
            }
            
            return bReturn;
        }

        public void chkDir(string strDirPATH)
        {
            DirectoryInfo dir = new DirectoryInfo(strDirPATH);

            if (dir.Exists == false)
            {
                dir.Create();
            }
        }

        public string[] getFile(string strPATH, string strFILE_TYPE)
        {
            string[] strArr = null;
            strArr = Directory.GetFiles(strPATH, strFILE_TYPE);
            return strArr;

        }

        public string GetIntToExcelColumn(int colIndex)
        {
            if (colIndex <= 26) return Convert.ToChar(colIndex + 64).ToString();

            int div = colIndex / 26;
            int mod = colIndex % 26;
            if (mod == 0) { mod = 26; div--; }
            return GetIntToExcelColumn(div) + GetIntToExcelColumn(mod);

        }

        public bool isOverLap(FpSpread f, int nChkCol)
        {
            bool b = false;
            string strValue         = string.Empty;
            string strValueConfront = string.Empty;

            for (int i = 0; i < f.ActiveSheet.RowCount; i++)
            {
                strValue = f.ActiveSheet.Cells[i, nChkCol].Text;

                if (string.IsNullOrEmpty(strValue) == false)
                {
                    for (int j = 0; j < f.ActiveSheet.RowCount; j++)
                    {
                        if (j != i)
                        {
                            strValueConfront = f.ActiveSheet.Cells[j, nChkCol].Text;

                            if (string.IsNullOrEmpty(strValueConfront) == false)
                            {
                                if (strValueConfront == strValue)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }

            return b;
        }

        public List<string> setDataRow(FpSpread f, int nRow)
        {
            List<string> lst = new List<string>();

            for (int j = 0; j < f.ActiveSheet.ColumnCount; j++)
            {
                lst.Add(f.ActiveSheet.Cells[nRow, j].Text);
            }

            return lst;
        }

        public DataRow[] setDataRow(DataTable dt, string strROWID)
        {
            DataRow[] dr = null;

            dt.CaseSensitive = true;

            dr = dt.Select("ROW_ID = '" + strROWID + "'");

            return dr;
        }

        public void setTranction(PsmhDb pDbCon, bool isChkTras, string SqlErr, string SQL, int chkRow, bool isMsg = true, bool isDelete = false)
        {
            if (isChkTras == true)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL,pDbCon); //에러로그 저장
            }
            else
            {
                clsDB.setCommitTran(clsDB.DbCon);

                if (isMsg == true)
                {
                    if (isDelete == true)
                    {
                        ComFunc.MsgBox(chkRow.ToString() + " 건을 삭제 하였습니다.");
                    }
                    else
                    {
                        ComFunc.MsgBox(chkRow.ToString() + " 건을 저장 하였습니다.");
                    }
                }
                else
                {
                    if (isDelete == true)
                    {
                        ComFunc.MsgBox("삭제 하였습니다.");
                    }
                    else
                    {
                        ComFunc.MsgBox("저장하였습니다.");
                    }
                }
                
            }
        }

        /// <summary>구분자가 있는 문자의 코드 갖고 오기
        /// </summary>
        /// <param name="s">대상 문자</param>
        /// <param name="gubun">구분짓는 문자</param>
        /// <returns>12345.생화학 -> 12345를 반환</returns>
        public string getGubunText(string s, string gubun) // 12345.생화학 -> 12345를 반환
        {
            string strReturn = "";
            strReturn = s;

            if (strReturn != null && strReturn.Length > 0 && strReturn.IndexOf(gubun) > 0)
            {
                strReturn = strReturn.Substring(0, strReturn.IndexOf(gubun));
                if (strReturn.ToUpper() == "NULL" || strReturn.IndexOf('*') == 0)
                {
                    strReturn = "*";
                }
            }

            return strReturn;
        }

        /// <summary>구분자가 있는 문자의 이름을 갖고 오기
        /// </summary>
        /// <param name="s">대상 문자</param>
        /// <param name="gubun">구분 깃는 문자</param>
        /// <returns>12345.생화학 -> 생화학을 반환</returns>
        public string getGubunTextName(string s, string gubun)
        {
            string strReturn = null;
            strReturn = s;

            if (strReturn != null && strReturn.Length > 0 && strReturn.IndexOf(gubun) > 0)
            {
                strReturn = strReturn.Substring(strReturn.IndexOf(gubun), (strReturn.Length - strReturn.IndexOf(gubun)));
                if (strReturn.ToUpper() == "NULL")
                {
                    strReturn = "";
                }
            }

            strReturn = strReturn.Replace(gubun, "");
            strReturn = strReturn.Trim();

            return strReturn;

        }

       
        /// <summary>숫자에 000,000표현
        /// </summary>
        /// <param name="s">변환대상 변수</param>
        /// <returns>123,456,789</returns>
        public string covNumComma(string s) // 문자형 숫자를 Comma를 찍어 표현한다. 123,456,789
        {
            string strSql = null;

            if (VB.IsNumeric(s) == false)
            {
                return s;
            }

            strSql = string.Format("{0:N0}", int.Parse(s));
            return strSql;
        }

        /// <summary> 숫자를 한글형 숫자로 변환
        /// </summary>
        /// <param name="n"></param>
        /// <returns>12345 -> 만이천삼백사십오</returns>
        public string covNum2Hangle(long n) //숫자를 문자로 변환 (예 : 12345 -> 만이천삼백사십오)
        {
            bool UseDecimal = false;
            string Sign = "";
            int i = 0;
            int Level = 0;

            string[] NumberChar = new string[] { "", "일", "이", "삼", "사", "오", "육", "칠", "팔", "구" };
            string[] LevelChar = new string[] { "", "십", "백", "천" };
            string[] DecimalChar = new string[] { "", "만", "억", "조", "경" };

            string strValue = string.Format("{0}", n);
            string NumToKorea = Sign;

            UseDecimal = false;

            for (i = 0; i < strValue.Length; i++)
            {
                Level = strValue.Length - i;
                if (strValue.Substring(i, 1) != "0")
                {
                    UseDecimal = true;
                    if (((Level - 1) % 4) == 0)
                    {
                        NumToKorea = NumToKorea + NumberChar[int.Parse(strValue.Substring(i, 1))] + DecimalChar[(Level - 1) / 4];
                        UseDecimal = false;
                    }

                    else
                    {
                        if (strValue.Substring(i, 1) == "1")
                            NumToKorea = NumToKorea + LevelChar[(Level - 1) % 4];
                        else
                            NumToKorea = NumToKorea + NumberChar[int.Parse(strValue.Substring(i, 1))] + LevelChar[(Level - 1) % 4];
                    }
                }

                else
                {
                    if ((Level % 4 == 0) && UseDecimal)
                    {
                        NumToKorea = NumToKorea + DecimalChar[Level / 4];
                        UseDecimal = false;
                    }
                }
            }
            return NumToKorea;
        }

        /// <summary>진료지원콤보박스설정</summary>
        /// <param name="cbo">콤버박스</param>
        /// <param name="str">설정명</param>
        /// <param name="e">타입</param>
        public void setCombo_View(ComboBox cbo, string[] str, enmComParamComboType e)
        {
            if (str != null)
            {

                string s = getGubunText(str[0], ".");
                string s2 = string.Empty;


                cbo.Text = null;
                cbo.Items.Clear();
                if (e == enmComParamComboType.ALL)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        s2 += "*";
                    }
                    cbo.Items.Add(s2 + ".전체");
                }
                else if (e == enmComParamComboType.NULL)
                {
                    cbo.Items.Add("");
                }

                for (int i = 0; i < str.Length; i++)
                {
                    cbo.Items.Add(str[i].ToString());
                }

                if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
            }
            else
            {
                cbo.Text = null;
                cbo.Items.Clear();
                cbo.Items.Add("");
            }
        }

        /// <summary>진료지원콤보박스설정</summary>
        /// <param name="cbo">콤버박스</param>
        /// <param name="dt">조회된 값으로 반드시 컬럼은 1개이며 코드.코드명으로 나와야 함</param>
        /// <param name="e">타입</param>
        public void setCombo_View(ComboBox cbo, DataTable dt, enmComParamComboType e)
        {
            if (ComFunc.isDataTableNull(dt) == false)
            {
                cbo.Text = null;
                cbo.Items.Clear();

                string s = getGubunText(dt.Rows[0][0].ToString(), ".");
                string s2 = string.Empty;

                if (e == enmComParamComboType.ALL)
                {
                    for (int i = 0; i < s.Length; i++)
                    {
                        s2 += "*";
                    }
                    cbo.Items.Add(s2 + ".전체");

                }
                else if (e == enmComParamComboType.NULL)
                {
                    cbo.Items.Add("");
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cbo.Items.Add(dt.Rows[i][0].ToString().Trim());
                }

                if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;

            }
            else
            {
                cbo.Text = null;
                cbo.Items.Clear();
                cbo.Items.Add("");
            }
        }

        /// <summary>진료지원콤보박스설정</summary>
        /// <param name="cbo">콤버박스</param>
        /// <param name="lstStr">조회된 값</param>
        /// <param name="e">타입</param>
        public void setCombo_View(ComboBox cbo, List<string> lstStr, enmComParamComboType e, string strTEXT = "")
        {
            try
            {
                if (lstStr.Count > 0)
                {
                    //cbo.Text = null;
                    cbo.Items.Clear();

                    string s = getGubunText(lstStr[0], ".");
                    string s2 = string.Empty;

                    if (e == enmComParamComboType.ALL)
                    {
                        for (int i = 0; i < s.Length; i++)
                        {
                            s2 += "*";
                        }
                        cbo.Items.Add(s2 + ".전체");

                    }
                    else if (e == enmComParamComboType.NULL)
                    {
                        cbo.Items.Add("");
                    }

                    for (int i = 0; i < lstStr.Count; i++)
                    {
                        cbo.Items.Add(lstStr[i].ToString().Trim());
                    }

                    if (string.IsNullOrEmpty(strTEXT) == true)
                    {
                        if (cbo.Items.Count > 0) cbo.SelectedIndex = 0;
                    }
                    else
                    {
                        cbo.Text = strTEXT;
                    }
                   
                }
                else
                {
                    cbo.Text = null;
                    cbo.Items.Clear();
                    cbo.Items.Add("");
                }

            }
            catch (Exception ex)
            {
                cbo.Text = null;
                cbo.Items.Clear();
                cbo.Items.Add("");


            }

        }

        /// <summary>날짜 사이 값 비교</summary>
        /// <param name="F"></param>
        /// <param name="T"></param>
        /// <returns></returns>
        public int getDate_Gap(DateTime F, DateTime T)
        {
            int n = 0;

            TimeSpan d = T - F;

            n = d.Days;

            return n;
        }

        public enum enmChartTitle { SERIES1, SERIES2, TITLE }
        public void setChart(ChartControl chartControl1, DataSet pDs, string[] pArrTitle, SeriesType pSeriesType)
        {
            ChartXy chartXy1 = (ChartXy)chartControl1.ChartPanel.ChartContainers[0];
            chartXy1.MinContentSize = new Size(700, pDs.Tables[0].Rows.Count * 20 );

            ChartTitle chartTitle1 = new ChartTitle();
            ChartSeries chartSeries1 = new ChartSeries();
            ChartSeries chartSeries2 = new ChartSeries();         

            setChart_TITLE(chartTitle1, pArrTitle[(int)enmChartTitle.TITLE].ToString());
            setChart_Axis(chartXy1);

            setChart_Series(chartSeries1, Tbool.True, Color.RoyalBlue   , "{Y:f0}", pArrTitle[(int)enmChartTitle.SERIES1].ToString(), pSeriesType, pDs,true);
            setChart_Series(chartSeries2, Tbool.True, Color.DarkRed     , "{Y:f1}", pArrTitle[(int)enmChartTitle.SERIES2].ToString(), pSeriesType, pDs,false);
            setChart_XY(chartXy1, chartTitle1, chartSeries1, chartSeries2);         
        }

        void setChart_Axis(ChartXy chartXy1)
        {
            ChartAxis axis = chartXy1.AxisX;
            axis.AxisMargins = 10;
            axis.MinorTickmarks.TickmarkCount = 0;
            axis.MajorTickmarks.StaggerLabels = true;

            axis.MajorGridLines.GridLinesVisualStyle.LineColor = Color.Gainsboro;
            axis.MinorGridLines.GridLinesVisualStyle.LineColor = Color.WhiteSmoke;
            axis.MajorTickmarks.LabelVisualStyle.TextFormat = "0.#";
        }

        void setChart_TITLE(ChartTitle chartTitle1, string strChartTitle)
        {
            chartTitle1.ChartTitleVisualStyle.Alignment = Alignment.MiddleCenter;
            chartTitle1.ChartTitleVisualStyle.Font      = new Font("굴림체",14F,FontStyle.Bold);
            chartTitle1.Text = strChartTitle;
        }

        void setChart_Series(ChartSeries chartSeries
                           , Tbool isHighlightPoints
                           , Color cTxt
                           , string strTextFormat, string strLegendText, SeriesType SeriseType
                           , DataSet pDs
                           , bool isINWON)
        {
            chartSeries.CrosshairHighlightPoints = isHighlightPoints;
            chartSeries.DataLabelVisualStyle.TextColor = cTxt;
            chartSeries.DataLabelVisualStyle.TextFormat = strTextFormat;

            chartSeries.EmptyValues = null;

            chartSeries.LegendText = strLegendText;
            chartSeries.Name = strLegendText;


            for (int i = 0; i < pDs.Tables[0].Rows.Count - 1; i++)
            {

                SeriesPoint seriesPoint1 = new SeriesPoint();
                seriesPoint1.ValueX = pDs.Tables[0].Rows[i][(int)clsComSupIjrmSQL.enmSel_ETC_JUSASUB_STT.TIT].ToString();

                if (isINWON == true)
                {
                    seriesPoint1.ValueY = new object[] { (object)Convert.ToDouble(pDs.Tables[0].Rows[i][(int)clsComSupIjrmSQL.enmSel_ETC_JUSASUB_STT.INWON].ToString()) };
                }
                else
                {
                    seriesPoint1.ValueY = new object[] { (object)Convert.ToDouble(pDs.Tables[0].Rows[i][(int)clsComSupIjrmSQL.enmSel_ETC_JUSASUB_STT.QTY].ToString()) };
                }
                chartSeries.SeriesPoints.Add(seriesPoint1);
            }

            chartSeries.SeriesType = SeriseType;
        }

        void setChart_XY(ChartXy chartXy1, ChartTitle chartTitle1, ChartSeries chartSeries1, ChartSeries chartSeries2)
        {
            chartXy1.BarShadingEnabled      = Tbool.True;
            chartXy1.AxisY.AxisMargins      = 35;
            chartXy1.AxisX.AxisMargins      = 20;
            chartXy1.BarLabelPosition       = BarLabelPosition.Far;
            chartXy1.ChartCrosshair.Visible = true;
            chartXy1.PointLabelDisplayMode  = PointLabelDisplayMode.AllSeriesPoints;
            chartXy1.Legend.ShowCheckBoxes  = true;
            chartXy1.Legend.Visible         = true;

            chartXy1.Titles.Clear();
            chartXy1.ChartSeries.Clear();

            chartXy1.Titles.Add(chartTitle1);
            chartXy1.ChartSeries.Add(chartSeries1);
            chartXy1.ChartSeries.Add(chartSeries2);

        }
        

        public void setCharPIE(ChartControl chartControl1, DataSet pDs, string strTitle)
        {
            PieChart pieChart1 = (PieChart)chartControl1.ChartPanel.ChartContainers[0];

            pieChart1.SubSliceVisualLayout.SliceLabelDisplayMode = SliceLabelDisplayMode.InnerAndOuter;
            pieChart1.InnerRadius = 0.3;
            pieChart1.SubSliceVisualLayout.OuterSliceLabel = "{x}(수량:{y0:f1})";

            PieSeries pieSeries1 = new PieSeries();
            ChartTitle chartTitle1 = new ChartTitle();
            

            pieChart1.CenterLabel = strTitle;

            setChart_TITLE(chartTitle1, strTitle);

            for (int i = 0; i < pDs.Tables[0].Rows.Count - 1; i++)
            {

                PieSeriesPoint pieSeriesPoint1 = new PieSeriesPoint();

                pieSeriesPoint1.ValueX = pDs.Tables[0].Rows[i][(int)clsComSupIjrmSQL.enmSel_ETC_JUSASUB_STT.TIT].ToString();
                pieSeriesPoint1.ValueY = new object[] { (object)Convert.ToDouble(pDs.Tables[0].Rows[i][(int)clsComSupIjrmSQL.enmSel_ETC_JUSASUB_STT.QTY].ToString()) };

                pieSeries1.SeriesPoints.Add(pieSeriesPoint1);
            }

            pieChart1.Titles.Clear();
            pieChart1.ChartSeries.Clear();

            pieChart1.ChartSeries.Add(pieSeries1);
            pieChart1.Titles.Add(chartTitle1);
        }
    }
}

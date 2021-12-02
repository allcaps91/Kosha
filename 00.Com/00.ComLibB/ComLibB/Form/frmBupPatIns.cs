using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using Oracle.DataAccess.Client;

namespace ComLibB
{
    public partial class frmBupPatIns : Form
    {
        public frmBupPatIns()
        {
            InitializeComponent();
        }

        public void Print1(string argSname, string argJumin, string argTel, string argHPhone, string argDept, string argDr,
            string argDrLic, string argPtNo, string argJuso, string argSabun)
        {
            string strFile = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssDiabetes1_Sheet1.Cells[3, 3].Text = "";
            ssDiabetes1_Sheet1.Cells[3, 8].Text = "";
            ssDiabetes1_Sheet1.Cells[4, 3].Text = "";
            ssDiabetes1_Sheet1.Cells[5, 3].Text = "";
            ssDiabetes1_Sheet1.Cells[3, 3].Text = argSname;
            ssDiabetes1_Sheet1.Cells[3, 8].Text = argJumin;
            ssDiabetes1_Sheet1.Cells[4, 3].Text = argTel;
            ssDiabetes1_Sheet1.Cells[5, 4].Text = argHPhone;
            ssDiabetes1_Sheet1.Cells[7, 3].Text = argDept;
            ssDiabetes1_Sheet1.Cells[27, 2].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");
            ssDiabetes1_Sheet1.Cells[29, 6].Text = argDr;
            ssDiabetes1_Sheet1.Cells[30, 6].Text = argDrLic;
            ssDiabetes1_Sheet1.Cells[33, 1].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");
            ssDiabetes1_Sheet1.Cells[45, 2].Text = argPtNo;
            ssDiabetes1_Sheet1.Cells[45, 5].Text = argDept;
            ssDiabetes1_Sheet1.Cells[45, 7].Text = argJuso;

            //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
            if (argSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes1_Sheet1.Cells[29, 9].CellType = imgCellType;
                ssDiabetes1_Sheet1.Cells[29, 9].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes1_Sheet1.Cells[29, 9].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(argSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(argSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes1_Sheet1.Cells[29, 9].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes1_Sheet1.Cells[29, 9].CellType = textCellType;
                ssDiabetes1_Sheet1.Cells[29, 9].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes1_Sheet1.Cells[29, 9].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes1_Sheet1.Cells[29, 9].Text = "(서명 또는 인)";
            }

            ssDiabetes1_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes1_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes1_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes1_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes1_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes1_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes1_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes1_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes1_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes1_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes1_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes1_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes1_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes1_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes1_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes1_Sheet1.PrintInfo.Preview = false;
            ssDiabetes1.PrintSheet(0);

            this.Close();
        }



        public void Print2(string argGKiho, string argJumin, string argSName, string argTel, string argHPhone, string argDept,
            string argDr, string argSabun)
        {
            string strFile = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssDiabetes2_Sheet1.Cells[5, 3].Text = "";
            ssDiabetes2_Sheet1.Cells[5, 8].Text = "";
            ssDiabetes2_Sheet1.Cells[6, 3].Text = "";
            ssDiabetes2_Sheet1.Cells[6, 9].Text = "";
            ssDiabetes2_Sheet1.Cells[7, 9].Text = "";
            ssDiabetes2_Sheet1.Cells[5, 3].Text = argGKiho;
            ssDiabetes2_Sheet1.Cells[5, 8].Text = argJumin;
            ssDiabetes2_Sheet1.Cells[6, 3].Text = argSName;
            ssDiabetes2_Sheet1.Cells[6, 9].Text = argTel;
            ssDiabetes2_Sheet1.Cells[7, 9].Text = argHPhone;
            ssDiabetes2_Sheet1.Cells[9, 2].Text = argDept;
            ssDiabetes2_Sheet1.Cells[27, 4].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");
            ssDiabetes2_Sheet1.Cells[29, 4].Text = argDr;

            //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
            if (argSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes2_Sheet1.Cells[29, 9].CellType = imgCellType;
                ssDiabetes2_Sheet1.Cells[29, 9].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes2_Sheet1.Cells[29, 9].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(argSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(argSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes2_Sheet1.Cells[29, 9].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes2_Sheet1.Cells[29, 9].CellType = textCellType;
                ssDiabetes2_Sheet1.Cells[29, 9].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes2_Sheet1.Cells[29, 9].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes2_Sheet1.Cells[29, 9].Text = "(서명 또는 인)";
            }

            ssDiabetes2_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes2_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes2_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes2_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes2_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes2_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes2_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes2_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes2_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes2_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes2_Sheet1.PrintInfo.Preview = false;
            ssDiabetes2.PrintSheet(0);

            this.Close();

        }


        public void Print3(string argGKiho, string argJumin, string argSName, string argTel, string argHPhone, string argJuso, 
            string argDept, string argDr, string argDrBunho, string argSabun)

        {

            string strFile = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssDiabetes3_Sheet1.Cells[6, 3].Text = argGKiho;
            ssDiabetes3_Sheet1.Cells[8, 3].Text = argSName;
            ssDiabetes3_Sheet1.Cells[8, 13].Text = argJumin;
            ssDiabetes3_Sheet1.Cells[9, 3].Text = argTel;
            ssDiabetes3_Sheet1.Cells[9, 13].Text = argHPhone;
            ssDiabetes3_Sheet1.Cells[10, 3].Text = argJuso;
            ssDiabetes3_Sheet1.Cells[11, 1].Text = argDept;
            ssDiabetes3_Sheet1.Cells[55, 0].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");
            ssDiabetes3_Sheet1.Cells[57, 6].Text = argDr;
            ssDiabetes3_Sheet1.Cells[57, 10].Text = argDrBunho;

            //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
            if (argSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes3_Sheet1.Cells[57, 13].CellType = imgCellType;
                ssDiabetes3_Sheet1.Cells[57, 13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes3_Sheet1.Cells[57, 13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(argSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(argSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes3_Sheet1.Cells[57, 13].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes3_Sheet1.Cells[57, 13].CellType = textCellType;
                ssDiabetes3_Sheet1.Cells[57, 13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes3_Sheet1.Cells[57, 13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes3_Sheet1.Cells[57, 13].Text = "(서명 또는 인)";
            }

            ssDiabetes3_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes3_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes3_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes3_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes3_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes3_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes3_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes3_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes3_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes3_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes3_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes3_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes3_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes3_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes3_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes3_Sheet1.PrintInfo.Preview = false;
            ssDiabetes3.PrintSheet(0);

            this.Close();

        }


        public void Print4(string argGKiho, string argJumin, string argSName, string argTel, string argHPhone, string argDept,
            string argDr, string argSabun)
        {

            string strFile = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssDiabetes4_Sheet1.Cells[8, 3].Text = argGKiho;        //건강보험증번호
            ssDiabetes4_Sheet1.Cells[8, 7].Text = argJumin;        //주민번호
            ssDiabetes4_Sheet1.Cells[9, 3].Text = argSName;        //환자성명
            ssDiabetes4_Sheet1.Cells[9, 7].Text = argTel;          //자택 전화번호
            ssDiabetes4_Sheet1.Cells[10, 7].Text = argHPhone;      //휴대전화
            ssDiabetes4_Sheet1.Cells[14, 2].Text = argDept;
            ssDiabetes4_Sheet1.Cells[35, 1].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");
            ssDiabetes4_Sheet1.Cells[37, 4].Text = argDr;
            ssDiabetes4_Sheet1.Cells[38, 4].Text = argDept;

            //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
            if (argSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes4_Sheet1.Cells[37, 5].CellType = imgCellType;
                ssDiabetes4_Sheet1.Cells[37, 5].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes4_Sheet1.Cells[37, 5].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(argSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(argSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes4_Sheet1.Cells[37, 5].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes4_Sheet1.Cells[37, 5].CellType = textCellType;
                ssDiabetes4_Sheet1.Cells[37, 5].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes4_Sheet1.Cells[37, 5].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes4_Sheet1.Cells[37, 5].Text = "(서명 또는 인)";
            }

            ssDiabetes4_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes4_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes4_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes4_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes4_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes4_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes4_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes4_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes4_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes4_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes4_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes4_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes4_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes4_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes4_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes4_Sheet1.PrintInfo.Preview = false;
            ssDiabetes4.PrintSheet(0);

            this.Close();
        }


        public void Print4_20201210(string argGKiho, string argJumin, string argSName, string argTel, string argHPhone, string argDept,
            string argDr, string argSabun)
        {

            string strFile = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssDiabetes4_20201210_Sheet1.Cells[8, 3].Text = argGKiho;        //건강보험증번호
            ssDiabetes4_20201210_Sheet1.Cells[8, 7].Text = argJumin;        //주민번호
            ssDiabetes4_20201210_Sheet1.Cells[9, 3].Text = argSName;        //환자성명
            ssDiabetes4_20201210_Sheet1.Cells[9, 7].Text = argTel;          //자택 전화번호
            ssDiabetes4_20201210_Sheet1.Cells[10, 7].Text = argHPhone;      //휴대전화
            ssDiabetes4_20201210_Sheet1.Cells[14, 2].Text = argDept;
            ssDiabetes4_20201210_Sheet1.Cells[44, 1].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");
            ssDiabetes4_20201210_Sheet1.Cells[46, 4].Text = argDr;
            ssDiabetes4_20201210_Sheet1.Cells[47, 4].Text = argDept;

            //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
            if (argSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes4_20201210_Sheet1.Cells[46, 5].CellType = imgCellType;
                ssDiabetes4_20201210_Sheet1.Cells[46, 5].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes4_20201210_Sheet1.Cells[46, 5].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(argSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(argSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes4_20201210_Sheet1.Cells[46, 5].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes4_20201210_Sheet1.Cells[37, 5].CellType = textCellType;
                ssDiabetes4_20201210_Sheet1.Cells[37, 5].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes4_20201210_Sheet1.Cells[37, 5].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes4_20201210_Sheet1.Cells[37, 5].Text = "(서명 또는 인)";
            }

            ssDiabetes4_20201210_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes4_20201210_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes4_20201210_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes4_20201210_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes4_20201210_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes4_20201210_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes4_20201210_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes4_20201210_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes4_20201210_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes4_20201210_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes4_20201210_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes4_20201210_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes4_20201210_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes4_20201210_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes4_20201210_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes4_20201210_Sheet1.PrintInfo.Preview = false;
            ssDiabetes4_20201210.PrintSheet(0);

            this.Close();
        }

        public void Print5(string argGKiho, string argJumin, string argSName, string argTel, string argHPhone, string argJuso,
                   string argDept, string argDr, string argDrBunho, string argSabun)

        {

            string strFile = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssDiabetes5_Sheet1.Cells[6, 3].Text = argGKiho;
            ssDiabetes5_Sheet1.Cells[8, 3].Text = argSName;
            ssDiabetes5_Sheet1.Cells[8, 13].Text = argJumin;
            ssDiabetes5_Sheet1.Cells[9, 3].Text = argTel;
            ssDiabetes5_Sheet1.Cells[9, 13].Text = argHPhone;
            ssDiabetes5_Sheet1.Cells[10, 3].Text = argJuso;
            ssDiabetes5_Sheet1.Cells[11, 1].Text = argDept;
            ssDiabetes5_Sheet1.Cells[55, 0].Text = Convert.ToDateTime(strDate).ToString("yyyy 년 MM 월 dd 일");
            ssDiabetes5_Sheet1.Cells[57, 6].Text = argDr;
            ssDiabetes5_Sheet1.Cells[57, 10].Text = argDrBunho;

            //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
            if (argSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssDiabetes5_Sheet1.Cells[57, 13].CellType = imgCellType;
                ssDiabetes5_Sheet1.Cells[57, 13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssDiabetes5_Sheet1.Cells[57, 13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(argSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(argSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssDiabetes5_Sheet1.Cells[57, 13].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssDiabetes5_Sheet1.Cells[57, 13].CellType = textCellType;
                ssDiabetes5_Sheet1.Cells[57, 13].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssDiabetes5_Sheet1.Cells[57, 13].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssDiabetes5_Sheet1.Cells[57, 13].Text = "(서명 또는 인)";
            }

            ssDiabetes5_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDiabetes5_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDiabetes5_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes5_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDiabetes5_Sheet1.PrintInfo.Margin.Top = 60;
            ssDiabetes5_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDiabetes5_Sheet1.PrintInfo.ShowColor = false;
            ssDiabetes5_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDiabetes5_Sheet1.PrintInfo.ShowBorder = false;
            ssDiabetes5_Sheet1.PrintInfo.ShowGrid = true;
            ssDiabetes5_Sheet1.PrintInfo.ShowShadows = false;
            ssDiabetes5_Sheet1.PrintInfo.UseMax = true;
            ssDiabetes5_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDiabetes5_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDiabetes5_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDiabetes5_Sheet1.PrintInfo.Preview = false;
            ssDiabetes5.PrintSheet(0);

            this.Close();

        }



        Image SIGNATUREFILE_DBToFile(string strSabun)
        {
            Image rtnVAL = null;

            if (string.IsNullOrEmpty(strSabun)) return rtnVAL;

            string SQL = "";
            IDataReader reader = null;
            OracleCommand cmd = null;

            try
            {
                SQL = "";
                SQL = SQL + "\r\n" + "SELECT SABUN, SIGNATURE ";
                SQL = SQL + "\r\n" + "FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
                SQL = SQL + "\r\n" + "WHERE TRIM(DRCODE) = '" + strSabun + "'";

                cmd = clsDB.DbCon.Con.CreateCommand();
                cmd.InitialLONGFetchSize = -1;
                cmd.CommandText = SQL;
                cmd.CommandTimeout = 30;
                reader = cmd.ExecuteReader();

                cmd.Dispose();
                cmd = null;

                if (reader == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                }

                while (reader.Read())
                {
                    byte[] byteArray = (byte[])reader.GetValue(1);
                    MemoryStream memStream = new MemoryStream(byteArray);
                    rtnVAL = Image.FromStream(memStream);
                }
                return rtnVAL;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                return rtnVAL;
            }
        }
    }


}

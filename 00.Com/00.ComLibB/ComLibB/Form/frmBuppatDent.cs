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
    public partial class frmBuppatDent : Form
    {
        public frmBuppatDent()
        {
            InitializeComponent();
        }

        public void PrintDent1(string argIndex, string argPtno, string argSdate, 
                               string argSname, string argJumin, string argGKiho, 
                               string argHphone, string argTel, string argJuso, 
                               string argIllName, string argILLCode, string argBi)
        {
            string strIndex = argIndex;

            string strPtNo = argPtno;        //등록번호
            string strDtpSDate = argSdate;    //접수일자(공단신청일
            string strSname = argSname;       //성명(수진자, 수급권자)
            string strPJumin = argJumin;      //주민등록번호
            string strGKiho = argGKiho;       //건강보험증번호
            string strHPhone = argHphone;      //휴대전화
            string strTel = argTel;         //자택전화
            string strJuso = argJuso;        //주소
            string strIllName = argIllName;     //상병명
            string strIllCode = argILLCode;     //상병코드

            string strCHAR1 = "";
            string strCHAR2 = "";
            string strCHAR3 = "";
            string strCHAR4 = "";
            string strCHAR5 = "";

            switch (strIndex)
            {
                case "1":
                    strIllName = "사고, 발치 또는 국한성 치주병에 의한 치아상실";
                    strIllCode = "K08.1";

                    strCHAR1 = "건강보험";
                    strCHAR2 = "수진자";
                    strCHAR3 = "국민건강보험공단 이사장 귀하";
                    strCHAR4 = "건강보험증번호";
                    strCHAR5 = "②" + ComNum.VBLF + "요양기관" + ComNum.VBLF + "확인란";
                    break;
                case "2":
                    strIllName = argIllName;
                    strIllCode = argILLCode;

                    strCHAR1 = "의료급여";
                    strCHAR2 = "수급권자";
                    strCHAR3 = "시장·군수·구청장 귀하";
                    strCHAR4 = "종별";
                    strCHAR5 = "②" + ComNum.VBLF + "의료급여기관" + ComNum.VBLF + "확인란";
                    break;
            }

            //필요없음
            //Denture_Clear();

            ssDenture_Sheet1.Cells[5, 0].BackColor = Color.FromArgb(222, 222, 222);
            ssDenture_Sheet1.Cells[4, 6].BackColor = Color.FromArgb(222, 222, 222);
            ssDenture_Sheet1.Cells[5, 6].BackColor = Color.FromArgb(222, 222, 222);
            ssDenture_Sheet1.Cells[4, 25].BackColor = Color.FromArgb(222, 222, 222);

            if (strIndex == "1") { ssDenture_Sheet1.Cells[6, 28].BackColor = Color.FromArgb(222, 222, 222); }

            ssDenture_Sheet1.Cells[1, 0].Text = strCHAR1 + " 완전틀니 대상자 등록 신청서";
            ssDenture_Sheet1.Cells[6, 0].Text = "①" + strCHAR2;

            if (strIndex == "1") { ssDenture_Sheet1.Cells[9, 0].Text = "②" + ComNum.VBLF + "요양기관" + ComNum.VBLF + "확인란"; }
            else if (strIndex == "2") { ssDenture_Sheet1.Cells[9, 0].Text = strCHAR5; }

            ssDenture_Sheet1.Cells[6, 22].Text = strCHAR4;
            ssDenture_Sheet1.Cells[18, 0].Text = "위와 같이 " + strCHAR1 + " 틀니 대상자 등록을 신청합니다.";
            ssDenture_Sheet1.Cells[21, 0].Text = strCHAR2 + "와의 관계";
            ssDenture_Sheet1.Cells[23, 0].Text = strCHAR3;
            ssDenture_Sheet1.Cells[36, 0].Text = strCHAR3;

            ssDenture_Sheet1.Cells[25, 1].Text = "본인은 " + strCHAR1 + " 틀니 대상자로 신청하거나 대상자로 선정·등록된 자로, 개인정보보호법 제15조1항제1호";
            ssDenture_Sheet1.Cells[26, 1].Text = "규정에 의거하여 본인의 개인정보1)를 처리할 것을 동의합니다.";
            ssDenture_Sheet1.Cells[27, 1].Text = "본인은 " + strCHAR1 + " 틀니 대상자로 신청하거나 대상자로 선정·등록된 자로, 개인정보보호법 제23조제1호";
            ssDenture_Sheet1.Cells[28, 1].Text = "규정에 의거하여 본인의 민감정보2)를 처리할 것을 동의합니다.";
            ssDenture_Sheet1.Cells[29, 1].Text = "본인은 " + strCHAR1 + " 틀니 대상자로 신청하거나 대상자로 선정·등록된 자로, 개인정보보호법 제24조제1항제1호";
            ssDenture_Sheet1.Cells[30, 1].Text = "규정에 의거하여 본인의 고유식별정보3)를 처리할 것을 동의합니다.";

            switch (strIndex)
            {
                case "1":
                    ssDenture_Sheet1.Cells[31, 0].Text = "";
                    ssDenture_Sheet1.Cells[31, 1].Text = "";
                    ssDenture_Sheet1.Cells[32, 1].Text = "";
                    ssDenture_Sheet1.Cells[32, 25].Text = "";
                    ssDenture_Sheet1.Cells[32, 29].Text = "";
                    break;
                case "2":
                    ssDenture_Sheet1.Cells[31, 0].Text = "○";
                    ssDenture_Sheet1.Cells[31, 1].Text = "본인은 " + strCHAR1 + " 틀니 대상자로 신청하거나 대상자로 선정·등록된 자로, 개인정보보호법 제17조제1항제2호";
                    ssDenture_Sheet1.Cells[32, 1].Text = "규정에 의거하여 본인의 개인정보1)를 제3자에게 제공할 것을 동의합니다.";
                    ssDenture_Sheet1.Cells[32, 25].Text = "□ 동의함";
                    ssDenture_Sheet1.Cells[32, 29].Text = "□ 동의하지 않음";
                    break;
            }

            ssDenture_Sheet1.Cells[35, 0].Text = "④ " + strCHAR2 + " 본인";

            if (strIndex == "1") { ssDenture_Sheet1.Cells[5, 0].Text = strPtNo; }   //등록번호

            ssDenture_Sheet1.Cells[6, 6].Text = strSname;       //성명
            ssDenture_Sheet1.Cells[6, 16].Text = strPJumin;       //주민등록번호

            if (strIndex == "2")
            {
                if (argBi == "21")
                {
                    ssDenture_Sheet1.Cells[6, 28].Text = "의료급여1종";
                }
                else if (argBi == "22")
                {
                    ssDenture_Sheet1.Cells[6, 28].Text = "의료급여2종";
                }
            }

            ssDenture_Sheet1.Cells[7, 6].Text = strJuso;        //주소

            ssDenture_Sheet1.Cells[8, 8].Text = strHPhone;      //휴대전화
            ssDenture_Sheet1.Cells[8, 18].Text = strTel;        //자택번호

            ssDenture_Sheet1.Cells[9, 8].Text = strIllName;     //상병명
            ssDenture_Sheet1.Cells[9, 27].Text = strIllCode;    //상병코드

            switch (strIndex)
            {
                case "1":
                    ssDenture_Sheet1.Cells[16, 4].Text = "요양기관명(기호)";
                    break;
                case "2":
                    ssDenture_Sheet1.Cells[16, 4].Text = "의료급여기관명(기호)";
                    break;
            }

            ssDenture_Sheet1.Cells[16, 16].Text = "포항성모병원";
            ssDenture_Sheet1.Cells[16, 24].Text = "37100068";

            ssDenture_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssDenture_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssDenture_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDenture_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssDenture_Sheet1.PrintInfo.Margin.Top = 60;
            ssDenture_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssDenture_Sheet1.PrintInfo.ShowColor = true;
            ssDenture_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssDenture_Sheet1.PrintInfo.ShowBorder = false;
            ssDenture_Sheet1.PrintInfo.ShowGrid = false;
            ssDenture_Sheet1.PrintInfo.ShowShadows = false;
            ssDenture_Sheet1.PrintInfo.UseMax = false;
            ssDenture_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssDenture_Sheet1.PrintInfo.UseSmartPrint = false;
            ssDenture_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssDenture_Sheet1.PrintInfo.Preview = false;
            ssDenture.PrintSheet(0);

            this.Close();
        }

        public void PrintDent2(string argIndex, string argPtno, string argSdate,
                               string argSname, string argJumin, string argGKiho,
                               string argHphone, string argTel, string argJuso,
                                string argBi, string argE000)
        {

            string strIndex = argIndex;

            FarPoint.Win.Spread.FpSpread ssSpread = null;

            string strPtNo = argPtno;        //등록번호
            string strDtpSDate = argSdate;    //접수일자(공단신청일
            string strSname = argSname;       //성명(수진자, 수급권자)
            string strPJumin = argJumin;      //주민등록번호
            string strGKiho = argGKiho;       //건강보험증번호
            string strHPhone = argHphone;      //휴대전화
            string strTel = argTel;         //자택전화
            string strJuso = argJuso;        //주소


            if (strIndex == "3") { ssSpread = ssImplant1; } else if (strIndex == "4") { ssSpread = ssImplant2; }

            if (ssSpread == ssImplant1)
            {
                ssSpread.ActiveSheet.Cells[4, 4].Text = "";     //등록번호

                ssSpread.ActiveSheet.Cells[5, 6].Text = "";     //성명
                ssSpread.ActiveSheet.Cells[5, 16].Text = "";    //주민등록번호
                ssSpread.ActiveSheet.Cells[5, 28].Text = "";    //건강보험증번호

                ssSpread.ActiveSheet.Cells[6, 6].Text = "";     //주소
                ssSpread.ActiveSheet.Cells[6, 28].Text = "";    //휴대전화

                ssSpread.ActiveSheet.Cells[7, 8].Text = "";     //자택번호

                ssSpread.ActiveSheet.Cells[19, 16].Text = "37100068";
                ssSpread.ActiveSheet.Cells[19, 24].Text = "포항성모병원";
            }
            else if (ssSpread == ssImplant2)
            {
                ssSpread.ActiveSheet.Cells[4, 5].Text = "";     //등록번호

                ssSpread.ActiveSheet.Cells[5, 7].Text = "";     //성명
                ssSpread.ActiveSheet.Cells[5, 17].Text = "";    //주민등록번호
                ssSpread.ActiveSheet.Cells[5, 28].Text = "";

                ssSpread.ActiveSheet.Cells[6, 7].Text = "";     //주소
                ssSpread.ActiveSheet.Cells[6, 29].Text = "";     //자택번호

                ssSpread.ActiveSheet.Cells[18, 17].Text = "37100068";
                ssSpread.ActiveSheet.Cells[18, 25].Text = "포항성모병원";
            }

            ssSpread.ActiveSheet.Rows[4].BackColor = Color.FromArgb(222, 222, 222);

            if (ssSpread == ssImplant1)
            {
                ssSpread.ActiveSheet.Cells[4, 4].Text = strPtNo;        //등록번호

                ssSpread.ActiveSheet.Cells[5, 6].Text = strSname;       //성명
                ssSpread.ActiveSheet.Cells[5, 16].Text = strPJumin;     //주민등록번호
                ssSpread.ActiveSheet.Cells[5, 28].Text = strGKiho + argE000;      //건강보험증번호

                ssSpread.ActiveSheet.Cells[6, 6].Text = strJuso;        //주소
                ssSpread.ActiveSheet.Cells[6, 28].Text = strHPhone;     //휴대전화

                ssSpread.ActiveSheet.Cells[7, 8].Text = strTel;         //자택번호

                ssSpread.ActiveSheet.Cells[19, 16].Text = "37100068";
                ssSpread.ActiveSheet.Cells[19, 24].Text = "포항성모병원";
            }
            else if (ssSpread == ssImplant2)
            {
                ssSpread.ActiveSheet.Cells[4, 5].Text = strPtNo;        //등록번호

                ssSpread.ActiveSheet.Cells[5, 7].Text = strSname;       //성명
                ssSpread.ActiveSheet.Cells[5, 17].Text = strPJumin;     //주민등록번호

                if (argBi == "21") { ssSpread.ActiveSheet.Cells[5, 28].Text = "의료급여1종"; } else if (argBi == "22") { ssSpread.ActiveSheet.Cells[5, 28].Text = "의료급여2종"; }

                ssSpread.ActiveSheet.Cells[6, 7].Text = strJuso;        //주소
                ssSpread.ActiveSheet.Cells[6, 29].Text = strTel;         //자택번호

                ssSpread.ActiveSheet.Cells[18, 17].Text = "37100068";
                ssSpread.ActiveSheet.Cells[18, 25].Text = "포항성모병원";
            }

            ssSpread.ActiveSheet.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssSpread.ActiveSheet.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssSpread.ActiveSheet.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssSpread.ActiveSheet.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssSpread.ActiveSheet.PrintInfo.Margin.Top = 60;
            ssSpread.ActiveSheet.PrintInfo.Margin.Bottom = 20;
            ssSpread.ActiveSheet.PrintInfo.ShowColor = true;
            ssSpread.ActiveSheet.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssSpread.ActiveSheet.PrintInfo.ShowBorder = false;
            ssSpread.ActiveSheet.PrintInfo.ShowGrid = false;
            ssSpread.ActiveSheet.PrintInfo.ShowShadows = false;
            ssSpread.ActiveSheet.PrintInfo.UseMax = false;
            ssSpread.ActiveSheet.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssSpread.ActiveSheet.PrintInfo.UseSmartPrint = false;
            ssSpread.ActiveSheet.PrintInfo.ShowPrintDialog = false;
            ssSpread.ActiveSheet.PrintInfo.Preview = false;
            ssSpread.PrintSheet(0);

            this.Close();
        }

        public void PrintRespirator(string argSname, string argJumin, string argGKiho, string argGstrE000, 
                                    string argTel, string argHphone, string argDept, string argDr)
        {

            string strFile = "";
            string strSabun = "";
            string strDate = ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-");

            ssRespirator_Sheet1.Cells[5, 4].Text = argSname;       //성명

            ssRespirator_Sheet1.Cells[6, 8].Text = argJumin;       //주민번호
            ssRespirator_Sheet1.Cells[6, 15].Text = argGKiho + argGstrE000;      //건강보험증번호

            ssRespirator_Sheet1.Cells[7, 7].Text = argTel;         //전화번호

            ssRespirator_Sheet1.Cells[8, 7].Text = argHphone;      //휴대폰번호

            ssRespirator_Sheet1.Cells[10, 5].Text = clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, VB.Left(argDept, 2));  //진료과목

            ssRespirator_Sheet1.Cells[35, 3].Text = Convert.ToDateTime(strDate).ToString("yyyy   년        MM   월        dd   일");
            ssRespirator_Sheet1.Cells[42, 1].Text = Convert.ToDateTime(strDate).ToString("yyyy   년        MM   월        dd   일");

            if (argDr.Replace(".", "") != "")
            {
                strSabun = VB.Left(argDr, 4);
                ssRespirator_Sheet1.Cells[37, 10].Text = "  " + VB.Split(argDr, ".")[1];
                ssRespirator_Sheet1.Cells[37, 10].Text = ssRespirator_Sheet1.Cells[37, 10].Text + "    (" + clsVbfunc.GetOCSDrBunhoDrCode(clsDB.DbCon, VB.Split(argDr, ".")[0]) + ")";
            }
            else
            {
                ssRespirator_Sheet1.Cells[37, 10].Text = "";
            }

            //의사 사인 이미지 로드 ------------------------------------------------------------------------------------
            if (strSabun != "")
            {
                FarPoint.Win.Spread.CellType.ImageCellType imgCellType = new FarPoint.Win.Spread.CellType.ImageCellType();
                imgCellType.Style = FarPoint.Win.RenderStyle.StretchAndScale;

                ssRespirator_Sheet1.Cells[37, 17].CellType = imgCellType;
                ssRespirator_Sheet1.Cells[37, 17].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
                ssRespirator_Sheet1.Cells[37, 17].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;

                strFile = "C:\\CMC\\" + ComFunc.LPAD(strSabun, 5, "0") + ".jpg";

                Image image = SIGNATUREFILE_DBToFile(strSabun);

                //해당하는 의사의 이미지를 DB에서 자동으로 화일로 만드는 작업
                if (image != null)
                {
                    ssRespirator_Sheet1.Cells[37, 17].Value = image;
                }
            }
            else
            {
                FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
                textCellType.WordWrap = false;
                textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.None;

                ssRespirator_Sheet1.Cells[37, 17].CellType = textCellType;
                ssRespirator_Sheet1.Cells[37, 17].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Left;
                ssRespirator_Sheet1.Cells[37, 17].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
                ssRespirator_Sheet1.Cells[37, 17].Text = "(서명 또는 인)";
            }

            ssRespirator_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssRespirator_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssRespirator_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssRespirator_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssRespirator_Sheet1.PrintInfo.Margin.Top = 60;
            ssRespirator_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssRespirator_Sheet1.PrintInfo.ShowColor = false;
            ssRespirator_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssRespirator_Sheet1.PrintInfo.ShowBorder = false;
            ssRespirator_Sheet1.PrintInfo.ShowGrid = true;
            ssRespirator_Sheet1.PrintInfo.ShowShadows = false;
            ssRespirator_Sheet1.PrintInfo.UseMax = true;
            ssRespirator_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssRespirator_Sheet1.PrintInfo.UseSmartPrint = false;
            ssRespirator_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssRespirator_Sheet1.PrintInfo.Preview = false;
            ssRespirator.PrintSheet(0);
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

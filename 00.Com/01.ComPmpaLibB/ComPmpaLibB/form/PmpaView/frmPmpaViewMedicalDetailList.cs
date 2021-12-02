using System;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using ComBase; //기본 클래스
using ComDbB;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMedicalDetailList.cs
    /// Description     : 진료비 상세내역 New
    /// Author          : 김효성
    /// Create Date     : 2017-09-04
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 약제상한코드포함 체크버튼 사용 하지 않음
    /// 전역변수 VB (Fstr임시자격_rowid) -> C# FstrTempJagugRowid
    /// </history>
    /// <seealso cref= "\psmh\IPD\iument\iument.vbp(Frm진료비상세내역_NEW) >> frmPmpaViewMedicalDetailList.cs 폼이름 재정의" />	

    public partial class frmPmpaViewMedicalDetailList : Form
    {
        string GstrHelpCode = "";
        string FstrTempJagugRowid = "";
        string FstrJob = "";
        string FstrHangDtl = "";
        string FstrHang = "";
        string FstrBDate = "";
        string FstrSuNext = "";
        string FstrBBBBBB = "";
        long FnIPDNO = 0;
        long FnTRSNO = 0;

        string FstrHUbyAct = "";
        //
        int nRow = 0;
        
        /// <summary>
        /// Discription : 항목별 진료비 상세내역 조회
        /// Author : 김민철
        /// Create Date : 2018.04.07
        /// <param name="strJob">작업구분</param>
        /// <param name="strHang">분류번호</param>
        /// <param name="nIPDNO">입원번호</param>
        /// <param name="nTRSNO">자격번호</param>
        /// <param name="strTempJagugRowid"></param>
        /// <param name="strHelpCode"></param>
        /// <param name="strHangDtl">분류이름</param>
        /// <param name="strBDate">처방일자</param>
        /// <param name="strSuNext">품목코드</param>
        /// </summary>
        public frmPmpaViewMedicalDetailList(string strJob, string strHang, long nIPDNO, long nTRSNO, string strTempJagugRowid, string strHelpCode, string strHangDtl, string strBDate, string strSuNext, string strHUbyAct = "")
        {
            InitializeComponent();
            SetEvents();

            FstrJob = strJob;
            FstrHang = strHang;             //분류번호
            FnIPDNO = nIPDNO;
            FnTRSNO = nTRSNO;
            FstrHangDtl = strHangDtl;       //항목이름
            FstrTempJagugRowid = strTempJagugRowid;
            GstrHelpCode = strHelpCode;
            FstrBDate = strBDate;
            FstrSuNext = strSuNext;
            FstrHUbyAct = strHUbyAct;       //호스피스 행위별
        }

        public frmPmpaViewMedicalDetailList()
        {
            InitializeComponent();
            SetEvents();
        }


        void SetEvents()
        {
            this.btnSubClose.Click += new EventHandler(eBtnClick);

        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSubClose)
            {
                btnSubClose_Click();
            }
        }

        private void frmPmpaViewMedicalDetailList_Load(object sender, EventArgs e)
        {
            chksel.Checked = false;
            panSub1.Visible = false;

            SCREEN_CLEAR();
            
            if(FstrHUbyAct == "1")
            {
                chkHU.Checked = true;
            }

            ssView_Sheet1.Columns[9].Visible = false;
            chkdetail.Checked = false;

            if (FstrJob == "상세내역")    //1
            {
                chkdetail.Checked = true;
            }

            Display_IPD_Master(FnIPDNO, FnTRSNO);   //2

            if (FstrHang == "")
            {
                Screen_Display_Detail_SUGA(FnIPDNO, FnTRSNO);
            }
            else if (chkdetail.Checked == true)
            {
                Screen_Display_Detail(FnIPDNO, FnTRSNO);    //3
            }
            else if (FstrHang == "DRG")
            {
                Screen_Display_Slip(clsDB.DbCon, FstrHangDtl);
            }
            else
            {
                Screen_display(FnIPDNO, FnTRSNO);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strDate = "";
            string[] strDay = new string[4];

            for (i = 0; i <= 3; i++)
            {
                strDay[i] = "";
            }
            panSub1.Visible = true;

            Cursor.Current = Cursors.WaitCursor;

            dtpPanel.Value.ToString("20170522");//2017 - 05 - 22

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT DIETNAME, DIETDAY, BUN FROM DIET_NEWORDER ";
                SQL = SQL + ComNum.VBLF + " WHERE ACTDATE = TO_DATE('" + dtpPanel.Value.ToString("2017-05-22") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND PANO  = '" + clsPmpaType.TIT.Pano + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER BY DIETDAY, BUN ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    switch (dt.Rows[i]["DIETDAY"].ToString().Trim())
                    {
                        case "1":
                            strDay[1] = strDay[1] + dt.Rows[i]["DIETNAME"].ToString().Trim();
                            break;
                        case "2":
                            strDay[2] = strDay[2] + dt.Rows[i]["DIETNAME"].ToString().Trim();
                            break;
                        case "3":
                            strDay[3] = strDay[3] + dt.Rows[i]["DIETNAME"].ToString().Trim();
                            break;

                    }
                }
                dt.Dispose();
                dt = null;

                if (i > 0)
                {
                    for (i = 1; i <= 3; i++)
                    {
                        strDay[i] = VB.Left(strDay[i], VB.Len(strDay[i]) - 1);
                    }
                }

                ssSub_Sheet1.Cells[0, 0].Text = strDay[1];
                ssSub_Sheet1.Cells[0, 1].Text = strDay[2];
                ssSub_Sheet1.Cells[0, 2].Text = strDay[3];

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;//권한 확인
            }

            Cursor.Current = Cursors.WaitCursor;

            string strHead1 = "";
            string strHead2 = "";
            string strFont1 = "";
            string strFont2 = "";
            string sFont3 = "";
            string sFoot = "";
            string strPano = "";
            string strSname = "";
            string strSex = "";
            string strInDate = "";
            string strOutDate = "";
            int nIlsu = 0;
            DateTime mdtp;
            mdtp = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A"));

            strPano = SSInfo_Sheet1.Cells[0, 0].Text;
            strSname = SSInfo_Sheet1.Cells[0, 1].Text;
            strSex = SSInfo_Sheet1.Cells[0, 2].Text;
            strInDate = SSInfo_Sheet1.Cells[0, 5].Text;
            strOutDate = SSInfo_Sheet1.Cells[0, 6].Text;
            nIlsu = Convert.ToInt32(VB.Val(SSInfo_Sheet1.Cells[0, 7].Text));

            strFont1 = "/c/fn\"굴림체\" /fz\"16\"  /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + " 진 료 비  상 세 내 역 " + "/f1/n";
            strHead2 = "/n/l/c/f2" + "등록번호 : " + strPano + "  성명: " + strSname + "(" + strSex + ") ";
            strHead2 = strHead2 + VB.Space(3) + "입원기간: " + strInDate + " ~ " + strOutDate + " (";
            strHead2 = strHead2 + nIlsu + "일) ";

            btnPrint.Enabled = false;

            ssView_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssView_Sheet1.PrintInfo.ZoomFactor = 0.85f;
            ssView_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n/n/n" + strFont2 + strHead2 + "/n";
            ssView_Sheet1.PrintInfo.Footer = sFont3 + sFoot;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 50;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 50;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = false;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = 0;
            ssView.PrintSheet(0);

            btnPrint.Enabled = true;

            Cursor.Current = Cursors.Default;
        }
        
        private void chkdetail_CheckedChanged(object sender, EventArgs e)
        {
            if (chkdetail.Checked == true)
            {
                FstrJob = "상세내역";
                View_Display();
            }
            else
            {
                FstrJob = "내역";
                View_Display();
            }
        }
        
        private void Display_IPD_Master(long ArgIpdNo, long ArgTRSNO)
        {

            clsIument ci = new clsIument();

            ci.Read_Ipd_Master(clsDB.DbCon, "", ArgIpdNo);
            ci.Read_Ipd_Mst_Trans(clsDB.DbCon, clsPmpaType.IMST.Pano, ArgTRSNO, "");

            SSInfo_Sheet1.Cells[0, 0].Text = clsPmpaType.TIT.Pano;
            SSInfo_Sheet1.Cells[0, 1].Text = clsPmpaType.TIT.Sname;
            SSInfo_Sheet1.Cells[0, 2].Text = clsPmpaType.TIT.Age + "/" + clsPmpaType.TIT.Sex;
            SSInfo_Sheet1.Cells[0, 3].Text = clsPmpaType.IMST.WardCode;
            SSInfo_Sheet1.Cells[0, 4].Text = clsPmpaType.IMST.RoomCode.ToString();
            SSInfo_Sheet1.Cells[0, 5].Text = clsPmpaType.TIT.InDate;
            SSInfo_Sheet1.Cells[0, 5].Text = clsPmpaType.TIT.OutDate;
            SSInfo_Sheet1.Cells[0, 7].Text = clsPmpaType.TIT.Ilsu.ToString();
            SSInfo_Sheet1.Cells[0, 8].Text = clsPmpaType.TIT.Bi;
            SSInfo_Sheet1.Cells[0, 9].Text = clsPmpaType.TIT.DeptCode;
            SSInfo_Sheet1.Cells[0, 10].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, clsPmpaType.TIT.DrCode);
            SSInfo_Sheet1.Cells[0, 11].Text = clsPmpaType.TIT.Bohun;
            SSInfo_Sheet1.Cells[0, 12].Text = clsPmpaType.TIT.Ipdno.ToString();
            SSInfo_Sheet1.Cells[0, 13].Text = clsPmpaType.TIT.Trsno.ToString();
            SSInfo_Sheet1.Cells[0, 14].Text = clsPmpaType.TIT.Jumin1 + "-" + clsPmpaType.TIT.Jumin2;

            ci = null;
        }

        private void Screen_display(long ArgIpdNo, long ArgTRSNO)
        {
            int nBun = 0;
            int i = 0;
            int nRow = 0;
            string strBun = "";
            string strNu = "";
            int nHangFrom = 0;
            int nHangTo = 0;
            string strOK = "";
            double nSubTot1 = 0;
            double nSubTot2 = 0;
            double nSubTot3 = 0;
            double nTot1 = 0;
            double nTot2 = 0;
            double nTot3 = 0;
            bool bTitleDisplay = false;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            ComFunc cf = new ComFunc();

            try
            {
                ssView_Sheet1.ColumnHeader.Cells[0, 0].Text = "처방일자";

                SQL = "";
                //'해당 환자의 자료를 Select
                SQL = SQL + ComNum.VBLF + "SELECT a.Bun,a.Nu,a.SuCode,a.SuNext,b.SuNameK,a.BaseAmt,a.Qty,a.GbSelf,";
                if (chkHU.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " SUM(a.Nal * -1) Nal,SUM(Amt1 * -1) Amt,SUM(Amt2 * -1) Amt2 ,";
                    SQL = SQL + ComNum.VBLF + " FC_ACCOUNT_BON_AMT(a.pano,a.bi,a.SuNext,SUM(Amt1 * -1) ,Qty,bun,nu,to_char(min(a.bdate),'yyyy-mm-dd'),'I','**',gbsugbs,trsno,GbSelf) BON";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " SUM(a.Nal) Nal,SUM(Amt1) Amt,SUM(Amt2) Amt2 ,";
                    SQL = SQL + ComNum.VBLF + " FC_ACCOUNT_BON_AMT(a.pano,a.bi,a.SuNext,SUM(Amt1) ,Qty,bun,nu,to_char(min(a.bdate),'yyyy-mm-dd'),'I','**',gbsugbs,trsno,GbSelf) BON";
                }
                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                }

                if (ArgTRSNO > 0)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE a.TRSNO = " + ArgTRSNO + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE a.IPDNO = " + ArgIpdNo + " ";
                }

                if (FstrTempJagugRowid != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND a.BDate>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND a.BDate<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD') ";
                }
                if (chkHU.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND TRIM(PART) = '!-' ";
                }
                SQL = SQL + ComNum.VBLF + "  AND a.SUNEXT NOT IN ('DRG001','DRG002')                   ";
                SQL = SQL + ComNum.VBLF + "  AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )           ";// '간호행위제외
                SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";

                if (FstrBBBBBB != "Y")
                {
                    SQL = SQL + ComNum.VBLF + " AND TRIM(a.SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";  //'저가약제 제외코드 2011-04-01
                }

                if (FstrHang == "22")
                {
                    SQL = SQL + ComNum.VBLF + "  AND a.GBSUGBS IN ('3','4','5','6','7','8','9') ";
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY a.pano,a.bi,a.Bun,a.Nu,a.SuCode,a.SuNext,b.SuNameK,a.BaseAmt,a.Qty,a.GbSelf,gbsugbs,trsno ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.Bun,a.Nu,a.SuCode,a.SuNext,a.BaseAmt ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                nTot1 = 0;
                nTot2 = 0;
                nTot3 = 0;

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                if (FstrHang == "00")
                {
                    nHangFrom = 1;
                    nHangTo = 22;
                }
                else
                {
                    nHangFrom = (int)VB.Val(FstrHang);
                    nHangTo = (int)VB.Val(FstrHang);
                }
                
                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (nBun = nHangFrom; nBun <= nHangTo; nBun++)
                {
                    nSubTot1 = 0;
                    nSubTot2 = 0;
                    nSubTot3 = 0;
                    bTitleDisplay = false;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strBun = dt.Rows[i]["Bun"].ToString().Trim();
                        strNu = dt.Rows[i]["Nu"].ToString().Trim();
                        strOK = "OK";

                        if (Bill_ItemNo_Check(strNu, strBun) != nBun && nBun != 22)
                        {
                            strOK = "";
                        }
                        if (VB.Val(dt.Rows[i]["AMT"].ToString().Trim()) == 0)
                        {
                            strOK = "";
                        }

                        if (strOK == "OK")
                        {
                            nRow = nRow + 1;
                            if (nRow > ssView_Sheet1.RowCount)
                            {
                                ssView_Sheet1.RowCount = nRow;
                            }
                            ssView_Sheet1.RowCount = nRow;
                            // '진료비영수증 항목명칭을 표시
                            if (bTitleDisplay == false)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = cf.Read_Bcode_Name(clsDB.DbCon, "BAS_진료비영수증항목", nBun.ToString("00"));
                                bTitleDisplay = true;
                            }

                            ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = " " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Val(dt.Rows[i]["BASEAMT"].ToString().Trim()).ToString("#,##0");
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("#0.00");
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["NAL"].ToString().Trim()).ToString("#0");

                            if (dt.Rows[i]["GbSelf"].ToString().Trim() == "0")
                            {
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = VB.Val(dt.Rows[i]["AMT"].ToString().Trim()).ToString("#,##0");
                                nSubTot1 = nSubTot1 + VB.Val(dt.Rows[i]["AMt"].ToString().Trim());
                                nTot1 = nTot1 + VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                            }
                            else
                            {
                                ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["AMT"].ToString().Trim()).ToString("#,##0");
                                nSubTot2 = nSubTot2 + VB.Val(dt.Rows[i]["AMt"].ToString().Trim());
                                nTot2 = nTot2 + VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                            }
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt.Rows[i]["BON"].ToString().Trim()).ToString("#,##0");
                            nSubTot3 = nSubTot3 + VB.Val(dt.Rows[i]["BON"].ToString().Trim());
                            nTot3 = nTot3 + VB.Val(dt.Rows[i]["BON"].ToString().Trim());
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["BUN"].ToString().Trim();

                            //2021-11-09 DRG 환자일 경우 본인부담 코드 색 변경
                            if (clsPmpaType.TIT.DrgCode != "")
                            {
                                if (Check_DRG_F(dt.Rows[i]["SUNEXT"].ToString().Trim()) == "OK")
                                {
                                    ssView_Sheet1.Cells[nRow - 1, 3, nRow - 1, 8].BackColor = Color.PeachPuff;
                                }
                            }

                        }
                    }

                    if (nSubTot1 != 0 || nSubTot2 != 0 || nSubTot3 != 0 || bTitleDisplay == true)
                    {
                        nRow = nRow + 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }

                        ssView_Sheet1.RowCount = nRow;
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 소  계 **";
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = nSubTot1.ToString("#,##0");
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = nSubTot2.ToString("#,##0");
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = nSubTot3.ToString("#,##0");
                    }

                }
                dt.Dispose();
                dt = null;

                if (FstrHang != "00")
                {
                    ssView_Sheet1.RowCount = nRow;
                }
                else
                {
                    nRow = nRow + 1;
                    ssView_Sheet1.RowCount = nRow;
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 합  계 **";
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = nTot1.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = nTot2.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTot3.ToString("#,##0");
                }

                if (FstrHangDtl != "00")
                {
                    this.Text = this.Text + " [" + FstrHangDtl + "]";
                }
                else
                {
                    this.Text = "진료비 상세내역 NEW";
                }

                Cursor.Current = Cursors.Default;

                cf = null;
            }
            catch (Exception ex)
            {
                cf = null;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private string Check_DRG_F(string argSunext)
        {
            //DRG 본인부담 수가 체크 2021-11-09
            //DRG 인 경우에만 사용.
            
            DataTable dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            string strOK = "";
            SQL += ComNum.VBLF + " SELECT * FROM ( ";
            SQL += ComNum.VBLF + "      SELECT '1' GBN, SUNEXT  ";
            SQL += ComNum.VBLF + "        FROM KOSMOS_PMPA.BAS_SUGA_DRGADD_NEW ";
            SQL += ComNum.VBLF + "       WHERE DRGF >= ' ' OR DRG100 >= ' '  ";
            SQL += ComNum.VBLF + "      UNION ALL ";
            SQL += ComNum.VBLF + "      SELECT '2' GBN, SUNEXT  ";
            SQL += ComNum.VBLF + "        FROM KOSMOS_PMPA.BAS_SUN ";
            SQL += ComNum.VBLF + "       WHERE DRGF = 'Y' ";
            SQL += ComNum.VBLF + "               )";
            SQL += ComNum.VBLF + " WHERE SUNEXT = '" + argSunext + "'";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (dt.Rows.Count > 0)
            {
                strOK = "OK";
            }
            dt.Dispose();
            dt = null;

            return strOK;
        }

        private void Screen_Display_Detail(long ArgIpdNo, long ArgTRSNO)
        {
            int nBun = 0;
            int i = 0;
            int nRow = 0;
            string strBun = "";
            string strNu = "";
            int nHangFrom = 0;
            int nHangTo = 0;
            string strOLD = "";
            string strNew = "";
            string strOK = "";
            double nSubTot1 = 0;
            double nSubTot2 = 0;
            double nSubTot3 = 0;
            double nTot1 = 0;
            double nTot2 = 0;
            double nTot3 = 0;
            bool bTitleDisplay = false;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            ComFunc cf = new ComFunc();

            try
            {
                ssView_Sheet1.ColumnHeader.Cells[0, 0].Text = "처방일자";

                SQL = "";
                //'해당 환자의 자료를 Select
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(a.BDate,'YYYYMMDD') BDate,";
                SQL = SQL + ComNum.VBLF + " a.Bun,a.Nu,a.SuCode,a.SuNext,b.SuNameK,a.BaseAmt,a.Qty,a.GbSelf,";
                SQL = SQL + ComNum.VBLF + " SUM(a.Nal) Nal,SUM(Amt1) Amt,SUM(Amt2) Amt2 ";

                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                }

                if (ArgTRSNO > 0)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE a.TRSNO = " + ArgTRSNO + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE a.IPDNO = " + ArgIpdNo + " ";
                }

                //'2012-09-07
                if (FstrTempJagugRowid != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND a.BDate>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND a.BDate<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD') ";
                }

                if (FstrBDate != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND a.BDate=TO_DATE('" + FstrBDate + "','YYYY-MM-DD') ";
                }
                if (FstrSuNext != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND a.SuNext='" + FstrSuNext + "' ";
                }
                SQL = SQL + ComNum.VBLF + " AND a.SuNext=b.SuNext(+) ";
                SQL = SQL + ComNum.VBLF + " AND a.SuNext NOT IN ('DRG001','DRG002') ";


                if (FstrBBBBBB != "Y")
                {
                    SQL = SQL + ComNum.VBLF + " AND TRIM(a.SUNEXT) NOT IN ( SELECT CODE FROM " + ComNum.DB_PMPA + "BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";  //'저가약제 제외코드 2011-04-01
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY a.BDate,a.Bun,a.Nu,a.SuCode,a.SuNext,b.SuNameK,a.BaseAmt,a.Qty,a.GbSelf ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.BDate,a.Bun,a.Nu,a.SuCode,a.SuNext ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                nTot1 = 0;
                nTot2 = 0;
                nTot3 = 0;

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //'약품반납대장 상세조회
                if (FstrHang == "")
                {
                    Screen_Display_Detail_SUB(ref bTitleDisplay, dt, ref strBun, ref strNu, ref strOK, ref nBun, ref strNew, ref strOLD, ref nSubTot1, ref nSubTot2, ref nSubTot3, ref nTot1, ref nTot2, ref nTot3);
                    return;
                }
                if (FstrHang == "00")
                {
                    nHangFrom = 1;
                    nHangTo = 22;
                }
                else
                {
                    nHangFrom = Convert.ToInt32(VB.Val(FstrHang));
                    nHangTo = Convert.ToInt32(VB.Val(FstrHang));
                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (nBun = nHangFrom; nBun <= nHangTo; nBun++)
                {
                    nSubTot1 = 0;
                    nSubTot2 = 0;
                    nSubTot3 = 0;
                    bTitleDisplay = false;
                    strOLD = "";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strBun = dt.Rows[i]["Bun"].ToString().Trim();
                        strNu = dt.Rows[i]["Nu"].ToString().Trim();
                        strOK = "OK";

                        if (Bill_ItemNo_Check(strNu, strBun) != nBun)
                        {
                            strOK = "";
                        }
                        if (VB.Val(dt.Rows[i]["AMT"].ToString().Trim()) == 0 && strBun != "74")
                        {
                            strOK = "";
                        }
                        if (strOK == "OK")
                        {
                            // '진료비영수증 항목명칭을 표시
                            if (bTitleDisplay == false)
                            {
                                nRow = nRow + 1;
                                if (nRow > ssView_Sheet1.RowCount)
                                {
                                    ssView_Sheet1.RowCount = nRow;
                                }
                                ssView_Sheet1.Cells[nRow - 1, 2].Text = " ▶" + cf.Read_Bcode_Name(clsDB.DbCon, "BAS_진료비영수증항목", nBun.ToString("00"));
                                bTitleDisplay = true;
                            }

                            nRow = nRow + 1;
                            if (nRow > ssView_Sheet1.RowCount)
                            {
                                ssView_Sheet1.RowCount = nRow;
                            }
                            ssView_Sheet1.RowCount = nRow;

                            strNew = dt.Rows[i]["BDATE"].ToString().Trim();
                            if (strNew != strOLD)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = strNew;
                                strOLD = strNew;
                            }

                            ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = " " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Val(dt.Rows[i]["BASEAMT"].ToString().Trim()).ToString("#,##0");
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("#0.0");
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["NAL"].ToString().Trim()).ToString("#0");

                            if (dt.Rows[i]["GbSelf"].ToString().Trim() == "0")
                            {
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("#,##0");
                                nSubTot1 = nSubTot1 + VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                                nTot1 = nTot1 + VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                            }
                            else
                            {
                                ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["Amt"].ToString().Trim()).ToString("#,##0");
                                nSubTot2 = nSubTot2 + VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                                nTot2 = nTot2 + VB.Val(dt.Rows[i]["Amt"].ToString().Trim());
                            }
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt.Rows[i]["Amt2"].ToString().Trim()).ToString("#,##0");
                            nSubTot3 = nSubTot3 + VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());
                            nTot3 = nTot3 + VB.Val(dt.Rows[i]["Amt2"].ToString().Trim());
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["BUN"].ToString().Trim();

                        }
                    }
                    if (nSubTot1 != 0 || nSubTot2 != 0 || nSubTot3 != 0 || bTitleDisplay == true)
                    {
                        nRow = nRow + 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 소  계 **";
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = nSubTot1.ToString("#,##0");
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = nSubTot2.ToString("#,##0");
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = nSubTot3.ToString("#,##0");
                    }
                }
                dt.Dispose();
                dt = null;

                if (FstrHang != "00")
                {
                    ssView_Sheet1.RowCount = nRow;
                }
                else
                {
                    nRow = nRow + 1;
                    ssView_Sheet1.RowCount = nRow;
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 합  계 **";
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = nTot1.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = nTot2.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTot3.ToString("#,##0");
                }

                if (FstrHangDtl != "00")
                {
                    this.Text = this.Text + " [" + FstrHangDtl + "]";
                }
                else
                {
                    this.Text = "진료비 상세내역 NEW";
                }

                Cursor.Current = Cursors.Default;
                cf = null;

            }
            catch (Exception ex)
            {
                cf = null;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Screen_Display_Detail_SUGA(long ArgIpdNo, long ArgTRSNO)
        {
            int i = 0;
            int nRow = 0;
            string strBun = "";
            string strNu = "";
            string strOLD = "";
            string strNew = "";
            double nSubTot1 = 0;
            double nSubTot2 = 0;
            double nSubTot3 = 0;
            double nTot1 = 0;
            double nTot2 = 0;
            double nTot3 = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                ssView_Sheet1.ColumnHeader.Cells[0, 0].Text = "처방일자";

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(a.BDate,'YYYYMMDD') BDate,";
                SQL = SQL + ComNum.VBLF + " a.Bun,a.Nu,a.SuCode,a.SuNext,b.SuNameK,a.BaseAmt,a.Qty,a.GbSelf,";
                SQL = SQL + ComNum.VBLF + " a.Nal,(Amt1) Amt,(Amt2) Amt2 ";

                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                }

                if (ArgTRSNO > 0)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE a.TRSNO = " + ArgTRSNO + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE a.IPDNO = " + ArgIpdNo + " ";
                }
                SQL = SQL + ComNum.VBLF + " AND a.Bun IN ('11','12','20','23') ";
                SQL = SQL + ComNum.VBLF + " AND a.SuNext NOT IN ('DRG001','DRG002') ";

                //'2012-09-07
                if (FstrTempJagugRowid != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND a.BDate>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND a.BDate<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD') ";
                }
                if (FstrBDate != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND a.BDate=TO_DATE('" + FstrBDate + "','YYYY-MM-DD') ";
                }
                if (FstrSuNext != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND a.SuNext='" + FstrSuNext + "' ";
                }
                SQL = SQL + ComNum.VBLF + " AND a.SuNext=b.SuNext(+) ";


                if (FstrBBBBBB != "Y")
                {
                    SQL = SQL + ComNum.VBLF + " AND TRIM(a.SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";  //'저가약제 제외코드 2011-04-01
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY a.BDate,a.Bun,a.Nu,a.SuCode,a.SuNext ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                nTot1 = 0;
                nTot2 = 0;
                nTot3 = 0;

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strBun = dt.Rows[i]["Bun"].ToString().Trim();
                    strNu = dt.Rows[i]["Nu"].ToString().Trim();
                    nRow = nRow + 1;

                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.RowCount = nRow;

                    strNew = dt.Rows[i]["BDATE"].ToString().Trim();
                    if (strNew != strOLD)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strNew;
                        strOLD = strNew;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = " " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Val(dt.Rows[i]["BASEAMT"].ToString().Trim()).ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["NAL"].ToString().Trim()).ToString("#,##0");

                    if (dt.Rows[i]["GbSelf"].ToString().Trim() == "0")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = VB.Val(dt.Rows[i]["AMT"].ToString().Trim()).ToString("#,##0");
                        nSubTot1 = nSubTot1 + VB.Val(dt.Rows[i]["AMt"].ToString().Trim());
                        nTot1 = nTot1 + VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["AMT"].ToString().Trim()).ToString("#,##0");
                        nSubTot2 = nSubTot2 + VB.Val(dt.Rows[i]["AMt"].ToString().Trim());
                        nTot2 = nTot2 + VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                    }
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt.Rows[i]["AMT2"].ToString().Trim()).ToString("#,##0");
                    nSubTot3 = nSubTot3 + VB.Val(dt.Rows[i]["AMt2"].ToString().Trim());
                    nTot3 = nTot3 + VB.Val(dt.Rows[i]["AMT2"].ToString().Trim());
                }

                nRow = nRow + 1;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 합  계 **";
                ssView_Sheet1.Cells[nRow - 1, 6].Text = nTot1.ToString("#,##0");
                ssView_Sheet1.Cells[nRow - 1, 7].Text = nTot2.ToString("#,##0");
                ssView_Sheet1.Cells[nRow - 1, 8].Text = nTot3.ToString("#,##0");

                dt.Dispose();
                dt = null;

                if (FstrHangDtl != "00")
                {
                    this.Text = this.Text + " [" + FstrHangDtl + "]";
                }
                else
                {
                    this.Text = "진료비 상세내역 NEW";
                }

                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

        }

        private void Screen_Display_SEL(long ArgIpdNo, long ArgTRSNO)
        {
            int nBun = 0;
            int i = 0;
            int nRow = 0;
            string strBun = "";
            string strNu = "";
            int nHangFrom = 0;
            int nHangTo = 0;
            string strOK = "";
            double nSubTot1 = 0;
            double nSubTot2 = 0;
            double nSubTot3 = 0;
            double nTot1 = 0;
            double nTot2 = 0;
            double nTot3 = 0;
            bool bTitleDisplay = false;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            ComFunc cf = new ComFunc();

            try
            {
                ssView_Sheet1.ColumnHeader.Cells[0, 0].Text = "처방일자";

                SQL = "";
                //'해당 환자의 자료를 Select
                SQL = SQL + ComNum.VBLF + "SELECT a.Bun,a.Nu,a.SuCode,a.SuNext,b.SuNameK,a.BaseAmt,a.Qty,a.GbSelf,";
                SQL = SQL + ComNum.VBLF + " SUM(a.Nal) Nal,SUM(Amt1) Amt,SUM(Amt2) Amt2 ";

                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                }

                if (ArgTRSNO > 0)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE a.TRSNO = " + ArgTRSNO + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE a.IPDNO = " + ArgIpdNo + " ";
                }

                SQL = SQL + ComNum.VBLF + "  AND (b.WONCODE NOT IN('1118') OR b.WONCODE IS NULL )           ";// '간호행위제외
                SQL = SQL + ComNum.VBLF + "  AND a.SuNext=b.SuNext(+) ";
                SQL = SQL + ComNum.VBLF + "  AND a.Amt2 <> 0 ";//  '선택금액만

                if (FstrBBBBBB != "Y")
                {
                    SQL = SQL + ComNum.VBLF + " AND TRIM(a.SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";  //'저가약제 제외코드 2011-04-01
                }

                SQL = SQL + ComNum.VBLF + "GROUP BY a.Bun,a.Nu,a.SuCode,a.SuNext,b.SuNameK,a.BaseAmt,a.Qty,a.GbSelf ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.Bun,a.Nu,a.SuCode,a.SuNext,a.BaseAmt ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                nTot1 = 0;
                nTot2 = 0;
                nTot3 = 0;

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                nHangFrom = 1;
                nHangTo = 22;

                for (nBun = nHangFrom; nBun <= nHangTo; nBun++)
                {
                    nSubTot1 = 0;
                    nSubTot2 = 0;
                    nSubTot3 = 0;
                    bTitleDisplay = false;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strBun = dt.Rows[i]["Bun"].ToString().Trim();
                        strNu = dt.Rows[i]["Nu"].ToString().Trim();
                        strOK = "OK";

                        if (Bill_ItemNo_Check(strNu, strBun) != nBun)
                        {
                            strOK = "";
                        }

                        if (VB.Val(dt.Rows[i]["AMT"].ToString().Trim()) == 0)
                        {
                            strOK = "";
                        }

                        if (strOK == "OK")
                        {
                            nRow = nRow + 1;
                            if (nRow > ssView_Sheet1.RowCount)
                            {
                                ssView_Sheet1.RowCount = nRow;
                            }
                            ssView_Sheet1.RowCount = nRow;
                            // '진료비영수증 항목명칭을 표시
                            if (bTitleDisplay == false)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = " ▶" + cf.Read_Bcode_Name(clsDB.DbCon, "BAS_진료비영수증항목", nBun.ToString("00"));
                                bTitleDisplay = true;
                            }

                            ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = " " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Val(dt.Rows[i]["BASEAMT"].ToString().Trim()).ToString("#,##0");
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("#0.0");
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["NAL"].ToString().Trim()).ToString("#0");

                            if (dt.Rows[i]["GbSelf"].ToString().Trim() == "0")
                            {
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = VB.Val(dt.Rows[i]["AMT"].ToString().Trim()).ToString("#,##0");
                                nSubTot1 = nSubTot1 + VB.Val(dt.Rows[i]["AMt"].ToString().Trim());
                                nTot1 = nTot1 + VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                            }
                            else
                            {
                                ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["AMT"].ToString().Trim()).ToString("#,##0");
                                nSubTot2 = nSubTot2 + VB.Val(dt.Rows[i]["AMt"].ToString().Trim());
                                nTot2 = nTot2 + VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                            }
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt.Rows[i]["AMT2"].ToString().Trim()).ToString("#,##0");
                            nSubTot3 = nSubTot3 + VB.Val(dt.Rows[i]["AMt2"].ToString().Trim());
                            nTot3 = nTot3 + VB.Val(dt.Rows[i]["AMT2"].ToString().Trim());
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        }
                    }

                    if (nSubTot1 != 0 || nSubTot2 != 0 || nSubTot3 != 0 || bTitleDisplay == true)
                    {
                        nRow = nRow + 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }
                        ssView_Sheet1.RowCount = nRow;
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 소  계 **";
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = nSubTot1.ToString("#,##0");
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = nSubTot2.ToString("#,##0");
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = nSubTot3.ToString("#,##0");
                    }

                }
                dt.Dispose();
                dt = null;

                if (FstrHang != "00")
                {
                    ssView_Sheet1.RowCount = nRow;
                }
                else
                {
                    nRow = nRow + 1;
                    ssView_Sheet1.RowCount = nRow;
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 합  계 **";
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = nTot1.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = nTot2.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTot3.ToString("#,##0");
                }

                Cursor.Current = Cursors.Default;
                cf = null;

            }
            catch (Exception ex)
            {
                cf = null;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Screen_Display_SEL_Detail(long ArgIpdNo, long ArgTRSNO)
        {
            int nBun = 0;
            int i = 0;
            int nRow = 0;
            string strBun = "";
            string strNu = "";
            int nHangFrom = 0;
            int nHangTo = 0;
            string strOLD = "";
            string strNew = "";
            string strOK = "";
            double nSubTot1 = 0;
            double nSubTot2 = 0;
            double nSubTot3 = 0;
            double nTot1 = 0;
            double nTot2 = 0;
            double nTot3 = 0;
            bool bTitleDisplay = false;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            ComFunc cf = new ComFunc();

            try
            {
                ssView_Sheet1.ColumnHeader.Cells[0, 0].Text = "처방일자";

                SQL = "";
                //'해당 환자의 자료를 Select
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(a.BDate,'YYYYMMDD') BDate,";
                SQL = SQL + ComNum.VBLF + " a.Bun,a.Nu,a.SuCode,a.SuNext,b.SuNameK,a.BaseAmt,a.Qty,a.GbSelf,";
                SQL = SQL + ComNum.VBLF + " SUM(a.Nal) Nal,SUM(Amt1) Amt,SUM(Amt2) Amt2 ";

                if (clsPmpaType.IMST.GbOldSlip == false)
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_OLD_SLIP a," + ComNum.DB_PMPA + "BAS_SUN b ";
                }

                if (ArgTRSNO > 0)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE a.TRSNO = " + ArgTRSNO + " ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE a.IPDNO = " + ArgIpdNo + " ";
                }

                //'2012-09-07
                if (FstrTempJagugRowid != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND a.BDate>=TO_DATE('" + clsPmpaType.TIT.InDate + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + " AND a.BDate<=TO_DATE('" + clsPmpaType.TIT.OutDate + "','YYYY-MM-DD') ";
                }

                if (FstrBDate != "")
                {
                    SQL = SQL + ComNum.VBLF + " AND a.BDate=TO_DATE('" + FstrBDate + "','YYYY-MM-DD') ";
                }

                if (FstrSuNext == "")
                {
                    SQL = SQL + ComNum.VBLF + " AND a.SuNext='" + FstrSuNext + "' ";
                }
                SQL = SQL + ComNum.VBLF + " AND a.SuNext=b.SuNext(+) ";
                SQL = SQL + ComNum.VBLF + "  AND a.Amt2 <> 0 ";//   '선택금액만

                if (FstrBBBBBB != "Y")
                {
                    SQL = SQL + ComNum.VBLF + " AND TRIM(a.SUNEXT) NOT IN ( SELECT CODE FROM KOSMOS_PMPA.BAS_BCODE WHERE GUBUN ='원무영수제외코드') ";  //'저가약제 제외코드 2011-04-01
                }
                SQL = SQL + ComNum.VBLF + "GROUP BY a.BDate,a.Bun,a.Nu,a.SuCode,a.SuNext,b.SuNameK,a.BaseAmt,a.Qty,a.GbSelf ";
                SQL = SQL + ComNum.VBLF + "ORDER BY a.BDate,a.Bun,a.Nu,a.SuCode,a.SuNext ";

                SQL = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                nTot1 = 0;
                nTot2 = 0;
                nTot3 = 0;

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;

                }

                if (FstrHang == "")
                {
                    Screen_Display_Detail_SUB(ref bTitleDisplay, dt, ref strBun, ref strNu, ref strOK, ref nBun, ref strNew, ref strOLD, ref nSubTot1, ref nSubTot2, ref nSubTot3, ref nTot1, ref nTot2, ref nTot3);
                    return;
                }

                nHangFrom = 1;
                nHangTo = 22;

                //스프레드 출력문
                ssView_Sheet1.RowCount = dt.Rows.Count;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (nBun = nHangFrom; nBun <= nHangTo; nBun++)
                {
                    nSubTot1 = 0;
                    nSubTot2 = 0;
                    nSubTot3 = 0;
                    bTitleDisplay = false;
                    strOLD = "";

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strBun = dt.Rows[i]["Bun"].ToString().Trim();
                        strNu = dt.Rows[i]["Nu"].ToString().Trim();
                        strOK = "OK";

                        if (Bill_ItemNo_Check(strNu, strBun) != nBun)
                        {
                            strOK = "";
                        }

                        if (VB.Val(dt.Rows[i]["AMT"].ToString().Trim()) == 0 && strBun != "74")
                        {
                            strOK = "";
                        }
                        if (strOK == "OK")
                        {
                            // '진료비영수증 항목명칭을 표시
                            if (bTitleDisplay == false)
                            {
                                nRow = nRow + 1;
                                if (nRow > ssView_Sheet1.RowCount)
                                {
                                    ssView_Sheet1.RowCount = nRow;
                                }
                                ssView_Sheet1.Cells[nRow - 1, 2].Text = " ▶" + cf.Read_Bcode_Name(clsDB.DbCon, "BAS_진료비영수증항목", nBun.ToString("00"));
                                bTitleDisplay = true;
                            }

                            nRow = nRow + 1;

                            if (nRow > ssView_Sheet1.RowCount)
                            {
                                ssView_Sheet1.RowCount = nRow;
                            }
                            ssView_Sheet1.RowCount = nRow;

                            strNew = dt.Rows[i]["BDATE"].ToString().Trim();
                            if (strNew != strOLD)
                            {
                                ssView_Sheet1.Cells[nRow - 1, 0].Text = strNew;
                                strOLD = strNew;
                            }
                            ssView_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 2].Text = " " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                            ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Val(dt.Rows[i]["BASEAMT"].ToString().Trim()).ToString("#,##0");
                            ssView_Sheet1.Cells[nRow - 1, 4].Text = VB.Val(dt.Rows[i]["QTY"].ToString().Trim()).ToString("#0.0");
                            ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dt.Rows[i]["NAL"].ToString().Trim()).ToString("#0");

                            if (dt.Rows[i]["GbSelf"].ToString().Trim() == "0")
                            {
                                ssView_Sheet1.Cells[nRow - 1, 6].Text = VB.Val(dt.Rows[i]["AMT"].ToString().Trim()).ToString("#,##0");
                                nSubTot1 = nSubTot1 + VB.Val(dt.Rows[i]["AMt"].ToString().Trim());
                                nTot1 = nTot1 + VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                            }
                            else
                            {
                                ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dt.Rows[i]["AMT"].ToString().Trim()).ToString("#,##0");
                                nSubTot2 = nSubTot2 + VB.Val(dt.Rows[i]["AMt"].ToString().Trim());
                                nTot2 = nTot2 + VB.Val(dt.Rows[i]["AMT"].ToString().Trim());
                            }
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dt.Rows[i]["AMT2"].ToString().Trim()).ToString("#,##0");
                            nSubTot3 = nSubTot3 + VB.Val(dt.Rows[i]["AMt2"].ToString().Trim());
                            nTot3 = nTot3 + VB.Val(dt.Rows[i]["AMT2"].ToString().Trim());
                            ssView_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        }
                    }

                    if (nSubTot1 != 0 || nSubTot2 != 0 || nSubTot3 != 0 || bTitleDisplay == true)
                    {
                        nRow = nRow + 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }
                        ssView_Sheet1.RowCount = nRow;
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 소  계 **";
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = nSubTot1.ToString("#,##0");
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = nSubTot2.ToString("#,##0");
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = nSubTot3.ToString("#,##0");
                    }

                }
                dt.Dispose();
                dt = null;

                if (FstrHang != "00")
                {
                    ssView_Sheet1.RowCount = nRow;
                }
                else
                {
                    nRow = nRow + 1;
                    ssView_Sheet1.RowCount = nRow;
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 합  계 **";
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = nTot1.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = nTot2.ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = nTot3.ToString("#,##0");
                }

                Cursor.Current = Cursors.Default;
                cf = null;

            }
            catch (Exception ex)
            {
                cf = null;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Screen_Display_Detail_SUB(ref bool bTitleDisplay, DataTable dtFunc, ref string strBun, ref string strNu, ref string strOK, ref int nBun, ref string strNew, ref string strOLD, ref double nSubTot1, ref double nSubTot2, ref double nSubTot3, ref double nTot1, ref double nTot2, ref double nTot3)
        {
            int i = 0;
            ComFunc cf = new ComFunc();

            bTitleDisplay = false;

            for (i = 0; i < dtFunc.Rows.Count; i++)
            {
                strBun = dtFunc.Rows[i]["Bun"].ToString().Trim();
                strNu = dtFunc.Rows[i]["Nu"].ToString().Trim();
                strOK = "OK";

                if (VB.Val(dtFunc.Rows[i]["AMT"].ToString().Trim()) == 0)
                {
                    strOK = "";
                }
                if (strOK == "OK")
                {
                    // '진료비영수증 항목명칭을 표시
                    if (bTitleDisplay == false)
                    {
                        nRow = nRow + 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                        }
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = " ▶" + cf.Read_Bcode_Name(clsDB.DbCon, "BAS_진료비영수증항목", nBun.ToString("00"));
                        bTitleDisplay = true;
                    }

                    nRow = nRow + 1;

                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                    }
                    ssView_Sheet1.RowCount = nRow;

                    strNew = dtFunc.Rows[i]["BDATE"].ToString().Trim();
                    if (strNew != strOLD)
                    {
                        ssView_Sheet1.Cells[nRow - 1, 0].Text = strNew;
                        strOLD = strNew;
                    }
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = dtFunc.Rows[i]["SUNEXT"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = " " + dtFunc.Rows[i]["SUNAMEK"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = VB.Val(dtFunc.Rows[i]["BASEAMT"].ToString().Trim()).ToString("#,##0");
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = VB.Val(dtFunc.Rows[i]["QTY"].ToString().Trim()).ToString("#0.0");
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = VB.Val(dtFunc.Rows[i]["NAL"].ToString().Trim()).ToString("#0");

                    if (dtFunc.Rows[i]["GbSelf"].ToString().Trim() == "0")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = VB.Val(dtFunc.Rows[i]["AMT"].ToString().Trim()).ToString("#,##0");
                        nSubTot1 = nSubTot1 + VB.Val(dtFunc.Rows[i]["AMt"].ToString().Trim());
                        nTot1 = nTot1 + VB.Val(dtFunc.Rows[i]["AMT"].ToString().Trim());
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = VB.Val(dtFunc.Rows[i]["AMT"].ToString().Trim()).ToString("#,##0");
                        nSubTot2 = nSubTot2 + VB.Val(dtFunc.Rows[i]["AMt"].ToString().Trim());
                        nTot2 = nTot2 + VB.Val(dtFunc.Rows[i]["AMT"].ToString().Trim());
                    }
                    ssView_Sheet1.Cells[nRow - 1, 8].Text = VB.Val(dtFunc.Rows[i]["AMT2"].ToString().Trim()).ToString("#,##0");
                    nSubTot3 = nSubTot3 + VB.Val(dtFunc.Rows[i]["AMt2"].ToString().Trim());
                    nTot3 = nTot3 + VB.Val(dtFunc.Rows[i]["AMT2"].ToString().Trim());
                }
            }
            nRow = nRow + 1;
            ssView_Sheet1.RowCount = nRow;
            ssView_Sheet1.Cells[nRow - 1, 2].Text = "** 합  계 **";
            ssView_Sheet1.Cells[nRow - 1, 6].Text = nTot1.ToString("#,##0");
            ssView_Sheet1.Cells[nRow - 1, 7].Text = nTot2.ToString("#,##0");
            ssView_Sheet1.Cells[nRow - 1, 8].Text = nTot3.ToString("#,##0");

            dtFunc.Dispose();
            dtFunc = null;
            cf = null; 
        }

        void Screen_Display_Slip(PsmhDb pDbCon, string ArgHangDtl)
        {
            clsIuSentChk cISCHK = new clsIuSentChk();

            if (ArgHangDtl == "부수술비용")
            {
                cISCHK.Rtn_Display_Ipd_SubOP(pDbCon, ssView_Sheet1, FnTRSNO);
            }
            else if (ArgHangDtl == "응급가산수가")
            {
                cISCHK.Rtn_Display_Ipd_ErPed(pDbCon, ssView_Sheet1, FnTRSNO);
            }
          
            else if (ArgHangDtl == "추가입원료")
            {
                cISCHK.Rtn_Display_Ipd_AddCharge(pDbCon, ssView_Sheet1, FnTRSNO);
            }
            else
            {
                cISCHK.Rtn_Display_Ipd_SlipDtl(pDbCon, ssView_Sheet1, FnTRSNO, ArgHangDtl);
            }
        }

        private void SCREEN_CLEAR()
        {
            ssView_Sheet1.RowCount = 0;
            //ssView_Sheet1.RowCount = 40;
        }

        private void View_Display()
        {
            if (FstrJob == "상세내역")
            {
                chkdetail.Checked = true;
            }
            else
            {
                chkdetail.Checked = false;
            }

            Display_IPD_Master(FnIPDNO, FnTRSNO);

            if (FstrHang == "")
            {
                Screen_Display_Detail_SUGA(FnIPDNO, FnTRSNO);
            }
            else if (chkdetail.Checked == true)
            {
                if (chksel.Checked == true)
                {
                    Screen_Display_SEL_Detail(FnIPDNO, FnTRSNO);
                }
                else
                {
                    Screen_Display_Detail(FnIPDNO, FnTRSNO);
                }

            }
            else
            {
                if (chksel.Checked == true)
                {
                    Screen_Display_SEL(FnIPDNO, FnTRSNO);
                }
                else
                {
                    Screen_display(FnIPDNO, FnTRSNO);
                }

            }
        }

        //'분류,누적코드로 영수증 항목번호를 가져옴
        private int Bill_ItemNo_Check(string ArgNu, string argBun)
        {
            int nReturn = 0;

            switch (ArgNu)
            {
                case "01":
                    nReturn = 1;  //'1.진찰료
                    break;
                case "02":
                case "03":
                    nReturn = 2;  //'2.입원료
                    break;
                case "21":
                    nReturn = 2;  //'2.입원료(협진료)
                    break;
                case "16":
                case "34":
                    nReturn = 3;  //'3.식대
                    break;
                case "04":
                case "22":
                    if (FstrHangDtl == "투약약품")
                    {
                        if (argBun == "11" || argBun == "12")
                        {
                            nReturn = 4;  //'4.투약및조제료 - 약품
                        }
                        else
                            nReturn = 100;

                    }
                    else if (FstrHangDtl == "투약행위")
                        if (argBun != "11" && argBun != "12")
                        {
                            nReturn = 4;//  '4.투약및조제료 - 행위
                        }
                        else
                        {
                            nReturn = 100;
                        }
                    else
                    {
                        nReturn = 4;// '4.투약및조제료
                    }
                    break;
                case "05":
                case "23":

                    if (FstrHangDtl == "투약약품")
                    {
                        if (argBun == "20" || argBun == "21")
                            nReturn = 5;//  '5주사 - 약품
                        else
                        {
                            nReturn = 100;
                        }
                    }
                    else if (FstrHangDtl == "투약행위")
                    {
                        if (argBun != "11" && argBun != "12")
                        {
                            nReturn = 5;//  '5.주사 - 행위
                        }
                        else
                        {
                            nReturn = 100;
                        }
                    }
                    else
                    {
                        nReturn = 5;//  '5.주사
                    }
                    break;
                case "06":
                case "24":
                    nReturn = 6;// '6.마취료
                    break;
                case "09":
                case "10":
                case "12":
                case "27":
                case "28":
                case "30":
                    switch (argBun)
                    {
                        case "29":
                        case "31":
                        case "32":
                        case "33":
                        case "36":
                        case "39":
                            nReturn = 10; //'치료재료대
                            break;
                        default:
                            nReturn = 7;//  '처치수술료
                            break;
                    }
                    break;
                case "13":
                case "14":
                case "31":
                case "32":
                    nReturn = 8;// '검사료
                    break;
                case "15":
                case "33":
                    nReturn = 9;//'방사선료
                    break;
                case "07":
                case "25":
                    nReturn = 12;//'재활및물리치료
                    break;
                case "08":
                case "26":
                    nReturn = 13;//'정신요법료
                    break;
                case "19":
                case "37":
                    nReturn = 14;//'CT
                    break;
                case "18":
                case "38":
                    nReturn = 15;//'MRI
                    break;
                case "36":
                    nReturn = 16;//'초음파
                    break;
                case "40":
                    nReturn = 17;//'보철.교정료
                    break;
                case "11":
                case "29":
                    nReturn = 18;//'수혈료
                    break;
                case "47":
                    nReturn = 20;//'증명료
                    break;
                case "35":
                    nReturn = 21;//'병실차액
                    break;
                default:
                    nReturn = 22;//'기타
                    break;

            }
            return nReturn;
        }

        private void btnSubClose_Click()
        {
            this.Close();
            return;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkHU_CheckedChanged(object sender, EventArgs e)
        {
            Screen_display(FnIPDNO, FnTRSNO);
        }
    }
}

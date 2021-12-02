using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComPmpaLibB;
using ComLibB;
using FarPoint.Win.Spread;
using System.Text.RegularExpressions;
using DevComponents.DotNetBar;
using System.Runtime.InteropServices;
using System.Threading;


namespace ComPmpaLibB
{
    public partial class frmPmPaJungganPrint : Form
    {

        clsPmpaPrint PPT = new clsPmpaPrint();
        FarPoint.Win.Spread.FpSpread ssSpread = null;

        #region Class 및 변수선언 ...
        //ComFunc CF              = new ComFunc();
        //clsSpread cSpd          = new clsSpread();
        //clsPmpaMirSpd cPMS      = new clsPmpaMirSpd();
        //clsIuSent cISent        = new clsIuSent();
        //clsIuSentChk cISentChk  = new clsIuSentChk();
        //clsIument cIMent        = new clsIument();
        //clsBasAcct cBAcct       = new clsBasAcct();
        //clsPmpaFunc cPF         = new clsPmpaFunc();
        //clsJSimSQL cJSimSQL     = new clsJSimSQL();
        //clsJSimFunc cJSF        = new clsJSimFunc();
        //clsPmpaPb cPb           = new clsPmpaPb();
        //clsIpdAcct cIAcct       = new clsIpdAcct();
        //clsPmpaType cType       = new clsPmpaType();
        clsBasAcct CBA = new clsBasAcct();
        clsPmpaType.cBas_Add_Arg cBArg = null;
        clsPmpaType.Bas_Acc_Rtn cBAR = null;
        clsPmpaType.JPatLst JPL = null;
        ComFunc CF = new ComFunc();
        clsVbfunc CV = new clsVbfunc();
        clsIpdAcct cIAcct = new clsIpdAcct();

        FpSpread spd;
        DataSet ds = null;

        string FstrYOIL = string.Empty;

        string strTIM = string.Empty;

        string strTBP = string.Empty;
        double[] nAmt = new double[64];
        string[] strAmt = new string[64];
        
        string SqlErr = "";
        int intRowCnt , nRow = 0;

        DataTable dt, dt1, dt2 = null;

        public delegate void EventClosed(Form frm);
        public static event EventClosed rEventClosed;
        #endregion
        string SQL = string.Empty;
        string FstrPano = "";
        string FstrJob = "";

        long FnIPDNO = 0;
        long FnTRSNO = 0;
        long FnBoho_IPDNO = 0;
      
        public frmPmPaJungganPrint()
        {
            InitializeComponent();
            setEvent();
        }
        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            //this.btnExit.Click += new EventHandler(eBtnEvent);
            //this.btnView.Click += new EventHandler(eBtnEvent);
            //this.btnPrint.Click += new EventHandler(eBtnEvent);
            //this.btnSim.Click += new EventHandler(eBtnEvent);

            //this.txtPano.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            //this.txtCnt.GotFocus += new EventHandler(eControl_GotFocus);


        }
        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void CmdBuild2_Click(object sender, EventArgs e)
        {
            string strRowId, strChk, strYear, strGbSang, strOK = "";

            string SqlErr = "";
            string strPano = "";
            int i, j, K, nREAD = 0;
            long nIPDNO, nTRSNo, nTrsCNT = 0;
            double[] nAmt = new double[65];


            if (ComFunc.MsgBoxQ("상한제대상자 금액계산을 실행하시겠습니까?", "상한제 생성", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

          
            SS1_Sheet1.RowCount = 0;
            strYear = VB.Left(clsPublic.GstrSysDate, 4);
            Cursor.Current = Cursors.WaitCursor;
            strChk = "";
           



            SQL = " SELECT   a.WardCode,a.RoomCode,a.Pano,a.SName,a.Bi,a.IPDNO,a.ROWID " + ComNum.VBLF;
            SQL = SQL + " FROM WORK_IPD_AMT a, IPD_TRANS b " + ComNum.VBLF;
            SQL = SQL + " WHERE a.IPDNO=b.IPDNO " + ComNum.VBLF;
            SQL = SQL + " AND b.ACTDATE IS NULL " + ComNum.VBLF;
            SQL = SQL + " AND b.SangAmt > 0  " + ComNum.VBLF;
            SQL = SQL + " AND b.GBIPD <> 'D'  " + ComNum.VBLF;
            SQL = SQL + " AND b.INDATE >=TO_DATE('" + strYear + "-01-01','YYYY-MM-DD')  " + ComNum.VBLF;
            SQL = SQL + " GROUP BY a.WardCode,a.RoomCode,a.Pano,a.SName,a.Bi,a.IPDNO,a.ROWID   " + ComNum.VBLF;



            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("대상자가 없습니다!!");
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nIPDNO = Convert.ToInt64(dt.Rows[i]["IPDNO"].ToString().Trim());
                strRowId = dt.Rows[i]["ROWID"].ToString().Trim();
                PanelMsg.Text = dt.Rows[i]["PANO"].ToString().Trim() + " ";
                PanelMsg.Text += dt.Rows[i]["SName"].ToString().Trim() + " ";
                PanelMsg.Text += "퇴원금액 계산중(" + i + 1 + " / " + dt.Rows.Count + ") +  ";


                for (j = 1; j < 65; j++)
                {
                    nAmt[j] = 0;

                }
                SQL = " SELECT Pano, TRSNO,TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,GbIPD,Amt64 " + ComNum.VBLF;
                SQL = SQL + " FROM IPD_TRANS " + ComNum.VBLF;
                SQL = SQL + " WHERE IPDNO=" + nIPDNO + "   " + ComNum.VBLF;
                SQL = SQL + " AND ACTDATE IS NULL " + ComNum.VBLF;
                SQL = SQL + " AND GBIPD <> 'D' " + ComNum.VBLF;


                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count == 0)
                {
                    nTrsCNT = dt1.Rows.Count;
                    Cursor.Current = Cursors.Default;

                }
                for (j = 0; j < dt1.Rows.Count; j++)
                {
                    strOK = "OK";
                    nTRSNo = Convert.ToInt64(dt1.Rows[j]["TRSNO"].ToString().Trim());
                    clsPmpaType.TIT.Pano = dt1.Rows[j]["Pano"].ToString().Trim();

                    if (dt1.Rows[j]["ActDate"].ToString().Trim() != "")

                    {
                        strOK = "NO";
                    }

                    if (dt1.Rows[j]["GbIPD"].ToString().Trim() == "D")

                    {
                        strOK = "NO";
                    }


                    if (strOK == "OK")
                    {
                        if (cIAcct.Ipd_Trans_Amt_ReBuild(clsDB.DbCon, nTRSNo, "") == false)
                        {
                            Cursor.Current = Cursors.Default;
                            dt1.Dispose();
                            dt1 = null;
                            ComFunc.MsgBox("총진료비를 재계산 도중에 오류가 발생함!!, 전산실로 연락바람!!!");
                            return;

                        }
                        cIAcct.Ipd_Tewon_Amt_Process(clsDB.DbCon, nTRSNo, "", "");

                        for (K = 1; K < 61; K++)
                        {
                            nAmt[K] = nAmt[K] + clsPmpaType.TIT.Amt[K];

                        }
                        nAmt[64] = nAmt[64] + Convert.ToInt64(dt1.Rows[j]["Amt64"].ToString().Trim());


                    }

                }

                dt1.Dispose();
                dt1 = null;


                // '중간납 납부액,대체액을 읽음
                SQL = " SELECT  SuNext,SUM(Amt) Amt FROM IPD_NEW_CASH " + ComNum.VBLF;
                SQL = SQL + " WHERE IPDNO=" + nIPDNO + "   " + ComNum.VBLF;
                SQL = SQL + " AND BUN IN ('87','85','88') " + ComNum.VBLF;
                SQL = SQL + " GROUP BY SuNext  " + ComNum.VBLF;


                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                nAmt[51] = 0; nAmt[52] = 0;

                for (j = 0; j < dt1.Rows.Count; j++)
                {
                    if (dt1.Rows[j]["SuNext"].ToString().Trim() != "Y88")
                    {
                        nAmt[51] = nAmt[51] + Convert.ToInt64(dt1.Rows[0]["Amt"].ToString().Trim());
                    }
                    else
                    {
                        nAmt[52] = nAmt[52] + Convert.ToInt64(dt1.Rows[0]["Amt"].ToString().Trim());
                    }

                }
                dt1.Dispose();
                dt1 = null;

                //'차인납부액은 이미 본인부담계산되어있음= X = X - 중간납 -감액 '2009 - 07 - 04  윤조연 수정 밑에꺼 사용해야할듯??
                nAmt[55] = nAmt[55] - (nAmt[52] - nAmt[51]);

                //'당해년도 상한체크
                strGbSang = "";

                SQL = " SELECT IPDNO " + ComNum.VBLF;
                SQL = SQL + " FROM IPD_TRANS WHERE IPDNO=" + nIPDNO + "   " + ComNum.VBLF;
                SQL = SQL + " AND INDATE >=TO_DATE('" + strYear + "-01-01','YYYY-MM-DD')  " + ComNum.VBLF;
                SQL = SQL + " AND GBIPD <> 'D'  " + ComNum.VBLF;
                SQL = SQL + " AND GbSang IS NOT NULL " + ComNum.VBLF;


                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    strGbSang = "Y";
                }
                dt1.Dispose();
                dt1 = null;


                SQL = "";
                SQL += ComNum.VBLF + " UPDATE WORK_IPD_AMT SET ";
                for (j = 1; j < 60; j++)
                {
                    SQL += ComNum.VBLF + " Amt" + string.Format("{0:00}", j) + "=" + nAmt[j] + ", " + ComNum.VBLF;

                }
                if (strGbSang == "Y")
                {
                    SQL += ComNum.VBLF + " GbSang = 'Y', " + ComNum.VBLF;
                }
                SQL += ComNum.VBLF + " Amt60=" + nAmt[60] + ", " + ComNum.VBLF;
                SQL += ComNum.VBLF + " Amt64=" + nAmt[64] + " " + ComNum.VBLF;
                SQL += ComNum.VBLF + " WHERE ROWID='" + strRowId + "' " + ComNum.VBLF;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);


            }
            dt.Dispose();
            dt = null;


            if (strChk == "OK")
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO WORK_IPD_AMT_HIS ";
                SQL += ComNum.VBLF + " SELECT * FROM WORK_IPD_AMT " + ComNum.VBLF;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);
            }

            PanelMsg.Text = " ";
            clsPublic.GstrMsgTitle = "확인";
            clsPublic.GstrMsgList = "현재시점 재원자 총진료비,조합부담" + '\r';
            clsPublic.GstrMsgList += "본인부담,할인액,중간납 미대체액 계산 완료" + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);

            CmdBuild.Enabled = false;
            CmdView.Enabled = true;
            CmdPrint_New.Enabled = true;


        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void menuView_Click(object sender, EventArgs e)
        {
           
            clsPublic.GstrMsgTitle = "확인";
            clsPublic.GstrMsgList = " 보험,보호 : 입원기간 1주 이상인 환자로서 총진료비중 본인" + '\r';
            clsPublic.GstrMsgList += "부담미납액이 30만원 이상인 환자" + '\r';
            clsPublic.GstrMsgList += "일반 환자 : 입원기간관계없이 총진료비중 미납액이 100만원" + '\r';
            clsPublic.GstrMsgList += "이상인 환자" + '\r';
            clsPublic.GstrMsgList += "자보 환자 : 입원기간관계없이 자보청구금액을 제외한 본인" + '\r';
            clsPublic.GstrMsgList += "부담액이 50만원 이상인 환자 " + '\r';
            clsPublic.GstrMsgList += "산재,공상 : 입원기간관계없이 청구금액을 제외한 본인" + '\r';
            clsPublic.GstrMsgList += "부담액이 30만원 이상인 환자 " + '\r';
            clsPublic.GstrMsgList += "규정사항 적용은 아래와 같습니다 ! " + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
        }

        private void ComboWard_Click(object sender, EventArgs e)
        {
            if(ComboWard.Text !="")
            {
            
                Load_Combo_Room(clsDB.DbCon, ComboWard.Text);

            }
        }
        void Load_Combo_Room(PsmhDb pDbCon, string  ArgWard  )
        {
            DataTable DtFunc = new DataTable();
            string SQL = "";
            string SqlErr = "";
            string strWard = "";

          
            SQL = "";
            SQL += ComNum.VBLF + " SELECT RoomCode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ROOM ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "  and  wardcode='" + ArgWard + "'   " + ComNum.VBLF;

            SQL += ComNum.VBLF + "  ORDER BY WardCode ";
            SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtFunc.Dispose();
                DtFunc = null;
                return;
            }

            ComboRoom.Items.Clear();

            if (DtFunc.Rows.Count > 0)
            {
           
                for (int i = 0; i < DtFunc.Rows.Count; i++)
                {
                    ComboRoom.Items.Add(DtFunc.Rows[i]["RoomCode"].ToString().Trim());
                }
            }

            DtFunc.Dispose();
            DtFunc = null;

        }

        private void ComboWard_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (ComboWard.Text != "")
            {

                Load_Combo_Room(clsDB.DbCon, ComboWard.Text);

            }
        }

        private void CmdExport_Click(object sender, EventArgs e)
        {
            bool x = false;

            if (ComFunc.MsgBoxQ("파일로 만드시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
                return;

            x = SS1.SaveExcel("C:\\재원자중간계산서.xlsx", FarPoint.Excel.ExcelSaveFlags.UseOOXMLFormat);
            {
                if (x == true)
                    ComFunc.MsgBox("엑셀파일이 생성이 되었습니다.", "확인");
                else
                    ComFunc.MsgBox("엑셀파일 생성에 오류가 발생 하였습니다.", "확인");
            }
        }
        void Print_Bill(PsmhDb pDbCon)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strInDate = string.Empty;
            string strOutDate_Drg = string.Empty;
            string strNgt = string.Empty;
            string strDrgCode = string.Empty;
            string strPano = string.Empty;

            long nChaamt = 0;
            long nIPDNO = 0;
            long nTRSNO = 0;

            if (FnIPDNO == 0)
            {
                ComFunc.MsgBox("환자선택 안됨.", "출력불가");
                return;
            }

            clsIument cIM = new clsIument();
            clsPmpaPrint cPP = new clsPmpaPrint();
            DRG DRG = new DRG();

            //자격정보 읽기
            cIM.Read_Ipd_Mst_Trans(pDbCon, FstrPano, FnTRSNO, "");

            strPano = clsPmpaType.TIT.Pano;
            strDrgCode = clsPmpaType.TIT.DrgCode;
            nIPDNO = clsPmpaType.TIT.Ipdno;
            nTRSNO = clsPmpaType.TIT.Trsno;

            cIM.Ipd_Trans_PrtAmt_Read(pDbCon, nTRSNO, "");
            cIM.Ipd_Tewon_PrtAmt_Gesan(pDbCon, strPano, nIPDNO, nTRSNO, "", "");



            #region 이미납부한 금액
            SQL = "";
            SQL = "SELECT SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88 ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
            SQL = SQL + ComNum.VBLF + " WHERE TRSNO =" + nTRSNO + " ";
            SQL = SQL + ComNum.VBLF + "   AND SuNext IN ('Y88') ";
            SQL = SQL + ComNum.VBLF + " GROUP BY SuNext ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                clsPmpaType.TIT.Amt[51] = (long)VB.Val(dt.Rows[0]["Y88"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;
            #endregion


            cPP.IPD_Sunap_Report_A4_New(pDbCon, FstrPano, FnIPDNO, FnTRSNO, "", false);
            //cPP.IPD_Sunap_Report_A4_Pay(pDbCon, FstrPano, FnIPDNO, FnTRSNO, "", false);
            

        }

        void Print_Bill_Again(PsmhDb pDbCon)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strInDate = string.Empty;
            string strOutDate_Drg = string.Empty;
            string strNgt = string.Empty;
            string strDrgCode = string.Empty;
            string strPano = string.Empty;

            long nChaamt = 0;
            long nIPDNO = 0;
            long nTRSNO = 0;

            if (FnIPDNO == 0)
            {
                ComFunc.MsgBox("환자선택 안됨.", "출력불가");
                return;
            }

            clsIument cIM = new clsIument();
            clsPmpaPrint cPP = new clsPmpaPrint();
            DRG DRG = new DRG();

            //자격정보 읽기
            cIM.Read_Ipd_Mst_Trans(pDbCon, FstrPano, FnTRSNO, "");

            strPano = clsPmpaType.TIT.Pano;
            strDrgCode = clsPmpaType.TIT.DrgCode;
            nIPDNO = clsPmpaType.TIT.Ipdno;
            nTRSNO = clsPmpaType.TIT.Trsno;

            cIAcct.Ipd_Trans_Amt_ReBuild(clsDB.DbCon, nTRSNO, "");
            cIAcct.Ipd_Tewon_Amt_Process(clsDB.DbCon, nTRSNO, "", "");
            cIM.Ipd_Trans_PrtAmt_Read(pDbCon, nTRSNO, "");
            cIM.Ipd_Tewon_PrtAmt_Gesan(pDbCon, strPano, nIPDNO, nTRSNO, "", "");
           

            #region 이미납부한 금액
            SQL = "";
            SQL = "SELECT SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88 ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
            SQL = SQL + ComNum.VBLF + " WHERE TRSNO =" + nTRSNO + " ";
            SQL = SQL + ComNum.VBLF + "   AND SuNext IN ('Y88') ";
            SQL = SQL + ComNum.VBLF + " GROUP BY SuNext ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                clsPmpaType.TIT.Amt[51] = (long)VB.Val(dt.Rows[0]["Y88"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;
            #endregion

            Ipd_Slip_Amt_Set();

            if (SS2_Sheet1.RowCount == 0)
                return;

            SS2_Sheet1.Cells[4, 4].Text = clsPmpaType.TIT.Sname;
            SS2_Sheet1.Cells[5, 4].Text = CF.Read_Bcode_Name(pDbCon, "BAS_환자종류", clsPmpaType.TIT.Bi);
            SS2_Sheet1.Cells[6, 4].Text = clsPmpaType.TIT.RoomCode + "호";
            SS2_Sheet1.Cells[4, 9].Text = clsPmpaType.TIT.Pano;
            SS2_Sheet1.Cells[5, 9].Text = clsPmpaType.TIT.DeptCode + " " + CF.READ_DEPTNAMEK(pDbCon, clsPmpaType.TIT.DeptCode);
            SS2_Sheet1.Cells[6, 5].Text = clsPmpaType.TIT.InDate + "∼" + clsPmpaType.TIT.ArcDate + "(" + clsPmpaType.TIT.Ilsu + ")";
            SS2_Sheet1.Cells[7, 2].Text = "[담당자: " + TxtPrintName.Text + " TEL. : " + TxtPrintTel.Text + "]";

            nRow = 11;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[1].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[1].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[1].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[1].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[1].ToString("#,### ");    //비급여

            //입원
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[2].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[2].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[2].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[2].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[2].ToString("#,### ");    //비급여

            ////식대
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[3].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[3].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[3].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[3].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[3].ToString("#,### ");    //비급여

            //// '투약료(행위+약품)
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = (clsPmpaType.TRPG.TotAmt5[4] + clsPmpaType.TRPG.TotAmt5[5]).ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = (clsPmpaType.TRPG.TotAmt6[4] + clsPmpaType.TRPG.TotAmt6[5]).ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = (clsPmpaType.TRPG.TotAmt4[4] + clsPmpaType.TRPG.TotAmt4[5]).ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = (clsPmpaType.TRPG.TotAmt3[4] + clsPmpaType.TRPG.TotAmt3[5]).ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = (clsPmpaType.TRPG.TotAmt2[4] + clsPmpaType.TRPG.TotAmt2[5]).ToString("#,### ");    //비급여


            ////주사 행위 + 약품
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = (clsPmpaType.TRPG.TotAmt5[6] + clsPmpaType.TRPG.TotAmt5[7]).ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = (clsPmpaType.TRPG.TotAmt6[6] + clsPmpaType.TRPG.TotAmt6[7]).ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = (clsPmpaType.TRPG.TotAmt4[6] + clsPmpaType.TRPG.TotAmt4[7]).ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = (clsPmpaType.TRPG.TotAmt3[6] + clsPmpaType.TRPG.TotAmt3[7]).ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = (clsPmpaType.TRPG.TotAmt2[6] + clsPmpaType.TRPG.TotAmt2[7]).ToString("#,### ");    //비급여


            ////마취
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[8].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[8].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[8].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[8].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[8].ToString("#,### ");    //비급여

            ////처치 수술
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[9].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[9].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[9].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[9].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[9].ToString("#,### ");    //비급여

            ////검사료
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[10].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[10].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[10].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[10].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[10].ToString("#,### ");    //비급여

            ////  '영상진단+방사선진단료
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = (clsPmpaType.TRPG.TotAmt5[11] + clsPmpaType.TRPG.TotAmt5[12]).ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = (clsPmpaType.TRPG.TotAmt6[11] + clsPmpaType.TRPG.TotAmt6[12]).ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = (clsPmpaType.TRPG.TotAmt4[11] + clsPmpaType.TRPG.TotAmt4[12]).ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = (clsPmpaType.TRPG.TotAmt3[11] + clsPmpaType.TRPG.TotAmt3[12]).ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = (clsPmpaType.TRPG.TotAmt2[11] + clsPmpaType.TRPG.TotAmt2[12]).ToString("#,### ");    //비급여


            ////치료재료대
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[13].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[13].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[13].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[13].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[13].ToString("#,### ");    //비급여

            ////물리치료
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[14].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[14].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[14].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[14].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[14].ToString("#,### ");    //비급여

            ////정신요법
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[15].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[15].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[15].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[15].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[15].ToString("#,### ");    //비급여

            ////전혈
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[16].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[16].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[16].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[16].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[16].ToString("#,### ");    //비급여

            ////CT
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[17].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[17].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[17].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[17].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[17].ToString("#,### ");    //비급여

            ////MRI
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[18].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[18].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[18].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[18].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[18].ToString("#,### ");    //비급여

            ////초음파
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[19].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[19].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[19].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[19].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[19].ToString("#,### ");    //비급여

            ////보철
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[20].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[20].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[20].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[20].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[20].ToString("#,### ");    //비급여

            ////증명료
            nRow += 2;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[22].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[22].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[22].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[22].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[22].ToString("#,### ");    //비급여

            ////병실차액
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[21].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[21].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[21].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[21].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[21].ToString("#,### ");    //비급여

            ////기타
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[49].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[49].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[49].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[49].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[49].ToString("#,### ");    //비급여

            ////선별급여
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = clsPmpaType.TRPG.TotAmt5[24].ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = clsPmpaType.TRPG.TotAmt6[24].ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[24].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[24].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[24].ToString("#,### ");    //비급여

            ////소계
            nRow += 1;
            SS2_Sheet1.Cells[nRow, 6].Text = (clsPmpaType.TRPG.TotAmt5[50]).ToString("#,### ");    //본인부담(급여)
            SS2_Sheet1.Cells[nRow, 7].Text = (clsPmpaType.TRPG.TotAmt6[50]).ToString("#,### ");    //조합부담(급여)
            SS2_Sheet1.Cells[nRow, 8].Text = clsPmpaType.TRPG.TotAmt4[50].ToString("#,### ");    //전액본인
            SS2_Sheet1.Cells[nRow, 9].Text = clsPmpaType.TRPG.TotAmt3[50].ToString("#,### ");    //선택진료
            SS2_Sheet1.Cells[nRow, 10].Text = clsPmpaType.TRPG.TotAmt2[50].ToString("#,### ");    //비급여


            //이미납부한 금액

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88 ,SUM(SUM(CASE WHEN SUNEXT IN ('Y85','Y87') THEN AMT END )) Y8785 ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
            SQL = SQL + ComNum.VBLF + " WHERE IPDNO =" + clsPmpaType.TIT.Ipdno + " ";
            SQL = SQL + ComNum.VBLF + "   AND SuNext IN ('Y88','Y85','Y87')  ";
            SQL = SQL + ComNum.VBLF + " GROUP BY SuNext ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                if (clsPmpaType.TIT.GbIpd =="1")
                {
                    clsPmpaType.TIT.Amt[52] = (long)VB.Val(dt.Rows[0]["Y8785"].ToString().Trim() ) - (long)VB.Val(dt.Rows[0]["Y88"].ToString().Trim());
                }
                
            }

            dt.Dispose();
            dt = null;


            ////총진료비 +납부금액
            nRow += 2;
            SS2_Sheet1.Cells[35, 4].Text = (clsPmpaType.TIT.Amt[50]).ToString("#,### ");    //총진료비
            SS2_Sheet1.Cells[35, 9].Text = clsPmpaType.TIT.Amt[52].ToString("#,### ");    //납부금액

            ////조합부담금
            nRow += 1;
            SS2_Sheet1.Cells[36, 4].Text = (clsPmpaType.TIT.Amt[53]).ToString("#,### ");    //본인부담(급여)


            
            SS2_Sheet1.Cells[36, 9].Text = (Math.Truncate((clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[52] - clsPmpaType.TIT.Amt[53] - clsPmpaType.TIT.Amt[54]) / 10.0) * 10).ToString("#,### ");    //내야할돈
         
            ////본인부담금
            nRow += 1;
            SS2_Sheet1.Cells[37, 4].Text = (clsPmpaType.TIT.Amt[50] - clsPmpaType.TIT.Amt[53]).ToString("#,### ");    //본인부담(급여)


            if (string.Compare(clsPmpaType.TIT.GbGameK, "00") > 0)
            {
                SS2_Sheet1.Cells[39, 9].Text = "  감액: " + (clsPmpaType.TIT.Amt[54]).ToString("#,### ") + "원(계정:" + clsPmpaType.TIT.GbGameK + ")";

            }
            else
            {
                SS2_Sheet1.Cells[39, 9].Text = "";
            }

            SS2_Sheet1.Cells[48, 9].Text = VB.Left(clsPublic.GstrSysDate, 4) + "년 " + VB.Mid(clsPublic.GstrSysDate, 6, 2) + "월 " + VB.Right(clsPublic.GstrSysDate, 2) + "일 " + clsPublic.GstrSysTime;

            string strFont1 = @"/fn""바탕체"" /fz""14"" /fb1 /fi0 /fu0 /fk0 /fs1";
            string strFont2 = @"/fn""바탕체"" /fz""12"" /fb0 /fi0 /fu0 /fk0 /fs2";
           

            SS2_Sheet1.PrintInfo.Header = strFont1 +  strFont2;
            SS2_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            SS2_Sheet1.PrintInfo.Margin.Top = 10;
            SS2_Sheet1.PrintInfo.Margin.Bottom = 10;

            SS2_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            SS2_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            SS2_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            SS2_Sheet1.PrintInfo.ShowBorder = false;
            SS2_Sheet1.PrintInfo.ShowColor = false;
            SS2_Sheet1.PrintInfo.ShowGrid = false;
            SS2_Sheet1.PrintInfo.ShowShadows = false;
            SS2_Sheet1.PrintInfo.UseMax = true;
            SS2_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            SS2_Sheet1.PrintInfo.Preview = false;
            SS2.PrintSheet(0);




        }

        public void Ipd_Slip_Amt_Set()
        {
            int nxx = 0;
            int j = 0;
            //입원용 금액 set


            clsPmpaType.TRPG.TotAmt1 = new long[51];
            clsPmpaType.TRPG.TotAmt2 = new long[51];
            clsPmpaType.TRPG.TotAmt3 = new long[51];
            clsPmpaType.TRPG.TotAmt4 = new long[51];
            clsPmpaType.TRPG.TotAmt5 = new long[51];
            clsPmpaType.TRPG.TotAmt6 = new long[51];
            clsPmpaType.TRPG.TotAmt7 = new long[51];
            clsPmpaType.TRPG.TotAmt8 = new long[51];
            clsPmpaType.TRPG.TotAmt9 = new long[51];

            for (j = 0; j <= 50; j++)
            {
                clsPmpaType.TRPG.TotAmt1[j] = 0;
                clsPmpaType.TRPG.TotAmt2[j] = 0;
                clsPmpaType.TRPG.TotAmt3[j] = 0;
                clsPmpaType.TRPG.TotAmt4[j] = 0;
                clsPmpaType.TRPG.TotAmt5[j] = 0;
                clsPmpaType.TRPG.TotAmt6[j] = 0;
                clsPmpaType.TRPG.TotAmt7[j] = 0;
                clsPmpaType.TRPG.TotAmt8[j] = 0;
                clsPmpaType.TRPG.TotAmt9[j] = 0;
            }


            for (nxx = 1; nxx <= 50; nxx++)
            {
                clsPmpaType.TRPG.TotAmt1[nxx] += clsPmpaType.RPG.Amt1[nxx];
                clsPmpaType.TRPG.TotAmt2[nxx] += clsPmpaType.RPG.Amt2[nxx];
                clsPmpaType.TRPG.TotAmt3[nxx] += clsPmpaType.RPG.Amt3[nxx];
                clsPmpaType.TRPG.TotAmt4[nxx] += clsPmpaType.RPG.Amt4[nxx];
                clsPmpaType.TRPG.TotAmt5[nxx] += clsPmpaType.RPG.Amt5[nxx];
                clsPmpaType.TRPG.TotAmt6[nxx] += clsPmpaType.RPG.Amt6[nxx];
                clsPmpaType.TRPG.TotAmt7[nxx] += clsPmpaType.RPG.Amt7[nxx];
                clsPmpaType.TRPG.TotAmt8[nxx] += clsPmpaType.RPG.Amt8[nxx];
                clsPmpaType.TRPG.TotAmt9[nxx] += clsPmpaType.RPG.Amt9[nxx];

            }

        }


        private void CmdPrint_New_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            
            string strOK = string.Empty;
            
          

            for (i = 0; i < SS1_Sheet1.Rows.Count; i++)
            {
                strOK = SS1_Sheet1.Cells[i, 0].Text.Trim();
                FnIPDNO = Convert.ToInt64(SS1_Sheet1.Cells[i, 10].Text.Trim());

                if (OptPrt.Checked == true)
                {
                    strOK = "OK";
                }
                else if (OptPrt1.Checked == true)
                {
                    if (strOK == "True")
                    {
                        strOK = "OK";
                    }
                }
                else
                {
                    if (strOK != "True")
                    {
                        strOK = "OK";
                    }
                }


 
                if (strOK == "OK")
                {

                    SQL = "";
                    SQL = "SELECT TRSNO,PANO ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS  ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO =" + FnIPDNO + " ";
                    SQL = SQL + ComNum.VBLF + "   AND OUTDATE IS NULL  ";


                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {

                        for (j = 0; j < dt.Rows.Count; j++)
                        {
                            FnTRSNO = (long)VB.Val(dt.Rows[j]["TRSNO"].ToString().Trim());
                            FstrPano = dt.Rows[j]["PANO"].ToString().Trim();
                            Print_Bill(clsDB.DbCon);
                        }

                        



                    }

                    dt.Dispose();
                    dt = null;


                   
                }


            }

            
        }

        private void TxtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (TxtPano.Text.Trim() == "")
                {
                    return;
                }

                TxtPano.Text = VB.Format(VB.Val(TxtPano.Text), "00000000");
              
            }
        }

        private void CmdPrint_Click(object sender, EventArgs e)
        {
            int i = 0;
            int j = 0;
            string strOK = string.Empty;



            for (i = 0; i < SS1_Sheet1.Rows.Count; i++)
            {
                strOK = SS1_Sheet1.Cells[i, 0].Text.Trim();
                FnIPDNO = Convert.ToInt64(SS1_Sheet1.Cells[i, 10].Text.Trim());

                if (OptPrt.Checked == true)
                {
                    strOK = "OK";
                }
                else if (OptPrt1.Checked == true)
                {
                    if (strOK == "True")
                    {
                        strOK = "OK";
                    }
                }
                else
                {
                    if (strOK != "True")
                    {
                        strOK = "OK";
                    }
                }



                if (strOK == "OK")
                {

                    SQL = "";
                    SQL = "SELECT TRSNO,PANO ";
                    SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_TRANS  ";
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO =" + FnIPDNO + " ";
                    SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL  ";


                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        for (j = 0; j < dt.Rows.Count; j++)
                        {
                            FnTRSNO = (long)VB.Val(dt.Rows[j]["TRSNO"].ToString().Trim());
                            FstrPano = dt.Rows[j]["PANO"].ToString().Trim();
                            Print_Bill_Again(clsDB.DbCon);
                            ComFunc.Delay(2000);
                        }
                        


                    }

                    dt.Dispose();
                    dt = null;


                   
        
                }


            }
        }

        private void CmdView_Click(object sender, EventArgs e)
        {
            int i, j, K, nREAD,  nIlsu, nDD = 0;
            string strToWard, strstartWard, strToRoom, strFromRoom, strFromBi, strToBi, strAccept, strBi, strAmSet4, strFLAG = "";

            long nAmt55 = 0;
            SS1_Sheet1.RowCount = 0;

            if (ComboWard.Text.Trim() == "전체")
            {
                strstartWard = "00";
                strToWard = "ZZ";
                strFromRoom = "0000";
                strToRoom = "9999";
            }
            else
            {
                strstartWard = VB.Left(ComboWard.Text, 2);
                strToWard = strstartWard;
                if (ComboRoom.Text == "0000" || ComboRoom.Text == "")
                {
                    strFromRoom = "0000";
                    strToRoom = "9999";
                }
                else
                {
                    strFromRoom = ComboRoom.Text.Trim();
                    strToRoom = strFromRoom;
                }
            }

            if (VB.Left(ComboBi.Text, 2) == "00")
            {
                strFromBi = "00";
                strToBi = "55";
            }
            else
            {
                strFromBi = VB.Left(ComboBi.Text, 2);
                strToBi = VB.Left(ComboBi.Text, 2);
            }

            SQL = " SELECT WardCode,RoomCode,Sname,Bi,Ilsu,IPDNO,AmSet4,Amt50,abs(Amt52-Amt51) Amt52,Amt53, Amt54, Amt55,GbSang,Amt64  " + ComNum.VBLF;
            SQL = SQL + " FROM WORK_IPD_AMT " + ComNum.VBLF;
            SQL = SQL + " WHERE Amt50 > 0 " + ComNum.VBLF;
            if (ComboBi.Text.Trim() != "")
            {
                SQL = SQL + " AND Bi = '" + VB.Left(ComboBi.Text.Trim(),2) + "'   " + ComNum.VBLF;
            }
            if (ComboWard.Text.Trim() != "전체")
            {
                SQL = SQL + " AND WardCode = '" + ComboWard.Text.Trim() + "'   " + ComNum.VBLF;
            }
            if (ComboRoom.Text.Trim() != "")
            {
                SQL = SQL + " AND RoomCode = '" + ComboRoom.Text.Trim() + "'   " + ComNum.VBLF;
            }
            if (VB.Val(TxtAmt.Text) > 0)
            {
                SQL = SQL + " AND Amt55 > " + VB.Val(TxtAmt.Text.Trim()) + "   " + ComNum.VBLF;
            }
            if (VB.Val(TxtIlsu.Text) > 0)
            {
                SQL = SQL + " AND ILSU > " + VB.Val(TxtIlsu.Text.Trim()) + "   " + ComNum.VBLF;
            }
            if (TxtPano.Text.Trim() != "")
            {
                SQL = SQL + " AND pano = '" + TxtPano.Text.Trim() + "'   " + ComNum.VBLF;
            }

            SQL = SQL + " ORDER BY WardCode,RoomCode,SName  " + ComNum.VBLF;


            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            nREAD = dt.Rows.Count;
            nRow = 0;
            if (nREAD == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }


            for (i = 0; i < nREAD; i++)
            {
                nRow += 1;
                SS1_Sheet1.Rows.Count = nRow;
              
                SS1_Sheet1.Cells[i, 0].Text = "";
                SS1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                SS1_Sheet1.Cells[i, 2].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                SS1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["SName"].ToString().Trim();
                SS1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Bi"].ToString().Trim();
                SS1_Sheet1.Cells[i, 5].Text = (VB.Val(dt.Rows[i]["Amt50"].ToString().Trim()) - VB.Val(dt.Rows[i]["Amt64"].ToString().Trim())).ToString("###,###,##0"); ;
                SS1_Sheet1.Cells[i, 6].Text = dt.Rows[i]["Amt52"].ToString().Trim();
                SS1_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Amt54"].ToString().Trim();
                SS1_Sheet1.Cells[i, 8].Text = dt.Rows[i]["Amt53"].ToString().Trim();
                SS1_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Amt55"].ToString().Trim();
                SS1_Sheet1.Cells[i, 10].Text = dt.Rows[i]["IPDNO"].ToString().Trim();

                if (dt.Rows[i]["GbSang"].ToString().Trim() =="Y")
                {
                    SS1_Sheet1.Cells[i, 13].Text = "Y";
                }
              


            }

            dt.Dispose();
            dt = null;
        }

            void eFormLoad(object sender, EventArgs e)
        {

            DataTable Dt = null;

            ComFunc.ReadSysDate(clsDB.DbCon);

            FstrYOIL = CF.READ_YOIL(clsDB.DbCon, clsPublic.GstrSysDate);
            TxtPrintName.Text = "";
            TxtPrintTel.Text = "";
            PanelMsg.Text = "";
            TxtPano.Text = "";

            //clsPmpaPb.GstrMirFlag = "";
            //ComFunc.ReadSysDate(clsDB.DbCon);

            CBA.Bas_Opd_Bon();              //외래본인부담율
            CBA.Bas_Ipd_Bon();              //입원본인부담율

            ComboBi.Items.Clear();
            CF.Combo_BCode_SET(clsDB.DbCon, ComboBi, "BAS_환자종류", true, 1, "");
            ComboBi.SelectedIndex = -1;


            ComboWard.Items.Clear();
            ComboWard.Items.Add("전체");
            CV.Combo_Wardcode_Set(clsDB.DbCon, ComboWard, "0", false, 2);
            ComboWard.SelectedIndex = 0;
            CBA.IPD_BON_SANG();              //본인부담상한액


            CmdBuild.Enabled = true;
            CmdView.Enabled = false;



            SQL = "";
            SQL += ComNum.VBLF + " SELECT  TO_CHAR(JobTime,'YYYY-MM-DD') JobTime  FROM " + ComNum.DB_PMPA + "WORK_IPD_AMT WHERE ROWNUM <= 10  ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
            
            }

            if (Dt.Rows.Count > 0)
            {
                if (Dt.Rows[0]["JobTime"].ToString() == clsPublic.GstrSysDate)
                {
                    CmdBuild.Enabled = true;
                    CmdView.Enabled = true;
                }

            }

            Dt.Dispose();
            Dt = null;
        }

        private void CmdBuild_Click(object sender, EventArgs e)
        {
            string  strRowId , strChk , strYear , strGbSang , strOK = "";
          
            string SqlErr = "";
            string strPano = "";
            int i , j , K , nREAD= 0;
            long nIPDNO  , nTRSNo ,nTrsCNT = 0;
            double[] nAmt = new double[65];

            SS1_Sheet1.RowCount = 0;
            strYear = VB.Left(clsPublic.GstrSysDate, 4);
            Cursor.Current = Cursors.WaitCursor;
            strChk = "";
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " DELETE  WORK_IPD_AMT ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }



                SQL = "";
                SQL += ComNum.VBLF + " DELETE  WORK_IPD_AMT_HIS ";
                SQL += ComNum.VBLF + " WHERE  JOBTIME >= TO_DATE('" + VB.DateAdd("D", -14, clsPublic.GstrSysDate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += ComNum.VBLF + " AND  JOBTIME <  TO_DATE('" + VB.DateAdd("D", -13, clsPublic.GstrSysDate).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') " + ComNum.VBLF;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }

                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO WORK_IPD_AMT ( " + ComNum.VBLF;
                SQL += ComNum.VBLF + " IPDNO,PANO,SNAME,BI,SEX,AGE,WARDCODE,ROOMCODE,INDATE,OUTDATE,GBSTS,DEPTCODE, " + ComNum.VBLF;
                SQL += ComNum.VBLF + " DRCODE,ILSU,GBGAMEK,BOHUN,GBSPC,GBKEKLI,GELCODE,AmSet4,JobTime) " + ComNum.VBLF;
                SQL += ComNum.VBLF + " SELECT IPDNO,PANO,SNAME,BI,SEX,AGE,WARDCODE,ROOMCODE,INDATE,OUTDATE,GBSTS,DEPTCODE, " + ComNum.VBLF;
                SQL += ComNum.VBLF + " DRCODE,ILSU,GBGAMEK,BOHUN,GBSPC,GBKEKLI,GELCODE,AmSet4, " + ComNum.VBLF;
                SQL += ComNum.VBLF + " TO_DATE('" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "','YYYY-MM-DD HH24:MI') " + ComNum.VBLF;
                SQL += ComNum.VBLF + " FROM IPD_NEW_MASTER " + ComNum.VBLF;
                SQL += ComNum.VBLF + " WHERE ActDate IS NULL  AND GbSTS = '0'  AND (GbDRG IS NULL OR GbDRG <>'D') " + ComNum.VBLF;

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }


                clsDB.setCommitTran(clsDB.DbCon);

            }
            

            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }



            SQL = " SELECT  WardCode,RoomCode,Pano,SName,Bi,IPDNO,ROWID " + ComNum.VBLF;
            SQL = SQL + " FROM WORK_IPD_AMT  " + ComNum.VBLF;
            SQL = SQL + " ORDER BY WardCode,RoomCode,SName " + ComNum.VBLF;



            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                nIPDNO = Convert.ToInt64(dt.Rows[i]["IPDNO"].ToString().Trim());
                strRowId = dt.Rows[i]["ROWID"].ToString().Trim();
                PanelMsg_P.Text = dt.Rows[i]["PANO"].ToString().Trim() + " ";
                PanelMsg_P.Text += dt.Rows[i]["SName"].ToString().Trim() + " ";
                PanelMsg_P.Text += "퇴원금액 계산중(" + i + 1 + " / " + dt.Rows.Count + ") +  ";


                for (j = 1; j < 65; j++)
                {
                    nAmt[j] = 0;

                }
                SQL = " SELECT Pano, TRSNO,TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,GbIPD,Amt64 " + ComNum.VBLF;
                SQL = SQL + " FROM IPD_TRANS " + ComNum.VBLF;
                SQL = SQL + " WHERE IPDNO=" + nIPDNO + "   " + ComNum.VBLF;
                SQL = SQL + " AND ACTDATE IS NULL " + ComNum.VBLF;
                SQL = SQL + " AND GBIPD <> 'D' " + ComNum.VBLF;


                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt1.Rows.Count == 0)
                {
                    nTrsCNT = dt1.Rows.Count;
                    Cursor.Current = Cursors.Default;

                }
                for (j = 0; j < dt1.Rows.Count; j++)
                {
                    strOK = "OK";
                    nTRSNo = Convert.ToInt64(dt1.Rows[j]["TRSNO"].ToString().Trim());
                    clsPmpaType.TIT.Pano = dt1.Rows[j]["Pano"].ToString().Trim();

                    if (dt1.Rows[j]["ActDate"].ToString().Trim() != "")

                    {
                        strOK = "NO";
                    }

                    if (dt1.Rows[j]["GbIPD"].ToString().Trim() == "D")

                    {
                        strOK = "NO";
                    }


                    if (strOK == "OK")
                    {
                        if (cIAcct.Ipd_Trans_Amt_ReBuild(clsDB.DbCon, nTRSNo, "") == false)
                        {
                            Cursor.Current = Cursors.Default;
                            dt1.Dispose();
                            dt1 = null;
                            ComFunc.MsgBox("총진료비를 재계산 도중에 오류가 발생함!!, 전산실로 연락바람!!!");
                            return;

                        }
                        cIAcct.Ipd_Tewon_Amt_Process(clsDB.DbCon, nTRSNo, "", "");

                        for (K = 1; K < 61; K++)
                        {
                            nAmt[K] = nAmt[K] + clsPmpaType.TIT.Amt[K];

                        }
                        nAmt[64] = nAmt[64] + Convert.ToInt64(dt1.Rows[j]["Amt64"].ToString().Trim());


                    }

                }

                dt1.Dispose();
                dt1 = null;


                // '중간납 납부액,대체액을 읽음
                SQL = " SELECT  SuNext,SUM(Amt) Amt FROM IPD_NEW_CASH " + ComNum.VBLF;
                SQL = SQL + " WHERE IPDNO=" + nIPDNO + "   " + ComNum.VBLF;
                SQL = SQL + " AND BUN IN ('87','85','88') " + ComNum.VBLF;
                SQL = SQL + " GROUP BY SuNext  " + ComNum.VBLF;


                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                nAmt[51] = 0; nAmt[52] = 0;

                for (j = 0; j < dt1.Rows.Count; j++)
                {
                    if (dt1.Rows[j]["SuNext"].ToString().Trim() != "Y88")
                    {
                        nAmt[51] = nAmt[51] + Convert.ToInt64(dt1.Rows[0]["Amt"].ToString().Trim());
                    }
                    else
                    {
                        nAmt[52] = nAmt[52] + Convert.ToInt64(dt1.Rows[0]["Amt"].ToString().Trim());
                    }

                }
                dt1.Dispose();
                dt1 = null;

                //'차인납부액은 이미 본인부담계산되어있음= X = X - 중간납 -감액 '2009 - 07 - 04  윤조연 수정 밑에꺼 사용해야할듯??
                nAmt[55] = nAmt[55] - nAmt[52] - nAmt[51];

                //'당해년도 상한체크
                strGbSang = "";

                SQL = " SELECT IPDNO " + ComNum.VBLF;
                SQL = SQL + " FROM IPD_TRANS WHERE IPDNO=" + nIPDNO + "   " + ComNum.VBLF;
                SQL = SQL + " AND INDATE >=TO_DATE('" + strYear + "-01-01','YYYY-MM-DD')  " + ComNum.VBLF;
                SQL = SQL + " AND GBIPD <> 'D'  " + ComNum.VBLF;
                SQL = SQL + " AND GbSang IS NOT NULL " + ComNum.VBLF;


                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    strGbSang = "Y";
                }
                dt1.Dispose();
                dt1 = null;


                SQL = "";
                SQL += ComNum.VBLF + " UPDATE WORK_IPD_AMT SET ";
                for (j = 1; j < 60; j++)
                {
                    SQL += ComNum.VBLF + " Amt" + string.Format("{0:00}", j) + "=" + nAmt[j] + ", " + ComNum.VBLF;
                   
                }
                if (strGbSang == "Y")
                {
                    SQL += ComNum.VBLF + " GbSang = 'Y', " + ComNum.VBLF;
                }
                SQL += ComNum.VBLF + " Amt60=" + nAmt[60] + ", " + ComNum.VBLF;
                SQL += ComNum.VBLF + " Amt64=" + nAmt[64] + " " + ComNum.VBLF;
                SQL += ComNum.VBLF + " WHERE ROWID='" + strRowId + "' " + ComNum.VBLF;
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);


            }
            dt.Dispose();
            dt = null;


            if ( strChk == "OK")
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO WORK_IPD_AMT_HIS ";
                SQL += ComNum.VBLF + " SELECT * FROM WORK_IPD_AMT " + ComNum.VBLF;
             
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);
            }

            PanelMsg.Text = " ";
            clsPublic.GstrMsgTitle = "확인";
            clsPublic.GstrMsgList = "현재시점 재원자 총진료비,조합부담" + '\r';
            clsPublic.GstrMsgList += "본인부담,할인액,중간납 미대체액 계산 완료" + '\r';
            ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);

            CmdBuild.Enabled = false;
            CmdView.Enabled = true;
            CmdPrint_New.Enabled = true;

        }
    }
}

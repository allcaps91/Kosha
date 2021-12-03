using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// Class Name      : ComMedLibB
    /// File Name       : FrmMedLastIpdOrdView.cs
    /// Description     : 입원환자 진료내역 조회(심사과용)
    /// Author          : 이상훈
    /// Create Date     : 2018-08-31
    /// <history> 
    /// 입원환자 진료내역 조회(심사과용)
    /// 재원기간 관계 없이 상단 일자 기준으로 처방조회(2018.08.31)
    /// </history>
    /// <seealso>
    /// PSMH\Ocs\ipdocs\iorder\mtsiorder\FrmOrdersViewOrder.frm
    /// PSMH\Ocs\ipdocs\eorder\eorder\FrmOrdersViewOrder.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\Ocs\ipdocs\iorder\mtsiorder.vbp
    /// default 		: PSMH\Ocs\ipdocs\eorder\eorder.vbp
    /// </vbp>
    /// </summary>
    public partial class FrmMedIpdOrdView : Form
    {
        public delegate void SENDListILLS(string sillCode, string sillName, string strRO);
        public event SENDListILLS SendILLS;

        public delegate void SENDListOrder(string argRowId, string argGBIO, int startRow);
        public event SENDListOrder SendORD;

        public delegate void EventClosed();
        public event EventClosed rEventClosed;
        
        private string GstrGBIO = "";
        private int GintStartRow = 0;
        private string GstrPtNo = "";

        private string GstrFrDate = "";
        private string GstrToDate = "";
        private bool GbolSim = false;

        public FrmMedIpdOrdView()
        {
            InitializeComponent();
        }

        public FrmMedIpdOrdView(string strGBIO, int intStartRow, string strPtNo = "", bool bolSim = false)
        {
            InitializeComponent();

            GstrGBIO = strGBIO;
            GintStartRow = intStartRow;
            GstrPtNo = strPtNo;
            GbolSim = bolSim;
        }

        public FrmMedIpdOrdView(string strGBIO, int intStartRow, string strFrDate, string strToDate, string strPtNo = "", bool bolSim = false)
        {
            InitializeComponent();

            GstrGBIO = strGBIO;
            GintStartRow = intStartRow;
            GstrFrDate = strFrDate;
            GstrToDate = strToDate;
            GstrPtNo = strPtNo;
            GbolSim = bolSim;
        }

        private void FrmMedLastIpdOrdView_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            SetDtpDate();

            //2018.05.21 생성자로 날짜 지정하는 부분 추가
            if (GstrFrDate != "")
            {
                dtpFrDate.Value = Convert.ToDateTime(GstrFrDate);

                if (GstrToDate != "")
                {
                    dtpToDate.Value = Convert.ToDateTime(GstrToDate);
                }
                else
                {
                    dtpToDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
                }
            }

            if (clsOrdFunction.Pat.PtNo != "" && clsOrdFunction.Pat.PtNo != null)
            {
                GstrPtNo = clsOrdFunction.Pat.PtNo;
            }

            ssList_Sheet1.RowCount = 0;
            ssIlls_Sheet1.RowCount = 0;
            ssOrder_Sheet1.RowCount = 0;

            //if (clsType.User.JobMan == "간호사" || SendILLS == null || SendORD == null)
            //{
            //    btnSend.Visible = false;
            //}
            //else
            //{
            //    btnSend.Visible = true;
            //}

            Read_Ipd_NEW_MASTER();
            Read_Opd_Slip();

            ssOrder_Sheet1.Columns[79, 82].Visible = true;

            //if (GbolSim == true)
            //{
            //    ssOrder_Sheet1.Columns[79, 82].Visible = true;
            //}
            //else
            //{
            //    ssOrder_Sheet1.Columns[79, 82].Visible = false;
            //}
        }

        private void SetDtpDate()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(InDate,  'YYYY-MM-DD') AS InDate, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(SysDate, 'YYYY-MM-DD') AS SysDate1 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + "     WHERE GBSTS = '0' ";
                SQL = SQL + ComNum.VBLF + "         AND Pano = '" + GstrPtNo + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
                    dtpToDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
                }
                else
                {
                    dtpFrDate.Value = Convert.ToDateTime(dt.Rows[0]["INDATE"].ToString().Trim());
                    dtpToDate.Value = Convert.ToDateTime(dt.Rows[0]["SYSDATE1"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Read_Ipd_NEW_MASTER()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(InDate, 'YYYY-MM-DD') AS InDate1, DeptCode, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(ACTDate, 'YYYY-MM-DD') AS DcDate1, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.OutDate,'YYYY-MM-DD') AS OutDate1, DrName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B  ";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.DrCode = B.DrCode(+)";
                SQL = SQL + ComNum.VBLF + "ORDER BY InDate DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.RowCount = ssList_Sheet1.RowCount + 1;
                        ssList_Sheet1.SetRowHeight(ssList_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["INDATE1"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3].Text = "입원";
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["OUTDATE1"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Read_Opd_Slip()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     distinct TO_CHAR(BDate,'YYYY-MM-DD') AS BDate1, DeptCode, DrName ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A, " + ComNum.DB_PMPA + "BAS_DOCTOR B  ";
                SQL = SQL + ComNum.VBLF + "     WHERE Ptno = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND GbSunap = '1' ";
                SQL = SQL + ComNum.VBLF + "         AND A.DrCode = B.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY 1 DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList_Sheet1.RowCount = ssList_Sheet1.RowCount + 1;
                        ssList_Sheet1.SetRowHeight(ssList_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["DRNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3].Text = "외래";
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void chk_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked == true)
            {
                if (((CheckBox)sender) != chk5)
                {
                    chk5.Checked = false;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            ssList_Sheet1.RowCount = 0;
            ssIlls_Sheet1.RowCount = 0;
            ssOrder_Sheet1.RowCount = 0;

            Read_Ipd_NEW_MASTER();
            Read_Opd_Slip();
        }

        private void Read_Orders_IPD()
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SqlErr = "";
            int i = 0;

            double dblOrderNo = 0;
            string strUnit = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region GoSub Data_Read

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.*, TO_CHAR(A.EntDate,'YYYY-MM-DD HH24:Mi') AS EntDate1, A.ROWID,  ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.BDate,'YYYY-MM-DD') AS BDate1, TO_CHAR(A.PICKUPDATE,'YYYY-MM-DD HH24:Mi') AS PICKUPDATE  ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IORDER A ";
                SQL = SQL + ComNum.VBLF + "     WHERE Ptno = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND GbStatus IN (' ','D','D+')  ";
                SQL = SQL + ComNum.VBLF + "         AND BDate >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND BDate <= TO_DATE('" + dtpToDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                //손동현
                //아래 두줄은 간호업무에서 DC한 자료를 읽지 안기 위해서 추가함
                SQL = SQL + ComNum.VBLF + "         AND OrderSite Not Like 'DC%' ";
                SQL = SQL + ComNum.VBLF + "         AND OrderSite <> 'CAN' ";

                if (clsPublic.Gstr구두Chk == "OK")
                {
                    SQL = SQL + ComNum.VBLF + "         AND (VerbC IS NULL OR VerbC <> 'Y') ";  //의사확인한 구두처방건 제외
                }

                if (chk5.Checked == false)
                {
                    if (chk0.Checked == false) { SQL = SQL + ComNum.VBLF + "        AND NOT (Bun < '22' And Bun > '01' ) "; }
                    if (chk1.Checked == false) { SQL = SQL + ComNum.VBLF + "        AND NOT (Bun < '65' AND Bun > '21' ) "; }
                    if (chk2.Checked == false) { SQL = SQL + ComNum.VBLF + "        AND NOT (Bun > '64' ) "; }
                    if (chk3.Checked == false) { SQL = SQL + ComNum.VBLF + "        AND NOT (GbPRN = 'P' ) "; }
                    if (chk4.Checked == false) { SQL = SQL + ComNum.VBLF + "        AND NOT (OrderCode = 'S/O' ) "; }
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY BDate, Seqno ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    #region GoSub Move_Orders

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssOrder_Sheet1.RowCount = ssOrder_Sheet1.RowCount + 1;
                        ssOrder_Sheet1.SetRowHeight(ssOrder_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        if (i != 0)
                        {
                            if (dt.Rows[i]["BDATE1"].ToString().Trim() != dt.Rows[i - 1]["BDATE1"].ToString().Trim())
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                            }
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 79].Text = dt.Rows[i]["GBPICKUP"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 80].Text = dt.Rows[i]["PICKUPDATE"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 81].Text = clsVbfunc.GetInSaName(clsDB.DbCon, dt.Rows[i]["PICKUPSABUN"].ToString().Trim());
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 82].Text = clsVbfunc.READ_INSA_BUSE(clsDB.DbCon, dt.Rows[i]["PICKUPSABUN"].ToString().Trim());

                        //2021-01-11 투여시점, 투여시간 추가
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 83].Text = dt.Rows[i]["TUYEOPOINT"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 84].Text = dt.Rows[i]["TUYEOTIME"].ToString().Trim();

                        if ((VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 11 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 20) || dt.Rows[i]["BUN"].ToString().Trim() == "23")
                        {
                            if (VB.Val(dt.Rows[i]["CONTENTS"].ToString().Trim()) == 0)
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 6].Text = "";
                            }
                            else
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["CONTENTS"].ToString().Trim();
                            }

                            if (VB.Val(dt.Rows[i]["BCONTENTS"].ToString().Trim()) == 0)
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 34].Text = "";
                            }
                            else
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 34].Text = dt.Rows[i]["BCONTENTS"].ToString().Trim();
                            }
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 32].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 10].Text = clsVbfunc.GetOCSDrNameSabun(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim()) + " " + ComFunc.RightH(dt.Rows[i]["ENTDATE1"].ToString().Trim(), 10);
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 36].Text = clsVbfunc.GetOCSDrNameSabun(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim()) + " " + ComFunc.RightH(dt.Rows[i]["ENTDATE1"].ToString().Trim(), 10);

                        if (VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 16 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 21)
                        {
                            if (dt.Rows[i]["GBNGT"].ToString().Trim() != "") { ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["GBNGT"].ToString().Trim(); }
                            if (dt.Rows[i]["GBGROUP"].ToString().Trim() != "") { ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["GBGROUP"].ToString().Trim(); }
                        }
                        else if (VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 28 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 39)
                        {
                            //처치/재료는 GbNgt    나머지는 Group
                            if (VB.IsNumeric(dt.Rows[i]["GBGROUP"].ToString().Trim()) || dt.Rows[i]["GBGROUP"].ToString().Trim() == "")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["GBNGT"].ToString().Trim();
                            }
                            else
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                            }
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["GBER"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["GBSELF"].ToString().Trim();

                        if (dt.Rows[i]["SLIPNO"].ToString().Trim() == "A1"
                            || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A2"
                            || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A3"
                            || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A4") { }
                        else
                        {
                            if (dt.Rows[i]["REMARK"].ToString().Trim() != "") { ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text = "#"; }
                            if (dt.Rows[i]["GBPRN"].ToString().Trim() != "") { ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text = dt.Rows[i]["GBPRN"].ToString().Trim(); }
                            if (dt.Rows[i]["GBTFLAG"].ToString().Trim() != "") { ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text = dt.Rows[i]["GBTFLAG"].ToString().Trim(); }
                            if (dt.Rows[i]["GBPRN"].ToString().Trim() == "A") { ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text = ""; }

                            if (ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text.Trim() == "S")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text = "";
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 44].Text = "S";
                            }
                        }

                        if (dt.Rows[i]["GBPORT"].ToString().Trim() != "") { ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 15].Text = dt.Rows[i]["GBPORT"].ToString().Trim(); }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 16].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 17].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 18].Text = dt.Rows[i]["SLIPNO"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(dt.Rows[i]["SLIPNO"].ToString().Trim(), ComFunc.LeftH(dt.Rows[i]["DOSCODE"].ToString().Trim(), 2), dt.Rows[i]["BUN"].ToString().Trim()), 7);
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 33].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(dt.Rows[i]["SLIPNO"].ToString().Trim(), ComFunc.LeftH(dt.Rows[i]["DOSCODE"].ToString().Trim(), 2), dt.Rows[i]["BUN"].ToString().Trim()), 3);

                        switch (dt.Rows[i]["GBORDER"].ToString().Trim())
                        {
                            case "F":
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text = "Pre";
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 33].Text = "92";
                                break;
                            case "T":
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text = "Post";
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 33].Text = "93";
                                break;
                            case "M":
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text = "Adm";
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 33].Text = "91";
                                break;
                        }

                        if (dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D") { ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text = "D/C"; }

                        switch (dt.Rows[i]["REALQTY"].ToString().Trim())
                        {
                            case "1/2": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.5"; break;
                            case "1/3": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.33"; break;
                            case "2/3": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.66"; break;
                            case "1/4": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.25"; break;
                            case "3/4": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.75"; break;
                            case "1/5": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.2"; break;
                            case "2/5": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.4"; break;
                            case "3/5": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.6"; break;
                            case "4/5": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.8"; break;
                            default:
                                if (VB.IsNumeric(dt.Rows[i]["REALQTY"].ToString().Trim()))
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = VB.Val(dt.Rows[i]["REALQTY"].ToString().Trim()).ToString();
                                }
                                else
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "1";
                                }
                                break;
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 20].Text = dt.Rows[i]["DOSCODE"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 21].Text = dt.Rows[i]["GBBOTH"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 22].Text = dt.Rows[i]["GBINFO"].ToString().Trim();

                        if (dt.Rows[i]["SLIPNO"].ToString().Trim() == "A1"
                            || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A2"
                            || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A3"
                            || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A4")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 23].Text = "";
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 23].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 31].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();

                        dblOrderNo = VB.Val(dt.Rows[i]["ORDERNO"].ToString().Trim());

                        #region GoSub Order_Read

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     DispHeader AS cDispHeader, OrderName AS cOrderName, ";
                        SQL = SQL + ComNum.VBLF + "     DispRGB AS cDispRGB, GbBoth AS cGbBoth, GbInfo AS cGbInfo, ";
                        SQL = SQL + ComNum.VBLF + "     GbQty AS cGbQty, GbDosage AS cGbDosage, NextCode AS cNextCode, ";
                        SQL = SQL + ComNum.VBLF + "     OrderNameS AS cOrderNameS, GbImiv AS cGbImiv, DrugName AS cDrugName ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE ";
                        SQL = SQL + ComNum.VBLF + "     WHERE OrderCode = '" + dt.Rows[i]["OrderCode"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND Slipno = '" + dt.Rows[i]["Slipno"].ToString().Trim() + "' ";

                        #endregion

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt1.Rows.Count == 0)
                        {
                            if (dt.Rows[i]["SLIPNO"].ToString().Trim() == "A1"
                                || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A2"
                                || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A3"
                                || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A4")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].ForeColor = Color.Brown;
                            }
                            else
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "삭제된 코드입니다.. 변경요망";
                            }
                        }
                        else
                        {
                            #region GoSub Order_Move

                            if (dt1.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                            {
                                strUnit = dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = strUnit + " " + dt1.Rows[0]["CORDERNAMES"].ToString().Trim();
                            }
                            else if (dt1.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt1.Rows[0]["CDISPHEADER"].ToString().Trim() + " " + dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                            }
                            else
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                            }

                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 24].Text = dt1.Rows[0]["CDISPRGB"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 25].Text = dt1.Rows[0]["CGBBOTH"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 26].Text = dt1.Rows[0]["CGBINFO"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 27].Text = dt1.Rows[0]["CGBQTY"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 28].Text = dt1.Rows[0]["CGBDOSAGE"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 29].Text = dt1.Rows[0]["CNEXTCODE"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 30].Text = dt1.Rows[0]["CGBIMIV"].ToString().Trim();

                            if (dt1.Rows[0]["CGBINFO"].ToString().Trim() == "1")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["GBINFO"].ToString().Trim();
                            }
                            else if (dt1.Rows[0]["CGBDOSAGE"].ToString().Trim() == "1")
                            {
                                #region GoSub Read_Dosage

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     DosName";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ODOSAGE ";
                                SQL = SQL + ComNum.VBLF + "     WHERE DosCode = '" + dt.Rows[i]["DosCode"].ToString().Trim() + "' ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                if (dt2.Rows.Count > 0)
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = dt2.Rows[0]["DOSNAME"].ToString().Trim();
                                }

                                dt2.Dispose();
                                dt2 = null;

                                #endregion
                            }
                            else
                            {
                                #region GoSub Read_Specman

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     Specname";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OSPECIMAN ";
                                SQL = SQL + ComNum.VBLF + "     WHERE SpecCode = '" + dt.Rows[i]["DosCode"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND Slipno = '" + dt.Rows[i]["Slipno"].ToString().Trim() + "' ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                if (dt2.Rows.Count > 0)
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = dt2.Rows[0]["SPECNAME"].ToString().Trim();
                                }

                                dt2.Dispose();
                                dt2 = null;

                                #endregion
                            }

                            if (dt1.Rows[0]["CGBBOTH"].ToString().Trim() == "1")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = ComFunc.LeftH(ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text, 30) + dt.Rows[i]["GBINFO"].ToString().Trim();
                            }

                            if (ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text.Trim() == "D/C")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 0);
                            }
                            else
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt1.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                            }

                            //2012-11-26
                            if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "★항혈전 " + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].ForeColor = Color.FromArgb(255, 0, 255);
                            }

                            if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "<!> " + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                            }

                            //손동현 추가 오더 화면에 선수납관련이 보이게
                            if (dt.Rows[i]["GBPRN"].ToString().Trim() == "S")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "(A)" + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                            }
                            else if (dt.Rows[i]["GBPRN"].ToString().Trim() == "A")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "(선)" + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                            }
                            else if (dt.Rows[i]["GBPRN"].ToString().Trim() == "B")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "(수)" + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                            }

                            //손동현 추가 오더 화면에 오더코드 보이기
                            if (dt.Rows[i]["ORDERCODE"].ToString().Trim() != "")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = ComFunc.RPAD(dt.Rows[i]["ORDERCODE"].ToString().Trim(), 8, " ") + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                            }

                            if (dt.Rows[i]["GBPRN"].ToString().Trim() == "P")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 64].Text = "";
                            }

                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 65].Text = dt.Rows[i]["PRN_REMARK"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 66].Text = dt.Rows[i]["PRN_INS_GBN"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 67].Text = dt.Rows[i]["PRN_UNIT"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 68].Text = dt.Rows[i]["PRN_INS_SDATE"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 69].Text = dt.Rows[i]["PRN_INS_EDATE"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 70].Text = dt.Rows[i]["PRN_INS_MAX"].ToString().Trim();

                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 75].Text = dt.Rows[i]["PRN_DOSCODE"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 76].Text = dt.Rows[i]["PRN_TERM"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 77].Text = dt.Rows[i]["PRN_NOTIFY"].ToString().Trim();

                            #endregion
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (dt.Rows[i]["SLIPNO"].ToString().Trim() == "A1"
                            || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A2"
                            || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A3"
                            || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A4") { }
                        else
                        {
                            if (dt.Rows[i]["REMARK"].ToString().Trim() != "")
                            {
                                ssOrder_Sheet1.RowCount = ssOrder_Sheet1.RowCount + 1;
                                ssOrder_Sheet1.SetRowHeight(ssOrder_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 3].Text = "";
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["REMARK"].ToString().Trim();

                                if (ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text.Trim() == "D/C")
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 0);
                                }
                            }
                        }

                        //심사기준사항 disply
                        clsOrdFunction.GstrSimCode = ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 13].Text;
                        clsOrdFunction.GstrSimFlag = ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 45].Text;

                        clsOrdFunction.GstrSimYN = clsOrdFunction.SimSaGiJun_Check(clsDB.DbCon, clsOrdFunction.GstrSimFlag, clsOrdFunction.GstrSimCode, "IPD");

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 45].Text = clsOrdFunction.GstrSimYN;
                    }

                    #endregion
                }

                dt.Dispose();
                dt = null;

                #endregion

                Cursor.Current = Cursors.Default;

                Read_Ills_IPD();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Read_Orders_OPD()
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SqlErr = "";
            int i = 0;

            double dblOrderNo = 0;
            string strUnit = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                #region GoSub Data_Read

                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.*, A.ROWID, TO_CHAR(A.BDate,'YYYY-MM-DD') AS BDate1";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OORDER A ";
                SQL = SQL + ComNum.VBLF + "     WHERE Ptno = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND GbSunap = '1' ";
                SQL = SQL + ComNum.VBLF + "         AND DeptCode = '" + ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDate >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.BDate <= TO_DATE('" + dtpToDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (chk5.Checked == false)
                {
                    if (chk0.Checked == false) { SQL = SQL + ComNum.VBLF + "        AND NOT (Bun < '22') "; }
                    if (chk1.Checked == false) { SQL = SQL + ComNum.VBLF + "        AND NOT (Bun > '22' AND Bun < '65') "; }
                    if (chk2.Checked == false) { SQL = SQL + ComNum.VBLF + "        AND NOT (Bun > '64') "; }
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY BDate, Seqno ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        #region GoSub Move_Orders

                        ssOrder_Sheet1.RowCount = ssOrder_Sheet1.RowCount + 1;
                        ssOrder_Sheet1.SetRowHeight(ssOrder_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        if (i != 0)
                        {
                            if (dt.Rows[i]["BDATE1"].ToString().Trim() != dt.Rows[i - 1]["BDATE1"].ToString().Trim())
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                            }
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 10].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());

                        //손동현 아래 한줄 추가
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 36].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString().Trim());

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["GBER"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["GBSELF"].ToString().Trim();
                        
                        if (dt.Rows[i]["REMARK"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text = "#";
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 32].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 16].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 17].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 18].Text = dt.Rows[i]["SLIPNO"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(dt.Rows[i]["SLIPNO"].ToString().Trim(), ComFunc.LeftH(dt.Rows[i]["DOSCODE"].ToString().Trim(), 2), dt.Rows[i]["BUN"].ToString().Trim()), 7);
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 33].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(dt.Rows[i]["SLIPNO"].ToString().Trim(), ComFunc.LeftH(dt.Rows[i]["DOSCODE"].ToString().Trim(), 2), dt.Rows[i]["BUN"].ToString().Trim()), 3);

                        switch (dt.Rows[i]["REALQTY"].ToString().Trim())
                        {
                            case "1/2": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.5"; break;
                            case "1/3": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.33"; break;
                            case "2/3": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.66"; break;
                            case "1/4": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.25"; break;
                            case "3/4": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.75"; break;
                            case "1/5": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.2"; break;
                            case "2/5": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.4"; break;
                            case "3/5": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.6"; break;
                            case "4/5": ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.8"; break;
                            default:
                                if (VB.IsNumeric(dt.Rows[i]["REALQTY"].ToString().Trim()))
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = VB.Val(dt.Rows[i]["REALQTY"].ToString().Trim()).ToString();
                                }
                                else
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "1";
                                }
                                break;
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 20].Text = dt.Rows[i]["DOSCODE"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 21].Text = dt.Rows[i]["GBBOTH"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 22].Text = dt.Rows[i]["GBINFO"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 23].Text = dt.Rows[i]["REMARK"].ToString().Trim();

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 31].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();

                        dblOrderNo = VB.Val(dt.Rows[i]["ORDERNO"].ToString().Trim());

                        #region GoSub Order_Read

                        SQL = "";
                        SQL = "SELECT";
                        SQL = SQL + ComNum.VBLF + "     DispHeader AS cDispHeader, OrderName AS cOrderName, ";
                        SQL = SQL + ComNum.VBLF + "     DispRGB AS cDispRGB, GbBoth AS cGbBoth, GbInfo AS cGbInfo, ";
                        SQL = SQL + ComNum.VBLF + "     GbQty AS cGbQty, GbDosage AS cGbDosage, NextCode AS cNextCode, ";
                        SQL = SQL + ComNum.VBLF + "     OrderNameS AS cOrderNameS, GbImiv AS cGbImiv ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ORDERCODE ";
                        SQL = SQL + ComNum.VBLF + "     WHERE OrderCode = '" + dt.Rows[i]["OrderCode"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "         AND Slipno = '" + dt.Rows[i]["Slipno"].ToString().Trim() + "' ";

                        #endregion

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (dt1.Rows.Count == 0)
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "삭제된 코드입니다.. 변경요망";

                            if (dt.Rows[i]["REMARK"].ToString().Trim() != "")
                            {
                                ssOrder_Sheet1.RowCount = ssOrder_Sheet1.RowCount + 1;
                                ssOrder_Sheet1.SetRowHeight(ssOrder_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 3].Text = "";
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                                //ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 24].Text = dt.Rows[i]["DISPRGB"].ToString().Trim();
                            }

                            //심사기준사항
                            clsOrdFunction.GstrSimCode = ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 13].Text;
                            clsOrdFunction.GstrSimFlag = ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 45].Text;

                            clsOrdFunction.GstrSimYN = clsOrdFunction.SimSaGiJun_Check(clsDB.DbCon, clsOrdFunction.GstrSimFlag, clsOrdFunction.GstrSimCode, "OPD");

                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 45].Text = clsOrdFunction.GstrSimYN;
                        }
                        else
                        {
                            #region GoSub Order_Move

                            if (dt1.Rows[0]["CORDERNAMES"].ToString().Trim() != "")
                            {
                                strUnit = dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = strUnit + " " + dt1.Rows[0]["CORDERNAMES"].ToString().Trim();
                            }
                            else if (dt1.Rows[0]["CDISPHEADER"].ToString().Trim() != "")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt1.Rows[0]["CDISPHEADER"].ToString().Trim() + " " + dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                            }
                            else
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt1.Rows[0]["CORDERNAME"].ToString().Trim();
                            }

                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 24].Text = dt1.Rows[0]["CDISPRGB"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 25].Text = dt1.Rows[0]["CGBBOTH"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 26].Text = dt1.Rows[0]["CGBINFO"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 27].Text = dt1.Rows[0]["CGBQTY"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 28].Text = dt1.Rows[0]["CGBDOSAGE"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 29].Text = dt1.Rows[0]["CNEXTCODE"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 30].Text = dt1.Rows[0]["CGBIMIV"].ToString().Trim();

                            if (VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 11 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 20)
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 6].Text = dt1.Rows[0]["CNEXTCODE"].ToString().Trim();
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 35].Text = dt1.Rows[0]["CNEXTCODE"].ToString().Trim();
                            }

                            if (dt1.Rows[0]["CGBINFO"].ToString().Trim() == "1")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["GBINFO"].ToString().Trim();
                            }
                            else if (dt1.Rows[0]["CGBDOSAGE"].ToString().Trim() == "1")
                            {
                                #region GoSub Read_Dosage

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     DosName";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_ODOSAGE ";
                                SQL = SQL + ComNum.VBLF + "     WHERE DosCode = '" + dt.Rows[i]["DosCode"].ToString().Trim() + "' ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                if (dt2.Rows.Count > 0)
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = dt2.Rows[0]["DOSNAME"].ToString().Trim();
                                }

                                dt2.Dispose();
                                dt2 = null;

                                #endregion
                            }
                            else
                            {
                                #region GoSub Read_Specman

                                SQL = "";
                                SQL = "SELECT";
                                SQL = SQL + ComNum.VBLF + "     Specname";
                                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OSPECIMAN ";
                                SQL = SQL + ComNum.VBLF + "     WHERE SpecCode = '" + dt.Rows[i]["DosCode"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "         AND Slipno = '" + dt.Rows[i]["Slipno"].ToString().Trim() + "' ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                                if (dt2.Rows.Count > 0)
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = dt2.Rows[0]["SPECNAME"].ToString().Trim();
                                }

                                dt2.Dispose();
                                dt2 = null;

                                #endregion
                            }

                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt1.Rows[0]["CDISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));

                            //2012-11-26
                            if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "★항혈전 " + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].ForeColor = Color.FromArgb(255, 0, 255);
                            }

                            if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "<!> " + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                            }

                            //손동현 추가 오더 화면에 오더코드 보이기
                            if (dt.Rows[i]["ORDERCODE"].ToString().Trim() != "")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = ComFunc.RPAD(dt.Rows[i]["ORDERCODE"].ToString().Trim(), 8, " ") + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                            }

                            #endregion
                        }

                        dt1.Dispose();
                        dt1 = null;

                        #endregion
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
                
                Cursor.Current = Cursors.Default;

                Read_Ills_OPD();
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Read_Ills_IPD()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.*, TO_CHAR(A.BDate,'YYYY-MM-DD') AS BDate1, IllNameE, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.RemoveDate,'YYYY-MM-DD') AS RemoveDate1, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.CfDate,'YYYY-MM-DD') AS CfDate1, ";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(A.EDate,'YYYY-MM-DD') AS EDate1 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IILLS A, " + ComNum.DB_PMPA + "BAS_ILLS B ";
                SQL = SQL + ComNum.VBLF + "     WHERE A.Ptno = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "         AND BDate >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND BDate <= TO_DATE('" + dtpToDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND A.IllCode = B.IllCode  ";
                SQL = SQL + ComNum.VBLF + "         AND B.IllCLASS = '1' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BDate, Main ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssIlls_Sheet1.RowCount = ssIlls_Sheet1.RowCount + 1;
                        ssIlls_Sheet1.SetRowHeight(ssIlls_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["ILLNAMEE"].ToString().Trim();
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 6].Text = ComFunc.MidH(dt.Rows[i]["REMOVEDATE1"].ToString().Trim(), 6, 5);
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 7].Text = ComFunc.MidH(dt.Rows[i]["CFDATE1"].ToString().Trim(), 6, 5);
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["EDATE1"].ToString().Trim();

                        if (dt.Rows[0]["DEPTCODE"].ToString().Trim() == "DN" || dt.Rows[0]["DEPTCODE"].ToString().Trim() == "DT")
                        {
                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["BOOWI1"].ToString().Trim();
                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["BOOWI2"].ToString().Trim();
                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["BOOWI3"].ToString().Trim();
                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["BOOWI4"].ToString().Trim();
                        }
                        else
                        {
                            if (dt.Rows[i]["MAIN"].ToString().Trim() == "*")
                            {
                                ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 5].BackColor = Color.FromArgb(0, 0, 255);
                            }

                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["RO"].ToString().Trim();

                            if (ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 2].Text.Trim() != "")
                            {
                                ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 0, ssIlls_Sheet1.RowCount - 1, 7].BackColor = Color.FromArgb(255, 234, 234);
                            }
                        }

                        if (dt.Rows[i]["EDATE1"].ToString().Trim() != "")
                        {
                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 0, ssIlls_Sheet1.RowCount - 1, 7].BackColor = Color.FromArgb(234, 234, 255);
                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 8].Text = ComFunc.MidH(dt.Rows[i]["EDATE1"].ToString().Trim(), 6, 5);
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void Read_Ills_OPD()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     A.*, TO_CHAR(A.BDate,'YYYY-MM-DD') AS BDate1, IllNameE ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_MED + "OCS_OILLS A, " + ComNum.DB_PMPA + "BAS_ILLS B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.Ptno = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND BDate >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND BDate <= TO_DATE('" + dtpToDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND A.IllCode = B.IllCode  ";
                SQL = SQL + ComNum.VBLF + "  AND B.IllCLASS = '1' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i <dt.Rows.Count; i++)
                    {
                        ssIlls_Sheet1.RowCount = ssIlls_Sheet1.RowCount + 1;
                        ssIlls_Sheet1.SetRowHeight(ssIlls_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["ILLNAMEE"].ToString().Trim();

                        if (dt.Rows[0]["DEPTCODE"].ToString().Trim() == "DT")
                        {
                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["BOOWI1"].ToString().Trim();
                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["BOOWI2"].ToString().Trim();
                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["BOOWI3"].ToString().Trim();
                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["BOOWI4"].ToString().Trim();
                        }
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            //ssListCellClick(e.Row);
        }

        private void ssListCellClick(int intRow)
        {
            //if (dtpFrDate.Value > Convert.ToDateTime(ssList_Sheet1.Cells[intRow, 0].Text))
            //{
            //    dtpFrDate.Value = Convert.ToDateTime(ssList_Sheet1.Cells[intRow, 0].Text);
            //}

            //if (ssList_Sheet1.Cells[intRow, 4].Text.Trim() == "1990-01-01")
            //{
            //    if (dtpFrDate.Value < Convert.ToDateTime(ssList_Sheet1.Cells[intRow, 0].Text))
            //    {
            //        dtpToDate.Value = Convert.ToDateTime(ssList_Sheet1.Cells[intRow, 0].Text);
            //    }
            //}
            //else
            //{
            //    if (ssList_Sheet1.Cells[intRow, 4].Text != "")
            //    {
            //        if (dtpToDate.Value < Convert.ToDateTime(ssList_Sheet1.Cells[intRow, 4].Text))
            //        {
            //            dtpToDate.Value = Convert.ToDateTime(ssList_Sheet1.Cells[intRow, 4].Text);
            //        }
            //    }
            //}
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssListCellDoubleClick(e.Row);
        }

        private void ssListCellDoubleClick(int intRow)
        {
            ssIlls_Sheet1.RowCount = 0;
            ssOrder_Sheet1.RowCount = 0;

            //dtpFrDate.Value = Convert.ToDateTime(ssList_Sheet1.Cells[intRow, 0].Text);

            //if (ssList_Sheet1.Cells[intRow, 4].Text.Trim() != "")
            //{
            //    dtpToDate.Value = Convert.ToDateTime(ssList_Sheet1.Cells[intRow, 4].Text);
            //}
            //else
            //{
            //    dtpToDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            //}

            if (ssList_Sheet1.Cells[intRow, 3].Text.Trim() == "입원")
            {
                Read_Orders_IPD();
            }
            else
            {
                Read_Orders_OPD();
            }
        }
        
        private void ssIlls_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (Convert.ToBoolean(ssIlls_Sheet1.Cells[e.Row, 0].Value) == true)
            {
                ssIlls_Sheet1.Cells[e.Row, 0, e.Row, ssIlls_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
            }
            else
            {
                ssIlls_Sheet1.Cells[e.Row, 0, e.Row, ssIlls_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            }
        }

        private void ssIlls_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SendILLS != null)
            {
                SendILLS(ssIlls_Sheet1.Cells[e.Row, 3].Text.Trim(), ssIlls_Sheet1.Cells[e.Row, 4].Text.Trim(), ssIlls_Sheet1.Cells[e.Row, 2].Text.Trim());
            }
        }

        private void ssOrder_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (Convert.ToBoolean(ssOrder_Sheet1.Cells[e.Row, 0].Value) == true)
                {
                    //2012-12-06
                    if (ssOrder_Sheet1.Cells[e.Row, 3].Text.Trim() == "V001" || ssOrder_Sheet1.Cells[e.Row, 3].Text == "S/O") { }
                    else
                    {
                        SQL = "";
                        SQL = "SELECT * FROM " + ComNum.DB_MED + "OCS_ORDERCODE ";
                        SQL = SQL + ComNum.VBLF + "     WHERE OrderCode = '" + ssOrder_Sheet1.Cells[e.Row, 3].Text.Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                        if (dt.Rows.Count == 0)
                        {
                            if (ssOrder_Sheet1.Cells[e.Row, 2].Text.Trim() == "S/O") { }
                            else
                            {
                                ComFunc.MsgBox("삭제된 수가 입니다. 처방을 다시 선택 하세요.", "삭제코드");
                                ssOrder_Sheet1.Cells[e.Row, 0].Value = false;
                                dt.Dispose();
                                dt = null;
                                return;
                            }
                        }
                        else
                        {
                            if (dt.Rows[0]["SENDDEPT"].ToString().Trim() == "N")
                            {
                                ComFunc.MsgBox("삭제된 수가 입니다. 처방을 다시 선택 하세요.", "삭제코드");
                                ssOrder_Sheet1.Cells[e.Row, 0].Value = false;
                                dt.Dispose();
                                dt = null;
                                return;
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    }

                    ssOrder_Sheet1.Cells[e.Row, 0, e.Row, ssOrder_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
                }
                else
                {
                    ssOrder_Sheet1.Cells[e.Row, 0, e.Row, ssOrder_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
                }
                
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssOrder_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strOrdIO = ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 3].Text.Trim() == "외래" ? "OPD" : "IPD";
            strOrdIO = ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim() == "ER" ? "ER" : strOrdIO;

            SendORD?.Invoke(ssOrder_Sheet1.Cells[e.Row, 32].Text.Trim(), strOrdIO, GintStartRow);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string strOrdIO = ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 3].Text.Trim() == "외래" ? "OPD" : "IPD";
            strOrdIO = ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim() == "ER" ? "ER" : strOrdIO;

            if (strOrdIO == "OPD")
            {
                MessageBox.Show("외래 처방은 처방이용이 불가능 합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            int i = 0;

            for (i = 0; i < ssIlls_Sheet1.RowCount; i++)
            {
                if (ssIlls_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                {
                    SendILLS?.Invoke(ssIlls_Sheet1.Cells[i, 3].Text.Trim(), ssIlls_Sheet1.Cells[i, 4].Text.Trim(), ssIlls_Sheet1.Cells[i, 2].Text.Trim());
                }
            }

            for (i = 0; i < ssOrder_Sheet1.RowCount; i++)
            {
                if (ssOrder_Sheet1.Cells[i, 0].Text.Trim().Equals("True"))
                {
                    SendORD?.Invoke(ssOrder_Sheet1.Cells[i, 32].Text.Trim(), strOrdIO, GintStartRow);
                }
            }
            
            //Dim i                   As Integer
            //Dim j                   As Integer
            //Dim strColor            As String
            //Dim strSuCode           As String
            //On Error Resume Next

            //For i = 1 To SSOrder.DataRowCnt
            //    SSOrder.Row = i
            //    SSOrder.Col = 1
            //    If Trim(SSOrder.Text) = "1" Then
            //        SSOrder.Col = 17: strSuCode = Trim(SSOrder.Text)
            //        SSOrder.Col = 4     'Code
            //        If Trim(SSOrder.Text) <> "" Then
            //            If GOrderFORM.SSOrder.MaxRows = GOrderFORM.SSOrder.DataRowCnt Then
            //                GOrderFORM.SSOrder.MaxRows = GOrderFORM.SSOrder.MaxRows + 1
            //            End If
            //            GOrderFORM.SSOrder.Row = GOrderFORM.SSOrder.DataRowCnt + 1
                        
            //            For j = 5 To 36
            //                If j < 11 Then
            //                    SSOrder.Col = j
            //                    GOrderFORM.SSOrder.Col = j - 2
            //                    GOrderFORM.SSOrder.Text = SSOrder.Text
            //                    If j = 5 Then
            //                        If READ_SUGA_항혈전수가(strSuCode) = "OK" Then
            //                            GOrderFORM.SSOrder.BackColor = RGB(255, 0, 0)
            //                        End If
            //                    End If
            //                ElseIf j = 15 Then
            //                    SSOrder.Col = j
            //                    If Trim(SSOrder.Text) <> "#" Then
            //                        GOrderFORM.SSOrder.Col = j - 2
            //                        GOrderFORM.SSOrder.Text = SSOrder.Text
            //                    End If
            //                ElseIf j < 34 Then
            //                    SSOrder.Col = j
            //                    GOrderFORM.SSOrder.Col = j - 2
            //                    GOrderFORM.SSOrder.Text = SSOrder.Text
            //                Else
            //                    SSOrder.Col = j
            //                    GOrderFORM.SSOrder.Col = j + 2
            //                    GOrderFORM.SSOrder.Text = SSOrder.Text
            //                End If
            //            Next j

            //            SSOrder.Col = 3
            //            GOrderFORM.SSOrder.Col = 2
            //            GOrderFORM.SSOrder.Text = Trim(SSOrder.Text)

            //            SSOrder.Col = 4
            //            GOrderFORM.SSOrder.Col = 35
            //            GOrderFORM.SSOrder.Text = Trim(SSOrder.Text)

            //            GOrderFORM.SSOrder.Col = 30         'OrderNo는 Space로
            //            GOrderFORM.SSOrder.Text = ""
            //            GOrderFORM.SSOrder.Col = 31         'ROWID는 Space로
            //            GOrderFORM.SSOrder.Text = ""
            //            GOrderFORM.SSOrder.Col = 33
            //            GOrderFORM.SSOrder.Text = GOrderFORM.SSOrder.Row

            //            SSOrder.Col = 35
            //            GOrderFORM.SSOrder.Col = 39
            //            GOrderFORM.SSOrder.Text = Trim(SSOrder.Text)
            //            GOrderFORM.SSOrder.Col = 5

            //            If Trim(GOrderFORM.SSOrder.Text) = "0" Then GOrderFORM.SSOrder.Text = ""

            //            GOrderFORM.SSOrder.Col = 23
            //            strColor = GOrderFORM.SSOrder.Text

            //            GOrderFORM.SSOrder.Col = 37
            //            GOrderFORM.SSOrder.Text = ""
            //            GOrderFORM.SSOrder.Col = 38
            //            GOrderFORM.SSOrder.Text = ""

            //            GOrderFORM.SSOrder.Row = GOrderFORM.SSOrder.Row
            //            GOrderFORM.SSOrder.Row2 = GOrderFORM.SSOrder.Row
            //            GOrderFORM.SSOrder.Col = 2:     GOrderFORM.SSOrder.col2 = GOrderFORM.SSOrder.MaxCols
            //            GOrderFORM.SSOrder.BlockMode = True
            //            GOrderFORM.SSOrder.ForeColor = "&H" & Format(Trim(strColor), "00000000")
            //            GOrderFORM.SSOrder.BlockMode = False

            //            '손동현 선수납 항목 체크
            //            GOrderFORM.SSOrder.Col = 3
            //            '수가12
            //            If MidH(Trim(GOrderFORM.SSOrder.Text), 9, 4) = "(수)" Then
            //                GOrderFORM.SSOrder.Text = LeftH(Trim(GOrderFORM.SSOrder.Text), 8) & Replace(MidH(Trim(GOrderFORM.SSOrder.Text), 9, 4), "(수)", "") & MidH(Trim(GOrderFORM.SSOrder.Text), 13)
            //            ElseIf MidH(Trim(GOrderFORM.SSOrder.Text), 9, 4) = "(선)" Then
            //                GOrderFORM.SSOrder.Text = LeftH(Trim(GOrderFORM.SSOrder.Text), 8) & Replace(MidH(Trim(GOrderFORM.SSOrder.Text), 9, 4), "(선)", "") & MidH(Trim(GOrderFORM.SSOrder.Text), 13)
            //            ElseIf MidH(Trim(GOrderFORM.SSOrder.Text), 9, 3) = "(A)" Then
            //                GOrderFORM.SSOrder.Text = LeftH(Trim(GOrderFORM.SSOrder.Text), 8) & Replace(MidH(Trim(GOrderFORM.SSOrder.Text), 9, 3), "(A)", "") & MidH(Trim(GOrderFORM.SSOrder.Text), 12)
            //            End If

            //            SSOrder.Col = 17: strSuCode = Trim(SSOrder.Text)
            //            GstrSQL = " SELECT    SuGbN "
            //            GstrSQL = GstrSQL & " FROM ADMIN.BAS_SUN  "
            //            GstrSQL = GstrSQL & "WHERE SuNext = '" & Trim(SSOrder.Text) & "' "
            //            GstrSQL = GstrSQL & "  AND SuGbN  = '1' "
            //            result = AdoOpenSet(AdoRes, GstrSQL)
            //            If RowIndicator > 0 Then
            //                GOrderFORM.SSOrder.Col = 46: GOrderFORM.SSOrder.Text = "S"
            //                GOrderFORM.SSOrder.Col = 3: GOrderFORM.SSOrder.Text = LeftH(Trim(GOrderFORM.SSOrder.Text), 8) & "(A)" & MidH(Trim(GOrderFORM.SSOrder.Text), 9)
            //            Else
            //                GOrderFORM.SSOrder.Col = 46: GOrderFORM.SSOrder.Text = ""
            //            End If
            //            AdoCloseSet AdoRes

            //            GOrderFORM.SSOrder.Col = 54: GOrderFORM.SSOrder.Text = READ_MAYAK(strSuCode)

            //            If Gstr산제Chk <> "OK" Then
            //                If READ_POWDER(strSuCode) = "Y" Then
            //                   GOrderFORM.SSOrder.Col = 9:
            //                   'CHECK 박스변경
            //                   GOrderFORM.SSOrder.CellType = SS_CELL_TYPE_CHECKBOX
            //                   GOrderFORM.SSOrder.TypeCheckText = ""
            //                   GOrderFORM.SSOrder.TypeCheckTextAlign = SS_CHECKBOX_TEXT_RIGHT
            //                   GOrderFORM.SSOrder.TypeHAlign = SS_CELL_H_ALIGN_CENTER
            //                   GOrderFORM.SSOrder.TypeVAlign = SS_CELL_V_ALIGN_VCENTER
            //                   GOrderFORM.SSOrder.TypeCheckType = SS_CHECKBOX_NORMAL
            //                   GOrderFORM.SSOrder.TypeCheckPicture(0) = LoadPicture("")
            //                   GOrderFORM.SSOrder.TypeCheckPicture(1) = LoadPicture("")
            //                   GOrderFORM.SSOrder.TypeCheckPicture(2) = LoadPicture("")
            //                   GOrderFORM.SSOrder.TypeCheckPicture(3) = LoadPicture("")
            //                   GOrderFORM.SSOrder.TypeCheckPicture(4) = LoadPicture("")
            //                   GOrderFORM.SSOrder.TypeCheckPicture(5) = LoadPicture("")
            //                End If
            //            Else
            //                If Gstr파우더New_STS = "Y" Then
            //                    Gstr파우더Gubun = ""
            //                    If READ_POWDER_SuCode_NEW(Trim(strSuCode)) = "OK" Then
            //                        If GnReadOrder < i Then
            //                            '''GOrderFORM.SSOrder.Row = i
            //                            GOrderFORM.SSOrder.Col = 9:
            //                            'CHECK 박스변경
            //                            GOrderFORM.SSOrder.CellType = SS_CELL_TYPE_CHECKBOX
            //                            GOrderFORM.SSOrder.TypeCheckText = ""
            //                            GOrderFORM.SSOrder.TypeCheckTextAlign = SS_CHECKBOX_TEXT_RIGHT
            //                            GOrderFORM.SSOrder.TypeHAlign = SS_CELL_H_ALIGN_CENTER
            //                            GOrderFORM.SSOrder.TypeVAlign = SS_CELL_V_ALIGN_VCENTER
            //                            GOrderFORM.SSOrder.TypeCheckType = SS_CHECKBOX_NORMAL
            //                            GOrderFORM.SSOrder.Text = "1"
            //                        End If
            //                    End If
            //                End If
            //            End If

            //            Call Verbal_Remark(GOrderFORM.SSOrder.DataRowCnt)

            //            '심사기준사항 disply
            //            GOrderFORM.SSOrder.Row = GOrderFORM.SSOrder.Row: GOrderFORM.SSOrder.Col = 15: GstrSimCode = GOrderFORM.SSOrder.Text
            //            GOrderFORM.SSOrder.Row = GOrderFORM.SSOrder.Row: GOrderFORM.SSOrder.Col = 47: GstrSimFlag = GOrderFORM.SSOrder.Text
            //            GstrSimYN = SimSaGiJun_Check(GstrSimFlag, GstrSimCode)
            //            GOrderFORM.SSOrder.Row = GOrderFORM.SSOrder.Row: GOrderFORM.SSOrder.Col = 47: GOrderFORM.SSOrder.Text = GstrSimYN
            //        End If
            //    End If
            //Next i
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (rEventClosed != null)
            {
                rEventClosed();
            }
            else
            {
                this.Close();
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ssOrder_Sheet1.RowCount == 0) { return; }
            if (ComFunc.MsgBoxQ("Order 내역을 출력 하시겠습니까?", "출력", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No) { return; }

            string strFont1 = "";
            string strHead1 = "";
            string strFont2 = "";
            string strHead2 = "";

            ssOrder_Sheet1.Columns[0].Visible = false;
            ssOrder_Sheet1.Columns[10].Visible = false;
            ssOrder_Sheet1.Columns[13, 14].Visible = false;
            ssOrder_Sheet1.Columns[16, 77].Visible = false;

            strFont1 = "/fn\"맑은 고딕\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"맑은 고딕\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "처방 내역 조회" + "/f1/n";
            strHead2 = "/r/f2" + "출력시간 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A", "-") + VB.Space(10) + "/f2/n";

            ssOrder_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssOrder_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Portrait;
            ssOrder_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssOrder_Sheet1.PrintInfo.Margin.Top = 20;
            ssOrder_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssOrder_Sheet1.PrintInfo.Margin.Header = 10;
            ssOrder_Sheet1.PrintInfo.ShowColor = false;
            ssOrder_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssOrder_Sheet1.PrintInfo.ShowBorder = true;
            ssOrder_Sheet1.PrintInfo.ShowGrid = true;
            ssOrder_Sheet1.PrintInfo.ShowShadows = false;
            ssOrder_Sheet1.PrintInfo.UseMax = true;
            ssOrder_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssOrder_Sheet1.PrintInfo.UseSmartPrint = false;
            ssOrder_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssOrder_Sheet1.PrintInfo.Preview = false;
            ssOrder.PrintSheet(0);

            Application.DoEvents();
            Task.Delay(2500);

            ssOrder_Sheet1.Columns[0].Visible = true;
            ssOrder_Sheet1.Columns[10].Visible = true;
            ssOrder_Sheet1.Columns[13, 14].Visible = true;
            ssOrder_Sheet1.Columns[16, 77].Visible = true;
        }
    }
}

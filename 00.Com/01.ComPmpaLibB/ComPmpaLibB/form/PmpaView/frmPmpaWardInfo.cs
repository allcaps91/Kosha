using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComBase; //기본 클래스
using System.Threading;
using ComDbB;

namespace ComPmpaLibB
{
    public partial class frmPmpaWardInfo : Form
    {
        clsSpread Spd = new clsSpread();

        public delegate void EventClosed(Form frm);
        public static event EventClosed rEventClosed;

        int FnCol = 0;
        int FnSeqNo = 0;
        string FstrWard = string.Empty;
        string FstrRoom = string.Empty;
        string FstrPano = string.Empty;
        
        public frmPmpaWardInfo()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }


        #region //MainFormMessage
        public MainFormMessage mCallForm = null;
        public void MsgActivedForm(Form frm)
        {

        }
        public void MsgUnloadForm(Form frm)
        {

        }
        public void MsgFormClear()
        {

        }
        public void MsgSendPara(string strPara)
        {

        }
        #endregion

        public frmPmpaWardInfo(MainFormMessage pform)
        {
            mCallForm = pform;
            InitializeComponent();
            SetControl();
            SetEvents();
        }


        void eFormActivated(object sender, EventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgActivedForm(this);
            }
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.mCallForm == null)
            {
                return;
            }
            else
            {
                this.mCallForm.MsgUnloadForm(this);
            }
        }

        void setEvent()
        {
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);
        }


        void SetControl()
        {
            cboWard.Items.Clear();
            cboWard.Items.Add("전체");
            clsVbfunc.SetWardCodeCombo(clsDB.DbCon, cboWard, "1", false, 2);
            clsVbfunc.SetDrCodeCombo(clsDB.DbCon, cboDoctor, "전체", "1", 1, "");
            
        }
        
        void SetEvents()
        {
            this.Load                += new EventHandler(frmPmpaWardInfo_Load);
            this.btnSearch.Click     += new EventHandler(btnSearch_Click);
            this.btnExit.Click       += new EventHandler(btnExit_Click);
            this.btnClose.Click      += new EventHandler(eBtnClick);
            this.btnSave_Res.Click   += new EventHandler(eBtnClick);
            this.btnDel.Click        += new EventHandler(eBtnClick);

            this.SS1.CellClick       += new CellClickEventHandler(eSpdClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnClose)
            {
                panRemark.Visible = false;
            }
            else if (sender == btnSave_Res)
            {
                eSave_Res(clsDB.DbCon, "등록");
            }
            else if (sender == btnDel)
            {
                eSave_Res(clsDB.DbCon, "삭제");
            }
        }

        void eSave_Res(PsmhDb pDbCon, string strJob)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strRowID = string.Empty;
            string strGubun = string.Empty; //'1.이실, 2.예약 구분

            strGubun = "1";

            if (rdoGubun2.Checked)
            {
                strGubun = "2";
            }

            if (txtRemark.Text.Trim() == "")
            {
                ComFunc.MsgBox("내용을 입력후 등록하세요", "확인");
                return;
            }

            try
            {
                SQL = "";
                SQL += " SELECT ROWID FROM " + ComNum.DB_PMPA + "BAS_ROOM_RESERVED          \r\n";
                SQL += "  WHERE WARDCODE = '" + FstrWard + "'                               \r\n";
                SQL += "    AND ROOMCODE = '" + FstrRoom + "'                               \r\n";
                SQL += "    AND ( PANO = '" + FstrPano + "' OR SEQNO = '" + FnSeqNo + "')       ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strRowID = dt.Rows[0]["ROWID"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                clsDB.setBeginTran(pDbCon);

                if (strJob == "삭제")
                {
                    SQL = "";
                    SQL += " UPDATE " + ComNum.DB_PMPA + "BAS_ROOM_RESERVED     ";
                    SQL += "    SET DELDATE = SYSDATE,                          ";
                    SQL += "        Gubun = '1',                                ";
                    SQL += "        DELSABUN = " + clsType.User.IdNumber + "    ";
                    SQL += " WHERE ROWID = '" + strRowID + "'                   ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }
                }
                else
                {
                    if (strRowID != "")
                    {
                        SQL = "";
                        SQL += " UPDATE " + ComNum.DB_PMPA + "BAS_ROOM_RESERVED     ";
                        SQL += "    SET RESERVED = '" + txtRemark.Text + "',        ";
                        SQL += "        DelSabun = 0,                               ";
                        SQL += "        DELDATE = '',                               ";
                        SQL += "        Gubun = '" + strGubun + "',                 ";
                        SQL += "        ENTSABUN = " + clsType.User.IdNumber + ",   ";
                        SQL += "        ENTDATE = SYSDATE                           ";
                        SQL += " WHERE ROWID = '" + strRowID + "'                   ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return;
                        }
                    }
                    else
                    {
                        SQL = "";
                        SQL += " INSERT INTO " + ComNum.DB_PMPA + "BAS_ROOM_RESERVED (              ";
                        SQL += "        WARDCODE, ROOMCODE, PANO, SEQNO, RESERVED,GUBUN, ENTSABUN,  ";
                        SQL += "        ENTDATE, DELSABUN, DELDATE                                  ";
                        SQL += "        ) VALUES ( ";
                        SQL += "        '" + FstrWard + "',                                         ";
                        SQL += "        '" + FstrRoom + "',                                         ";
                        SQL += "        '" + FstrPano + "',                                         ";
                        SQL += "        '" + FnSeqNo + "',                                          ";
                        SQL += "        '" + txtRemark.Text + "',                                   ";
                        SQL += "        '" + strGubun + "',                                         ";
                        SQL += "        '" + clsType.User.IdNumber + "',                            ";
                        SQL += "        SYSDATE, '','')                                             ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                            return;
                        }
                    }
                }
                
                clsDB.setCommitTran(pDbCon);

                ComFunc.MsgBox("등록완료.", "확인");

                panRemark.Visible = false;  

                GetData();


            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strWard = string.Empty;
            string strSabun = string.Empty;
            string strSname = string.Empty;
            string strBuse = string.Empty;
            string strEntDate = string.Empty;

            int nRoom = 0;
            int nGCnt = 0, nJCnt = 0, nTCnt = 0;

            if (e.Row < 0 || e.Column < 0) { return; }

            if (e.Column == 1)
            {
                lblInfo.Text = "";

                strWard = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                nRoom = (int)VB.Val(VB.Right(SS1.ActiveSheet.Cells[e.Row, 1].Text, 3));

                SQL = "";
                SQL += " SELECT WARDCODE,ROOMCODE,WARDAMT,DECODE(OVERAMT,'0',OVERAMT,OVERAMT+60000) OVERAMT   \r\n";
                SQL += "   FROM " + ComNum.DB_PMPA + "BAS_ROOM      \r\n";
                SQL += "  WHERE WARDCODE ='" + strWard + "'         \r\n";
                SQL += "    AND ROOMCODE =" + nRoom + "             \r\n";
                SQL += "    AND TBED > 0                                ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblInfo.Text = "               병실차액 : " + VB.Val(dt.Rows[0]["OVERAMT"].ToString()).ToString("###,###,###") + "원";
                }

                dt.Dispose();
                dt = null;

                return;
            }

            if (e.Column < 7) { return; }

            FnSeqNo = e.Column - 6;

            //가동병상수
            nGCnt = (int)VB.Val(SS1.ActiveSheet.Cells[e.Row, 5].Text);

            //잔여병상수
            if ((int)VB.Val(SS1.ActiveSheet.Cells[e.Row, 6].Text) > 0)
            {
                nJCnt = (int)VB.Val(SS1.ActiveSheet.Cells[e.Row, 6].Text);
            }

            nTCnt = nGCnt + nJCnt;

            FstrWard = SS1.ActiveSheet.Cells[e.Row, 15].Text.Trim();
            FnSeqNo = (e.Column - 6) + (int)VB.Val(SS1.ActiveSheet.Cells[e.Row, 25].Text);

            if (FstrWard != "IU" && FstrWard != "ND")
            {
                if (nTCnt + 7 < e.Column)
                    return;
            }

            if (FstrWard == "SUB" || FstrWard == "총" || FstrWard == "") { return; }

            FstrRoom = SS1.ActiveSheet.Cells[e.Row, 16].Text.Trim();
            FstrPano = SS1.ActiveSheet.Cells[e.Row, e.Column + 9].Text.Trim();

            lblRoom.Text = "병동: " + FstrWard + " " + "호실: " + FstrRoom;

            SQL = "";
            SQL += " SELECT RESERVED,ENTSABUN,TO_CHAR(EntDate,'MM/DD HH24:MI') ENTDATE,GUBUN    \r\n";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_ROOM_RESERVED                             \r\n";
            SQL += "  WHERE WARDCODE = '" + FstrWard + "'                                       \r\n";
            SQL += "    AND ROOMCODE = '" + FstrRoom + "'                                       \r\n";
            SQL += "    AND (PANO = '" + FstrPano + "' or  SEQNO = '" + FnSeqNo + "' )          \r\n";
            SQL += "    AND DELDATE IS NULL                                                         ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }


            txtRemark.Text = "";
            rdoGubun1.Checked = true;  //기본 이실

            if (dt.Rows.Count > 0)
            {
                txtRemark.Text = dt.Rows[0]["Reserved"].ToString().Trim();
                strEntDate = dt.Rows[0]["ENTDATE"].ToString().Trim();

                if (dt.Rows[0]["Gubun"].ToString().Trim() == "2")
                {
                    rdoGubun2.Checked = true;  //예약
                }

                if (VB.Val(dt.Rows[0]["ENTSABUN"].ToString()) > 99999)
                {
                    strSabun = VB.Val(dt.Rows[0]["ENTSABUN"].ToString()).ToString("000000");
                }
                else
                {
                    strSabun = VB.Val(dt.Rows[0]["ENTSABUN"].ToString()).ToString("00000");
                }
            }

            dt.Dispose();
            dt = null;

            if (strSabun != "")
            {
                SQL = "";
                SQL += " SELECT KorName,Buse FROM " + ComNum.DB_ERP + "INSA_MST ";
                SQL += "  WHERE Sabun = '" + strSabun + "' ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strSname = dt.Rows[0]["KorName"].ToString().Trim(); 
                    strBuse = clsVbfunc.GetBASBuSe(clsDB.DbCon, dt.Rows[0]["Buse"].ToString().Trim());
                    lblRoom.Text = lblRoom.Text + ComNum.VBLF + "등록자 : " + strSname + "(" + strBuse + ") " + strEntDate;
                }

                dt.Dispose();
                dt = null;
            }
            
            panRemark.Visible = true;
            txtRemark.Focus();
            
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int nGCnt = 0, nJCnt = 0, nTCnt = 0;
            string strMsg = string.Empty;

            if (e.Row < 0 || e.Column < 7)
            {
                return;
            }

            FnSeqNo = e.Column - 6;

            //가동병상수
            nGCnt = (int)VB.Val(SS1.ActiveSheet.Cells[e.Row, 5].Text);

            //잔여병상수
            if ((int)VB.Val(SS1.ActiveSheet.Cells[e.Row, 6].Text) > 0)
            {
                nJCnt = (int)VB.Val(SS1.ActiveSheet.Cells[e.Row, 5].Text);
            }

            nTCnt = nGCnt + nJCnt;

            FstrWard = SS1.ActiveSheet.Cells[e.Row, 15].Text.Trim();
            FnSeqNo = (e.Column - 6) + (int)VB.Val(SS1.ActiveSheet.Cells[e.Row, 25].Text);

            //if (FstrWard != "IU" && FstrWard != "ND")
            //{
            //    if (nTCnt + 7 < e.Column)
            //        return;
            //}

            if (FstrWard == "SUB" || FstrWard == "총" || FstrWard == "") { return; }

            FstrRoom = SS1.ActiveSheet.Cells[e.Row, 16].Text.Trim();
            FstrPano = SS1.ActiveSheet.Cells[e.Row, e.Column + 10].Text.Trim();

            if (e.Row >= 0 && e.Column >= 7 && e.Column <= 14)
            {
                strMsg = VB.Pstr(SS1.ActiveSheet.Cells[e.Row, e.Column].Text.Trim(), "\r\n", 1);

                if (VB.Left(strMsg, 1) == "◆")
                {
                    strMsg = "";
                }
            }
            
            FnCol = e.Column;

            lblInfo.Text = "";

            #region //예약현황 조회
            SQL = "";
            SQL += " SELECT RESERVED,ENTSABUN,TO_CHAR(EntDate,'MM/DD HH24:MI') ENTDATE,GUBUN ";
            SQL += "   FROM " + ComNum.DB_PMPA + "BAS_ROOM_RESERVED ";
            SQL += "  WHERE WARDCODE = '" + FstrWard + "' ";
            SQL += "    AND ROOMCODE = '" + FstrRoom + "' ";
            SQL += "    AND (  PANO = '" + FstrPano + "' OR  SEQNO = '" + FnSeqNo + "' ) ";
            SQL += "    AND DELDATE IS NULL ";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            lblInfo.Text = "등록번호: " + FstrPano + " " + "병동: " + FstrWard + " " + "호실: " + FstrRoom + "   " + strMsg; 

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["Reserved"].ToString().Trim() != "")
                {
                    lblInfo.Text += "▶" + dt.Rows[0]["Reserved"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
            #endregion

        }

        void btnExit_Click(object sender, EventArgs e)
        {
            if (rEventClosed == null)
            {
                if (this.mCallForm == null)
                {
                    this.Close();
                }
                else
                {
                    this.mCallForm.MsgUnloadForm(this);
                }
            }
            else
            {
                rEventClosed(this);
            }
            
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        void GetData()
        {
            DataTable dt = null;

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            int i = 0;
            int j = 0;
            int nGaCnt = 0;
            int nTaCnt = 0;
            int nGaCnt_S = 0;
            int nTaCnt_S = 0;
            int nGaCnt_T = 0;
            int nTaCnt_T = 0;
            int nDept = 0;  //2013-09-05
            int nTotOBed = 0;   //퇴원인원

            int nSexM = 0;
            int nSexW = 0;
            int nSexU = 0;
            int nTResv = 0;

            long nREAD = 0;
            int nRow = 0;
            int nCol = 0;
            int nTemp = 0;

            string strRoom = "";
            string strWard = "";
            string strClass = "";
            string strSex = "";
            string strReserved = "";
            string strSeqno = "";
            string strSpc = "";     //선택구분 ★
            string strTCate = "";   //통합간호간병 ♥

            string[] strDeptName        = new string[33];   //2013-09-05
            string[] strDeptName_Tot    = new string[33];   //2013-09-05
            int[] nDeptInwon            = new int[33];      //2013-09-05
            int[] nDeptInwon_Tot        = new int[33];      //2013-09-05
            int[] nDeptInwon_Dept        = new int[33];      //2013-09-05
            int[] nBi                   = new int[6];       //종별 인원수

            clsIuSentChk clsIuS = new clsIuSentChk();
            clsPmpaFunc cPF     = new clsPmpaFunc();

            Cursor.Current = Cursors.WaitCursor;

            for (i = 0; i < 33; i++)
            {
                strDeptName[i] = "";
                strDeptName_Tot[i] = "";
                nDeptInwon[i] = 0;
                nDeptInwon_Tot[i] = 0;
                nDeptInwon_Dept[i] = 0;
            }

            Spd.Spread_All_Clear(SS1);

            SS1_Sheet1.Rows.Count = 0;
            SS_Sheet1.Rows.Count = 1;
            SS_Sheet1.Rows[0].Height = 24;
            SS_Sheet1.ColumnCount = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            try
            {
                #region SQL Query
                SQL = "";
                SQL += ComNum.VBLF + " SELECT DECODE(A.WARDCODE,'NR','NR','IQ','NR',A.WARDCODE) WARDCODE, A.ROOMCODE,  ";
                SQL += ComNum.VBLF + "        A.ROOMCLASS, A.SEX BEDSEX , A.TBED, B.PANO, B.SNAME, B.SEX, A.GBROOM,    ";
                SQL += ComNum.VBLF + "        B.AGE, B.INDATE, B.OUTDATE, B.DEPTCODE, B.GBSPC, B.ROUTDATE, B.T_CARE,   ";
                SQL += ComNum.VBLF + "        B.BI                                                                     ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ROOM A, (                                      ";
                SQL += ComNum.VBLF + "          SELECT a.WARDCODE,a.ROOMCODE,a.PANO,a.SNAME,a.SEX,a.AGE,a.DEPTCODE,    ";
                SQL += ComNum.VBLF + "                 a.INDATE,TO_CHAR(a.OUTDATE, 'YYYY-MM-DD') OUTDATE,a.GBSPC,      ";
                SQL += ComNum.VBLF + "                 TO_CHAR(b.ROUTDATE,'YYYY-MM-DD') ROUTDATE, a.T_CARE, a.BI, a.DRCODE  ";
                SQL += ComNum.VBLF + "            FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a,                         ";
                SQL += ComNum.VBLF + "                 " + ComNum.DB_PMPA + "NUR_MASTER b                              ";
                SQL += ComNum.VBLF + "           WHERE a.ACTDATE IS NULL                                               ";
                SQL += ComNum.VBLF + "             AND a.GBSTS NOT IN ('1','7','9')                                    ";
                SQL += ComNum.VBLF + "             AND a.Pano <> '81000004'                                            ";
                SQL += ComNum.VBLF + "             AND a.Pano = b.Pano                                                 ";
                SQL += ComNum.VBLF + "             AND a.IPDNO = b.IPDNO                                               ";
                if (chkEr.Checked == true)
                {
                    SQL += ComNum.VBLF + "         AND a.OUTDATE IS NULL                                               ";              
                }
                else
                {
                    SQL += ComNum.VBLF + "         AND ( a.OUTDATE IS NULL OR a.OUTDATE >=TRUNC(SYSDATE) )             ";
                }
                SQL += ComNum.VBLF + "  ) B                                                                            ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1                                                                    ";
                SQL += ComNum.VBLF + "    AND A.TBED > 0                                                               ";
                if (VB.Left(cboDoctor.Text, 4) != "****")
                {
                    SQL += ComNum.VBLF + "    AND B.DRCODE = '" + VB.Left(cboDoctor.Text, 4) + "'                      ";
                }
                
                switch (cboWard.Text)
                {
                    case "SICU":
                        SQL += ComNum.VBLF + "    AND A.WardCode = '33'                                                ";
                        break;
                    case "MICU":
                        SQL += ComNum.VBLF + "    AND A.WardCode = '35'                                                ";
                        break;
                    case "ND":
                        SQL += ComNum.VBLF + "    AND A.WardCode IN ('NR','IQ')                                        ";
                        break;
                    case "전체":
                        SQL += ComNum.VBLF + "    AND A.WardCode > ' '                                                 ";
                        break;
                    case "":
                        SQL += ComNum.VBLF + "    AND A.WardCode > ' '                                                 ";
                        break;
                    default:
                        SQL += ComNum.VBLF + "    AND A.WardCode = '" + cboWard.Text.Trim() + "'                       ";
                        break;
                }
                SQL += ComNum.VBLF + "    AND A.ROOMCODE = B.ROOMCODE(+)                                                ";
                if (chkEr.Checked == true)
                {
                    //SQL += ComNum.VBLF + "  AND (a.WardCode NOT IN ('3A','IQ','NR') OR a.RoomCode ='641' ) ";
                    SQL += ComNum.VBLF + "  AND A.ROOMCODE NOT IN ('564','565','561','557')  ";
                    SQL += ComNum.VBLF + "  AND A.WardCode NOT IN ('2W','3A','IQ','IU','ND','NR','32','33','35','65') ";
                }
                SQL += ComNum.VBLF + "  ORDER BY DECODE(A.WARDCODE,'NR','NR','IQ','NR',A.WARDCODE) , A.ROOMCODE, B.DEPTCODE, B.SNAME ";
                #endregion

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = dt.Rows.Count;

                if (nREAD > 0)
                {
                    for (i = 0; i < nREAD; i++)
                    {
                        //if (dt.Rows[i]["PANO"].ToString().Trim() == "06700436")
                        //{
                        //    ComFunc.MsgBox("");
                        //}

                        if (strWard != dt.Rows[i]["WardCode"].ToString().Trim() || strRoom != dt.Rows[i]["RoomCode"].ToString().Trim())
                        {
                            if (strRoom != dt.Rows[i]["RoomCode"].ToString().Trim())
                            {
                                if (i != 0)
                                {
                                    SS1_Sheet1.Cells[nRow - 1, 5].Text = nGaCnt.ToString();
                                    SS1_Sheet1.Cells[nRow - 1, 6].Text = (nTaCnt - nGaCnt).ToString();
                                    //SS1.Row = nRow;
                                }
                                //잔여병상 점검
                                //예약현황 DIPLPAY
                                if (nTaCnt != nGaCnt)
                                {
                                    for (j = 1; j <= (nTaCnt - nGaCnt); j++)
                                    {
                                        if (nGaCnt == 0 && j == 1)
                                        {
                                            nCol = nCol;
                                        }
                                        else
                                        {
                                            nCol = nCol + 1;
                                        }

                                        nTemp = (int)VB.Val(SS1_Sheet1.Cells[nRow - 1, 25].ToString());

                                        strSeqno = (nCol - 6 + nTemp).ToString();
                                        strReserved = clsIuS.Rtn_Bas_Room_Reserved(clsDB.DbCon, strWard, strRoom, "", strSeqno);

                                        if (strReserved != "")
                                        {
                                            clsSpread.SetTypeAndValue(SS1_Sheet1, nRow - 1, nCol, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_C, ComNum.VBLF + "◆" + strReserved + "◆", true);
                                            //SS1_Sheet1.Cells[nRow - 1, nCol].Text += ComNum.VBLF + "◆" + strReserved + "◆";
                                        }

                                        if (strSex == "M")
                                        {
                                            SS1_Sheet1.Cells[nRow - 1, nCol].BackColor = Color.LightSteelBlue;
                                            nSexM += 1;
                                        }
                                        else if (strSex == "F")
                                        {
                                            SS1_Sheet1.Cells[nRow - 1, nCol].BackColor = Color.Thistle;
                                            nSexW += 1;
                                        }
                                        else
                                        {
                                            SS1_Sheet1.Cells[nRow - 1, nCol].BackColor = Color.NavajoWhite;
                                            nSexU += 1;
                                        }
                                    }
                                }

                                if (i != 0 && strWard != dt.Rows[i]["WARDCODE"].ToString().Trim())
                                {
                                    //SUB 합계
                                    nRow += 1;
                                    if (SS1_Sheet1.Rows.Count < nRow)
                                    {
                                        Spread_Row_Insert(SS1, nRow, 24);
                                    }

                                    SS1_Sheet1.Cells[nRow - 1, 0].Text = "SUB";
                                    SS1_Sheet1.Cells[nRow - 1, 1].Text = "TOT";
                                    SS1_Sheet1.Cells[nRow - 1, 4].Text = nTaCnt_S.ToString();
                                    SS1_Sheet1.Cells[nRow - 1, 5].Text = nGaCnt_S.ToString();
                                    SS1_Sheet1.Cells[nRow - 1, 6].Text = (nTaCnt_S - nGaCnt_S).ToString();

                                    clsSpread.gSpreadLineBoder(SS1, nRow - 1, 0, nRow - 1, 14, Color.Black, 1, false, true, false, true);

                                    nGaCnt_S = 0;
                                    nTaCnt_S = 0;

                                    SS1_Sheet1.Cells[nRow - 1, 7].Text = "";

                                    //2013-09-05
                                    Spd.CellSpan(SS1, nRow - 1, 7, 1, 6);

                                    for (nDept = 0; nDept < 33; nDept++)
                                    {
                                        if (strDeptName[nDept] != "")
                                        {
                                            SS1_Sheet1.Cells[nRow - 1, 7].Text += strDeptName[nDept] + ":" + nDeptInwon[nDept] + "  ";
                                        }
                                    }

                                    for (j = 0; j < 33; j++)
                                    {

                                        for (nDept = 0; nDept < 33; nDept++)
                                        {
                                            if (strDeptName_Tot[nDept] == strDeptName[j])
                                            {
                                                nDeptInwon_Tot[nDept] += nDeptInwon[j];
                                                break;
                                            }
                                            else if (strDeptName_Tot[nDept] == "")
                                            {
                                                strDeptName_Tot[nDept] = strDeptName[j];
                                                nDeptInwon_Tot[nDept] += nDeptInwon[j];
                                                break;
                                            }

                                        }
                                    }

                                    for (nDept = 0; nDept < 33; nDept++)
                                    {
                                        strDeptName[nDept] = "";
                                        nDeptInwon[nDept] = 0;
                                    }
                                }


                                nRow += 1;
                                if (SS1_Sheet1.Rows.Count < nRow)
                                {
                                    //SS1_Sheet1.Rows.Count = nRow;
                                    Spread_Row_Insert(SS1, nRow, 24);
                                }

                                if (dt.Rows[i]["RoomCode"].ToString().Trim() == "565")
                                {
                                    SS1_Sheet1.Cells[nRow - 1, 0].Text = "IQ";
                                }
                                else
                                {
                                    SS1_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                                }
                                
                                SS1_Sheet1.Cells[nRow - 1, 1].Text = cPF.READ_ROOM_SPECIAL(clsDB.DbCon, dt.Rows[i]["RoomCode"].ToString().Trim()) + dt.Rows[i]["RoomCode"].ToString().Trim();

                                //2013-07-03
                                if (dt.Rows[i]["RoomCode"].ToString().Trim() == "637")
                                {
                                    SS1_Sheet1.Cells[nRow - 1, 1].BackColor = Color.FromArgb(85, 255, 170);
                                }

                                SS1_Sheet1.Cells[nRow - 1, 15].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                                SS1_Sheet1.Cells[nRow - 1, 16].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                                SS1_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["RoomClass"].ToString().Trim();
                                SS1_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["BEDSex"].ToString().Trim();

                                //2013-07-25 혼용방일경우 - 최초성별표시
                                if (dt.Rows[i]["BEDSex"].ToString().Trim() == "*" && dt.Rows[i]["WardCode"].ToString().Trim() != "32")
                                {
                                    if (dt.Rows[i]["Sex"].ToString().Trim() != "")
                                    {
                                        SS1_Sheet1.Cells[nRow - 1, 3].Text += "(" + dt.Rows[i]["Sex"].ToString().Trim() + ")";
                                    }
                                }

                                switch (dt.Rows[i]["GBRoom"].ToString().Trim())
                                {
                                    case "11":  //분만대기실
                                        SS1_Sheet1.Cells[nRow - 1, 3].Text = "DR";
                                        nTaCnt = Convert.ToInt32(dt.Rows[i]["Tbed"].ToString().Trim());
                                        break;
                                    case "12":  //분만회복실
                                        SS1_Sheet1.Cells[nRow - 1, 3].Text = "DR";
                                        nTaCnt = Convert.ToInt32(dt.Rows[i]["Tbed"].ToString().Trim());
                                        break;
                                    case "14":  //bassinet
                                        nTaCnt = 0;
                                        SS1_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["BEDSex"].ToString().Trim();
                                        break;
                                    case "55":  //VIP분리방
                                        nTaCnt = 0;
                                        SS1_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["BEDSex"].ToString().Trim();
                                        break;
                                    default:
                                        nTaCnt = Convert.ToInt32(dt.Rows[i]["Tbed"].ToString().Trim());
                                        break;
                                }

                                SS1_Sheet1.Cells[nRow - 1, 4].Text = nTaCnt.ToString();
                            }

                            strWard = dt.Rows[i]["WardCode"].ToString().Trim();
                            strRoom = dt.Rows[i]["RoomCode"].ToString().Trim();
                            strClass = dt.Rows[i]["RoomClass"].ToString().Trim();
                            strSex = dt.Rows[i]["BEDSex"].ToString().Trim();

                            nTaCnt_S += nTaCnt;
                            nTaCnt_T += nTaCnt;

                            nCol = 6;
                            nGaCnt = 0;

                        }

                        if (dt.Rows[i]["PANO"].ToString().Trim() != "")
                        {
                            nGaCnt += 1;
                            nGaCnt_S += 1;
                            nGaCnt_T += 1;
                        }

                        nCol += 1;

                        if (nCol > 14)
                        {
                            nRow += 1;
                            if (SS1_Sheet1.Rows.Count < nRow)
                            {
                                //SS1_Sheet1.Rows.Count = nRow;
                                Spread_Row_Insert(SS1, nRow, 24);
                            }

                            SS1_Sheet1.Cells[nRow - 1, 15].Text = strWard;
                            SS1_Sheet1.Cells[nRow - 1, 16].Text = strRoom;

                            SS1_Sheet1.Cells[nRow - 1, 25].Text = "8";
                            nCol = 7;
                        }

                        nTemp = (int)VB.Val(SS1_Sheet1.Cells[nRow - 1, 25].ToString());

                        strSeqno = (nCol - 6 + nTemp).ToString();
                        strSpc = "";
                        //if (dt.Rows[i]["GbSpc"].ToString().Trim() == "1")
                        //{
                        //    strSpc = "★";
                        //}

                        strTCate = "";
                        if (dt.Rows[i]["T_CARE"].ToString().Trim() == "Y" && (dt.Rows[i]["WardCode"].ToString().Trim() == "40" || dt.Rows[i]["WardCode"].ToString().Trim() == "73" || dt.Rows[i]["WardCode"].ToString().Trim() == "75"))
                        {
                            strTCate = "♥";
                        }

                        if (dt.Rows[i]["Sname"].ToString().Trim() != "")
                        {
                            SS1_Sheet1.Cells[nRow - 1, nCol].Text += dt.Rows[i]["DeptCode"].ToString().Trim() + "," + strSpc + dt.Rows[i]["Sname"].ToString().Trim() + strTCate + "," + dt.Rows[i]["Age"].ToString().Trim();
                        }

                        //당일입원
                        if (VB.Left(dt.Rows[i]["InDate"].ToString().Trim(), 10) == clsPublic.GstrSysDate)
                        {
                            SS1_Sheet1.Cells[nRow - 1, nCol].BackColor = Color.Pink;
                        }

                        //퇴원예고자 2015-01-28
                        if (dt.Rows[i]["ROutDate"].ToString().Trim() == clsPublic.GstrSysDate)
                        {
                            SS1_Sheet1.Cells[nRow - 1, nCol].BackColor = Color.FromArgb(170, 228, 167);
                            nTResv += 1;
                        }

                        //당일퇴원
                        if (dt.Rows[i]["OutDate"].ToString().Trim() == clsPublic.GstrSysDate)
                        {
                            SS1_Sheet1.Cells[nRow - 1, nCol].BackColor = Color.Silver;
                            nTotOBed += 1;
                        }

                        //종별 인원합계
                        switch (dt.Rows[i]["BI"].ToString().Trim())
                        {
                            case "11":
                            case "12":
                            case "13":  nBi[1] += 1; break;
                            case "21":
                            case "22":  nBi[2] += 1; break;
                            case "31":
                            case "33":  nBi[3] += 1; break;
                            case "41":
                            case "42":
                            case "43":
                            case "51":  nBi[4] += 1; break;
                            case "52":
                            case "55":  nBi[5] += 1; break;
                            case  "" :  break;
                            default:
                                nBi[4] += 1; break;
                        }

                      

                        //예약현환 DIPLPAY
                        strReserved = "";
                        strReserved = clsIuS.Rtn_Bas_Room_Reserved(clsDB.DbCon, strWard, strRoom, dt.Rows[i]["pano"].ToString().Trim(), strSeqno);
                        if (strReserved != "")
                        {
                            string strRmk = SS1_Sheet1.Cells[nRow - 1, nCol].Text.Trim();

                            clsSpread.SetTypeAndValue(SS1_Sheet1, nRow - 1, nCol, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_C, ComNum.VBLF + "◆" + strReserved + "◆", true);

                            SS1_Sheet1.Cells[nRow - 1, nCol].Text = strRmk + ComNum.VBLF + SS1_Sheet1.Cells[nRow - 1, nCol].Text.Trim();
                            //SS1_Sheet1.Cells[nRow - 1, nCol].Text += ComNum.VBLF + "◆" + strReserved + "◆";
                        }

                        SS1_Sheet1.Cells[nRow - 1, nCol + 10].Text = dt.Rows[i]["pano"].ToString().Trim();

                        //병동별 과별 인원 통계...   2013-09-05
                        for (nDept = 0; nDept < 33; nDept++)
                        {
                            if (strDeptName[nDept] == dt.Rows[i]["DeptCode"].ToString().Trim())
                            {
                                nDeptInwon[nDept] += 1;
                                break;
                            }
                            else if (strDeptName[nDept] == "")
                            {
                                strDeptName[nDept] = dt.Rows[i]["DeptCode"].ToString().Trim();
                                nDeptInwon[nDept] += 1;
                                break;
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    if (strRoom == "233" || strRoom == "234")
                    {
                        if (nGaCnt > 8) { nRow -= 1; }
                    }

                    SS1_Sheet1.Cells[nRow - 1, 5].Text = nGaCnt.ToString();
                    SS1_Sheet1.Cells[nRow - 1, 6].Text = (nTaCnt - nGaCnt).ToString();

                    if (nTaCnt != nGaCnt)
                    {
                        for (j = 1; j <= (nTaCnt - nGaCnt); j++)
                        {
                            if (nGaCnt == 0 && j == 1) { nCol = nCol; }
                            else { nCol += 1; }

                            nTemp = (int)VB.Val(SS1_Sheet1.Cells[nRow - 1, 25].ToString());
                            strSeqno = (nCol - 7 + nTemp).ToString();

                            //예약현환 DIPLPAY
                            strReserved = "";
                            strReserved = clsIuS.Rtn_Bas_Room_Reserved(clsDB.DbCon, strWard, strRoom, "", strSeqno);
                            if (strReserved != "")
                            {
                                clsSpread.SetTypeAndValue(SS1_Sheet1, nRow - 1, nCol - 1, "TypeText", false, clsSpread.VAlign_C, clsSpread.HAlign_C, ComNum.VBLF + "◆" + strReserved + "◆", true);
                                //SS1_Sheet1.Cells[nRow - 1, nCol - 1].Text += ComNum.VBLF + "◆" + strReserved + "◆";
                            }

                            if (strSex == "M")
                            {
                                SS1_Sheet1.Cells[nRow - 1, nCol - 1].BackColor = Color.LightSteelBlue;
                                nSexM += 1;
                            }
                            else if (strSex == "F")
                            {
                                SS1_Sheet1.Cells[nRow - 1, nCol - 1].BackColor = Color.Thistle;
                                nSexW += 1;
                            }
                            else
                            {
                                SS1_Sheet1.Cells[nRow - 1, nCol - 1].BackColor = Color.NavajoWhite;
                                nSexU += 1;
                            }
                            Application.DoEvents();
                        }
                    }
                }
                
                //SUB TOT
                nRow += 1;
                if (SS1_Sheet1.Rows.Count < nRow)
                {
                    //SS1_Sheet1.Rows.Count = nRow;
                    Spread_Row_Insert(SS1, nRow, 24);
                }

                SS1_Sheet1.Cells[nRow - 1, 0].Text = "SUB";
                SS1_Sheet1.Cells[nRow - 1, 1].Text = "TOT";
                SS1_Sheet1.Cells[nRow - 1, 4].Text = nTaCnt_S.ToString();
                SS1_Sheet1.Cells[nRow - 1, 5].Text = nGaCnt_S.ToString();
                SS1_Sheet1.Cells[nRow - 1, 6].Text = (nTaCnt_S - nGaCnt_S).ToString();

                clsSpread.gSpreadLineBoder(SS1, nRow - 1, 0, nRow - 1, 14, Color.Black, 1, false, true, false, true);

                SS1_Sheet1.Cells[nRow - 1, 7].Text = "  ";
                //2013-09-05
                Spd.CellSpan(SS1, nRow, 7, 1, 6);

                for (nDept = 0; nDept < 33; nDept++)
                {
                    if (strDeptName[nDept] != "")
                    {
                        SS1_Sheet1.Cells[nRow - 1, 7].Text += strDeptName[nDept] + ":" + nDeptInwon[nDept].ToString() + "  ";
                    }
                }

                for (j = 0; j < 33; j++)
                {
                    for (nDept = 0; nDept < 33; nDept++)
                    {
                        if (strDeptName_Tot[nDept] == strDeptName[j])
                        {
                            nDeptInwon_Tot[nDept] = nDeptInwon_Tot[nDept] + nDeptInwon[j];
                            break;
                        }
                        else if (strDeptName_Tot[nDept] == "")
                        {
                            strDeptName_Tot[nDept] = strDeptName[j];
                            nDeptInwon_Tot[nDept] = nDeptInwon_Tot[nDept] + nDeptInwon[j];
                            break;
                        }
                    }
                }

                //TOT TOT
                nRow += 1;
                if (SS1_Sheet1.Rows.Count < nRow)
                {
                    //SS1_Sheet1.Rows.Count = nRow;
                    Spread_Row_Insert(SS1, nRow, 24);
                }

                SS1_Sheet1.Cells[nRow - 1, 0].Text = "총";
                SS1_Sheet1.Cells[nRow - 1, 1].Text = "TOT";
                SS1_Sheet1.Cells[nRow - 1, 4].Text = nTaCnt_T.ToString();
                SS1_Sheet1.Cells[nRow - 1, 5].Text = nGaCnt_T.ToString();
                SS1_Sheet1.Cells[nRow - 1, 6].Text = (nTaCnt_T - nGaCnt_T).ToString();
                SS1_Sheet1.Cells[nRow - 1, 7].Text = "퇴원중 : " + nTotOBed.ToString();

                clsSpread.gSpreadLineBoder(SS1, nRow - 1, 0, nRow - 1, 14, Color.Black, 1, false, true, false, true);

                for (i = 0; i < 33; i++)
                {
                    if (strDeptName_Tot[i] != "")
                    {
                        if (SS_Sheet1.ColumnCount < i + 1)
                        {
                            SS_Sheet1.ColumnCount = i + 1;
                            SS_Sheet1.Columns[i].Width = 40;
                        }
                        SS_Sheet1.ColumnHeader.Cells[0, i].Text = strDeptName_Tot[i];
                        //SS_Sheet1.Cells[0, i].Text = nDeptInwon_Tot[i].ToString();
                        SS_Sheet1.Cells[0, i].Text = eDept_Group(clsDB.DbCon,strDeptName_Tot[i]);
                    }
                }

                Spd.CellAlignMent(SS1, 0, 0, 0, SS_Sheet1.ColumnCount - 1, clsSpread.HAlign_C, clsSpread.VAlign_C);   //셀정렬  

                lblMan.Text     = nSexM.ToString() + "명";
                lblWoman.Text   = nSexW.ToString() + "명";
                lblUSex.Text    = nSexU.ToString() + "명";
                lblNDis.Text    = nTResv.ToString() + "명";
                lblDisIng.Text  = nTotOBed.ToString() + "명";

                lblBohum.Text   = nBi[1].ToString() + "명";
                lblGub.Text     = nBi[2].ToString() + "명";
                lblSan.Text     = nBi[3].ToString() + "명";
                lblTA.Text      = nBi[5].ToString() + "명";
                lblilban.Text   = nBi[4].ToString() + "명";

                clsPublic.GstrHelpCode = "실행";

                Cursor.Current = Cursors.Default;

            }

            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                clsDB.SaveSqlErrLog(e.Message, SQL, clsDB.DbCon);
            }
        }


        string eDept_Group(PsmhDb pDbCon, string strJob)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            string strRowID = string.Empty;
            string strGubun = string.Empty; //'1.이실, 2.예약 구분
            string rtnVal = "";


            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT count(*)  count ";

                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ROOM A, (                                      ";
                SQL += ComNum.VBLF + "          SELECT a.WARDCODE,a.ROOMCODE,a.PANO,a.SNAME,a.SEX,a.AGE,a.DEPTCODE,    ";
                SQL += ComNum.VBLF + "                 a.INDATE,TO_CHAR(a.OUTDATE, 'YYYY-MM-DD') OUTDATE,a.GBSPC,      ";
                SQL += ComNum.VBLF + "                 TO_CHAR(b.ROUTDATE,'YYYY-MM-DD') ROUTDATE, a.T_CARE, a.BI, a.DRCODE  ";
                SQL += ComNum.VBLF + "            FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER a,                         ";
                SQL += ComNum.VBLF + "                 " + ComNum.DB_PMPA + "NUR_MASTER b                              ";
                SQL += ComNum.VBLF + "           WHERE a.ACTDATE IS NULL                                               ";
                SQL += ComNum.VBLF + "             AND a.GBSTS NOT IN ('1','7','9')                                    ";
                SQL += ComNum.VBLF + "             AND a.Pano <> '81000004'                                            ";
                SQL += ComNum.VBLF + "             AND a.Pano = b.Pano                                                 ";
                SQL += ComNum.VBLF + "             AND a.IPDNO = b.IPDNO                                               ";
                if (chkEr.Checked == true)
                {
                    SQL += ComNum.VBLF + "         AND a.OUTDATE IS NULL                                               ";
                }
                else
                {
                    SQL += ComNum.VBLF + "         AND ( a.OUTDATE IS NULL OR a.OUTDATE >=TRUNC(SYSDATE) )             ";
                }
                SQL += ComNum.VBLF + "  ) B                                                                            ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1                                                                    ";
                SQL += ComNum.VBLF + "    AND A.TBED > 0                                                               ";
                if (VB.Left(cboDoctor.Text, 4) != "****")
                {
                    SQL += ComNum.VBLF + "    AND B.DRCODE = '" + VB.Left(cboDoctor.Text, 4) + "'                      ";
                }

                switch (cboWard.Text)
                {
                    case "SICU":
                        SQL += ComNum.VBLF + "    AND A.WardCode = '33'                                                ";
                        break;
                    case "MICU":
                        SQL += ComNum.VBLF + "    AND A.WardCode = '35'                                                ";
                        break;
                    case "ND":
                        SQL += ComNum.VBLF + "    AND A.WardCode IN ('NR','IQ')                                        ";
                        break;
                    case "전체":
                        SQL += ComNum.VBLF + "    AND A.WardCode > ' '                                                 ";
                        break;
                    case "":
                        SQL += ComNum.VBLF + "    AND A.WardCode > ' '                                                 ";
                        break;
                    default:
                        SQL += ComNum.VBLF + "    AND A.WardCode = '" + cboWard.Text.Trim() + "'                       ";
                        break;
                }
                SQL += ComNum.VBLF + "    AND A.ROOMCODE = B.ROOMCODE(+)                                                ";
                SQL += ComNum.VBLF + "    AND B.DEPTCODE = '" + strJob + "'                       ";
                if (chkEr.Checked == true)
                {
                    //SQL += ComNum.VBLF + "  AND (a.WardCode NOT IN ('3A','IQ','NR') OR a.RoomCode ='641' ) ";
                    SQL += ComNum.VBLF + "  AND A.ROOMCODE NOT IN ('564','565','561','557')  ";
                    SQL += ComNum.VBLF + "  AND A.WardCode NOT IN ('2W','3A','IQ','IU','ND','NR','32','33','35','65') ";
                }
                SQL += ComNum.VBLF + "  group BY B.DEPTCODE ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                   
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["count"].ToString().Trim();
                  
                }

                dt.Dispose();
                dt = null;
                return rtnVal;



            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                MessageBox.Show(ex.ToString());
                return rtnVal;
            }
        }


        void Spread_Row_Insert(FpSpread o, int Row, int Height)
        {
            o.Sheets[0].Rows.Count = Row;
            Spd.CellAlignMent(SS1, Row - 1, 0, Row - 1, o.Sheets[0].ColumnCount - 1, clsSpread.HAlign_C, clsSpread.VAlign_T);   //셀정렬         
            SS1_Sheet1.Rows[Row - 1].Height = 38;
        }

        void frmPmpaWardInfo_Load(object sender, EventArgs e)
        {
            Screen_Clear();
        }

        void Screen_Clear()
        {
            lblMan.Text = "0명";
            lblWoman.Text = "0명";
            lblNDis.Text = "0명";
            lblUSex.Text = "0명";
            lblDisIng.Text = "0명";

            lblBohum.Text = "0명";
            lblGub.Text = "0명";
            lblSan.Text = "0명";
            lblTA.Text = "0명";
            lblilban.Text = "0명";

            Spd.Spread_All_Clear(SS1);

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;  //권한확인
            }

            string PrintDate = "";
            string strOK = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            btnPrint.Enabled = false;
            PrintDate = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

            int i = 0;

            //strOK = "OK";
            //for (i = 0; i < SS1_Sheet1.RowCount; i++)
            //{
            //    if (SS1_Sheet1.Cells[i, 15].Text != "")
            //    {
            //        strOK = "NO";
            //    }
            //}

            //if (strOK == "OK")
            //{
            //    SS1_Sheet1.SetColumnVisible(14, false);
            //}

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "병동별 병상 가동현황";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String("출력시간 : " + PrintDate, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 10, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 1f);

            CS.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter, Centering.Horizontal);

            btnPrint.Enabled = true;


            //        Dim strFont1        As String
            //Dim strFont2        As String
            //Dim strFont3        As String
            //Dim strHead1        As String
            //Dim strHead2        As String
            //Dim Headtitle       As String
            //Dim JobDate         As String
            //Dim PrintDate       As String *18
            //Dim JobMan          As String

            //CmdPrint.Enabled = False
            //Call READ_SYSDATE
            //PrintDate = GstrSysDate & " " & GstrSysTime
            //'JobMan = GStrPassName

            //'Print Head 지정
            //strFont1 = "/fn""굴림체"" /fz""16"" /fb1 /fi0 /fu0 /fk0 /fs1"
            //strFont2 = "/fn""굴림체"" /fz""11.25"" /fb0 /fi0 /fu0 /fk0 /fs2"


            //strHead1 = "/f1" & "/f1/n" & "  " & "/f1/n" & Space(30) & "병동별 병상 가동현황" & "/f1/n" & "  " & "/f1/n" & "   "
            //JobDate = GstrSysDate
            //strHead2 = "/l/f2" & "출력시간 : " & PrintDate

            //'Print Body


            //SS1.PrintHeader = strFont1 + strHead1 + "/n" + strFont2 + strHead2 & "/n"
            //SS1.PrintMarginLeft = 1000
            //SS1.PrintMarginRight = 0
            //SS1.PrintMarginTop = 50
            //SS1.PrintMarginBottom = 200
            //SS1.PrintColHeaders = True
            //SS1.PrintRowHeaders = False
            //SS1.PrintBorder = True
            //SS1.PrintColor = False
            //SS1.PrintGrid = False
            //SS1.PrintShadows = False
            //SS1.PrintUseDataMax = False
            //SS1.PrintOrientation = SS_PRINTORIENT_LANDSCAPE
            //SS1.PrintType = SS_PRINT_ALL
            //SS1.Action = SS_ACTION_PRINT

            //CmdPrint.Enabled = True
        }
    }
}

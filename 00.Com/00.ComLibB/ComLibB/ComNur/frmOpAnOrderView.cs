using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;

namespace ComLibB
{
    /// Class Name      : MedOprNr
    /// File Name       : frmOpAnOrderView.cs
    /// Description     : 수술, 마취 처방 조회
    /// Author          : 안정수
    /// Create Date     : 2018-01-08
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 Frm수술마취처방조회.frm(Frm수술마취처방조회) 폼 frmOpAnOrderView.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\Ocs\oproom\opmain\Frm수술마취처방조회.frm(Frm수술마취처방조회) >> frmOpAnOrderView.cs 폼이름 재정의" />
    public partial class frmOpAnOrderView : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = null;
        ComFunc CF = null;

        string FstrWRTNO = "";

        #endregion

        #region MainFormMessage InterFace

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


        public frmOpAnOrderView()
        {
            InitializeComponent();
            setEvent();
        }

        string mstrPtNo = "";
        string mstrOpDate = "";

        public frmOpAnOrderView(string strPtNo, string strOpDate)
        {
            InitializeComponent();

            mstrPtNo = strPtNo;
            mstrOpDate = strOpDate;

            setEvent();
        }

        public frmOpAnOrderView(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnView.Click += new EventHandler(eBtnClick);

            this.txtPano.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.cboDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }
            else
            {
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            }

            CS = new clsSpread();
            CF = new ComFunc();

            SCREEN_CLEAR();

            if (mstrPtNo != "")
            {
                clsPublic.GstrHelpCode = "";
                txtPano.Text = mstrPtNo;
                txtPano_KeyPress();

                if (cboDate.Items.Count > 0)
                {
                    ComFunc.ComboFind(cboDate, "T", 0, mstrOpDate);
                    eGetData();
                }
            }

            if (clsPublic.GstrHelpCode != "")
            {
                txtPano.Text = clsPublic.GstrHelpCode;
                clsPublic.GstrHelpCode = "";
                txtPano_KeyPress();
            }
        }

        void SCREEN_CLEAR()
        {
            txtPano.Text = "";
            cboDate.Items.Clear();
            FstrWRTNO = "";

            CS.Spread_All_Clear(ssList1);
            ssList2.ActiveSheet.Rows.Count = 23;
            CS.Spread_All_Clear(ssList2);
            ssList3.ActiveSheet.Rows.Count = 30;
            CS.Spread_All_Clear(ssList3);
            ssGume.ActiveSheet.Rows.Count = 30;
            CS.Spread_All_Clear(ssGume);

            ComFunc.SetAllControlClear(panel5);
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

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
                return;
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }

                eGetData();
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPano)
            {
                if (e.KeyChar == 13)
                {
                    txtPano_KeyPress();
                }
            }

            else if (sender == this.cboDate)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void txtPano_KeyPress()
        {
            int i = 0;
            int nREAD = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (txtPano.Text.Trim() == "")
            {
                return;
            }

            txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, ComNum.LENPTNO);

            //해당환자의 수술일자를 읽어 ComboDate에 SET
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  TO_CHAR(OpDate,'YYYY-MM-DD') OpDate,WRTNO ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ORAN_MASTER";
            SQL += ComNum.VBLF + "WHERE Pano='" + txtPano.Text + "'";
            SQL += ComNum.VBLF + "ORDER BY OpDate DESC,WRTNO";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                cboDate.Items.Clear();
                FstrWRTNO = "";
                nREAD = dt.Rows.Count;

                for (i = 0; i < nREAD; i++)
                {
                    cboDate.Items.Add(dt.Rows[i]["OpDate"].ToString().Trim());
                    FstrWRTNO += dt.Rows[i]["WRTNO"].ToString().Trim() + ",";
                }
            }

            if (nREAD > 0)
            {
                cboDate.SelectedIndex = 0;
            }

            if (nREAD == 0)
            {
                Screen_display(Convert.ToInt64(VB.Val(VB.Pstr(FstrWRTNO, ",", 1))));
            }

            dt.Dispose();
            dt = null;

            btnView.Focus();
        }

        void eGetData()
        {
            long nWrtno = 0;

            if (cboDate.SelectedIndex >= 0)
            {
                nWrtno = Convert.ToInt64(VB.Val(VB.Pstr(FstrWRTNO, ",", cboDate.SelectedIndex + 1)));
               // SCREEN_CLEAR();
                Screen_display(nWrtno);
            }
        }

        void Screen_display(long argWRTNO)
        {
            int i = 0;
            int nREAD = 0;
            int nRow = 0;
            int nOpTime = 0;

            //string strData = "";
            string strJumin1 = "";
            string strJumin2 = "";
            string strOpTime1 = "";
            string strOpTime2 = "";
            string strCodeGbn = "";
            string strGubun = "";
            string strJepCode = "";
            string strSuCode = "";
            string strBuCode = "";
            string strOldBuCode = "";
            string strGel = "";
            string strSuName = "";

            long nGAmt1 = 0;    //소계
            long nGAmt2 = 0;    //총계
            double nQty = 0;
            double nQty2 = 0;

            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            //수술마스타를 Display
            #region Screen_Master_Display(GoSub)

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  a.WRTNO,TO_CHAR(a.OpDate,'YYYY-MM-DD') OpDate,a.Pano,b.SName,";
            SQL += ComNum.VBLF + "  b.Jumin1,b.Jumin2,a.Age,b.Sex,a.IpdOpd,a.WardCode,a.RoomCode,";
            SQL += ComNum.VBLF + "  a.DeptCode,a.DrCode,a.Bi,a.OpRoom,a.OpDoct1,a.OpDoct2,a.OpCode,";
            SQL += ComNum.VBLF + "  a.OpGubun,a.OpTimeFrom,a.OpTimeTo,a.OpSTime,a.GbNight,a.OpNurse,";
            SQL += ComNum.VBLF + "  a.OpPosition,a.OpRemark,a.Diagnosis,a.OpTitle,a.SpadeWork,a.PtRemark,";
            SQL += ComNum.VBLF + "  a.GbEr,a.OpRe,a.OpBun,a.OpSTime,a.CNurse,OpCancel,AnGbn,AnDoct1,";
            SQL += ComNum.VBLF + "  a.AnTime,a.AnNurse,a.GbNight, A.KIDNEY";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ORAN_MASTER a, " + ComNum.DB_PMPA + "BAS_PATIENT b";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND a.WRTNO=" + argWRTNO + "";
            SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+)";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ssList1.ActiveSheet.Rows.Count = dt.Rows.Count;

                strJumin1 = dt.Rows[0]["Jumin1"].ToString().Trim();
                clsAES.Read_Jumin_AES(clsDB.DbCon, txtPano.Text);
                strJumin2 = clsAES.GstrAesJumin2;

                strOpTime1 = VB.Left(dt.Rows[0]["OpTimeFrom"].ToString().Trim(), 2) + ":" + VB.Right(dt.Rows[0]["OpTimeFrom"].ToString().Trim(), 2);
                strOpTime2 = VB.Left(dt.Rows[0]["OpTimeTo"].ToString().Trim(), 2) + ":" + VB.Right(dt.Rows[0]["OpTimeTo"].ToString().Trim(), 2);
                nOpTime = clsVbfunc.TimeGeSan(strOpTime1 + "-" + strOpTime2);

                //인적사항 Sheet Display
                ssList1.ActiveSheet.Cells[0, 0].Text = txtPano.Text;
                ssList1.ActiveSheet.Cells[0, 1].Text = dt.Rows[0]["Sname"].ToString().Trim();
                ssList1.ActiveSheet.Cells[0, 2].Text = strJumin1 + "-" + strJumin2;
                ssList1.ActiveSheet.Cells[0, 3].Text = dt.Rows[0]["Age"].ToString().Trim();
                ssList1.ActiveSheet.Cells[0, 4].Text = dt.Rows[0]["Sex"].ToString().Trim();
                ssList1.ActiveSheet.Cells[0, 5].Text = dt.Rows[0]["IpdOpd"].ToString().Trim();
                ssList1.ActiveSheet.Cells[0, 6].Text = dt.Rows[0]["Bi"].ToString().Trim();
                ssList1.ActiveSheet.Cells[0, 7].Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                ssList1.ActiveSheet.Cells[0, 8].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[0]["DrCode"].ToString().Trim());

                if (dt.Rows[0]["IpdOpd"].ToString().Trim() == "I")
                {
                    ssList1.ActiveSheet.Cells[0, 9].Text = dt.Rows[0]["WardCode"].ToString().Trim();
                    ssList1.ActiveSheet.Cells[0, 10].Text = dt.Rows[0]["RoomCode"].ToString().Trim();
                }

                txtOpDate.Text = dt.Rows[0]["OpDate"].ToString().Trim();
                txtDrName.Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[0]["DrCode"].ToString().Trim());
                txtOpTime.Text = strOpTime1 + "~" + strOpTime2 + " (" + nOpTime + "분)";
                txtDiagnosis.Text = dt.Rows[0]["Diagnosis"].ToString().Trim();
                txtOpTitle.Text = dt.Rows[0]["OpTitle"].ToString().Trim();
                txtAnDoct.Text = dt.Rows[0]["AnDoct1"].ToString().Trim();

                //수술 Position
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  NAME";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPR_CODE";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND Gubun = '4' ";
                SQL += ComNum.VBLF + "      AND Code = '" + dt.Rows[0]["OpPosition"].ToString().Trim() + "' ";
                SQL += ComNum.VBLF + "ORDER BY Code";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    txtPosName.Text = dt1.Rows[0]["Name"].ToString().Trim();
                }

                dt1.Dispose();
                dt1 = null;

                //수술분류
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  Bun,BunName";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ORAN_OPBUN";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND Dept='" + dt.Rows[0]["DeptCode"].ToString().Trim() + "' ";
                SQL += ComNum.VBLF + "      AND Bun=" + VB.Val(dt.Rows[0]["OpCode"].ToString().Trim()) + "";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    txtOpCode.Text = ComFunc.SetAutoZero(dt.Rows[0]["OpCode"].ToString().Trim(), 3) + ".";
                    txtOpCode.Text += dt1.Rows[i]["BunName"].ToString().Trim();
                }

                dt1.Dispose();
                dt1 = null;

                //야간,공휴가산
                switch (dt.Rows[0]["GbNight"].ToString().Trim())
                {
                    case "0":
                        txtGbNight.Text = "평일";
                        break;

                    case "1":
                        txtGbNight.Text = "공휴";
                        break;

                    case "2":
                        txtGbNight.Text = "야간";
                        break;

                    default:
                        txtGbNight.Text = "";
                        break;
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  Name";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPR_CODE";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND Gubun= '3'";
                SQL += ComNum.VBLF + "      AND CODE ='" + dt.Rows[0]["AnGbn"].ToString().Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    txtAnes.Text = dt1.Rows[0]["Name"].ToString().Trim();
                }

                dt1.Dispose();
                dt1 = null;

                //수술구분
                txtOpBun.Text = CF.Read_Bcode_Name(clsDB.DbCon, "ORAN_수술구분", dt.Rows[0]["OpBun"].ToString().Trim());
            }
            dt.Dispose();
            dt = null;

            #endregion

            nRow = 0;

            //마취,수술료
            //수술처방 내역 Display(수술료,마취료)
            #region Screen_ORAN_Slip1(GoSub)

            ssList2.ActiveSheet.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  BuCode,TO_CHAR(OpDate,'YYYY-MM-DD') OpDate,DeptCode,IpdOpd,";
            SQL += ComNum.VBLF + "  CodeGbn,GuBun,JepCode,SuCode,SuBun,GbSelf,GbSunap,GbSusul,GbNgt,";
            SQL += ComNum.VBLF + "  GbSunap,OpRoom,SUM(Qty) Qty";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ORAN_SLIP";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND WRTNO='" + argWRTNO + "'";
            SQL += ComNum.VBLF + "      AND SuBun IN ('22','34')";
            SQL += ComNum.VBLF + "GROUP BY BuCode,OpDate,DeptCode,IpdOpd,CodeGbn,GuBun,JepCode,";
            SQL += ComNum.VBLF + "         SuCode,SuBun,GbSelf,GbSunap,GbSusul,GbNgt,GbSunap,OpRoom";
            SQL += ComNum.VBLF + "ORDER BY BuCode,Gubun,JepCode";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;
                nRow = 0;
                strOldBuCode = "";

                for (i = 0; i < nREAD; i++)
                {
                    if (VB.Val(dt.Rows[i]["Qty"].ToString().Trim()) != 0)
                    {
                        strBuCode = dt.Rows[i]["BuCode"].ToString().Trim();
                        strCodeGbn = dt.Rows[i]["CodeGbn"].ToString().Trim();
                        strGubun = dt.Rows[i]["Gubun"].ToString().Trim();
                        strJepCode = dt.Rows[i]["JepCode"].ToString().Trim();
                        strSuCode = dt.Rows[i]["SuCode"].ToString().Trim();

                        nRow += 1;

                        ssList2.ActiveSheet.Rows.Count = nRow;
                        ssList2.ActiveSheet.Cells[nRow - 1, 0].Text = "";

                        if (strBuCode != strOldBuCode)
                        {
                            strOldBuCode = strBuCode;
                            if (strBuCode == "033102")
                            {
                                ssList2.ActiveSheet.Cells[nRow - 1, 0].Text = "수술";
                            }

                            else
                            {
                                ssList2.ActiveSheet.Cells[nRow - 1, 0].Text = "마취";
                            }
                        }

                        ssList2.ActiveSheet.Rows[nRow - 1].Height = 20;
                        ssList2.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                        ssList2.ActiveSheet.Cells[nRow - 1, 2].Text = strJepCode;
                        ssList2.ActiveSheet.Cells[nRow - 1, 3].Text = dt.Rows[i]["Qty"].ToString().Trim();
                        ssList2.ActiveSheet.Cells[nRow - 1, 4].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                        ssList2.ActiveSheet.Cells[nRow - 1, 5].Text = dt.Rows[i]["GbSunap"].ToString().Trim();
                        ssList2.ActiveSheet.Cells[nRow - 1, 8].Text = dt.Rows[i]["GbSusul"].ToString().Trim();

                        if (dt.Rows[i]["GbNgt"].ToString().Trim() == "1")
                        {
                            ssList2.ActiveSheet.Cells[nRow - 1, 10].Text = "휴일";
                        }

                        else if (dt.Rows[i]["GbNgt"].ToString().Trim() == "2")
                        {
                            ssList2.ActiveSheet.Cells[nRow - 1, 10].Text = "야간";
                        }

                        else if (dt.Rows[i]["GbNgt"].ToString().Trim() == "3")
                        {
                            ssList2.ActiveSheet.Cells[nRow - 1, 10].Text = "심야";
                        }

                        //관리과 물품코드를 읽음
                        if (strCodeGbn != "1")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT ";
                            SQL += ComNum.VBLF + "  JepName,Buse_Unit,GbExchange";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "ORD_JEP";
                            SQL += ComNum.VBLF + "WHERE JepCode='" + strJepCode + "' ";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                ssList2.ActiveSheet.Cells[nRow - 1, 6].Text = dt1.Rows[0]["Buse_Unit"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow - 1, 7].Text = dt1.Rows[0]["JepName"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow - 1, 11].Text = "일반";

                                if (dt1.Rows[0]["GbExchange"].ToString().Trim() == "1")
                                {
                                    ssList2.ActiveSheet.Cells[nRow - 1, 11].Text = "교환";
                                }

                                if (strGubun == "94")
                                {
                                    ssList2.ActiveSheet.Cells[nRow - 1, 11].Text = "대여";
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        //수가정보를 읽음
                        if (strSuCode != "")
                        {
                            //수가코드가 있는지 Check
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT ";
                            SQL += ComNum.VBLF + "  a.SugbF,a.Bun,b.Unit,b.SuNameK,b.HCode,b.SugbN,";
                            SQL += ComNum.VBLF + "  TO_CHAR(a.DelDate,'YYYY-MM-DD') DelDate";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUT a, " + ComNum.DB_PMPA + "BAS_SUN b";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "      AND a.SuCode='" + strSuCode + "'";
                            SQL += ComNum.VBLF + "      AND a.SuNext=b.SuNext(+)";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                if (ssList2.ActiveSheet.Cells[nRow - 1, 5].Text == "")
                                {
                                    ssList2.ActiveSheet.Cells[nRow - 1, 5].Text = dt1.Rows[0]["SugbN"].ToString().Trim();
                                }

                                if (strCodeGbn == "1")
                                {

                                    strSuName = dt1.Rows[0]["SuNameK"].ToString().Trim();
                                    if (dt1.Rows[0]["HCode"].ToString().Trim() != "")
                                    {
                                        strSuName += "[" + dt1.Rows[0]["HCode"].ToString().Trim() + "]";
                                    }

                                    ssList2.ActiveSheet.Cells[nRow - 1, 6].Text = dt1.Rows[0]["Unit"].ToString().Trim();
                                    ssList2.ActiveSheet.Cells[nRow - 1, 7].Text = strSuName;
                                }

                                ssList2.ActiveSheet.Cells[nRow - 1, 9].Text = dt1.Rows[0]["SugbF"].ToString().Trim();
                                if (ssList2.ActiveSheet.Cells[nRow - 1, 11].Text == "")
                                {
                                    ssList2.ActiveSheet.Cells[nRow - 1, 11].Text = "수가";
                                }
                            }

                            else
                            {
                                ssList2.ActiveSheet.Cells[nRow - 1, 1].Text = strSuCode;
                                ssList2.ActiveSheet.Cells[nRow - 1, 11].Text = "오류";
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        else
                        {
                            ssList2.ActiveSheet.Cells[nRow - 1, 9].Text = "";
                        }

                    }


                }

                dt.Dispose();
                dt = null;
            }

            #endregion

            //마취,수술료외 기타
            //수술처방 내역 Display(수술.마취료 제외)
            #region Screen_ORAN_Slip2(GoSub)

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  BuCode,TO_CHAR(OpDate,'YYYY-MM-DD') OpDate,DeptCode,IpdOpd,";
            SQL += ComNum.VBLF + "  CodeGbn,GuBun,JepCode,SuCode,SuBun,GbSelf,GbSunap,GbSusul,GbNgt,";
            SQL += ComNum.VBLF + "  GbSunap,OpRoom,SUM(Qty) Qty";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ORAN_SLIP";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND WRTNO = '" + argWRTNO + "' ";
            SQL += ComNum.VBLF + "      AND (SuBun NOT IN ('22','34') OR SuBun IS NULL)";
            SQL += ComNum.VBLF + "GROUP BY BuCode,OpDate,DeptCode,IpdOpd,CodeGbn,GuBun,JepCode,";
            SQL += ComNum.VBLF + "         SuCode,SuBun,GbSelf,GbSunap,GbSusul,GbNgt,GbSunap,OpRoom";
            SQL += ComNum.VBLF + "ORDER BY BuCode,SuBun,Gubun,JepCode";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            //if(dt.Rows.Count == 0)
            //{
            //    dt.Dispose();
            //    dt = null;
            //    return;
            //}

            if (dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;

                strOldBuCode = "";

                for (i = 0; i < nREAD; i++)
                {
                    if (VB.Val(dt.Rows[i]["Qty"].ToString().Trim()) != 0)
                    {
                        strBuCode = dt.Rows[i]["BuCode"].ToString().Trim();
                        strCodeGbn = dt.Rows[i]["CodeGbn"].ToString().Trim();
                        strGubun = dt.Rows[i]["Gubun"].ToString().Trim();
                        strJepCode = dt.Rows[i]["JepCode"].ToString().Trim();
                        strSuCode = dt.Rows[i]["SuCode"].ToString().Trim();

                        nRow += 1;
                        ssList2.ActiveSheet.Rows.Count = nRow;


                        ssList2.ActiveSheet.Cells[nRow - 1, 0].Text = "";

                        if (strBuCode != strOldBuCode)
                        {
                            strOldBuCode = strBuCode;
                            if (strBuCode == "033102")
                            {
                                ssList2.ActiveSheet.Cells[nRow - 1, 0].Text = "수술";
                            }

                            else
                            {
                                ssList2.ActiveSheet.Cells[nRow - 1, 0].Text = "마취";
                            }
                        }

                        ssList2.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["SuCode"].ToString().Trim();
                        ssList2.ActiveSheet.Cells[nRow - 1, 2].Text = strJepCode;
                        ssList2.ActiveSheet.Cells[nRow - 1, 3].Text = dt.Rows[i]["Qty"].ToString().Trim();
                        ssList2.ActiveSheet.Cells[nRow - 1, 4].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                        ssList2.ActiveSheet.Cells[nRow - 1, 5].Text = dt.Rows[i]["GbSunap"].ToString().Trim();
                        ssList2.ActiveSheet.Cells[nRow - 1, 8].Text = dt.Rows[i]["GbSusul"].ToString().Trim();

                        if (dt.Rows[i]["GbNgt"].ToString().Trim() == "1")
                        {
                            ssList2.ActiveSheet.Cells[nRow - 1, 10].Text = "휴일";
                        }

                        else if (dt.Rows[i]["GbNgt"].ToString().Trim() == "2")
                        {
                            ssList2.ActiveSheet.Cells[nRow - 1, 10].Text = "야간";
                        }

                        else if (dt.Rows[i]["GbNgt"].ToString().Trim() == "3")
                        {
                            ssList2.ActiveSheet.Cells[nRow - 1, 10].Text = "심야";
                        }

                        //관리과 물품코드를 읽음
                        if (strCodeGbn != "1")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT ";
                            SQL += ComNum.VBLF + "  JepName,Buse_Unit,GbExchange";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "ORD_JEP";
                            SQL += ComNum.VBLF + "WHERE JepCode='" + strJepCode + "'";


                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                ssList2.ActiveSheet.Cells[nRow - 1, 6].Text = dt1.Rows[0]["Buse_Unit"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow - 1, 7].Text = dt1.Rows[0]["JepName"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow - 1, 11].Text = "일반";

                                if (dt1.Rows[0]["GbExchange"].ToString().Trim() == "1")
                                {
                                    ssList2.ActiveSheet.Cells[nRow - 1, 11].Text = "교환";
                                }

                                if (strGubun == "94")
                                {
                                    ssList2.ActiveSheet.Cells[nRow - 1, 11].Text = "대여";
                                }
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        //수가정보를 읽음
                        if (strSuCode != "")
                        {
                            //수가코드가 있는지 Check
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT ";
                            SQL += ComNum.VBLF + "  a.SugbF,a.Bun,b.Unit,b.SuNameK,b.HCode,b.SugbN,";
                            SQL += ComNum.VBLF + "  TO_CHAR(a.DelDate,'YYYY-MM-DD') DelDate";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUT a, " + ComNum.DB_PMPA + "BAS_SUN b";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "      AND a.SuCode='" + strSuCode + "'";
                            SQL += ComNum.VBLF + "      AND a.SuNext=b.SuNext(+)";

                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                if (ssList2.ActiveSheet.Cells[nRow - 1, 5].Text == "")
                                {
                                    ssList2.ActiveSheet.Cells[nRow - 1, 5].Text = dt1.Rows[0]["SugbN"].ToString().Trim();
                                }

                                if (strCodeGbn == "1")
                                {
                                    strSuName = dt1.Rows[0]["SuNameK"].ToString().Trim();

                                    if (dt1.Rows[0]["HCode"].ToString().Trim() != "")
                                    {
                                        strSuName += "[" + dt1.Rows[0]["HCode"].ToString().Trim() + "]";
                                    }

                                    ssList2.ActiveSheet.Cells[nRow - 1, 6].Text = dt1.Rows[0]["Unit"].ToString().Trim();
                                    ssList2.ActiveSheet.Cells[nRow - 1, 7].Text = strSuName;
                                }

                                ssList2.ActiveSheet.Cells[nRow - 1, 9].Text = dt1.Rows[0]["SugbF"].ToString().Trim();

                                if (ssList2.ActiveSheet.Cells[nRow - 1, 11].Text == "")
                                {
                                    ssList2.ActiveSheet.Cells[nRow - 1, 11].Text = "수가";
                                }
                            }

                            else
                            {
                                ssList2.ActiveSheet.Cells[nRow - 1, 1].Text = strSuCode;
                                ssList2.ActiveSheet.Cells[nRow - 1, 11].Text = "오류";
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        else
                        {
                            ssList2.ActiveSheet.Cells[nRow - 1, 9].Text = "";
                        }
                    }

                    ssList2.ActiveSheet.Rows[nRow - 1].Height = 20;
                }
            }

            dt.Dispose();
            dt = null;

            #endregion

            //재원처방 내역
            //재원처방을 Display
            #region Screen_IPD_Slip(GoSub)

            int nNu = 0;
            int nNuChk = 0;
            string strNujuk = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  a.Sucode, a.Sunext, b.SunameK, a.BaseAmt, a.Qty,";
            SQL += ComNum.VBLF + "  a.GbSpc, a.GbNgt, a.GbGisul, a.GbSelf, a.GbChild, a.Nu, a.Bun,";
            SQL += ComNum.VBLF + "  SUM(a.Nal) Nal, SUM(a.Amt1+a.Amt2) Amt";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_SLIP a, " + ComNum.DB_PMPA + "BAS_SUN b";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND a.Pano='" + txtPano.Text + "'";
            SQL += ComNum.VBLF + "      AND a.BDate=TO_DATE('" + txtOpDate.Text + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND a.Sunext = b.Sunext(+)";
            SQL += ComNum.VBLF + "GROUP BY a.Sucode, a.Sunext, b.SunameK, a.BaseAmt, a.Qty,";
            SQL += ComNum.VBLF + "         a.GbSpc,  a.GbNgt,  a.GbGisul, a.GbSelf,  a.GbChild, a.Nu, a.Bun";
            SQL += ComNum.VBLF + "ORDER BY a.Nu,a.Sucode,a.SuNext";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;
                nRow = 0;
                //nOldNu = 0;

                for (i = 0; i < nREAD; i++)
                {
                    if (VB.Val(dt.Rows[i]["Nal"].ToString().Trim()) != 0)
                    {
                        if (VB.Val(dt.Rows[i]["Nu"].ToString().Trim()) != 0)
                        {
                            nNu = Convert.ToInt32(VB.Val(dt.Rows[i]["Nu"].ToString().Trim()));
                        }

                        if (nNuChk == 0)
                        {
                            nNuChk = nNu;
                            strNujuk = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_누적행위명", ComFunc.SetAutoZero(nNu.ToString(), 2));
                        }

                        if (nNu != nNuChk)
                        {
                            nNuChk = nNu;
                            strNujuk = CF.Read_Bcode_Name(clsDB.DbCon, "BAS_누적행위명", ComFunc.SetAutoZero(nNu.ToString(), 2));
                        }

                        nRow += 1;

                        if (nRow > ssList3.ActiveSheet.Rows.Count)
                        {
                            ssList3.ActiveSheet.Rows.Count = nRow;
                        }

                        ssList3.ActiveSheet.Cells[nRow - 1, 0].Text = strNujuk;
                        ssList3.ActiveSheet.Cells[nRow - 1, 1].Text = " " + dt.Rows[i]["SuNext"].ToString().Trim();
                        ssList3.ActiveSheet.Cells[nRow - 1, 2].Text = " " + dt.Rows[i]["SuNameK"].ToString().Trim();
                        ssList3.ActiveSheet.Cells[nRow - 1, 3].Text = String.Format("{0:#,##0}", VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()));
                        ssList3.ActiveSheet.Cells[nRow - 1, 4].Text = String.Format("{0:#,##0.0}", VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                        ssList3.ActiveSheet.Cells[nRow - 1, 5].Text = String.Format("{0:#,##0}", VB.Val(dt.Rows[i]["Nal"].ToString().Trim()));
                        ssList3.ActiveSheet.Cells[nRow - 1, 6].Text = String.Format("{0:#,##0}", VB.Val(dt.Rows[i]["Amt"].ToString().Trim()));
                        ssList3.ActiveSheet.Cells[nRow - 1, 7].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                        ssList3.ActiveSheet.Cells[nRow - 1, 8].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                        ssList3.ActiveSheet.Cells[nRow - 1, 9].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                        ssList3.ActiveSheet.Cells[nRow - 1, 10].Text = dt.Rows[i]["GbChild"].ToString().Trim();

                        ssList3.ActiveSheet.Rows[nRow - 1].Height = 18;
                        strNujuk = "";
                    }
                }
            }

            dt.Dispose();
            dt = null;

            ssList3.ActiveSheet.Rows.Count = nRow;

            #endregion

            //구매과 수술재료대 구입내역
            #region Screen_Gume(GoSub)

            ssGume.ActiveSheet.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  A.SUSUL, A.GELCODE,  A.JEPCODE, C.JEPNAME, A.DEPTCODE, A.AMT, ";
            SQL += ComNum.VBLF + "  A.INDATE,  B.NAME, C.PRICE,  A.INDATE, A.AMT, A.*";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "ORD_SUSUL1 A, " + ComNum.DB_ERP + "AIS_LTD B, " + ComNum.DB_ERP + "ORD_JEP C";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND INDATE =TO_DATE('" + cboDate.SelectedItem.ToString() + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND PANO ='" + txtPano.Text + "'";
            SQL += ComNum.VBLF + "      AND A.GELCODE = B.LTDCODE";
            SQL += ComNum.VBLF + "      AND A.JEPCODE = C.JEPCODE";
            SQL += ComNum.VBLF + "ORDER BY A.SUSUL";

            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt2.Rows.Count > 0)
            {                
                ssGume.ActiveSheet.Rows.Count = dt2.Rows.Count;
                nRow = 0;
                strGel = "";
                nGAmt1 = 0;
                nGAmt2 = 0;
                nQty = 0;
                nQty2 = 0;

                for (i = 0; i < dt2.Rows.Count; i++)
                {
                    if (dt2.Rows[i]["GelCode"].ToString().Trim() != strGel)
                    {
                        if (strGel != "")
                        {
                            nRow += 1;

                            if (ssGume.ActiveSheet.Rows.Count < nRow)
                            {
                                ssGume.ActiveSheet.Rows.Count = nRow;
                            }

                            ssGume.ActiveSheet.Cells[nRow - 1, 0].Text = "소계";
                            ssGume.ActiveSheet.Cells[nRow - 1, 2].Text = nQty.ToString();
                            ssGume.ActiveSheet.Cells[nRow - 1, 3].Text = nGAmt1.ToString();

                            ssGume.ActiveSheet.Rows[nRow - 1].BackColor = Color.LightBlue;
                        }

                        nGAmt1 = 0;
                        nQty = 0;
                        strGel = dt2.Rows[i]["GelCode"].ToString().Trim();
                    }

                    nRow += 1;

                    if (ssGume.ActiveSheet.Rows.Count < nRow)
                    {
                        ssGume.ActiveSheet.Rows.Count = nRow;
                    }

                    ssGume.ActiveSheet.Cells[nRow - 1, 0].Text = dt2.Rows[i]["Name"].ToString().Trim();
                    ssGume.ActiveSheet.Cells[nRow - 1, 1].Text = dt2.Rows[i]["DeptCode"].ToString().Trim();
                    ssGume.ActiveSheet.Cells[nRow - 1, 2].Text = dt2.Rows[i]["Qty"].ToString().Trim();
                    nQty += VB.Val(dt2.Rows[i]["Qty"].ToString().Trim());
                    nQty2 += VB.Val(dt2.Rows[i]["Qty"].ToString().Trim());

                    ssGume.ActiveSheet.Cells[nRow - 1, 3].Text = dt2.Rows[i]["Amt"].ToString().Trim();
                    nGAmt1 += Convert.ToInt64(VB.Val(dt2.Rows[i]["Amt"].ToString().Trim()));
                    nGAmt2 += Convert.ToInt64(VB.Val(dt2.Rows[i]["Amt"].ToString().Trim()));

                    ssGume.ActiveSheet.Cells[nRow - 1, 4].Text = VB.Left(dt2.Rows[i]["InDate"].ToString().Trim(), 10);
                    ssGume.ActiveSheet.Cells[nRow - 1, 5].Text = dt2.Rows[i]["JepCode"].ToString().Trim();
                }

                nRow += 1;

                if (ssGume.ActiveSheet.Rows.Count < nRow)
                {
                    ssGume.ActiveSheet.Rows.Count = nRow;
                }

                ssGume.ActiveSheet.Cells[nRow - 1, 0].Text = "소계";
                ssGume.ActiveSheet.Cells[nRow - 1, 2].Text = nQty.ToString();
                ssGume.ActiveSheet.Cells[nRow - 1, 3].Text = nGAmt1.ToString();
                ssGume.ActiveSheet.Rows[nRow - 1].BackColor = Color.LightBlue;

                nRow += 1;

                if (ssGume.ActiveSheet.Rows.Count < nRow)
                {
                    ssGume.ActiveSheet.Rows.Count = nRow;
                }

                ssGume.ActiveSheet.Cells[nRow - 1, 0].Text = "총계";
                ssGume.ActiveSheet.Cells[nRow - 1, 2].Text = nQty2.ToString();
                ssGume.ActiveSheet.Cells[nRow - 1, 3].Text = nGAmt2.ToString();
                ssGume.ActiveSheet.Rows[nRow - 1].BackColor = Color.LightBlue;

                dt2.Dispose();
                dt2 = null;
            }
            #endregion





        }
    }
}

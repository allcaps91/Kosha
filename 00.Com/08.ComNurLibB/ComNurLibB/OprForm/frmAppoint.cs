using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase;
using System.Drawing.Printing;
using ComDbB;
using ComLibB;
using ComBase.Mvc;
using ComBase.Controls;

namespace ComNurLibB
{
    public partial class frmAppoint : Form
    {
        string strPtNo = "";
        string strSname = "";
        string strJumin = "";
        string strWardCode = "";
        string strRoomCode = "";
        string FstrDeptCode = "";
        string strDrCode = "";
        string strAnDrCode = "";
        string strInDate = "";
        //string strSite = "";

        //string SaveFlag = "";
        //string FindFlag = "";
        //string FstrJob = "";
        string FstrDrDept = "";

        int oldOpSeq = 0;
        int newOpSeq = 0;
        long FnWRTNO = 0;
        string FstrName = "";
        string FstrSex = "";
        int FnAge = 0;

        clsSpread SP = new clsSpread();
        clsQuery Query = null;

        DateTime NowDate;

        public frmAppoint()
        {
            InitializeComponent();
        }

        string mstrRetValue = "";
        string mstrEXEName = "";
        string mstrABO = "";

        //2019-02-13
        string gPart = "";
        string gGSROWID = "";
        string SQL = "";
        //string SqlErr = ""; //에러문 받는 변수

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strRetValue">gstrRetValue</param>
        /// <param name="strEXEName">EXEName</param>
        public frmAppoint(string strRetValue, string strEXEName)
        {
            InitializeComponent();

            mstrRetValue = strRetValue;
            mstrEXEName = strEXEName;
        }
        
        private void frmAppoint_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            } //폼 권한 조회

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsOpMain.SetGstrBuCode(clsDB.DbCon);
            if (clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "XRAY_ANGIO", Convert.ToString(Convert.ToInt32(clsType.User.Sabun))) != "")
            {
                clsOpMain.GstrBuCode = "100570"; //'혈관조영실
            }

            if (clsOpMain.GstrBuCode == clsOpMain.BUSE_MARCH || clsOpMain.GstrBuCode == clsOpMain.BUSE_SUSUL)
            {
                dtpOpDate1.Visible = true;
            }
            else
            {
                dtpOpDate1.Visible = false;
            }
            
            panHelp.Dock = DockStyle.Fill;

            Query = new clsQuery();
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            NowDate = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D"));

            txtSName.Location = cboViewDept.Location;

            txtSName.Text = "";
            Combo_Additem();

            lblView.Text = "과";
            cboViewDept.Visible = true;
            txtSName.Text = "";
            txtSName.Visible = false;

            if (clsOpMain.GstrBuCode == clsOpMain.BUSE_SUSUL
             || clsOpMain.GstrBuCode == clsOpMain.BUSE_MARCH)
            {
                ssView2_Sheet1.Columns.Get(7).Visible = true; //'등록번호
                ssView2_Sheet1.Columns.Get(9).Visible = true; //'마취방법
                ssView2_Sheet1.Columns.Get(10).Visible = true; //'마취의사
                dtpOpDate.Value = NowDate;
                //this.Width = 1009;
                this.Width = 1109;
            }
            else
            {
                ssView2_Sheet1.Columns.Get(7).Visible = false; //'등록번호
                ssView2_Sheet1.Columns.Get(9).Visible = false; //'마취방법
                ssView2_Sheet1.Columns.Get(10).Visible = false;//'마취의사
                dtpOpDate.Value = NowDate.AddDays(1);
                //this.Width = 821;
                this.Width = 921;
            }

            ssView2_Sheet1.Columns.Get(8).Visible = false; //'WRTNO
            ssView2_Sheet1.Columns.Get(11).Visible = false; //'ROWID
            dtpOpDate1.Value = NowDate.AddDays(1);

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //'작업자 사번으로 수술과 선택
                SQL = "SELECT DEPTCODE FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE SABUN=" + clsType.User.Sabun + " ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                FstrDrDept = "";

                if (dt.Rows.Count > 0)
                {
                    FstrDrDept = dt.Rows[0]["DEPTCODE"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message); 
            }


            //'의사이면 해당과만 등록/수정이 가능함
            cboViewDept.SelectedIndex = 0;

            if (FstrDrDept != "")
            {
                ComFunc.ComboFind(cboDeptCode, "T", 0, FstrDrDept);
                cboDeptCode.Enabled = false;
                cboViewDept.SelectedIndex = cboDeptCode.SelectedIndex + 1;
            }

            SCREEN_CLEAR();
            btnSearch_Click(btnSearch, new EventArgs());

            if (clsType.User.Sabun == "37379")
            {
                ComFunc.ComboFind(cboRDept, "T", 0, "OT");
            }

       

            //메뉴사용권한 체크  2019-02-13           
            dt = Query.Get_BasBcode(clsDB.DbCon, "C#_수술스케쥴_외과설정", clsType.User.IdNumber, "", "", "");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                gPart = "OK";
                if (dt.Rows[0]["Name"].ToString().Trim() !="")
                {
                    cboDeptCode.Text  = dt.Rows[0]["Name"].ToString().Trim();
                }
                chkGS.Visible = true;

                FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

                unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "GS", false);
                unary.BackColor = Color.LightGreen;
                unary.ForeColor = Color.Black;
                ssView2.ActiveSheet.SetConditionalFormatting(-1,1, unary);  //

            }
            dt.Dispose();
            dt = null;


            lblAnti.Text = ""; //'항혈전제 표시

            //'2014-03-24 ocs연동
            if (mstrRetValue.Length == 8 && (mstrEXEName.ToUpper() == "MTSOORDER" || mstrEXEName.ToUpper() == "MTSIORDER"))
            {
                //FstrJob = "신규";
                if (clsOpMain.GstrBuCode == clsOpMain.BUSE_SUSUL
                || clsOpMain.GstrBuCode == clsOpMain.BUSE_MARCH)
                {
                    dtpOpDate.Value = NowDate;
                }
                else
                {
                    dtpOpDate.Value = NowDate.AddDays(1);
                }

                btnSave.Enabled = true;
                btnDelete.Enabled = true;
                txtPtno.Enabled = true;

                FnWRTNO = 0;

                txtPtno.Text = mstrRetValue;

                txtPtnoSearch();
            }

            txtSName.ImeMode = ImeMode.Hangul;

            cboOpRoom1.SelectedIndex = 0;

            #region GS 과장일경우 과 GS 선택 자동으로 되게.
            if (clsType.User.DeptCode.Equals("GS"))
            {
                ComFunc.ComboFind(cboRDept, "T", 0, "GS");
            }
            //else if (clsType.User.DeptCode.Equals("OT"))
            //{
            //    ComFunc.ComboFind(cboRDept, "T", 0, "OT");
            //}
          

            Set_Op_Dept();
            #endregion

            SP.setSpdFilter(ssView2, 0, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            SP.setSpdFilter(ssView2, 4, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);                    
        }

        private void Set_Op_Dept()
        {
            if (clsType.User.DrCode.NotEmpty())
                return;

            MParameter mParameter = new MParameter();
            mParameter.AppendSql("SELECT VALUEV                                                         ");
            mParameter.AppendSql("  FROM KOSMOS_PMPA.BAS_PCCONFIG                                       ");
            mParameter.AppendSql(" WHERE GUBUN      = '외래간호'                                         ");   
            mParameter.AppendSql("   AND CODE       = 'OPDWAIT_DEPT'                                    ");
            mParameter.AppendSql("   AND IPADDRESS  = '" + clsCompuInfo.gstrCOMIP  + "'                 ");

            string DEPTCODE = clsDB.ExecuteScalar<string>(mParameter, clsDB.DbCon);
            if (DEPTCODE.NotEmpty())
            {
                if (DEPTCODE.IndexOf(",") != -1)
                {
                    DEPTCODE = DEPTCODE.Split(',')[0];
                }
                ComFunc.ComboFind(cboRDept, "T", 0, DEPTCODE);
                ComFunc.ComboFind(cboDeptCode, "T", 0, DEPTCODE);
            }
            else
            {
                cboRDept.SelectedIndex = 0;
            }
        }


        private void READ_ANTI(string argPano)
        {

            lblAnti.Text = "";
            lblAnti.BackColor = Color.FromArgb(236, 233, 216);

            lblAnti.Text = clsOrderEtc.READ_ANTIBLOOD_CHK(clsDB.DbCon, argPano);

            if (lblAnti.Text != "")
            {
                lblAnti.BackColor = Color.FromArgb(255, 0, 255);
            }
        }

        private void SCREEN_CLEAR()
        {
            txtPtno.Text = "";
            lblPtData.Text = "";
            txtPtno.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = true;

            cboGbio.SelectedIndex = -1;
            cboWardCode.SelectedIndex = -1;
            cboRoomCode.SelectedIndex = -1;
            cboOpStaff.SelectedIndex = -1;
            cboOpRoom.SelectedIndex = -1;
            cboAndrcode.SelectedIndex = -1;
            txtOpSeq.Text = "";
            txtOpTime1.Text = "";
            cboAnesth.SelectedIndex = -1;
            cboPosition.SelectedIndex = -1;
            dtpOpDate.Value = NowDate.AddDays(1);
            txtPreDiagnosis.Text = "";
            txtOpIll.Text = "";
            txtRemark.Text = "";
            txtRefer.Text = "";
            FnWRTNO = 0;
            FstrName = "";
            FstrSex = "";
            FnAge = 0;

            gGSROWID = "";

            chkOpRe.Checked = false;
            chkGBER.Checked = false;
            chkAngio.Checked = false;
            chkGBDay.Checked = false;

            optAnti1.Checked = true;

            chkGS.Checked = false;

            cboLR.SelectedIndex = -1;
        }


        private void Screen_Display()
        {
            int nAge = 0;
            string strSex = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (txtPtno.Text.Trim() == "")
            {
                return;
            }

            if (cboDeptCode.Text.Trim() == "")
            {
                return;
            }

            txtPtno.Text = ComFunc.LPAD(txtPtno.Text.Trim(), 8, "0");
            dtpOpDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D")).AddDays(1);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                if (gGSROWID !="")
                {
                    #region //외래 과검사 스케쥴 쿼리
                    SQL = "";
                    SQL = "SELECT 'GS' AS tGbn, TO_CHAR(A.ExDATE,'YYYY-MM-DD') OPDATE,A.PANO,b.SNAME,A.AGE,A.SEX, ";
                    SQL = SQL + ComNum.VBLF + " a.GBIO,'' WARDCODE,'' ROOMCODE,A.DEPTCODE,A.OPSTAFF,'' OPROOM, 0 OPSEQ, ";
                    SQL = SQL + ComNum.VBLF + " A.OPTIME, a.PREDIAGNOSIS,a.OPILL,'' ANDRCODE,'' ANESTH,'' POSITION,'' IDNO, '' GBANTI, '' GBDRG, ";
                    SQL = SQL + ComNum.VBLF + " '' OPTIME_NEW ,'' OPRE,'' GBER,'' GBANGIO,'' GBDAY, '' LEFTRIGHT, ";
                    SQL = SQL + ComNum.VBLF + " ExamName REMARK,'' REFERENCE,'' GBMAGAM,'' GBDEL,0 WRTNO,B.JUMIN1,B.JUMIN2 ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.EXAM_ABR_SCHEDULE A,KOSMOS_PMPA.BAS_PATIENT B ";                   
                    SQL = SQL + ComNum.VBLF + " WHERE A.ROWID = '" + gGSROWID + "' ";
                    SQL = SQL + ComNum.VBLF + " AND A.PANO=B.PANO(+) "; 
                    #endregion
                }
                else
                {
                    #region //수술 스케쥴 쿼리
                    SQL = "";
                    SQL = "SELECT 'OP' AS tGbn,TO_CHAR(A.OPDATE,'YYYY-MM-DD') OPDATE,A.PANO,A.SNAME,A.AGE,A.SEX,";
                    SQL = SQL + ComNum.VBLF + "A.GBIO,A.WARDCODE,A.ROOMCODE,A.DEPTCODE,A.OPSTAFF,A.OPROOM,A.OPSEQ,";
                    SQL = SQL + ComNum.VBLF + "A.OPTIME,A.PREDIAGNOSIS,A.OPILL,A.ANDRCODE,A.ANESTH,A.POSITION,A.IDNO, A.GBANTI, A.GBDRG, ";
                    SQL = SQL + ComNum.VBLF + "A.OPTIME_NEW ,A.OPRE,A.GBER,A.GBANGIO,A.GBDAY, A.LEFTRIGHT, ";
                    SQL = SQL + ComNum.VBLF + "A.REMARK,A.REFERENCE,A.GBMAGAM,A.GBDEL,A.WRTNO,B.JUMIN1,B.JUMIN2 ";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_OPSCHE A,KOSMOS_PMPA.BAS_PATIENT B ";

                    if (FnWRTNO == 0)
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE A.PANO = '" + txtPtno.Text.Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.DEPTCODE='" + VB.Left(cboDeptCode.Text.Trim(), 2) + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND A.OPDATE >= TRUNC(SYSDATE) ";
                        SQL = SQL + ComNum.VBLF + "   AND (A.GBDEL <> '*' OR A.GBDEL IS NULL ) ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE A.WRTNO = " + FnWRTNO + " ";
                    }

                    SQL = SQL + ComNum.VBLF + " AND A.PANO=B.PANO(+) ";
                    #endregion
                }


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    strPtNo = dt.Rows[0]["PANO"].ToString().Trim();
                    
                    strSname = dt.Rows[0]["SNAME"].ToString().Trim();
                    FstrName = dt.Rows[0]["SNAME"].ToString().Trim();
                    strSex = dt.Rows[0]["SEX"].ToString().Trim();
                    FstrSex = dt.Rows[0]["SEX"].ToString().Trim();
                    nAge = Convert.ToInt32(VB.Val(dt.Rows[0]["AGE"].ToString().Trim()));
                    FnAge = nAge;
                    strWardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    strRoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    FstrDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    strDrCode = dt.Rows[0]["OPSTAFF"].ToString().Trim();
                    strAnDrCode = dt.Rows[0]["ANDRCODE"].ToString().Trim();
                    strJumin = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1) + "******";

                    //2019-02-13
                    chkGS.Checked = false;
                    if (dt.Rows[0]["TGbn"].ToString().Trim()=="GS")
                    {
                        chkGS.Checked = true;
                    }

                    if (FnWRTNO == 0)
                    {
                        dt.Rows[0]["WRTNO"].ToString().Trim();
                    }

                    lblPtData.Text = strPtNo + "  " + strSname.Trim() + "  " + strSex + "/" + Convert.ToString(nAge) + "  " + strJumin;

                    switch (dt.Rows[0]["GBIO"].ToString().Trim())
                    {
                        case "I":
                            cboGbio.SelectedIndex = 0;
                            break;
                        case "O":
                            cboGbio.SelectedIndex = 1;
                            break;
                    }

                    chkAngio.Checked = false;

                    if (dt.Rows[0]["GBANGIO"].ToString().Trim() == "Y")
                    {
                        chkAngio.Checked = true;
                    }


                    chkGBDay.Checked = false;

                    if (dt.Rows[0]["GBDAY"].ToString().Trim() == "Y")
                    {
                        chkGBDay.Checked = true;
                    }

                    chkGbDRG.Checked = false;

                    if (dt.Rows[0]["GBDRG"].ToString().Trim() == "Y")
                    {
                        chkGbDRG.Checked = true;
                    }


                    ComFunc.ComboFind(cboWardCode, "T", 0, strWardCode);

                    ComFunc.ComboFind(cboRoomCode, "T", 0, strRoomCode);

                    ComFunc.ComboFind(cboDeptCode, "T", 0, FstrDeptCode);

                    ComFunc.ComboFind(cboOpStaff, "R", 4, strDrCode);

                    ComFunc.ComboFind(cboAndrcode, "R", 5, strAnDrCode);

                    switch (dt.Rows[0]["OPROOM"].ToString().Trim())
                    {
                        case "1":
                        case "2":
                        case "3":
                        case "4":
                        case "5":
                        case "6":
                        case "7":
                        case "8":
                        case "9":
                            cboOpRoom.SelectedIndex = Convert.ToInt32(VB.Val(dt.Rows[0]["OPROOM"].ToString().Trim()) - 1);  //'1~9번방
                            break;
                        case "G":
                            cboOpRoom.SelectedIndex = 9;  //'Angio
                            break;
                        case "A":
                            cboOpRoom.SelectedIndex = 10;  //'Angio
                            break;
                        case "B":
                            cboOpRoom.SelectedIndex = 11;  //'Angio
                            break;
                        case "M":
                            cboOpRoom.SelectedIndex = 12;  //'MRI실
                            break;
                        case "N":
                            cboOpRoom.SelectedIndex = 13;  //'Block
                            break;
                        default:
                            cboOpRoom.SelectedIndex = -1;  //'오류
                            break;
                    }

                    //'좌우
                    cboLR.SelectedIndex = (dt.Rows[0]["LEFTRIGHT"].ToString().Trim() == "" ? -1 : Convert.ToInt32(VB.Val(dt.Rows[0]["LEFTRIGHT"].ToString().Trim())));

                    txtOpSeq.Text = dt.Rows[0]["OPSEQ"].ToString().Trim();
                    oldOpSeq = Convert.ToInt32(VB.Val(txtOpSeq.Text));
                    dtpOpDate.Value = Convert.ToDateTime(dt.Rows[0]["OPDATE"].ToString().Trim());

                    if (dt.Rows[0]["OPTIME"].ToString().Trim() == "")
                    {
                        txtOpTime1.Text = VB.Val(VB.Left(dt.Rows[0]["OPTIME_NEW"].ToString().Trim(), 2)).ToString("00") + ":" + VB.Val(VB.Right(dt.Rows[0]["OPTIME_NEW"].ToString().Trim(), 2)).ToString("00");
                    }
                    else
                    {
                        txtOpTime1.Text = dt.Rows[0]["OPTIME"].ToString().Trim();
                    }

                    cboAnesth.SelectedIndex = (dt.Rows[0]["ANESTH"].ToString().Trim() == "" ? -1 : Convert.ToInt32(VB.Val(dt.Rows[0]["ANESTH"].ToString().Trim())));
                    cboPosition.SelectedIndex = (dt.Rows[0]["POSITION"].ToString().Trim() == "" ? -1 : Convert.ToInt32(VB.Val(dt.Rows[0]["POSITION"].ToString().Trim())));

                    txtPreDiagnosis.Text = dt.Rows[0]["PREDIAGNOSIS"].ToString().Trim();

                    txtOpIll.Text = dt.Rows[0]["OPILL"].ToString().Trim();
                    txtRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();
                    txtRefer.Text = dt.Rows[0]["REFERENCE"].ToString().Trim();

                    if (dt.Rows[0]["OPRE"].ToString().Trim() == "1")
                    {
                        chkOpRe.Checked = true;
                    }
                    else
                    {
                        chkOpRe.Checked = false;
                    }

                    if (dt.Rows[0]["GBER"].ToString().Trim() == "*")
                    {
                        chkGBER.Checked = true;
                    }
                    else
                    {
                        chkGBER.Checked = false;
                    }

                    if (dt.Rows[0]["GBANTI"].ToString().Trim() == "Y")
                    {
                        optAnti0.Checked = true;
                    }
                    else
                    {
                        optAnti1.Checked = true;
                    }

                    if (FstrDrDept != FstrDeptCode)
                    {
                        btnDelete.Enabled = false;
                    }
                    else
                    {
                        btnDelete.Enabled = true;
                    }
                }

                dt.Dispose();
                dt = null;

                //'마감 점검 로직 주석 처리 하지마세요
                //GoSub Magam_Check
                string strMessage = ""; 

                SQL = "";
                SQL = " SELECT * FROM KOSMOS_OCS.OCS_OPSCHE ";
                SQL = SQL + ComNum.VBLF + " WHERE  OPDATE  = TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND    GBMAGAM = '*' ";
                SQL = SQL + ComNum.VBLF + " AND  ( GBDEL <> '*' OR GBDEL IS NULL )";

                //2020-06-17 조건추가, 혈관조영실인경우
                if(clsOpMain.GstrBuCode == "100570" && 
                    (VB.Left(cboOpRoom1.SelectedItem.ToString().Trim(), 1) == "G"
                    || VB.Left(cboOpRoom1.SelectedItem.ToString().Trim(), 1) == "A"
                    || VB.Left(cboOpRoom1.SelectedItem.ToString().Trim(), 1) == "B"))
                {
                    SQL = SQL + ComNum.VBLF + "     AND  GBANGIO = 'Y' ";
                    SQL = SQL + ComNum.VBLF + "     AND  PANO = '" + txtPtno.Text.Trim() + "'";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    btnSave.Enabled = false;
                    btnDelete.Enabled = false;
                    strMessage = "[" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "]의 스케쥴이 이미 마감되었습니다." + ComNum.VBLF + ComNum.VBLF;
                    strMessage = strMessage + "작업을 원하시면 마취과로 연락바랍니다.";

                    ComFunc.MsgBox(strMessage, "확인");

                    if (clsOpMain.GstrBuCode == clsOpMain.BUSE_SUSUL
                    || clsOpMain.GstrBuCode == clsOpMain.BUSE_MARCH)
                    {
                        btnSave.Enabled = true;
                        btnDelete.Enabled = true;
                    }

                }
                else
                {
                    btnSave.Enabled = true;
                    btnDelete.Enabled = true;
                }

                READ_ANTI(txtPtno.Text.Trim());
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Pano_Display(string argPano)
        {
            int i = 0;
            string strMsg = "";
            string strSex = "";
            int nAge = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            if (cboDeptCode.Text == "")
            {
                ComFunc.MsgBox("진료과가 공란입니다.", "확인");
                return;
            }

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //'이미 수술예약이 있는지 Check
                SQL = "";
                SQL = "SELECT WRTNO,TO_CHAR(OPDATE,'YYYY-MM-DD') OPDATE,DEPTCODE,OPSTAFF,";
                SQL = SQL + ComNum.VBLF + "       PANO,SNAME,SEX,AGE,OPTIME, OPTIME_NEW ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_OCS.OCS_OPSCHE ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO   = '" + argPano.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND OPDATE >= TRUNC(SYSDATE) ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE='" + VB.Left(cboDeptCode.Text.Trim(), 2) + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (GBDEL <> '*'  OR GBDEL IS NULL) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strMsg = dt.Rows[0]["SNAME"].ToString().Trim() + "님 등록된 수술예약이 있습니다." + ComNum.VBLF;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strMsg = strMsg + dt.Rows[i]["OPDATE"].ToString().Trim() + "일 ";
                        strMsg = strMsg + dt.Rows[i]["OPTIME"].ToString().Trim() + " ";
                        strMsg = strMsg + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "(";
                        strMsg = strMsg + clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["OPSTAFF"].ToString().Trim()) + ") " + ComNum.VBLF;
                    }

                    strMsg = strMsg + ComNum.VBLF + "등록된 수술예약을 수정하시겠습니까?" + ComNum.VBLF;

                    if (ComFunc.MsgBoxQ(strMsg, "선택", MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        FnWRTNO = Convert.ToInt32(VB.Val(dt.Rows[0]["WRTNO"].ToString().Trim()));
                        //FstrJob = "수정";
                        Screen_Display();
                        dt.Dispose();
                        dt = null;
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }

                dt.Dispose();
                dt = null;

                //'현재 재원자이면 재원정보를 Display함

                SQL = " SELECT M.PANO,     M.SNAME,    M.SEX,   P.JUMIN1,   P.JUMIN2,P.JUMIN3,  ";
                SQL = SQL + ComNum.VBLF + "        M.WARDCODE, M.ROOMCODE, M.DEPTCODE, M.DRCODE, M.BI, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(M.INDATE,'YYYY-MM-DD') INDATE         ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_PMPA.IPD_NEW_MASTER  M, ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P  ";
                SQL = SQL + ComNum.VBLF + " WHERE  M.PANO  =  '" + txtPtno.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  M.GBSTS IN ('0','2')";
                SQL = SQL + ComNum.VBLF + "   AND  M.OUTDATE IS NULL";
                SQL = SQL + ComNum.VBLF + "   AND  M.PANO  =  P.PANO ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strPtNo = dt.Rows[0]["PANO"].ToString().Trim();
                    strSname = dt.Rows[0]["SNAME"].ToString().Trim();
                    FstrName = dt.Rows[0]["SNAME"].ToString().Trim();
                    strSex = dt.Rows[0]["SEX"].ToString().Trim();
                    FstrSex = dt.Rows[0]["SEX"].ToString().Trim();

                    if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                    {
                        nAge = ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim()));
                        strJumin = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim()), 1) + "******";
                        FnAge = nAge;
                    }
                    else
                    {
                        nAge = ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim());
                        strJumin = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1) + "******";
                        FnAge = nAge;
                    }

                    strWardCode = dt.Rows[0]["WARDCODE"].ToString().Trim();
                    strRoomCode = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    FstrDeptCode = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    strDrCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                    strInDate = dt.Rows[0]["INDATE"].ToString().Trim();

                    switch (dt.Rows[0]["BI"].ToString().Trim())
                    {
                        case "11":
                        case "12":
                        case "13":
                            chkGbDRG.Visible = true;
                            break;
                        default:
                            chkGbDRG.Visible = false;
                            break;
                    }

                    lblPtData.Text = strPtNo + "  " + strSname.Trim() + "  " + strSex + "/" + Convert.ToString(nAge) + "  " + strJumin;

                    cboGbio.SelectedIndex = 0;

                    ComFunc.ComboFind(cboWardCode, "T", 0, strWardCode);

                    ComFunc.ComboFind(cboRoomCode, "T", 0, strRoomCode);

                    switch (cboDeptCode.Text.Trim())
                    {
                        case "OS":
                            cboOpRoom.SelectedIndex = 0;
                            break;
                        case "NS":
                            cboOpRoom.SelectedIndex = 1;
                            break;
                        case "CS":
                        case "UR":
                            cboOpRoom.SelectedIndex = 2;
                            break;
                        case "OG":
                            cboOpRoom.SelectedIndex = 4;
                            break;
                        case "EN":
                            cboOpRoom.SelectedIndex = 5;
                            break;
                        case "GS":
                            cboOpRoom.SelectedIndex = 3;
                            break;
                        case "OT":
                            cboOpRoom.SelectedIndex = 7;
                            break;
                        default:
                            cboOpRoom.SelectedIndex = 3;
                            break;
                    }
                }
                else
                {

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = " SELECT PANO, SNAME, SEX, JUMIN1, JUMIN2,JUMIN3  ";
                    SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_PMPA.BAS_PATIENT ";
                    SQL = SQL + ComNum.VBLF + " WHERE  PANO = '" + txtPtno.Text.Trim() + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {

                        strPtNo = dt.Rows[0]["PANO"].ToString().Trim();
                        strSname = dt.Rows[0]["SNAME"].ToString().Trim();
                        FstrName = dt.Rows[0]["SNAME"].ToString().Trim();
                        strSex = dt.Rows[0]["SEX"].ToString().Trim();
                        FstrSex = dt.Rows[0]["SEX"].ToString().Trim();

                        if (dt.Rows[0]["JUMIN3"].ToString().Trim() != "")
                        {
                            nAge = ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[0]["JUMIN1"].ToString().Trim() + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim()));
                            strJumin = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim()), 1) + "******";
                            FnAge = nAge;
                        }
                        else
                        {
                            nAge = ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[0]["JUMIN1"].ToString().Trim() + dt.Rows[0]["JUMIN2"].ToString().Trim());
                            strJumin = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + VB.Left(dt.Rows[0]["JUMIN2"].ToString().Trim(), 1) + "******";
                            FnAge = nAge;
                        }

                        strWardCode = "";
                        strRoomCode = "";
                        FstrDeptCode = "";
                        strDrCode = "";
                        strInDate = "";

                        lblPtData.Text = strPtNo + "  " + strSname.Trim() + "  " + strSex + "/" + Convert.ToString(nAge) + "  " + strJumin;


                        cboGbio.SelectedIndex = 1;

                        dt.Dispose();
                        dt = null;

                        SQL = "";
                        SQL = " SELECT BI FROM KOSMOS_PMPA.OPD_MASTER ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPtno.Text.Trim() + "'";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE >=TRUNC(SYSDATE -10) ";
                        SQL = SQL + ComNum.VBLF + " ORDER BY BI DESC ";
                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            switch (dt.Rows[0]["BI"].ToString().Trim())
                            {
                                case "11":
                                case "12":
                                case "13":
                                    chkGbDRG.Visible = true;
                                    break;
                                default:
                                    chkGbDRG.Visible = false;
                                    break;
                            }
                        }

                    }
                    else
                    {
                        lblPtData.Text = "";
                        ComFunc.MsgBox("등록되지 않은 환자입니다.", "확인");
                    }
                }


                dt.Dispose();
                dt = null;

                //'진료과별 수술실을 자동 배정
                if (cboDeptCode.Text.Trim() != "")
                {
                    switch (cboDeptCode.Text.Trim())
                    {
                        case "OS":
                            cboOpRoom.SelectedIndex = 0;
                            break;
                        case "NS":
                            cboOpRoom.SelectedIndex = 1;
                            break;
                        case "CS":
                        case "UR":
                            cboOpRoom.SelectedIndex = 2;
                            break;
                        case "OG":
                            cboOpRoom.SelectedIndex = 4;
                            break;
                        case "EN":
                            cboOpRoom.SelectedIndex = 5;
                            break;
                        case "GS":
                            cboOpRoom.SelectedIndex = 6;
                            break;
                        default:
                            cboOpRoom.SelectedIndex = 3;
                            break;
                    }
                }

                //'의사코드 Combo SET
                SQL = " SELECT DRCODE, DRNAME ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_PMPA.BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE  DRDEPT1 = '" + cboDeptCode.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + " AND    TOUR <> 'Y' ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY DRCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    cboOpStaff.Items.Clear();
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboOpStaff.Items.Add(dt.Rows[i]["DRNAME"].ToString().Trim() + dt.Rows[i]["DRCODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                ComFunc.ComboFind(cboOpStaff, "R", 4, strDrCode);

                READ_ANTI(txtPtno.Text.Trim());

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Yeyak_Display()
        {
            int i = 0;

            string strOpRoom = "";
            string strOpSeq = "";
            string strName = "";
            string strOpTime = "";

            string strOpDate = "";

            ssView2_Sheet1.RowCount = 0;

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (cboOpRoom1.Text.Trim() == "")
            {
                return;
            }

            if (cboRDept.Text.Trim() == "")
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = "SELECT 'OP' AS TGBN, OPROOM,  OPSEQ, SNAME, OPTIME, PANO,DEPTCODE,OPSTAFF,";
                SQL = SQL + ComNum.VBLF + "SEX,AGE,TO_CHAR(OPDATE,'YYYY/MM/DD') OPDATE,WRTNO, OPTIME_NEW, ANESTH ";
                SQL = SQL + ComNum.VBLF + " ,KOSMOS_OCS.FC_BAS_PATIENT_SNAME(Pano) f_SName,ROWID ";

                if (clsOpMain.GstrBuCode == clsOpMain.BUSE_MARCH || clsOpMain.GstrBuCode == clsOpMain.BUSE_SUSUL)
                {
                    SQL = SQL + ComNum.VBLF + ",(SELECT B.DRNAME FROM KOSMOS_OCS.OCS_DOCTOR B WHERE B.SABUN = ANDRCODE) AS ANDRNAME      ";
                }

                SQL = SQL + ComNum.VBLF + "FROM  KOSMOS_OCS.OCS_OPSCHE";

                if (clsOpMain.GstrBuCode == clsOpMain.BUSE_MARCH || clsOpMain.GstrBuCode == clsOpMain.BUSE_SUSUL)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE  OPDATE   = TO_DATE('" + dtpOpDate1.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "WHERE  OPDATE   >= TRUNC(SYSDATE) ";
                }

                if (cboRDept.Text != "전체")
                {
                    SQL = SQL + ComNum.VBLF + " AND DEPTCODE = '" + cboRDept.Text.Trim() + "' ";
                }

                if (clsOpMain.GstrBuCode == clsOpMain.BUSE_MARCH || clsOpMain.GstrBuCode == clsOpMain.BUSE_SUSUL)
                {
                    SQL = SQL + ComNum.VBLF + " AND  ( GBANGIO IS NULL OR GBANGIO='N') ";
                }

                SQL = SQL + ComNum.VBLF + " AND  ( GBDEL   <> '*' OR GBDEL IS NULL )";

                if (VB.Left(cboOpRoom1.Text.Trim(), 1) != "*")
                {
                    SQL = SQL + ComNum.VBLF + "  AND OPROOM = '" + VB.Left(cboOpRoom1.Text, 1) + "' ";
                }

                SQL = SQL + ComNum.VBLF + "   AND OPROOM <> '*' ";

                if (clsOpMain.GstrBuCode == clsOpMain.BUSE_MARCH || clsOpMain.GstrBuCode == clsOpMain.BUSE_SUSUL)
                {
                }
                else
                {
                    if (gPart == "OK")
                    {
                        SQL += " UNION ALL                                                                                          \r\n";
                        SQL += " SELECT 'GS' AS TGBN, 'GS' OPROOM,  0 OPSEQ, ' ' SNAME, OPTIME, PANO,DEPTCODE,OPSTAFF         \r\n";
                        SQL += "  ,SEX,AGE,TO_CHAR(ExDATE,'YYYY/MM/DD') OPDATE,0 WRTNO, '' OPTIME_NEW, '' ANESTH              \r\n";
                        SQL += "  ,KOSMOS_OCS.FC_BAS_PATIENT_SNAME(Pano) f_SName,ROWID              \r\n";
                        SQL += "  FROM " + ComNum.DB_MED + "EXAM_ABR_SCHEDULE                                                       \r\n";
                        SQL += "   WHERE 1=1                                                                                        \r\n";
                        SQL += "    AND Gubun ='02'                                                                                 \r\n";
                        SQL += "    AND TRUNC(ExDate) >= TO_DATE('" + dtpOpDate1.Value.AddDays(-1).ToString("yyyy-MM-dd") + "','YYYY-MM-DD')     \r\n";

                        if (cboRDept.Text != "전체")
                        {
                            SQL += " AND DEPTCODE = '" + cboRDept.Text.Trim() + "'  \r\n";
                        }

                    }
                }                    

                SQL = SQL + ComNum.VBLF + " ORDER  BY OPDATE,OPROOM, OPSEQ ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView2_Sheet1.RowCount = dt.Rows.Count;
                    ssView2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);


                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strOpRoom = dt.Rows[i]["OPROOM"].ToString().Trim();
                        strOpSeq = dt.Rows[i]["OPSEQ"].ToString().Trim();
                        
                        strOpTime = dt.Rows[i]["OPTIME"].ToString().Trim();

                        if (strOpDate != dt.Rows[i]["OPDATE"].ToString().Trim())
                        {
                            strOpDate = dt.Rows[i]["OPDATE"].ToString().Trim();
                            ssView2_Sheet1.Rows.Get(i).Border = new FarPoint.Win.LineBorder(Color.Red, 2, false, true, false, false);
                        }

                        ssView2_Sheet1.Cells[i, 0].Text = VB.Right(dt.Rows[i]["OPDATE"].ToString().Trim(), 5);
                        if (gPart =="OK")
                        {
                            strName = dt.Rows[i]["f_SNAME"].ToString().Trim();
                            ssView2_Sheet1.Cells[i, 1].Text = strOpRoom;
                        }
                        else
                        {
                            strName = dt.Rows[i]["SNAME"].ToString().Trim();
                            ssView2_Sheet1.Cells[i, 1].Text = strOpRoom + "-" + strOpSeq;
                        }
                        


                        if (dt.Rows[i]["OPTIME_NEW"].ToString().Trim() != "")
                        {
                            ssView2_Sheet1.Cells[i, 2].Text = VB.Left(dt.Rows[i]["OPTIME_NEW"].ToString().Trim(), 2) + ":" + VB.Right(dt.Rows[i]["OPTIME_NEW"].ToString().Trim(), 2);
                        }
                        else
                        {
                            ssView2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["OPTIME"].ToString().Trim();
                        }

                        ssView2_Sheet1.Cells[i, 3].Text = strName;
                        ssView2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 5].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["OPSTAFF"].ToString().Trim());
                        ssView2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["AGE"].ToString().Trim() + "/" + dt.Rows[i]["SEX"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["WRTNO"].ToString().Trim();

                        //GS외래 검사 스케쥴만 ROWID 표시
                        ssView2_Sheet1.Cells[i, 11].Text = "";
                        if (dt.Rows[i]["TGBN"].ToString().Trim() =="GS" )
                        {
                            ssView2_Sheet1.Cells[i, 11].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        }                        

                        SQL = " SELECT VDRL, HCV_IGG, HBS_AG  FROM KOSMOS_OCS.EXAM_INFECTMASTER ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["VDRL"].ToString().Trim() != ""
                            || dt1.Rows[0]["HCV_IGG"].ToString().Trim() != ""
                            || dt1.Rows[0]["HBS_AG"].ToString().Trim() != "")
                            {
                                ssView2_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(230, 220, 255);
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (clsOpMain.GstrBuCode == clsOpMain.BUSE_SUSUL
                        || clsOpMain.GstrBuCode == clsOpMain.BUSE_MARCH)
                        {
                            if (dt.Rows[i]["ANESTH"].ToString().Trim() != "")
                            {
                                ssView2_Sheet1.Cells[i, 9].Text = cboAnesth.Items[Convert.ToInt32(VB.Val(dt.Rows[i]["ANESTH"].ToString().Trim()))].ToString().Trim();
                            }

                            ssView2_Sheet1.Cells[i, 10].Text = dt.Rows[i]["ANDRNAME"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Combo_Additem()
        {
            Dept_Add();//'진료과 & 수술과
            Ward_Add();//'진료과 & 수술과
            Gbio_Add();//'환자구분
            OpRoom_Add();//'수술실
            AnDrCode_Add();//'마취의사
            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboAnesth, "OP_마취방법", 1, true, "N");//'마취방법
            clsVbfunc.SetBCodeCombo(clsDB.DbCon, cboPosition, "OP_Position", 1, true, "");
            LeftRight_Add();//'좌우
        }

        private void Dept_Add()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT DEPTCODE, DEPTNAMEK FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE  DEPTCODE  NOT IN ('II','R6','HR','PT', 'TO' ) ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY  PRINTRANKING ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboDeptCode.Items.Clear();
                cboRDept.Items.Clear();
                cboRDept.Items.Add("전체");
                cboViewDept.Items.Clear();
                cboViewDept.Items.Add("전체");

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboRDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                        cboDeptCode.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
                        cboViewDept.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim());
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void Ward_Add()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT WARDCODE, WARDNAME FROM KOSMOS_PMPA.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY WARDCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboWardCode.Items.Clear();
                cboWardCode.Items.Add(" ");

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWardCode.Items.Add(dt.Rows[i]["WARDCODE"].ToString().Trim());
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Gbio_Add()
        {
            cboGbio.Items.Clear();
            cboGbio.Items.Add("입원수술");
            cboGbio.Items.Add("통원수술");
        }

        private void OpRoom_Add()
        {
            cboOpRoom.Items.Clear();
            cboOpRoom.Items.Add("1.1실");
            cboOpRoom.Items.Add("2.2실");
            cboOpRoom.Items.Add("3.3실");
            cboOpRoom.Items.Add("4.4실");
            cboOpRoom.Items.Add("5.5실");
            cboOpRoom.Items.Add("6.6실");
            cboOpRoom.Items.Add("7.7실");
            cboOpRoom.Items.Add("8.8실");
            cboOpRoom.Items.Add("9.9실");
            cboOpRoom.Items.Add("G.Angio");
            cboOpRoom.Items.Add("A.AngioA");
            cboOpRoom.Items.Add("B.AngioB");
            cboOpRoom.Items.Add("M.MRI실");
            cboOpRoom.Items.Add("N.Block");

            cboOpRoom1.Items.Clear();
            cboOpRoom1.Items.Add("*.전체");
            cboOpRoom1.Items.Add("1.1실");
            cboOpRoom1.Items.Add("2.2실");
            cboOpRoom1.Items.Add("3.3실");
            cboOpRoom1.Items.Add("4.4실");
            cboOpRoom1.Items.Add("5.5실");
            cboOpRoom1.Items.Add("6.6실");
            cboOpRoom1.Items.Add("7.7실");
            cboOpRoom1.Items.Add("8.8실");
            cboOpRoom1.Items.Add("9.9실");
            cboOpRoom1.Items.Add("G.Angio");
            cboOpRoom1.Items.Add("A.AngioA");
            cboOpRoom1.Items.Add("B.AngioB");
            cboOpRoom1.Items.Add("M.MRI실");
            cboOpRoom1.Items.Add("N.Block");
        }

        private void AnDrCode_Add()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT SABUN, DRNAME FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE  DEPTCODE IN ( 'RT','PC')  ";  // '~1;
                SQL = SQL + ComNum.VBLF + "   AND  GBOUT = 'N'   ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY DRCODE  ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                cboAndrcode.Items.Clear();
                cboAndrcode.Items.Add("");

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboAndrcode.Items.Add(dt.Rows[i]["DRNAME"].ToString().Trim() + VB.Space(20) + dt.Rows[i]["SABUN"].ToString().Trim());
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void LeftRight_Add()
        {
            cboLR.Items.Clear();
            cboLR.Items.Add("0.해당없음");
            cboLR.Items.Add("1.Right(Rt)");
            cboLR.Items.Add("2.Left(Lt)");
            cboLR.Items.Add("3.Right(OD)");
            cboLR.Items.Add("4.Left(OS)");
            cboLR.Items.Add("5.Both");
        }

        private void cboDeptCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            if (cboDeptCode.SelectedIndex == -1)
            {
                return;
            }

            cboOpStaff.Items.Clear();

            try
            {
                SQL = "";
                SQL = " SELECT DRCODE, DRNAME ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_PMPA.BAS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE  DRDEPT1 = '" + cboDeptCode.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + " AND    TOUR <> 'Y' ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY PRINTRANKING, DRCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboOpStaff.Items.Add(dt.Rows[i]["DRNAME"].ToString().Trim() + dt.Rows[i]["DRCODE"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                switch (cboDeptCode.Text.Trim())
                {
                    case "GS":
                    case "OG":
                    case "OT":
                    case "EN":
                        chkGbDRG.Enabled = true;
                        break;

                    default:
                        chkGbDRG.Enabled = false;
                        break;

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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboOpRoom_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (dtpOpDate.Value.ToString("yyyy-MM-dd") == "9998-12-31")
            {
                return;
            }

            if (cboOpRoom.SelectedIndex == -1)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = " SELECT OPSEQ FROM KOSMOS_OCS.OCS_OPSCHE ";
                SQL = SQL + ComNum.VBLF + " WHERE  OPDATE = TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND    OPROOM = '" + VB.Left(cboOpRoom.Text.Trim(), 1) + "' ";
                SQL = SQL + ComNum.VBLF + " AND  (  GBDEL <> '*' OR GBDEL IS NULL )";
                SQL = SQL + ComNum.VBLF + " ORDER  BY OPSEQ DESC ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    txtOpSeq.Text = "1";
                }
                else
                {
                    txtOpSeq.Text = Convert.ToString(VB.Val(dt.Rows[0]["OPSEQ"].ToString().Trim()) + 1);
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void cboWardCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (cboWardCode.SelectedIndex == -1)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                cboRoomCode.Items.Clear();
                cboRoomCode.Items.Add(" ");

                SQL = " SELECT ROOMCODE FROM KOSMOS_PMPA.BAS_ROOM ";
                SQL = SQL + ComNum.VBLF + " WHERE  WARDCODE = '" + cboWardCode.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY  ROOMCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboRoomCode.Items.Add(dt.Rows[i]["ROOMCODE"].ToString().Trim());
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void chkAngio_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAngio.Checked == true)
            {
                if (VB.Left(cboOpRoom.Text.Trim(), 1) != "G")
                {
                    cboOpRoom.SelectedIndex = 9;
                }
            }
            else
            {
                if (VB.Left(cboOpRoom.Text.Trim(), 1) == "G")
                {
                    cboOpRoom.SelectedIndex = -1;
                }
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            clsPrint CP = new clsPrint();
            PrintDocument pd = new PrintDocument();

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            string strPrintName1 = "";
            string strPrintName2 = "";

            mstrABO = "";

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                return; //권한 확인

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                strPrintName1 = clsPrint.gGetDefaultPrinter();
                strPrintName2 = CP.getPrinter_Chk("접수증");

                clsPrint.gSetDefaultPrinter(strPrintName2);

                //'혈액형정보를 읽음;
                SQL = "";
                SQL = "SELECT ABO FROM KOSMOS_OCS.EXAM_BLOOD_MASTER ";
                SQL = SQL + ComNum.VBLF + "WHERE PANO='" + txtPtno.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND SNAME='" + FstrName + "' ";
                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    mstrABO = dt.Rows[0]["ABO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                //pd.Print.Size
                pd.PrintPage += new PrintPageEventHandler(ePrintPage);
                pd.Print();    //프린트

                mstrABO = "";

                ComFunc.Delay(1000);
                clsPrint.gSetDefaultPrinter(strPrintName1);

                CP = null;
                pd = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox("접수증 출력 오류" + ComNum.VBLF + ComNum.VBLF + ex.Message);
            }
        }

        void ePrintPage(object sender, PrintPageEventArgs ev)
        {
            Font printFont;
            int nX;
            int nY;

            nX = 1;
            nY = 3;

            string sFont = "굴림체";
            string s;

            printFont = new Font(sFont, 11, FontStyle.Bold);
            s = txtPtno.Text + VB.Space(3) + FstrDeptCode + " (" + FstrSex + "/" + FnAge + ")  외래";
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 0, nY + 20);

            printFont = new Font(sFont, 16, FontStyle.Bold);
            s = FstrName + VB.Space(2) + mstrABO;
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 0, nY + 37);

            printFont = new Font(sFont, 9, FontStyle.Bold);
            s = "진단명: " + txtPreDiagnosis.Text.Trim();
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 0, nY + 60);

            printFont = new Font(sFont, 9, FontStyle.Bold);
            s = "수술명: " + txtOpIll.Text.Trim();
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 0, nY + 75);

            printFont = new Font(sFont, 9, FontStyle.Bold);
            s = "수술부위: " + cboLR.Text.Trim();
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 0, nY + 90);

            printFont = new Font(sFont, 9, FontStyle.Bold);
            s = "외래:           수술실:";
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 0, nY + 105);

            printFont = new Font(sFont, 9, FontStyle.Bold);
            s = "                                   I";
            ev.Graphics.DrawString(s, printFont, Brushes.Black, nX + 0, nY + 180);

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return; //권한 확인
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            int intRowAffected = 0;

            if (txtPtno.Text.Trim() == "")
            {
                return;
            }

            if (dtpOpDate.Value.ToString("yyyy-MM-dd") == "9998-12-31")
            {
                return;
            }

            if (ComFunc.MsgBoxQ("예약정보를 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                Yeyak_Display();
                return;
            }
                    
            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                if (gPart == "OK" && chkGS.Checked == true)
                {
                    #region //외과 과검사 스케쥴 삭제
                    //자료있으면 삭제      
                    SqlErr = "";
                    if (gGSROWID != "")
                    {
                        SqlErr = del_EXAM_ABR_SCHEDULE(clsDB.DbCon, gGSROWID, ref intRowAffected);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                            
                            return;
                        }
                        else
                        {
                            clsDB.setCommitTran(clsDB.DbCon);
                            SCREEN_CLEAR();
                            txtPtno.Text = "";
                            Yeyak_Display();
                        }
                    }
                    else
                    {
                        clsDB.setCommitTran(clsDB.DbCon);
                        SCREEN_CLEAR();
                        txtPtno.Text = "";
                        Yeyak_Display();
                    }

                    Cursor.Current = Cursors.Default;

                    #endregion
                }
                else
                {
                    #region //수술 스케쥴 삭제 부분
                    if (clsOpMain.GstrBuCode != clsOpMain.BUSE_SUSUL  && clsOpMain.GstrBuCode != clsOpMain.BUSE_MARCH)
                    {
                        SQL = "";
                        SQL = " SELECT COUNT(*)  CNT FROM KOSMOS_OCS.OCS_OPSCHE ";
                        SQL = SQL + ComNum.VBLF + " WHERE OPDATE = TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND GBMAGAM ='*' ";

                        //2020-06-17 조건추가, 혈관조영실인경우
                        if (clsOpMain.GstrBuCode == "100570" &&
                            (VB.Left(cboOpRoom.SelectedItem.ToString().Trim(), 1) == "G"
                            || VB.Left(cboOpRoom.SelectedItem.ToString().Trim(), 1) == "A"
                            || VB.Left(cboOpRoom.SelectedItem.ToString().Trim(), 1) == "B"))
                        {
                            SQL = SQL + ComNum.VBLF + "     AND  PANO = '" + txtPtno.Text.Trim() + "'";
                            SQL = SQL + ComNum.VBLF + "     AND  GBANGIO = 'Y' ";
                        }

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            if (dt.Rows[0]["CNT"].ToString().Trim() != "0")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                dt.Dispose();
                                dt = null;
                                ComFunc.MsgBox("마감된 수술 스케쥴은 삭제가 불가능함", "확인");
                                Yeyak_Display();
                                return;
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    }


                    //'이미 오더를 입력하였으면 삭제가 않되도록 처리함(2006.3.24)
                    SQL = "SELECT SUM(QTY) QTY FROM KOSMOS_PMPA.ORAN_SLIP ";
                    SQL = SQL + ComNum.VBLF + "WHERE WRTNO=" + FnWRTNO + " ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        if (VB.Val(dt.Rows[0]["QTY"].ToString().Trim()) > 0)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            dt.Dispose();
                            dt = null;
                            ComFunc.MsgBox("입력된 처방이 있어 삭제가 불가능함", "확인");
                            Yeyak_Display();
                            return;
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    SQL = "";
                    SQL = "          UPDATE KOSMOS_OCS.OCS_OPSCHE SET ";
                    SQL = SQL + ComNum.VBLF + " IDNO   = '" + (clsType.User.Sabun) + "', ";
                    SQL = SQL + ComNum.VBLF + " REMARK = '" + txtRemark.Text.Trim() + "', ";
                    SQL = SQL + ComNum.VBLF + " GBDEL  = '*', ";
                    SQL = SQL + ComNum.VBLF + " DDATE = SYSDATE  ";

                    if (FnWRTNO == 0)
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE OPDATE = TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " AND   PANO   = '" + txtPtno.Text.Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + " AND   DEPTCODE='" + cboDeptCode.Text.Trim() + "' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE WRTNO=" + FnWRTNO + " ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    SQL = "   DELETE KOSMOS_PMPA.ORAN_MASTER ";

                    if (FnWRTNO == 0)
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE OPDATE = TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " AND   PANO   = '" + txtPtno.Text.Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + " AND   DEPTCODE='" + cboDeptCode.Text.Trim() + "' ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE WRTNO=" + FnWRTNO + " ";
                    }

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    SCREEN_CLEAR();
                    txtPtno.Text = "";
                    Yeyak_Display();
                    Cursor.Current = Cursors.Default;
                    #endregion
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }


        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
        }

        private void btnDeleteH_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            string strGbn = "";
            string strRemark = "";

            if (lblHelp.Text == "진단명")
            {
                strGbn = "1";
                strRemark = txtPreDiagnosis.Text.Trim();
            }
            else if (lblHelp.Text == "수술명")
            {
                strGbn = "2";
                strRemark = txtOpIll.Text.Trim();
            }
            else
            {
                strGbn = "3";
                strRemark = txtRemark.Text.Trim();
            }

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " DELETE   KOSMOS_OCS.OCS_REMARKOP  ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + clsType.User.Sabun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GBBUN = '" + strGbn + "' ";
                SQL = SQL + ComNum.VBLF + "   AND REMARKCODE = '" + txtHelp.Text + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                txtHelp.Text = "";
                ssViewH_Sheet1.RowCount = 0;
                READ_CREMARK();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExitH_Click(object sender, EventArgs e)
        {
            panHelp.Visible = false;
            txtHelp.Text = "";
            ssViewH_Sheet1.RowCount = 0;
        }

        private void btnHelp01_Click(object sender, EventArgs e)
        {
            panHelp.Visible = true;
            lblHelp.Text = "진단명";
            READ_CREMARK();
        }

        private void btnHelp02_Click(object sender, EventArgs e)
        {
            panHelp.Visible = true;
            lblHelp.Text = "수술명";
            READ_CREMARK();
        }

        private void btnHelp03_Click(object sender, EventArgs e)
        {
            panHelp.Visible = true;
            lblHelp.Text = "준비사항";
            READ_CREMARK();
        }

        private void READ_CREMARK()
        {
            int i = 0;
            string strGbn = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            if (lblHelp.Text == "진단명")
            {
                strGbn = "1";
            }
            else if (lblHelp.Text == "수술명")
            {
                strGbn = "2";
            }
            else
            {
                strGbn = "3";
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                SQL = "";
                SQL = " SELECT REMARKCODE, REMARK FROM KOSMOS_OCS.OCS_REMARKOP ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + clsType.User.Sabun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GBBUN = '" + strGbn + "' ";
                SQL = SQL + ComNum.VBLF + "  ORDER BY REMARKCODE ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssViewH_Sheet1.RowCount = dt.Rows.Count;
                    ssViewH_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssViewH_Sheet1.Cells[i, 0].Text = dt.Rows[i]["REMARKCODE"].ToString().Trim();
                        ssViewH_Sheet1.Cells[i, 1].Text = dt.Rows[i]["REMARK"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            #region //권한 및 변수세팅..
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false)
                return; //권한 확인
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인

            string strAnesth = "";
            string strPosition = "";
            string strGbio = "";
            string strAnDrCode = "";
            string strDrCode = "";// 'Oran_Master
            string strBi = "";// 'Oran_Master

            string strJumin = "";
            string strSex = "";
            int nAge = 0;
            string strOpRe = "";
            string strGbEr = "";
            string strGbAngio = "";
            string strGbDay = "";
            string strGBANTI = "";
            string strGbDRG = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;
            string sRemark = string.Empty;

            if (Data_Check() == false)
            {
                return;
            }

            if (optAnti0.Checked == true)
            {
                strGBANTI = "Y";
            }
            else if (optAnti1.Checked == true)
            {
                strGBANTI = "N";
            }

            strOpRe = "0";
            if (chkOpRe.Checked == true)
            {
                strOpRe = "1";
            }

            strGbEr = "";
            if (chkGBER.Checked == true)
            {
                strGbEr = "*";
            }

            strGbAngio = "N";
            if (chkAngio.Checked == true)
            {
                strGbAngio = "Y";
            }

            strGbDay = "N";
            if (chkGBDay.Checked == true)
            {
                strGbDay = "Y";
            }

            strGbDRG = "N";
            {
                if (chkGbDRG.Checked == true)
                {
                    strGbDRG = "Y";
                }

            } 
            #endregion

            clsAES.Read_Jumin_AES(clsDB.DbCon, txtPtno.Text.Trim());

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {

                #region //환자 정보, 입원정보 변수 설정 등
                SQL = "";
                SQL = "SELECT JUMIN1,JUMIN2,SEX FROM KOSMOS_PMPA.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO='" + txtPtno.Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                strJumin = "";
                strSex = "";

                nAge = 0;

                if (dt.Rows.Count > 0)
                {
                    strJumin = dt.Rows[0]["JUMIN1"].ToString().Trim();
                    strJumin = strJumin + clsAES.GstrAesJumin2;

                    nAge = ComFunc.AgeCalc(clsDB.DbCon, strJumin);
                    strSex = dt.Rows[0]["SEX"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;


                if (VB.Left(cboOpRoom.Text.Trim(), 1) == "")
                {
                    switch (cboDeptCode.Text.Trim())
                    {
                        case "GS":
                            cboOpRoom.SelectedIndex = 1;
                            break;
                        case "OT":
                            cboOpRoom.SelectedIndex = 2;
                            break;
                        case "OB":
                        case "OG":
                            cboOpRoom.SelectedIndex = 3;  //'~1
                            break;
                        case "UR":
                            cboOpRoom.SelectedIndex = 4;
                            break;
                        case "DN":
                        case "DT":
                            cboOpRoom.SelectedIndex = 4;   //'~1
                            break;
                        case "NS":
                            cboOpRoom.SelectedIndex = 4;
                            break;
                        case "OS":
                            cboOpRoom.SelectedIndex = 5;
                            break;
                        case "EN":
                            cboOpRoom.SelectedIndex = 6;
                            break;
                        default:
                            cboOpRoom.SelectedIndex = 0;
                            break;
                    }
                }

                strGbio = (cboGbio.SelectedIndex == 0 ? "I" : "O");
                strAnDrCode = VB.Right(cboAndrcode.Text.Trim(), 5);
                strAnesth = (cboAnesth.SelectedIndex == -1 ? "" : Convert.ToString(cboAnesth.SelectedIndex));
                strPosition = (cboPosition.SelectedIndex == -1 ? "" : Convert.ToString(cboPosition.SelectedIndex));
                strDrCode = "";
                strBi = "";

                SQL = "";
                SQL = "SELECT DRCODE, BI FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE SNAME ='" + strSname.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + txtPtno.Text.Trim() + "'";
                SQL = SQL + ComNum.VBLF + "   AND GBSTS IN ('0','2') ";
                SQL = SQL + ComNum.VBLF + "   AND OUTDATE IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strDrCode = dt.Rows[0]["DRCODE"].ToString().Trim();
                    strBi = dt.Rows[0]["BI"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strDrCode.Trim() == "")
                {
                    strDrCode = VB.Right(cboOpStaff.Text.Trim(), 4);
                }

                #endregion

                if (gPart =="OK" && chkGS.Checked ==true)
                {
                    //자료있으면 삭제      
                    SqlErr = "";
                    if (gGSROWID != "")
                    {
                        SqlErr = del_EXAM_ABR_SCHEDULE(clsDB.DbCon, gGSROWID, ref intRowAffected);
                    }

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                            
                        return;
                    }
                    else
                    {
                        sRemark = txtRemark.Text.Trim();
                        if (sRemark == "")
                        {
                            sRemark = "외과 VABB 검사 ";
                        }

                        //저장
                        SqlErr = ins_EXAM_ABR_SCHEDULE(clsDB.DbCon, "02", txtPtno.Text.Trim(), dtpOpDate.Value.ToString("yyyy-MM-dd"), sRemark, Convert.ToInt32(clsType.User.IdNumber), txtOpTime1.Text.Trim(), cboDeptCode.Text.Trim(), VB.Right(cboOpStaff.Text.Trim(), 4), nAge, strSex, txtPreDiagnosis.Text.Trim(), txtOpIll.Text.Trim(), strGbio.Trim() , ref intRowAffected);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장                            
                            return;
                        }
                        else
                        {
                            clsDB.setCommitTran(clsDB.DbCon);

                            SCREEN_CLEAR();
                            Yeyak_Display();

                            txtPtno.Text = "";

                        }
                    }
                    
                }
                else
                {
                    #region //수술 스케쥴 등록 부분
                    
                    #region //수술 스케쥴 번호 체크
                    if (oldOpSeq != newOpSeq)
                    {
                        //GoSub OpSeq_Update
                        SQL = "";
                        SQL = " SELECT OPSEQ FROM KOSMOS_OCS.OCS_OPSCHE ";
                        SQL = SQL + ComNum.VBLF + " WHERE  OPDATE  = TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + " AND    OPROOM  = '" + VB.Left(cboOpRoom.Text.Trim(), 1) + "' ";
                        SQL = SQL + ComNum.VBLF + " AND    OPSEQ  >= '" + txtOpSeq.Text.Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + " AND  (  GBDEL  <> '*' OR GBDEL IS NULL )";
                        SQL = SQL + ComNum.VBLF + " ORDER  BY  OPSEQ ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            if (VB.Val(txtOpSeq.Text.Trim()) >= VB.Val(dt.Rows[0]["OPSEQ"].ToString().Trim()))
                            {
                                SQL = "";
                                SQL = " UPDATE KOSMOS_OCS.OCS_OPSCHE ";
                                SQL = SQL + ComNum.VBLF + " SET OPSEQ = OPSEQ+1 ";
                                SQL = SQL + ComNum.VBLF + " WHERE OPDATE  = TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                                SQL = SQL + ComNum.VBLF + "   AND OPROOM  = '" + VB.Left(cboOpRoom.Text.Trim(), 1) + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND   OPSEQ  >= '" + VB.Val(txtOpSeq.Text.Trim()) + "' ";
                                SQL = SQL + ComNum.VBLF + "   AND (  GBDEL  <> '*'  OR GBDEL IS NULL ) ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    dt.Dispose();
                                    dt = null;
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                                    Cursor.Current = Cursors.Default;
                                    return;
                                }
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    } 
                    #endregion
                   

                    if (FnWRTNO == 0)
                    {
                        //GoSub Data_Insert
                        #region //신규이면
                        FnWRTNO = clsOpMain.READ_New_JepsuNo(clsDB.DbCon);

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "    INSERT INTO KOSMOS_OCS.OCS_OPSCHE ";
                        SQL = SQL + ComNum.VBLF + " (  OPDATE,          PANO,        SNAME,       AGE,         SEX,              ";
                        SQL = SQL + ComNum.VBLF + "    GBIO,            WARDCODE,    ROOMCODE,    DEPTCODE,                      ";
                        SQL = SQL + ComNum.VBLF + "    OPSTAFF,         OPROOM,      OPSEQ,       OPTIME,                        ";
                        SQL = SQL + ComNum.VBLF + "    PREDIAGNOSIS,    OPILL,       ANDRCODE,    ANESTH,      POSITION,         ";
                        SQL = SQL + ComNum.VBLF + "    IDNO,            REMARK,      REFERENCE,   GBMAGAM,                       ";
                        SQL = SQL + ComNum.VBLF + "    WRTNO,           GBDEL,       ENTDATE,     OPTIME_NEW,  OPRE, GBER, GBANGIO, GBDAY, LEFTRIGHT, GBANTI , GBDRG ) ";
                        SQL = SQL + ComNum.VBLF + "    VALUES                                                       ";
                        SQL = SQL + ComNum.VBLF + "  ( TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),  ";
                        SQL = SQL + ComNum.VBLF + "   '" + txtPtno.Text.Trim() + "','" + strSname.Trim() + "'," + Convert.ToString(nAge) + ",  ";
                        SQL = SQL + ComNum.VBLF + "   '" + strSex.Trim() + "','" + strGbio.Trim() + "', '" + cboWardCode.Text.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + "   '" + VB.Left(cboRoomCode.Text.Trim(), 1) + "', '" + cboDeptCode.Text.Trim() + "', '" + VB.Right(cboOpStaff.Text.Trim(), 4) + "', ";
                        SQL = SQL + ComNum.VBLF + "   '" + VB.Left(cboOpRoom.Text.Trim(), 1) + "','" + txtOpSeq.Text.Trim() + "', '" + txtOpTime1.Text.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + "   '" + txtPreDiagnosis.Text.Trim() + "','" + txtOpIll.Text.Trim() + "','" + strAnDrCode.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + "   '" + strAnesth.Trim() + "','" + strPosition.Trim() + "','" + ComFunc.LPAD(clsType.User.Sabun, 5, "0") + "','" + txtRemark.Text.Trim() + "',";
                        SQL = SQL + ComNum.VBLF + "   '" + txtRefer.Text.Trim() + "', ' '," + FnWRTNO + ",' ', SYSDATE, '','";
                        SQL = SQL + strOpRe + "','" + strGbEr + "','" + strGbAngio + "','" + strGbDay + "', '" + VB.Left(cboLR.Text.Trim(), 1) + "' ,'" + strGBANTI + "' , '" + strGbDRG + "'   ";
                        SQL = SQL + ComNum.VBLF + "  ) ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            dt.Dispose();
                            dt = null;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 INSERT중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        //GoSub ORAN_MASTER_INSERT
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "    INSERT INTO KOSMOS_PMPA.ORAN_MASTER                          ";
                        SQL = SQL + ComNum.VBLF + " (  OPDATE,          PANO,        SNAME,       BI,  AGE,  SEX,   ";
                        SQL = SQL + ComNum.VBLF + "    IPDOPD,          WARDCODE,    ROOMCODE,    DEPTCODE, DRCODE, ";
                        SQL = SQL + ComNum.VBLF + "    OPTIMEFROM,      OPROOM,      DIAGNOSIS,   OPTITLE,  OPDOCT1,";
                        SQL = SQL + ComNum.VBLF + "    WRTNO,           OPRE,        GBER,        GBANGIO, GBDAY) ";
                        SQL = SQL + ComNum.VBLF + "    VALUES                                                       ";
                        SQL = SQL + ComNum.VBLF + "  ( TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),  ";
                        SQL = SQL + ComNum.VBLF + "   '" + txtPtno.Text.Trim() + "','" + strSname.Trim() + "', '" + strBi + "', " + Convert.ToString(nAge) + ", ";
                        SQL = SQL + ComNum.VBLF + "   '" + strSex.Trim() + "','" + strGbio.Trim() + "', '" + cboWardCode.Text.Trim() + "',";
                        SQL = SQL + ComNum.VBLF + "   '" + VB.Left(cboRoomCode.Text.Trim(), 1) + "', '" + cboDeptCode.Text.Trim() + "', '" + strDrCode + "',";
                        SQL = SQL + ComNum.VBLF + "   '" + VB.Left(txtOpTime1.Text.Trim(), 4) + "', '" + VB.Left(cboOpRoom.Text.Trim(), 1) + "', ";
                        SQL = SQL + ComNum.VBLF + "   '" + txtPreDiagnosis.Text.Trim() + "', '" + txtOpIll.Text.Trim() + "', '";
                        SQL = SQL + VB.Left(cboOpStaff.Text.Trim(), 10) + "'," + FnWRTNO;

                        if (chkOpRe.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + " ,  '1', ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " , '0', ";
                        }

                        if (chkGBER.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "  '*',";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " '',";
                        }

                        SQL = SQL + ComNum.VBLF + "'" + strGbAngio + "','" + strGbDay + "') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            dt.Dispose();
                            dt = null;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 INSERT중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        } 
                        #endregion
                    }
                    else
                    {
                        //'이미 오더를 입력하였으면 삭제가 않되도록 처리함(2006.3.24)
                        #region //기존 자료 있는지 체크
                        SQL = "SELECT SUM(QTY) QTY FROM KOSMOS_PMPA.ORAN_SLIP ";
                        SQL = SQL + ComNum.VBLF + "WHERE WRTNO=" + FnWRTNO + " ";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            if (VB.Val(dt.Rows[0]["QTY"].ToString().Trim()) > 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                dt.Dispose();
                                dt = null;
                                ComFunc.MsgBox("수술이 완료 되었거나 입력된 처방이 있어 수정이 불가능함", "확인");
                                Yeyak_Display();
                                return;
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        //'HISTORY
                        SQL = " INSERT INTO KOSMOS_OCS.OCS_OPSCHE_HIS  ( OPDATE,PANO,SNAME,AGE,SEX,GBIO,WARDCODE,ROOMCODE,DEPTCODE,OPSTAFF,OPROOM,OPSEQ,OPTIME, ";
                        SQL = SQL + ComNum.VBLF + "   PREDIAGNOSIS,OPILL,ANDRCODE,ANESTH,POSITION,IDNO,REMARK,REFERENCE,GBMAGAM,GBDEL,ENTDATE,WRTNO, ";
                        SQL = SQL + ComNum.VBLF + "   OPTIME_NEW,OPRE,GBER,GBANGIO,GBDAY,LEFTRIGHT,MDATE,DDATE, CHSABUN, CHDATE ) ";
                        SQL = SQL + ComNum.VBLF + "   SELECT OPDATE,PANO,SNAME,AGE,SEX,GBIO,WARDCODE,ROOMCODE,DEPTCODE,OPSTAFF,OPROOM,OPSEQ,OPTIME, ";
                        SQL = SQL + ComNum.VBLF + "   PREDIAGNOSIS,OPILL,ANDRCODE,ANESTH,POSITION,IDNO,REMARK,REFERENCE,GBMAGAM,GBDEL,ENTDATE,WRTNO, ";
                        SQL = SQL + ComNum.VBLF + "   OPTIME_NEW,OPRE,GBER,GBANGIO,GBDAY,LEFTRIGHT,MDATE,DDATE, '" + clsType.User.Sabun + "', SYSDATE  ";
                        SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_OPSCHE ";
                        SQL = SQL + ComNum.VBLF + "  WHERE WRTNO = '" + FnWRTNO + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            dt.Dispose();
                            dt = null;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 INSERT중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        //    GoSub Data_Update
                        SQL = " UPDATE KOSMOS_OCS.OCS_OPSCHE SET         ";
                        SQL = SQL + ComNum.VBLF + " OPDATE       = TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + " SNAME        = '" + strSname.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " AGE          = " + Convert.ToString(nAge) + ", ";
                        SQL = SQL + ComNum.VBLF + " SEX          = '" + strSex.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " GBIO         = '" + strGbio.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " WARDCODE     = '" + cboWardCode.Text.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " ROOMCODE     = '" + VB.Left(cboRoomCode.Text.Trim(), 1) + "', ";
                        SQL = SQL + ComNum.VBLF + " DEPTCODE     = '" + cboDeptCode.Text.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " OPSTAFF      = '" + VB.Right(cboOpStaff.Text.Trim(), 4) + "',  ";
                        SQL = SQL + ComNum.VBLF + " OPROOM       = '" + VB.Left(cboOpRoom.Text.Trim(), 1) + "',  ";
                        SQL = SQL + ComNum.VBLF + " OPSEQ        = '" + txtOpSeq.Text.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " OPTIME       = '" + txtOpTime1.Text.Trim() + "',   ";
                        SQL = SQL + ComNum.VBLF + " PREDIAGNOSIS = '" + txtPreDiagnosis.Text.Trim() + "',  ";
                        SQL = SQL + ComNum.VBLF + " OPILL        = '" + txtOpIll.Text.Trim() + "',  ";
                        SQL = SQL + ComNum.VBLF + " ANDRCODE     = '" + strAnDrCode.Trim() + "',   ";
                        SQL = SQL + ComNum.VBLF + " ANESTH       = '" + strAnesth.Trim() + "',   ";
                        SQL = SQL + ComNum.VBLF + " POSITION     = '" + strPosition.Trim() + "',   ";
                        SQL = SQL + ComNum.VBLF + " IDNO         = '" + ComFunc.LPAD(clsType.User.Sabun, 5, "0") + "',  ";
                        SQL = SQL + ComNum.VBLF + " REMARK       = '" + txtRemark.Text.Trim() + "',  ";
                        SQL = SQL + ComNum.VBLF + " REFERENCE    = '" + txtRefer.Text.Trim() + "', ";

                        ////'允(2006-09-28) 마취과 요청  마감된 환자를 수술일자를 변경시 마감FLAG NULL 처리 요청;
                        //if (dtpOpDate.Value > NowDate)
                        //{
                        //    SQL = SQL + ComNum.VBLF + " GBMAGAM      = '' ,";
                        //}

                        SQL = SQL + ComNum.VBLF + " OPRE         = '" + strOpRe + "', ";
                        SQL = SQL + ComNum.VBLF + " GBER         = '" + strGbEr + "', ";
                        SQL = SQL + ComNum.VBLF + " GBDAY        = '" + strGbDay + "', ";
                        SQL = SQL + ComNum.VBLF + " GBANGIO      = '" + strGbAngio + "' , ";
                        SQL = SQL + ComNum.VBLF + " LEFTRIGHT    = '" + VB.Left(cboLR.Text.Trim(), 1) + "', ";
                        SQL = SQL + ComNum.VBLF + " GBANTI       = '" + strGBANTI + "', ";
                        SQL = SQL + ComNum.VBLF + " GBDRG        = '" + strGbDRG + "' ";
                        SQL = SQL + ComNum.VBLF + " WHERE WRTNO  = " + FnWRTNO + " ";


                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            dt.Dispose();
                            dt = null;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 UPDATE중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        SQL = " UPDATE KOSMOS_PMPA.ORAN_MASTER SET         ";
                        SQL = SQL + ComNum.VBLF + " OPDATE       = TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD'),";
                        SQL = SQL + ComNum.VBLF + " SNAME        = '" + strSname.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " BI           = '" + strBi.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " AGE          = " + Convert.ToString(nAge) + ", ";
                        SQL = SQL + ComNum.VBLF + " SEX          = '" + strSex.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " IPDOPD       = '" + strGbio.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " WARDCODE     = '" + cboWardCode.Text.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " ROOMCODE     = '" + VB.Left(cboRoomCode.Text.Trim(), 1) + "', ";
                        SQL = SQL + ComNum.VBLF + " DEPTCODE     = '" + cboDeptCode.Text.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " DRCODE       = '" + strDrCode.Trim() + "', ";
                        SQL = SQL + ComNum.VBLF + " OPTIMEFROM   = '" + VB.Left(txtOpTime1.Text.Trim(), 4) + "',   ";
                        SQL = SQL + ComNum.VBLF + " OPROOM       = '" + VB.Left(cboOpRoom.Text.Trim(), 1) + "',  ";
                        SQL = SQL + ComNum.VBLF + " DIAGNOSIS    = '" + txtPreDiagnosis.Text.Trim() + "',  ";
                        SQL = SQL + ComNum.VBLF + " OPTITLE      = '" + txtOpIll.Text.Trim() + "',  ";
                        SQL = SQL + ComNum.VBLF + " OPDOCT1      = '" + VB.Left(cboOpStaff.Text.Trim(), 10) + "', ";
                        SQL = SQL + ComNum.VBLF + " GBANGIO      = '" + strGbAngio + "', ";
                        SQL = SQL + ComNum.VBLF + " GBDAY        = '" + strGbDay + "', ";

                        if (chkOpRe.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + " OPRE = '1', ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " OPRE = '0', ";
                        }

                        if (chkGBER.Checked == true)
                        {
                            SQL = SQL + ComNum.VBLF + "  GBER = '*' ";
                        }
                        else
                        {
                            SQL = SQL + ComNum.VBLF + " GBER = '' ";
                        }

                        SQL = SQL + ComNum.VBLF + " WHERE WRTNO = " + FnWRTNO + " ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            dt.Dispose();
                            dt = null;
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 UPDATE중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        } 
                        #endregion
                    }

                    //ORAN_MASTER == KOSMOS_OCS.OCS_OPSCHE 싱크 맞추기 위해서 넣어둠. 

                    SQL = "";
                    SQL = "SELECT TO_CHAR(OPDATE,'YYYY-MM-DD') AS OPDATE, PANO, SNAME, AGE, SEX, GBIO, WARDCODE, ROOMCODE, DEPTCODE,        ";
                    SQL = SQL + ComNum.VBLF + "RTRIM(OPSTAFF)AS OPSTAFF, PREDIAGNOSIS, OPILL, ANESTH, POSITION, REMARK, REFERENCE, OPRE, GBER       ";
                    SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_OPSCHE       ";
                    SQL = SQL + ComNum.VBLF + "WHERE WRTNO = " + Convert.ToString(FnWRTNO) + "     ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        #region //ORAN_MASTER 자료 갱신
                        SQL = "";
                        SQL = "UPDATE ORAN_MASTER";
                        SQL = SQL + ComNum.VBLF + "SET      ";
                        SQL = SQL + ComNum.VBLF + "OPDATE = TO_DATE('" + dt.Rows[0]["OPDATE"].ToString().Trim() + "', 'YYYY-MM-DD'),      ";
                        SQL = SQL + ComNum.VBLF + "PANO = '" + dt.Rows[0]["PANO"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "SNAME = '" + dt.Rows[0]["SNAME"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "AGE = " + dt.Rows[0]["AGE"].ToString().Trim() + ",      ";
                        SQL = SQL + ComNum.VBLF + "SEX = '" + dt.Rows[0]["SEX"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "IPDOPD = '" + dt.Rows[0]["GBIO"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "WARDCODE = '" + dt.Rows[0]["WARDCODE"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "ROOMCODE = '" + dt.Rows[0]["ROOMCODE"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "DEPTCODE = '" + dt.Rows[0]["DEPTCODE"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "DRCODE = '" + dt.Rows[0]["OPSTAFF"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "DIAGNOSIS = '" + dt.Rows[0]["PREDIAGNOSIS"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "OPTITLE = '" + dt.Rows[0]["OPILL"].ToString().Trim() + "',      ";
                        //SQL = SQL + ComNum.VBLF + "ANGBN = '" + dt.Rows[0]["ANESTH"].ToString().Trim() + "',      ";
                        //SQL = SQL + ComNum.VBLF + "ANDOCT1 = '" + dt.Rows[0]["ANESTH"].ToString().Trim() + "',      "; 마취의사 성명을 기존에 안 넣어 주고 있어서 보류
                        SQL = SQL + ComNum.VBLF + "OPPOSITION = '" + dt.Rows[0]["POSITION"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "SPADEWORK = '" + dt.Rows[0]["REMARK"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "PTREMARK = '" + dt.Rows[0]["REFERENCE"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "OPRE = '" + dt.Rows[0]["OPRE"].ToString().Trim() + "',      ";
                        SQL = SQL + ComNum.VBLF + "GBER = '" + dt.Rows[0]["GBER"].ToString().Trim() + "'      ";
                        SQL = SQL + ComNum.VBLF + "WHERE WRTNO = " + Convert.ToString(FnWRTNO) + "      ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                            Cursor.Current = Cursors.Default;
                            return;
                        } 
                        #endregion
                    }

                    dt.Dispose();
                    dt = null;

                    clsDB.setCommitTran(clsDB.DbCon);

                    SCREEN_CLEAR();
                    Yeyak_Display();

                    txtPtno.Text = "";
                    Cursor.Current = Cursors.Default;
                    #endregion
                }

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }
        
        private bool Data_Check()
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수


            if (clsOpMain.GstrBuCode != clsOpMain.BUSE_SUSUL
                && clsOpMain.GstrBuCode != clsOpMain.BUSE_MARCH)
            {
                //'允(2005-11-14) 수술방요청으로 당일이전은 등록불가
                if (dtpOpDate.Value < NowDate)
                {
                    ComFunc.MsgBox("당일이전은 수술예약등록을 할수 없습니다.", "확인");
                    dtpOpDate.Focus();
                    return false;
                }
            }

            if (chkGS.Checked == false)
            {
                if (chkGBER.Checked == true && dtpOpDate.Value != NowDate)
                {
                    ComFunc.MsgBox("응급수술은 당일수술예약만 가능합니다.", "확인");
                    return false;
                }
            }            

            txtPreDiagnosis.Text = txtPreDiagnosis.Text.Trim();

            if (txtPreDiagnosis.Text.Length > 1000)
            {
                ComFunc.MsgBox("진단명이 너무 길어 등록이 불가능 합니다.", "오류");
                return false;
            }

            txtOpIll.Text = txtOpIll.Text.Trim();

            if (txtOpIll.Text.Length > 1000)
            {
                ComFunc.MsgBox("수술명이 너무 길어 등록이 불가능 합니다.", "오류");
                return false;
            }

            if (txtPtno.Text.Trim() == "")
            {
                ComFunc.MsgBox("환자번호를 입력하세요!", "확인");
                txtPtno.Focus();
                return false;
            }

            if (dtpOpDate.Value.ToString("yyyy-MM-dd") == "9998-12-31")
            {
                ComFunc.MsgBox("수술일자를 입력하세요!", "확인");
                dtpOpDate.Focus();
                return false;
            }

            if (cboDeptCode.SelectedIndex < 0)
            {
                ComFunc.MsgBox("수술과를 입력하세요!", "확인");
                cboDeptCode.Focus();
                return false;
            }
           

            //외과 스케쥴이 아니라면
            if (chkGS.Checked == false)
            {

                if (cboPosition.Text.Trim() == "")
                {
                    ComFunc.MsgBox("Position을 입력하세요!", "확인");
                    cboPosition.Focus();
                    return false;
                }

                if (cboOpRoom.Text.Trim() == "")
                {
                    ComFunc.MsgBox("수술실을 입력하세요!", "확인");
                    cboOpRoom.Focus();
                    return false;
                }

                if (txtOpSeq.Text.Trim() == "")
                {
                    ComFunc.MsgBox("수술순번을 입력하세요!", "확인");
                    txtOpSeq.Focus();
                    return false;
                }

                if (cboOpStaff.Text.Trim() == "")
                {
                    ComFunc.MsgBox("수술의사를 입력하세요!", "확인");
                    cboOpStaff.Focus();
                    return false;
                }

                if (cboLR.Text.Trim() == "")
                {
                    ComFunc.MsgBox("위치를 입력하세요!", "확인");
                    cboLR.Focus();
                    return false;
                }

                if (VB.Left(cboOpRoom.Text.Trim(), 1) == "G")
                {
                    ComFunc.MsgBox("혈관조영실은 수술실구분 A 또는 B를 선택 해주세요", "확인");
                    cboOpRoom.Focus();
                    return false;
                }

                if (VB.Left(cboOpRoom.Text.Trim(), 1) == "G" || VB.Left(cboOpRoom.Text.Trim(), 1) == "A" || VB.Left(cboOpRoom.Text.Trim(), 1) == "B")
                {
                    if (chkAngio.Checked == false)
                    {
                        ComFunc.MsgBox("수술실 선택오류(수술실코드 또는 Angio 선택)", "확인");
                        return false;
                    }
                }
                else
                {
                    if (chkAngio.Checked == true)
                    {
                        ComFunc.MsgBox("수술실 선택오류(수술실코드 또는 Angio 선택)", "확인");
                        return false;
                    }
                }

                if (chkAngio.Checked == false)
                {
                    if (chkGBER.Checked == false && dtpOpDate.Value == NowDate)
                    {
                        if (clsOpMain.GstrBuCode != clsOpMain.BUSE_SUSUL
                            && clsOpMain.GstrBuCode != clsOpMain.BUSE_MARCH)
                        {
                            ComFunc.MsgBox("당일수술예약등록은 응급수술을 선택 해주세요", "확인");
                            chkGBER.Focus();
                            return false;
                        }
                        else
                        {
                            if (ComFunc.MsgBoxQ("당일수술예약등록입니다. 응급 환자가 아닙니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
                            {
                                ComFunc.MsgBox("응급수술을 선택 해주세요", "확인");
                                chkGBER.Focus();
                                return false;
                            }
                        }
                    }
                }
            }
             
            //'마감을 하였는지 Check
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (clsOpMain.GstrBuCode != clsOpMain.BUSE_SUSUL
                   && clsOpMain.GstrBuCode != clsOpMain.BUSE_MARCH)
                {

                    if (chkAngio.Checked == false && VB.Left(cboOpRoom.Text.Trim(), 1) != "N")
                    {
                        if (clsOpMain.GstrBuCode != clsOpMain.BUSE_SUSUL
                        && clsOpMain.GstrBuCode != clsOpMain.BUSE_MARCH
                        && clsOpMain.GstrBuCode != "011106"
                        && clsType.User.Sabun != "4349")
                        {
                            SQL = "";
                            SQL = " SELECT COUNT(*) CNT FROM KOSMOS_OCS.OCS_OPSCHE ";
                            SQL = SQL + ComNum.VBLF + " WHERE  OPDATE  = TO_DATE('" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                            SQL = SQL + ComNum.VBLF + " AND    GBMAGAM = '*' ";
                            SQL = SQL + ComNum.VBLF + " AND  ( GBDEL <> '*' OR GBDEL IS NULL )";

                            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            }

                            if (dt.Rows.Count > 0)
                            {
                                if (VB.Val(dt.Rows[0]["CNT"].ToString().Trim()) > 0)
                                {
                                    dt.Dispose();
                                    dt = null;
                                    Cursor.Current = Cursors.Default;

                                    ComFunc.MsgBox("[" + dtpOpDate.Value.ToString("yyyy-MM-dd") + "]의 스케쥴이 이미 마감되었습니다." + ComNum.VBLF + ComNum.VBLF
                                    + "작업을 원하시면 마취과로 연락바랍니다.", "확인");

                                    return false;
                                }
                            }

                            dt.Dispose();
                            dt = null;
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        private void btnSaveH_Click(object sender, EventArgs e)
        {
            if (txtHelp.Text.Trim() == "")
            {
                ComFunc.MsgBox("명칭을 입력해주세요", "확인");
                return;
            }

            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return; //권한 확인
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            string strGbn = "";
            string strRemark = "";

            if (lblHelp.Text == "진단명")
            {
                strGbn = "1";
                strRemark = txtPreDiagnosis.Text;
            }
            else if (lblHelp.Text == "수술명")
            {
                strGbn = "2";
                strRemark = txtOpIll.Text;
            }
            else
            {
                strGbn = "3";
                strRemark = txtRemark.Text;
            }

            if (strRemark.Length > 1000)
            {
                ComFunc.MsgBox("저장 할 상용구의 내용의 길이는 1000자리 입니다.", "확인");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;
            clsTrans TRS = new clsTrans();
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " DELETE   KOSMOS_OCS.OCS_REMARKOP  ";
                SQL = SQL + ComNum.VBLF + " WHERE SABUN = '" + clsType.User.Sabun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND REMARKCODE = '" + txtHelp.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND GBBUN = '" + strGbn + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 Update중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                SQL = " INSERT INTO  KOSMOS_OCS.OCS_REMARKOP (SABUN, GBBUN, REMARKCODE, REMARK) VALUES ";
                SQL = SQL + ComNum.VBLF + "  ( '" + clsType.User.Sabun + "' , '" + strGbn + "', '" + txtHelp.Text.Trim() + "' , '" + strRemark + "' ) ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("DB에 INSERT중 오류가 발생함.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                READ_CREMARK();
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            string strSname = "";
            int nAge = 0;
            string strSex = "";

            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            ssView1_Sheet1.RowCount = 0;

            strSname = txtSName.Text.Trim();

            if (strSname == "" && optJob1.Checked == true)
            {
                ComFunc.MsgBox("성명이 공란입니다.", "오류");
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (optJob0.Checked == true) //재원자
                {
                    SQL = " SELECT A.PANO,B.SNAME,A.AGE,A.SEX,B.TEL,B.PNAME,A.WARDCODE,";
                    SQL = SQL + ComNum.VBLF + " A.ROOMCODE,A.DEPTCODE ";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER A,KOSMOS_PMPA.BAS_PATIENT B ";
                    SQL = SQL + ComNum.VBLF + "WHERE A.GBSTS IN ('0','2')";
                    SQL = SQL + ComNum.VBLF + "  AND A.JDATE = TO_DATE('1900-01-01','YYYY-MM-DD')";
                    SQL = SQL + ComNum.VBLF + "  AND A.PANO < '90000000' ";
                    SQL = SQL + ComNum.VBLF + "  AND A.PANO=B.PANO(+) ";

                    if (cboViewDept.Text.Trim() != "전체")
                    {
                        SQL = SQL + ComNum.VBLF + " AND A.DEPTCODE='" + cboViewDept.Text.Trim() + "' ";
                    }

                    SQL = SQL + ComNum.VBLF + " ORDER BY A.ROOMCODE,A.SNAME,A.PANO ";

                }
                else
                {
                    SQL = "SELECT PANO,SNAME,SEX,JUMIN1,JUMIN2,TEL,PNAME,DEPTCODE ,'' AS WARDCODE, '' AS ROOMCODE";
                    SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_PATIENT ";
                    SQL = SQL + ComNum.VBLF + "WHERE SNAME='" + strSname + "' ";
                    SQL = SQL + ComNum.VBLF + " ORDER BY PANO DESC ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssView1_Sheet1.RowCount = dt.Rows.Count;
                    ssView1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {

                        ssView1_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        strSex = dt.Rows[i]["SEX"].ToString().Trim();

                        clsAES.Read_Jumin_AES(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim());

                        if (optJob0.Checked == true) //'재원자
                        {
                            nAge = Convert.ToInt32(VB.Val(dt.Rows[i]["AGE"].ToString().Trim()));
                        }
                        else
                        {
                            nAge = ComFunc.AgeCalc(clsDB.DbCon, dt.Rows[i]["JUMIN1"].ToString().Trim() + clsAES.GstrAesJumin2);
                        }

                        ssView1_Sheet1.Cells[i, 2].Text = nAge + "/" + strSex;
                        ssView1_Sheet1.Cells[i, 3].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 4].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView1_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();

                        SQL = "";
                        SQL = " SELECT VDRL, HCV_IGG, HBS_AG  FROM KOSMOS_OCS.EXAM_INFECTMASTER ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["VDRL"].ToString().Trim() != ""
                            || dt1.Rows[0]["HCV_IGG"].ToString().Trim() != ""
                            || dt1.Rows[0]["HBS_AG"].ToString().Trim() != "")
                            {
                                ssView1_Sheet1.Rows.Get(i).BackColor = Color.FromArgb(230, 220, 255);
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void cboOpRoom1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Yeyak_Display();
        }

        private void cboRDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            Yeyak_Display();
        }

        private void btnClearNew_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            SCREEN_CLEAR();

            try
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                    return; //권한 확인

                //'작업자 사번으로 수술과 선택
                SQL = "";
                SQL = "SELECT DEPTCODE FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + "WHERE SABUN=" + clsType.User.Sabun + " ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrDrDept = dt.Rows[0]["DEPTCODE"].ToString().Trim();
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
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
            }

            //'의사이면 해당과만 등록/수정이 가능함
            cboViewDept.SelectedIndex = 0;

            if (FstrDrDept != "")
            {
                ComFunc.ComboFind(cboDeptCode, "T", 0, FstrDrDept);
                cboDeptCode.Enabled = false;
                cboViewDept.SelectedIndex = cboDeptCode.SelectedIndex + 1;
            }

            //FstrJob = "신규";

            if (clsOpMain.GstrBuCode == clsOpMain.BUSE_SUSUL
             || clsOpMain.GstrBuCode == clsOpMain.BUSE_MARCH)
            {
                dtpOpDate.Value = NowDate;
            }
            else
            {
                dtpOpDate.Value = NowDate.AddDays(1);
            }



            btnSave.Enabled = true;
            btnDelete.Enabled = true;
            txtPtno.Enabled = true;
            txtPtno.Focus();
            FnWRTNO = 0;
        }

        private void optJob_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked == true)
            {
                if (optJob0.Checked == true)
                {
                    lblView.Text = "과";
                    cboViewDept.Visible = true;
                    txtSName.Text = "";
                    txtSName.Visible = false;
                }
                else
                {
                    lblView.Text = "성명";
                    cboViewDept.Visible = false;
                    txtSName.Text = "";
                    txtSName.Visible = true;
                }
            }
        }

        private void lblAnti_Click(object sender, EventArgs e)
        {
            //Frm오더내역보기
            FrmOrderDrugInfoView frm = new FrmOrderDrugInfoView(txtPtno.Text.Trim());
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.TopMost = true;
            frm.ShowDialog();

        }

        private void ssView1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView1_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            SCREEN_CLEAR();

            //FstrJob = "신규";
            if (clsOpMain.GstrBuCode == clsOpMain.BUSE_SUSUL
             || clsOpMain.GstrBuCode == clsOpMain.BUSE_MARCH)
            {
                dtpOpDate.Value = NowDate;
            }
            else
            {
                dtpOpDate.Value = NowDate.AddDays(1);
            }
            txtPtno.Text = ssView1_Sheet1.Cells[e.Row, 0].Text.Trim();
            ComFunc.ComboFind(cboDeptCode, "T", 0, ssView1_Sheet1.Cells[e.Row, 5].Text.Trim());

            txtPtno.Enabled = false;
            txtPtnoSearch();
            dtpOpDate.Focus();
        }

        private void ssView2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

            SCREEN_CLEAR();

            if (ssView2_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            FstrName = ssView2_Sheet1.Cells[e.Row, 3].Text.Trim();

            if (VB.Split(ssView2_Sheet1.Cells[e.Row, 6].Text.Trim(), "/").Length == 2)
            {
                FstrSex = VB.Split(ssView2_Sheet1.Cells[e.Row, 6].Text.Trim(), "/")[1];
                FnAge = Convert.ToInt32(VB.Val(VB.Split(ssView2_Sheet1.Cells[e.Row, 6].Text.Trim(), "/")[0]));
            }

            txtPtno.Text = ssView2_Sheet1.Cells[e.Row, 7].Text.Trim();
            FnWRTNO = Convert.ToInt32((ssView2_Sheet1.Cells[e.Row, 8].Text.Trim()));

            gGSROWID = ssView2_Sheet1.Cells[e.Row, 11].Text.Trim(); //ROWID 2019-02-13

            ComFunc.ComboFind(cboDeptCode, "T", 0, ssView2_Sheet1.Cells[e.Row, 4].Text.Trim());

            Screen_Display();
        }

        private void ssViewH_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssViewH_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                return;
            }

            txtHelp.Text = ssViewH_Sheet1.Cells[e.Row, 0].Text.Trim();

            if (lblHelp.Text.Trim() == "진단명")
            {
                txtPreDiagnosis.Text = ssViewH_Sheet1.Cells[e.Row, 0].Text.Trim();
            }
            else if (lblHelp.Text.Trim() == "수술명")
            {
                txtOpIll.Text = ssViewH_Sheet1.Cells[e.Row, 0].Text.Trim();
            }
            else
            {
                txtRemark.Text = ssViewH_Sheet1.Cells[e.Row, 0].Text.Trim();
            }
        }

        private void txtOpSeq_Leave(object sender, EventArgs e)
        {
            newOpSeq = Convert.ToInt32(VB.Val(txtOpSeq.Text.Trim()));
        }

        private void txtPtno_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPtnoSearch();
            }
        }

        private void txtPtnoSearch()
        {
            txtPtno.Text = txtPtno.Text.Trim();

            if (txtPtno.Text == "")
            {
                return;
            }

            txtPtno.Text = ComFunc.LPAD(txtPtno.Text, 8, "0");

            Pano_Display(txtPtno.Text);
        }

        private void txtPtno_Enter(object sender, EventArgs e)
        {
            txtPtno.SelectAll();
        }

        private void txtOpTime1_Enter(object sender, EventArgs e)
        {
            txtOpTime1.SelectAll();
        }

        private void txtOpSeq_Enter(object sender, EventArgs e)
        {
            txtOpSeq.SelectAll();
        }

        private void txtPreDiagnosis_Enter(object sender, EventArgs e)
        {
            txtPreDiagnosis.SelectAll();
        }

        private void txtOpIll_Enter(object sender, EventArgs e)
        {
            txtOpIll.SelectAll();
        }

        private void txtRemark_Enter(object sender, EventArgs e)
        {
            txtRemark.SelectAll();
        }

        private void txtRefer_Enter(object sender, EventArgs e)
        {
            txtRefer.SelectAll();
        }

        private void txtSName_Enter(object sender, EventArgs e)
        {
            txtSName.SelectAll();
        }

        private void txtHelp_Enter(object sender, EventArgs e)
        {
            txtHelp.SelectAll();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch_Click(null, null);
            }
        }

        private void dtpOpDate1_ValueChanged(object sender, EventArgs e)
        {
            Yeyak_Display();
        }
        
        #region //DB 저장, 갱신 
        string del_EXAM_ABR_SCHEDULE(PsmhDb pDbCon, string argROWID, ref int intRowAffected, bool bBack = true)
        {
            string SqlErr = string.Empty;

            if (bBack) //백업실행
            {
                SQL = "";
                SQL += "  INSERT INTO " + ComNum.DB_MED + "EXAM_ABR_SCHEDULE_HISTORY        \r\n";
                SQL += "   (EXDATE,PANO,EXAMNAME, WRITEDATE, WRITESABUN                     \r\n";
                SQL += "    ,GUBUN,OPTIME,DEPTCODE,OPSTAFF,AGE,SEX)                         \r\n";
                SQL += "  SELECT EXDATE,PANO,EXAMNAME, WRITEDATE, WRITESABUN                \r\n";
                SQL += "    ,GUBUN,OPTIME,DEPTCODE,OPSTAFF,AGE,SEX                          \r\n";
                SQL += "   FROM " + ComNum.DB_MED + "EXAM_ABR_SCHEDULE                      \r\n";
                SQL += "   WHERE 1=1                                                        \r\n";
                SQL += "    AND ROWID = '" + argROWID + "'                                  \r\n";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);
            }

            SQL = "";
            SQL += "  DELETE FROM " + ComNum.DB_MED + "EXAM_ABR_SCHEDULE                    \r\n";
            SQL += "   WHERE 1=1                                                            \r\n";
            SQL += "    AND ROWID = '" + argROWID + "'                                      \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        string ins_EXAM_ABR_SCHEDULE(PsmhDb pDbCon, string argPart, string argPano, string argExDate, string argExName, long argJobSabun, string argOpTime,string argDept,string argSTAFF,int argAge, string argSex, string argDia,string argOpName,string argIO, ref int intRowAffected)
        {
            string SqlErr = string.Empty;

            SQL = "";
            SQL += "  INSERT INTO " + ComNum.DB_MED + "EXAM_ABR_SCHEDULE                    \r\n";
            SQL += "   (EXDATE,PANO,EXAMNAME,Gubun, WRITEDATE, WRITESABUN                   \r\n";
            SQL += "    ,OPTIME,DEPTCODE,OPSTAFF,AGE,SEX, PREDIAGNOSIS, OPILL,GBIO  )       \r\n";
            SQL += "   VALUES (                                                             \r\n";
            SQL += "   TO_DATE('" + argExDate + "','YYYY-MM-DD HH24:MI')                    \r\n";
            SQL += "     ,'" + argPano + "'                                                 \r\n";
            SQL += "     ,'" + argExName + "'                                               \r\n";
            SQL += "     ,'" + argPart + "'                                                 \r\n";
            SQL += "     ,SYSDATE                                                           \r\n";
            SQL += "     ," + argJobSabun + "                                               \r\n";
            SQL += "     ,'" + argOpTime + "'                                               \r\n";
            SQL += "     ,'" + argDept + "'                                                 \r\n";
            SQL += "     ,'" + argSTAFF + "'                                                \r\n";
            SQL += "     ," + argAge + "                                                    \r\n";
            SQL += "     ,'" + argSex + "'                                                  \r\n";
            SQL += "     ,'" + argDia + "'                                                  \r\n";
            SQL += "     ,'" + argOpName + "'                                               \r\n";
            SQL += "     ,'" + argIO + "'                                                   \r\n";
            SQL += "         )                                                              \r\n";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            return SqlErr;
        }

        #endregion

        void panel12_Paint(object sender, PaintEventArgs e)
        {
            ControlPaint.DrawBorder(e.Graphics, panel12.DisplayRectangle, Color.Red, ButtonBorderStyle.Inset); 
        }
    }
}

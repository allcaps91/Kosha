using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

/// <summary>
/// Description : 예방접종 기본 인적사항 등록관리
/// Author : 박병규
/// Create Date : 2017.06.29
/// </summary>
/// <history>
/// </history>

namespace ComLibB
{
    public partial class frmPmpaMasterVaccine : Form
    {
        ComQuery CQ = null;
        ComFunc CF = null;

        string FstrPtno = string.Empty;
        string Fvalue = string.Empty;
        bool bSMS = false;


        public frmPmpaMasterVaccine(string ArgPtno)
        {
            InitializeComponent();

            FstrPtno = ArgPtno;
            setEvent();
        }

        public frmPmpaMasterVaccine(string ArgPtno,bool argSMS)
        {
            InitializeComponent();

            FstrPtno = ArgPtno;
            bSMS = argSMS;
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eForm_Load);

            #region KeyPress 이벤트
            this.txtPtno.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtJumin1.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtJumin2.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtSname.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtBirth.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtGubun.KeyPress += new KeyPressEventHandler(eControl_Keypress);

            this.txtPJumin1.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtPJumin2.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtMSname.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.cboMGwange.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtTel1.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtTel2.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtTel3.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtHp1.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtHp2.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtHp3.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtPostCode.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtJuso2.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.txtEmail.KeyPress += new KeyPressEventHandler(eControl_Keypress);
            this.cboUse.KeyPress += new KeyPressEventHandler(eControl_Keypress);



            #endregion
        }
        
        //KeyPress 이벤트
        private void eControl_Keypress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPtno && e.KeyChar == (Char)13) { txtJumin1.Focus(); }
            if (sender == this.txtJumin1 && e.KeyChar == (Char)13) { txtJumin2.Focus(); }
            if (sender == this.txtJumin2 && e.KeyChar == (Char)13) { txtSname.Focus(); }
            if (sender == this.txtSname && e.KeyChar == (Char)13) { txtBirth.Focus(); }
            if (sender == this.txtBirth && e.KeyChar == (Char)13) { txtGubun.Focus(); }
            if (sender == this.txtGubun && e.KeyChar == (Char)13) { txtPJumin1.Focus(); }

            if (sender == this.txtPJumin1 && e.KeyChar == (Char)13) { txtPJumin2.Focus(); }
            if (sender == this.txtPJumin2 && e.KeyChar == (Char)13) { txtMSname.Focus(); }
            if (sender == this.txtMSname && e.KeyChar == (Char)13) { cboMGwange.Focus(); }
            if (sender == this.cboMGwange && e.KeyChar == (Char)13) { txtTel1.Focus(); }
            if (sender == this.txtTel1 && e.KeyChar == (Char)13) { txtTel2.Focus(); }
            if (sender == this.txtTel2 && e.KeyChar == (Char)13) { txtTel3.Focus(); }
            if (sender == this.txtTel3 && e.KeyChar == (Char)13) { txtHp1.Focus(); }
            if (sender == this.txtHp1 && e.KeyChar == (Char)13) { txtHp2.Focus(); }
            if (sender == this.txtHp2 && e.KeyChar == (Char)13) { txtHp3.Focus(); }
            if (sender == this.txtHp3 && e.KeyChar == (Char)13) { txtPostCode.Focus(); }
            if (sender == this.txtPostCode && e.KeyChar == (Char)13) { txtJuso2.Focus(); }
            if (sender == this.txtJuso2 && e.KeyChar == (Char)13) { txtEmail.Focus(); }
            if (sender == this.txtEmail && e.KeyChar == (Char)13) { cboUse.Focus(); }
            if (sender == this.cboUse && e.KeyChar == (Char)13) { btnSave.Focus(); }
        }

        private void eForm_Load(object sender, EventArgs e)
        {
            CQ = new ComQuery();
            CF = new ComFunc();

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.SetAllControlClear(pnlBody);

            //스테이션에서 문자여만 체크
            if(bSMS == true)
            {
                panMain.Visible = false;
                panInj_SMS.Visible = true;
                lblInfo_inj.Visible = true;
                this.Size = new System.Drawing.Size(655, 180);
            }

            cboMGwange.Items.Clear();
            cboMGwange.Items.Add("세대주");
            cboMGwange.Items.Add("본인");
            cboMGwange.Items.Add("부");
            cboMGwange.Items.Add("모");
            cboMGwange.Items.Add("자");
            cboMGwange.Items.Add("기타");
            cboMGwange.SelectedIndex = 0;

            cboUse.Items.Clear();
            cboUse.Items.Add("Y:동의");
            cboUse.Items.Add("N:동의안함");
            cboUse.SelectedIndex = 0;

            cboAge.Items.Clear();
            cboAge.Items.Add("1. 3세미만");
            cboAge.Items.Add("2. 3세~59개월");
            //cboAge.Items.Add("3. 만65세이상"); //노인기준 변경
            cboAge.Items.Add("3. 만62세이상");
            cboAge.Items.Add("4. 만75세이상");
            cboAge.Items.Add("5. 13세~18세");
            cboAge.SelectedIndex = 0;

            //접종자 및 보호자 정보표시
            txtPtno.Text = FstrPtno;
            Get_DataLoad(txtPtno.Text);

            txtBirth.Select();
        }

        //접종자 및 보호자 정보표시
        private void Get_DataLoad(string ArgPtno)
        {
            DataTable Dt = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            String strTelNo = string.Empty;
            string strHphone = string.Empty;
            
            Cursor.Current = Cursors.WaitCursor;

            #region 환자기본정보 가져오기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT ZipCode1 || ZipCode2 AS ZipCode , Juso , ";
            SQL += ComNum.VBLF + "        JUMIN1 ,JUMIN2, JUMIN3, ";
            SQL += ComNum.VBLF + "        SNAME, TO_CHAR(BIRTH, 'YYYYMMDD') BIRTH, TEL, ";
            SQL += ComNum.VBLF + "        HPHONE2, BuildNo, ZipCode3, ";
            SQL += ComNum.VBLF + "        RoadDetail ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND PANO = '" + ArgPtno + "' ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count != 0)
            {
                txtJumin1.Text = Dt.Rows[0]["JUMIN1"].ToString().Trim();
                txtJumin2.Text = clsAES.DeAES(Dt.Rows[0]["JUMIN3"].ToString().Trim());
                txtSname.Text = Dt.Rows[0]["SNAME"].ToString().Trim();
                txtBirth.Text = VB.Replace( Dt.Rows[0]["BIRTH"].ToString().Trim(), "-", "");

                //
                lblInfo_inj.Text = ArgPtno + "[ " + txtSname.Text.Trim() + " (" + txtJumin1.Text.Trim() + "-" + Dt.Rows[0]["JUMIN2"].ToString().Trim()+ ") ]" ;

                strTelNo = Dt.Rows[0]["TEL"].ToString().Trim();                 //보호자 전화번호
                if (strTelNo != "")
                {
                    if (Convert.ToInt32(VB.I(strTelNo.Trim(), "-") -1) == 1)
                    {
                        txtTel2.Text = VB.Pstr(strTelNo.Trim(), "-", 1);
                        txtTel3.Text = VB.Pstr(strTelNo.Trim(), "-", 2);
                    }
                    else if (Convert.ToInt32(VB.I(strTelNo.Trim(), "-") -1) == 2)
                    {
                        txtTel1.Text = VB.Pstr(strTelNo.Trim(), "-", 1);
                        txtTel2.Text = VB.Pstr(strTelNo.Trim(), "-", 2);
                        txtTel3.Text = VB.Pstr(strTelNo.Trim(), "-", 3);
                    }
                }

                strHphone = Dt.Rows[0]["HPHONE2"].ToString().Trim();            //보호자 휴대폰번호
                if (strHphone != "")
                {
                    if (Convert.ToInt32(VB.I(strHphone.Trim(), "-") -1) == 2)
                    {
                        txtHp1.Text = VB.Pstr(strHphone.Trim(), "-", 1);
                        txtHp2.Text = VB.Pstr(strHphone.Trim(), "-", 2);
                        txtHp3.Text = VB.Pstr(strHphone.Trim(), "-", 3);
                    }
                }

                if (Dt.Rows[0]["BuildNo"].ToString().Trim() != "")
                {
                    txtPostCode.Text = Dt.Rows[0]["ZipCode3"].ToString().Trim();
                    txtJuso1.Text =  CQ.Read_RoadJuso(clsDB.DbCon, Dt.Rows[0]["BuildNo"].ToString().Trim());
                    txtJuso2.Text = Dt.Rows[0]["RoadDetail"].ToString().Trim();
                }
                else
                {
                    txtPostCode.Text = Dt.Rows[0]["ZipCode"].ToString().Trim();
                    txtJuso1.Text = CQ.Read_Juso(clsDB.DbCon, Dt.Rows[0]["ZipCode"].ToString().Trim());
                    txtJuso2.Text = Dt.Rows[0]["Juso"].ToString().Trim();
                }
                AutoCalc(txtBirth.Text);
            }

            Dt.Dispose();
            Dt = null;
            #endregion

            #region 기존 등록자료에서 찾기
            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, PPERID, PPERID2, PNAME,             --등록번호, 피접종자주민번호, 피접종자주민번호(암호화), 피접종자성명";
            SQL += ComNum.VBLF + "        HPERID, HPERID2, RELA,                    --보호자주민번호, 보호자주민번호(암호화), 보호자와의관계";
            SQL += ComNum.VBLF + "        PTELNO1, PTELNO2, PTELNO3,                --전화번호(지역), 전화번호(국번), 전화번호(번호)";
            SQL += ComNum.VBLF + "        PLNO, PADD1, PADD2,                       --우편번호, 주소, 상세주소";
            SQL += ComNum.VBLF + "        EMAIL, MPHONE1, MPHONE2, MPHONE3,         --이메일, 휴대폰(식별), 휴대폰(국번), 휴대폰(번호)";
            SQL += ComNum.VBLF + "        BABYGUBUN, BIRTHDAY, PINFOUSEDYON,        --쌍둥이구분, 실제생년월일, 개인정보동의여부, ";
            SQL += ComNum.VBLF + "        HNAME, JINGUBUN , NEXTCALL ,ALLERGY                        --보호자성명,나이구분";
            SQL += ComNum.VBLF + "        ,inj_sts ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "VACCINE_TPATIENT ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND PANO = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "  ORDER BY LAST_UPDATE DESC ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                if (bSMS ==false)
                {
                    ComFunc.MsgBox("등록된 자료가 없으므로 기본 환자정보를 표시합니다." + '\r' + '\r' + "기본정보를 확인후 저장하시기 바랍니다.");
                }
                
                if (txtGubun.Text == "") { txtGubun.Text = "1"; }
            }
            else
            {
                ComFunc.ComboFind(cboMGwange, "L", 10, Dt.Rows[0]["RELA"].ToString().Trim());

                txtBirth.Text = Dt.Rows[0]["BIRTHDAY"].ToString().Trim();
                txtGubun.Text = Dt.Rows[0]["BABYGUBUN"].ToString().Trim();

                if (Dt.Rows[0]["HPERID2"].ToString().Trim() != "")
                {
                    string strMJumin2 = clsAES.DeAES(Dt.Rows[0]["HPERID2"].ToString().Trim());

                    this.txtPJumin1.Text = VB.Left(strMJumin2, 6);
                    this.txtPJumin2.Text = VB.Right(strMJumin2, 7);
                }
                else
                {
                    string strMJumin = clsAES.DeAES(Dt.Rows[0]["HPERID"].ToString().Trim());

                    this.txtPJumin1.Text = VB.Left(strMJumin, 6);
                    this.txtPJumin2.Text = VB.Right(strMJumin, 7);
                }


                txtMSname.Text = Dt.Rows[0]["HName"].ToString().Trim();

                txtTel1.Text = Dt.Rows[0]["PTELNO1"].ToString().Trim();       
                txtTel2.Text = Dt.Rows[0]["PTELNO2"].ToString().Trim();
                txtTel3.Text = Dt.Rows[0]["PTELNO3"].ToString().Trim();

                txtHp1.Text = Dt.Rows[0]["MPHONE1"].ToString().Trim();
                txtHp2.Text = Dt.Rows[0]["MPHONE2"].ToString().Trim();
                txtHp3.Text = Dt.Rows[0]["MPHONE3"].ToString().Trim();

                if (txtPostCode.Text.Trim() == "") { txtPostCode.Text = Dt.Rows[0]["PLNO"].ToString().Trim(); }
                if (txtJuso1.Text.Trim() == "") { txtJuso1.Text = Dt.Rows[0]["PADD1"].ToString().Trim(); }
                if (txtJuso2.Text.Trim() == "") { txtJuso2.Text = Dt.Rows[0]["PADD2"].ToString().Trim(); }

                txtEmail.Text = Dt.Rows[0]["EMAIL"].ToString().Trim();

                ComFunc.ComboFind(cboUse, "L", 1, Dt.Rows[0]["PINFOUSEDYON"].ToString().Trim());
                ComFunc.ComboFind(cboAge, "L", 1, Dt.Rows[0]["JINGUBUN"].ToString().Trim());

                AutoCalc(txtBirth.Text);

                if (Dt.Rows[0]["NEXTCALL"].ToString().Trim() == "1") { chkPush.Checked = true; }
                if (Dt.Rows[0]["ALLERGY"].ToString().Trim() == "1") { chkAret.Checked = true; }

                if (bSMS ==false)
                {
                    if (Dt.Rows[0]["inj_sts"].ToString().Trim() =="Y")
                    {
                        ComFunc.MsgBox("스테이션에서 예방접종 SMS 입력대상입니다..." + "\r\n" + "\r\n" +  "기본정보 다시 체크 해주세요!!");
                    }
                }
            }

            Dt.Dispose();
            Dt = null;
            #endregion

            Cursor.Current = Cursors.Default;
        }

        //저장버튼
        private void btnSave_Click(object sender, EventArgs e)
        {
            
        }

        //닫기버튼
        private void btnExit_Click(object sender, EventArgs e)
        {

        }

        //우편번호 검색
        private void btnPost_Click(object sender, EventArgs e)
        {
            
        }

        private void ePost_value(string GstrValue)
        {
            Fvalue = GstrValue;
        }

        private void cboMGwange_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void AutoCalc(string ArgBirth)
        {
            DataTable DtFunc = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (txtBirth.Text.Length == 8)
            {
                SQL = "";
                SQL += ComNum.VBLF + " select trunc(MONTHS_BETWEEN ( ";
                SQL += ComNum.VBLF + "        trunc(sysdate),to_date('" + ArgBirth + "', 'yyyymmdd')  )) MONTHS  ";
                SQL += ComNum.VBLF + "   from dual ";
                SqlErr = clsDB.GetDataTableEx(ref DtFunc, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (DtFunc.Rows.Count > 0)
                {
                    if (Convert.ToInt32(VB.Val(DtFunc.Rows[0]["MONTHS"].ToString())) < 36)
                        cboAge.SelectedIndex = 0;
                    else if (Convert.ToInt32(VB.Val(DtFunc.Rows[0]["MONTHS"].ToString())) < 60)
                        cboAge.SelectedIndex = 1;
                    //else if (Convert.ToInt32(VB.Val(DtFunc.Rows[0]["MONTHS"].ToString())) >= 780)
                    else if (Convert.ToInt32(VB.Val(DtFunc.Rows[0]["MONTHS"].ToString())) >= 744)
                        cboAge.SelectedIndex = 2;
                    else if (Convert.ToInt32(VB.Val(DtFunc.Rows[0]["MONTHS"].ToString())) >= 900)
                        cboAge.SelectedIndex = 3;
                    else if (Convert.ToInt32(VB.Val(DtFunc.Rows[0]["MONTHS"].ToString())) >= 156 && Convert.ToInt32(VB.Val(DtFunc.Rows[0]["MONTHS"].ToString())) <= 216)
                        cboAge.SelectedIndex = 4;
                }

                DtFunc.Dispose();
                DtFunc = null;
            }
            else
            {
                ComFunc.MsgBox("생년월일 형식 조건이 맞지 않습니다.");
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string strToDate = string.Empty;
            string strPush = string.Empty;
            string strAret = string.Empty;


            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) { return; }     //권한확인

            ComFunc.ReadSysDate(clsDB.DbCon);
            strToDate = VB.Left(clsPublic.GstrSysDate, 4) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2);

            if (chkPush.Checked == true) { strPush = "1"; }
            if (chkAret.Checked == true) { strAret = "1"; }

            #region 입력데이터 점검
            if (txtPtno.Text == "") { ComFunc.MsgBox("등록번호 공란입니다."); txtPtno.Focus(); return; }
            if (txtBirth.Text.Length != 8) { ComFunc.MsgBox("생년월일 자릿수 오류"); txtBirth.Focus(); return; }
            if (VB.I(txtBirth.Text, "-") > 1) { ComFunc.MsgBox("생년월일에 '-' 삭제요청"); txtBirth.Focus(); return; }
            if (txtJumin1.Text == "") { ComFunc.MsgBox("피접종자 주민번호(앞) 공란입니다."); txtJumin1.Focus(); return; }
            if (txtJumin2.Text == "") { ComFunc.MsgBox("피접종자 주민번호(뒤) 공란입니다."); txtJumin2.Focus(); return; }
            if (txtJumin1.Text.Length != 6) { ComFunc.MsgBox("피접종자 주민번호(앞) 자릿수 오류"); txtJumin1.Focus(); return; }
            if (txtJumin2.Text.Length != 7) { ComFunc.MsgBox("피접종자 주민번호(뒤) 자릿수 오류"); txtJumin2.Focus(); return; }

            if (txtJumin2.Text != "")
            {
                if (txtJumin2.Text.Length == 7 && VB.Mid(txtJumin2.Text, 2, 5) == "00000" && VB.Right(txtJumin2.Text, 1) != "0")
                {
                    ComFunc.MsgBox("피접종자 주민번호(뒤)가 아직 확정안되었을경우 끝자리를 0으로 해주세요(예 4000000,5000000) ");
                    txtJumin2.Focus();
                    return;
                }
            }

            if (txtPJumin1.Text == "") { ComFunc.MsgBox("보호자 주민번호(앞) 공란입니다."); txtPJumin1.Focus(); return; }
            if (txtPJumin2.Text == "") { ComFunc.MsgBox("보호자 주민번호(뒤) 공란입니다."); txtPJumin2.Focus(); return; }
            if (txtPJumin1.Text.Length != 6) { ComFunc.MsgBox("보호자 주민번호(앞) 자릿수 오류"); txtPJumin1.Focus(); return; }
            if (txtPJumin2.Text.Length != 7) { ComFunc.MsgBox("보호자 주민번호(뒤) 자릿수 오류"); txtPJumin2.Focus(); return; }

            string ErrCheck = ComFunc.JuminNoCheck(clsDB.DbCon, txtPJumin1.Text, txtPJumin2.Text);

            if (ErrCheck.Trim() != "")
            {
                clsPublic.GstrMsgTitle = "오류";
                clsPublic.GstrMsgList = ErrCheck + '\r';
                clsPublic.GstrMsgList += "보호자 등록번호로 자격조회가 된다면 무시하고 저장하십시오." + '\r';
                clsPublic.GstrMsgList += "저장하시겠습니까?" + '\r';

                DialogResult result = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "설정확인", MessageBoxDefaultButton.Button1);
                if (result == DialogResult.No) { txtPJumin2.Focus(); return; }
            }

            if (cboMGwange.Text.Trim() == "") { ComFunc.MsgBox("보호자와의 관계 공란입니다."); cboMGwange.Focus(); return; }
            if (txtMSname.Text.Trim() == "") { ComFunc.MsgBox("보호자 성명 공란입니다."); txtMSname.Focus(); return; }
            if (txtTel1.Text.Trim() == "") { ComFunc.MsgBox("전화번호(지역) 공란입니다."); txtTel1.Focus(); return; }
            if (txtTel2.Text.Trim() == "") { ComFunc.MsgBox("전화번호(국번) 공란입니다."); txtTel2.Focus(); return; }
            if (txtTel3.Text.Trim() == "") { ComFunc.MsgBox("전화번호(번호) 공란입니다."); txtTel3.Focus(); return; }
            if (txtHp1.Text.Trim() == "") { ComFunc.MsgBox("휴대폰1 공란입니다."); txtHp1.Focus(); return; }
            if (txtHp2.Text.Trim() == "") { ComFunc.MsgBox("휴대폰2 공란입니다."); txtHp2.Focus(); return; }
            if (txtHp3.Text.Trim() == "") { ComFunc.MsgBox("휴대폰3 공란입니다."); txtHp3.Focus(); return; }
            if (txtPostCode.Text.Trim() == "") { ComFunc.MsgBox("우편번호 공란입니다."); txtPostCode.Focus(); return; }

            if (txtEmail.Text.Trim() != "")
            {
                if (VB.I(txtEmail.Text.Trim(), "@") != 2) { ComFunc.MsgBox("Email 형식에러(ex:123@han.net)"); txtEmail.Focus(); return; }
            }

            if (cboUse.Text.Trim() == "") { ComFunc.MsgBox("개인정보동의 공란입니다."); cboUse.Focus(); return; }
            #endregion

            #region 입력데이터 저장
            Cursor.Current = Cursors.WaitCursor;

            DataTable Dt = new DataTable();
            String strRowID = String.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM ADMIN.VACCINE_TPATIENT ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "   AND PANO = '" + txtPtno.Text + "' ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count > 0)
                strRowID = Dt.Rows[0]["ROWID"].ToString().Trim();

            Dt.Dispose();
            Dt = null;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                if (strRowID == "")
                {
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "VACCINE_TPATIENT ";
                    SQL += ComNum.VBLF + "        (PANO, PPERID, PPERID_OLD, PPERID2, PPERID_OLD2,  --등록번호, 피접종자주민번호, 신생아주민번호(old), 피접종자주민번호암호화, 신생아주민번호(old)암호화";
                    SQL += ComNum.VBLF + "         PNAME, HPERID, HPERID2, RELA,                    --성명, 보호자주민번호, 보호자주민번호(암호화), 보호자와의관계";
                    SQL += ComNum.VBLF + "         PTELNO1, PTELNO2, PTELNO3,                       --전화번호(지역), 국번, 번호";
                    SQL += ComNum.VBLF + "         PLNO, PADD1, PADD2, EMAIL,                       --우편번호, 주소, 상세주소, 이메일";
                    SQL += ComNum.VBLF + "         MPHONE1, MPHONE2, MPHONE3, BABYGUBUN, BIRTHDAY,  --휴대폰번호(식별), 국번, 번호, 쌍둥이구분, 실생년월일";
                    SQL += ComNum.VBLF + "         LAST_UPDATE, PINFOUSEDYON, HName,                --수정일자, 개인정보동의여부, 보호자성명";
                    SQL += ComNum.VBLF + "         JINGUBUN,NEXTCALL,ALLERGY, Gubun,inj_sts )                                --나이구분, 임시구분";
                    SQL += ComNum.VBLF + " VALUES ('" + txtPtno.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtJumin1.Text.Trim() + VB.Left(txtJumin2.Text.Trim(), 1) + "******" + "',";
                    SQL += ComNum.VBLF + "         '" + txtJumin1.Text.Trim() + VB.Left(txtJumin2.Text.Trim(), 1) + "******" + "',";
                    SQL += ComNum.VBLF + "         '" + clsAES.AES(txtJumin1.Text.Trim() + txtJumin2.Text.Trim()) + "',";
                    SQL += ComNum.VBLF + "         '" + clsAES.AES(txtJumin1.Text.Trim() + txtJumin2.Text.Trim()) + "',";
                    SQL += ComNum.VBLF + "         '" + txtSname.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtPJumin1.Text.Trim() + VB.Left(txtPJumin2.Text.Trim(), 1) + "******" + "',";
                    SQL += ComNum.VBLF + "         '" + clsAES.AES(txtPJumin1.Text.Trim() + txtPJumin2.Text.Trim()) + "',";
                    SQL += ComNum.VBLF + "         '" + cboMGwange.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtTel1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtTel2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtTel3.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtPostCode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtJuso1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtJuso2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtEmail.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtHp1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtHp2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtHp3.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtGubun.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtBirth.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + strToDate + "', ";
                    SQL += ComNum.VBLF + "         '" + VB.Left(cboUse.Text.Trim(), 1) + "', ";
                    SQL += ComNum.VBLF + "         '" + txtMSname.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "         '" + VB.Left(cboAge.Text.Trim(), 1) + "', ";
                    SQL += ComNum.VBLF + "         '" + strPush + "', ";
                    SQL += ComNum.VBLF + "         '" + strAret + "', ";
                    SQL += ComNum.VBLF + "         'N2','' ) ";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "VACCINE_TPATIENT ";
                    SQL += ComNum.VBLF + "    SET PPERID        = '" + txtJumin1.Text.Trim() + VB.Left(txtJumin2.Text.Trim(), 1) + "******" + "', ";
                    SQL += ComNum.VBLF + "        PPERID2       = '" + clsAES.AES(txtJumin1.Text.Trim() + txtJumin2.Text.Trim()) + "', ";
                    SQL += ComNum.VBLF + "        Pname         = '" + txtSname.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        HPERID        = '" + txtPJumin1.Text.Trim() + VB.Left(txtPJumin2.Text.Trim(), 1) + "******" + "', ";
                    SQL += ComNum.VBLF + "        HPERID2       = '" + clsAES.AES(txtPJumin1.Text.Trim() + txtPJumin2.Text.Trim()) + "', ";
                    SQL += ComNum.VBLF + "        RELA          = '" + cboMGwange.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        PTELNO1       = '" + txtTel1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        PTELNO2       = '" + txtTel2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        PTELNO3       = '" + txtTel3.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        PLNO          = '" + txtPostCode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        PADD1         = '" + txtJuso1.Text.Trim() + "',  ";
                    SQL += ComNum.VBLF + "        PADD2         = '" + txtJuso2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        EMAIL         = '" + txtEmail.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        MPHONE1       = '" + txtHp1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        MPHONE2       = '" + txtHp2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        MPHONE3       = '" + txtHp3.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        BABYGUBUN     = '" + txtGubun.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        BIRTHDAY      = '" + txtBirth.Text.Trim() + "',  ";
                    SQL += ComNum.VBLF + "        LAST_UPDATE   = '" + strToDate + "', ";
                    SQL += ComNum.VBLF + "        HName         = '" + txtMSname.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        JINGUBUN      = '" + VB.Left(cboAge.Text.Trim(), 1) + "', ";
                    SQL += ComNum.VBLF + "        NEXTCALL      = '" + strPush + "', ";
                    SQL += ComNum.VBLF + "        ALLERGY      = '" + strAret + "', ";
                    SQL += ComNum.VBLF + "        inj_sts      = '', ";
                    SQL += ComNum.VBLF + "        PINFOUSEDYON  = '" + VB.Left(cboUse.Text.Trim(), 1) + "' ";
                    SQL += ComNum.VBLF + "  WHERE ROWID         = '" + strRowID + "'";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            #endregion

            this.Close();
        }

        private void btnExit_Click_1(object sender, EventArgs e)
        {
            //ComFunc.MsgBox("예방접종 정보를 저장하지 않으면 수납처리가 되지 않습니다.", "확인");
            this.Close();
        }

        private void btnPost_Click_1(object sender, EventArgs e)
        {
            Fvalue = "";

            frmSearchRoadAdd frm = new frmSearchRoadAdd();
            frm.rSetGstrValue += new frmSearchRoadAdd.SetGstrValue(ePost_value);
            frm.ShowDialog();

            if (Fvalue != "")
            {
                txtPostCode.Text = VB.Left(VB.Pstr(Fvalue, "|", 1), 3);
                txtPostCode.Text += VB.Mid(VB.Pstr(Fvalue, "|", 1), 4, 2);

                txtJuso1.Text = VB.Pstr(Fvalue, "|", 2).Trim();
                txtJuso2.Text = "";
            }

            txtJuso2.Focus();
        }

        private void cboMGwange_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboMGwange.Text.Trim() == "본인")
            {
                txtPJumin1.Text = txtJumin1.Text;
                txtPJumin2.Text = txtJumin2.Text;
                txtMSname.Text = txtSname.Text;
            }
            else
            {
                txtPJumin1.Text = "";
                txtPJumin2.Text = "";
                txtMSname.Text = "";
            }
        }

        private void btnSave_inj_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string strToDate = string.Empty;
            string strPush = string.Empty;
            string strAret = string.Empty;
                                  

            ComFunc.ReadSysDate(clsDB.DbCon);
            strToDate = VB.Left(clsPublic.GstrSysDate, 4) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2);

            if (chkPush.Checked == true) { strPush = "1"; }
            if (chkAret.Checked == true) { strAret = "1"; }

            #region 입력데이터 점검
            if (txtPtno.Text == "") { ComFunc.MsgBox("등록번호 공란입니다."); txtPtno.Focus(); return; }
            //if (txtBirth.Text.Length != 8) { ComFunc.MsgBox("생년월일 자릿수 오류"); txtBirth.Focus(); return; }
            //if (VB.I(txtBirth.Text, "-") > 1) { ComFunc.MsgBox("생년월일에 '-' 삭제요청"); txtBirth.Focus(); return; }
            //if (txtJumin1.Text == "") { ComFunc.MsgBox("피접종자 주민번호(앞) 공란입니다."); txtJumin1.Focus(); return; }
            //if (txtJumin2.Text == "") { ComFunc.MsgBox("피접종자 주민번호(뒤) 공란입니다."); txtJumin2.Focus(); return; }
            //if (txtJumin1.Text.Length != 6) { ComFunc.MsgBox("피접종자 주민번호(앞) 자릿수 오류"); txtJumin1.Focus(); return; }
            //if (txtJumin2.Text.Length != 7) { ComFunc.MsgBox("피접종자 주민번호(뒤) 자릿수 오류"); txtJumin2.Focus(); return; }

            //if (txtJumin2.Text != "")
            //{
            //    if (txtJumin2.Text.Length == 7 && VB.Mid(txtJumin2.Text, 2, 5) == "00000" && VB.Right(txtJumin2.Text, 1) != "0")
            //    {
            //        ComFunc.MsgBox("피접종자 주민번호(뒤)가 아직 확정안되었을경우 끝자리를 0으로 해주세요(예 4000000,5000000) ");
            //        txtJumin2.Focus();
            //        return;
            //    }
            //}

            //if (txtPJumin1.Text == "") { ComFunc.MsgBox("보호자 주민번호(앞) 공란입니다."); txtPJumin1.Focus(); return; }
            //if (txtPJumin2.Text == "") { ComFunc.MsgBox("보호자 주민번호(뒤) 공란입니다."); txtPJumin2.Focus(); return; }
            //if (txtPJumin1.Text.Length != 6) { ComFunc.MsgBox("보호자 주민번호(앞) 자릿수 오류"); txtPJumin1.Focus(); return; }
            //if (txtPJumin2.Text.Length != 7) { ComFunc.MsgBox("보호자 주민번호(뒤) 자릿수 오류"); txtPJumin2.Focus(); return; }

            //string ErrCheck = ComFunc.JuminNoCheck(clsDB.DbCon, txtPJumin1.Text, txtPJumin2.Text);

            //if (ErrCheck.Trim() != "")
            //{
            //    clsPublic.GstrMsgTitle = "오류";
            //    clsPublic.GstrMsgList = ErrCheck + '\r';
            //    clsPublic.GstrMsgList += "보호자 등록번호로 자격조회가 된다면 무시하고 저장하십시오." + '\r';
            //    clsPublic.GstrMsgList += "저장하시겠습니까?" + '\r';

            //    DialogResult result = ComFunc.MsgBoxQ(clsPublic.GstrMsgList, "설정확인", MessageBoxDefaultButton.Button1);
            //    if (result == DialogResult.No) { txtPJumin2.Focus(); return; }
            //}

            //if (cboMGwange.Text.Trim() == "") { ComFunc.MsgBox("보호자와의 관계 공란입니다."); cboMGwange.Focus(); return; }
            //if (txtMSname.Text.Trim() == "") { ComFunc.MsgBox("보호자 성명 공란입니다."); txtMSname.Focus(); return; }
            //if (txtTel1.Text.Trim() == "") { ComFunc.MsgBox("전화번호(지역) 공란입니다."); txtTel1.Focus(); return; }
            //if (txtTel2.Text.Trim() == "") { ComFunc.MsgBox("전화번호(국번) 공란입니다."); txtTel2.Focus(); return; }
            //if (txtTel3.Text.Trim() == "") { ComFunc.MsgBox("전화번호(번호) 공란입니다."); txtTel3.Focus(); return; }
            //if (txtHp1.Text.Trim() == "") { ComFunc.MsgBox("휴대폰1 공란입니다."); txtHp1.Focus(); return; }
            //if (txtHp2.Text.Trim() == "") { ComFunc.MsgBox("휴대폰2 공란입니다."); txtHp2.Focus(); return; }
            //if (txtHp3.Text.Trim() == "") { ComFunc.MsgBox("휴대폰3 공란입니다."); txtHp3.Focus(); return; }
            //if (txtPostCode.Text.Trim() == "") { ComFunc.MsgBox("우편번호 공란입니다."); txtPostCode.Focus(); return; }

            //if (txtEmail.Text.Trim() != "")
            //{
            //    if (VB.I(txtEmail.Text.Trim(), "@") != 2) { ComFunc.MsgBox("Email 형식에러(ex:123@han.net)"); txtEmail.Focus(); return; }
            //}

            //if (cboUse.Text.Trim() == "") { ComFunc.MsgBox("개인정보동의 공란입니다."); cboUse.Focus(); return; }
            #endregion

            #region 입력데이터 저장
            Cursor.Current = Cursors.WaitCursor;

            DataTable Dt = new DataTable();
            String strRowID = String.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM ADMIN.VACCINE_TPATIENT ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "   AND PANO = '" + txtPtno.Text + "' ";
            SqlErr = clsDB.GetDataTableEx(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count > 0)
                strRowID = Dt.Rows[0]["ROWID"].ToString().Trim();

            Dt.Dispose();
            Dt = null;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                if (strRowID == "")
                {
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "VACCINE_TPATIENT ";
                    SQL += ComNum.VBLF + "        (PANO, PPERID, PPERID_OLD, PPERID2, PPERID_OLD2,";
                    SQL += ComNum.VBLF + "         PNAME, HPERID, HPERID2, RELA,                  ";
                    SQL += ComNum.VBLF + "         PTELNO1, PTELNO2, PTELNO3,                      ";
                    SQL += ComNum.VBLF + "         PLNO, PADD1, PADD2, EMAIL,                      ";
                    SQL += ComNum.VBLF + "         MPHONE1, MPHONE2, MPHONE3, BABYGUBUN, BIRTHDAY, ";
                    SQL += ComNum.VBLF + "         LAST_UPDATE, PINFOUSEDYON, HName,               ";
                    SQL += ComNum.VBLF + "         JINGUBUN,NEXTCALL,ALLERGY, Gubun,inj_sts )              ";
                    SQL += ComNum.VBLF + " VALUES ('" + txtPtno.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtJumin1.Text.Trim() + VB.Left(txtJumin2.Text.Trim(), 1) + "******" + "',";
                    SQL += ComNum.VBLF + "         '" + txtJumin1.Text.Trim() + VB.Left(txtJumin2.Text.Trim(), 1) + "******" + "',";
                    SQL += ComNum.VBLF + "         '" + clsAES.AES(txtJumin1.Text.Trim() + txtJumin2.Text.Trim()) + "',";
                    SQL += ComNum.VBLF + "         '" + clsAES.AES(txtJumin1.Text.Trim() + txtJumin2.Text.Trim()) + "',";
                    SQL += ComNum.VBLF + "         '" + txtSname.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtPJumin1.Text.Trim() + VB.Left(txtPJumin2.Text.Trim(), 1) + "******" + "',";
                    SQL += ComNum.VBLF + "         '" + clsAES.AES(txtPJumin1.Text.Trim() + txtPJumin2.Text.Trim()) + "',";
                    SQL += ComNum.VBLF + "         '" + cboMGwange.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtTel1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtTel2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtTel3.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtPostCode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtJuso1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtJuso2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtEmail.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtHp1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtHp2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtHp3.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtGubun.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + txtBirth.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "         '" + strToDate + "', ";
                    SQL += ComNum.VBLF + "         '" + VB.Left(cboUse.Text.Trim(), 1) + "', ";
                    SQL += ComNum.VBLF + "         '" + txtMSname.Text.Trim() + "',";
                    SQL += ComNum.VBLF + "         '" + VB.Left(cboAge.Text.Trim(), 1) + "', ";
                    SQL += ComNum.VBLF + "         '" + strPush + "', ";
                    SQL += ComNum.VBLF + "         '" + strAret + "', ";
                    SQL += ComNum.VBLF + "         'N2','Y' ) ";
                }
                else
                {
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "VACCINE_TPATIENT ";
                    SQL += ComNum.VBLF + "    SET PPERID        = '" + txtJumin1.Text.Trim() + VB.Left(txtJumin2.Text.Trim(), 1) + "******" + "', ";
                    SQL += ComNum.VBLF + "        PPERID2       = '" + clsAES.AES(txtJumin1.Text.Trim() + txtJumin2.Text.Trim()) + "', ";
                    SQL += ComNum.VBLF + "        Pname         = '" + txtSname.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        HPERID        = '" + txtPJumin1.Text.Trim() + VB.Left(txtPJumin2.Text.Trim(), 1) + "******" + "', ";
                    SQL += ComNum.VBLF + "        HPERID2       = '" + clsAES.AES(txtPJumin1.Text.Trim() + txtPJumin2.Text.Trim()) + "', ";
                    SQL += ComNum.VBLF + "        RELA          = '" + cboMGwange.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        PTELNO1       = '" + txtTel1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        PTELNO2       = '" + txtTel2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        PTELNO3       = '" + txtTel3.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        PLNO          = '" + txtPostCode.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        PADD1         = '" + txtJuso1.Text.Trim() + "',  ";
                    SQL += ComNum.VBLF + "        PADD2         = '" + txtJuso2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        EMAIL         = '" + txtEmail.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        MPHONE1       = '" + txtHp1.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        MPHONE2       = '" + txtHp2.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        MPHONE3       = '" + txtHp3.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        BABYGUBUN     = '" + txtGubun.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        BIRTHDAY      = '" + txtBirth.Text.Trim() + "',  ";
                    SQL += ComNum.VBLF + "        LAST_UPDATE   = '" + strToDate + "', ";
                    SQL += ComNum.VBLF + "        HName         = '" + txtMSname.Text.Trim() + "', ";
                    SQL += ComNum.VBLF + "        JINGUBUN      = '" + VB.Left(cboAge.Text.Trim(), 1) + "', ";
                    SQL += ComNum.VBLF + "        NEXTCALL      = '" + strPush + "', ";
                    SQL += ComNum.VBLF + "        ALLERGY      = '" + strAret + "', ";
                    SQL += ComNum.VBLF + "        inj_sts      = 'Y', ";
                    SQL += ComNum.VBLF + "        PINFOUSEDYON  = '" + VB.Left(cboUse.Text.Trim(), 1) + "' ";
                    SQL += ComNum.VBLF + "  WHERE ROWID         = '" + strRowID + "'";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장되었습니다.", "알림");
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            #endregion

            this.Close();
        }

        private void btnExit_inj_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

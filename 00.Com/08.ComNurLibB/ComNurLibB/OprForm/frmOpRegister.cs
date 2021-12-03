using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : frmOpRegister.cs
    /// Description     : 수술부 대장
    /// Author          : 안정수
    /// Create Date     : 2018-01-11
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 Frm수술실대장.frm(Frm수술실대장) 폼 frmOpRegister.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\Ocs\oproom\opmain\Frm수술실대장.frm(Frm수술실대장) >> frmOpRegister.cs 폼이름 재정의" />
    public partial class frmOpRegister : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = null;
        ComFunc CF = null;
        clsQuery Query = null;
        
        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;

        string gPart = "";

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

        public frmOpRegister()
        {
            InitializeComponent();
            setEvent();
        }

        public frmOpRegister(MainFormMessage pform)
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

            this.btnPrint.Click += new EventHandler(eBtnPrint);

            this.optPano.CheckedChanged += new EventHandler(eControl_CheckedChanged);
            this.optDept.CheckedChanged += new EventHandler(eControl_CheckedChanged);

            this.txtPano.KeyPress += new KeyPressEventHandler(eControl_KeyPress);

            this.ssList.ButtonClicked += new EditorNotifyEventHandler(eSpreadButtonClick);
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
                optDept.Checked = true;
                optGbn0.Checked = true;
                
                groupBox5.Visible = false;
                groupBox6.Visible = true;
                groupBox7.Visible = true;

                Set_Init();
            }
        }

        void Set_Init()
        {
            CS = new clsSpread();
            CF = new ComFunc();
            Query = new clsQuery();

            //메뉴사용권한 체크  2019-02-13           
            dt = Query.Get_BasBcode(clsDB.DbCon, "C#_수술스케쥴_외과설정", clsType.User.IdNumber, "", "", "");
            if (ComFunc.isDataTableNull(dt) == false)
            {
                gPart = "OK";                
                chkGS.Visible = true;
                chkGS.Checked = true;

            }
            dt.Dispose();
            dt = null;

            if (clsType.User.BuseCode != "078001")
            {
                CS.setColStyle(ssList, -1, 24, clsSpread.enmSpdType.Hide);
                ssList_Sheet1.OperationMode = FarPoint.Win.Spread.OperationMode.ReadOnly;
            }
            clsOpMain.Combo_OPRCODE_SET(clsDB.DbCon, cboRoom, "1", 1, "ALL");
            cboRoom.SelectedIndex = 0;

            cboGbn.Items.Clear();
            cboGbn.Items.Add(" ");
            cboGbn.Items.Add("1.정규수술대장");
            cboGbn.Items.Add("2.응급수술대장");
            cboGbn.Items.Add("3.통원수술대장");
            cboGbn.Items.Add("4.수술실처치대장");
            cboGbn.Items.Add("5.수술실운동대장");
            cboGbn.Items.Add("6.수술실교정대장");
            cboGbn.Items.Add("7.조직검사시술대장");
            cboGbn.Items.Add("8.수술취소대장");
            cboGbn.Items.Add("9.단순처치");
            cboGbn.Items.Add("10.단순처치(S)");
            cboGbn.Items.Add("11.Angio시술");
            //cboGbn.SelectedIndex = -1;
            cboGbn.SelectedIndex = 0;

            cboGbn2.Items.Clear();
            cboGbn2.Items.Add(" ");
            cboGbn2.Items.Add("1.전신마취대장");
            cboGbn2.Items.Add("2.전신마취대장(기타)");
            cboGbn2.Items.Add("3.부위마취대장(전체)");
            cboGbn2.Items.Add("4.부위마취대장(척추마취)");
            cboGbn2.Items.Add("5.부위마취대장(경막외마취)");
            cboGbn2.Items.Add("6.부위마취대장(미추마취)");
            cboGbn2.Items.Add("7.부위마취대장(상박신경총마취)");
            cboGbn2.Items.Add("8.국소마취대장");
            //cboGbn2.SelectedIndex = -1;
            cboGbn2.SelectedIndex = 0;

            cboIO.Items.Clear();
            cboIO.Items.Add(" ");
            cboIO.Items.Add("1.전체");
            cboIO.Items.Add("2.입원");
            cboIO.Items.Add("3.외래");
            cboIO.SelectedIndex = 1;

            //진료과 Combo SET
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DeptCode";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_ClinicDept";
            SQL += ComNum.VBLF + "WHERE DeptCode NOT IN ('II','HR','TO','R6','HD','PT','AN')";
            SQL += ComNum.VBLF + "ORDER BY PrintRanking";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                cboDept.Items.Add("전체");
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim());
                }
            }

            dt.Dispose();
            dt = null;

            cboDept.SelectedIndex = 0;

            txtPano.Text = "";
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

        void eControl_CheckedChanged(object sender, EventArgs e)
        {
            if(sender == this.optPano)
            {
                groupBox5.Visible = true;
                groupBox6.Visible = false;
                groupBox7.Visible = false;
            }

            else if(sender == this.optDept)
            {
                groupBox5.Visible = false;
                groupBox6.Visible = true;
                groupBox7.Visible = true;
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(sender == this.txtPano)
            {
                if(e.KeyChar != 13)
                {
                    return;
                }

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  PANO";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL += ComNum.VBLF + "WHERE PANO = '" + txtPano.Text + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if(dt.Rows.Count == 0)
                {
                    ComFunc.MsgBox("등록번호를 다시 확인 해주세요.");
                    txtPano.Text = "";
                }

                dt.Dispose();
                dt = null;                
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
               else
                {
                    eGetData();
                }
                               
            }
        }
        void eSpreadButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            if (e.Row < 0 || e.Column < 0) return;

            if (e.Column == 24)
            {
                string strPano = ssList.ActiveSheet.Cells[e.Row, 6].Text.Trim();
                string strOpdate = ssList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                string strOptime = ssList.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                string strDrcode = ssList.ActiveSheet.Cells[e.Row, 25].Text.Trim();
                string strAsaAdd = ssList.ActiveSheet.Cells[e.Row, 26].Text.Trim();
                string strErGubun = ssList.ActiveSheet.Cells[e.Row, 27].Text.Trim();
                string strOpreGubun = ssList.ActiveSheet.Cells[e.Row, 28].Text.Trim();
                string strOpWrtno = ssList.ActiveSheet.Cells[e.Row, 29].Text.Trim();

                frmOpInfectWatch f = new frmOpInfectWatch(strPano, strOpdate, strOptime, strDrcode, strAsaAdd, strErGubun, strOpreGubun, strOpWrtno);
                f.ShowDialog();
                clsNurse.setClearMemory(f);
            }

        }
        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                ePrint();
            }
        }

        void ePrint()
        {
            using (clsSpread SPR = new clsSpread())
            {
                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                string strTitle = "";
                string strSubTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                strTitle = "수 술 실 대 장";
                strSubTitle = "조회기간 : " + dtpFDate.Text + "~" + dtpTDate.Text + VB.Space(10) + "인쇄일자 : " + ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "A"), "A");

                strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 10, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

                strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, true, true, true, false, false, false, (float)0.73);

                SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        void eGetData()
        {
            int i = 0;
            int nREAD = 0;
            int nRow = 0;

            string strOK = "";
            string strOpSize = "";
            string strOpSize2 = "";
            string strFDate = "";
            string strTDate = "";
            string strOpGbn = "";
            string strOpGbn2 = "";
            string strOpGbn3 = "";
            string strOpGbnX = "";
            string strAngbn = "";
            string strIPDNO = "";

            strFDate = dtpFDate.Text;
            strTDate = dtpTDate.Text;

            Cursor.Current = Cursors.WaitCursor;

            if (cboGbn.SelectedItem.ToString().Trim() != "")
            {
                switch(VB.Pstr(cboGbn.SelectedItem.ToString().Trim(), ".", 1))
                {
                    case "1":
                        strOpGbn = "1";         //정규수술
                        break;
                    case "2":
                        strOpGbn = "2', '3";    //응급수술
                        break;
                    case "3":
                        strOpGbn = "4";         //통원수술
                        break;
                    case "4":
                        strOpGbn2 = "4";        //처치
                        break;
                    case "5":
                        strOpGbn2 = "5";        //교정
                        break;
                    case "6":
                        strOpGbn2 = "6";        //운동
                        break;
                    case "7":
                        strOpGbn3 = "E/B";      //조직검사시술
                        break;
                    case "8":
                        strOpGbnX= "X";         //취소건
                        break;

                    case "9":
                        strOpGbn = "5";         //단순처치
                        break;
                    case "10":
                        strOpGbn = "6";         //단순처치(S)
                        break;
                    case "11":
                        strOpGbn = "7";         //Angio시술
                        break;
                }
            }

            if(cboGbn2.SelectedItem.ToString().Trim() != "")
            {
                switch(VB.Left(cboGbn2.SelectedItem.ToString().Trim(), 1))
                {
                    case "1":
                        strAngbn = "G";                                         //전신마취
                        break;

                    case "2":
                        strAngbn = "M','MASK','LV-K','LV-A','LV-B','LV-C";      //전신마취기타 -mask,정맥-a,b,c
                        break;

                    case "3":
                        strAngbn = "S','E','A";                                 //부위마취전체
                        break;

                    case "4":
                        strAngbn = "S";                                         //부위마취-척추마취
                        break;

                    case "5":
                        strAngbn = "E";                                         //부위마취-경막외마취
                        break;

                    case "6":
                        strAngbn = "C";                                         //부위마취-미추마취
                        break;

                    case "7":
                        strAngbn = "A";                                         //부위마취-상박신경총마취
                        break;

                    case "8":
                        strAngbn = "L";                                         //국소마취
                        break;
                }
            }

            CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT  ";
            SQL += ComNum.VBLF + "   'OP' AS tGbn,Pano,OpTimeFrom,DeptCode,OpBun,TO_CHAR(OpDate,'YYYY-MM-DD') OpDate,OPSTIME,OPETIME    ";
            SQL += ComNum.VBLF + "   ,OpRoom,SName,IPDOPD,age,sex,RoomCode,DrCode   ";
            SQL += ComNum.VBLF + "   ,DIAGNOSIS,OpTitle,AnDoct1,AnGbn,OPCANCEL,OpErr,OpHapSayu,OPDOCT2,AsaAdd,GbEr,Opre,Wrtno  ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ORAN_MASTER";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND OpDate >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "      AND OpDate <=TO_DATE('" + strTDate + "','YYYY-MM-DD')";

            if(groupBox5.Visible == true)
            {
                if(txtPano.Text != "")
                {
                    SQL += ComNum.VBLF + "AND PANO = '" + txtPano.Text + "'";
                }
            }

            else
            {
                if(cboDept.SelectedItem.ToString().Trim() != "전체")
                {
                    SQL += ComNum.VBLF + "AND DeptCode = '" + cboDept.SelectedItem.ToString().Trim() + "'";

                    if(VB.Left(cboDr.SelectedItem.ToString().Trim(), 4) != "****")
                    {
                        //2020-04-08 안정수, 안길영과장 변경전 의사코드 포함되도록
                        if (VB.Left(cboDr.SelectedItem.ToString().Trim(), 4) == "2223")
                        {
                            SQL += ComNum.VBLF + "  AND DRCODE IN ('2216', '2223')";
                        }
                        //2021-03-11 안정수, 안길영과장 변경전 의사코드 포함되도록
                        else if (VB.Left(cboDr.SelectedItem.ToString().Trim(), 4) == "2224")
                        {
                            SQL += ComNum.VBLF + "  AND DRCODE IN ('2203', '2224')";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "  AND DRCODE = '" + VB.Left(cboDr.SelectedItem.ToString().Trim(), 4) + "'";
                        } 
                    }
                }
            }

            if(VB.Left(cboRoom.SelectedItem.ToString().Trim(), 1) != "*")
            {
                SQL += ComNum.VBLF + "  AND OPROOM = '" + VB.Left(cboRoom.SelectedItem.ToString().Trim(), 1) + "'";
            }

            if(strOpGbn != "")
            {
                SQL += ComNum.VBLF + "  AND OpBun IN ('" + strOpGbn + "')";         //수술구분
            }

            if(strAngbn != "")
            {
                SQL += ComNum.VBLF + "  AND AnGbn IN ('" + strAngbn + "')";         //마취구분
            }

            if(strOpGbn3 != "")
            {
                SQL += ComNum.VBLF + "  AND OpTitle IN ('" + strOpGbn3 + "')";      //마취구분
            }

            if(strOpGbnX == "X")
            {
                SQL += ComNum.VBLF + "  AND OpCancel IS NOT NULL";                  //수술취소
            }

            else
            {
                SQL += ComNum.VBLF + "  AND Opcancel IS NULL";
            }

            if(optGbn1.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND GbAngio = 'Y'";
            }

            else if(optGbn2.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND OpRoom = 'N'";
            }

            else
            {
                SQL += ComNum.VBLF + "  AND (GbAngio IS NULL OR GbAngio <> 'Y')";
                SQL += ComNum.VBLF + "  AND OpRoom <> 'N'";
            }

            if(chkWarning.Checked == true && chkOP.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND (OpErr>='1' OR OpHapSayu IS NOT NULL)";
            }

            else if(chkWarning.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND OpErr>='1'";
            }

            else if(chkOP.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND OpHapSayu IS NOT NULL";
            }

            if(VB.Left(cboIO.SelectedItem.ToString().Trim(), 1) == "2")
            {
                SQL += ComNum.VBLF + "  AND IpdOpd ='I'";
            }

            else if(VB.Left(cboIO.SelectedItem.ToString().Trim(), 1) == "3")
            {
                SQL += ComNum.VBLF + "  AND IpdOpd ='O'";
            }

            if(chkSMS.Checked == true)
            {
                SQL += ComNum.VBLF + "  AND GbSMS IS NOT NULL";
            }

            if (gPart =="OK" && chkGS.Checked == true)
            {
                SQL += ComNum.VBLF + " UNION ALL ";
                SQL += ComNum.VBLF + " SELECT  ";
                SQL += ComNum.VBLF + "   'GS' AS tGbn,a.Pano,'' OpTimeFrom,a.DeptCode,'' OpBun,TO_CHAR(ExDate,'YYYY-MM-DD') OpDate,a.OpTime OPSTIME,'' OPETIME    ";
                SQL += ComNum.VBLF + "   ,'GS' OpRoom,b.SName,'O' IPDOPD,a.Age,a.Sex,'' RoomCode,a.OpStaff DrCode    ";
                SQL += ComNum.VBLF + "   ,a.PREDIAGNOSIS DIAGNOSIS,a.OPILL OpTitle,'' AnDoct1,'' AnGbn, '' OPCANCEL,'O' pErr,'' OpHapSayu,'' OPDOCT2, '' as AsaAdd, '' as GbEr, '' as Opre, 0 as Wrtno  ";
                SQL += ComNum.VBLF + " FROM " + ComNum.DB_MED + "EXAM_ABR_SCHEDULE a  ";
                SQL += ComNum.VBLF + "  , " + ComNum.DB_PMPA + "BAS_PATIENT  b  ";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND a.Pano=b.Pano(+) ";
                SQL += ComNum.VBLF + "      AND a.Gubun='02' ";
                SQL += ComNum.VBLF + "      AND ExDate >=TO_DATE('" + strFDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "      AND ExDate <TO_DATE('"  + Convert.ToDateTime(strTDate).AddDays(1).ToShortDateString() + "','YYYY-MM-DD')";

                if (groupBox5.Visible == true)
                {
                    if (txtPano.Text != "")
                    {
                        SQL += ComNum.VBLF + "AND a.PANO = '" + txtPano.Text + "'";
                    }
                }

                else
                {
                    if (cboDept.SelectedItem.ToString().Trim() != "전체")
                    {
                        SQL += ComNum.VBLF + "AND a.DeptCode = '" + cboDept.SelectedItem.ToString().Trim() + "'";

                        if (VB.Left(cboDr.SelectedItem.ToString().Trim(), 4) != "****")
                        {
                            SQL += ComNum.VBLF + "  AND a.OpStaff = '" + VB.Left(cboDr.SelectedItem.ToString().Trim(), 4) + "'";
                        }
                    }
                }

            }

            if(optGbn1.Checked == true || optGbn2.Checked == true)
            {
                SQL += ComNum.VBLF + "ORDER BY 1,OPROOM, OpDate, OpTimeFrom";
            }

            else
            {
                SQL += ComNum.VBLF + "ORDER BY 1,OpDate,OpTimeFrom";
            }

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;

                ssList.ActiveSheet.Rows.Count = nREAD;

                for(i = 0; i < nREAD; i++)
                {
                    strOK = "OK";

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT ";
                    SQL += ComNum.VBLF + "  OpSize";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ORAN_OPBUN";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND Dept='" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'  ";
                    SQL += ComNum.VBLF + "      AND Bun=" + VB.Val(dt.Rows[i]["OpBun"].ToString().Trim()) + " ";

                    SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if(dt1.Rows.Count > 0)
                    {
                        switch (dt1.Rows[0]["OpSize"].ToString().Trim())
                        {
                            case "1":
                                strOpSize = "대";
                                break;
                            case "2":
                                strOpSize = "중";
                                break;
                            case "3":
                                strOpSize = "소";
                                break;
                            case "4":
                                strOpSize2 = "처치";
                                break;
                            case "5":
                                strOpSize2 = "교정";
                                break;
                            case "6":
                                strOpSize2 = "운동";
                                break;

                            default:
                                strOpSize = "";
                                strOpSize2 = "";
                                break;
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if(strOpGbn2 != "")
                    {
                        strOK = "";

                        switch (strOpGbn2)
                        {
                            case "4":
                                if(strOpSize2 == "처치")
                                {
                                    strOK = "OK";
                                }
                                break;

                            case "5":
                                if (strOpSize2 == "교정")
                                {
                                    strOK = "OK";
                                }
                                break;

                            case "6":
                                if (strOpSize2 == "운동")
                                {
                                    strOK = "OK";
                                }
                                break;
                        }
                    }

                    if(strOK == "OK")
                    {
                        nRow += 1;

                        ssList.ActiveSheet.Cells[nRow - 1, 0].Text = VB.Left(dt.Rows[i]["OpDate"].ToString().Trim(), 10);

                        if (gPart =="OK" && chkGS.Checked ==true)
                        {
                            if(dt.Rows[i]["OPSTIME"].ToString().Trim() == ":")
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 1].Text = "";
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["OPSTIME"].ToString().Trim();
                            }
                            
                        }
                        else
                        {
                            if (dt.Rows[i]["OPETIME"].ToString().Trim() != ":")
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["OPSTIME"].ToString().Trim() + "/" + dt.Rows[i]["OPETIME"].ToString().Trim();
                            }
                            else
                            {
                                if(dt.Rows[i]["OPSTIME"].ToString().Trim() != ":")
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 1].Text = dt.Rows[i]["OPSTIME"].ToString().Trim();
                                }
                                else
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 1].Text = "";
                                }
                                
                            }
                        }

                        if (dt.Rows[i]["OPSTIME"].ToString().Trim() != ":" && dt.Rows[i]["OPETIME"].ToString().Trim() != ":")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 2].Text = ComFunc.TimeDiff(dt.Rows[i]["OPSTIME"].ToString().Trim(), dt.Rows[i]["OPETIME"].ToString().Trim());
                        }

                        ssList.ActiveSheet.Cells[nRow - 1, 3].Text = dt.Rows[i]["OpRoom"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 4].Text = dt.Rows[i]["SName"].ToString().Trim();

                        //2015-10-12 낙상표시
                        if(dt.Rows[i]["IPDOPD"].ToString().Trim() == "I")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT ";
                            SQL += ComNum.VBLF + "  IPDNO, ROOMCODE, WARDCODE, DRGCODE, GBDRG";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "      AND TRUNC(INDATE) <=TO_DATE('" + VB.Left(dt.Rows[i]["OPDATE"].ToString().Trim(), 10) + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "      AND PANO = '" + dt.Rows[i]["Pano"].ToString().Trim() + "'";
                            SQL += ComNum.VBLF + "ORDER BY INDATE DESC";

                            SqlErr = clsDB.GetDataTableEx(ref dt2, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if(dt2.Rows.Count > 0)
                            {
                                strIPDNO = dt2.Rows[0]["IPDNO"].ToString().Trim();
                            }

                            dt2.Dispose();
                            dt2 = null;

                            if(strIPDNO != "")
                            {
                                if(clsVbfunc.READ_WARNING_FALL(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(), VB.Left(dt.Rows[i]["OpDate"].ToString().Trim(), 10), 
                                                            VB.Val(strIPDNO), dt.Rows[i]["Age"].ToString().Trim()) != "")
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 5].Text = "◈";
                                }
                            }
                        }

                        else
                        {
                            //2015-10-12 낙상표시
                            if(clsVbfunc.READ_낙상고위험체크_OPD(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(), VB.Left(dt.Rows[i]["OpDate"].ToString().Trim(), 10), 
                                                            dt.Rows[i]["Age"].ToString().Trim()) != "")
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 5].Text = "◈";
                            }
                        }

                        ssList.ActiveSheet.Cells[nRow - 1, 6].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 7].Text = dt.Rows[i]["Sex"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 8].Text = dt.Rows[i]["Age"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 9].Text = VB.Left(clsVbfunc.GetBasPatientJumin2(clsDB.DbCon, dt.Rows[i]["Pano"].ToString().Trim(),"Y"),6);
                        ssList.ActiveSheet.Cells[nRow - 1, 10].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 11].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 12].Text = CF.READ_DrName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                        ssList.ActiveSheet.Cells[nRow - 1, 13].Text = dt.Rows[i]["DIAGNOSIS"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 14].Text = dt.Rows[i]["OpTitle"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 15].Text = clsOpMain.READ_OPR_CODE(clsDB.DbCon, "E", dt.Rows[i]["OPBUN"].ToString().Trim());
                        ssList.ActiveSheet.Cells[nRow - 1, 16].Text = strOpSize2;
                        ssList.ActiveSheet.Cells[nRow - 1, 17].Text = dt.Rows[i]["AnDoct1"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 18].Text = dt.Rows[i]["AnGbn"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 19].Text = strOpSize;
                        ssList.ActiveSheet.Cells[nRow - 1, 20].Text = "무";

                        if(dt.Rows[i]["OPCANCEL"].ToString().Trim() != "")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 20].Text = "유"; 
                        }

                        ssList.ActiveSheet.Cells[nRow - 1, 21].Text = dt.Rows[i]["OpErr"].ToString().Trim();

                        if(dt.Rows[i]["OpHapSayu"].ToString().Trim() == "")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 22].Text = "무";
                        }

                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 22].Text = "@";
                        }

                        ssList.ActiveSheet.Cells[nRow - 1, 23].Text = dt.Rows[i]["OPDOCT2"].ToString().Trim();

                        ssList.ActiveSheet.Rows[nRow - 1].Height = 20;

                        if(i == ssList.ActiveSheet.Rows.Count)
                        {
                            ssList.ActiveSheet.Rows[i].Height = 20;
                        }

                        ssList.ActiveSheet.Cells[nRow - 1, 25].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 26].Text = dt.Rows[i]["AsaAdd"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 27].Text = dt.Rows[i]["GbEr"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 28].Text = dt.Rows[i]["Opre"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 29].Text = dt.Rows[i]["Wrtno"].ToString().Trim();
                    }

                    // 화면상의 정렬표시 Clear
                    ssList.ActiveSheet.ColumnHeader.Cells[0, 0, 0, ssList.ActiveSheet.ColumnCount - 1].Column.SortIndicator = FarPoint.Win.Spread.Model.SortIndicator.None;
                }
            }

            dt.Dispose();
            dt = null;

            ssList.ActiveSheet.Rows.Count = nRow;
            Cursor.Current = Cursors.Default;


        }

        void cboDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            clsOpMain.ComboDrCode_SET(clsDB.DbCon, cboDr, VB.Left(cboDept.SelectedItem.ToString().Trim(), 2), "");
        }

        void ssList_CellDoubleClick(object sender, CellClickEventArgs e)
        {            
            FpSpread o = (FpSpread)sender;

            string strPano = "";
            string strOpDate = "";
            string strSayu = "";

            if (e.Row < 0 || e.Column < 0) return;

            //마우스 우클릭
            if (e.Button == MouseButtons.Right)
            {
                return;
            }

            if (sender == this.ssList)
            {
                if (e.ColumnHeader == true)
                {
                    clsSpread.gSpdSortRow(o, e.Column);
                    return;
                }
                if (e.RowHeader == true)
                {
                    return;
                }
                

                //string strJob = s.ActiveSheet.Cells[ssList.ActiveSheet.ActiveRowIndex, (int)clsSupEndsSpd.enmSupEndsSCH01A.STS02].Text.Trim();

                if (e.Column != 21)
                {
                    return;
                }

                if (ssList.ActiveSheet.Cells[e.Row, 21].Text == "")
                {
                    return;
                }

                strOpDate = ssList.ActiveSheet.Cells[e.Row, 0].Text;
                strPano = ssList.ActiveSheet.Cells[e.Row, 4].Text;

                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  OpHapSayu";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "ORAN_MASTER";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND Pano='" + strPano + "'";
                SQL += ComNum.VBLF + "      AND OpDate=TO_DATE('" + strOpDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "      AND OpHapSayu IS NOT NULL";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    strSayu = dt.Rows[0]["OpHapSayu"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                if (strSayu != "")
                {
                    ComFunc.MsgBox("합병증", strSayu);
                }                

            }
            
        }
    }                                                                                                                               
}

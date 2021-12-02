using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using ComLibB;
using FarPoint.Win.Spread;

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewSlip1.cs
    /// Description     : 개인별 진료 내역 조회 (E,ER 항목 => 응급실 6시간 대상자)
    /// Author          : 안정수
    /// Create Date     : 2017-09-07
    /// Update History  : 2017-11-03
    /// <history>       
    /// TODO : FrmDrug폼 구현필요
    ///        frmPmpaCheckNhic 폼 호출시 ArgGkiho에 들어가는 값 확인 필요
    /// d:\psmh\OPD\oviewa\Oviewa20.frm(FrmSlipView1) => frmPmpaViewSlip1.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\Oviewa20.frm(FrmSlipView1)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewSlip1 : Form
    {
       // frmPmpaViewMsg frmPmpaViewMsg = new frmPmpaViewMsg();
        

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsPmpaFunc CPF = new clsPmpaFunc();

        string strFlagChange = "";
        string strArrDate = "";
        string strArrDept = "";
        string strArrBi = "";
        string strArrDrCode = "";
        string strArrActDate = "";

        string mstrHelpCode = "";
        //int mnJobSabun = 0;
        string mstrPano = "";
        string mstrSunext_B = "";
        string strTpe = "";
        string gstrPano = "";
        
        public frmPmpaViewSlip1()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewSlip1(string GstrPano) //2018-11-08 김해수
        {
            clsPublic.GstrPANO = gstrPano;
            gstrPano = GstrPano;           
            //CS.Spread_All_Clear(ssRate);

            //if (txtPano.Text.Length == 0) return;

            //READ_DATA();

            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewSlip1(string GstrPANO, string rType)
        {
            InitializeComponent();
            setEvent();
            mstrPano = GstrPANO;
            strTpe = rType;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
        
            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnNHic.Click += new EventHandler(eBtnEvent);
            this.cboPano.Click += new EventHandler(eBtnEvent);
            this.cboPano.KeyPress +=  new KeyPressEventHandler(eControl_KeyPress);

            this.txtPano.LostFocus += new EventHandler(eControl_LostFocus);
            this.txtPano.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtPano.GotFocus += new EventHandler(eControl_GotFocus);
    

            this.ssPat.CellDoubleClick += new CellClickEventHandler(eSpreadDClick);//더블클릭 이벤트
            this.ssSlip.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);     //Spread 에서 팝업창 실행
        }
        void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

            if (sender == ssSlip )
            {
                if (e.Column ==0 || e.Column ==1)
                {
                    ShowDrugInfo(ssSlip_Sheet1.Cells[e.Row, 0].Text);     //약품정보 조회
                }
                   
               
            }


        }

        void ShowDrugInfo(string sucode)
        {
            //clsPmpaFunc cPF = new clsPmpaFunc();

            string strSuCode = sucode;

            //frmSupDrstInfoEntryNew frm = new frmSupDrstInfoEntryNew(strSuCode);
            //frm.ShowDialog();
            //cPF.fn_ClearMemory(frm);


            frmSupDrstInfoEntryNew frmSupDrstInfoEntryNewX = new frmSupDrstInfoEntryNew(strSuCode,"BUSUGA", "VIEW");
            frmSupDrstInfoEntryNewX.ShowDialog();
           
        }

        void eSpreadDClick(object sender, CellClickEventArgs e)
        {


            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssPat, e.Column);
                return;
            }
        }
        
        void Read_BAS_OCSMEMO_O()
        {
            int i = 0;
            string strMsg = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                    ";
            SQL += ComNum.VBLF + "  MEMO                                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_O  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                 ";
            SQL += ComNum.VBLF + "      AND PANO ='" + txtPano.Text + "'    ";
            SQL += ComNum.VBLF + "      AND DDATE IS NULL                   ";

            try
            {
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
                   //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            strMsg += dt.Rows[i]["MEMO"].ToString().Trim();
                        }

                        strMsg = strMsg.Replace("`", "'");

                        if (strMsg != "")
                        {
                            //FrmMSG, WBS상에는 사용안함 처리가 되어있음
                            clsPmpaPb.GstrDoctMsg = strMsg;
                            frmPmpaViewMsg frmPmpaViewMsg = new frmPmpaViewMsg("R");
                            frmPmpaViewMsg.Show();                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        void Read_BAS_OCSMEMO_MIR()
        {
            int i = 0;
            string strMsg = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                      ";
            SQL += ComNum.VBLF + "  MEMO                                      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_OCSMEMO_MIR  ";
            SQL += ComNum.VBLF + "WHERE 1=1                                   ";
            SQL += ComNum.VBLF + "      AND PANO ='" + txtPano.Text + "'      ";
            SQL += ComNum.VBLF + "      AND DDATE IS NULL                     ";

            try
            {
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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            strMsg += dt.Rows[i]["MEMO"].ToString().Trim();
                        }

                        strMsg = strMsg.Replace("`", "'");

                        if (strMsg != "")
                        {
                            //FrmMSG, WBS상에는 사용안함 처리가 되어있음
                            clsPmpaPb.GstrDoctMsg = strMsg;
                            frmPmpaViewMsg frmPmpaViewMsg = new frmPmpaViewMsg("R");
                            frmPmpaViewMsg.Show();


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
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

                //ComFunc.SetFormInit(clsDB.DbCon, this, "y", "y", "y"); //폼 기본값 세팅 등  

                //this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
                this.ControlBox = false;

                txtID1.Text = "";
                txtID2.Text = "";
                txtID3.Text = "";
                txtID4.Text = "";
                txtID5.Text = "";

                txtPano.Text = "";
                ssList1_Sheet1.Rows.Count = 0;
                ssPat_Sheet1.Rows.Count = 0;
                strFlagChange = "";

                ssSlip_Sheet1.Columns[16].Visible = false;
                ssSlip_Sheet1.Columns[17].Visible = false;
                ssSlip_Sheet1.Columns[18].Visible = false;
                ssSlip_Sheet1.Columns[19].Visible = false;
                ssSlip_Sheet1.Columns[20].Visible = false;
                ssSlip_Sheet1.Columns[21].Visible = false;
                ssSlip_Sheet1.Columns[22].Visible = false;

                cboPano.Items.Clear();

                //등록번호 자동입력 2012-04-03 이주형
                if (mstrPano != "")
                {
                    txtPano.Text = mstrPano;
                }

                if (gstrPano != "")
                {
                    txtPano.Text = gstrPano;
                }

                CS.Spread_All_Clear(ssRate);

                if (txtPano.Text.Length == 0) return;

                READ_DATA();
            }
        }

        void READ_DATA()
        {
            Read_PAT_MAST();
            Read_OS_JINRYO();

            ssSlip_Sheet1.Columns[8].Locked = true;

            //심사계전용 clsType.User.Sabun
            switch (ComFunc.LPAD(clsType.User.Sabun, 5, "0"))
            {
                case "04349":
                case "07834":
                case "07843":
                case "13537":
                case "00468":
                case "02749":
                case "15273":
                case "13635":
                case "19399":
                case "21181":
                case "28253":
                case "17812":
                case "22456":
                case "22699":
                case "33674":
                case "37074":
                case "45316":
                case "46000":
                case "50773":
                    Read_BAS_OCSMEMO_O();
                    // Read_BAS_OCSMEMO_MIR();// '2017-08-30 청구 참고사항은 팝업으로 안뜨도록 요청함(이민주선생님과 통화함. '이 부분 심사팀 요청으로 풀었다 묶었다 하는거 같은데 주석을 풀 경우 이민주쌤과 통화 필요합니다.
                    ssSlip_Sheet1.Columns[8].Locked = false;
                    break;
            }
        }

        public void RE_READ_DATE(string GstrPano)
        {
            gstrPano = GstrPano;
            clsPublic.GstrPANO = gstrPano;

            txtID1.Text = "";
            txtID2.Text = "";
            txtID3.Text = "";
            txtID4.Text = "";
            txtID5.Text = "";

            txtPano.Text = "";
            ssList1_Sheet1.Rows.Count = 0;
            ssPat_Sheet1.Rows.Count = 0;
            strFlagChange = "";

            cboPano.Items.Clear();

            //등록번호 자동입력 2012-04-03 이주형
            if (mstrPano != "")
            {
                txtPano.Text = mstrPano;
            }

            if (gstrPano != "")
            {
                txtPano.Text = GstrPano;
            }

            CS.Spread_All_Clear(ssRate);

            if (txtPano.Text.Length == 0) return;

            READ_DATA();
        } //김해수 2018-11-16

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                txtPano.Text = "";
            this.Close();
            return;
            }

            else if (sender == this.btnNHic)
            {
                //                
                btnNHic_Click();
            }

            else if (sender == this.cboPano)
            {
                if (cboPano.SelectedIndex != -1)
                {
                    txtPano.Text = cboPano.SelectedItem.ToString().Trim();
                }

                eControl_LostFocus(txtPano, e);
            }
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            TextBox tP = (TextBox)sender;
            tP.SelectionStart = 0;
            tP.SelectionLength = tP.Text.Length;
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            string strChk = "";

            if (   sender == this.cboPano)
            {
                if (e.KeyChar == 13)
                {
                    txtPano.Text = cboPano.Text;
                    if (VB.IsNumeric(txtPano.Text))
                    {
                        READ_DATA();

                        strChk = "";


                        foreach (string s in cboPano.Items)
                        {
                            if (s == txtPano.Text)
                            {
                                strChk = "NO";
                            }
                        }

                        if (strChk == "")
                        {
                            cboPano.Items.Add(txtPano.Text);
                        }

                    }
                }

            }
            else
            {
                if (e.KeyChar == 13)
                {
                    if (VB.IsNumeric(txtPano.Text))
                    {
                        READ_DATA();

                        strChk = "";


                        foreach (string s in cboPano.Items)
                        {
                            if (s == txtPano.Text)
                            {
                                strChk = "NO";
                            }
                        }

                        if (strChk == "")
                        {
                            cboPano.Items.Add(txtPano.Text);
                        }

                    }
                }
            }
           
        }

        void eControl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.txtPano)
            {
                string strChk = "";

                if (strFlagChange == "**")
                {
                    txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
                }

               
                //등록번호 자동입력을 위한 전역 변수 2012-04-03 이주형
                mstrPano = txtPano.Text;

                strFlagChange = "";
            }
        }

        void Read_PAT_MAST()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                        ";
            SQL += ComNum.VBLF + "  TO_CHAR(StartDate, 'YYYY-MM-DD') Sdate,     ";
            SQL += ComNum.VBLF + "  TO_CHAR(LastDate, 'YYYY-MM-DD') Ldate,      ";
            SQL += ComNum.VBLF + "  Sname, Sex, Jumin1, Jumin2, Jumin3          ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT        ";
            SQL += ComNum.VBLF + "WHERE Pano = '" + txtPano.Text + "'           ";

            try
            {
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
                    //txtPano.Focus();
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }else if (dt.Rows.Count > 0)
                {
                    txtID1.Text = "수진자명 : ";
                    txtID2.Text = "성　　별 : ";
                    txtID3.Text = "주민번호 : ";
                    txtID4.Text = "최초내원 : ";
                    txtID5.Text = "최종내원 : ";

                    txtID1.Text += dt.Rows[0]["Sname"].ToString().Trim();
                    txtID2.Text += dt.Rows[0]["Sex"].ToString().Trim();
                    txtID3.Text += dt.Rows[0]["Jumin1"].ToString().Trim() + " - ";
                    txtID3.Text += clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                    txtID4.Text += dt.Rows[0]["Sdate"].ToString().Trim();
                    txtID5.Text += dt.Rows[0]["Ldate"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

        }

        void Read_OS_JINRYO()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            DataTable dt1 = null;

            ssPat_Sheet1.Rows.Count = 0;
            ssSlip_Sheet1.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                    ";
            SQL += ComNum.VBLF + "  TO_CHAR(ActDate,'YYYY-MM-DD') Adate,                                    ";
            SQL += ComNum.VBLF + "  DeptCode, DrName, Bi, o.DRCode,                                         ";
            SQL += ComNum.VBLF + "  TO_CHAR(BDATE,'YYYY-MM-DD') BDATE , MCODE, MSEQNO                       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP O, " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                 ";
            SQL += ComNum.VBLF + "      AND Pano     = '" + txtPano.Text + "'                               ";
            SQL += ComNum.VBLF + "      AND O.DrCode = B.DrCode(+)                                          ";
            SQL += ComNum.VBLF + "GROUP BY ActDate, DeptCode, DrName, Bi, o.DrCode, BDATE,  MCODE, MSEQNO   ";
            SQL += ComNum.VBLF + "ORDER BY ActDate Desc, BDATE Desc                                         ";

            try
            { 
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
                 //   ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }else if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssPat_Sheet1.Rows.Count = i + 1;

                        ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 0].Text = dt.Rows[i]["Adate"].ToString().Trim();
                        ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 1].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 2].Text = dt.Rows[i]["DrName"].ToString().Trim();

                        switch (dt.Rows[i]["Bi"].ToString().Trim())
                        {
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 3].Text = "보험";
                                break;

                            case "21":
                            case "22":
                            case "23":
                            case "24":
                            case "25":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 3].Text = "보호";
                                break;

                            case "31":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 3].Text = "산재";
                                break;

                            case "32":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 3].Text = "공무공상";
                                break;

                            case "33":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 3].Text = "산재공상";
                                break;

                            case "41":
                            case "42":
                            case "43":
                            case "44":
                            case "45":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 3].Text = "보험100%";
                                break;

                            case "52":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 3].Text = "TA보험";
                                break;

                            case "55":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 3].Text = "TA일반";
                                break;

                            default:
                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 3].Text = "일반";
                                break;
                        }

                        ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 4].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                        ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["Bi"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                                                    ";
                        SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE, 'YYYY-MM-DD') JOBDATE,                                                 ";
                        SQL += ComNum.VBLF + "  TO_CHAR(BDATE, 'YYYY-MM-DD') BDATE                                                      ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_HOANBUL                                                    ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                        SQL += ComNum.VBLF + "      AND PANO  = '" + txtPano.Text + "'                                                  ";
                        SQL += ComNum.VBLF + "      AND JOBDATE = TO_DATE('" + dt.Rows[i]["Adate"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD')   ";
                        SQL += ComNum.VBLF + "      AND DEPTCODE = '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'                   ";

                        try
                        {
                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            //if (dt1.Rows.Count == 0)
                            //{
                            //    //dt1.Dispose();
                            //    //dt1 = null;
                            //    // ComFunc.MsgBox("해당 DATA가 없습니다.");
                            //    //return;
                            //}else 
                            if (dt1.Rows.Count > 0)
                            {
                                if (dt1.Rows[0]["JOBDATE"].ToString().Trim() != dt1.Rows[0]["BDATE"].ToString().Trim())
                                {
                                    ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 6].Text = "◈";
                                }
                            }

                            ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 7].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        }
                        catch (Exception ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }
                        dt1.Dispose();
                        dt1 = null;

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT                                                                                    ";
                        SQL += ComNum.VBLF + "  BonRate ,MCODE, MSEQNO , MQCODE                                                         ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_Master                                                     ";
                        SQL += ComNum.VBLF + "WHERE 1=1                                                                                 ";
                        SQL += ComNum.VBLF + "      AND PANO  = '" + txtPano.Text + "'                                                  ";
                        SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD')   ";
                        SQL += ComNum.VBLF + "      AND DEPTCODE = '" + dt.Rows[i]["DeptCode"].ToString().Trim() + "'                   ";

                        try
                        {
                            SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                //return;
                            }
                            //if (dt1.Rows.Count == 0)
                            //{
                            //    dt1.Dispose();
                            //    dt1 = null;
                            //    //ComFunc.MsgBox("해당 DATA가 없습니다."); //JJY (2018-06-11) 심사팀에서 메세지 않뜨도록 요청함
                            //    return;
                            //} 
                            if (dt1.Rows.Count > 0)
                            {

                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 8].Text = (dt1.Rows[0]["BonRate"].ToString().Trim() == "E" ? "E" : "");
                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 9].Text = (dt1.Rows[0]["MCode"].ToString().Trim());
                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 10].Text = (dt1.Rows[0]["MQCode"].ToString().Trim());
                                ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 11].Text = (dt1.Rows[0]["MSeqNo"].ToString().Trim() );

                            }

                            ssPat_Sheet1.Cells[ssPat_Sheet1.Rows.Count - 1, 7].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                           
                        }
                        catch (Exception ex)
                        {
                            ComFunc.MsgBox(ex.Message);
                            clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        }
                        dt1.Dispose();
                        dt1 = null;
                    }

                    ssPat_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

        }

        void btnNHic_Click()
        {
            string strSname = "";
            string strPano = "";
            string strJumin = "";
            string strDEPTCODE = "";
            string strBDate = "";

            strBDate = DateTime.Now.ToString("yyyy-MM-dd");
            strSname = VB.Pstr(txtID1.Text.Trim(), ":", 2);
            strPano = txtPano.Text;
            strJumin = (VB.Pstr(txtID3.Text.Trim(), ":", 2).Replace("-", "")).Trim().Replace(" ", "");

            //2017-11-03 안정수
            //자격관리를 위해 주민번호 암호화 -> 복호화
            #region
            string strOrgJumin = clsAES.Read_Jumin_AES(clsDB.DbCon, strPano);
            string strJumin1 = VB.Left(strOrgJumin, 6);
            string strJumin2 = VB.Mid(strOrgJumin, 7, 7);
            #endregion

            strDEPTCODE = strArrDept;

            if (strPano == "")
            {
                ComFunc.MsgBox("환자등록번호가 공란입니다..");
                return;
            }

            if (strDEPTCODE == "")
            {
                strDEPTCODE = "ME";
                ComFunc.MsgBox("최종진료과가 없습니다.. 자격조회시 ME로 자격조회합니다...");
            }

            if (strPano == "06927136")
            {
                strSname = "마리벨시파곤산";
            }

            //2012-09-11
            if (VB.I(strSname, "A") > 1 || VB.I(strSname, "B") > 1 || VB.I(strSname, "C") > 1 || VB.I(strSname, "D") > 1)
            {
                if (MessageBox.Show("성명에 A,B,C,D가 포함되어 있습니다...성명수정후 자격조회 하시겠습니까??", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    strSname = strSname;
                    return;
                }
                else
                {
                    strSname = VB.InputBox("성명을 다시 확인하십시오", "이름확인", strSname);
                }
            }

            mstrHelpCode = "";
            mstrHelpCode = strPano + "," + strDEPTCODE + "," + strSname + "," + strJumin + "," + strBDate;

            //TODO : frmPmpaCheckNhic 폼 호출시 ArgGkiho에 들어가는 값 확인 필요

            frmPmpaCheckNhic frmPmpaCheckNhicX = new frmPmpaCheckNhic(strPano, strDEPTCODE, strSname, strJumin1, strJumin2, strBDate, null);
            frmPmpaCheckNhicX.Show();

            mstrHelpCode = "";
        }

        public string Read_Opd_Jepsu_Gubun(string ArgPano, string ArgBDate, string ArgDept)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                        ";
            SQL += ComNum.VBLF + "  JIN, JINdtL, JepsuSayu                                      ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_MASTER                         ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                     ";
            SQL += ComNum.VBLF + "      AND Pano ='" + ArgPano + "'                             ";
            SQL += ComNum.VBLF + "      AND BDate = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')    ";
            SQL += ComNum.VBLF + "      AND DEPTCODE ='" + ArgDept + "'                         ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["JepsuSayu"].ToString().Trim();

                    if (Read_JindtlName(dt.Rows[0]["JINdtL"].ToString().Trim()) != "")
                    {
                        rtnVal += " (" + Read_JindtlName(dt.Rows[0]["JINdtL"].ToString().Trim()) + ")";
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            return rtnVal;

        }

        //외래접수 진찰료 수납구분명
        public string Read_Jin(string ArgJin)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                ";
            SQL += ComNum.VBLF + "  Name                                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_OPDJIN ";
            SQL += ComNum.VBLF + "WHERE Code='" + ArgJin + "'           ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["Name"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            return rtnVal;
        }

        public string Read_JindtlName(string ArgCode)
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                ";
            SQL += ComNum.VBLF + "  Name                                ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE  ";
            SQL += ComNum.VBLF + "WHERE 1=1                             ";
            SQL += ComNum.VBLF + "      AND GUBUN  = '외래접수상세코드' ";
            SQL += ComNum.VBLF + "      AND CODE = '" + ArgCode + "'    ";
            SQL += ComNum.VBLF + "ORDER BY CODE                         ";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장

                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return rtnVal;

                }

                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["Name"].ToString().Trim();
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;


            return rtnVal;
        }
        
        void ssPat_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strDate = "";
            

            if (e.ColumnHeader ==true || e.RowHeader == true )return;
            
            strDate = ssPat_Sheet1.Cells[e.Row, 0].Text;
            strArrActDate = ssPat_Sheet1.Cells[e.Row, 0].Text;
            strArrDept = ssPat_Sheet1.Cells[e.Row, 1].Text;
            strArrDrCode = ssPat_Sheet1.Cells[e.Row, 4].Text;
            strArrBi = ssPat_Sheet1.Cells[e.Row, 5].Text;
            strArrDate = ssPat_Sheet1.Cells[e.Row, 7].Text;

            CS.Spread_All_Clear(ssRate);

            Read_OPD_SLIP(strDate, strArrDept, strArrDrCode, strArrBi, strArrDate);
        }

        void Read_OPD_SLIP(string ArgDate, string ArgDept, string ArgDrCode, string ArgBi, string ArgBDate)
        {
            int i = 0;

            string strNew = "";
            string strOld = "";

            string strTemp = "";
            int nSeqNo = 0;
            int nREAD = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            label1.Text = Read_Opd_Jepsu_Gubun(txtPano.Text, ArgBDate, ArgDept);

            ssSlip_Sheet1.Rows.Count = 0;
            ssSlip.ActiveSheet.RowCount = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                                     ";
            SQL += ComNum.VBLF + "  Sucode, SunameK, BaseAmt, Qty, Nal,o.bun,o.bi,o.nu,b.SUGBQ,                                              ";
            SQL += ComNum.VBLF + "  GbSpc, GbNgt, GbGisul, GbSelf, GbChild,                                                                  ";
            SQL += ComNum.VBLF + "  Amt1, Amt2, SeqNo, Part, O.Rowid, O.GbSlip, O.GbSuGbS                                                    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP O, " + ComNum.DB_PMPA + "BAS_SUN B                                     ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                                                  ";
            SQL += ComNum.VBLF + "      AND ActDate  = TO_DATE('" + ArgDate + "','YYYY-MM-DD')                                               ";
            SQL += ComNum.VBLF + "      AND Pano     = '" + txtPano.Text + "'                                                                ";
            SQL += ComNum.VBLF + "      AND Bi       = '" + ArgBi + "'                                                                       ";
            SQL += ComNum.VBLF + "      AND BDATE = TO_DATE('" + ArgBDate + "','YYYY-MM-DD')                                                 ";
            SQL += ComNum.VBLF + "      AND DeptCode = '" + ArgDept + "'                                                                     ";
            SQL += ComNum.VBLF + "      AND DrCode = '" + ArgDrCode + "'                                                                     ";
            SQL += ComNum.VBLF + "      AND O.Sunext = B.Sunext                                                                              ";
            if (chkF.Checked == false)
            {
                SQL += ComNum.VBLF + "ORDER BY  SeqNo, O.Bun, O.Sucode, O.Sunext                                                             ";
            }
            else
            {
                SQL += ComNum.VBLF + "ORDER BY SeqNo,decode(o.bun,'92','99','96','99','98','99','99','99',gbself) , O.Bun, O.Sucode, O.Sunext";
            }


            try
            {
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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }else if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;

                    for (i = 0; i < nREAD; i++)
                    {
                        ssSlip_Sheet1.Rows.Count = i + 1;

                        strNew = dt.Rows[i]["SeqNo"].ToString().Trim();
                        nSeqNo = Convert.ToInt32(VB.Val(strNew));

                        if (strNew != strOld)
                        {
                            if (i != 0)
                            {
                                strTemp = READ_SLIP_nHIC_STS(dt.Rows[i]["Part"].ToString().Trim(), dt.Rows[i]["SeqNo"].ToString().Trim());
                                strTemp = ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 12].Text;
                                //.SetCellBorder 1, .Row, .MaxCols, .Row, SS_BORDER_TYPE_TOP Or SS_BORDER_TYPE_OUTLINE, &HFF0000, SS_BORDER_STYLE_SOLID  '2012-11-30
                                FarPoint.Win.LineBorder lb = new FarPoint.Win.LineBorder(Color.Blue, 1, false, true, false, false); 
                                ssSlip.ActiveSheet.Rows[ssSlip_Sheet1.Rows.Count - 1].Border = lb;
                            }
                            strOld = dt.Rows[i]["SeqNo"].ToString().Trim();
                        }

                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 0].Text = dt.Rows[i]["Sucode"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 1].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 2].Text = String.Format("{0:##,###,##0}", VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()));
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 3].Text = String.Format("{0:#0.0}", VB.Val(dt.Rows[i]["Qty"].ToString().Trim()));
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 4].Text = String.Format("{0:##0}", VB.Val(dt.Rows[i]["Nal"].ToString().Trim()));
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 5].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 6].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 7].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 8].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 9].Text = dt.Rows[i]["GbChild"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 10].Text = String.Format("{0:##,###,##0}", VB.Val(dt.Rows[i]["Amt1"].ToString().Trim()));
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 11].Text = String.Format("{0:####,##0}", VB.Val(dt.Rows[i]["Amt2"].ToString().Trim()));

                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 13].Text = String.Format("{0:###0}", VB.Val(dt.Rows[i]["SeqNo"].ToString().Trim()));
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 14].Text = dt.Rows[i]["Part"].ToString().Trim();

                        if (dt.Rows[i]["GbSlip"].ToString().Trim() == "Z" || dt.Rows[i]["GbSlip"].ToString().Trim() == "E" || dt.Rows[i]["GbSlip"].ToString().Trim() == "Q")
                        {
                            ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 15].Text = "E";
                        }

                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 16].Text = dt.Rows[i]["RowID"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 17].Text = dt.Rows[i]["Bun"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 18].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 19].Text = dt.Rows[i]["Bi"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 20].Text = dt.Rows[i]["Nu"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 21].Text = dt.Rows[i]["Nu"].ToString().Trim();
                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 22].Text = dt.Rows[i]["SUGBQ"].ToString().Trim();

                        if (i == nREAD - 1)
                        {
                            strTemp = READ_SLIP_nHIC_STS(dt.Rows[i]["Part"].ToString().Trim(), nSeqNo.ToString());
                            ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 12].Text = strTemp;
                        }

                        ssSlip_Sheet1.Cells[ssSlip_Sheet1.Rows.Count - 1, 23].Text = dt.Rows[i]["GBSUGBS"].ToString().Trim();

                        if (CF.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim()) == "6" || CF.READ_BAS_Sun_S(clsDB.DbCon, dt.Rows[i]["Sucode"].ToString().Trim()) == "7")
                        {
                            ssSlip_Sheet1.Rows[ssSlip_Sheet1.Rows.Count - 1].BackColor = Color.GreenYellow;
                        }
                        else
                        {
                            ssSlip_Sheet1.Rows[ssSlip_Sheet1.Rows.Count - 1].BackColor = Color.White;
                        }

                        strOld = dt.Rows[i]["SeqNo"].ToString().Trim();
                    }

                    ssList1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            ssList1_Sheet1.Rows.Count = 0;

            //JJY(2005-02-17) 심사계요청으로 추가
            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  A.ILLCODE, B.ILLNAMEK";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_OILLS A, " + ComNum.DB_PMPA + "BAS_ILLS B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND a.BDate  = TO_DATE('" + strArrDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND a.PTno     = '" + txtPano.Text + "'";
            SQL += ComNum.VBLF + "      AND A.DeptCode = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "      AND b.IllClass ='1'";
            SQL += ComNum.VBLF + "      AND A.ILLCODE = B.ILLCODE";
            SQL += ComNum.VBLF + "ORDER BY A.SEQNO ";

            try
            {
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
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");  //JJY (2018-06-11) 심사팀에서 메세지 않뜨도록 요청함
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssList1_Sheet1.Rows.Count = i + 1;

                        ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 0].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                        ssList1_Sheet1.Cells[ssList1_Sheet1.Rows.Count - 1, 1].Text = dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        string READ_SLIP_nHIC_STS(string ArgPart, string ArgSeq)
        {
            string rtnVal = "";
            string strTemp = "";

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                        ";
            SQL += ComNum.VBLF + "  PANO,VCODE,MCODE,JIN,JINdtL,ETCAMT,ETCAMT2,JINdtL2,GELCODE,Remark,amt       ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SUNAP                                          ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
            SQL += ComNum.VBLF + "      AND PANO ='" + txtPano.Text + "'                                        ";
            SQL += ComNum.VBLF + "      AND (DEPTCODE ='" + strArrDept + "' OR DEPTCODE ='II')                  ";
            SQL += ComNum.VBLF + "      AND ACTDATE =TO_DATE('" + strArrActDate + "','YYYY-MM-DD')              ";
            SQL += ComNum.VBLF + "      AND (BDATE =TO_DATE('" + strArrDate + "','YYYY-MM-DD') OR BDATE IS NULL)";
            SQL += ComNum.VBLF + "      AND PART ='" + ArgPart + "'                                             ";
            SQL += ComNum.VBLF + "      AND SEQNO2 =" + ArgSeq + "                                              ";
            SQL += ComNum.VBLF + "      AND (DELDATE IS NULL OR DELDATE ='')                                    ";

            //접수비
            if (ArgSeq == "0" || ArgSeq == "" || ArgSeq == null)
            {
                SQL += ComNum.VBLF + "  AND REMARK IN ('접수비')                                                ";
            }

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }

                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                strTemp = "";

                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["VCODE"].ToString().Trim() != "")
                    {
                        strTemp += dt.Rows[0]["VCODE"].ToString().Trim() + " ";
                    }

                    if (dt.Rows[0]["mCODE"].ToString().Trim() != "")
                    {
                        strTemp += dt.Rows[0]["mCODE"].ToString().Trim() + " ";
                    }

                    if (dt.Rows[0]["Jindtl2"].ToString().Trim() != "")
                    {
                        switch (dt.Rows[0]["Jindtl2"].ToString().Trim())
                        {
                            case "01":
                                strTemp += "장루.요루";
                                break;

                            case "02":
                                strTemp += "완전틀니";
                                break;
                        }
                    }

                    if (Convert.ToInt32(VB.Val(dt.Rows[0]["amt"].ToString().Trim())) < 0)
                    {
                        strTemp = "";
                    }
                    else
                    {
                        strTemp = strTemp.Trim();
                    }
                }
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;

            if (strTemp.Trim() != "")
            {
                rtnVal = strTemp;
            }

            return rtnVal;
        }

        void ssSlip_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {


            if (strTpe == "MIR") return;

            string strPano = "";
            string strActDate = "";
            string strBDate = "";
            string strBi = "";
            string strDept = "";
            string strPart = "";
            int nSeqNo = 0;

            strPano = txtPano.Text.Trim();
            strActDate = strArrActDate;
            strBDate = strArrDate;

            strBi = strArrBi;
            strDept = strArrDept;

            if (e.Column > 0 && e.Row > 0)
            {
                nSeqNo = Convert.ToInt32(VB.Val(ssSlip_Sheet1.Cells[e.Row, 13].Text));
                strPart = ssSlip_Sheet1.Cells[e.Row, 14].Text;

                //TODO 확인필요
                CPF.Read_Patient_Rate_Chk(clsDB.DbCon, ssRate_Sheet1, "O", strPano, strActDate, strBDate, strBi, strDept, strPart, nSeqNo, 0, 0);
            }

        }

        void ssSlip_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {

            if (strTpe == "MIR") return;
            if (e.Column != 8)
            {
                return;
            }

            string strROWID = "";
            string strF = "";
            string strOldF = "";
            string strNu = "";
            string strOldNu = "";
            string strSuCode = "";
            string strBi = "";
            string strQ = "";

            int nAmt1 = 0;
            int nAmt2 = 0;

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;


            strSuCode = ssSlip_Sheet1.Cells[e.Row, 0].Text;
            strF = ssSlip_Sheet1.Cells[e.Row, 8].Text;
            strOldF = ssSlip_Sheet1.Cells[e.Row, 18].Text;
            strBi = ssSlip_Sheet1.Cells[e.Row, 19].Text;

            strNu = ssSlip_Sheet1.Cells[e.Row, 20].Text;
            strOldNu = ssSlip_Sheet1.Cells[e.Row, 21].Text;
            strQ = ssSlip_Sheet1.Cells[e.Row, 22].Text;

            strOldNu = ssSlip_Sheet1.Cells[e.Row, 10].Text.Replace(",", "");
            strQ = ssSlip_Sheet1.Cells[e.Row, 11].Text.Replace(",", "");

            strROWID = ssSlip_Sheet1.Cells[e.Row, 16].Text;


            if ((nAmt1 + nAmt2) != 0)
            {
                ComFunc.MsgBox("금액발생 수가는 수정불가함!!");
                strOldF = ssSlip_Sheet1.Cells[e.Row, 8].Text = "";
            }

            if (strF != "0" && strF != "1" && strF != "2")
            {
                ComFunc.MsgBox("F항의 값은 0, 1, 2 값만 허용됩니다.");
                strOldF = ssSlip_Sheet1.Cells[e.Row, 8].Text;
            }

            //누적재정리 2012-11-12
            if (strBi == "31" && String.Compare(strQ, "0") > 1)
            {
                switch (strOldNu)
                {
                    case "38":
                        strNu = "20";
                        break;
                }
            }

            else if (String.Compare(strBi, "40") < 1)
            {
                if (strF == "2")
                {
                    switch (strOldNu)
                    {
                        case "01":
                        case "02":
                        case "03":
                            strNu = "21";
                            break;

                        case "04":
                        case "05":
                        case "06":
                        case "07":
                        case "08":
                        case "09":
                        case "10":
                        case "11":
                        case "12":
                        case "13":
                        case "14":
                        case "15":
                            strNu = ComFunc.SetAutoZero((VB.Val(strOldNu) + 18).ToString(), 2);
                            break;

                        case "16":
                            strNu = "34";
                            break;
                        case "17":
                            strNu = "42";
                            break;
                        case "18":
                            strNu = "38";
                            break;
                        case "19":
                            strNu = "37";
                            break;
                        case "20":
                            strNu = "27";
                            break;
                    }
                }

                else if (strF == "0" || strF == "")
                {
                    //기존 수가 읽기 2012-11-12

                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT                                                                        ";
                    SQL += ComNum.VBLF + "  Sucode,Bun,Nu,SugbA,SugbB,SugbC,                                            ";
                    SQL += ComNum.VBLF + "  SugbD,SugbE,SugbF,SugbG,SugbH,SugbI,SugbJ, n.SugbQ, n.SugbR,n.SugbS, Iamt,  ";
                    SQL += ComNum.VBLF + "  Tamt,Bamt,TO_CHAR(Sudate, 'yyyy-mm-dd') Suday,                              ";
                    SQL += ComNum.VBLF + "  OldIamt,Oldtamt,OldBamt,DayMax,TotMax, t.Sunext,                            ";
                    SQL += ComNum.VBLF + "  TO_CHAR(t.Sudate3, 'yyyy-mm-dd') Sudate3,                                   ";
                    SQL += ComNum.VBLF + "  t.Iamt3, t.Tamt3, t.Bamt3, TO_CHAR(t.Sudate4, 'yyyy-mm-dd') Sudate4,        ";
                    SQL += ComNum.VBLF + "  t.Iamt4, t.Tamt4, t.Bamt4, TO_CHAR(t.Sudate5, 'yyyy-mm-dd') Sudate5,        ";
                    SQL += ComNum.VBLF + "  t.Iamt5, t.Tamt5, t.Bamt5,                                                  ";
                    SQL += ComNum.VBLF + "  Sunamek,SuHam,Unit,Hcode,TO_CHAR(DelDate,'YYYY-MM-DD') DelDay               ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SUT t, " + ComNum.DB_PMPA + "BAS_SUN n         ";
                    SQL += ComNum.VBLF + "WHERE 1=1                                                                     ";
                    SQL += ComNum.VBLF + "      AND t.Sucode = '" + strSuCode + "'                                      ";
                    SQL += ComNum.VBLF + "      AND T.SuNext = n.SuNext(+)                                              ";

                    try
                    {
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
                            ComFunc.MsgBox("해당 DATA가 없습니다.");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            strNu = dt.Rows[0]["Nu"].ToString().Trim();
                        }
                    }
                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                    dt.Dispose();
                    dt = null;

                    clsDB.setBeginTran(clsDB.DbCon);
                    //외래 처방 수정 2012-11-12

                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "OPD_SLIP" + "SET";
                    SQL += ComNum.VBLF + "GBSELF = '" + strF + "' ,                    ";
                    SQL += ComNum.VBLF + "Nu ='" + strNu + "'                          ";
                    SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'             ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    try
                    {
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }

                    //원외약제 루틴
                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_MED + "OCS_OUTDRUG" + "SET";
                    SQL += ComNum.VBLF + "GBSELF = '" + strF + "'";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "      AND PANO = '" + txtPano.Text + "'";
                    SQL += ComNum.VBLF + "      AND SLIPDATE = TO_DATE('" + strArrDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "      AND SUCODE = '" + strSuCode + "'";
                    SQL += ComNum.VBLF + "      AND DEPTCODE = '" + strArrDept + "'";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    try
                    {
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                        }
                    }
                    catch (Exception ex)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    ComFunc.MsgBox("저장하였습니다.");
                    Cursor.Current = Cursors.Default;
                }
            }
        }

        void ssSlip_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (strTpe == "MIR") return;
            if (e.Column == 0)
            {
                mstrSunext_B = ssSlip_Sheet1.Cells[e.Row, 0].Text;

                //TODO : FrmDrug 폼 구현 필요
            }
        }

        void txtPano_TextChanged(object sender, EventArgs e)
        {
            //if (String.Compare(txtID1.Text, "") > 1)
            //{
            //    txtID1.Text = "";
            //    txtID2.Text = "";
            //    txtID3.Text = "";
            //    txtID4.Text = "";
            //    txtID5.Text = "";

            //    ssPat_Sheet1.Rows.Count = 0;
            //    ssSlip_Sheet1.Rows.Count = 0;
            //}
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            Read_OS_JINRYO();
            Cursor.Current = Cursors.Default;
        }

        private void txtPano_Leave(object sender, EventArgs e)
        {
            //txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

            //if (VB.IsNumeric(txtPano.Text))
            //{
            //    Read_PAT_MAST();
            //    Read_OS_JINRYO();
            //}
        }

        private void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtPano.Text = VB.Val(txtPano.Text).ToString("00000000");

                if (VB.IsNumeric(txtPano.Text))
                {
                    Read_PAT_MAST();
                    Read_OS_JINRYO();
                }
            }
        }
    }
}

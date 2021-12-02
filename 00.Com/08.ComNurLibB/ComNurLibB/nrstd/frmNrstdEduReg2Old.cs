using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB;
using ComBase;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmNrstdEduReg2Old.cs
    /// Description     : 개인별 교육등록
    /// Author          : 안정수
    /// Create Date     : 2018-02-01
    /// TODO : 클래스변수 사용하였으므로 테스트 필요함
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 Frm교육관리등록2.frm(Frm교육관리등록2) 폼 frmNrstdEduReg2Old.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrstd\Frm교육관리등록2.frm(Frm교육관리등록2) >> frmNrstdEduReg2Old.cs 폼이름 재정의" />
    public partial class frmNrstdEduReg2Old : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();


        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

        int intRowAffected = 0;

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

        public frmNrstdEduReg2Old(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmNrstdEduReg2Old()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            //this.btnView.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnNew.Click += new EventHandler(eBtnClick);
            this.btnInfo.Click += new EventHandler(eBtnClick);

            this.cboManJong.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtFDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtTDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtJumsu.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtMan.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtPlace.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtRemark.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtSeqNo.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtTime.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.txtTopic.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            //this.eControl.LostFocus += new EventHandler(eControl_LostFocus);
            //this.eControl.GotFocus += new EventHandler(eControl_GotFocus);
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
                ComFunc.ReadSysDate(clsDB.DbCon);
                Set_Init();
            }
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

            else if (sender == this.btnDel)
            {
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eDelData();
            }

            else if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eSaveData();
            }

            else if (sender == this.btnInfo)
            {
                btnInfo_Click();
            }
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(sender == this.cboManJong || sender == this.txtFDate || sender == this.txtTDate ||
               sender == this.txtJumsu || sender == this.txtMan || sender == this.txtPlace ||
               sender == this.txtRemark || sender == this.txtSeqNo || sender == this.txtTime ||
               sender == this.txtTopic)
            {
                if(e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }
        void Set_Init()
        {
            //int i = 0;
            string strSabun = "";

            ssList3.Visible = false;
            //ssList3.ActiveSheet.Visible = false;
            //panel2.Visible = true;
            //ssList.Visible = true;

            ssList.ActiveSheet.Columns[7].Visible = false;

            SCREEN_CLEAR();

            cboJong.Items.Clear();
            cboJong.Items.Add(" ");
            cboJong.Items.Add("01.병동교육");
            cboJong.Items.Add("02.감염교육");
            cboJong.Items.Add("03.QI교육");
            cboJong.Items.Add("04.CS교육");
            cboJong.Items.Add("05.CPR교육");
            cboJong.Items.Add("06.학술강좌");
            cboJong.Items.Add("07.간호부주최 직무교육");
            cboJong.Items.Add("08.전직원교육");
            cboJong.Items.Add("09.특강(간협)");
            cboJong.Items.Add("10.연수교육");
            cboJong.Items.Add("11.10대질환");
            cboJong.Items.Add("12.보수교육");
            cboJong.Items.Add("13.기타Report");
            cboJong.Items.Add("14.강사활동(교육)");
            cboJong.Items.Add("15.프리셉터교육");
            cboJong.Items.Add("16.Cyber 교육");
            cboJong.Items.Add("17.승진자교육");
            cboJong.Items.Add("18.기타");
            cboJong.SelectedIndex = 0;

            cboManJong.Items.Clear();
            cboManJong.Items.Add(" ");
            cboManJong.Items.Add("01.의사");
            cboManJong.Items.Add("02.전담간호사");
            cboManJong.Items.Add("03.병동간호사");
            cboManJong.Items.Add("04.타부서의뢰");
            cboManJong.Items.Add("05.기타");
            cboManJong.Items.Add("06.간호사");
            cboJong.SelectedIndex = 0;

            txtName.Text = "[등록자 : " + clsType.User.JobName + " ]  사번 : " + clsType.User.IdNumber;

            if(String.Compare(clsType.User.IdNumber, "99999") <= 0)
            {
                strSabun = ComFunc.SetAutoZero(clsType.User.IdNumber, 5);
            }

            else
            {
                strSabun = ComFunc.SetAutoZero(clsType.User.IdNumber, 6);
            }

            //개인정보 읽기
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  c.Code, a.Sabun,TO_CHAR(a.IpsaDay,'YYYY-MM-DD') IpsaDay,a.Jik,a.KorName,a.Buse,b.Name BuseName,c.Name JikName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_ERP + "INSA_MST a, " + ComNum.DB_PMPA + "BAS_BUSE b, " + ComNum.DB_ERP + "INSA_CODE c";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND a.IpsaDay<=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')";
            SQL += ComNum.VBLF + "      AND (a.ToiDay IS NULL OR a.ToiDay>=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'))";
            SQL += ComNum.VBLF + "      AND a.Sabun  ='" + strSabun + "'";
            SQL += ComNum.VBLF + "      AND a.Buse=b.BuCode(+)";
            SQL += ComNum.VBLF + "      AND a.Jik=c.Code(+)";
            SQL += ComNum.VBLF + "      AND c.Gubun='2'";   //직책
            SQL += ComNum.VBLF + "ORDER BY c.Code, a.Sabun, a.KorName";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                txtPInfo.Text += "[ 부서 : " + dt.Rows[0]["BuseName"].ToString().Trim() + " ] ";
                txtPInfo.Text += "[ 직책 : " + dt.Rows[0]["JikName"].ToString().Trim() + " ] ";
                txtPInfo.Text += "[ 입사일자 : " + dt.Rows[0]["IpsaDay"].ToString().Trim() + " ] ";

                txtPsts.Text += dt.Rows[0]["Buse"].ToString().Trim() + "^^";
                txtPsts.Text += dt.Rows[0]["Jik"].ToString().Trim() + "^^";
                txtPsts.Text += dt.Rows[0]["IpsaDay"].ToString().Trim() + "^^";
            }

            eGetData();
        }

        void SCREEN_CLEAR()
        {
            int i = 0;

            ComFunc.SetAllControlClear(panel1);

            cboJong.Text = "";
            cboManJong.Text = "";

            RadioButton[] OptTime = new RadioButton[5] { optTime0, optTime1, optTime2, optTime3, optTime4 };
            RadioButton[] OptPlace = new RadioButton[2] { optPlace0, optPlace1 };

            for(i = 0; i < OptTime.Length; i++)
            {
                OptTime[i].Checked = false;
            }

            for(i = 0; i < OptPlace.Length; i++)
            {
                OptPlace[i].Checked = false;
            }

            btnSave.Enabled = false;
            btnDel.Enabled = false;
            btnNew.Enabled = true;
        }

        void btnNew_Click()
        {
            // 소스는 있지만 버튼 자체가 없으므로 주석처리

            //Dim strSEQNO        As String

            //Call READ_SYSDATE
            //Call SCREEN_CLEAR

            //strSEQNO = READ_NUR_EDU_SEQ
            //Call AdoCloseSet(AdoRes)

            //CmdSave.Enabled = True
            //Frame_Sub.Enabled = True
            //TxtSeqNo.Text = strSEQNO
            //TxtTopic.SetFocus
        }

        void eGetData()
        {
            int i = 0;

            CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  WRTNO,SABUN,SNAME,BUCODE,IPSADATE,SDATE,JIKJONG,EDUJONG,EDUNAME,FrDate,ToDate,";
            SQL += ComNum.VBLF + "  OptTime,EDUTIME , MAN, OptPlace,PLACE, JUMSU, GUBUN, ENTDATE,ROWID ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_MST";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND Gubun ='1'";

            if(clsPublic.GstrHelpCode != "")
            {
                SQL += ComNum.VBLF + "  AND WRTNO ='" + clsPublic.GstrHelpCode + "' ";
            }

            else
            {
                if(clsType.User.IdNumber != "4349")
                {
                    SQL += ComNum.VBLF + "AND Sabun ='" + clsType.User.IdNumber + "'";
                }
            }

            SQL += ComNum.VBLF + "      AND EntDate >= TRUNC(SYSDATE - 600 )";
            SQL += ComNum.VBLF + "      AND CERT IS NULL ";
            SQL += ComNum.VBLF + "Order By EntDate ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            if(dt.Rows.Count > 0)
            {
                ssList.ActiveSheet.Rows.Count = dt.Rows.Count;

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["WRTNO"].ToString().Trim();

                    switch (Convert.ToInt32(VB.Val(dt.Rows[i]["EduJong"].ToString().Trim())))
                    {
                        case 1:
                            ssList.ActiveSheet.Cells[i, 1].Text = "병동";
                            break;

                        case 2:
                            ssList.ActiveSheet.Cells[i, 1].Text = "감염";
                            break;

                        case 3:
                            ssList.ActiveSheet.Cells[i, 1].Text = "QI";
                            break;

                        case 4:
                            ssList.ActiveSheet.Cells[i, 1].Text = "CS";
                            break;

                        case 5:
                            ssList.ActiveSheet.Cells[i, 1].Text = "CPR";
                            break;

                        case 6:
                            ssList.ActiveSheet.Cells[i, 1].Text = "학술";
                            break;

                        case 7:
                            ssList.ActiveSheet.Cells[i, 1].Text = "직무";
                            break;

                        case 8:
                            ssList.ActiveSheet.Cells[i, 1].Text = "전직원";
                            break;

                        case 9:
                            ssList.ActiveSheet.Cells[i, 1].Text = "특강";
                            break;

                        case 10:
                            ssList.ActiveSheet.Cells[i, 1].Text = "연수";
                            break;

                        case 11:
                            ssList.ActiveSheet.Cells[i, 1].Text = "10대";
                            break;

                        case 12:
                            ssList.ActiveSheet.Cells[i, 1].Text = "보수";
                            break;

                        case 13:
                            ssList.ActiveSheet.Cells[i, 1].Text = "Report";
                            break;

                        case 14:
                            ssList.ActiveSheet.Cells[i, 1].Text = "강사";
                            break;

                        case 15:
                            ssList.ActiveSheet.Cells[i, 1].Text = "프리셉터";
                            break;

                        case 16:
                            ssList.ActiveSheet.Cells[i, 1].Text = "Cyber";
                            break;

                        case 17:
                            ssList.ActiveSheet.Cells[i, 1].Text = "승진자";
                            break;

                        default:
                            ssList.ActiveSheet.Cells[i, 1].Text = "기타";
                            break;
                    }

                    ssList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["EduName"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["FrDate"].ToString().Trim();

                    if(dt.Rows[i]["ToDate"].ToString().Trim() != "")
                    {
                        ssList.ActiveSheet.Cells[i, 3].Text += "~" + dt.Rows[i]["ToDate"].ToString().Trim();
                    }

                    switch (dt.Rows[i]["OptTime"].ToString().Trim())
                    {
                        case "0":
                            ssList.ActiveSheet.Cells[i, 4].Text = "10분";
                            break;

                        case "1":
                            ssList.ActiveSheet.Cells[i, 4].Text = "30분내";
                            break;

                        case "2":
                            ssList.ActiveSheet.Cells[i, 4].Text = "1시간내";
                            break;

                        case "3":
                            ssList.ActiveSheet.Cells[i, 4].Text = "90분";
                            break;

                        case "4":
                            ssList.ActiveSheet.Cells[i, 4].Text = "2시간";
                            break;
                    }

                    if(dt.Rows[i]["EDUTime"].ToString().Trim() != "")
                    {
                        ssList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["EDUTime"].ToString().Trim();
                    }

                    switch (dt.Rows[i]["OptPlace"].ToString().Trim())
                    {
                        case "0":
                            ssList.ActiveSheet.Cells[i, 5].Text = "마리아홀";
                            break;

                        case "1":
                            ssList.ActiveSheet.Cells[i, 5].Text = "466호실";
                            break;

                    }

                    ssList.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["Man"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
        }

        void btnInfo_Click()
        {
            ssList3.Visible = true;
            //ssList3.ActiveSheet.Visible = true;       
            return;
        }

        void eSaveData()
        {
            int i = 0;

            string strROWID = "";
            string strBuCode = "";

            string strWRTNO = "";
            string strSabun = "";
            string strIpSaDay = "";
            //string strJikJong = "";            
            
            string strTopic = "";
            string strJik = "";
            string strOptTime = "";
            string strOptPlace = "";
            string strEduDate = "";
            string strEduDate2 = "";
            string strEduTime = "";
            
            string strPlace = "";
            string strMan = "";
            string strRemark = "";
            string strJumsu = "";
            string strJong = "";
            string strSName = "";
            string strManJong = "";

            //string strDate1 = "";
            //string strDate2 = "";
            //string strTIME = "";
            //string strTIME_REMARK = "";
            //string strREQUIRE = "";

            strManJong = VB.Left(cboManJong.SelectedItem.ToString().Trim(), 2);

            if(strManJong == "")
            {
                ComFunc.MsgBox("강사종류를 선택하세요.");
                {
                    return;
                }
            }

            strWRTNO = txtSeqNo.Text.Trim();

            if(strWRTNO == "")
            {
                ComFunc.MsgBox("다시 작업하세요");
                return;
            }

            strJong = VB.Left(cboJong.SelectedItem.ToString().Trim(), 2);
            strTopic = txtTopic.Text.Trim();
            strEduDate = txtFDate.Text.Trim();
            strEduDate2 = txtTDate.Text.Trim();

            RadioButton[] OptTime = new RadioButton[5] { optTime0, optTime1, optTime2, optTime3, optTime4 };
            RadioButton[] OptPlace = new RadioButton[2] { optPlace0, optPlace1 };

            for (i = 0; i < OptTime.Length; i++)
            {
                if (OptTime[i].Checked == true)
                {
                    strOptTime = i.ToString();
                    break;
                }
            }

            strEduTime = txtTime.Text.Trim();
            strMan = txtMan.Text.Trim();

            for (i = 0; i < OptPlace.Length; i++)
            {
                if (OptPlace[i].Checked == true)
                {
                    strOptPlace = i.ToString();
                    break;
                }
            }

            strPlace = txtPlace.Text.Trim();

            if (!VB.IsNumeric(txtJumsu.Text))
            {
                ComFunc.MsgBox("점수는 숫자만 가능합니다.");
                return;
            }

            strJumsu = txtJumsu.Text.Trim();            
            strRemark = txtRemark.Text.Trim();
            strSabun = clsType.User.IdNumber;
            strSName = clsType.User.JobName;
            strBuCode = VB.Pstr(txtPsts.Text, "^^", 1);
            strJik = VB.Pstr(txtPsts.Text, "^^", 2);
            strIpSaDay = VB.Pstr(txtPsts.Text, "^^", 3);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_MST";
            SQL += ComNum.VBLF + "WHERE WRTNO ='" + strWRTNO + "' ";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            clsDB.setBeginTran(clsDB.DbCon);
         
            if (strROWID == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "NUR_EDU_MST";
                SQL += ComNum.VBLF + "  (WRTNO,Sabun,SName,BuCode,";
                SQL += ComNum.VBLF + "   IpsaDate,SDate,JikJong,EDUJONG,";
                SQL += ComNum.VBLF + "   EDUNAME, FrDate,ToDate, OptTime,";
                SQL += ComNum.VBLF + "   EDUTIME, MAN,ManJong,OptPlace,";
                SQL += ComNum.VBLF + "   PLACE,Remark, JUMSU ,Gubun,";
                SQL += ComNum.VBLF + "   EntDate )";
                SQL += ComNum.VBLF + "VALUES (";
                SQL += ComNum.VBLF + "    '" + strWRTNO + "'";
                SQL += ComNum.VBLF + "  , '" + strSabun + "'";
                SQL += ComNum.VBLF + "  , '" + strSName + "'";
                SQL += ComNum.VBLF + "  , '" + strBuCode + "'";
                SQL += ComNum.VBLF + "  , '" + strIpSaDay + "'";
                SQL += ComNum.VBLF + "  , TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  , '" + strJik +"'";
                SQL += ComNum.VBLF + "  , '" + strJong + "'";
                SQL += ComNum.VBLF + "  , '" + strTopic + "'";
                SQL += ComNum.VBLF + "  , TO_DATE('" + strEduDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  , TO_DATE('" + strEduDate2 + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  , '" + strOptTime + "'";
                SQL += ComNum.VBLF + "  , '" + strEduTime + "'";
                SQL += ComNum.VBLF + "  , '" + strMan + "'";
                SQL += ComNum.VBLF + "  , '" + strManJong + "'";
                SQL += ComNum.VBLF + "  , '" + strOptPlace + "'";
                SQL += ComNum.VBLF + "  , '" + strPlace + "'";                
                SQL += ComNum.VBLF + "  , '" + strRemark + "'";
                SQL += ComNum.VBLF + "  , '" + strJumsu + "'";
                SQL += ComNum.VBLF + "  , '1' ";                
                SQL += ComNum.VBLF + "  , SYSDATE ";
                SQL += ComNum.VBLF + ")";
            }

            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "NUR_EDU_MST";
                SQL += ComNum.VBLF + "SET";
                SQL += ComNum.VBLF + "  ,JikJong= '" + strJik + "'";
                SQL += ComNum.VBLF + "  ,BuCode= '" + strBuCode + "'";
                SQL += ComNum.VBLF + "  ,EDUJONG= '" + strJong + "'";
                SQL += ComNum.VBLF + "  ,EDUNAME= '" + strTopic + "'";
                SQL += ComNum.VBLF + "  ,FrDate= TO_DATE('" + strEduDate + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  ,ToDate= TO_DATE('" + strEduDate2 + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "  ,OptTime = '" + strOptTime + "'";
                SQL += ComNum.VBLF + "  ,EDUTIME = '" + strEduTime + "'";
                SQL += ComNum.VBLF + "  ,MAN= '" + strMan + "'";
                SQL += ComNum.VBLF + "  ,ManJong= '" + strManJong + "'";
                SQL += ComNum.VBLF + "  ,Remark= '" + strRemark + "'";
                SQL += ComNum.VBLF + "  ,OptPlace= '" + strOptPlace + "'";
                SQL += ComNum.VBLF + "  ,PLACE= '" + strPlace + "'";
                SQL += ComNum.VBLF + "  ,JumSu= '" + strJumsu + "'";                
                SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
            }

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("저장하였습니다.");

            SCREEN_CLEAR();
            eGetData();
        }

        void eDelData()
        {
            //int i = 0;
            string strWRTNO = "";

            if (MessageBox.Show("자료를 정말로 삭제하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            strWRTNO = txtSeqNo.Text.Trim();         

            clsDB.setBeginTran(clsDB.DbCon);

            SQL = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "NUR_EDU_MST";
            SQL += ComNum.VBLF + "WHERE WRTNO='" + strWRTNO + "'";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("삭제하였습니다.");

            SCREEN_CLEAR();
            eGetData();
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strWRTNO = "";

            SCREEN_CLEAR();

            strWRTNO = ssList.ActiveSheet.Cells[e.Row, 0].Text;

            Screen_Display(strWRTNO);
        }

        void Screen_Display(string argWRTNO)
        {
            RadioButton[] OptTime = new RadioButton[5] { optTime0, optTime1, optTime2, optTime3, optTime4 };
            RadioButton[] OptPlace = new RadioButton[2] { optPlace0, optPlace1 };

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  WRTNO,SABUN,SNAME,BUCODE,IPSADATE,";
            SQL += ComNum.VBLF + "  TO_CHAR(FrDate,'YYYY-MM-DD') FrDate,TO_CHAR(ToDate,'YYYY-MM-DD') ToDate,";
            SQL += ComNum.VBLF + "  JIKJONG,EDUJONG,EDUNAME,Remark,";
            SQL += ComNum.VBLF + "  OptTime,EDUTIME , MAN,ManJong, OptPlace,PLACE, JUMSU";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_EDU_MST";
            SQL += ComNum.VBLF + "WHERE WRTNO ='" + argWRTNO + "'";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                txtSeqNo.Text = dt.Rows[0]["WRTNO"].ToString().Trim();
                cboJong.SelectedIndex = Convert.ToInt32(VB.Val(dt.Rows[0]["EDUJONG"].ToString().Trim()));
                cboManJong.SelectedIndex = Convert.ToInt32(VB.Val(dt.Rows[0]["ManJong"].ToString().Trim()));
                txtTopic.Text = dt.Rows[0]["WRTNO"].ToString().Trim();
                txtFDate.Text = dt.Rows[0]["WRTNO"].ToString().Trim();
                txtTDate.Text = dt.Rows[0]["WRTNO"].ToString().Trim();

                if(dt.Rows[0]["OptTime"].ToString().Trim() != "")
                {
                    OptTime[Convert.ToInt32(VB.Val(dt.Rows[0]["OptTime"].ToString().Trim()))].Checked = true;
                }

                txtTime.Text = dt.Rows[0]["EDUTIME"].ToString().Trim();

                if(dt.Rows[0]["OptPlace"].ToString().Trim() != "")
                {
                    OptPlace[Convert.ToInt32(VB.Val(dt.Rows[0]["OptPlace"].ToString().Trim()))].Checked = true;
                }

                txtPlace.Text = dt.Rows[0]["PLACE"].ToString().Trim();
                txtMan.Text = dt.Rows[0]["MAN"].ToString().Trim();
                txtJumsu.Text = dt.Rows[0]["JUMSU"].ToString().Trim();
                txtRemark.Text = dt.Rows[0]["REMARK"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            btnSave.Enabled = true;
            btnDel.Enabled = true;
            btnNew.Enabled = false;
        }

        void ssList3_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssList3.Visible = false;
        }
    }
}

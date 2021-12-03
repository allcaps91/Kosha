using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmFlagUpdate.cs
    /// Description     : 환자 Master Flag 변경하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06-13
    /// Update History  : 2018-04-10
    /// <history>       
    /// D:\타병원\PSMHH\basic\buppat\BUPPAT01.frm(FrmFlagUpdate) => frmFlagUpdate.cs 으로 변경함    
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\basic\buppat\BUPPAT01.frm(FrmFlagUpdate)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\basic\buppat\buppat.vbp
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmFlagUpdate : Form, MainFormMessage
    {
        clsPublic cpublic = new clsPublic();

        string strGbMsg = "";
        string strFlagBp = "";
        string strCancelFlag = "";
        string mstrJobName = "";
        string FstrPtno = "";

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

        public frmFlagUpdate()
        {
            InitializeComponent();
            setEvent();
        }

        public frmFlagUpdate(MainFormMessage pform, string GstrJobName)
        {
            InitializeComponent();
            this.mCallForm = pform;
            mstrJobName = GstrJobName;
            setEvent();
        }     

        public frmFlagUpdate(string GstrJobName, string ArgPtno = "")
        {
            InitializeComponent();
            mstrJobName = GstrJobName;
            FstrPtno = ArgPtno;
            setEvent();
        } 

        void setEvent()
        {
            this.txtGbMsg.GotFocus += new EventHandler(eTxtGotFocus);
            this.txtPano.GotFocus += new EventHandler(eTxtGotFocus);

            this.txtPano.LostFocus += new EventHandler(eTxtLostFocus);
        }

        void eTxtGotFocus(object sender, EventArgs e)
        {
            if(sender == this.txtGbMsg)
            {
                txtGbMsg.SelectionStart = 0;
                txtGbMsg.SelectionLength = txtGbMsg.Text.Length;
            }

            else if(sender == this.txtPano)
            {
                txtPano.SelectionStart = 0;
                txtPano.SelectionLength = txtPano.Text.Length;
            }

            
        }

        void eTxtLostFocus(object sender, EventArgs e)
        {
            if(sender == this.txtPano)
            {
                string strDeptCode = "";
                string strDrCode = "";
                string strKiho = "";
                string strGbMsg = "";
                int i = 0;
                string strPano = "";

                strPano = ComFunc.SetAutoZero(txtPano.Text.Trim(), 8);

                if (VB.IsNull(txtPano.Text) || txtPano.Text == "")
                {
                    return;
                }

                if (strCancelFlag == "OK")
                {
                    return;
                }

                if (!VB.IsNumeric(txtPano.Text))
                {
                    txtGuide.Text = "병록번호 다시 입력하세요 !!";
                    txtPano.Focus();
                    return;
                }

                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);

                #region BAS_PATIENT
                string SQL = "";
                string SqlErr = "";
                DataTable dt = null;
                DataTable dt1 = null;

                try
                {
                    //2018.06.18 박병규 : bohun column 추가
                    SQL = "";
                    SQL += ComNum.VBLF + "SELECT Pano, Sname, Jumin1, ";
                    SQL += ComNum.VBLF + "       Jumin2,Jumin3, to_char(StartDate,'yy-mm-dd') Sdate, ";
                    SQL += ComNum.VBLF + "       to_char(Lastdate,'yy-mm-dd') Ldate, JiCode, Tel, ";
                    SQL += ComNum.VBLF + "       Bi, Pname, Gwange, ";
                    SQL += ComNum.VBLF + "       Kiho, Gkiho, DeptCode, ";
                    SQL += ComNum.VBLF + "       Drcode, GbMsg, gbInfor, ";
                    SQL += ComNum.VBLF + "       gbInfor2, Remark, Bohun,GB_BLACK ";
                    SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                    SQL += ComNum.VBLF + "WHERE Pano = '" + strPano + "' ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        ss1_Sheet1.Cells[0, 2].Text = dt.Rows[0]["Sname"].ToString().Trim();
                        ss1_Sheet1.Cells[1, 2].Text = dt.Rows[0]["Jumin1"].ToString().Trim();
                        ss1_Sheet1.Cells[1, 3].Text = clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                        ss1_Sheet1.Cells[2, 2].Text = dt.Rows[0]["Tel"].ToString().Trim();
                        ss1_Sheet1.Cells[3, 2].Text = dt.Rows[0]["JiCode"].ToString().Trim();

                        ss2_Sheet1.Cells[0, 2].Text = dt.Rows[0]["SDate"].ToString().Trim();
                        ss2_Sheet1.Cells[1, 2].Text = dt.Rows[0]["LDate"].ToString().Trim();
                        ss2_Sheet1.Cells[2, 2].Text = dt.Rows[0]["DeptCode"].ToString().Trim();

                        strDeptCode = dt.Rows[0]["DeptCode"].ToString().Trim();

                        if (strDeptCode.Trim() != "")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT ";
                            SQL += ComNum.VBLF + "DeptNameK ";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_CLINICDEPT";
                            SQL += ComNum.VBLF + "WHERE DeptCode = '" + strDeptCode + "' ";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                ss2_Sheet1.Cells[2, 3].Text = dt1.Rows[0]["DeptNameK"].ToString().Trim();
                            }
                            else
                            {
                                ss2_Sheet1.Cells[2, 3].Text = "Error !!!";
                            }

                            dt1.Dispose();
                            dt1 = null;
                        }

                        ss2_Sheet1.Cells[3, 2].Text = dt.Rows[0]["DrCode"].ToString().Trim();
                        strDrCode = dt.Rows[0]["DrCode"].ToString().Trim();
                        
                        if (strDrCode.Trim() != "0000")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "SELECT ";
                            SQL += ComNum.VBLF + "  DrName ";
                            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
                            SQL += ComNum.VBLF + "WHERE DrCode = '" + strDrCode + "'";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                ss2_Sheet1.Cells[3, 3].Text = dt1.Rows[0]["DrName"].ToString().Trim();
                            }
                            else
                            {
                                ss2_Sheet1.Cells[3, 3].Text = "Error !!!";
                            }
                            dt1.Dispose();
                            dt1 = null;
                        }

                        ss3_Sheet1.Cells[0, 2].Text = dt.Rows[0]["Bi"].ToString();
                        ss3_Sheet1.Cells[1, 2].Text = dt.Rows[0]["Gwange"].ToString();
                        ss3_Sheet1.Cells[2, 2].Text = dt.Rows[0]["Pname"].ToString();
                        ss3_Sheet1.Cells[3, 2].Text = dt.Rows[0]["Kiho"].ToString();

                        switch (dt.Rows[0]["Bi"].ToString().Trim().Substring(0,1))
                        {
                            case "2":
                                
                                ss3_Sheet1.Cells[3, 1].Text = "기관 기호";
                                ss3_Sheet1.Cells[4, 1].Text = "관리 번호";
                                ss3_Sheet1.Cells[5, 1].Text = "장애 여부";
                                ss3_Sheet1.Cells[6, 1].Text = "";
                                ss3_Sheet1.Cells[6, 2].Text = "";
                                ss3_Sheet1.Cells[6, 3].Text = "";

                                ss3_Sheet1.Cells[4, 2].Locked = false;
                                //ss3_Sheet1.Cells[4, 2].TypeEditLen = 14;
                                ss3_Sheet1.Cells[5, 2].Locked = false;
                                //ss3_Sheet1.Cells[5, 2].TypeEditLen = 1;
                                ss3_Sheet1.Cells[6, 2].Locked = true;
                                ss3_Sheet1.Cells[6, 3].Locked = true;

                                ss3_Sheet1.Cells[4, 2].Text = dt.Rows[0]["Gkiho"].ToString().Trim();
                                ss3_Sheet1.Cells[5, 2].Text = dt.Rows[0]["Bohun"].ToString().Trim();
                                break;
                            case "3":
                                ss3_Sheet1.Cells[3, 1].Text = "계약처 Co";
                                ss3_Sheet1.Cells[4, 1].Text = "사고발생일";
                                ss3_Sheet1.Cells[5, 1].Text = "진료시작일";
                                ss3_Sheet1.Cells[6, 1].Text = "진료종료일";

                                ss3_Sheet1.Cells[4, 2].Locked = false;
                                //ss3_Sheet1.Cells[4, 2].TypeEditLen = 6;
                                ss3_Sheet1.Cells[5, 2].Locked = false;
                                //ss3_Sheet1.Cells[5, 2].TypeEditLen = 6;
                                ss3_Sheet1.Cells[6, 2].Locked = false;
                                //ss3_Sheet1.Cells[6, 2].TypeEditLen = 6;
                                ss3_Sheet1.Cells[6, 3].Locked = true;

                                // VB에선 MidB로 사용
                                ss3_Sheet1.Cells[4, 2].Text = VB.Mid(dt.Rows[0]["Gkiho"].ToString().Trim(), 1, 6);
                                ss3_Sheet1.Cells[5, 2].Text = VB.Mid(dt.Rows[0]["Gkiho"].ToString().Trim(), 1, 6);
                                ss3_Sheet1.Cells[6, 2].Text = VB.Mid(dt.Rows[0]["Gkiho"].ToString().Trim(), 1, 6);
                                break;
                            case "5":
                                ss3_Sheet1.Cells[3, 1].Text = "계약처 Co";
                                ss3_Sheet1.Cells[4, 1].Text = "차량 번호";
                                ss3_Sheet1.Cells[5, 1].Text = "";
                                ss3_Sheet1.Cells[5, 2].Text = "";
                                ss3_Sheet1.Cells[5, 3].Text = "";
                                ss3_Sheet1.Cells[6, 1].Text = "";
                                ss3_Sheet1.Cells[6, 2].Text = "";
                                ss3_Sheet1.Cells[6, 3].Text = "";

                                ss3_Sheet1.Cells[4, 2].Locked = false;
                                //ss3_Sheet1.Cells[4, 2].TypeEditLen = 18;
                                ss3_Sheet1.Cells[5, 2].Locked = true;
                                ss3_Sheet1.Cells[6, 2].Locked = true;
                                ss3_Sheet1.Cells[6, 3].Locked = true;

                                ss3_Sheet1.Cells[4, 2].Text = dt.Rows[0]["Gkiho"].ToString().Trim();
                                break;

                            default:
                                ss3_Sheet1.Cells[3, 1].Text = "기관 기호";
                                ss3_Sheet1.Cells[4, 1].Text = "증   번  호";
                                ss3_Sheet1.Cells[5, 1].Text = "승인 신청";
                                ss3_Sheet1.Cells[6, 1].Text = "";
                                ss3_Sheet1.Cells[6, 2].Text = "";
                                ss3_Sheet1.Cells[6, 3].Text = "";

                                //ss3_Sheet1.Cells[4, 2].TypeEditLen = 14;
                                ss3_Sheet1.Cells[4, 2].Locked = false;
                                //ss3_Sheet1.Cells[5, 2].TypeEditLen = 20;
                                ss3_Sheet1.Cells[5, 2].Locked = false;
                                //ss3_Sheet1.Cells[6, 2].TypeEditLen = 2;
                                ss3_Sheet1.Cells[6, 2].Locked = false;

                                ss3_Sheet1.Cells[4, 2].Text = dt.Rows[0]["Gkiho"].ToString().Trim();
                                ss3_Sheet1.Cells[5, 2].Text = dt.Rows[0]["Remark"].ToString().Trim();
                                break;
                        }

                        strKiho = dt.Rows[0]["Kiho"].ToString().Trim();

                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT";
                        SQL += ComNum.VBLF + "  * FROM";
                        SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_MIA";
                        SQL += ComNum.VBLF + "WHERE MiaCode  = '" + strKiho + "' ";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ss3_Sheet1.Cells[3, 3].Text = dt1.Rows[0]["MiaName"].ToString().Trim();
                        }
                        dt1.Dispose();
                        dt1 = null;

                        #region GbMsg_Set

                        strGbMsg = dt.Rows[0]["GbMsg"].ToString().Trim();
                        txtGbMsg.Text = strGbMsg;
                        txtGbMsgInfo.Text = dt.Rows[0]["GbInfor"].ToString().Trim();

                        #endregion

                        strFlagBp = "OK";

                        txtInfo2.Text = dt.Rows[0]["GbInfor2"].ToString().Trim();
                        txtInfo3.Text = dt.Rows[0]["GB_BLACK"].ToString().Trim();
                    }
                    else
                    {
                        strFlagBp = "NO";
                    }

                    dt.Dispose();
                    dt = null;

                    #endregion
                    

                    if (strFlagBp == "NO")
                    {
                        txtGuide.Text = "해당번호 환자 MASTER에 없음 !";
                        SCREEN_CLEAR();
                        txtPano.Focus();
                    }

                    else
                    {
                        btnOK.Enabled = true;
                        btnCancel.Enabled = true;
                    }
                }

                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                }
            }
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmFlagUpdate_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");
            read_sysdate();
            
            btnOK.Enabled = false;

            //2018.06.14 박병규 : 파라미터로 값 있을경우
            if (FstrPtno.Trim() != "")
                txtPano.Text = FstrPtno;
            else
                txtPano.Text = "";

            txtGbMsg.Text = "";
            txtGuide.Text = "";
            txtGbMsgInfo.Text = "";
            txtInfo2.Text = "";
            txtInfo3.Text = "";

            ss4_Sheet1.Columns[2].Visible = false;

            READ_BAS_INFOR();

            //2018.06.14 박병규 : 화면Load시 등록번호가 있을경우 정보 Read
            if (txtPano.Text.Trim() != "")
            {
                eTxtLostFocus(txtPano, null);
            }
           
            if (clsType.User.IdNumber != "19684"  && clsType.User.IdNumber != "20175" && clsType.User.IdNumber != "38358" && clsType.User.IdNumber != "45432" && clsType.User.IdNumber != "45441")
            {
                txtInfo3.Enabled = false;
            }


        }

        void read_sysdate()
        {            
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void READ_BAS_INFOR()
        {
            int i = 0;
            string SQL = "";
            string SqlErr = "";

            ss4_Sheet1.RowCount = 0;

            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  ICODE, INFORMATION, ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_INFOR";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }


            ss4_Sheet1.RowCount = dt.Rows.Count + 10;
            if(dt.Rows.Count > 0)
            {
                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ss4_Sheet1.Cells[i, 1].Text = dt.Rows[i]["INFORMATION"].ToString().Trim();
                    ss4_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }

            if(ss4_Sheet1.RowCount == 0)
            {
                ss4_Sheet1.RowCount = 100;
            }

            dt.Dispose();
            dt = null;
        }

        void SCREEN_CLEAR()
        {
            txtPano.Text = "";
            txtGbMsg.Text = "";
            txtGbMsgInfo.Text = "";

            ss1_Sheet1.Cells[0, 2].Text = "";
            ss1_Sheet1.Cells[0, 3].Text = "";
            ss1_Sheet1.Cells[1, 2].Text = "";
            ss1_Sheet1.Cells[1, 3].Text = "";
            ss1_Sheet1.Cells[2, 2].Text = "";
            ss1_Sheet1.Cells[2, 3].Text = "";
            ss1_Sheet1.Cells[3, 2].Text = "";
            ss1_Sheet1.Cells[3, 3].Text = "";

            ss2_Sheet1.Cells[0, 2].Text = "";
            ss2_Sheet1.Cells[0, 3].Text = "";
            ss2_Sheet1.Cells[1, 2].Text = "";
            ss2_Sheet1.Cells[1, 3].Text = "";
            ss2_Sheet1.Cells[2, 2].Text = "";
            ss2_Sheet1.Cells[2, 3].Text = "";
            ss2_Sheet1.Cells[3, 2].Text = "";
            ss2_Sheet1.Cells[3, 3].Text = "";

            ss3_Sheet1.Cells[0, 2].Text = "";
            ss3_Sheet1.Cells[0, 3].Text = "";
            ss3_Sheet1.Cells[1, 2].Text = "";
            ss3_Sheet1.Cells[1, 3].Text = "";
            ss3_Sheet1.Cells[2, 2].Text = "";
            ss3_Sheet1.Cells[2, 3].Text = "";
            ss3_Sheet1.Cells[3, 2].Text = "";
            ss3_Sheet1.Cells[3, 3].Text = "";
            ss3_Sheet1.Cells[4, 2].Text = "";
            ss3_Sheet1.Cells[4, 3].Text = "";
            ss3_Sheet1.Cells[5, 2].Text = "";
            ss3_Sheet1.Cells[5, 3].Text = "";
            ss3_Sheet1.Cells[6, 2].Text = "";
            ss3_Sheet1.Cells[6, 3].Text = "";
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            string GstrMsgTitle = "";
            string GstrMsgList = "";
            int GnMsgType = 0;
            int GnMsgReturn = 0;

            if(txtPano.Text.Trim() != "")
            {
                if (MessageBox.Show("화면의 Data를 Computer에 수록하지 않습니다." + "\r\n" + "취소 하시겠습니까?", "작업선택", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    return;
                }

                else
                {
                    txtPano.Text = "";
                    txtGbMsg.Text = "";
                    txtGbMsgInfo.Text = "";

                    ss1_Sheet1.Cells[0, 2].Text = "";
                    ss1_Sheet1.Cells[0, 3].Text = "";
                    ss1_Sheet1.Cells[1, 2].Text = "";
                    ss1_Sheet1.Cells[1, 3].Text = "";
                    ss1_Sheet1.Cells[2, 2].Text = "";
                    ss1_Sheet1.Cells[2, 3].Text = "";
                    ss1_Sheet1.Cells[3, 2].Text = "";
                    ss1_Sheet1.Cells[3, 3].Text = "";

                    ss2_Sheet1.Cells[0, 2].Text = "";
                    ss2_Sheet1.Cells[0, 3].Text = "";
                    ss2_Sheet1.Cells[1, 2].Text = "";
                    ss2_Sheet1.Cells[1, 3].Text = "";
                    ss2_Sheet1.Cells[2, 2].Text = "";
                    ss2_Sheet1.Cells[2, 3].Text = "";
                    ss2_Sheet1.Cells[3, 2].Text = "";
                    ss2_Sheet1.Cells[3, 3].Text = "";

                    ss3_Sheet1.Cells[0, 2].Text = "";
                    ss3_Sheet1.Cells[0, 3].Text = "";
                    ss3_Sheet1.Cells[1, 2].Text = "";
                    ss3_Sheet1.Cells[1, 3].Text = "";
                    ss3_Sheet1.Cells[2, 2].Text = "";
                    ss3_Sheet1.Cells[2, 3].Text = "";
                    ss3_Sheet1.Cells[3, 2].Text = "";
                    ss3_Sheet1.Cells[3, 3].Text = "";
                    ss3_Sheet1.Cells[4, 2].Text = "";
                    ss3_Sheet1.Cells[4, 3].Text = "";
                    ss3_Sheet1.Cells[5, 2].Text = "";
                    ss3_Sheet1.Cells[5, 3].Text = "";
                    ss3_Sheet1.Cells[6, 2].Text = "";
                    ss3_Sheet1.Cells[6, 3].Text = "";

                    txtPano.Focus();

                }
                
            }
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            UpdateData();
        }

        void UpdateData()
        {
            string strGbInfor = "";
            string strPano = "";
            string strGbMsg = "";
            string strMsg2 = "";
            string strMsg3 = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            strPano = ComFunc.SetAutoZero(txtPano.Text, 8);
            strGbMsg = txtGbMsg.Text;
            strGbInfor = txtGbMsgInfo.Text;
            strMsg2 = txtInfo2.Text;
            strMsg3 = txtInfo3.Text;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);            

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "UPDATE BAS_PATIENT SET";             
                SQL += ComNum.VBLF + "GbInfo_Detail ='" + cpublic.strSysDate + " " + cpublic.strSysTime + " [" + mstrJobName + "]', ";
                SQL += ComNum.VBLF + "GbInfor = '" + strGbInfor + "' ,";
                SQL += ComNum.VBLF + "GbInfor2 = '" + strMsg2 + "' ,";
                SQL += ComNum.VBLF + "GbMsg = '" + strGbMsg + "', ";
                SQL += ComNum.VBLF + "GB_BLACK = '" + strMsg3 + "' ";
                
                SQL += ComNum.VBLF + "WHERE Pano = '" + strPano + "'";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;


                txtPano.Text = "";
                txtGbMsg.Text = "";
                txtInfo2.Text = "";
                txtInfo3.Text = "";
                txtGbMsgInfo.Text = "";

                ss1_Sheet1.Cells[0, 2].Text = "";
                ss1_Sheet1.Cells[0, 3].Text = "";
                ss1_Sheet1.Cells[1, 2].Text = "";
                ss1_Sheet1.Cells[1, 3].Text = "";
                ss1_Sheet1.Cells[2, 2].Text = "";
                ss1_Sheet1.Cells[2, 3].Text = "";
                ss1_Sheet1.Cells[3, 2].Text = "";
                ss1_Sheet1.Cells[3, 3].Text = "";

                ss2_Sheet1.Cells[0, 2].Text = "";
                ss2_Sheet1.Cells[0, 3].Text = "";
                ss2_Sheet1.Cells[1, 2].Text = "";
                ss2_Sheet1.Cells[1, 3].Text = "";
                ss2_Sheet1.Cells[2, 2].Text = "";
                ss2_Sheet1.Cells[2, 3].Text = "";
                ss2_Sheet1.Cells[3, 2].Text = "";
                ss2_Sheet1.Cells[3, 3].Text = "";

                ss3_Sheet1.Cells[0, 2].Text = "";
                ss3_Sheet1.Cells[0, 3].Text = "";
                ss3_Sheet1.Cells[1, 2].Text = "";
                ss3_Sheet1.Cells[1, 3].Text = "";
                ss3_Sheet1.Cells[2, 2].Text = "";
                ss3_Sheet1.Cells[2, 3].Text = "";
                ss3_Sheet1.Cells[3, 2].Text = "";
                ss3_Sheet1.Cells[3, 3].Text = "";
                ss3_Sheet1.Cells[4, 2].Text = "";
                ss3_Sheet1.Cells[4, 3].Text = "";
                ss3_Sheet1.Cells[5, 2].Text = "";
                ss3_Sheet1.Cells[5, 3].Text = "";
                ss3_Sheet1.Cells[6, 2].Text = "";
                ss3_Sheet1.Cells[6, 3].Text = "";

                txtPano.Focus();
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            //ss4의 정보를 레코드를  DB에 저장
            SaveData();
        }

        void SaveData()
        {
            int i = 0;
            string strData = "";
            string strROWID = "";
            string strDel = "";
            string strFlag = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);            

            try
            {
                for (i = 0; i < ss4_Sheet1.RowCount; i++)
                {
                    strDel = ss4_Sheet1.Cells[i, 0].Text;
                    strData = ss4_Sheet1.Cells[i, 1].Text;
                    strROWID = ss4_Sheet1.Cells[i, 2].Text;

                    if (strData != "")
                    {
                        if (strDel == "True" && strROWID.Trim() != "")
                        {

                            SQL = "";
                            SQL += ComNum.VBLF + "DELETE ";
                            SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_INFOR";
                            SQL += ComNum.VBLF + "WHERE ROWID ='" + strROWID + "' ";

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                            }                            
                        }

                        else
                        {
                            if (strROWID.Trim() != "")
                            {
                                SQL = "";
                                SQL += ComNum.VBLF + "UPDATE ";
                                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_INFOR";
                                SQL += ComNum.VBLF + "SET ";
                                SQL += ComNum.VBLF + "INFORMATION = '" + strData + "' ";
                                SQL += ComNum.VBLF + "WHERE ROWID ='" + strROWID + "' ";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                }     
                            }
                            else
                            {
                               
                                SQL = "";
                                SQL += ComNum.VBLF + "INSERT INTO ";
                                SQL += ComNum.VBLF + ComNum.DB_PMPA + "BAS_INFOR";
                                SQL += ComNum.VBLF + "(ICODE, INFORMATION) ";
                                SQL += ComNum.VBLF + "VALUES(  ";
                                SQL += ComNum.VBLF + "'" + i + "', ";
                                SQL += ComNum.VBLF + "'" + strData + "'";
                                SQL += ComNum.VBLF + ")";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    ComFunc.MsgBox(SqlErr);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    Cursor.Current = Cursors.Default;
                                }
                            }
                        }

                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("수정하였습니다.");
                Cursor.Current = Cursors.Default;
            }

            catch(Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            READ_BAS_INFOR();
            txtPano.Focus();
        }

        void ss4_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            txtGbMsg.Text = "";
            txtGbMsgInfo.Text = ss4_Sheet1.Cells[e.Row, 1].Text.Trim();           
        }

        void txtGbMsg_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
            }
        }

        void txtPano_TextChanged(object sender, EventArgs e)
        {
            strCancelFlag = "NO";
        }

        void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            ComFunc CF = new ComFunc();

            if(e.KeyChar == 13)
            {
                if (CF.READ_BARCODE(txtPano.Text.Trim()) == true)
                {
                    txtPano.Text = clsPublic.GstrBarPano;
                }
                else
                {
                    txtPano.Text = string.Format("{0:D8}", Convert.ToInt32(txtPano.Text));
                }

                //e.KeyChar = 0;
                txtGuide.Text = "";
                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
                SendKeys.Send("{TAB}");
            }
        }        
    }
}

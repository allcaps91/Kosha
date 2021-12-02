using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewMihHistory.cs
    /// Description     : 보험사항 변경내역 History 조회
    /// Author          : 안정수
    /// Create Date     : 2017-09-18
    /// Update History  : 2017-11-06
    /// <history>       
    /// d:\psmh\OPD\oviewa\OVIEWA12.FRM(FrmMihHistory) => frmPmpaViewMihHistory.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oviewa\OVIEWA12.FRM(FrmMihHistory)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewMihHistory : Form
    {
        clsSpread CS = new clsSpread();

        int i = 0;
        int j = 0;
        int k = 0;

        string strJOB = "";
        string strBi = "";
        string strTransDate = "";
        string strPname = "";
        string strGwange = "";
        string strKiho = "";
        string strGKiho = "";
        string strAreaDept1 = "";
        string strAreaDept2 = "";
        string strAreaDept3 = "";
        string strAreaDate1 = "";
        string strAreaDate2 = "";
        string strAreaDate3 = "";
        string strAreaBdate3 = "";
        string strAreaBdate = "";
        string strROWID = "";

        string strPano = "";
        string strSname = "";
        string strBFdate = "";
        string strBLdate = "";
        string CVPANO = "";
        string strMsgList = "";
        string strMsgTitle = "";


        public frmPmpaViewMihHistory()
        {
            InitializeComponent();
            setEvent();
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
            this.btnNext.Click += new EventHandler(eBtnEvent);

            this.txtPano.GotFocus += new EventHandler(eControl_GotFocus);
            this.txtPano.KeyPress += new KeyPressEventHandler(eControl_KeyPress);


        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            txtPano.Focus();

            ssList_Sheet1.Columns[7].Visible = false;
            ssList_Sheet1.Columns[8].Visible = false;
            ssList_Sheet1.Columns[9].Visible = false;

            txt0.Enabled = false;
            txt1.Enabled = false;
            txt2.Enabled = false;
            txt3.Enabled = false;
            txt4.Enabled = false;
            txt5.Enabled = false;
            txt6.Enabled = false;
        }

        void eBtnEvent(object sender, EventArgs e)
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

                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
                CVPANO = txtPano.Text;

                // 개인 ID 사항 Select
                SQL_PATIENT();

                SQL_MIH();

            }

            else if (sender == this.btnNext)
            {
                CS.Spread_All_Clear(ssList);
                ComFunc.SetAllControlClear(panel2);
                txtPano.Enabled = true;
                txtPano.Focus();
            }
        }

        void eControl_GotFocus(object sender, EventArgs e)
        {
            txtPano.SelectionStart = 0;
            txtPano.SelectionLength = txtPano.TextLength;
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
                CVPANO = txtPano.Text;

                // 개인 ID 사항 Select
                SQL_PATIENT();

                SQL_MIH();
            }
        }

        void SQL_PATIENT()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strBi = "";

            CVPANO = txtPano.Text;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                            ";
            SQL += ComNum.VBLF + "  BI, SNAME, TO_CHAR(STARTDATE, 'YYYY-MM-DD') STARTDATE,          ";
            SQL += ComNum.VBLF + "  TO_CHAR(LASTDATE, 'YYYY-MM-DD') LASTDATE, PNAME, KIHO, GKIHO    ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT                            ";
            SQL += ComNum.VBLF + "WHERE PANO = '" + CVPANO + "'                                     ";

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

                if (dt.Rows.Count == 0)
                {
                    #region Patient_Not_found_message(GoSub)
                    strMsgTitle = "경고 !";
                    strMsgList = "환자 Master 미등록 환자 !! ";
                    ComFunc.MsgBox(strMsgList, strMsgTitle);
                    return;
                    #endregion Patient_Not_found_message(GoSub) End
                }
                else
                {
                    txt0.Text = dt.Rows[0]["Bi"].ToString().Trim();
                    txt1.Text = dt.Rows[0]["Sname"].ToString().Trim();
                    txt2.Text = dt.Rows[0]["Startdate"].ToString().Trim();
                    txt3.Text = dt.Rows[0]["Lastdate"].ToString().Trim();
                    txt4.Text = dt.Rows[0]["Pname"].ToString().Trim();
                    txt5.Text = dt.Rows[0]["Kiho"].ToString().Trim();
                    txt6.Text = dt.Rows[0]["Gkiho"].ToString().Trim();
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

        void SQL_MIH()
        {
            txtPano.Enabled = false;
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            #region SQL_PREPARE(GoSub)
            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  M.Pano,M.Bi,TO_CHAR(M.TransDate, 'YYYY-MM-DD') TransDate,";
            SQL += ComNum.VBLF + "  M.Pname , M.Gwange, M.Kiho, M.Gkiho,";
            SQL += ComNum.VBLF + "  ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_MIH M";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND M.PANO     =  '" + CVPANO + "' ";
            SQL += ComNum.VBLF + "ORDER BY M.TransDate DESC,M.Bi";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            #endregion SQL_PREPARE(GoSub) End

            #region SQL_MAIN(GoSub)

            int nCNT = 0;

            nCNT = dt.Rows.Count;
            CS.Spread_All_Clear(ssList);
            ssList_Sheet1.Rows.Count = nCNT;

            if (nCNT == 0)
            {
                #region Not_Fount_mih_message(GoSub)

                ComFunc.MsgBox("찾는 데이타가 없습니다. ");

                #endregion Not_Fount_mih_message(GoSub) End
            }
            else
            {
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < nCNT; i++)
                    {
                        #region GET_DATA_MOVE_SET(GoSub)

                        strPano = dt.Rows[i]["Pano"].ToString().Trim();
                        strBi = dt.Rows[i]["Bi"].ToString().Trim();
                        strTransDate = dt.Rows[i]["TransDate"].ToString().Trim();
                        strPname = dt.Rows[i]["Pname"].ToString().Trim();
                        strGwange = dt.Rows[i]["Gwange"].ToString().Trim();
                        strKiho = dt.Rows[i]["Kiho"].ToString().Trim();
                        strGKiho = dt.Rows[i]["Gkiho"].ToString().Trim();
                        strROWID = dt.Rows[i]["Rowid"].ToString().Trim();

                        #endregion GET_DATA_MOVE_SET(GoSub) End

                        #region SET_DATA_MOVE_SS(GoSub)

                        ssList_Sheet1.Cells[i, 1].Text = strTransDate;
                        ssList_Sheet1.Cells[i, 2].Text = strBi;
                        ssList_Sheet1.Cells[i, 3].Text = strPname;
                        ssList_Sheet1.Cells[i, 4].Text = strGwange;
                        ssList_Sheet1.Cells[i, 5].Text = strKiho;
                        ssList_Sheet1.Cells[i, 6].Text = strGKiho;
                        ssList_Sheet1.Cells[i, 7].Text = strROWID;
                        ssList_Sheet1.Cells[i, 8].Text = strPano;
                        ssList_Sheet1.Cells[i, 9].Text = strBi;

                        #endregion SET_DATA_MOVE_SS(GoSub) End
                    }
                }
                else
                {
                    return;
                }
            }

            #endregion SQL_MAIN(GoSub) End

            dt.Dispose();
            dt = null;

        }


    }
}

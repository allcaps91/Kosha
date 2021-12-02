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
    /// Class Name      : MedOpdNr
    /// File Name       : frmCode.cs
    /// Description     : 코드등록
    /// Author          : 안정수
    /// Create Date     : 2018-01-23    
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 FrmNrMess02.frm(Frm코드등록) 폼 frmCode.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrmess\FrmNrMess02.frm(Frm코드등록) >> frmCode.cs 폼이름 재정의" />
    public partial class frmCode : Form, MainFormMessage
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

        public frmCode(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmCode()
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
            this.btnView.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);

            //this.eControl.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
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

                cboGubun.Items.Add("01.환자용 메시지");
                cboGubun.SelectedIndex = 0;

                ssList.ActiveSheet.Columns[3].Visible = false;
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

            else if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eSaveData();
            }

            else if (sender == this.btnCancel)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                btnCancel_Click();
            }
        }

        void btnCancel_Click()
        {
            ssList.ActiveSheet.Rows.Count = 0;
            ssList.ActiveSheet.Rows.Count = 50;
            ssList.ActiveSheet.Rows[0, ssList.ActiveSheet.Rows.Count - 1].Height = 18;

        }

        void eSaveData()
        {
            int i = 0;
            
            string strName = "";
            string strChk = "";
            //string strYes = "";
            string strCode = "";            
            string strGubun = "";            
            string strROWID = "";            

            clsDB.setBeginTran(clsDB.DbCon);

            strGubun = VB.Left(cboGubun.SelectedItem.ToString().Trim(), 2);

            for (i = 0; i < ssList.ActiveSheet.GetLastNonEmptyRow(FarPoint.Win.Spread.NonEmptyItemFlag.Data); i++)
            {
                strChk = ssList.ActiveSheet.Cells[i, 0].Text;
                strCode = ssList.ActiveSheet.Cells[i, 1].Text;
                strName = ssList.ActiveSheet.Cells[i, 2].Text;
                strROWID = ssList.ActiveSheet.Cells[i, 3].Text;
                //strYes = ssList.ActiveSheet.Cells[i, 4].Text;

                if (strChk == "True")
                {
                    //삭제함
                    if (strROWID != "")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "NUR_MESSAGE_CODE";
                        SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";


                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        ssList.ActiveSheet.Cells[i, 0].Text = 0.ToString();

                    }
                }

                else
                {
                    if (strROWID == "")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "NUR_MESSAGE_CODE";
                        SQL += ComNum.VBLF + "(GUBUN, CODE, NAME)";
                        SQL += ComNum.VBLF + "VALUES(";
                        SQL += ComNum.VBLF + "  '" + strGubun + "',";
                        SQL += ComNum.VBLF + "  '" + strCode + "',";
                        SQL += ComNum.VBLF + "  '" + strName + "',";                        
                        SQL += ComNum.VBLF + ")";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }

                    else
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "NUR_MESSAGE_CODE";
                        SQL += ComNum.VBLF + "SET ";
                        SQL += ComNum.VBLF + "  CODE = '" + strCode + "',";
                        SQL += ComNum.VBLF + "  NAME = '" + strName + "'";
                        SQL += ComNum.VBLF + "WHERE ROWID  = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                   
                }
                
            }

            clsDB.setCommitTran(clsDB.DbCon);
            ComFunc.MsgBox("저장하였습니다.");
            eGetData();
        }

        void eGetData()
        {
            int i = 0;
            int nRead = 0;
            string strGubun = "";

            strGubun = VB.Left(cboGubun.SelectedItem.ToString().Trim(), 2);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  CODE, NAME, ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "NUR_MESSAGE_CODE";
            SQL += ComNum.VBLF + "WHERE GUBUN  = '" + strGubun + "'";
            SQL += ComNum.VBLF + "ORDER BY CODE";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저
                return;
            }

            if(dt.Rows.Count > 0)
            {
                nRead = dt.Rows.Count;
                ssList.ActiveSheet.Rows.Count = nRead + 20;

                for(i = 0; i < nRead; i++)
                {
                    ssList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["CODE"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 2].Text = " " + dt.Rows[i]["NAME"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;

        }

       
    }
}

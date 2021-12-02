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

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmDoctPlan3.cs
    /// Description     : 의사별 기타 스케쥴 등록
    /// Author          : 안정수
    /// Create Date     : 2018-01-22
    /// Update History  : 
    /// 저장부분 테스트 필요
    /// </summary>
    /// <history>  
    /// 기존 nropd14.frm(FrmDoctPlan3) 폼 frmDoctPlan3.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nropd\nropd14.frm(FrmDoctPlan3) >> frmDoctPlan3.cs 폼이름 재정의" />
    public partial class frmDoctPlan3 : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;

        string FstrDrCode = "";

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

        public frmDoctPlan3(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmDoctPlan3()
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

                btnCancel_Click();
            }
        }

        void Set_Init()
        {
            int i = 0;            

            ssList.ActiveSheet.Columns[5].Visible = false;  //ROWID
            ssList.ActiveSheet.Columns[6].Visible = false;  //변경

            //의사코드를 스프레드에 SET (VB LIST ===> C# 스프레드로 변경)
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  DrDept1,DrCode,DrName";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_DOCTOR";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND TOUR <> 'Y'";
            SQL += ComNum.VBLF + "      AND DrDept1 NOT IN ('HR','R6','TO')";
            SQL += ComNum.VBLF + "      AND SUBSTR(DrCode,3,2) <> '99'";
            SQL += ComNum.VBLF + "ORDER BY DrDept1,PrintRanking";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                ssDoct.ActiveSheet.Rows.Count = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssDoct.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["DrDept1"].ToString().Trim();
                    ssDoct.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                    ssDoct.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DrName"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;
        }

        void Screen_Clear()
        {
            lblDrName.Text = "";
            ssList.ActiveSheet.Rows.Count = 50;
            CS.Spread_All_Clear(ssList);
            collapsibleSplitContainer1.Panel2.Enabled = false;  //Main
            collapsibleSplitContainer1.Panel1.Enabled = true;   //Doct
        }

        void btnCancel_Click()
        {
            Screen_Clear();
            ssDoct.Focus();
        }

        void eSaveData()
        {
            int i = 0;
            int intRowAffected = 0; //변경된 Row 받는 변수

            string strDel = "";
            string strDate = "";
            string strSTime = "";
            string strETime = "";
            string strRemark = "";
            string strROWID = "";
            string strChange = "";
            string strOK = "";

            //자료에 오류가 있는지 Check
            for(i = 0; i < ssList.ActiveSheet.Rows.Count; i++)
            {
                strDel = ssList.ActiveSheet.Cells[i, 0].Text;
                strDate = ssList.ActiveSheet.Cells[i, 1].Text;
                strSTime = ssList.ActiveSheet.Cells[i, 2].Text;
                strETime = ssList.ActiveSheet.Cells[i, 3].Text;
                strRemark = ssList.ActiveSheet.Cells[i, 4].Text;
                strROWID = ssList.ActiveSheet.Cells[i, 5].Text;

                strOK = "NO";

                if(strDate != "")
                {
                    strOK = "OK";
                }

                if(strSTime != "")
                {
                    strOK = "OK";
                }

                if(strETime != "")
                {
                    strOK = "OK";
                }

                if(strDel == "True")
                {
                    strOK = "NO";
                }

                if(strOK == "OK")
                {
                    if(String.Compare(strDate, clsPublic.GstrSysDate) < 0)
                    {
                        ComFunc.MsgBox("날짜가 현재일보다 적음", "오류");
                        return;
                    }

                    if (strDate == "")
                    {
                        ComFunc.MsgBox("날짜가 공란입니다.", "오류");
                        return;
                    }

                    if (strSTime == "")
                    {
                        ComFunc.MsgBox("시작시간이 공란입니다.", "오류");
                        return;
                    }

                    if (strETime == "")
                    {
                        ComFunc.MsgBox("종료시간이 공란입니다.", "오류");
                        return;
                    }

                    if (String.Compare(strETime, strSTime) < 0)
                    {
                        ComFunc.MsgBox("종료시간이 시작시간보다 적음");
                        return;
                    }

                    if(strSTime.Length != 5 || strETime.Length != 5)
                    {
                        ComFunc.MsgBox("시간을 HH:MI형태로 입력하세요");
                        return;
                    }

                    if(VB.Mid(strSTime, 3, 1) != ":" || VB.Mid(strETime, 3, 1) != ":")
                    {
                        ComFunc.MsgBox("시간을 HH:MI형태로 입력하세요");
                        return;
                    }

                    switch(Convert.ToInt32(VB.Val(VB.Left(strSTime, 2))))
                    {
                        #region 0~23
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:

                            break;

                        #endregion

                        default:
                            ComFunc.MsgBox("시작시간의 시간이 오류(00-23)", "오류");
                            return;                            
                    }

                    switch(Convert.ToInt32(VB.Val(VB.Right(strSTime, 2))))
                    {
                        #region 0~59
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                        case 30:
                        case 31:
                        case 32:
                        case 33:
                        case 34:
                        case 35:
                        case 36:
                        case 37:
                        case 38:
                        case 39:
                        case 40:
                        case 41:
                        case 42:
                        case 43:
                        case 44:
                        case 45:
                        case 46:
                        case 47:
                        case 48:
                        case 49:
                        case 50:
                        case 51:
                        case 52:
                        case 53:
                        case 54:
                        case 55:
                        case 56:
                        case 57:
                        case 58:
                        case 59:
                            break;

                        #endregion

                        default:
                            ComFunc.MsgBox("시작시간의 분이 오류(00-59)", "오류");
                            return;
                    }

                    switch (Convert.ToInt32(VB.Val(VB.Left(strETime, 2))))
                    {
                        #region 0~23
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:

                            break;

                        #endregion

                        default:
                            ComFunc.MsgBox("종료시간의 시간이 오류(00-23)", "오류");
                            return;
                    }

                    switch (Convert.ToInt32(VB.Val(VB.Right(strETime, 2))))
                    {
                        #region 0~59
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                        case 24:
                        case 25:
                        case 26:
                        case 27:
                        case 28:
                        case 29:
                        case 30:
                        case 31:
                        case 32:
                        case 33:
                        case 34:
                        case 35:
                        case 36:
                        case 37:
                        case 38:
                        case 39:
                        case 40:
                        case 41:
                        case 42:
                        case 43:
                        case 44:
                        case 45:
                        case 46:
                        case 47:
                        case 48:
                        case 49:
                        case 50:
                        case 51:
                        case 52:
                        case 53:
                        case 54:
                        case 55:
                        case 56:
                        case 57:
                        case 58:
                        case 59:
                            break;

                        #endregion

                        default:
                            ComFunc.MsgBox("종료시간의 분이 오류(00-59)", "오류");
                            return;
                    }

                    if(strRemark == "")
                    {
                        ComFunc.MsgBox("참고사항이 공란입니다.", "오류");
                        return;
                    }
                }
            }

            clsDB.setBeginTran(clsDB.DbCon);

            //기타 스케쥴의 오늘 이전의 자료는 삭제함
            SQL = "";
            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC";
            SQL += ComNum.VBLF + "WHERE SCHDATE<TRUNC(SYSDATE)";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            //자료를 DB에 저장
            for(i = 0; i < ssList.ActiveSheet.Rows.Count; i++)
            {
                strDel = ssList.ActiveSheet.Cells[i, 0].Text;
                strDate = ssList.ActiveSheet.Cells[i, 1].Text;
                strSTime = ssList.ActiveSheet.Cells[i, 2].Text;
                strETime = ssList.ActiveSheet.Cells[i, 3].Text;
                strRemark = ssList.ActiveSheet.Cells[i, 4].Text;
                strROWID = ssList.ActiveSheet.Cells[i, 5].Text;
                strChange = ssList.ActiveSheet.Cells[i, 6].Text;

                SQL = "";

                if(strROWID == "")
                {
                    if(strDel != "" && strDel != "True")
                    {
                        SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC";
                        SQL += ComNum.VBLF + "(DrCode,SchDate,STime,ETime,Remark)";
                        SQL += ComNum.VBLF + "VALUES(";
                        SQL += ComNum.VBLF + "  '" + FstrDrCode + "',";
                        SQL += ComNum.VBLF + "  TO_DATE('" + strDel + "','YYYY-MM-DD'), ";
                        SQL += ComNum.VBLF + "  '" + strSTime + "', ";
                        SQL += ComNum.VBLF + "  '" + strETime + "',";
                        SQL += ComNum.VBLF + "  '" + strRemark + "'";
                        SQL += ComNum.VBLF + ")";
                    }

                    else if(strROWID != "")
                    {
                        if(strDel == "True")
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "DELETE " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC";
                            SQL += ComNum.VBLF + "WHERE ROWID='" + strROWID + "'";                            
                        }

                        else if(strChange == "Y")
                        {
                            SQL += ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC";
                            SQL += ComNum.VBLF + "SET ";
                            SQL += ComNum.VBLF + "  SchDate=TO_DATE('" + strDate + "','YYYY-MM-DD'),";
                            SQL += ComNum.VBLF + "  STime='" + strSTime + "',";
                            SQL += ComNum.VBLF + "  ETime='" + strETime + "',";
                            SQL += ComNum.VBLF + "  Remark='" + strRemark + "' ";
                            SQL += ComNum.VBLF + "WHERE ROWID='" + strROWID + "' ";                            
                        }
                    }

                    if(SQL != "")
                    {
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

            Screen_Clear();
            ssDoct.Focus();
        }

        void ssDoct_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            int i = 0;
            int nREAD = 0;

            collapsibleSplitContainer1.Panel1.Enabled = false;
            collapsibleSplitContainer1.Panel2.Enabled = true;

            FstrDrCode = ssDoct.ActiveSheet.Cells[e.Row, 1].Text;
            lblDrName.Text = ssDoct.ActiveSheet.Cells[e.Row, 0].Text + " " 
                           + ssDoct.ActiveSheet.Cells[e.Row, 1].Text + " " 
                           + ssDoct.ActiveSheet.Cells[e.Row, 2].Text;

            //기존의 자료를 SELECT
            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  ROWID,TO_CHAR(SchDate,'YYYY-MM-DD') SchDate,STime,ETime,Remark ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE_ETC";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND DrCode='" + FstrDrCode + "'";
            SQL += ComNum.VBLF + "      AND SchDate>=TRUNC(SYSDATE)";
            SQL += ComNum.VBLF + "ORDER BY SchDate,STime ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if(dt.Rows.Count > 0)
            {
                nREAD = dt.Rows.Count;
                ssList.ActiveSheet.Rows.Count = nREAD + 50;

                for(i = 0; i < nREAD; i++)
                {
                    ssList.ActiveSheet.Cells[i, 0].Text = "";
                    ssList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SchDate"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["STime"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["ETime"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["Remark"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 6].Text = "";                    
                }
            }

            dt.Dispose();
            dt = null;

            ssList.Focus();
        }

        void ssList_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            ssList.ActiveSheet.Cells[e.Row, 6].Text = "Y";
        }
    }
}

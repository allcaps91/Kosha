using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : FrmMedBaseCode.cs
    /// Description     : 수술실 바코드 상용구 관리폼 
    /// Author          : 안정수
    /// Create Date     : 2020-04-14
    /// Update History  : 
    /// </summary>    
    /// 
    public partial class FrmMedBaseCode : Form
    {
        clsSpread CS = new clsSpread();

        public delegate void SendText(string strText, string strQTY);
        public event SendText rSendText;
        string GstrSendTxt = "";

        //#region MainFormMessage InterFace

        //public MainFormMessage mCallForm = null;

        //public void MsgActivedForm(Form frm)
        //{

        //}

        //public void MsgUnloadForm(Form frm)
        //{

        //}

        //public void MsgFormClear()
        //{

        //}

        //public void MsgSendPara(string strPara)
        //{

        //}

        //#endregion

        public FrmMedBaseCode()
        {
            InitializeComponent();
            setEvent();
        }

        public FrmMedBaseCode(string argOpDate, string argPano)
        {
            InitializeComponent();
            setEvent();

            GetPatientInfo(argOpDate, argPano);

            txtQty.Focus();
        }        

        //public FrmMedBaseCode(MainFormMessage pform)
        //{
        //    InitializeComponent();
        //    this.mCallForm = pform;
        //    setEvent();
        //}

        void GetPatientInfo(string argOpDate, string argPano)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += "SELECT DEPTCODE, SNAME, SEX, AGE ";
                SQL += ComNum.VBLF + " FROM KOSMOS_PMPA.ORAN_MASTER ";
                SQL += ComNum.VBLF + "WHERE OpDate = TO_DATE('" + argOpDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND PANO = '" + argPano + "' ";                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon); 
                
                if(dt.Rows.Count > 0)
                {
                    lblPatInfo.Text = dt.Rows[0]["DEPTCODE"].ToString().Trim() + " / " +
                                         dt.Rows[0]["SNAME"].ToString().Trim() + " / " +
                                         argPano + " / " + dt.Rows[0]["SEX"].ToString().Trim() + " / " +
                                         dt.Rows[0]["AGE"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);
            this.Activated += new EventHandler(eFormActivated);
            this.FormClosed += new FormClosedEventHandler(eFormClosed);

            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnAdd.Click  += new EventHandler(eBtnClick);
            this.btnDel.Click  += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);

            //this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
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

                ssList.ActiveSheet.Columns[3].Visible = false;  //ROWID
                ssList.ActiveSheet.Columns[4].Visible = false;  //수정여부                

                GetData();
            }
        }

        void eFormActivated(object sender, EventArgs e)
        {
            //if (this.mCallForm == null)
            //{
            //    return;
            //}
            //else
            //{
            //    this.mCallForm.MsgActivedForm(this);
            //}
        }

        void eFormClosed(object sender, FormClosedEventArgs e)
        {
            //if (this.mCallForm == null)
            //{
            //    return;
            //}
            //else
            //{
            //    this.mCallForm.MsgUnloadForm(this);
            //}
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                if(ssList.ActiveSheet.Rows.Count > 0)
                {
                    for(int i = 0; i < ssList.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        if(ssList.ActiveSheet.Cells[i, 1].Text == "True")
                        {
                            GstrSendTxt += ssList.ActiveSheet.Cells[i, 2].Text.Trim() + ",";  
                        }
                    }
                } 
                rSendText(GstrSendTxt, txtQty.Text.Trim()); 

                this.Close();
                return;
            }

            else if (sender == this.btnSave)
            {
                if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
                if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인
                if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return; //권한 확인

                eSaveData(); 
            }

            else if (sender == this.btnAdd)
            {
                ssList.ActiveSheet.Rows.Count += 1;
            }
            
            else if (sender == this.btnDel)
            {
                if(ssList.ActiveSheet.Rows.Count > ssList.ActiveSheet.NonEmptyRowCount && ssList.ActiveSheet.Cells[ssList.ActiveSheet.Rows.Count - 1, 2].Text == "")
                {
                    ssList.ActiveSheet.Rows.Count -= 1;
                }
            }
        }

        void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            if (sender == this.ssList)
            {
                if (ssList.ActiveSheet.Rows.Count > 0)
                {
                    if(ssList.ActiveSheet.Cells[e.Row, 2].Text.Trim() != "")
                    {
                        rSendText(ssList.ActiveSheet.Cells[e.Row, 2].Text.Trim(), txtQty.Text.Trim());

                        this.Close();
                        return;
                    }
                }
            }
        }

        void eSaveData()
        {
            string SQL = "";
            string SqlErr = "";
            string strCode = "";

            int intRowAffected = 0;

            DataTable dt = null; 

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            for (int i = 0; i < ssList.ActiveSheet.Rows.Count; i++)
            {
                if(ssList.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    SQL = "";
                    SQL += ComNum.VBLF + "UPDATE KOSMOS_PMPA.BAS_BCODE";
                    SQL += ComNum.VBLF + "SET DELDATE = TRUNC(SYSDATE)";
                    SQL += ComNum.VBLF + "WHERE 1=1";
                    SQL += ComNum.VBLF + "  AND GUBUN = 'C#_수술실_바코드상용구'";
                    SQL += ComNum.VBLF + "  AND ROWID = '" + ssList.ActiveSheet.Cells[i, 3].Text.Trim() + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                }
                else
                {
                    if (ssList.ActiveSheet.Cells[i, 4].Text == "Y")
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + "SELECT ROWID ";
                        SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
                        SQL += ComNum.VBLF + "WHERE 1=1";
                        SQL += ComNum.VBLF + "  AND GUBUN = 'C#_수술실_바코드상용구'";
                        SQL += ComNum.VBLF + "  AND ROWID = '" + ssList.ActiveSheet.Cells[i, 3].Text.Trim() + "'";
                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                        if (dt.Rows.Count > 0)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + "UPDATE KOSMOS_PMPA.BAS_BCODE";
                            SQL += ComNum.VBLF + "SET NAME = '" + ssList.ActiveSheet.Cells[i, 2].Text.Trim() + "' ";
                            //SQL += ComNum.VBLF + "   ,GUBUN2 = '" + (ssList.ActiveSheet.Cells[i, 1].Text == "True" ? "1" : "") + "'";
                            SQL += ComNum.VBLF + "WHERE 1=1";
                            SQL += ComNum.VBLF + "  AND GUBUN = 'C#_수술실_바코드상용구'";
                            SQL += ComNum.VBLF + "  AND ROWID = '" + dt.Rows[0]["ROWID"].ToString().Trim() + "'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        }
                        else
                        {
                            strCode = READ_MAXCODE();
                            SQL = "";
                            SQL += ComNum.VBLF + "INSERT INTO KOSMOS_PMPA.BAS_BCODE";
                            SQL += ComNum.VBLF + "(GUBUN, CODE, NAME, ENTSABUN, ENTDATE, SORT, CNT, GUNUM1, GUNUM2, GUNUM3)";
                            SQL += ComNum.VBLF + "VALUES";
                            SQL += ComNum.VBLF + "(";
                            SQL += ComNum.VBLF + "   'C#_수술실_바코드상용구'";
                            SQL += ComNum.VBLF + "  ,'" + strCode + "'";
                            SQL += ComNum.VBLF + "  ,'" + ssList.ActiveSheet.Cells[i, 2].Text.Trim() + "'";
                            SQL += ComNum.VBLF + "  ,'" + clsType.User.Sabun + "'";
                            SQL += ComNum.VBLF + "  ,SYSDATE";
                            SQL += ComNum.VBLF + "  ,'0'";
                            SQL += ComNum.VBLF + "  ,0";
                            SQL += ComNum.VBLF + "  ,0";
                            SQL += ComNum.VBLF + "  ,0";
                            SQL += ComNum.VBLF + "  ,0";
                            //if(ssList.ActiveSheet.Cells[i, 1].Text == "True")
                            //{
                            //    SQL += ComNum.VBLF + "  ,'1'";
                            //}
                            //else
                            //{
                            //    SQL += ComNum.VBLF + "  ,''";
                            //}                            
                            SQL += ComNum.VBLF + ")";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }
            }

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

            GetData();
            Cursor.Current = Cursors.Default;
        }

        string  READ_MAXCODE()
        {
            string SQL = "";
            string SqlErr = "";
            string rtnVal = "";
            DataTable dt = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT";
            SQL += ComNum.VBLF + "  MAX(CODE) AS CODE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND GUBUN = 'C#_수술실_바코드상용구'";
            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if(dt.Rows.Count > 0)
            {
                rtnVal = (VB.Val(dt.Rows[0]["CODE"].ToString().Trim()) + 1).ToString();
            }

            return rtnVal;
        }

        void GetData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT NAME, ROWID, GUBUN2";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BCODE";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND GUBUN = 'C#_수술실_바코드상용구'";
            SQL += ComNum.VBLF + "  AND DELDATE IS NULL";

            SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("BAS_BCODE 조회중 문제가 발생했습니다." + SQL);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                //ssList.ActiveSheet.Rows.Count = dt.Rows.Count + 5;
                ssList.ActiveSheet.Rows.Count = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if(ssList.ActiveSheet.Rows.Count < dt.Rows.Count)
                    {
                        ssList.ActiveSheet.Rows.Count += 1;
                    }

                    ssList.ActiveSheet.Rows[i].Height = 30;

                    //ssList.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["GUBUN2"].ToString().Trim() == "1" ? "True" : "False";
                    //if(ssList.ActiveSheet.Cells[i, 1].Text == "True")
                    //{
                    //    ssList.ActiveSheet.Cells[i, 1, i, ssList.ActiveSheet.Columns.Count - 1].BackColor = Color.Aqua;
                    //}
                    //else
                    //{
                    //    ssList.ActiveSheet.Cells[i, 1, i, ssList.ActiveSheet.Columns.Count - 1].BackColor = Color.White;
                    //}
                    ssList.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["NAME"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;

        }

        void ssList_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            ssList.ActiveSheet.Cells[e.Row, 4].Text = "Y";
        }
    }
}

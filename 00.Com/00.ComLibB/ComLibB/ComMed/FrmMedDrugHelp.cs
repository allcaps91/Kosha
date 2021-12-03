using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : FrmMedDrugHelp.cs
    /// Description     : 약품 및 기타 처방 찾기
    /// Author          : 안정수
    /// Create Date     : 2017-11-24
    /// Update History  : 
    /// <history>       
    /// d:\psmh\Ocs\FrmDrugHelp.frm(FrmDrugHelp) => FrmMedDrugHelp.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\Ocs\FrmDrugHelp.frm(FrmDrugHelp) => FrmMedDrugHelp.cs
    /// </seealso>
    /// </summary>
    public partial class FrmMedDrugHelp : Form
    {
        string mstrHelpCode = "";        

        public FrmMedDrugHelp()
        {
            InitializeComponent();
            setEvent();
        }

        public FrmMedDrugHelp(string GstrHelpCode)
        {
            InitializeComponent();
            setEvent();
            mstrHelpCode = GstrHelpCode;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);
        }
    
        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }
        
            else if (sender == this.btnView)
            {
                //                

                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }
        }    

        void eFormLoad(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            opt1.Checked = true;

            if (mstrHelpCode != "")
            {
                txtSearch.Text = mstrHelpCode;

                Drug_Find(mstrHelpCode);
            }                     

            mstrHelpCode = "";
        }

        void Drug_Find(string ArgData = "")
        {
            int i = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;
            Cursor.Current = Cursors.WaitCursor;

            ssDrug.ActiveSheet.Rows.Count = 0;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ";
            SQL += ComNum.VBLF + "  A.SuNext, A.HName, A.SName,TO_CHAR(A.DELDATE,'YYYY-MM-DD') DELDATE, '' SDATE ,  B.DELDATE";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED  + "OCS_DRUGINFO_new A, " + ComNum.DB_ERP + "DRUG_JEP B";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "      AND ( UPPER(A.EName) LIKE '%" + ArgData.ToUpper() + "%' ";  //영문명
            SQL += ComNum.VBLF + "       OR   UPPER(A.SName) LIKE '%" + ArgData.ToUpper() + "%'";   //성분명
            SQL += ComNum.VBLF + "       OR   UPPER(A.HName) LIKE '%" + ArgData.ToUpper() + "%'";   //한글명
            SQL += ComNum.VBLF + "          )";
            SQL += ComNum.VBLF + "      AND A.SUNEXT = B.JEPCODE(+) ";            
            if(chkDel.Checked == false)
            {
                SQL += ComNum.VBLF + "  AND B.DELDATE IS NULL";
            }
            SQL += ComNum.VBLF + "ORDER BY A.SName, A.SuNext ";

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
                    ssDrug.ActiveSheet.Rows.Count = dt.Rows.Count;

                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDrug.ActiveSheet.Cells[i, 0].Text = clsOrderEtc.READ_DRUG_OUTCHK(clsDB.DbCon, dt.Rows[i]["SUNEXT"].ToString().Trim(), "");
                        if(ssDrug.ActiveSheet.Cells[i, 0].Text == "원외") /* UCase(Trim(App.EXEName)) = "MTSIORDER" */
                        {
                            ssDrug.ActiveSheet.Rows[i].BackColor = Color.LightPink;
                        }
                        if(ssDrug.ActiveSheet.Cells[i, 0].Text == "")
                        {
                            if(dt.Rows[i]["DELDATE"].ToString().Trim() != "")
                            {
                                ssDrug.ActiveSheet.Cells[i, 0].Text = "삭제";
                            }
                        }

                        ssDrug.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["SUNEXT"].ToString().Trim();
                        ssDrug.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["hName"].ToString().Trim();
                        ssDrug.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["sName"].ToString().Trim();
                        ssDrug.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DELDATE"].ToString().Trim();

                        if(dt.Rows[i]["DELDATE"].ToString().Trim() != "")
                        {
                            ssDrug.ActiveSheet.Rows[i].BackColor = Color.Red;
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
            Cursor.Current = Cursors.Default;

        }

        void eGetData()
        {
            if(opt1.Checked == true)
            {
                Drug_Find(txtSearch.Text);
            }


            // 2017-11-24 안정수, opt2 컨트롤이 사용안하므로 주석처리 하였음
            //else if(opt2.Checked == true)
            //{
            //    OrderCode_Find(txtSearch.Text);
            //}
        }

        void ssDrug_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(ssDrug.ActiveSheet.Cells[e.Row, 4].Text != "")
            {
                ComFunc.MsgBox("삭제된 코드입니다.");
                return;
            }

            clsPublic.GstrHelpCode = ssDrug.ActiveSheet.Cells[e.Row, 1].Text;
        }

        void ssDrug_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {

                if(ssDrug.ActiveSheet.Cells[ssDrug.ActiveSheet.ActiveRowIndex, 4].Text != "")
                {
                    ComFunc.MsgBox("삭제된 코드입니다.");
                    return;
                }
                SendKeys.Send("{TAB}");
            }
        }




        //void OrderCode_Find(string ArgData)
        //{
        //    Dim i As Integer



        //    SQL = " SELECT A.ORDERCODE, A.ORDERNAMES, A.ORDERNAME, B.SUNEXT, A.SENDDEPT, "
        //    SQL = SQL & vbCr & " (SELECT DELDATE FROM KOSMOS_PMPA.BAS_SUT WHERE B.SUNEXT = SUNEXT) DELDATE, A.BUN"
        //    SQL = SQL & vbCr & " FROM KOSMOS_OCS.OCS_ORDERCODE A, KOSMOS_PMPA.BAS_SUN B"
        //    SQL = SQL & vbCr & " WHERE A.SUCODE = B.SUNEXT"
        //    SQL = SQL & vbCr & "   AND (   UPPER(A.ORDERNAMES) LIKE '%" & UCase(ArgData) & "%' "
        //    SQL = SQL & vbCr & "        OR UPPER(A.ORDERNAME) LIKE '%" & UCase(ArgData) & "%' "
        //    SQL = SQL & vbCr & "       ) "
        //    SQL = SQL & vbCr & "       AND A.BUN NOT IN ('11','12','20','23')"
        //    SQL = SQL & vbCr & " ORDER BY A.ORDERNAMES, A.ORDERNAME "
        //    Result = AdoOpenSet(Rs, SQL)


        //    SSDrug.MaxRows = 0
        //    SSDrug.MaxRows = RowIndicator


        //    For i = 0 To RowIndicator -1
        //        SSDrug.Row = i + 1
        //        SSDrug.Col = 1: SSDrug.Text = ""
        //        If SSDrug.Text = "" Then
        //            If Trim(Rs!SENDDEPT & "") = "N" Then SSDrug.Text = "삭제"
        //        End If
        //        SSDrug.Col = 2: SSDrug.Text = Trim(Rs!OrderCode & "")
        //        SSDrug.Col = 3: SSDrug.Text = Trim(Rs!ORDERNAMES & "") & " " & Trim(Rs!OrderName & "")
        //        SSDrug.Col = 4: SSDrug.Text = Read_BunName(Trim(Rs!BUN & ""))
        //        SSDrug.Col = 5: SSDrug.Text = Trim(Rs!DELDATE & "")


        //        If Trim(Rs!SENDDEPT & "") = "N" Then
        //            'SSDrug.Col = -1:  SSDrug.BackColor = RGB(255, 200, 200)
        //            SSDrug.Col = -1:  SSDrug.ForeColor = RGB(255, 0, 0)
        //        End If




        //        Rs.MoveNext

        //    Next i

        //    AdoCloseSet Rs
        //}

    }
}

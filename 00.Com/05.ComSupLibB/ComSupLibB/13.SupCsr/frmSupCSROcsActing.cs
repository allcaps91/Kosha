using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ComBase;
using FarPoint.Win.Spread;


namespace ComSupLibB
{
    /// <summary>
    /// Class Name      : ComSupLibB
    /// File Name       : frmOpdCSRGureqSearch.cs
    /// Description     : 공급실 물품 청구내역조회
    /// Author          : 김욱동
    /// Create Date     : 2021-05-07
    /// TODO :
    /// 3. 테스트 필요
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 기존 Frm공급실물품청구 폼 frmOpdCSRGureq.cs 으로 변경함
    /// </history>
    /// <seealso cref= "\nurse\nrinfo\Frm공급실물품청구.frm(Frm공급실물품청구) >> frmOpdCSRGureq.cs 폼이름 재정의" />
    public partial class frmSupCSROcsActing : Form, MainFormMessage
    {
        #region 클래스 선언 및 etc....

        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        //clsIpdNr CIN = new clsIpdNr();

        string SQL = "";
        string SqlErr = "";
        DataTable dt = null;
        DataTable dt1 = null;

        string FstrMaster = "";
        int intRowAffected = 0; //변경된 Row 받는 변수

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

        public frmSupCSROcsActing(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmSupCSROcsActing()
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

            

            this.mnuExit.Click += new EventHandler(eMenu_Click);

            this.dtpDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpDate2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            //this.eControl.LostFocus += new EventHandler(eControl_LostFocus);
            //this.eControl.GotFocus += new EventHandler(eControl_GotFocus);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
            ComFunc.ReadSysDate(clsDB.DbCon);

            Set_Init();

            int i = 0;
            
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT DEPTCODE, DEPTNAMEK FROM KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL += ComNum.VBLF + "ORDER BY PRINTRANKING";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    cboBusecode.Items.Clear();
                    cboBusecode.Items.Add("**.전체");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboBusecode.Items.Add(dt.Rows[i]["DEPTCODE"].ToString().Trim() + "." + dt.Rows[i]["DEPTNAMEK"].ToString().Trim());
                    }
                }

                dt.Dispose();
                dt = null;

                cboBusecode.SelectedIndex = 0;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            ssList_Sheet1.Columns[11].Visible = false;
            ssList2_Sheet1.Columns[11].Visible = false;

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
            //    if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //    {
            //        return; //권한 확인
            //    }
                eGetData();
            }

        }


        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.dtpDate || sender == this.dtpDate2)
            {
                if (e.KeyChar == 13)
                {
                    SendKeys.Send("{TAB}");
                }
            }
        }

        void eMenu_Click(object sender, EventArgs e)
        {
            if (sender == this.mnuExit)
            {
                this.Close();
                return;
            }


        }

        void ssList_Change(object sender, ChangeEventArgs e)
        {
            ssList.ActiveSheet.Cells[e.Row, 9].Text = "Y";
        }

        void Set_Init()
        {
            ComboWard_SET();


            dtpDate.Text = clsPublic.GstrSysDate;
            dtpDate2.Text = clsPublic.GstrSysDate;

            btnView.Enabled = true;

            FstrMaster = "";
            
        }



        void eGetData()
        {
            int i = 0;
            int nRow = 0;
            int nRow2 = 0;
            int nRead = 0;

            ssList.ActiveSheet.Rows.Count = 0;
            ssList2.ActiveSheet.Rows.Count = 0;
            string strBucode = VB.Pstr(cboWard.SelectedItem.ToString().Trim(), ".", 1);



            CS.Spread_All_Clear(ssList);
            CS.Spread_All_Clear(ssList2);

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT  TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, TO_CHAR(A.CDATE,'YYYYMMDD') CDATE, A.PANO, A.SNAME, A.DEPTCODE, A.DRCODE, A.JEPCODE, A.ORDERNAME, A.GBINFO, A.BUCODE, B.NAME,  A.ROWID, A.QTY ";
                SQL += ComNum.VBLF + "   FROM KOSMOS_OCS.OCS_GUMESEND A, KOSMOS_PMPA.BAS_BUSE B ";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND  A.BDATE >= TO_DATE('" + dtpDate.Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "      AND  A.BDATE <= TO_DATE('" + dtpDate2.Text.Trim() + "','YYYY-MM-DD') ";
                if(chkChul.Checked == false)
                {
                    SQL += ComNum.VBLF + "      AND  A.CDATE IS NULL ";
                }
                SQL += ComNum.VBLF + "   AND A.GBBUSE = '2' ";
                SQL += ComNum.VBLF + "   AND A.GBIO = 'O' ";
                SQL += ComNum.VBLF + "   AND A.BUCODE = B.BUCODE(+) ";
                if (VB.Left(cboBusecode.Text,2) != "**")
                {
                    SQL += ComNum.VBLF + "      AND A.DEPTCODE ='" + VB.Left(cboBusecode.Text, 2) +  "'";
                }
                SQL += ComNum.VBLF + "      ORDER BY 3,1, A.JEPCODE ";


                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt.Rows.Count;
                ssList.ActiveSheet.Rows.Count = nRead;
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow += 1;

                        if (string.IsNullOrEmpty(dt.Rows[i]["CDATE"].ToString().Trim()) == false)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 0].Locked = true;
                            ssList.ActiveSheet.Cells[nRow - 1, 1].Text = "출고" + dt.Rows[i]["CDATE"].ToString().Trim();
                        }
                        ssList.ActiveSheet.Cells[nRow - 1, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 5].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 6].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 7].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 8].Text = dt.Rows[i]["GBINFO"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 9].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 10].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 11].Text = dt.Rows[i]["ROWID"].ToString().Trim();

                        if (strBucode ==  dt.Rows[i]["BUCODE"].ToString().Trim())
                        {
                            nRow2 += 1;
                            if(ssList2.ActiveSheet.Rows.Count < nRow2)
                            {
                                ssList2.ActiveSheet.Rows.Count = nRow2;
                            }
                            if (string.IsNullOrEmpty(dt.Rows[i]["CDATE"].ToString().Trim()) == false)
                            {
                                ssList2.ActiveSheet.Cells[nRow2 - 1, 1].Text = "출고";
                            }
                                ssList2.ActiveSheet.Cells[nRow2 - 1, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow2 - 1, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow2 - 1, 4].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow2 - 1, 5].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow2 - 1, 6].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow2 - 1, 7].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow2 - 1, 8].Text = dt.Rows[i]["GBINFO"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow2 - 1, 9].Text = dt.Rows[i]["QTY"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow2 - 1, 10].Text = dt.Rows[i]["NAME"].ToString().Trim();
                                ssList2.ActiveSheet.Cells[nRow2 - 1, 11].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                            
                        }

                    }
                }

                dt.Dispose();
                dt = null;


                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }


        }
        void ssListClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == false && e.Column != 0)
            {
                if (ssList.ActiveSheet.Cells[e.Row, 0].Locked == true) return;

                
                if (ssList.ActiveSheet.Cells[e.Row, 0].Text == "True")
                {
                    ssList.ActiveSheet.Cells[e.Row, 0, e.Row, this.ssList.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;
                    ssList.ActiveSheet.Cells[e.Row, 0].Text = "False";
                }
                else
                {
                    ssList.ActiveSheet.Cells[e.Row, 0, e.Row, this.ssList.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                    ssList.ActiveSheet.Cells[e.Row, 0].Text = "True";                    
                }
            }
            else
            {
                return;
            }
            
        }
        void ssListButtonClicked(object sender, EditorNotifyEventArgs e)
        {
           // if (e. == false )
            //{
                //if (ssList.ActiveSheet.Cells[e.Row, 0].Text == "True")
                //{
                //    ssList.ActiveSheet.Cells[e.Row, 0, e.Row, this.ssList.ActiveSheet.ColumnCount - 1].ForeColor = Color.Black;
                //}
                //else
                //{
                //    ssList.ActiveSheet.Cells[e.Row, 0, e.Row, this.ssList.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                //}
            //}
        }
        void ComboWard_SET()
        {
            int i = 0;
            int intIdxCbo = 0;
            string Bucode = "";
            Bucode = File.Exists(@"c:\cmc\gumebuse.dat") ? File.ReadAllText(@"c:\cmc\gumebuse.dat").Split('.')[0] : clsType.User.BuseCode;
            Cursor.Current = Cursors.WaitCursor;
            cboWard.Enabled = false;
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT ";
                SQL += ComNum.VBLF + "  BuCode,Name";
                SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_BUSE";
                SQL += ComNum.VBLF + "WHERE ORDFLAG = 'Y' ";
                if (Bucode == "033102" || Bucode == "033103")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033102','033103') ";
                }
                else if (Bucode == "033113" || Bucode == "033125")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033113','033125') ";
                }
                else if (Bucode == "033122" || Bucode == "033126")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033122','033126') ";
                }
                else if (Bucode == "033123" || Bucode == "033104")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033123','033104') ";
                }
                else if (Bucode == "033118" || Bucode == "033108")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('033118','033108') ";
                }
                else if (Bucode == "055101")
                {
                    SQL += ComNum.VBLF + "AND BuCode IN ('055101','044510','044520','100570') ";
                }
                else if (Bucode == "056102")
                {
                    SQL += ComNum.VBLF + " AND BuCode in ('011101','011102','011103','011104','011105','011106','011109','011110', ";
                   SQL += ComNum.VBLF + " '011111','011112','011113','011114','011117','011116','011124','011129','033140','056102','056104','056101', ";
                   SQL += ComNum.VBLF + " '100130','100070','100090','100150','100110', '011150')            ";
                                   }
                else if (Bucode != "******")
                {
                    SQL += ComNum.VBLF + "AND BuCode = '" + Bucode + "' ";
                }
                SQL += ComNum.VBLF + "ORDER BY BuCode";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    cboWard.Items.Clear();

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["BuCode"].ToString().Trim() + "." + dt.Rows[i]["Name"].ToString().Trim());

                        if (dt.Rows[i]["BuCode"].ToString().Trim() == Bucode)
                        {
                            intIdxCbo = i;
                        }


                    }
                }

                dt.Dispose();
                dt = null;

                cboWard.SelectedIndex = intIdxCbo;

                if (intIdxCbo > 0)
                {
                    cboWard.Enabled = true;
                }
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
        }

        private void btnsaveevery_Click(object sender, EventArgs e)
        {
            string strBucode = VB.Pstr(cboWard.SelectedItem.ToString().Trim(), ".", 1);

            for (int i = 0; i < ssList.ActiveSheet.RowCount; i++)
            {
                if (ssList.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    SQL = "";
                    SQL = "UPDATE KOSMOS_OCS.OCS_GUMESEND SET";
                    SQL = SQL + ComNum.VBLF + "  BUCODE = '" + strBucode + "' ";
                    SQL = SQL + ComNum.VBLF + "  WHERE ROWID =   '" + ssList.ActiveSheet.Cells[i, 11].Text.Trim() + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
            }

            btnView.PerformClick();
        }

        private void btndelevery_Click(object sender, EventArgs e)
        {
            string strBucode = VB.Pstr(cboWard.SelectedItem.ToString().Trim(), ".", 1);

            for (int i = 0; i < ssList2.ActiveSheet.RowCount; i++)
            {
                if (ssList2.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    SQL = "";
                    SQL = "UPDATE KOSMOS_OCS.OCS_GUMESEND SET";
                    SQL = SQL + ComNum.VBLF + "  BUCODE = '' ";
                    SQL = SQL + ComNum.VBLF + "  WHERE ROWID =   '" + ssList2.ActiveSheet.Cells[i, 11].Text.Trim() + "'";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(SqlErr);
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                }
            }

            btnView.PerformClick();
        }
    }
}


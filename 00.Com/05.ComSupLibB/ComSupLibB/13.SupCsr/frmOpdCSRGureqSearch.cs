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
    public partial class frmOpdCSRGureqSearch : Form, MainFormMessage
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

        public frmOpdCSRGureqSearch(MainFormMessage pform)
        {
            InitializeComponent();
            this.mCallForm = pform;
            setEvent();
        }

        public frmOpdCSRGureqSearch()
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

            this.btnPrint.Click += new EventHandler(eBtnPrint);

            this.mnuExit.Click += new EventHandler(eMenu_Click);

            this.dtpDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpDate2.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            //this.eControl.LostFocus += new EventHandler(eControl_LostFocus);
            //this.eControl.GotFocus += new EventHandler(eControl_GotFocus);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            //else
            //{
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등
                ComFunc.ReadSysDate(clsDB.DbCon);

                Set_Init();
            // }
            ssList_Sheet1.Columns[6].Visible = false;
            ssList_Sheet1.Columns[7].Visible = false;
            ssList_Sheet1.Columns[8].Visible = false;
            ssList_Sheet1.Columns[9].Visible = false;

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

        void eBtnPrint(object sender, EventArgs e)
        {
            if (sender == this.btnPrint)
            {
                //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
                //{
                //    return; //권한 확인
                //}
                ePrint();
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

            
            ssList.ActiveSheet.Columns[6].Visible = false;  //ROWID
            ssList.ActiveSheet.Columns[7].Visible = false;  //4차
            ssList.ActiveSheet.Columns[8].Visible = false;  //5차
            ssList.ActiveSheet.Columns[9].Visible = false;  //수정

            dtpDate.Text = clsPublic.GstrSysDate;
            dtpDate2.Text = clsPublic.GstrSysDate;

            btnView.Enabled = true;
            btnPrint.Enabled = false;

            FstrMaster = "";
            
        }

        void ePrint()
        {
            clsSpread SPR = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strSubTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            strTitle = "(" + VB.Pstr(cboWard.SelectedItem.ToString().Trim(), ".", 1) + ") 멸균 및 소모물품 청구 내역";
            strSubTitle = "작업일자 : " + dtpDate.Text + " ~ " + dtpDate2.Text;

            strHeader = SPR.setSpdPrint_String(strTitle, new Font("굴림체", 13, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String("\r\n", new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += SPR.setSpdPrint_String(strSubTitle, new Font("굴림체", 11, FontStyle.Regular), clsSpread.enmSpdHAlign.Left, false, true);

            strFooter = SPR.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 20, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, false, true, false, false, false);

            SPR.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }



        void eGetData()
        {
            int i = 0;
            int nRow = 0;
            int nRead = 0;
            string strOldData = "";
            string strNewData = "";

            string strBucode = VB.Pstr(cboWard.SelectedItem.ToString().Trim(), ".", 2);


            btnPrint.Enabled = false;

            CS.Spread_All_Clear(ssList);


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT A.BuCode, TO_CHAR(A.RDate,'YYYY-MM-DD') RDate, A.JepCode, B.CsrName, A.Req4, A.Req5, A.Req4 + A.Req5 SReq, A.ROWID ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_ADM.CSR_REQ A, KOSMOS_ADM.ORD_JEP B ";
                SQL += ComNum.VBLF + "WHERE 1=1";
                SQL += ComNum.VBLF + "      AND BuCode ='" + strBucode + "'";
                SQL += ComNum.VBLF + "      AND RDate >= TO_DATE('" + dtpDate.Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "      AND RDate <= TO_DATE('" + dtpDate2.Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND ( REQ4 > 0 OR REQ5 > 0 ) ";
                SQL += ComNum.VBLF + "  AND A.JepCode = B.JepCode(+) ";
                SQL += ComNum.VBLF + " ORDER BY A.RDate, B.CsrName, A.JepCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt1.Rows.Count;
                ssList.ActiveSheet.Rows.Count = nRead;

                if (dt1.Rows.Count > 0)
                {
                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        strNewData = dt1.Rows[i]["RDate"].ToString().Trim(); 
                        nRow += 1;
                        if (strOldData == "")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 0].Text = strNewData;
                            strOldData = strNewData;
                        }
                        else if (strOldData != strNewData)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 0].Text = strNewData;
                            strOldData = strNewData;
                        }
     
                        ssList.ActiveSheet.Cells[nRow - 1, 1].Text = dt1.Rows[i]["JepCode"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 2].Text = dt1.Rows[i]["CsrName"].ToString().Trim();

                        ssList.ActiveSheet.Cells[nRow - 1, 3].Text = VB.Val(dt1.Rows[i]["Req4"].ToString().Trim()).ToString();
                        ssList.ActiveSheet.Cells[nRow - 1, 4].Text = VB.Val(dt1.Rows[i]["Req5"].ToString().Trim()).ToString();
                        ssList.ActiveSheet.Cells[nRow - 1, 5].Text = VB.Val(dt1.Rows[i]["SReq"].ToString().Trim()).ToString();

                        ssList.ActiveSheet.Cells[nRow - 1, 6].Text = dt1.Rows[i]["ROWID"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 1, 7].Text = VB.Val(dt1.Rows[i]["Req4"].ToString().Trim()).ToString();
                        ssList.ActiveSheet.Cells[nRow - 1, 8].Text = VB.Val(dt1.Rows[i]["Req5"].ToString().Trim()).ToString();
                    }
                }

                dt1.Dispose();
                dt1 = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT A.JepCode, B.CsrName, DECODE(GROUPING(A.JepCode), 0, ROUND(SUM(A.Req4 + A.Req5), 0)) TSReq, ";
                SQL += ComNum.VBLF + " DECODE(GROUPING(A.JepCode), 0, ROUND(SUM(A.Req4), 0)) TReq4, DECODE(GROUPING(A.JepCode), 0, ROUND(SUM(A.Req5), 0)) TReq5 ";
                SQL += ComNum.VBLF + " FROM KOSMOS_ADM.CSR_REQ A, KOSMOS_ADM.ORD_JEP B ";
                SQL += ComNum.VBLF + " WHERE BuCode='" + strBucode + "' ";
                SQL += ComNum.VBLF + " AND RDate >= TO_DATE('" + dtpDate.Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " AND RDate <= TO_DATE('" + dtpDate2.Text.Trim() + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + " AND ( REQ4 > 0  or REQ5 > 0  )  ";
                SQL += ComNum.VBLF + " AND A.JepCode = B.JepCode(+) ";
                SQL += ComNum.VBLF + " GROUP BY ROLLUP(A.JepCode), B.CsrName ";
                SQL += ComNum.VBLF + " Having GROUPING(A.JepCode) = 0 ";
                SQL += ComNum.VBLF + " ORDER BY B.CsrName, A.JepCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRow = nRow + 2;
                ssList.ActiveSheet.Rows.Count = nRow;

                ssList.ActiveSheet.Cells[nRow - 1, 0].Text = "** 합계 **";
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow += 1;
                        ssList.ActiveSheet.Rows.Count = nRow - 1;
                        ssList.ActiveSheet.Cells[nRow - 2, 1].Text = dt.Rows[i]["JepCode"].ToString().Trim();
                        ssList.ActiveSheet.Cells[nRow - 2, 2].Text = dt.Rows[i]["CsrName"].ToString().Trim();

                        ssList.ActiveSheet.Cells[nRow - 2, 3].Text = VB.Val(dt.Rows[i]["TReq4"].ToString().Trim()).ToString();
                        ssList.ActiveSheet.Cells[nRow - 2, 4].Text = VB.Val(dt.Rows[i]["TReq5"].ToString().Trim()).ToString();
                        ssList.ActiveSheet.Cells[nRow - 2, 5].Text = VB.Val(dt.Rows[i]["TSReq"].ToString().Trim()).ToString();
                    }
                }

                dt.Dispose();
                dt = null;

                btnPrint.Enabled = true;
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

        void ComboWard_SET()
        {
            int i = 0;
            int intIdxCbo = 0;
            string Bucode = "";
            Bucode = File.Exists(@"c:\cmc\gumebuse.dat") ? File.ReadAllText(@"c:\cmc\gumebuse.dat").Split('.')[0] : clsType.User.BuseCode;
            Cursor.Current = Cursors.WaitCursor;

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
                    cboWard.Items.Add("전체");

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt.Rows[i]["Name"].ToString().Trim() + "." + dt.Rows[i]["BuCode"].ToString().Trim());

                        if (dt.Rows[i]["BuCode"].ToString().Trim() == Bucode)
                        {
                            intIdxCbo = i + 1;
                        }


                    }
                }

                dt.Dispose();
                dt = null;

                cboWard.SelectedIndex = intIdxCbo;

                if (intIdxCbo > 0)
                {
                    cboWard.Enabled = false;
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
    }
}


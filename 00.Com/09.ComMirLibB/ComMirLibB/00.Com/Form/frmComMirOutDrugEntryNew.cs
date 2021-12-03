using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirOutDrugEntryNew.cs
    /// Description     : 원외처방전 등록 - 약제과처방
    /// Author          : 박성완
    /// Create Date     : 2018-01-10
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\FrmOutDrugNEW.FRM
    public partial class frmComMirOutDrugEntryNew : Form
    {
        clsComMir.cls_Table_Mir_Insid TID = new clsComMir.cls_Table_Mir_Insid();

        //public frmComMirOutDrugEntryNew()
        //{   
        //    테스트용 데이터
        //    TID.Pano = "09128670";
        //    TID.SName = "김태준";
        //    TID.DTno = "11";
        //    TID.WRTNO = 5223857;
        //    TID.JinDate1 = "20170112";
        //    TID.FrDate = "";
        //    TID.IpdOpd = "I";
        //    TID.Bi = "12";

        //    InitializeComponent();

        //    SetEvent();
        //}

        public frmComMirOutDrugEntryNew(clsComMir.cls_Table_Mir_Insid cls_Table_Mir_Insid)
        {
            TID = cls_Table_Mir_Insid;

            InitializeComponent();

            SetEvent();
        }

        private void SetEvent()
        {
            this.Load += FrmComMirOutDrugEntry_Load;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnSave.Click += BtnSave_Click;
            this.btnExit.Click += BtnExit_Click;

            this.ss1.ButtonClicked += Ss1_ButtonClicked;
        }

        private void Ss1_ButtonClicked(object sender, EditorNotifyEventArgs e)
        {
            if (ss1.ActiveSheet.Cells[e.Row, 0].Text == "True")
            {
                ss1.ActiveSheet.Rows[e.Row].BackColor = Color.Yellow;
            }
            else
            {
                ss1.ActiveSheet.Rows[e.Row].BackColor = Color.White;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            //TODO:청구 메인 완성후 구현 필요 
            //BaseMir 에서 메인폼 컨트롤 제어
            //Call READ_MIR_OUTDRUG
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {

            if (TID.Bi == "52")
            {
                SaveData_TA();
            }
            //else if ( TID.Bi =="31")
            //{
            //    SaveData_SAN();
            //}
            else
            {
                SaveData();
            }

        }

        private void SaveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            long nWrtno = 0;
            string strChk = "";
            string strRowid = "";
            string strSlip = "";
            string strSlipNo = "";

            int intRowAffected = 0; //변경된 Row 받는 변수
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < ss1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = ss1.ActiveSheet.Cells[i, 0].Text;
                    strSlip = VB.Left(ss1.ActiveSheet.Cells[i, 1].Text, 8);
                    strSlipNo = VB.Mid(ss1.ActiveSheet.Cells[i, 1].Text, 10, 5);
                    strRowid = ss1.ActiveSheet.Cells[i, 10].Text;

                    nWrtno = 0;

                    if (strChk == "True")
                    {
                        nWrtno = TID.WRTNO;

                        SQL = "INSERT INTO KOSMOS_PMPA.MIR_OUTDRUGMST (WRTNO,SLIPDATE,SLIPNO,Pano,BDate,DeptCode,DrCode," + ComNum.VBLF;
                        SQL += " Bi,ActDate,PART,SEQNO,ENTDATE,DIEASE1,DIEASE2,FLAG,PRTDEPT,SENDDATE,PRTDATE,Remark," + ComNum.VBLF;
                        SQL += " DRBUNHO,WEBSEND,IPDOPD,PRTBUN,DRSABUN,HAPPRINT,Change,MAXNAL,GBAUTO,CHKPRT,GBV252,GBV352 ) " + ComNum.VBLF;
                        SQL += " SELECT '" + TID.WRTNO + "'  ,  SLIPDATE, SLIPNO, Pano, BDate, DeptCode, DrCode, " + ComNum.VBLF;
                        SQL += " Bi,ActDate,PART,SEQNO,ENTDATE,DIEASE1,DIEASE2,FLAG,PRTDEPT,SENDDATE,PRTDATE,Remark," + ComNum.VBLF;
                        SQL += " DRBUNHO,WEBSEND,IPDOPD,PRTBUN,DRSABUN,HAPPRINT,Change,MAXNAL,GBAUTO,CHKPRT,GBV252,GBV352  " + ComNum.VBLF;
                        SQL += "  FROM  KOSMOS_OCS.OCS_OUTDRUGMST " + ComNum.VBLF;
                        SQL += " WHERE ROWID='" + strRowid + "' " + ComNum.VBLF;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            MessageBox.Show("INSERT MIR_OUTDRUGMST ERROR!!!");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }


                        //상세내역도 wrtno 빼기
                        SQL = "INSERT INTO KOSMOS_PMPA.MIR_OUTDRUG ( WRTNO,SLIPDATE,SLIPNO,Pano,DeptCode,Bun,Sucode," + ComNum.VBLF;
                        SQL += " Qty,REALQTY,Div,DIVQTY,Nal,DOSCODE,OrderNo,Remark,FLAG,EdiSeq,EdiCode,EdiQty," + ComNum.VBLF;
                        SQL += " GbSelf,MULTI,MULTIREMARK,DUR,SCODESAYU,SCODEREMARK ) " + ComNum.VBLF;
                        SQL += "  SELECT '" + TID.WRTNO + "' ,SLIPDATE,SLIPNO,Pano,DeptCode,Bun,Sucode," + ComNum.VBLF;
                        SQL += " Qty,REALQTY,Div,DIVQTY,Nal,DOSCODE,OrderNo,Remark,FLAG,EdiSeq,EdiCode,EdiQty," + ComNum.VBLF;
                        SQL += " GbSelf,MULTI,MULTIREMARK,DUR,SCODESAYU,SCODEREMARK " + ComNum.VBLF;
                        SQL += "  FROM  KOSMOS_OCS.OCS_OUTDRUG " + ComNum.VBLF;
                        SQL += " WHERE  SLIPDATE =TO_DATE( '" + strSlip + "' ,'YYYYMMDD') " + ComNum.VBLF;
                        SQL += "   AND SLIPNO = '" + strSlipNo + "' " + ComNum.VBLF;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            MessageBox.Show("INSERT OCS_OUTDRUG ERROR!!!");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        SQL = "";
                        SQL += "UPDATE KOSMOS_PMPA.MIR_OUTDRUGMST SET WRTNO=" + TID.WRTNO + " " + ComNum.VBLF;
                        SQL += " WHERE  SLIPDATE =TO_DATE( '" + strSlip + "' ,'YYYYMMDD') " + ComNum.VBLF;
                        SQL += "   AND SLIPNO = '" + strSlipNo + "' " + ComNum.VBLF;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            MessageBox.Show("INSERT OCS_OUTDRUG ERROR!!!");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                //TODO:청구 메인 완성후 구현 필요 
                //BaseMir 에서 메인폼 컨트롤 제어
                //Call READ_MIR_OUTDRUG
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void SaveData_TA()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            long nWrtno = 0;
            string strChk = "";
            string strRowid = "";
            string strSlip = "";
            string strSlipNo = "";

            int intRowAffected = 0; //변경된 Row 받는 변수
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < ss1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = ss1.ActiveSheet.Cells[i, 0].Text;
                    strSlip = VB.Left(ss1.ActiveSheet.Cells[i, 1].Text, 8);
                    strSlipNo = VB.Mid(ss1.ActiveSheet.Cells[i, 1].Text, 10, 5);
                    strRowid = ss1.ActiveSheet.Cells[i, 10].Text;

                    nWrtno = 0;

                    if (strChk == "True")
                    {                        
                        nWrtno = TID.WRTNO;

                        SQL = "INSERT INTO KOSMOS_PMPA.MIR_TAOUTDRUGMST (WRTNO,SLIPDATE,SLIPNO,Pano,BDate,DeptCode,DrCode," + ComNum.VBLF;
                        SQL += " Bi,ActDate,PART,SEQNO,ENTDATE,DIEASE1,DIEASE2,FLAG,PRTDEPT,SENDDATE,PRTDATE,Remark," + ComNum.VBLF;
                        SQL += " DRBUNHO,WEBSEND,IPDOPD,PRTBUN,DRSABUN,HAPPRINT,Change,MAXNAL,GBAUTO,CHKPRT,GBV252,GBV352 ) " + ComNum.VBLF;
                        SQL += " SELECT '" + TID.WRTNO + "'  ,  SLIPDATE, SLIPNO, Pano, BDate, DeptCode, DrCode, " + ComNum.VBLF;
                        SQL += " Bi,ActDate,PART,SEQNO,ENTDATE,DIEASE1,DIEASE2,FLAG,PRTDEPT,SENDDATE,PRTDATE,Remark," + ComNum.VBLF;
                        SQL += " DRBUNHO,WEBSEND,IPDOPD,PRTBUN,DRSABUN,HAPPRINT,Change,MAXNAL,GBAUTO,CHKPRT,GBV252,GBV352  " + ComNum.VBLF;
                        SQL += "  FROM  KOSMOS_OCS.OCS_OUTDRUGMST " + ComNum.VBLF;
                        SQL += " WHERE ROWID='" + strRowid + "' " + ComNum.VBLF;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            MessageBox.Show("INSERT MIR_TAOUTDRUGMST ERROR!!!");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }


                        //상세내역도 wrtno 빼기
                        SQL = "INSERT INTO KOSMOS_PMPA.MIR_TAOUTDRUG ( WRTNO,SLIPDATE,SLIPNO,Pano,DeptCode,Bun,Sucode," + ComNum.VBLF;
                        SQL += " Qty,REALQTY,Div,DIVQTY,Nal,DOSCODE,OrderNo,Remark,FLAG,EdiSeq,EdiCode,EdiQty," + ComNum.VBLF;
                        SQL += " GbSelf,MULTI,MULTIREMARK,DUR,SCODESAYU,SCODEREMARK ) " + ComNum.VBLF;
                        SQL += "  SELECT '" + TID.WRTNO + "' ,SLIPDATE,SLIPNO,Pano,DeptCode,Bun,Sucode," + ComNum.VBLF;
                        SQL += " Qty,REALQTY,Div,DIVQTY,Nal,DOSCODE,OrderNo,Remark,FLAG,EdiSeq,EdiCode,EdiQty," + ComNum.VBLF;
                        SQL += " GbSelf,MULTI,MULTIREMARK,DUR,SCODESAYU,SCODEREMARK " + ComNum.VBLF;
                        SQL += "  FROM  KOSMOS_OCS.OCS_OUTDRUG " + ComNum.VBLF;
                        SQL += " WHERE  SLIPDATE =TO_DATE( '" + strSlip + "' ,'YYYYMMDD') " + ComNum.VBLF;
                        SQL += "   AND SLIPNO = '" + strSlipNo + "' " + ComNum.VBLF;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            MessageBox.Show("INSERT MIR_TAOUTDRUG ERROR!!!");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        SQL = "";
                        SQL += "UPDATE KOSMOS_PMPA.MIR_TAOUTDRUGMST SET WRTNO=" + TID.WRTNO + " " + ComNum.VBLF;
                        SQL += " WHERE  SLIPDATE =TO_DATE( '" + strSlip + "' ,'YYYYMMDD') " + ComNum.VBLF;
                        SQL += "   AND SLIPNO = '" + strSlipNo + "' " + ComNum.VBLF;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            MessageBox.Show("INSERT MIR_TAOUTDRUGMST ERROR!!!");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }                   
                }

                clsDB.setCommitTran(clsDB.DbCon);

                //TODO:청구 메인 완성후 구현 필요 
                //BaseMir 에서 메인폼 컨트롤 제어
                //Call READ_MIR_OUTDRUG
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void SaveData_SAN()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            long nWrtno = 0;
            string strChk = "";
            string strRowid = "";
            string strSlip = "";
            string strSlipNo = "";

            int intRowAffected = 0; //변경된 Row 받는 변수
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < ss1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strChk = ss1.ActiveSheet.Cells[i, 0].Text;
                    strSlip = VB.Left(ss1.ActiveSheet.Cells[i, 1].Text, 8);
                    strSlipNo = VB.Mid(ss1.ActiveSheet.Cells[i, 1].Text, 10, 5);
                    strRowid = ss1.ActiveSheet.Cells[i, 10].Text;

                    nWrtno = 0;

                    if (strChk == "True")
                    {
                        nWrtno = TID.WRTNO;

                        SQL = "INSERT INTO KOSMOS_PMPA.MIR_SANOUTDRUGMST (WRTNO,SLIPDATE,SLIPNO,Pano,BDate,DeptCode,DrCode," + ComNum.VBLF;
                        SQL += " Bi,ActDate,PART,SEQNO,ENTDATE,DIEASE1,DIEASE2,FLAG,PRTDEPT,SENDDATE,PRTDATE,Remark," + ComNum.VBLF;
                        SQL += " DRBUNHO,WEBSEND,IPDOPD,PRTBUN,DRSABUN,HAPPRINT,Change,MAXNAL,GBAUTO,CHKPRT,GBV252,GBV352 ) " + ComNum.VBLF;
                        SQL += " SELECT '" + TID.WRTNO + "'  ,  SLIPDATE, SLIPNO, Pano, BDate, DeptCode, DrCode, " + ComNum.VBLF;
                        SQL += " Bi,ActDate,PART,SEQNO,ENTDATE,DIEASE1,DIEASE2,FLAG,PRTDEPT,SENDDATE,PRTDATE,Remark," + ComNum.VBLF;
                        SQL += " DRBUNHO,WEBSEND,IPDOPD,PRTBUN,DRSABUN,HAPPRINT,Change,MAXNAL,GBAUTO,CHKPRT,GBV252,GBV352  " + ComNum.VBLF;
                        SQL += "  FROM  KOSMOS_OCS.OCS_OUTDRUGMST " + ComNum.VBLF;
                        SQL += " WHERE ROWID='" + strRowid + "' " + ComNum.VBLF;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            MessageBox.Show("INSERT MIR_SANOUTDRUGMST ERROR!!!");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }


                        //상세내역도 wrtno 빼기
                        SQL = "INSERT INTO KOSMOS_PMPA.MIR_SANOUTDRUG ( WRTNO,SLIPDATE,SLIPNO,Pano,DeptCode,Bun,Sucode," + ComNum.VBLF;
                        SQL += " Qty,REALQTY,Div,DIVQTY,Nal,DOSCODE,OrderNo,Remark,FLAG,EdiSeq,EdiCode,EdiQty," + ComNum.VBLF;
                        SQL += " GbSelf,MULTI,MULTIREMARK,DUR,SCODESAYU,SCODEREMARK ) " + ComNum.VBLF;
                        SQL += "  SELECT '" + TID.WRTNO + "' ,SLIPDATE,SLIPNO,Pano,DeptCode,Bun,Sucode," + ComNum.VBLF;
                        SQL += " Qty,REALQTY,Div,DIVQTY,Nal,DOSCODE,OrderNo,Remark,FLAG,EdiSeq,EdiCode,EdiQty," + ComNum.VBLF;
                        SQL += " GbSelf,MULTI,MULTIREMARK,DUR,SCODESAYU,SCODEREMARK " + ComNum.VBLF;
                        SQL += "  FROM  KOSMOS_OCS.OCS_OUTDRUG " + ComNum.VBLF;
                        SQL += " WHERE  SLIPDATE =TO_DATE( '" + strSlip + "' ,'YYYYMMDD') " + ComNum.VBLF;
                        SQL += "   AND SLIPNO = '" + strSlipNo + "' " + ComNum.VBLF;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            MessageBox.Show("INSERT MIR_SANOUTDRUG ERROR!!!");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }

                        SQL = "";
                        SQL += "UPDATE KOSMOS_PMPA.MIR_SANOUTDRUGMST SET WRTNO=" + TID.WRTNO + " " + ComNum.VBLF;
                        SQL += " WHERE  SLIPDATE =TO_DATE( '" + strSlip + "' ,'YYYYMMDD') " + ComNum.VBLF;
                        SQL += "   AND SLIPNO = '" + strSlipNo + "' " + ComNum.VBLF;
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            MessageBox.Show("INSERT MIR_SANOUTDRUGMST ERROR!!!");
                            clsDB.setRollbackTran(clsDB.DbCon);
                            return;
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                //TODO:청구 메인 완성후 구현 필요 
                //BaseMir 에서 메인폼 컨트롤 제어
                //Call READ_MIR_OUTDRUG
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            READ_OUTDRUGMST("ALL");
        }

        private void FrmComMirOutDrugEntry_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) this.Close(); //폼 권한 조회                           
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            READ_OUTDRUGMST();
        }

        private void READ_OUTDRUGMST(string argGb = "")
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인	

            clsComMirMethod method = new clsComMirMethod();

            string strSlipDate = "";
            int? nSlipNo = 0;
            string strFlag = "";
            string strSDate = "";
            string strEDate = "";
            int nRowss2 = 0;

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt1 = null;

            if (TID.Bi == "31")
            {
                strSDate = TID.FrDate;
            }
            else
            {
                if (TID.JinDate1.Length != 8) { return; }
                if (TID.JinDate1.Trim() == "") { return; }
                strSDate = VB.Left(TID.JinDate1, 4) + "-" + VB.Mid(TID.JinDate1, 5, 2) + "-" + VB.Right(TID.JinDate1, 2);
            }

            DateTime firstDay = new DateTime(Convert.ToInt16(VB.Left(TID.JinDate1, 4)), Convert.ToInt16(VB.Mid(TID.JinDate1, 5, 2)), 01);
            strEDate = firstDay.AddMonths(1).AddDays(-1).ToShortDateString();

            lblPano.Text = TID.Pano;
            lblSName.Text = TID.SName.Trim();
            lblDtName.Text = method.getDtName_Dtno(TID.DTno);
            lblWrtno.Text = TID.WRTNO.ToString().Trim();

            Cursor.Current = Cursors.WaitCursor;

            ss1.ActiveSheet.Columns[10].Visible = false;

            try
            {
                //원외처방전 자료 READ
                SQL = "" + ComNum.VBLF;
                SQL += "SELECT TO_CHAR(a.SlipDate,'YYYYMMDD') SlipDate,a.SlipNo," + ComNum.VBLF;
                SQL += "       TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,a.Bi,a.DeptCode," + ComNum.VBLF;
                SQL += "       TO_CHAR(a.ActDate,'YYYY-MM-DD') ActDate, a.WRTNO, A.Flag, " + ComNum.VBLF;
                SQL += "       MAX(DECODE(b.Bun,'11',b.Nal,0)) Bun11," + ComNum.VBLF;
                SQL += "       MAX(DECODE(b.Bun,'12',b.Nal,0)) Bun12," + ComNum.VBLF;
                SQL += "       MAX(DECODE(b.Bun,'20',b.Nal,0)) Bun20, A.ROWID AROWID" + ComNum.VBLF;
                SQL += "  FROM KOSMOS_OCS.OCS_OUTDRUGMST a, KOSMOS_OCS.OCS_OUTDRUG b " + ComNum.VBLF;
                SQL += " WHERE a.Pano = '" + TID.Pano + "' " + ComNum.VBLF;
                SQL += "   AND a.BDate >= TO_DATE('" + strSDate + "','YYYY-MM-DD') " + ComNum.VBLF;
                SQL += "   AND a.BDate <= TO_DATE('" + strEDate + "','YYYY-MM-DD') " + ComNum.VBLF;

                if (argGb == "ALL")
                {
                    if (TID.IpdOpd == "O")
                    {
                        SQL += "  AND A.PANO ='" + TID.Pano + "'" + ComNum.VBLF;
                    }                    
                }
                else
                {
                    SQL += "   AND a.Flag = 'P' " + ComNum.VBLF;  //인쇄한 원외처방전
                    SQL += "   AND (a.WRTNO = 0 OR a.WRTNO IS NULL OR a.WRTNO=" + TID.WRTNO + ") " + ComNum.VBLF;
                }

                if (TID.Bi == "31")
                {
                    SQL += "  AND BI ='31'" + ComNum.VBLF;
                }
                else
                {
                    SQL += "  AND BI NOT IN ('31') " + ComNum.VBLF;
                }
                SQL += "   AND a.SlipDate=b.SlipDate(+) " + ComNum.VBLF;
                SQL += "   AND a.SlipNo  =b.SlipNo(+)   " + ComNum.VBLF;
                SQL += " GROUP BY a.SlipDate,a.SlipNo,a.BDate,a.Bi,a.DeptCode,a.ActDate,a.WRTNO,a.ROWID, A.FLAG " + ComNum.VBLF;
                SQL += " ORDER BY a.SlipDate,a.SlipNo,a.BDate,a.Bi,a.DeptCode " + ComNum.VBLF;


                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                ss1.ActiveSheet.Rows.Count = 0;
                ss2.ActiveSheet.Rows.Count = 0;
                ss1.ActiveSheet.Rows.Count = dt.Rows.Count;
                ss1.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                ss2.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ss1.ActiveSheet.Cells[i, 1].Text = $"{dt.Rows[i]["SLIPDATE"].ToString()}-{dt.Rows[i]["SLipNo"].ToString().PadLeft(5, '0')}";
                        ss1.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["BDate"].ToString();
                        ss1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["Bi"].ToString();
                        ss1.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["DeptCode"].ToString();
                        ss1.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["ActDate"].ToString();
                        ss1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["Bun11"].ToString();
                        ss1.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["Bun12"].ToString();
                        ss1.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["Bun20"].ToString();
                        ss1.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["WRTNO"].ToString();
                        ss1.ActiveSheet.Cells[i, 10].Text = dt.Rows[i]["AROWID"].ToString();

                        ss1.ActiveSheet.Cells[i, 11].Text = dt.Rows[i]["FLAG"].ToString() == "P" ? "인쇄" : "미인쇄";

                        if (Convert.ToInt64(dt.Rows[i]["WRTNO"]) == TID.WRTNO)
                        {
                            ss1.ActiveSheet.Cells[i, 0].Text = "True";
                            ss1.ActiveSheet.Rows[i].BackColor = Color.Yellow;
                        }

                        strSlipDate = dt.Rows[i]["SlipDate"].ToString();
                        nSlipNo = Convert.ToInt32(dt?.Rows?[i]["SlipNo"]);


                        SQL = "";
                        SQL += "SELECT a.Bun,a.SuCode,a.DivQty,a.Div,a.Nal,a.EdiCode,a.EdiQty, a.Flag , a.GBSELF, " + ComNum.VBLF;
                        SQL += "      b.SuNameK,c.Pname,c.Danwi1,c.Danwi2,c.Spec,a.ROWID " + ComNum.VBLF;
                        SQL += " FROM KOSMOS_OCS.OCS_OUTDRUG a, KOSMOS_PMPA.BAS_SUN b, KOSMOS_PMPA.EDI_SUGA c " + ComNum.VBLF;
                        SQL += " WHERE a.Pano = '" + TID.Pano + "' " + ComNum.VBLF;
                        SQL += "  AND  a.SlipDate = TO_DATE('" + strSlipDate + "', 'YYYYMMDD') " + ComNum.VBLF;
                        SQL += "  AND a.SlipNo = " + nSlipNo + " " + ComNum.VBLF;
                        SQL += "  AND a.SuCode = b.SuNext " + ComNum.VBLF;
                        SQL += "  AND a.EdiCode = c.Code(+) " + ComNum.VBLF;
                        SQL += "ORDER BY a.Bun,a.SuCode " + ComNum.VBLF;
                                          
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            ss2.ActiveSheet.Rows.Count++;

                            if (strFlag != VB.Right(strSlipDate, 4) + "-" + nSlipNo)
                            {
                                ss2.ActiveSheet.Cells[nRowss2, 0].Text = VB.Right(strSlipDate, 4) + "-" + nSlipNo;
                                strFlag = VB.Right(strSlipDate, 4) + "-" + nSlipNo;
                            }

                            ss2.ActiveSheet.Cells[nRowss2, 1].Text = dt1.Rows[j]["SuCode"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 2].Text = string.Format("{0:#####0.00}", dt1.Rows[j]["DivQty"]);
                            ss2.ActiveSheet.Cells[nRowss2, 3].Text = dt1.Rows[j]["Div"].ToString().Trim();
                            ss2.ActiveSheet.Cells[nRowss2, 4].Text = dt1.Rows[j]["Nal"].ToString().Trim();
                            ss2.ActiveSheet.Cells[nRowss2, 5].Text = dt1.Rows[j]["EdiCode"].ToString().Trim();
                            ss2.ActiveSheet.Cells[nRowss2, 6].Text = string.Format("{0:#####0.00}", dt1.Rows[j]["EdiQty"]);
                            ss2.ActiveSheet.Cells[nRowss2, 7].Text = dt1.Rows[j]["SuNameK"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 8].Text = dt1.Rows[j]["Pname"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 9].Text = dt1.Rows[j]["ROWID"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 10].Text = dt1.Rows[j]["Flag"].ToString();
                            ss2.ActiveSheet.Cells[nRowss2, 11].Text = dt1.Rows[j]["GBSELF"].ToString();

                            if (dt1.Rows[j]["Flag"].ToString() == "D")
                            {
                                ss2.ActiveSheet.Rows[nRowss2].BackColor = Color.FromArgb(255, 200, 200);
                            }

                            nRowss2++;
                        }

                        dt1.Dispose();
                        dt1 = null;

                    }
                    dt.Dispose();
                    dt = null;
                }

                ss2.ActiveSheet.Columns[9].Visible = false;
                ss2.ActiveSheet.Columns[10].Visible = false;

                Cursor.Current = Cursors.Default;
                //DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }
    }
}

using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB;
using FarPoint.Win.Spread;
using System.ComponentModel;
using System.Drawing.Printing;

namespace ComPmpaLibB
{
    public partial class frmEaseCash : Form
    {


        string FstrPano = string.Empty;
        string FnIPDNO = string.Empty;
        long FnTRSNO =0;
        clsPmpaPrint CPP = null;

        public frmEaseCash()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            ComFunc CF = new ComFunc();
            
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSelet.Click += new EventHandler(eBtnClick);
            this.btnDel.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.txtPano.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDbClick);
            this.dtpInDate.ValueChanged += new EventHandler(CF.eDtpFormatSet);

            CF = null;
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            clsSpread CS = new clsSpread();

            if (e.ColumnHeader == true)
            {
                CS.setSpdSort(SS1, e.Column, true);
                return;
            }
        }

        void eSpdDbClick(object sender, CellClickEventArgs e)
        {
            string strPano = string.Empty, strSname = string.Empty,
            strInDate = string.Empty, nIPDNO = string.Empty, strOutDate = string.Empty,
            nTRSNo = string.Empty, strJumin = string.Empty, strRemark = string.Empty;

            if (e.Row < 0 || e.Column < 0)
            {
                return;
            }

          
            FstrPano = SS1.ActiveSheet.Cells[e.Row, 0].Text.Trim();
            strSname = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();
            strInDate = SS1.ActiveSheet.Cells[e.Row, 2].Text.Trim();
            strOutDate = SS1.ActiveSheet.Cells[e.Row, 3].Text.Trim();
            FnIPDNO = SS1.ActiveSheet.Cells[e.Row, 16].Text.Trim();
            FnTRSNO = Convert.ToInt64(SS1.ActiveSheet.Cells[e.Row, 17].Text.Trim().ToString()); 

            P_Info.Text = "";
            P_Info.Text = FstrPano + " " + strSname + " " + strInDate;
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtPano)
            {
                if (e.KeyChar == (char)13)
                {
                    txtPano.Text = string.Format("{0:D8}", Convert.ToInt32(txtPano.Text));
                    eSearch1(clsDB.DbCon);
                }
            }
        }

      

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc CF = new ComFunc();

            ComFunc.ReadSysDate(clsDB.DbCon);

            Screen_Clear();

            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1_Sheet1.Rows.Count = 0;

            SS1.ActiveSheet.ClearRange(0, 0, SS1_Sheet1.Rows.Count, SS1_Sheet1.ColumnCount, false);
            SS1_Sheet1.Rows.Count = 0;

            dtpInDate.Text = clsPublic.GstrSysDate;
            

            CF = null;
        }

        void Screen_Clear()
        {
            ComFunc CF = new ComFunc();

            TxtAmt.Text = "";

            CF = null;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSelet)
            {
                eSearch1(clsDB.DbCon);
            }
            else if (sender == btnSave)
            {
                eSave(clsDB.DbCon);
            }
            else if (sender == btnSearch)
            {
                eSearch(clsDB.DbCon);
            }
            else if (sender == btnDel)
            {
                eDel(clsDB.DbCon);
            }
            else if (sender == btnPrint)
            {
                ePrint();
            }
        }
        void eDel(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;
           // int nREAD = 0;
           // long nAmt = 0;
            string strROWID = string.Empty;
          //  DataTable Dt = null;

            if (ComFunc.MsgBoxQ("선택하신 자료를 삭제하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
           

            ComFunc.ReadSysDate(clsDB.DbCon);



          
            for (i = 0; i < ssOrder.ActiveSheet.RowCount; i++)
            {
                if (ssOrder.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strROWID = ssOrder.ActiveSheet.Cells[i, 12].Text;
                    
                  
                    Cursor.Current = Cursors.WaitCursor;


                    SQL += ComNum.VBLF + "UPDATE KOSMOS_PMPA.ETC_IPDPRT                     ";
                    SQL += ComNum.VBLF + "       SET DELDATE =SYSDATE ,                     ";
                    SQL += ComNum.VBLF + "           ENTDATE =SYSDATE ,                     ";
                    SQL += ComNum.VBLF + "           ENTPART = " + clsType.User.IdNumber + " ";
                    SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'                  ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

             
                    Cursor.Current = Cursors.Default;
             

                }

            }
            

            eSearch(clsDB.DbCon);


        }
        void eSearch(PsmhDb pDbCon)
        {
            int i = 0;
            int nREAD = 0;
            string strPano = string.Empty, strSname = string.Empty,
            strInDate = string.Empty, nIPDNO = string.Empty,
            nTRSNo = string.Empty, strJumin = string.Empty, strRemark = string.Empty;

            DataTable Dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            P_Info.Text = "";
            try
            {
                ComFunc CF = new ComFunc();


                SQL = "";
                SQL += " SELECT a.Pano,a.Trsno,a.IPDNO,a.Amt,a.SName,b.Bi,b.DeptCode,c.WardCode,c.RoomCode,a.ROWID,  \r\n";
                SQL += "        TO_CHAR(a.ENTDATE,'YYYY-MM-DD HH24:MI') ENTDATE,TO_CHAR(b.InDate,'YYYY-MM-DD') InDate,a.ENTPART   \r\n";
                SQL += "   FROM  " + ComNum.DB_PMPA + "ETC_IPDPRT a, " + ComNum.DB_PMPA + "IPD_TRANS b   , " + ComNum.DB_PMPA + "IPD_NEW_MASTER  c  \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                SQL += "    AND  a.Pano=b.Pano                                        \r\n";
                SQL += "    AND  a.trsno=b.trsno                                       \r\n";
                SQL += "    AND  a.IPDNO=c.IPDNO                                        \r\n";
                SQL += "    AND  a.PANO=c.PANO                                        \r\n";
                SQL += "    AND  A.EntDate >= TO_DATE('" + dtpInDate.Text + "','yyyy-mm-dd')         ";
                SQL += "    AND  A.EntDate < TO_DATE('" + dtpInDate.Text + "','yyyy-mm-dd') + 1        ";
                SQL += "    AND  (a.DELDATE IS NULL OR a.DELDATE ='')                                       \r\n";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;
                ssOrder.ActiveSheet.RowCount = nREAD;

                if (nREAD == 0)
                {
                    ComFunc.MsgBox("기록이 없습니다.", "확 인");
                    txtPano.Focus();
                    Dt.Dispose();
                    Dt = null;
                    return;
                }
                else
                {
                    for (i = 0; i < nREAD; i++)
                    {
                        ssOrder.ActiveSheet.Cells[i, 0].Text = "";
                        ssOrder.ActiveSheet.Cells[i, 1].Text = Dt.Rows[i]["Pano"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 2].Text = Dt.Rows[i]["SName"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 3].Text = Dt.Rows[i]["Bi"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 4].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 5].Text = Dt.Rows[i]["WARDCODE"].ToString().Trim() +"/" +Dt.Rows[i]["RoomCode"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 6].Text = string.Format("{0:#,##0}", Convert.ToInt64(Dt.Rows[i]["Amt"]));   //(long)VB.Val(Dt.Rows[i]["Amt"]).ToString("#,### "); //Dt.Rows[i]["Amt"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 7].Text = Dt.Rows[i]["EntDate"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 8].Text = CF.Read_SabunName(pDbCon, Dt.Rows[i]["EntPart"].ToString().Trim());
                        ssOrder.ActiveSheet.Cells[i, 9].Text = Dt.Rows[i]["IPDNO"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 10].Text = Dt.Rows[i]["TRSNO"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 11].Text = Dt.Rows[i]["InDate"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 12].Text = Dt.Rows[i]["ROWID"].ToString().Trim();



                    }
                   
                }

             
                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
        void eSearch1(PsmhDb pDbCon)
        {
            int i = 0;
            int nREAD = 0;
            string strPano = string.Empty, strSname = string.Empty, 
            strInDate = string.Empty, nIPDNO = string.Empty,
            nTRSNo = string.Empty, strJumin = string.Empty,strRemark = string.Empty;

            DataTable Dt = null;
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            if (rdoJobFlag0.Checked ==true)
            {
                txtPano.Text = string.Format("{0:D8}", Convert.ToInt32(txtPano.Text));
            }


            P_Info.Text = "";
            try
            {
                ComFunc CF = new ComFunc();


                SQL = "";
                SQL += " SELECT  A.PANO, B.SNAME, TO_CHAR(A.INDATE,'YYYY-MM-DD') INDATE,  \r\n";
                SQL += "        TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE,  \r\n";
                SQL += "        a.ILSU, A.BI, A.DEPTCODE, A.DRCODE, A.GBIPD, A.SANGAMT,  \r\n";
                SQL += "         A.OGPDBUN,A.OGPDBUNdtl, A.AMSET3,b.Secret,    \r\n";
                SQL += "         B.ROOMCODE, B.WARDCODE, A.VCODE, A.IPDNO, A.TRSNO,KOSMOS_OCS.FC_BAS_DOCTOR_DRNAME(A.DRCODE) DRNAME , \r\n";
                SQL += "        TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE, B.AGE,a.GbSPC   \r\n";
                SQL += "   FROM  " + ComNum.DB_PMPA + "IPD_TRANS A, " + ComNum.DB_PMPA + "IPD_NEW_MASTER B                \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                SQL += "    AND A.GBSTS != '7'                                        \r\n";
                SQL += "    AND A.ACTDATE IS NULL  AND A.IPDNO = B.IPDNO  AND A.GBIPD <> 'D'         \r\n";
                if (rdoJobFlag0.Checked == true)
                {
                    SQL += "   AND B.PANO = '" + txtPano.Text + "'        ";
                }
                else if (rdoJobFlag1.Checked == true)
                {
                    SQL += "    AND B.SNAME = '" + txtPano.Text + "'        ";
                }
                else
                {
                    SQL += "   AND A.OUTDATE = TO_DATE('" + txtPano.Text + "','yyyy-mm-dd')         ";
                }
                
                SQL += "  ORDER BY PANO,INDATE DESC, TRSNO                                           ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;
                SS1.ActiveSheet.RowCount = nREAD;

                if (nREAD == 0)
                {
                    ComFunc.MsgBox("기록이 없습니다.", "확 인");
                    txtPano.Focus();
                    Dt.Dispose();
                    Dt = null;
                    return;
                }
                else
                {
                    for (i = 0; i < nREAD; i++)
                    {
                        SS1.ActiveSheet.Cells[i, 0].Text = Dt.Rows[i]["PANO"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 1].Text = Dt.Rows[i]["SNAME"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 2].Text = Dt.Rows[i]["InDate"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 3].Text = Dt.Rows[i]["OutDate"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 4].Text = Dt.Rows[i]["Ilsu"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 5].Text = Dt.Rows[i]["WARDCODE"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 6].Text = Dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 7].Text = Dt.Rows[i]["Bi"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 8].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 9].Text = Dt.Rows[i]["DRNAME"].ToString().Trim();

                        SS1.ActiveSheet.Cells[i, 10].Text = "";
                        if (Dt.Rows[i]["GbIPD"].ToString().Trim()=="9")
                        {
                            SS1.ActiveSheet.Cells[i, 11].Text = "지병";
                        }
                        if (string.Compare(Dt.Rows[i]["SangAmt"].ToString().Trim(), "0") >= 0  )
                        {
                            SS1.ActiveSheet.Cells[i, 12].Text = "상한";
                        }
                        if (Dt.Rows[i]["VCODE"].ToString().Trim() == "V192" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V193" || Dt.Rows[i]["VCODE"].ToString().Trim() == "V194" )
                        {
                            SS1.ActiveSheet.Cells[i, 13].Text = "중증";
                        }

                        SS1.ActiveSheet.Cells[i, 14].Text = "";
                        if (Dt.Rows[i]["AmSet3"].ToString().Trim() == "9" )
                        {
                            SS1.ActiveSheet.Cells[i, 14].Text = "완료";
                        }
                        SS1.ActiveSheet.Cells[i, 15].Text = Dt.Rows[i]["ActDate"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 16].Text =  Dt.Rows[i]["IPDNO"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 17].Text =  Dt.Rows[i]["TRSNO"].ToString().Trim();
                        SS1.ActiveSheet.Cells[i, 18].Text = "";
                        SS1.ActiveSheet.Cells[i, 19].Text =  Dt.Rows[i]["OgPdBundtl"].ToString().Trim();
                        if (Dt.Rows[i]["Secret"].ToString().Trim() != "")
                        {
                            ComFunc.MsgBox("사생활보호 대상요청자 입니다...안내시 주의하십시오.", "안내주의");
                        }
                    }
                }

                if (nREAD == 1)
                {
                    strSname = Dt.Rows[0]["SNAME"].ToString().Trim();
                    strInDate = Dt.Rows[0]["InDate"].ToString().Trim();


                    P_Info.Text = txtPano.Text + " " + strSname + " " + strInDate;
                }


                Dt.Dispose();
                Dt = null;
            }
            catch (Exception ex)
            {
                if (Dt != null)
                {
                    Dt.Dispose();
                    Dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }


        void ePrint()
        {
            string strOPD = string.Empty;
            string strAmt = string.Empty;

          //  int nX = 0;
          //  int nY = 0;
          //  int nCY = 18;
            int i = 0;
            string strPano = string.Empty, strSname = string.Empty,
            strInDate = string.Empty, nIPDNO = string.Empty, strWard = string.Empty, strDept = string.Empty,
            nTRSNo = string.Empty, strRemark = string.Empty , strRoom = string.Empty;  ;


            //ssOrder.ActiveSheet.Cells[i, 0].Text = "";
            //ssOrder.ActiveSheet.Cells[i, 1].Text = Dt.Rows[i]["Pano"].ToString().Trim();
            //ssOrder.ActiveSheet.Cells[i, 2].Text = Dt.Rows[i]["SName"].ToString().Trim();
            //ssOrder.ActiveSheet.Cells[i, 3].Text = Dt.Rows[i]["Bi"].ToString().Trim();
            //ssOrder.ActiveSheet.Cells[i, 4].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
            //ssOrder.ActiveSheet.Cells[i, 5].Text = Dt.Rows[i]["WARDCODE"].ToString().Trim() + "/" + Dt.Rows[i]["RoomCode"].ToString().Trim();
            //ssOrder.ActiveSheet.Cells[i, 6].Text = Dt.Rows[i]["Amt"].ToString().Trim();
            //ssOrder.ActiveSheet.Cells[i, 7].Text = Dt.Rows[i]["EntDate"].ToString().Trim();
            //ssOrder.ActiveSheet.Cells[i, 8].Text = CF.Read_SabunName(pDbCon, Dt.Rows[i]["EntPart"].ToString().Trim());
            //ssOrder.ActiveSheet.Cells[i, 9].Text = Dt.Rows[i]["IPDNO"].ToString().Trim();
            //ssOrder.ActiveSheet.Cells[i, 10].Text = Dt.Rows[i]["TRSNO"].ToString().Trim();
            //ssOrder.ActiveSheet.Cells[i, 11].Text = Dt.Rows[i]["InDate"].ToString().Trim();
            //ssOrder.ActiveSheet.Cells[i, 12].Text = Dt.Rows[i]["ROWID"].ToString().Trim();

            for (i = 0; i < ssOrder.ActiveSheet.RowCount; i++)
            {
                if (ssOrder.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    CPP = new clsPmpaPrint();
                    strPano = ssOrder.ActiveSheet.Cells[i, 1].Text;
                    strSname = ssOrder.ActiveSheet.Cells[i, 2].Text;
                    strDept = ssOrder.ActiveSheet.Cells[i, 4].Text;
                    strWard = ssOrder.ActiveSheet.Cells[i, 5].Text;
                    strAmt = ssOrder.ActiveSheet.Cells[i, 6].Text;
                    nIPDNO = ssOrder.ActiveSheet.Cells[i, 9].Text;
                    strInDate = ssOrder.ActiveSheet.Cells[i, 11].Text;

                    CPP.Report_Print_JungganAmt("**병원용", strPano, strSname, strDept, strWard, "", strAmt, nIPDNO, strInDate, ssPrint, clsPublic.GstrMsgList);
                    CPP.Report_Print_JungganAmt("**환자용", strPano, strSname, strDept, strWard, "",strAmt, nIPDNO, strInDate, ssPrint, clsPublic.GstrMsgList);
                }
            }

        }

        void eSave(PsmhDb pDbCon)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            int i = 0;
            int nREAD = 0;
            long nAmt = 0;
            string strPano = string.Empty, strSname = string.Empty, strAmt = string.Empty,
            strInDate = string.Empty, nIPDNO = string.Empty,
            nTRSNo = string.Empty, strBi = string.Empty, strDept = string.Empty, strWard = string.Empty, strRoom = string.Empty;
            DataTable Dt = null;
            CPP = new clsPmpaPrint();

            nAmt = Convert.ToInt64(TxtAmt.Text.ToString());
            strAmt = nAmt.ToString("#,### ")  ;

            if (FnTRSNO == 0 || nAmt == 0 )
            {
                ComFunc.MsgBox("대상,금액을 선택후 저장하십시오!!", "확인");
                return;
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            try
            {
                Cursor.Current = Cursors.WaitCursor;

                SQL = "";
                SQL += " SELECT  a.PANO,b.SName,a.IPDNO,a.OUTDATE,a.BI,a.DEPTCODE,a.DRCODE,b.ROOMCODE,b.WARDCODE,TO_CHAR(a.InDate,'YYYY-MM-DD') InDate  \r\n";
                SQL += "   FROM  " + ComNum.DB_PMPA + "IPD_TRANS a, " + ComNum.DB_PMPA + "IPD_NEW_MASTER  b               \r\n";
                SQL += "  WHERE 1 = 1                                               \r\n";
                SQL += "    AND a.IPDNO=b.IPDNO                                        \r\n";
                SQL += "   AND a.TRSNO = " + FnTRSNO + "       ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                nREAD = Dt.Rows.Count;
                if (nREAD > 0)
                {
                    strBi = Dt.Rows[i]["BI"].ToString().Trim();
                    strPano = Dt.Rows[i]["PANO"].ToString().Trim();
                    strInDate = Dt.Rows[i]["INDATE"].ToString().Trim();
                    strSname = Dt.Rows[i]["SName"].ToString().Trim();
                    strDept = Dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    strWard = Dt.Rows[i]["WARDCODE"].ToString().Trim();
                    strRoom = Dt.Rows[i]["ROOMCODE"].ToString().Trim();

                    clsDB.setBeginTran(pDbCon);
                    SQL = "";
                    SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_IPDPRT ";
                    SQL += ComNum.VBLF + "       ( PANO,IPDNO,TRSNO,AMT,SNAME,ENTDATE,ENTPART ) ";
                    SQL += ComNum.VBLF + " VALUES ('" + FstrPano + "', ";
                    SQL += ComNum.VBLF + "         " + FnIPDNO + ", ";
                    SQL += ComNum.VBLF + "         " + FnTRSNO + ", ";
                    SQL += ComNum.VBLF + "         " + nAmt + ", ";
                    SQL += ComNum.VBLF + "         '" + strSname + "', ";
                    SQL += ComNum.VBLF + "         SYSDATE, ";
                    SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + ") ";
                 
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox(SqlErr);
                        clsDB.setRollbackTran(pDbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);

                    Cursor.Current = Cursors.Default;


                    CPP.Report_Print_JungganAmt("**병원용",strPano, strSname, strDept, strWard + "/" + strRoom , strRoom,  strAmt, nIPDNO, strInDate, ssPrint, clsPublic.GstrMsgList);

                    CPP.Report_Print_JungganAmt("**환자용", strPano, strSname, strDept, strWard + "/" +  strRoom , strRoom ,  strAmt, nIPDNO, strInDate, ssPrint, clsPublic.GstrMsgList);

                    Screen_Clear();

                }

                else
                {
                    ComFunc.MsgBox("저장실패");
                }

                Dt.Dispose();
                Dt = null;
                eSearch(clsDB.DbCon);
            }
              
                
          
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.setRollbackTran(pDbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }
    }
}

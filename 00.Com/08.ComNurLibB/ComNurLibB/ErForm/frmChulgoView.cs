using ComBase;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : ComNurLibB
    /// File Name       : frmChulgoView.cs
    /// Description     : OCS전달 출고대장
    /// Author          : 이현종
    /// Create Date     : 2018-05-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\nurse\nrer\GuOcs02.frm(FrmChulgoView.frm) >> frmChulgoView.cs 폼이름 재정의" />	
    public partial class frmChulgoView : Form
    {
        bool bolSort = false;

        public frmChulgoView()
        {
            InitializeComponent();
        }

        private void frmChulgoView_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            ComFunc.ReadSysDate(clsDB.DbCon);

            cboWard.Items.Clear();
            cboWard.Items.Add( "입원"    );
            cboWard.Items.Add( "외래"    );
            cboWard.Items.Add( "외래HD"  );
            cboWard.Items.Add( "ER"      );
            cboWard.Items.Add( "OR"      );
            cboWard.Items.Add( "AN"      );
            cboWard.Items.Add( "AG"      );
            cboWard.Items.Add( "내시경"  );
            cboWard.Items.Add("HD");

            Set_Ward();

            //'전산실이 아니면 부서를 변경 못함
            if(clsType.User.Sabun != "4349")
            {
                if(clsType.User.BuseCode == "2")
                {
                    rdoBuse1.Checked = true;
                    btnRegist.Enabled = false;
                }
                panBuse.Enabled = false;
            }

            ss1_Sheet1.RowCount = 0;
            //   '------응급실은 응급실(033109)만 조회 가능하게 추가 2008-12-08 김현욱
            Set_EmergencyRoom();

            clsVbfunc.SetComboDept(clsDB.DbCon, cboDept, "1");
            ss1_Sheet1.Columns[10].Visible = false;
            ss1_Sheet1.Columns[11].Visible = false;
            ss1_Sheet1.Columns[12].Visible = false;
            ss1_Sheet1.Columns[13].Visible = false;
            ss1_Sheet1.Columns[14].Visible = false;
            ss1_Sheet1.Columns[15].Visible = false;
        }

        void Set_Ward()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //'병동코드를 READ
                SQL = "SELECT WardCode FROM KOSMOS_PMPA.BAS_WARD ";
                SQL += ComNum.VBLF + "WHERE WardCode <> 'IU' AND WardCode <> 'IQ' ";
                SQL += ComNum.VBLF + "  AND USED ='Y' ";
                SQL += ComNum.VBLF + "ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
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
                    Cursor.Current = Cursors.Default;
                    return;
                }

                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }

                cboWard.SelectedIndex = 0;

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }
            
        void Set_EmergencyRoom()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT BUSE FROM KOSMOS_ADM.INSA_MST ";
                SQL += ComNum.VBLF + "  WHERE SABUN = '" + ComFunc.SetAutoZero(clsType.User.Sabun, 5) + "' ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
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
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if(dt.Rows[0]["BUSE"].ToString() == "033109")
                {
                    cboWard.Items.Clear();
                    cboWard.Items.Add("ER");
                    cboWard.SelectedIndex = 0;
                    cboWard.Enabled = false;
                    panBuse.Enabled = true;
                    chkMiChulgo.Visible = true;
                    btnRegist.Visible = false;
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("확정하시겠습니까?") == DialogResult.No) return;

            if (IPCH_INSERT_RTN(Get_TDI()) == false) return;
            GetSearchData();
        }

        clsGume00.TABLE_ORD_IPCH[] Get_TDI()
        {
            string SQL = "";
            string SqlErr = "";

            clsGume00.TABLE_ORD_IPCH[] TDl = new clsGume00.TABLE_ORD_IPCH[200];
            DataTable dt = null;

            int i = 0;
            int j = 0;

            double nQty1 = 0;
            double nQty2 = 0;
            double nAmt1 = 0;
            double nAmt2 = 0;
            string strJEP1 = "";
            string strJEP2 = "";
            string strDept1 = "";
            string strDept2 = "";
            string strDeptCode = "";
            string strDel = "";   //'삭제여부
            string strPano = "";
            int nCnt = 0;

            for (i = 0; i < ss1_Sheet1.RowCount; i++)
            {
                if ((bool)ss1_Sheet1.Cells[i, 0].Value == false)
                {
                    nCnt = nCnt + 1;
                }

                if (nCnt == ss1_Sheet1.RowCount)
                {
                    ComFunc.MsgBox("선택한 물품이 없습니다.");
                    TDl = null;
                    return TDl;
                }
            }

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);
            try
            {
                //Paste_SS
                ss2_Sheet1.RowCount = 0;
                for (i = 0; i < ss1_Sheet1.NonEmptyRowCount; i++)
                {
                    if ((bool)ss1_Sheet1.Cells[i, 0].Value == true)
                    {
                        ss2_Sheet1.RowCount = ss2_Sheet1.RowCount + 1;
                        ss2_Sheet1.Cells[ss2_Sheet1.RowCount - 1, 11].Text = ss1_Sheet1.Cells[i, 3].Text.Trim();
                        ss2_Sheet1.Cells[ss2_Sheet1.RowCount - 1, 2].Text = ss1_Sheet1.Cells[i, 5].Text.Trim(); // 수량
                        ss2_Sheet1.Cells[ss2_Sheet1.RowCount - 1, 1].Text = ss1_Sheet1.Cells[i, 8].Text.Trim(); // 물품코드
                        ss2_Sheet1.Cells[ss2_Sheet1.RowCount - 1, 5].Text = ss1_Sheet1.Cells[i, 9].Text.Trim(); // 물품명칭
                        ss2_Sheet1.Cells[ss2_Sheet1.RowCount - 1, 3].Text = ss1_Sheet1.Cells[i, 10].Text.Trim(); // 해당과
                    }
                }
                //Paste_SS

                //'/---------( 신규등록 작업 )--------------/
                //Data_INSERT_Main:
                //TABLE_IPCH_Clear();
                //

                clsGume00.TABLE_ORD_IPCH[] TDI = new clsGume00.TABLE_ORD_IPCH[200];

                for (i = 0; i < ss2_Sheet1.RowCount; i++)
                {
                    strDel = (bool)ss1_Sheet1.Cells[i, 0].Value ? "1" : "0";
                    strDeptCode = ss2_Sheet1.Cells[i, 3].Text.Trim();
                    strJEP1 = ss2_Sheet1.Cells[i, 1].Text.Trim(); //변경물품코드
                    strJEP2 = ss2_Sheet1.Cells[i, 9].Text.Trim(); //변경전
                    nQty1 = VB.Val(ss2_Sheet1.Cells[i, 2].Text.Trim()); //변경수량
                    nQty2 = VB.Val(ss2_Sheet1.Cells[i, 7].Text.Trim()); //Old 수량
                    nAmt1 = VB.Val(ss2_Sheet1.Cells[i, 4].Text.Trim()); //New 입고금액
                    nAmt2 = VB.Val(ss2_Sheet1.Cells[i, 8].Text.Trim()); //Old 입고금액
                    strDept1 = ss2_Sheet1.Cells[i, 3].Text.Trim();
                    strDept2 = ss2_Sheet1.Cells[i, 10].Text.Trim();
                    strPano = ss2_Sheet1.Cells[i, 11].Text.Trim();

                    if (ss2_Sheet1.Cells[i, 6].Text == "") // 불출 rowid
                    {
                        if (strDel == "1")
                        {
                            nQty1 = 0;
                            nAmt1 = 0;
                        }
                        if (nQty1 != nQty2 || nAmt1 != nAmt2 || strJEP1 != strJEP2 || strDel == "1")
                        {
                            j = j + 1;
                            TDI[j].strINDATE = clsPublic.GstrSysDate;
                            if (VB.Left(clsPublic.GstrSysDate, 4) == "2001")
                            {
                                if (VB.Val(VB.Right(strPano, 1)) >= 1 && VB.Val(VB.Right(strPano, 1)) <= 9)
                                {
                                    TDI[j].strGELCODE = strPano; //원무과, 매점
                                }
                                else
                                {
                                    TDI[j].strGELCODE = "BUSE10"; //환자에게 투여분
                                }
                            }
                            else
                            {
                                if (strPano.Trim() == "00099300")
                                {
                                    TDI[j].strGELCODE = "099300";
                                }
                                else
                                {
                                    SQL = "";
                                    SQL += ComNum.VBLF + " SELECT BUCODE FROM KOSMOS_PMPA.BAS_BUSE   ";
                                    SQL += ComNum.VBLF + "  WHERE DEPTCODE = '" + strDeptCode + "'   ";
                                    SQL += ComNum.VBLF + "    AND ORDFLAG = 'Y'                      ";

                                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        Cursor.Current = Cursors.Default;
                                        TDI = null;
                                        return TDl;
                                    }

                                    if (dt.Rows.Count > 0)
                                    {
                                        TDI[j].strGELCODE = dt.Rows[0]["BUCODE"].ToString().Trim();
                                    }

                                    dt.Dispose();
                                    dt = null;
                                }
                            }

                            TDI[j].strIPCHGBN = "3";
                            TDI[j].intSEQNO = 1;
                            TDI[j].strJEPCODE = strJEP1;
                            TDI[j].strGUBUN = "0";
                            TDI[j].dBOXQTY = nQty1;
                            TDI[j].dAMT = nAmt1;
                            TDI[j].dPRICE = 1;
                            //If nQty1 <> 0 And nAmt1 <> 0 Then TDI.Price(j) = Fix(nAmt1 / nQty1)
                            TDI[j].strBUSECODE = strDept1;
                            TDI[j].dBALNO = 0;
                            TDI[j].strPANO = strPano;
                            TDI[j].strROWID = ss2_Sheet1.Cells[i, 6].Text.Trim();     //'불출 ROWID
                            TDI[j].dOLDQTY = nQty2;                 //'변경전수량
                            TDI[j].dOLDAMT = nAmt2;                 //'변경전금액
                        }
                    }
                }
                return TDI;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return TDl;
            }
        }

        //'입출고,출금 등록작업
        bool IPCH_INSERT_RTN(clsGume00.TABLE_ORD_IPCH[] TDI)
        {
            if (TDI == null) return false;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            DataTable dt = null;

            int i = 0;

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(clsDB.DbCon);

            //'*-------------------------------------*
            //'|  입출고내역을 ORD_IPCH에 INSERT     |
            //'*-------------------------------------*
            try
            {
                for(i = 0; i < TDI.Length; i++)
                {
                    if(TDI[i].dQTY != 0 || TDI[i].dAMT != 0)
                    {
                        SQL = " SELECT JEPNAME FROM KOSMOS_ADM.ORD_JEP ";
                        SQL += ComNum.VBLF + " WHERE JEPCODE  = '" + TDI[i].strJEPCODE + "' ";
                        SQL += ComNum.VBLF + "";

                        SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return rtnVal;
                        }

                        if(dt.Rows.Count > 0)
                        {
                            if(dt.Rows[0]["JEPNAME"].ToString().Trim() == TDI[i].strREMARK.Trim())
                            {
                                TDI[i].strREMARK = "";
                            }
                        }

                        SQL = "";
                        SQL = SQL + " INSERT INTO KOSMOS_ADM.ORD_IPCH                                                                                      " + ComNum.VBLF;
                        SQL = SQL + "        (InDate,GelCode,IpchGbn,SeqNo,                                                                     " + ComNum.VBLF;
                        SQL = SQL + "         JepCode,Gubun,BoxQty,Qty,Price,Amt,BuseCode,                                                      " + ComNum.VBLF;
                        SQL = SQL + "         BalNo,Pano,EntDate,Part, GelGe,REMARK,GBTAX, WRTNO, WRTNO_ETC)                                                      " + ComNum.VBLF;
                        SQL = SQL + "  VALUES (TO_DATE('" + TDI[i].strINDATE + "','YYYY-MM-DD'),'" + TDI[i].strGELCODE.Trim() + "',                    " + ComNum.VBLF;
                        SQL = SQL + "          '" + TDI[i].strIPCHGBN + "','" + TDI[i].intSEQNO + "',                                                 " + ComNum.VBLF;
                        SQL = SQL + "          '" + TDI[i].strJEPCODE + "','" + TDI[i].strGUBUN + "','" + TDI[i].dBOXQTY + "','" + TDI[i].dQTY + "',    " + ComNum.VBLF;
                        SQL = SQL + "          '" + TDI[i].dPRICE + "','" + TDI[i].dAMT + "','" + TDI[i].strBUSECODE + "',                           " + ComNum.VBLF;
                        SQL = SQL + "          '" + TDI[i].dBALNO + "','" + TDI[i].strPANO.Trim() + "', TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'), " + ComNum.VBLF;
                        SQL = SQL + "          '" + clsType.User.Sabun + "', '" + TDI[i].strGELGE + "','" + TDI[i].strREMARK.Trim() + "', ";
                        SQL = SQL + "          '" + TDI[i].strGBTAX.Trim() + "', '" + TDI[i].strWRTNO.Trim() + "' , '" + TDI[i].strWRTNO_ETC.Trim() + "' )" + ComNum.VBLF;

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            this.Close();
                            return rtnVal;
                        }

                        //'구매과에서 부서로 출고를 부서에는 입고처리함. 부서도 입출고 관리함. 2006-12-16
                        if(TDI[i].strIPCHGBN == "2")
                        {
                            SQL = " INSERT INTO KOSMOS_ADM.ORD_IPCH_BUSE                                                                        " + ComNum.VBLF;
                            SQL = SQL + "(INDATE,BUSECODE,JEPCODE,IPCHGBN,SEQNO,GUBUN,BOXQTY,                                                   " + ComNum.VBLF;
                            SQL = SQL + "  QTY,PRICE,AMT,ENTDATE,PART,Remark, FEEDBACK)                                                         " + ComNum.VBLF;
                            SQL = SQL + "  VALUES (TO_DATE('" + TDI[i].strINDATE + "','YYYY-MM-DD'),'" + TDI[i].strBUSECODE + "',                     " + ComNum.VBLF;
                            SQL = SQL + "          '" + TDI[i].strJEPCODE.Trim() + "', '0','" + TDI[i].intSEQNO + "',                                  " + ComNum.VBLF;
                            SQL = SQL + "          '" + TDI[i].strGUBUN + "','" + TDI[i].dBOXQTY + "','" + TDI[i].dQTY + "',                         " + ComNum.VBLF;
                            SQL = SQL + "          '" + TDI[i].dPRICE + "','" + TDI[i].dAMT + "',                                                 " + ComNum.VBLF;
                            SQL = SQL + "          TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD'),                                                 " + ComNum.VBLF;
                            SQL = SQL + "          '" + clsType.User.Sabun + "','" + TDI[i].strREMARK.Trim() + "','" + TDI[i].strFEEDBACK.Trim() + "' )         " + ComNum.VBLF;

                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                ComFunc.MsgBox(SqlErr);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                Cursor.Current = Cursors.Default;
                                this.Close();
                                return rtnVal;
                            }
                        }
                    }
                }

                //'*-------------------------------*
                //'| 입출고내역을 수불누계 UPDATE  |
                //'*-------------------------------*

                for (i = 0; i < TDI.Length; i++)
                {
                    if(TDI[i].dQTY != 0)
                    {
                        clsGume00.TDS.strYYMM = VB.Left(TDI[i].strINDATE, 4) + VB.Mid(TDI[i].strINDATE, 6, 2);
                        clsGume00.TDS.strJEPCODE = TDI[i].strJEPCODE.Trim();
                        clsGume00.TDS.dQTY = TDI[i].dQTY;
                        clsGume00.TDS.dAMT = TDI[i].dAMT;

                        switch(TDI[i].strIPCHGBN)
                        {
                            case "0"://' 입고시
                                SUBUL_UPDATE("IPGO", "00"); //' 창고에서 입고처리
                                break;
                            case "2"://' 입고시
                            case "3":
                                //' 창고에서 출고시
                                SUBUL_UPDATE("CHUL", "00");  //' 창고에서 출고처리
                                break;

                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;

                rtnVal = true;
                return rtnVal;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        void SUBUL_UPDATE_MOVE(string arg1)
        {
            if (arg1 == "IPGO")
            {
                clsGume00.TDS.dIPGOQTY = clsGume00.TDS.dIPGOQTY + clsGume00.TDS.dQTY;
                clsGume00.TDS.dIPGOAMT = clsGume00.TDS.dIPGOAMT + clsGume00.TDS.dAMT;
            }
            else
            {
                clsGume00.TDS.dCHULQTY = clsGume00.TDS.dCHULQTY + clsGume00.TDS.dQTY;
                clsGume00.TDS.dCHULAMT = clsGume00.TDS.dCHULAMT + clsGume00.TDS.dAMT;
            }
        }

        void SUBUL_UPDATE(string arg1, string arg2)
        {                 //입고, 출고 //창고코드
            if (arg1.Length == 0) return;

            clsGume00.TDS.strBUSECODE = arg2;
            SUBUL_READ();

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (clsGume00.TDS.strROWID == "")
            {
                // 수불집계가 없는 경우
                SUBUL_UPDATE_MOVE(arg1);

                //SUBUL_UPDATE_INSERT:
                SQL = "";
                SQL = SQL + " INSERT INTO KOSMOS_ADM.ORD_SUBUL                                     " + ComNum.VBLF;
                SQL = SQL + "        (YYMM,JepCode,BuseCode,IwolQty,IwolAmt,            " + ComNum.VBLF;
                SQL = SQL + "        IpgoQty,IpgoAmt,ChulQty,ChulAmt)                   " + ComNum.VBLF;
                SQL = SQL + " VALUES                                                    " + ComNum.VBLF;
                SQL = SQL + "        ('" + clsGume00.TDS.strYYMM + "','" + clsGume00.TDS.strJEPCODE + "',         " + ComNum.VBLF;
                SQL = SQL + "         '" + arg2 + "','" + clsGume00.TDS.dIWOLQTY + "',             " + ComNum.VBLF;  // '이월수량
                SQL = SQL + "         '" + clsGume00.TDS.dIWOLAMT + "','" + clsGume00.TDS.dIPGOQTY + "',      "
                    + ComNum.VBLF;// '이월금액, 입고수량
                SQL += ComNum.VBLF + "         '" + clsGume00.TDS.dIPGOAMT + "','" + clsGume00.TDS.dCHULQTY + "',      "
                    + ComNum.VBLF;  // '입고금액, 출고수량
                SQL += ComNum.VBLF + "         '" + clsGume00.TDS.dCHULAMT + "')                            " + ComNum.VBLF;
                //'출고금액

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
                //' 수불집계가 있는 경우
                SUBUL_UPDATE_MOVE(arg1);

                //SUBUL_UPDATE_Update:
                SQL = "";
                SQL = SQL + " UPDATE KOSMOS_ADM.ORD_SUBUL SET                                                      " + ComNum.VBLF;
                SQL = SQL + "        IpgoQty = '" + clsGume00.TDS.dIPGOQTY + "',  IpgoAmt = '" + clsGume00.TDS.dIPGOAMT + "', " + ComNum.VBLF;
                SQL = SQL + "        ChulQty = '" + clsGume00.TDS.dCHULQTY + "',  ChulAmt = '" + clsGume00.TDS.dCHULAMT + "'  " + ComNum.VBLF;
                SQL = SQL + "  WHERE ROWID =   '" + clsGume00.TDS.strROWID.Trim() + "'                                " + ComNum.VBLF;

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

        void SUBUL_READ()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT IwolQty,IwolAmt,IpgoQty,IpgoAmt,ChulQty,ChulAmt,ROWID   ";
                SQL += ComNum.VBLF + "  FROM KOSMOS_ADM.ORD_SUBUL                                               ";
                SQL += ComNum.VBLF + " WHERE YYMM     = '" + clsGume00.TDS.strYYMM + "'                           ";
                SQL += ComNum.VBLF + "   AND JepCode  = '" + clsGume00.TDS.strJEPCODE + "'                  ";
                SQL += ComNum.VBLF + "   AND BuseCode = '" + clsGume00.TDS.strBUSECODE + "'                 ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    clsGume00.TDS.dIWOLQTY = 0;     clsGume00.TDS.dIWOLAMT = 0;
                    clsGume00.TDS.dIPGOQTY = 0;     clsGume00.TDS.dIPGOAMT = 0;
                    clsGume00.TDS.dCHULQTY = 0;     clsGume00.TDS.dCHULAMT = 0;
                    clsGume00.TDS.strROWID = "";
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsGume00.TDS.dIWOLQTY = VB.Val(dt.Rows[0]["IwolQty"].ToString());
                clsGume00.TDS.dIWOLAMT = VB.Val(dt.Rows[0]["IwolAmt"].ToString());
                clsGume00.TDS.dIPGOQTY = VB.Val(dt.Rows[0]["IpgoQty"].ToString());
                clsGume00.TDS.dIPGOAMT = VB.Val(dt.Rows[0]["IpgoAmt"].ToString());
                clsGume00.TDS.dCHULQTY = VB.Val(dt.Rows[0]["ChulQty"].ToString());
                clsGume00.TDS.dCHULAMT = VB.Val(dt.Rows[0]["ChulAmt"].ToString());
                clsGume00.TDS.strROWID = dt.Rows[0]["ROWID"].ToString();

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            GetSearchData();
            //' 해당과 및 물품명 출력 및 확정물품인지 체크
            Display_Chulgo_Chk();
            clsSpread.gSpreadLineBoder(ss1, 0, 0, ss1_Sheet1.RowCount - 1, ss1_Sheet1.ColumnCount - 1, Color.Black, 1, false, false, true, true);
        }

        void GetSearchData()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strSDate = dtpFDate.Text;
            string strEdate = dtpTDate.Text;
            string strWard = cboWard.Text.Trim();
            btnRegist.Enabled = true;

            ss1_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT TO_CHAR(BDATE,'YYYY-MM-DD') BDATE, GBIO,PANO, SNAME,DEPTCODE,WARDCODE, ORDERCODE, ORDERNAME,GbInfo,";
                SQL += ComNum.VBLF + "  SUM(QTY) QTY,TO_CHAR(EntDate,'MM-DD HH24:MI') EntDate, JEPCODE   ";
                SQL += ComNum.VBLF + " FROM KOSMOS_OCS.OCS_GUMESEND ";
                //'응급실에서 업무의뢰 올림(미출고 조회할 수 있게 해달라고)

                if(chkMiChulgo.Checked == false)
                {
                    SQL += ComNum.VBLF + " WHERE CDATE >=TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "   AND CDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD')";
                }
                else if(chkMiChulgo.Checked == true)
                {
                    SQL += ComNum.VBLF + " WHERE BDATE >=TO_DATE('" + strSDate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "   AND BDATE <=TO_DATE('" + strEdate + "','YYYY-MM-DD')";
                    SQL += ComNum.VBLF + "   AND CDATE IS NULL";
                }

                if (VB.Left(cboDept.Text, 2) != "**") SQL += ComNum.VBLF + " AND DEPTCODE = '" + VB.Left(cboDept.Text, 2) + "' ";
                switch(strWard)
                {
                    case "입원":
                        SQL += ComNum.VBLF + "   AND GBIO ='I'";
                        break;
                    case "외래":
                        SQL += ComNum.VBLF + "   AND GBIO ='O' AND DEPTCODE <> 'HD' ";
                        break;
                    case "외래HD":
                        SQL += ComNum.VBLF + "  AND GBIo = 'O' AND DEPTCODE = 'HD' ";
                        break;
                    case "내시경":
                        SQL += ComNum.VBLF + "  AND WARDCODE = 'ENDO'  ";
                        break;
                    case "AG":
                        SQL += ComNum.VBLF + " AND WARDCODE ='" + strWard + "'                                 ";
                        //'SQL = SQL & " AND A.ORDERCODE IN (SELECT ORDERCODE FROM OCS_ORDERCODE WHERE SLIPNO = '0059')    " & vbLf
                        SQL += ComNum.VBLF + " AND ( A.JEPCODE NOT IN    (SELECT JEPCODE FROM KOSMOS_ADM.ORD_JEP WHERE GBOPLENTAL = 'Y') OR A.JEPCODE IS NULL) ";

                        break;
                    case "AN":
                        SQL += ComNum.VBLF + " AND (WARDCODE ='" + strWard + "'  OR  ORDERCODE IN (SELECT ORDERCODE FROM OCS_ORDERCODE WHERE SLIPNO = '0074'))    ";

                        break;
                    default:
                        if(strWard == "NR")
                        {
                            SQL += ComNum.VBLF + "   AND (WARDCODE = 'NR' OR WARDCODE = 'IQ') ";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + "   AND WARDCODE ='" + strWard + "'";
                        }
                        break;
                }

                SQL += ComNum.VBLF + "  AND ( SUCODE <>'BM5305BH' OR  SUCODE IS NULL )";

                if (rdoBuse0.Checked == true)
                {
                    SQL += ComNum.VBLF + " AND GBBUSE ='1'"; //'관리과
                }
                if (rdoBuse1.Checked == true)
                {
                    SQL += ComNum.VBLF + " AND GBBUSE ='2'"; //'공급실
                }

                SQL += ComNum.VBLF + " GROUP BY BDATE, GBIO, PANO ,SNAME,WARDCODE,DEPTCODE, ORDERCODE, ORDERNAME,GbInfo, EntDATE,JEPCODE ";
                if (rdoSort0.Checked == true)
                {
                    SQL += ComNum.VBLF + " ORDER BY WARDCODE,DEPTCODE, EntDate,PANO, ORDERCODE,GbInfo ";
                }
                if (rdoSort1.Checked == true)
                {
                    SQL += ComNum.VBLF + " ORDER BY ORDERCODE, EntDate ,PANO, WARDCODE,DEPTCODE ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
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
                    Cursor.Current = Cursors.Default;
                    return;
                }
                
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    if(VB.Val(dt.Rows[i]["QTY"].ToString().Trim()) != 0)
                    {
                        ss1_Sheet1.RowCount = ss1_Sheet1.RowCount + 1;

                        if(dt.Rows[i]["GBIO"].ToString().Trim() == "I")
                        {
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        }
                        else
                        {
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        }

                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["QTY"].ToString().Trim();
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["EntDATE"].ToString().Trim();
                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();

                        if (dt.Rows[i]["GbInfo"].ToString() == "")
                        {
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim() + " " +
                                dt.Rows[i]["GbInfo"].ToString().Trim();
                        }
                        else
                        {
                            ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["GbInfo"].ToString().Trim();
                        }

                        ss1_Sheet1.Cells[ss1_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["JEPCODE"].ToString().Trim();
                    }
                }

                ss1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        void Display_Chulgo_Chk()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            int i = 0;
            int j = 0;

            string strJep = "";
            string strPano = "";

            Cursor.Current = Cursors.WaitCursor;
            try
            {

                for(i = 0; i < ss1_Sheet1.RowCount; i++)
                {
                    //'/---- 물품명 Display ----------------------------------------------------

                    strJep = ss1_Sheet1.Cells[i, 8].Text.Trim();

                    SQL = " SELECT JEPCODE, JEPNAME FROM KOSMOS_ADM.ORD_JEP ";
                    SQL = SQL + ComNum.VBLF + " WHERE JEPCODE = '" + strJep + "' ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ss1_Sheet1.Cells[i, 9].Text = dt.Rows[0]["JEPNAME"].ToString().Trim();
                    }
                    else
                    {
                        ss1_Sheet1.Cells[i, 9].Text = "";
                        ss1_Sheet1.Cells[i, 9].BackColor = Color.FromArgb(192, 255, 255);
                    }

                    dt.Dispose();
                    dt = null;
                    //'/---- 해당과 Display ----------------------------------------------------

                    strPano = ss1_Sheet1.Cells[i, 3].Text;

                    SQL = " SELECT DeptCode FROM KOSMOS_PMPA.IPD_NEW_MASTER   "; // '＃원본 윗줄
                    SQL = SQL + ComNum.VBLF + "  WHERE Pano='" + strPano + "'            ";
                    SQL = SQL + ComNum.VBLF + "    AND AmSet6='*'                             "; // '구분변경자 아닌것
                    SQL = SQL + ComNum.VBLF + "    AND GBSTS IN ('0', '2', '3', '4')          ";
                    SQL = SQL + ComNum.VBLF + "    AND OUTDATE IS NULL                        ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ss1_Sheet1.Cells[i, 10].Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    dt.Dispose();
                    dt = null;

                    //'당일의 접수마스타에서 진료과를 READ
                    SQL = " SELECT DeptCode FROM KOSMOS_PMPA.OPD_MASTER               ";
                    SQL = SQL + ComNum.VBLF + "  WHERE Pano='" + strPano + "'                        ";
                    SQL = SQL + ComNum.VBLF + "    AND BDate=TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ss1_Sheet1.Cells[i, 9].Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                        if(dt.Rows.Count > 1)
                        {
                            ss1_Sheet1.Cells[i, 9].Text += "," + dt.Rows[1]["DeptCode"].ToString().Trim();
                        }
                        if (dt.Rows.Count > 2)
                        {
                            ss1_Sheet1.Cells[i, 9].Text += "," + dt.Rows[2]["DeptCode"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;
                    //'환자마스타에서 최종진료과를 Dispaly

                    SQL = "SELECT DeptCode FROM KOSMOS_PMPA.BAS_PATIENT   ";
                    SQL = SQL + ComNum.VBLF + " WHERE Pano='" + strPano + "'             ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        ss1_Sheet1.Cells[i, 10].Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                    //'/---- 확정된 물품인지 체크 ------------------------------------------

                    SQL = "  SELECT a.Jepcode, a.Pano, b.Pano, b.ROWID bROWID ";
                    SQL = SQL + "  FROM  KOSMOS_OCS.OCS_GUMESEND a, KOSMOS_ADM.ORD_IPCH b                  ";
                    SQL = SQL + "  WHERE b.InDate >= TO_DATE('" + dtpFDate.Text.Trim() + "','yyyy-mm-dd')   ";
                    SQL = SQL + "    AND b.InDate <= TO_DATE('" + dtpFDate.Text.Trim() + "','yyyy-mm-dd')   ";
                    SQL = SQL + "    AND b.IpchGbn = '3'                                                   ";
                    SQL = SQL + "    AND b.Pano = '" + strPano + "'                                        ";
                    SQL = SQL + "    AND b.JepCode = '" + strJep + "'                                      ";
                    SQL = SQL + "    AND b.Indate = a.Cdate ";

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for(j = 1; j < 10; j++)
                        {
                            ss1_Sheet1.Cells[i, j].BackColor = Color.FromArgb(255, 224, 192);
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
              
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            ss1_Sheet1.RowCount = 0;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            if (ComFunc.MsgBoxQ("자료를 출력하시겠습니까?") == DialogResult.No) return;

            Set_Print();
        }

        void Set_Print()
        {
            btnPrint.Enabled = false;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            clsSpread CS = new clsSpread();
            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "OCS전달 출고대장";

            strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 16, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += CS.setSpdPrint_String(" 조회일자 : " + dtpFDate.Text + "부터 " + dtpTDate.Text + "까지" +
                VB.Space(5) + " 출력시간 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 11), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 40, 20, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, false, true, false, true, true, false, false);

            CS.setSpdPrint(ss1, PrePrint, setMargin, setOption, strHeader, strFooter);
            CS = null;

            btnPrint.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkMiChulgo_CheckedChanged(object sender, EventArgs e)
        {
            if(chkMiChulgo.Checked == true)
            {
                lblDate.Text = "처방기간";
            }
            else
            {
                lblDate.Text = "출고기간";
            }
        }

        private void ss1_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ss1, e.Column, ref bolSort, true);
                return;
            }
        }
    }
}

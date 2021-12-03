using ComBase; //기본 클래스
using FarPoint.Win;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

/// <summary>
/// Description : OCS 추가오더 알림창
/// Author : 이상훈
/// Create Date : 2017.07.24
/// </summary>
/// <history>
/// </history>
/// <seealso cref="FrmOrdersView.frm, "/>

namespace ComLibB
{
    public partial class frmOrdersView : Form
    {
        clsSpread SP = new clsSpread();
        clsPrint ClsPrint = new clsPrint();

        FarPoint.Win.Spread.CellType.NumberCellType Numtype = new FarPoint.Win.Spread.CellType.NumberCellType();
        FarPoint.Win.Spread.CellType.TextCellType TxtType = new FarPoint.Win.Spread.CellType.TextCellType();

        string SQL;
        DataTable dt = null;
        string SqlErr = "";
        string strReturn = "";
        int intRowAffected = 0; //변경된 Row 받는 변수

        string strPano;
        string strOcsDate;
        string strRowid;

        string strAGE;
        string strAgeP;
        string strRoomP;
        string strDeptP;
        string strInDate;
        string strDrName;
        string strMonthP;
        string strBDate;
        string strSNameP;
        string strSexP;
        string strDateP;

        string strOrderCodeP;
        string strSlipNoP;
        string strOrderP;
        string strOrderNameP;
        string strContentsP;
        string strQtyP;
        string strNalP;
        string strGroupP;
        string strDivP;
        string strImivP;
        string strTFlagP;
        string strErP;
        string strRemarkP;

        string strRO;
        string strillCode;
        string strillName;
        string strPrtillName;

        int j = 0;

        public frmOrdersView(string sPano, string sOcsDate, string sRowid)
        {
            strPano = sPano;
            strOcsDate = sOcsDate;
            strRowid = sRowid;

            InitializeComponent();
        }

        private void frmOrdersView_Load(object sender, EventArgs e)
        {
            //string sTemp="";

            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)//폼 권한 조회
            //{
            //    this.Close();
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            try
            {
                SQL = "";
                SQL += " SELECT a.PANO, a.SNAME, TO_CHAR(a.InDate, 'YYYY-MM-DD') INDATE1    \r";
                SQL += "      , a.DEPTCODE, b.DRNAME, a.SEX, a.AGE, a.WARDCODE, a.ROOMCODE  \r";
                SQL += "   FROM ADMIN.IPD_NEW_MASTER a                                \r";
                SQL += "      , ADMIN.BAS_DOCTOR     b                                \r";
                SQL += "  WHERE a.DrCode = b.DrCode(+)                                      \r";
                SQL += "    AND a.PANO = '" + strPano + "'                                  \r";
                SQL += "  ORDER BY a.InDate DESC                                            \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    lblInfo.Text =  "  등록번호 : " + dt.Rows[0]["PANO"].ToString().Trim().PadRight(15);
                    lblInfo.Text += "성    명 : " + dt.Rows[0]["SNAME"].ToString().Trim().PadRight(15);
                    lblInfo.Text += "입 원 일 : " + dt.Rows[0]["INDATE1"].ToString().Trim().PadRight(15) + "\n\n";
                    lblInfo.Text += "  진 료 과 : " + dt.Rows[0]["DEPTCODE"].ToString().Trim().PadRight(15);
                    lblInfo.Text += "진료의사 : " + dt.Rows[0]["DRNAME"].ToString().Trim().PadRight(15);
                    lblInfo.Text += "성    별 : " + dt.Rows[0]["SEX"].ToString().Trim().PadRight(15) + "\n\n";
                    lblInfo.Text += "  나    이 : " + dt.Rows[0]["AGE"].ToString().Trim().PadRight(15);
                    lblInfo.Text += "병    동 : " + dt.Rows[0]["WARDCODE"].ToString().Trim().PadRight(15);
                    lblInfo.Text += "호    실 : " + dt.Rows[0]["ROOMCODE"].ToString().Trim().PadRight(15);
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            btnSearch_Click(btnSearch, e);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)//권한 확인
            //{
            //    return; 
            //}

            SP.Spread_All_Clear(ssOrder);
            
            try
            {
                SQL = "";
                SQL += " SELECT '' DC                                                                                                                           \r";
                SQL += "      , TO_CHAR(A.BDate, 'YYYY-MM-DD') BDate                                                                                            \r";
                SQL += "      , case when gbstatus = 'D' then 'D/C'                                                                                             \r";
                SQL += "             else decode(a.gborder, 'F', 'Pre', 'T', 'Post', 'M', 'Adm',                                                                \r";
                SQL += "                         ADMIN.FC_OCS_SLIP_GUBUN(a.slipno, substr(a.doscode, 1, 2), a.bun))  end gbstatus                          \r";
                SQL += "      , a.ordercode                                                                                                                     \r";
                SQL += "           , decode(a.gbprn, 'S', '(A)', 'A', '(선)', 'B', '(수)', '') ||                                                               \r";
                SQL += "             decode(b.ordercode, '', '삭제된 코드입니다.. 변경요망', case when a.slipno >= 'A1' and a.slipno <= 'A4' then remark else   \r";
                SQL += "             DECODE(A.GBBOTH, '1', case when B.ORDERNAMES = '' then b.ordername                                                         \r";
                SQL += "                                        else case when b.dispheader != '' then b.dispheader || ' ' || ordername                         \r";
                SQL += "                                        else ordername || ' ' || ordernames end                                                         \r";
                SQL += "                                    end || A.GBINFO, case when B.ORDERNAMES = '' then b.ordername                                       \r";
                SQL += "                                                         else case when b.dispheader != '' then b.dispheader || ' ' || ordername        \r";
                SQL += "                                                         else ordername || ' ' || ordernames end                                        \r";
                SQL += "                                                     end) end) ordname                                                                  \r";
                SQL += "      , case when b.gbinfo = '1' then A.gbinfo else decode(b.gbdosage, '1', ADMIN.FC_OCS_ODOSAGE_NAME(a.doscode),                  \r";
                SQL += "                                                           ADMIN.FC_OCS_OSPECIMAN_NAME(a.doscode, a.slipno)) end gbinfo            \r";
                SQL += "      , a.contents                                                                                                                      \r";
                SQL += "      , a.realqty                                                                                                                       \r";
                SQL += "      , a.gbdiv                                                                                                                         \r";
                SQL += "      , a.nal                                                                                                                           \r";
                SQL += "      , ADMIN.FC_BAS_DOCTOR_DRNAME(a.staffid) || ' ' || substr(TO_CHAR(A.EntDate, 'YYYY-MM-DD HH24:Mi'), 9, 16) EntDate1           \r";
                SQL += "      , case when a.bun >= '16' and a.bun <= '21' and a.gbngt <> '' then gbngt                                                          \r";
                SQL += "             when a.bun >= '16' and a.bun <= '21' and gbgroup <> '' then a.gbgroup                                                      \r";
                SQL += "             when a.bun >= '28' and a.bun <= '39' and(a.gbgroup = '' or ADMIN.is_number(a.gbgroup) = 1) then a.gbngt              \r";
                SQL += "             when a.bun >= '28' and a.bun <= '39' and not (a.gbgroup = '' or ADMIN.is_number(a.gbgroup) = 1) then gbgroup         \r";
                SQL += "        else gbgroup                                                                                                                    \r";
                SQL += "         end gbngt                                                                                                                      \r";
                SQL += "      , a.gber                                                                                                                          \r";
                SQL += "      , a.gbself                                                                                                                        \r";
                SQL += "      , case when not (a.slipno >= 'A1' and A.slipno <= 'A4') and a.remark <> '' then '#'                                               \r";
                SQL += "             when not (a.slipno >= 'A1' and A.slipno <= 'A4') and a.gbprn <> '' then a.gbprn                                            \r";
                SQL += "             when not(a.slipno >= 'A1' and A.slipno <= 'A4') and a.gbtflag <> '' then a.gbtflag                                         \r";
                SQL += "             when not(a.slipno >= 'A1' and A.slipno <= 'A4') and a.gbprn = 'A' then ''                                                  \r";
                SQL += "          end gbprn                                                                                                                     \r";
                SQL += "      , a.gbport                                                                                                                        \r";
                SQL += "      , a.sucode                                                                                                                        \r";
                SQL += "      , a.bun                                                                                                                           \r";
                SQL += "      , a.slipno                                                                                                                        \r";
                SQL += "      , case when a.realqty = '1/2' then '0.5'                                                                                          \r";
                SQL += "             when a.realqty = '1/3' then '0.33'                                                                                         \r";
                SQL += "             when a.realqty = '2/3' then '0.66'                                                                                         \r";
                SQL += "             when a.realqty = '1/4' then '0.25'                                                                                         \r";
                SQL += "             when a.realqty = '3/4' then '0.75'                                                                                         \r";
                SQL += "             when a.realqty = '1/5' then '0.2'                                                                                          \r";
                SQL += "             when a.realqty = '2/5' then '0.4'                                                                                          \r";
                SQL += "             when a.realqty = '3/5' then '0.6'                                                                                          \r";
                SQL += "             when a.realqty = '4/5' then '0.8'                                                                                          \r";
                SQL += "        else case when ADMIN.is_number(a.realqty) = 1 then a.realqty else '1' end                                                 \r";
                SQL += "         end Rqty                                                                                                                       \r";
                SQL += "      , a.doscode                                                                                                                       \r";
                SQL += "      , a.gbboth                                                                                                                        \r";
                SQL += "      , a.gbinfo                                                                                                                        \r";
                SQL += "      , case when a.slipno >= 'A1' and a.slipno <= 'A4' then '' else a.remark end remark                                                \r";
                SQL += "      , b.disprgb                                                                                                                       \r";
                SQL += "      , b.gbboth                                                                                                                        \r";
                SQL += "      , b.gbinfo                                                                                                                        \r";
                SQL += "      , b.gbqty                                                                                                                         \r";
                SQL += "      , b.gbdosage                                                                                                                      \r";
                SQL += "      , b.nextcode                                                                                                                      \r";
                SQL += "      , b.gbimiv                                                                                                                        \r";
                SQL += "      , a.orderno                                                                                                                       \r";
                SQL += "      , '' col33                                                                                                                        \r";
                SQL += "      , decode(a.gborder, 'F', '92', 'T', '93', 'M', '91',                                                                              \r";
                SQL += "                          ADMIN.FC_OCS_SLIP_GUBUN(a.slipno, substr(a.doscode, 1, 2), a.bun)) slipgubun                             \r";
                SQL += "      , decode(a.bcontents, 0, ' ', a.bcontents) bcontents                                                                              \r";
                SQL += "      , '' col36                                                                                                                        \r";
                SQL += "      , ADMIN.FC_BAS_DOCTOR_DRNAME(a.staffid) || ' ' || substr(TO_CHAR(A.EntDate, 'YYYY-MM-DD HH24:Mi'), 9, 16) drname             \r";
                SQL += "   FROM ADMIN.OCS_IORDER    A                                                                                                      \r";
                SQL += "      , ADMIN.OCS_ORDERCODE B                                                                                                      \r";
                SQL += "  WHERE Ptno        = '" + strPano + "'                                                                                                 \r";
                SQL += "    AND A.ORDERCODE = B.ORDERCODE                                                                                                       \r";
                SQL += "    AND A.SLIPNO    = B.SLIPNO                                                                                                          \r";
                SQL += "    AND A.GbStatus IN (' ','D','D+')                                                                                                    \r";
                SQL += "    AND A.ENTDATE >= TO_DATE('" + strOcsDate + "','YYYY-MM-DD HH24:MI')                                                                 \r";
                //'아래 두줄은 간호업무에서 DC한 자료를 읽지 안기 위해서 추가함
                SQL += "    AND A.OrderSite Not Like 'DC%'                                                                                                      \r";
                SQL += "    AND A.OrderSite <> 'CAN'                                                                                                            \r";
                SQL += "    AND A.SLIPNO NOT IN ('A7')                                                                                                          \r";
                SQL += "  ORDER BY A.BDate, A.Seqno                                                                                                             \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, ssOrder, 0, true);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["slipno"].ToString().Trim() == "A1" || dt.Rows[i]["slipno"].ToString().Trim() == "A2" ||
                            dt.Rows[i]["slipno"].ToString().Trim() == "A3" || dt.Rows[i]["slipno"].ToString().Trim() == "A4")
                        {
                            ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
                        }

                        if (ssOrder.ActiveSheet.Cells[i, 2].Text.Trim() == "D/C")
                        {
                            ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.Red;
                        }
                        else
                        {
                            ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[i]["disprgb"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("해당하는 오더지를 출력 하시겠습니까?", "확인", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.No)
            {
                return;
            }

            fn_OrderPrint();

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                SQL = "";
                SQL += " UPDATE ADMIN.OCS_MSG          \r";
                SQL += "    SET STATE = 'Y'                 \r";
                SQL += "  WHERE ROWID ='" + strRowid + "'   \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void fn_OrderPrint()
        {   
            ComFunc.ReadSysDate(clsDB.DbCon);

            strDateP = clsPublic.GstrSysDate;

            strBDate = ssOrder.ActiveSheet.Cells[0, 1].Text;

            try
            {
                SQL = "";
                SQL += " SELECT a.ROOMCODE, a.DEPTCODE, a.AGE, a.DRCODE     \r";
                SQL += "      , TO_CHAR(a.INDATE,'YYYY-MM-DD') INDATE       \r";
                SQL += "      , b.SNAME, b.SEX, b.JUMIN1                    \r";
                //SQL += "      , ADMIN.FC_OCS_ILLS(a.PANO, 'I', TO_CHAR(a.INDATE, 'YYYY-MM-DD'), a.DEPTCODE, '2')\r";
                SQL += "   FROM ADMIN.IPD_NEW_MASTER a                \r";
                SQL += "      , ADMIN.BAS_PATIENT    b                \r";
                SQL += "  WHERE a.PANO = b.PANO                             \r"; 
                SQL += "    AND a.ACTDATE IS NULL                           \r";
                SQL += "    AND a.PANO = '" + strPano + "'                  \r";               
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    strAGE = dt.Rows[0]["AGE"].ToString().Trim();
                    if (int.Parse(strAGE) < 3 && dt.Rows[0]["JUMIN1"].ToString().Trim() != "")
                    {
                        strMonthP = clsOrderEtc.CHK_AGE_MONTH_GESAN(clsDB.DbCon, dt.Rows[0]["JUMIN1"].ToString().Trim(), strBDate) + "개월";
                    }
                    strAgeP = strAGE + "세";
                    strRoomP = dt.Rows[0]["ROOMCODE"].ToString().Trim();
                    strDeptP = dt.Rows[0]["DEPTCODE"].ToString().Trim();
                    strInDate = dt.Rows[0]["INDATE"].ToString().Trim();
                    strDrName = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[0]["DRCODE"].ToString().Trim());
                    strSNameP = dt.Rows[0]["SNAME"].ToString().Trim();
                    strSexP = dt.Rows[0]["SEX"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            SP.Spread_All_Clear(ssPrint);

            for (int i = 0; i < ssOrder.ActiveSheet.RowCount; i++)
            {
                ssPrint.ActiveSheet.ActiveRow.Add();

                if (ssOrder.ActiveSheet.Cells[i, 2].Text.Trim() != "")
                {
                    strOrderCodeP = ssOrder.ActiveSheet.Cells[i, 2].Text;
                    strOrderNameP = ssOrder.ActiveSheet.Cells[i, 4].Text;
                    strImivP = ssOrder.ActiveSheet.Cells[i, 5].Text;
                    
                    if (VB.InStr(ssOrder.ActiveSheet.Cells[i, 6].Text, ".") == 0)
                    {   
                        strContentsP = ssOrder.ActiveSheet.Cells[i, 6].Text == "0" ? " " : ssOrder.ActiveSheet.Cells[i, 6].Text;  
                    }
                    else
                    {
                        strContentsP = string.Format("{0:##0.##}", int.Parse(ssOrder.ActiveSheet.Cells[i, 6].Text));
                    }

                    if (VB.InStr(ssOrder.ActiveSheet.Cells[i, 7].Text, ".") == 0)
                    {
                        strQtyP = ssOrder.ActiveSheet.Cells[i, 7].Text == "0" ? " " : ssOrder.ActiveSheet.Cells[i, 7].Text;
                    }
                    else
                    {
                        strQtyP = string.Format("{0:##0.##}", int.Parse(ssOrder.ActiveSheet.Cells[i, 7].Text));
                    }
                    strDivP = ssOrder.ActiveSheet.Cells[i, 8].Text == "0" ? " " : ssOrder.ActiveSheet.Cells[i, 8].Text;
                    strNalP = ssOrder.ActiveSheet.Cells[i, 9].Text == "0" ? " " : ssOrder.ActiveSheet.Cells[i, 9].Text;
                    strGroupP = ssOrder.ActiveSheet.Cells[i, 11].Text == "0" ? " " : ssOrder.ActiveSheet.Cells[i, 11].Text;
                    strSlipNoP = ssOrder.ActiveSheet.Cells[i, 14].Text;
                    strTFlagP = "";
                    if (ssOrder.ActiveSheet.Cells[i, 14].Text == "T") strTFlagP = " [T]";
                    if (ssOrder.ActiveSheet.Cells[i, 14].Text == "P") strTFlagP = " [P]";
                    if (ssOrder.ActiveSheet.Cells[i, 12].Text == "M") strTFlagP = " [M]";
                    if (ssOrder.ActiveSheet.Cells[i, 12].Text == "E")
                    {
                        strErP = "응)";
                    }
                    else
                    {
                        strErP = "   ";
                    }

                    if (ssOrder.ActiveSheet.Cells[i, 0].Value != null)
                    {
                        strErP = "DC)";
                    }
                    strRemarkP = ssOrder.ActiveSheet.Cells[i, 23].Text;

                    strOrderP = ssOrder.ActiveSheet.Cells[i, 33].Text;

                    ssPrint.ActiveSheet.Cells[j, 0].Text = strErP;
                    ssPrint.ActiveSheet.Cells[j, 1].Text = strOrderCodeP;
                    ssPrint.ActiveSheet.Cells[j, 2].Text = strOrderNameP;
                    ssPrint.ActiveSheet.Cells[j, 3].Text = strContentsP;
                    ssPrint.ActiveSheet.Cells[j, 4].Text = strQtyP;
                    ssPrint.ActiveSheet.Cells[j, 5].Text = strImivP;
                    ssPrint.ActiveSheet.Cells[j, 6].Text = strGroupP;
                    ssPrint.ActiveSheet.Cells[j, 7].Text = strDivP;
                    ssPrint.ActiveSheet.Cells[j, 8].Text = strNalP;
                    ssPrint.ActiveSheet.Cells[j, 9].Text = strTFlagP;

                    if (strRemarkP != "")
                    {
                        if (strSlipNoP.Trim() != "A1" && strSlipNoP.Trim() != "A2" && strSlipNoP.Trim() != "A3" && strSlipNoP.Trim() != "A4")
                        {
                            j += 1;
                            ssPrint.ActiveSheet.ActiveRow.Add();
                            SP.setRowMerge(ssPrint, j);
                            ssPrint.ActiveSheet.Cells[j, 1].Text = VB.Space(13) + "* " + strRemarkP;
                            strRemarkP = "";
                        }
                    }
                    ssPrint.Sheets[0].Cells[j, 0, j, (int)ssPrint.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.Black, 1, false, false, false, true);
                    j += 1;
                }
            }

            try
            {
                SQL = "";
                SQL += " SELECT a.RO, B.ILLCODE, nvl(B.ILLNAMEE, B.ILLNAMEK) ILLNAME    \r";
                SQL += "   FROM ADMIN.OCS_IILLS a                                  \r";
                SQL += "      , ADMIN.BAS_ILLS b                                  \r";
                SQL += "  WHERE A.IllCode = B.IllCode                                   \r";
                SQL += "    AND A.Ptno    = '" + strPano + "'                           \r";
                SQL += "    AND BDate    >= TO_DATE('" + strInDate + "','YYYY-MM-DD')   \r";
                SQL += "  ORDER BY BDate, Main                                          \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        strRO = dt.Rows[i]["RO"].ToString();
                        strillCode = dt.Rows[i]["ILLCODE"].ToString();
                        strillName = dt.Rows[i]["ILLNAME"].ToString();

                        strPrtillName += VB.Space(5) + "상병명 : " + strRO + "  " + strillCode + " : " + strillName;
                        break; // for 문을 걸고 왜 1 Row만 읽고 빠져 나갈까????? for문을 걸지 않음 될텐데....일단 As_Is 대로....
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = false;

            strTitle = "POHANG SAINT MARY'S HOSPITAL" + "\r\n";
            strTitle += "PHYSICIAN'S ORDERS" + "\r\n\r\n";
            strTitle += VB.Space(5) + "진료일자 : " + strBDate + VB.Space(42) + "출력일자: " + strDateP.Trim() + "(추가처방)" + "\r\n\r\n";
            strTitle += "  Name : " + strSNameP.PadRight(14) + "Hosp.No : " + strPano + "     Sex : " + strSexP;
            strTitle += "   Age : " + strAgeP + " Room: " + strRoomP + "  Dept: " + strDeptP;
            strTitle += VB.Space(3) + "---------------------------------------------------------------------------------------------------------";
            strTitle += VB.Space(5) + strPrtillName;
            //strTitle += VB.Space(3) + "---------------------------------------------------------------------------------------------------------";
            strTitle += "\r\n";

            strHeader = "";
            strHeader = SP.setSpdPrint_String(strTitle, new Font("굴림체", 14, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);


            strFooter = SP.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += VB.Space(75) + "주치의: " + strDrName;
            //strFooter += SP.setSpdPrint_String("출력일자:" + DateTime.Now.ToString(), new Font("굴림체", 15), clsSpread.enmSpdHAlign.Right, false, true);
            

            setMargin = new clsSpread.SpdPrint_Margin(2, 10, 5, 10, 5, 10);
            setOption = new clsSpread.SpdPrint_Option(FarPoint.Win.Spread.PrintOrientation.Portrait, FarPoint.Win.Spread.PrintType.All, 0, 0, true, true, true, true, true, false, false);

            SP.setSpdPrint(ssPrint, PrePrint, setMargin, setOption, strHeader, strFooter);

            MessageBox.Show("출력 되었습니다", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

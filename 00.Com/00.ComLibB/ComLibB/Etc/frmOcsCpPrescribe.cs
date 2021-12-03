using ComBase;
using ComBase.Controls;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmOcsCpPrescribe : Form
    {
        FarPoint.Win.Spread.FpSpread spd = null;
        string CPCODE = string.Empty;
        string CPDAY = string.Empty;

        public frmOcsCpPrescribe()
        {
            InitializeComponent();
        }

        public frmOcsCpPrescribe(FarPoint.Win.Spread.FpSpread spd)
        {
            InitializeComponent();
            this.spd = spd;
        }

        private enum IpdOrderCol
        {
            DC = 0,
            ORDERGUBUN,
            NAMEENG,
            PLUSNAME,
            CONTENTS,
            REALQTY,
            DIV,
            NAL,
            POWDER,
            NGT,
            ER, //10
            SELF,
            PRN,
            POTABLE,
            SUCODE,
            BUN,
            SLIPNO,
            QTY,
            DOSAGE,
            GBBOTH,
            GBINFO, //20
            REMARK,
            DISPRGB,
            CGBBOTH,
            CGBINFO,
            GBQTY,
            GBDOSAGE,
            NEXTCODE,
            GBIMIV,
            ORDERNO,
            ROWID,  //30
            GBACT,
            SEQNO,
            GBSTATUS,
            ORDERCODE,
            SLIPGUBUN,
            CDIV,
            VERBALID,
            BCONTENTS,
            GBSEND,
            DRCODE, //40
            DEPTCODE,
            ENTDATE,
            ADD,
            RESULT,
            SUNSUNAP,
            SIMFLAG,
            STAFFID,
            ORDERSITE,
            MULTI,
            MULTIREASON, //50
            DURGUBUN,
            TRANSREMARK,
            MAYAK,
            CONSULT,
            MAYAKREMARK,
            GBIOE,
            CGBSPCNO,
            NSTSMS,
            GBTAX,
            SUGBF,  //60
            VERBALGUBUN,
            VORDERNO,
            GBVERB,
            PRNMARK,
            PRNREMARK,
            INSULINSCALE,
            INSULINUNIT,
            INSULINSDATE,
            INSULINEDATE,
            INSULINMAX, //70
            POWDERSAYUMARK,
            POWDERSAYU,
            ASA,
            INSULINCOFMTIME,
            PRNDOSCODE,
            PRNTERM,
            PRNNOTITIME,
            PRNROWID,
            AIRSHT,
            PCHASU, //80
            SUBUL_WARD,
            HIGHRISKGBN,
            DOSCODE,
            REALQTY2,
            GBDIV,
            NAL2,
            ONEDAYINORD,
            GSADD,
            SUPSTATUS,
            CBUN,
            BURNADD,
            OPGUBUN,
            GBPICKUP
        }
        int nSetSEQNO;
        string setORDERCODE;
        string setSUCODE;
        string setBUN;
        string setGbNgt;
        string setSLIPNO;
        double nSetREALQTY;
        double nSetQTY;
        int nSetNAL;
        int nSetGBDIV;
        string setDOSCODE;
        string setGBBOTH;
        string setGBINFO;
        string setGBER;
        string setGBSELF;
        string setGBSPC;
        string setREMARK;
        string setILLCODES;
        string setBOOWI1;
        string setBOOWI2;
        string setBOOWI3;
        string setBOOWI4;
        string setGBORDER;
        string setGBPRN;
        string setGBTFLAG;
        string setGBPORT;
        string setGBGROUP;
        string setItemCD;
        double nSetCONTENTS;
        double nSetBCONTENTS;
        string setILLCODES_KCD6;
        string setPRN_REMARK;
        string setCPNAME;
        string setPRN_INS_GBN;
        string setPRN_INS_UNIT;
        string setPRN_INS_SDATE;
        string setPRN_INS_EDATE;
        string setPRN_INS_MAX;
        string setPRN_DOSCODE;
        string setPRN_TERM;
        string setPRN_NOTIFY;
        string setPRN_UNIT;
        string setSUBUL_WARD;
        string setCORDERCODE;
        string setCSUCODE;
        string setCBUN;
        string setPRN_ORDSEQ;
        string strBun = "";

        string setMayak = string.Empty;
        string setMayakRemark = string.Empty;


        private void frmOcsCpPrescribe_Load(object sender, EventArgs e)
        {
            ////폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            if(spd == null)
            {
                Close();
                return;
            }

            ssIpdOrder.Sheets.Clear();
            ssIpdOrder.Sheets.Add(spd.ActiveSheet);
            GetDataCpCode();
            fn_ColumnHidden();
        }

        void fn_ColumnHidden()
        {
            //처방 Spread
            for (int i = 14; i <= ssIpdOrder.ActiveSheet.ColumnCount - 1; i++)
            {
                ssIpdOrder_Sheet1.Columns.Get(i).Visible = false;
            }
            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.MULTI).Visible = true;
            //ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.MULTIREASON).Visible = true;
            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.DURGUBUN).Visible = true;

            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.VERBALID).Visible = true;
            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.VERBALGUBUN).Visible = true;
            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.PRNMARK).Visible = true;
            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.POWDERSAYUMARK).Visible = true;

            //ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.CGBSPCNO).Visible = true;
            //ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.GBTAX).Visible = true;
            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.SUBUL_WARD).Visible = true;
            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.HIGHRISKGBN).Visible = true;
            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.ONEDAYINORD).Visible = true;
            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.GSADD).Visible = true;
            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.BURNADD).Visible = true;
            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.OPGUBUN).Visible = true;
            ssIpdOrder_Sheet1.Columns.Get((int)BaseOrderInfo.IpdOrderCol.SEDATION).Visible = false;
        }

        private void GetDataCpCode()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            ssCPList_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "    BASCD, BASNAME                                       ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_BASCD                    ";
                SQL = SQL + ComNum.VBLF + "WHERE GRPCDB    = 'CP관리'                                ";
                SQL = SQL + ComNum.VBLF + "  AND GRPCD     = 'CP코드관리'                             ";
                SQL = SQL + ComNum.VBLF + "  AND BASNAME1  = 'IPD'                                  ";
                SQL = SQL + ComNum.VBLF + "  AND VFLAG1    = '" + clsType.User.DeptCode + "'        ";
                SQL = SQL + ComNum.VBLF + "  AND USECLS    = '1' -- CP의뢰서 관련 추가(2021-04-28)     ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BASCD                                           ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssCPList_Sheet1.RowCount = dt.Rows.Count;
                    ssCPList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssCPList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["BASNAME"].ToString().Trim();
                        ssCPList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["BASCD"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인
            if (ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (CPCODE.IsNullOrEmpty())
            {
                ComFunc.MsgBoxEx(this, "CP를 선택해주세요.");
                return;
            }

            if (CPDAY.IsNullOrEmpty())
            {
                ComFunc.MsgBoxEx(this, "날짜를 선택해주세요.");
                return;
            }

            int nMulti = 0;
            bool bOK = false;
            bool bDelete = false;

            btnSave.Enabled = false;

            if (ssIpdOrder.ActiveSheet.NonEmptyRowCount > 0)
            {
                if (ssDay_Sheet1.Cells[ssDay_Sheet1.ActiveRowIndex, 0].ForeColor == Color.RoyalBlue)
                {
                    if(ComFunc.MsgBoxQEx(this, "이미 저장된 CP처방이 있습니다 정말 수정하시겠습니까?") == DialogResult.No)
                    {
                        btnSave.Enabled = true;
                        return;
                    }

                    bDelete = true;
                }

                if (ComFunc.MsgBoxQEx(this, CPDAY + "일자 CP Set을 저장 하시겠습니까?  ", "CP Set 저장", MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    clsDB.setBeginTran(clsDB.DbCon);

                    if (bDelete == true && CP_Prescribe_Delete(CPDAY) == false)
                    {
                        //clsDB.setRollbackTran(clsDB.DbCon);
                        //ComFunc.MsgBoxEx(this, "기존 CP처방 삭제를 실패했습니다.");
                        //return;
                    }

                    setGBORDER = "C";

                    DateTime dtpSysDate = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();

                    //for (int i = clsOrdFunction.GnReadOrder; i < ssIpdOrder.ActiveSheet.NonEmptyRowCount; i++)
                    for (int i = 0; i < ssIpdOrder.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        if (ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.ErOrderCol.DC].Text != "True")
                        {
                            setORDERCODE = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.Trim();
                            if (setORDERCODE != "")
                            {
                                #region  변수 클리어
                                setSUCODE = string.Empty;
                                setBUN = string.Empty;
                                setGbNgt = string.Empty;
                                setSLIPNO = string.Empty;
                                nSetREALQTY = 0;
                                nSetQTY = 0;
                                nSetNAL = 0;
                                nSetGBDIV = 0;
                                setDOSCODE = string.Empty;
                                setGBBOTH = string.Empty;
                                setGBINFO = string.Empty;
                                setGBER = string.Empty;
                                setGBSELF = string.Empty;
                                setGBSPC = string.Empty;
                                setREMARK = string.Empty;
                                setILLCODES = string.Empty;
                                setBOOWI1 = string.Empty;
                                setBOOWI2 = string.Empty;
                                setBOOWI3 = string.Empty;
                                setBOOWI4 = string.Empty;
                                setGBPRN = string.Empty;
                                setGBTFLAG = string.Empty;
                                setGBPORT = string.Empty;
                                setGBGROUP = string.Empty;
                                setItemCD = string.Empty;
                                nSetCONTENTS = 0;
                                nSetBCONTENTS = 0;
                                setILLCODES_KCD6 = string.Empty;
                                setPRN_REMARK = string.Empty;
                                setCPNAME = string.Empty;
                                setPRN_INS_GBN = string.Empty;
                                setPRN_INS_UNIT = string.Empty;
                                setPRN_INS_SDATE = string.Empty;
                                setPRN_INS_EDATE = string.Empty;
                                setPRN_INS_MAX = string.Empty;
                                setPRN_DOSCODE = string.Empty;
                                setPRN_TERM = string.Empty;
                                setPRN_NOTIFY = string.Empty;
                                setPRN_UNIT = string.Empty;
                                setSUBUL_WARD = string.Empty;
                                setCORDERCODE = string.Empty;
                                setCSUCODE = string.Empty;
                                setCBUN = string.Empty;
                                setPRN_ORDSEQ = string.Empty;
                                strBun = string.Empty;
                                setMayak = string.Empty;
                                setMayakRemark = string.Empty;
                                #endregion

                                nSetSEQNO = i + 1;
                                setSUCODE = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim();
                                setBUN    = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.BUN].Text;
                                setSLIPNO = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text;

                                if (ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text.Trim() != "")
                                {
                                    nSetREALQTY = Double.Parse(ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text);
                                }
                                else
                                {
                                    nSetREALQTY = 1;
                                }

                                if (ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.QTY].Text.Trim() != "")
                                {
                                    nSetQTY = VB.Val(ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.QTY].Text);
                                }
                                else
                                {
                                    nSetQTY = 1;
                                }

                                nSetNAL = (int)VB.Val(ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.NAL].Text);
                                if (ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.DIV].Text.Trim() == "")
                                {
                                    nSetGBDIV = 1;
                                }
                                else
                                {
                                    nSetGBDIV = (int)VB.Val(ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.DIV].Text);
                                }

                                nMulti = nSetGBDIV;

                                setDOSCODE = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.DOSCODE].Text;
                                setGBBOTH = "0";
                                setGBINFO = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text;
                                setGBER = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.ER].Text == "" ? " " : ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.ER].Text;
                                if (setBUN == "28" || setBUN == "34" || setBUN == "35")
                                {
                                    setGbNgt = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.NGT].Text;
                                }
                                else
                                {
                                    setGbNgt = "";
                                }

                                if (ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.PRN].Text == "T")
                                {
                                    setGBTFLAG = "T";
                                }
                                else
                                {
                                    setGBTFLAG = " ";
                                }

                                if (ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.PRN].Text == "P" ||
                                    ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.PRN].Text == "S")
                                {
                                    setGBPRN = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.PRN].Text;
                                }
                                else
                                {
                                    setGBPRN = " ";
                                }

                                if (ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.POTABLE].Text == "M")
                                {
                                    setGBPORT = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.POTABLE].Text;
                                }
                                else
                                {
                                    setGBPORT = " ";
                                }

                                setGBSELF = VB.Left(ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.SELF].Text, 1);
                                setGBSPC = "";
                                setREMARK = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.REMARK].Text;
                                setILLCODES = "";
                                setBOOWI1 = "";
                                setBOOWI2 = "";
                                setBOOWI3 = "";
                                setBOOWI4 = "";
                                setGBGROUP = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.NGT].Text;
                                nSetCONTENTS = VB.Val(ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text);
                                nSetBCONTENTS = VB.Val(ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.BCONTENTS].Text);

                                setItemCD = clsOrderEtc.fn_Get_ItemCD(clsDB.DbCon, setORDERCODE);
                                if (strBun.Equals("11") || strBun.Equals("12") || strBun.Equals("20"))
                                {
                                    if (setItemCD.Equals("A"))
                                    {
                                        if (nSetBCONTENTS != 0)
                                        {
                                            nSetQTY = Math.Truncate((nSetREALQTY * (nSetCONTENTS / nSetBCONTENTS) / nMulti) + 0.9);
                                        }
                                        else
                                        {
                                            nSetQTY = Math.Truncate((nSetREALQTY / nMulti) + 0.9);
                                        }
                                    }
                                    else if (setItemCD.Equals("B"))
                                    {
                                        if (nSetBCONTENTS != 0)
                                        {
                                            nSetQTY = Math.Truncate((nSetREALQTY * (nSetCONTENTS / nSetBCONTENTS) / nMulti) + 0.9);
                                        }
                                        else
                                        {
                                            nSetQTY = Math.Truncate(nSetREALQTY + 0.9);
                                        }
                                    }
                                    else
                                    {
                                        if (nSetBCONTENTS != 0)
                                        {
                                            nSetQTY = nSetREALQTY * (nSetCONTENTS / nSetBCONTENTS);
                                        }
                                        else
                                        {
                                            nSetQTY = nSetREALQTY;
                                        }
                                    }
                                }

                                setILLCODES_KCD6 = "";
                                setPRN_REMARK = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.PRNREMARK].Text;
                                setCPNAME = "";

                                setPRN_INS_GBN = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.INSULINSCALE].Text;
                                setPRN_INS_UNIT = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.INSULINUNIT].Text;
                                setPRN_INS_SDATE = "";
                                setPRN_INS_EDATE = "";
                                setPRN_INS_MAX = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.INSULINMAX].Text;
                                setPRN_DOSCODE = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.PRNDOSCODE].Text;
                                setPRN_TERM = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.PRNTERM].Text;
                                setPRN_NOTIFY = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.PRNNOTITIME].Text;
                                setPRN_UNIT = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.INSULINUNIT].Text;

                                setSUBUL_WARD = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.SUBUL_WARD].Text;
                                setCORDERCODE = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text.Trim();
                                setCSUCODE = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text.Trim();
                                setCBUN = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.CBUN].Text;
                                setPRN_ORDSEQ = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.PRNORDSEQ].Text;

                                setMayak = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.MAYAK].Text.Trim();
                                setMayakRemark = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.MAYAKREMARK].Text.Trim();

                                if (string.Compare(setSLIPNO.Trim(), "A1") >= 0 && string.Compare(setSLIPNO.Trim(), "A4") <= 0 &&
                                    (setORDERCODE.Trim().Equals("S/O") || setORDERCODE.Trim().Equals("V/S") || setORDERCODE.Trim().Left(2).Equals("VO")))
                                {
                                    setREMARK = ssIpdOrder.ActiveSheet.Cells[i, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text.Trim();
                                }

                                bOK = SetOrder_Insert(CPCODE, CPDAY, "CP처방" + CPDAY.To<int>(0).ToString("00"));
                                if (bOK == false)
                                {
                                    Cursor.Current = Cursors.Default;
                                    break;
                                }
                            }
                        }
                    }

                    btnSave.Enabled = true;
                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBoxEx(this, "저장 되었습니다!", "확인");
                }
            }
        }

        bool CP_Prescribe_Delete(string strDay)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;

            SQL = "";
            SQL += " DELETE ADMIN.OCS_OPRM_CP                                                                                          \r";
            SQL += "  WHERE CPCODE   = '" + ssCPList_Sheet1.Cells[ssCPList_Sheet1.ActiveRowIndex, 1].Text.Trim() + "'                       \r";
            SQL += "    AND CPDAY    = '" + strDay + "'                                                                                     \r"; 

            //SQL += "    where PRMNAME  = 'CP처방" + Convert.ToInt32(strDay).ToString("00") + "'                         \r";
            //SQL += "      and CPCODE   = '" + ssCPList_Sheet1.Cells[ssCPList_Sheet1.ActiveRowIndex, 1].Text.Trim() + "'                         \r";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr + " : CP약속처방 삭제중 오류 발생!!!");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            rtnVal = true;
            return rtnVal;
        }

        bool SetOrder_Insert(string strCpCode, string strDay, string strSetName)
        {

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
                     

            SQL = "";
            SQL += " INSERT INTO ADMIN.OCS_OPRM_CP                             \r";
            SQL += "        (CPCODE, CPDAY                                          \r";
            SQL += "      , PRMNAME, SEQNO                                          \r";
            SQL += "      , ORDERCODE, SUCODE, BUN                                  \r";
            SQL += "      , SLIPNO, REALQTY, QTY                                    \r";
            SQL += "      , NAL, GBDIV, DOSCODE                                     \r";
            SQL += "      , GBBOTH, GBINFO, GBER                                    \r";
            SQL += "      , GBSELF, GBSPC, REMARK                                   \r";
            SQL += "      , ILLCODES, BOOWI1, BOOWI2                                \r";
            SQL += "      , BOOWI3, BOOWI4, ENTDATE                                 \r";
            SQL += "      , GBORDER, GBPRN, GBTFLAG                                 \r";
            SQL += "      , GBPORT, GBGROUP, CONTENTS                               \r";
            SQL += "      , BCONTENTS                                               \r";
            SQL += "      , ILLCODES_KCD6, PRN_REMARK, CPNAME                       \r";
            SQL += "      , PRN_INS_GBN, PRN_INS_UNIT, PRN_INS_SDATE                \r";
            SQL += "      , PRN_INS_EDATE, PRN_INS_MAX, PRN_DOSCODE                 \r";
            SQL += "      , PRN_TERM, PRN_NOTIFY, PRN_UNIT                          \r";
            SQL += "      , SUBUL_WARD, CORDERCODE, CSUCODE                         \r";
            SQL += "      , CBUN, PRN_ORDSEQ                                        \r";
            SQL += "      , MAYAK                                                   \r";
            SQL += "      , MAYAKREMARK)                                            \r";
            SQL += " VALUES                                                         \r";
            SQL += "        ('" + strCpCode + "'                                    \r";
            SQL += "       , '" + strDay + "'                                       \r";
            SQL += "       , '" + strSetName + "'                                   \r";
            SQL += "       ,  " + nSetSEQNO + "                                     \r";
            SQL += "       , '" + setORDERCODE + "'                                 \r";
            SQL += "       , '" + setSUCODE + "'                                    \r";
            SQL += "       , '" + setBUN + "'                                       \r";
            SQL += "       , '" + setSLIPNO + "'                                    \r";
            SQL += "       , '" + nSetREALQTY + "'                                  \r";
            SQL += "       , "  + nSetQTY + "                                       \r";
            SQL += "       , "  + nSetNAL + "                                       \r";
            SQL += "       , "  + nSetGBDIV + "                                     \r";
            SQL += "       , '" + setDOSCODE + "'                                   \r";
            SQL += "       , '" + setGBBOTH + "'                                    \r";
            SQL += "       , '" + setGBINFO + "'                                    \r";
            SQL += "       , '" + setGBER + "'                                      \r";
            SQL += "       , '" + setGBSELF + "'                                    \r";
            SQL += "       , '" + setGBSPC + "'                                     \r";
            SQL += "       , '" + setREMARK.Replace("'", "`") + "'                  \r";
            SQL += "       , '" + setILLCODES + "'                                  \r";
            SQL += "       , '" + setBOOWI1 + "'                                    \r";
            SQL += "       , '" + setBOOWI2 + "'                                    \r";
            SQL += "       , '" + setBOOWI3 + "'                                    \r";
            SQL += "       , '" + setBOOWI4 + "'                                    \r";
            SQL += "       , SYSDATE                                                \r";
            SQL += "       , '" + setGBORDER + "'                                   \r";
            SQL += "       , '" + setGBPRN + "'                                     \r";
            SQL += "       , '" + setGBTFLAG + "'                                   \r";
            SQL += "       , '" + setGBPORT + "'                                    \r";
            SQL += "       , '" + setGBGROUP + "'                                   \r";
            SQL += "       , "  + nSetCONTENTS + "                                  \r";
            SQL += "       , "  + nSetBCONTENTS + "                                 \r";
            SQL += "       , '" + setILLCODES_KCD6 + "'                             \r";
            SQL += "       , '" + setPRN_REMARK + "'                                \r";
            SQL += "       , '" + setCPNAME + "'                                    \r";
            SQL += "       , '" + setPRN_INS_GBN + "'                               \r";
            SQL += "       , '" + setPRN_INS_UNIT + "'                              \r";

            if (setPRN_REMARK.NotEmpty())
            {
                SQL += "       , TO_DATE('" + DateTime.Now.ToString("yyyy-MM-dd") + "', 'YYYY-MM-DD')      \r";
                SQL += "       , TO_DATE('" + setPRN_INS_EDATE + "', 'YYYY-MM-DD')      \r";
            }
            else
            {
                SQL += "       , NULL                                               \r";
                SQL += "       , NULL                                               \r";
            }
            
            SQL += "       , '" + setPRN_INS_MAX + "'                               \r";
            SQL += "       , '" + setPRN_DOSCODE + "'                               \r";
            SQL += "       , '" + setPRN_TERM + "'                                  \r";
            SQL += "       , '" + setPRN_NOTIFY + "'                                \r";
            SQL += "       , '" + setPRN_UNIT + "'                                  \r";
            SQL += "       , '" + setSUBUL_WARD + "'                                \r";
            SQL += "       , '" + setCORDERCODE + "'                                \r";
            SQL += "       , '" + setCSUCODE.Trim() + "'                            \r";
            SQL += "       , '" + setCBUN + "'                                      \r";
            SQL += "       , '" + setPRN_ORDSEQ + "'                                \r";
            SQL += "       , '" + setMayak + "'                                     \r";
            SQL += "       , '" + setMayakRemark + "'                               \r";
            SQL += "                             )                                  \r";

            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
            if (SqlErr != "")
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBoxEx(this, SqlErr + " : 약속 처방 등록 중 오류 발생!!!");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            rtnVal = true;
            return rtnVal;
        }        

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ssCPList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssCPList_Sheet1.RowCount == 0) return;

            GetCpDayValue(ssCPList_Sheet1.Cells[e.Row, 1].Text.Trim());
        }

        void GetCpDayValue(string strCpCode)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            Font font = new Font("맑은 고딕", 9.75f, FontStyle.Regular);

            ssDay_Sheet1.RowCount = 0;
            CPCODE = strCpCode;

            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL += ComNum.VBLF + "	WITH CP_DATA AS                                                                                                          ";
                SQL += ComNum.VBLF + "    (                                                                                                                      ";
                SQL += ComNum.VBLF + "	    SELECT (LEVEL || '') AS DAY_VAL                                                                                      ";
                SQL += ComNum.VBLF + "    	  FROM DUAL                                                                                                          ";
                SQL += ComNum.VBLF + "	    CONNECT BY LEVEL <= (SELECT CPDAY                                                                                    ";
                SQL += ComNum.VBLF + "	                           FROM ADMIN.OCS_CP_MAIN A                                                                 ";
                SQL += ComNum.VBLF + "	                          WHERE CPCODE = '" + strCpCode + "'                                                             ";
                SQL += ComNum.VBLF + "	                            AND GBIO = 'I'                                                                               ";
                SQL += ComNum.VBLF + "	                            AND SDATE = (SELECT MAX(SDATE) FROM ADMIN.OCS_CP_MAIN WHERE CPCODE = A.CPCODE))         ";
                SQL += ComNum.VBLF + " 	)                                                                                                                        ";
                SQL += ComNum.VBLF + " 	SELECT DAY_VAL, CASE WHEN EXISTS                                                                                                  ";
                SQL += ComNum.VBLF + "     (                                                                                                                     ";
                SQL += ComNum.VBLF + "     	SELECT 1                                                                                                             ";
                SQL += ComNum.VBLF + "     	  FROM ADMIN.OCS_OPRM_CP                                                                                        ";
                SQL += ComNum.VBLF + "     	 WHERE CPCODE = '" + strCpCode + "'                                                                                  ";
                SQL += ComNum.VBLF + "     	   AND CPDAY = DAY_VAL                                                                                               ";
                SQL += ComNum.VBLF + "     ) THEN DAY_VAL END CPDAY                                                                                              ";
                SQL += ComNum.VBLF + "   FROM CP_DATA                                                                                                            ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
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

                font = new Font(font, FontStyle.Bold);

                ssDay_Sheet1.RowCount = dt.Rows.Count;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ssDay_Sheet1.Cells[i, 0].Text = dt.Rows[i]["DAY_VAL"].ToString().Trim();
                    if (dt.Rows[i]["CPDAY"].ToString().Trim().Length > 0)
                    {
                        ssDay_Sheet1.Cells[Convert.ToInt32(dt.Rows[i]["CPDAY"].ToString().Trim()) - 1, 0].ForeColor = Color.RoyalBlue;
                        ssDay_Sheet1.Cells[Convert.ToInt32(dt.Rows[i]["CPDAY"].ToString().Trim()) - 1, 0].Font = font;
                    }
                    
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssDay_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssIpdOrder_Sheet1.RowCount == 0) return;

            CPDAY = ssDay_Sheet1.Cells[e.Row, 0].Text.Trim();

            fn_Read_SetOrder(ssSetOrder, CPCODE, "CP처방" + CPDAY.To<int>(0).ToString("00"), 1);
        }

        void fn_Read_SetOrder(FarPoint.Win.Spread.FpSpread SpdNm, string strCpCode, string strSetName, int sGbOrder)
        {
            SpdNm.ActiveSheet.RowCount = 0;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            try
            {
                SQL = "";
                SQL += " SELECT A.ORDERCODE,                                                                                                    \r";
                SQL += "        CASE WHEN A.ORDERCODE  IN('S/O', 'V/S', 'V001', 'V002', 'V003', 'V005', 'V008', 'V009') THEN REMARK             \r";
                SQL += "             WHEN B.ORDERNAMES IS NOT NULL THEN B.ORDERNAME  || ' ' ||  B.ORDERNAMES                                    \r";
                SQL += "             WHEN B.DISPHEADER IS NOT NULL THEN B.DISPHEADER || ' ' ||  B.ORDERNAME                                     \r";
                SQL += "             ELSE B.ORDERNAME                                                                                           \r";
                SQL += "         END  ORDERNAME1                                                                                                \r";
                SQL += "      , CASE WHEN A.GBINFO IS NOT NULL THEN A.GBINFO                                                                    \r";
                SQL += "             WHEN A.BUN < '30' THEN ADMIN.FC_OCS_ODOSAGE_NAME(A.DOSCODE)                                           \r";
                SQL += "             ELSE ADMIN.FC_OCS_OSPECIMAN_NAME(A.DOSCODE, A.SLIPNO) END DOSNAME                                     \r";
                SQL += "      , A.CONTENTS, A.QTY, A.GBDIV, A.NAL, A.GBSELF, A.GBER, A.GBPORT                                                   \r";
                SQL += "      , A.GBGROUP, A.REMARK, A.SUCODE, B.GBINPUT, A.PRN_REMARK                                                          \r";
                SQL += "      , A.PRN_INS_GBN, A.PRN_INS_UNIT, A.PRN_INS_SDATE, A.PRN_INS_EDATE                                                 \r";
                SQL += "      , A.PRN_INS_MAX, A.PRN_DOSCODE, A.PRN_TERM, A.PRN_NOTIFY, A.PRN_UNIT                                              \r";
                SQL += "      , A.SUBUL_WARD, A.ROWID RID                                                                                       \r";
                SQL += "      , NVL(A.ILLCODES_KCD6, A.ILLCODES) ILLCODES                                                                       \r";
                SQL += "      , A.BOOWI1,  A.BOOWI2, A.BOOWI3, A.BOOWI4, A.GBINFO , A.SLIPNO                                                    \r";
                SQL += "      , B.SENDDEPT, B.DISPHEADER, B.GBBOTH, B.ORDERNAME, B.ORDERNAMES                                                   \r";
                SQL += "   FROM ADMIN.OCS_OPRM_CP   A                                                                                      \r";
                SQL += "      , ADMIN.OCS_ORDERCODE B                                                                                      \r";
                SQL += "    WHERE a.PRMname   = '" + strSetName + "'                                                                            \r";
                SQL += "      AND A.CPCODE = '" + strCpCode + "'";

                if (sGbOrder == 3)
                {
                    SQL += "    AND A.GBORDER = 'P'                                                                                             \r";    //검사처방
                }
                SQL += "    AND a.ORDERCODE = b.ORDERCODE(+)                                                                                    \r";
                SQL += "  ORDER BY a.Seqno, a.Slipno                                                                                            \r";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    SpdNm.ActiveSheet.RowCount = 0;
                    dt.Dispose();
                    dt = null;
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 0)
                {
                    SpdNm.ActiveSheet.RowCount = 0;
                    dt.Dispose();
                    dt = null;
                    return;
                }

                SpdNm.ActiveSheet.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, SpdNm, 0, true);
                }

                dt.Dispose();
                dt = null;

                SpdNm.ActiveSheet.RowCount = SpdNm.ActiveSheet.NonEmptyRowCount;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }
    }
}

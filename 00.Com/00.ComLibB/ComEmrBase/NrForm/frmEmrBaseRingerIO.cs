using ComBase;
using ComBase.Controls;
using FarPoint.Win;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    public partial class frmEmrBaseRingerIO : Form
    {
        #region //변수선언
        EmrPatient p = null;
        string mstrChartDate = "";
        ContextMenu PopupMenu = null;
        int mPopRow = -1;
        string mstrFormNameGb = "기록지관리";
        string mstrFormNo = "";
        string mstrUpdateNo = "0";
        #endregion //변수선언

        #region //생성자
        public frmEmrBaseRingerIO()
        {
            InitializeComponent();
        }

        public frmEmrBaseRingerIO(EmrPatient po, string strChartDate, string strFormNo, string strUpdateNo)
        {
            InitializeComponent();
            p = po;
            mstrChartDate = strChartDate;
            mstrFormNo = strFormNo;
            mstrUpdateNo = strUpdateNo;
        }

        private void frmEmrBaseRingerIO_Load(object sender, EventArgs e)
        {
            if (mstrChartDate != "")
            {
                dtpOrderDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDate(mstrChartDate, "D"));
            }

            //if (p != null)
            //{
            //    GetOrderData();
            //}
        }
        #endregion //생성자

        #region //폼 이벤트

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetOrderData();
        }

        private void dtpOrderDate_ValueChanged(object sender, EventArgs e)
        {
            GetOrderData();
        }

        private void mnuItemValue_Click(object sender, EventArgs e) // 이벤트 헨들러
        {
            string strPopMenuName = ((MenuItem)sender).Text.Trim();
            ssView.ContextMenu = null;

            if (mPopRow == -1) return;

            string strSITEGB = ssView_Sheet1.Cells[mPopRow, 1].Text.Trim();
            string strORDROWID = ssView_Sheet1.Cells[mPopRow, 2].Text.Trim();
            string strOrderDate = ssView_Sheet1.Cells[mPopRow, 5].Text.Trim();
            double OrderNo = VB.Val(ssView_Sheet1.Cells[mPopRow, 3].Text.Trim());
            string OrderCode = ssView_Sheet1.Cells[mPopRow, 4].Text.Trim();
            string OrderName = ssView_Sheet1.Cells[mPopRow, 6].Text.Trim();
            double ActSeq = 0; //= VB.Val(ssView_Sheet1.Cells[mPopRow, 0].Text.Trim());

            ActSeq = 0;
            string mACTGB = "00";

            if (strPopMenuName == "시작")
            {
                mACTGB = "00";
                using (frmEmrBaseRingerIOAct frm = new frmEmrBaseRingerIOAct(p, dtpOrderDate.Value.ToString("yyyyMMdd"), strOrderDate, 
                                                                            strSITEGB, strORDROWID, OrderNo, OrderCode, OrderName, ActSeq, mACTGB))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }

                GetOrderData();
            }
            else if (strPopMenuName == "유지")
            {
                mACTGB = "01";
                using (frmEmrBaseRingerIOAct frm = new frmEmrBaseRingerIOAct(p, dtpOrderDate.Value.ToString("yyyyMMdd"), strOrderDate, 
                                                                            strSITEGB, strORDROWID, OrderNo, OrderCode, OrderName, ActSeq, mACTGB))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }

                GetOrderData();
            }
            else if (strPopMenuName == "종료")
            {
                mACTGB = "02";
                using (frmEmrBaseRingerIOAct frm = new frmEmrBaseRingerIOAct(p, dtpOrderDate.Value.ToString("yyyyMMdd"), strOrderDate,
                                                                            strSITEGB, strORDROWID, OrderNo, OrderCode, OrderName, ActSeq, mACTGB))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }

                GetOrderData();
            }
            else if (strPopMenuName == "원타임")
            {
                mACTGB = "03";
                using (frmEmrBaseRingerIOAct frm = new frmEmrBaseRingerIOAct(p, dtpOrderDate.Value.ToString("yyyyMMdd"), strOrderDate,
                                                                            strSITEGB, strORDROWID, OrderNo, OrderCode, OrderName, ActSeq, mACTGB))
                {
                    frm.StartPosition = FormStartPosition.CenterParent;
                    frm.ShowDialog(this);
                }

                GetOrderData();
            }
            //else if (strPopMenuName == "주사 추가")
            //{
            //    frmEmrBaseRingerIOAct frm = new frmEmrBaseRingerIOAct(p, dtpOrderDate.Value.ToString("yyyyMMdd"), strOrderDate, OrderNo, OrderCode, OrderName, ActSeq, mACTGB);
            //    frm.StartPosition = FormStartPosition.CenterParent;
            //    frm.ShowDialog(this);

            //    GetOrderData();
            //}
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            if (SaveChart() == true)
            {
                ComFunc.MsgBoxEx(this, "수액기록을 저장하였습니다.");
                this.Close();
            }
            btnSave.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEmrBaseRingerIO_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (SaveChart() == false)
            //{
            //    e.Cancel = true;
            //}
        }

        private void frmEmrBaseRingerIO_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        #endregion //폼 이벤트

        #region //함수
        
        /// <summary>
        /// 처방조회
        /// </summary>
        private void GetOrderData()
        {
            ssView_Sheet1.RowCount = 0;

            if (p == null) return;
            if (VB.Val(p.acpNo) == 0) return;

            string strODate = string.Empty;

            int i = 0;
            DataTable dt = null;
            string SQL = string.Empty;    //Query문
            string SqlErr = string.Empty; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            strODate = dtpOrderDate.Value.ToString("yyyy-MM-dd");

            //string strBun = "'20','23' ";
            string strBun = "'20','23' ";

            #region Query

            #region //이전
            //SQL = " ";
            //SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(M.INDATE,'YYYY-MM-DD') INDATE, M.WARDCODE,   M.ROOMCODE,   M.PANO,       M.SNAME,      M.SEX,       ";
            //SQL = SQL + ComNum.VBLF + "        M.AGE,        M.DEPTCODE,   O.SLIPNO,     O.ORDERCODE, O.POWDER, O.GBER, O.GBPRN,  ";
            //SQL = SQL + ComNum.VBLF + "        C.ORDERNAME,  C.ORDERNAMES, C.DISPHEADER, O.CONTENTS,  O.DCDIV, O.GBSELF, O.GBNGT, ";
            //SQL = SQL + ComNum.VBLF + "        O.BCONTENTS,  O.REALQTY,    O.DOSCODE,    D.DOSNAME,   D.GBDIV, O.NURREMARK, ";
            //SQL = SQL + ComNum.VBLF + "        O.GBGROUP,    O.REMARK,     O.GBINFO,     O.GBTFLAG,   O.BUN,   O.ACTDIV, ";
            //SQL = SQL + ComNum.VBLF + "        O.GBPRN,      O.GBORDER,    O.ORDERSITE,  O.ORDERNO,   TO_CHAR(O.BDATE,'YYYY-MM-DD') BDATE , O.GBSTATUS,  ";
            //SQL = SQL + ComNum.VBLF + "        O.GBPORT,     N.DRNAME,     O.SUCODE,     O.QTY,  O.BUN ,O.NAL, O.ROWID, ";
            //SQL = SQL + ComNum.VBLF + "        S.UNITNEW1,   S.UNITNEW2,   S.UNITNEW3,   S.UNITNEW4, C.DISPRGB, C.ITEMCD, ";
            //SQL = SQL + ComNum.VBLF + "        TO_CHAR(O.PICKUPDATE,'YYYY-MM-DD HH24:MI') PICKUPDATE, S.SUNAMEK ,O.PICKUPSABUN, TO_CHAR(O.ENTDATE, 'YYYY-MM-DD') AS ENTDATE, ";
            //SQL = SQL + ComNum.VBLF + "        O.GBIOE, O.GBGROUP ";      //2019-02-14 응급실 NDC 용
            //SQL = SQL + ComNum.VBLF + "		, KOSMOS_OCS.FC_INSA_MST_KORNAME(LPAD(TRIM(O.PICKUPSABUN), 5, '0')) AS PICKUPNAME ";
            //SQL = SQL + ComNum.VBLF + "        , KOSMOS_OCS.FC_INSA_BUSE(LPAD(TRIM(O.PICKUPSABUN), 5, '0')) AS PICKUPBUSE ";
            //SQL = SQL + ComNum.VBLF + "        , KOSMOS_OCS.FC_READ_ATTENTION(O.SUCODE) AS ATTENTION ";
            //SQL = SQL + ComNum.VBLF + "        , KOSMOS_OCS.FC_READ_AST_REACTION(O.SUCODE, O.PTNO, TO_CHAR(M.INDATE,'YYYY-MM-DD'), KOSMOS_OCS.FC_GET_AGE2(O.PTNO, SYSDATE)) AS AST_ATTENTION ";
            //SQL = SQL + ComNum.VBLF + "        , KOSMOS_OCS.FC_OCS_DRUGINFO_SNAME(O.ORDERCODE) AS DRUGNAME ";
            //SQL = SQL + ComNum.VBLF + "        , KOSMOS_OCS.FC_OCS_OSPECIMAN_SPECNAME(O.SLIPNO, O.DOSCODE) AS SPECNAME ";
            //SQL = SQL + ComNum.VBLF + "        , KOSMOS_OCS.FC_OCS_ODOSAGE_NAME1(O.DOSCODE) AS DOSCODEYN ";
            //SQL = SQL + ComNum.VBLF + "		, (SELECT COUNT(*) CNT ";
            //SQL = SQL + ComNum.VBLF + "		             FROM KOSMOS_OCS.OCS_IORDER ";
            //SQL = SQL + ComNum.VBLF + "		             WHERE ORDERNO = O.ORDERNO ";
            //SQL = SQL + ComNum.VBLF + "		                  AND PTNO = O.PTNO ";
            //SQL = SQL + ComNum.VBLF + "		                  AND BDATE = O.BDATE ";
            //SQL = SQL + ComNum.VBLF + "		                  AND ORDERSITE   IN ('DC0','DC1','DC2') ";
            //SQL = SQL + ComNum.VBLF + "		                  AND DIVQTY IS NULL) AS IORDER_CNT1 ";
            //SQL = SQL + ComNum.VBLF + "        , ( SELECT  ";
            //SQL = SQL + ComNum.VBLF + "                MAX(AA1.ORDERNO) ";
            //SQL = SQL + ComNum.VBLF + "            FROM KOSMOS_OCS.OCS_IORDER AA1  ";
            //SQL = SQL + ComNum.VBLF + "            WHERE AA1.PTNO  = O.PTNO ";
            //SQL = SQL + ComNum.VBLF + "               AND AA1.BDATE  = (O.BDATE - 1) ";
            //SQL = SQL + ComNum.VBLF + "               AND AA1.GBSTATUS  IN  (' ','D+','D','D-')  ";
            //SQL = SQL + ComNum.VBLF + "               AND AA1.ORDERCODE = O.ORDERCODE ";
            //SQL = SQL + ComNum.VBLF + "               AND AA1.QTY = O.QTY ";
            //SQL = SQL + ComNum.VBLF + "               AND AA1.REALQTY = O.REALQTY ";
            //SQL = SQL + ComNum.VBLF + "               AND AA1.CONTENTS = O.CONTENTS ";
            //SQL = SQL + ComNum.VBLF + "               AND ( AA1.GBIOE NOT IN ('E','EI') OR AA1.GBIOE IS NULL) ) AS BEFORDAY ";
            #endregion

            #region //입원
            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT ";
            SQL = SQL + ComNum.VBLF + "     'IPD' AS SITEGB, ";
            SQL = SQL + ComNum.VBLF + "      O.ORDERNO,";
            SQL = SQL + ComNum.VBLF + "      O.ORDERCODE,";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(O.BDATE,'YYYY-MM-DD') AS BDATE,";
            SQL = SQL + ComNum.VBLF + "      S.SUNAMEK,";
            SQL = SQL + ComNum.VBLF + "      O.GBDIV,";
            SQL = SQL + ComNum.VBLF + "      O.GBSTATUS,";
            SQL = SQL + ComNum.VBLF + "      O.GBGROUP, ";
            SQL = SQL + ComNum.VBLF + "      O.ENTDATE, ";
            SQL = SQL + ComNum.VBLF + "      O.ROWID AS ORDROWID ";
            //SQL = SQL + ComNum.VBLF + "      , (SELECT SUM(NAL) FROM KOSMOS_OCS.OCS_IORDER WHERE ORDERNO = O.ORDERNO GROUP BY ORDERNO) AS SUMQTY";
            SQL = SQL + ComNum.VBLF + "      , CASE WHEN EXISTS(SELECT 1 FROM KOSMOS_OCS.OCS_IORDER WHERE ORDERNO = O.ORDERNO AND DIVQTY IS NULL AND ORDERSITE NOT IN ('CAN')) THEN 1 END IS_ORDER";
            SQL = SQL + ComNum.VBLF + "      , CASE WHEN EXISTS(SELECT 1 FROM KOSMOS_OCS.OCS_DOCTOR WHERE SABUN = O.DRCODE AND DEPTCODE = 'PC') THEN 1 END PC_BUSE";

            #region 그룹
            SQL = SQL + ComNum.VBLF + "      ,  CASE WHEN LENGTH(TRIM(GBGROUP)) > 0 THEN  ( ";
            SQL = SQL + ComNum.VBLF + "         SELECT ";
            SQL = SQL + ComNum.VBLF + "             LISTAGG(S.SUNAMEK , '\r\n') WITHIN GROUP(ORDER BY BDATE, GBGROUP, GBDIV, ENTDATE)  ";
            SQL = SQL + ComNum.VBLF + "         FROM KOSMOS_OCS.OCS_IORDER O2";
            SQL = SQL + ComNum.VBLF + "         INNER JOIN KOSMOS_OCS.OCS_ORDERCODE C ";
            SQL = SQL + ComNum.VBLF + "             ON O2.ORDERCODE = C.ORDERCODE ";
            SQL = SQL + ComNum.VBLF + "             AND O2.SLIPNO     =  C.SLIPNO   ";
            SQL = SQL + ComNum.VBLF + "             AND O2.ORDERCODE  =  C.ORDERCODE  ";
            SQL = SQL + ComNum.VBLF + "             AND (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
            SQL = SQL + ComNum.VBLF + "         INNER JOIN KOSMOS_PMPA.BAS_SUN S ";
            SQL = SQL + ComNum.VBLF + "             ON O2.SUCODE = S.SUNEXT ";
            SQL = SQL + ComNum.VBLF + "         WHERE O2.PTNO = O.PTNO";
            SQL = SQL + ComNum.VBLF + "             AND O2.BUN IN ( '20','23'  )  ";
            SQL = SQL + ComNum.VBLF + "             AND O2.BDATE = O.BDATE";
            SQL = SQL + ComNum.VBLF + "             AND (O2.GBPRN IN  NULL OR O2.GBPRN <> 'P')  ";
            SQL = SQL + ComNum.VBLF + "             AND (O2.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') OR O2.ORDERSITE IS NULL ) ";
            SQL = SQL + ComNum.VBLF + "             AND O2.GBPRN <>'S'  ";
            SQL = SQL + ComNum.VBLF + "             AND ( O2.GBSTATUS NOT IN ('D-','D+' )  OR  (  O2.GBSTATUS = 'D' AND O2.ACTDIV > 0 ) )   ";
            SQL = SQL + ComNum.VBLF + "             AND O2.GBPICKUP = '*'  ";
            SQL = SQL + ComNum.VBLF + "             AND ( O2.VERBC IS NULL OR O2.VERBC <>'Y' ) ";
            SQL = SQL + ComNum.VBLF + "             AND ( O2.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL) ";
            SQL = SQL + ComNum.VBLF + "             AND O2.QTY  <>  0   ";
            SQL = SQL + ComNum.VBLF + "             AND O2.ORDERNO <> O.ORDERNO";
            SQL = SQL + ComNum.VBLF + "         	AND O2.GBGROUP = O.GBGROUP";
            SQL = SQL + ComNum.VBLF + "       ) END NOTE";
            #endregion

            SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER O, ";
            SQL = SQL + ComNum.VBLF + " (SELECT ACTDATE, INDATE, OUTDATE, GBSTS, WARDCODE, ROOMCODE, PANO, SNAME, SEX, AGE, DEPTCODE, IPDNO ";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
            SQL = SQL + ComNum.VBLF + "  WHERE IPDNO = (SELECT ";
            SQL = SQL + ComNum.VBLF + "                     MAX(IPDNO) ";
            SQL = SQL + ComNum.VBLF + "                  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
            SQL = SQL + ComNum.VBLF + "                  WHERE(ACTDATE IS NULL OR OUTDATE = TRUNC(SYSDATE))   ";
            SQL = SQL + ComNum.VBLF + " 		         AND PANO = '" + p.ptNo + "')) M,                 ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P,                           ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_ORDERCODE           C,                           ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_ODOSAGE             D,                           ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_DOCTOR              N,                            ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_SUN     S   ,";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_ADM.DRUG_MASTER2 F   ,";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_ADM.DRUG_MASTER1 F2   ";
            SQL = SQL + ComNum.VBLF + "  WHERE PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "   AND O.BUN IN ( " + strBun + " ) ";
            SQL = SQL + ComNum.VBLF + "   AND O.BDATE >= TO_DATE('" + dtpOrderDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') - 1";
            SQL = SQL + ComNum.VBLF + "   AND O.BDATE <= TO_DATE('" + dtpOrderDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD')";
            SQL = SQL + ComNum.VBLF + "   AND  (O.GBPRN IN  NULL OR O.GBPRN <> 'P') ";
            if (p.ward == "ER")
            {
                SQL = SQL + ComNum.VBLF + " AND  (  O.ORDERSITE  IN ('ERO')   OR  O.GBIOE IN ('E','EI') )";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " AND  (O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') OR O.ORDERSITE IS NULL )";
            }
            SQL = SQL + ComNum.VBLF + "  AND    O.GBPRN <>'S' "; //'JJY 추가(2000/05/22 'S는 선수납(선불);
            SQL = SQL + ComNum.VBLF + "  AND   ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND   ACTDIV > 0 ) )  ";
            SQL = SQL + ComNum.VBLF + "  AND    O.PTNO       =  M.PANO           ";
            SQL = SQL + ComNum.VBLF + "  AND  O.GBPICKUP = '*' ";
            SQL = SQL + ComNum.VBLF + "  AND  ( O.VERBC IS NULL OR O.VERBC <>'Y' )";
            if (p.ward == "HD")
            {
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else if (p.ward == "ENDO")
            {
                SQL = SQL + ComNum.VBLF + " AND O.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE WARDCODE IN ( 'EN') ) ";
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else if (p.ward == "CT/MRI")
            {
                SQL = SQL + ComNum.VBLF + " AND O.DOSCODE IN ( SELECT DOSCODE FROM KOSMOS_OCS.OCS_ODOSAGE WHERE WARDCODE IN ( 'RD') ) ";
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else if (p.ward == "OP" || p.ward == "AG")
            {
                //    '수술방은 모든 오더 보이도록 처리 추후 보완 예정;
            }
            else if (p.ward == "RA")
            {
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else if (p.ward == "MICU")
            {
                SQL = SQL + ComNum.VBLF + " AND M.WARDCODE ='IU'";
                SQL = SQL + ComNum.VBLF + " AND M.ROOMCODE ='234'";
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + " AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL)";
                if (p.ward == "SICU")
                {
                    SQL = SQL + ComNum.VBLF + " AND M.WARDCODE ='IU'   ";
                    SQL = SQL + ComNum.VBLF + " AND M.ROOMCODE ='233'";
                }
                else if (p.ward != "ER")
                {
                    if (p.ward == "IQ" || p.ward == "ND" || p.ward == "NR")
                    {
                        SQL = SQL + ComNum.VBLF + " AND  M.WARDCODE IN ('IQ','ND','NR')";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + " AND  M.WARDCODE = '" + p.ward.Trim() + "' ";
                    }
                }
            }
            SQL = SQL + ComNum.VBLF + "  AND    O.QTY  <>  '0'    ";
            SQL = SQL + ComNum.VBLF + "  AND    M.GBSTS NOT IN  ('9') "; //" '입원취소 제외;
            SQL = SQL + ComNum.VBLF + "  AND    O.PTNO       =  P.PANO(+)        ";
            SQL = SQL + ComNum.VBLF + "  AND    O.SLIPNO     =  C.SLIPNO(+)      ";
            SQL = SQL + ComNum.VBLF + "  AND    O.ORDERCODE  =  C.ORDERCODE(+)   ";
            SQL = SQL + ComNum.VBLF + "  AND    (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
            SQL = SQL + ComNum.VBLF + "  AND    O.DOSCODE    =  D.DOSCODE(+)     ";
            SQL = SQL + ComNum.VBLF + "  AND    O.DRCODE      =  N.SABUN(+)      ";
            SQL = SQL + ComNum.VBLF + "  AND    O.SUCODE = S.SUNEXT(+) ";
            SQL = SQL + ComNum.VBLF + "  AND    O.SUCODE = F.JEPCODE ";
            SQL = SQL + ComNum.VBLF + "  AND    F.SUGABUN = '20'  ";
            //SQL = SQL + ComNum.VBLF + "  AND F.JEHYENGBUN = '02' ";
            SQL = SQL + ComNum.VBLF + "  AND    F.JEPCODE = F2.JEPCODE ";
            SQL = SQL + ComNum.VBLF + "  AND    (F2.GBIO = 'Y' AND F2.POJANG2 = 'ml' OR (F2.GBIO = 'Y' AND F2.HAMYANG2 = 'ml'))";

            #endregion //입원

            SQL = SQL + ComNum.VBLF + " UNION ALL ";

            #region //응급실
            SQL = SQL + ComNum.VBLF + " SELECT ";
            SQL = SQL + ComNum.VBLF + "     'ERD' AS SITEGB, ";
            SQL = SQL + ComNum.VBLF + "      O.ORDERNO,";
            SQL = SQL + ComNum.VBLF + "      O.ORDERCODE,";
            SQL = SQL + ComNum.VBLF + "      TO_CHAR(O.BDATE,'YYYY-MM-DD') AS BDATE,";
            SQL = SQL + ComNum.VBLF + "      S.SUNAMEK,";
            SQL = SQL + ComNum.VBLF + "      O.GBDIV,";
            SQL = SQL + ComNum.VBLF + "      O.GBSTATUS,";
            SQL = SQL + ComNum.VBLF + "      O.GBGROUP, ";
            SQL = SQL + ComNum.VBLF + "      O.ENTDATE, ";
            SQL = SQL + ComNum.VBLF + "      O.ROWID AS ORDROWID ";
            //SQL = SQL + ComNum.VBLF + "      , (SELECT SUM(NAL) FROM KOSMOS_OCS.OCS_IORDER WHERE ORDERNO = O.ORDERNO GROUP BY ORDERNO) AS SUMQTY";
            SQL = SQL + ComNum.VBLF + "      , CASE WHEN EXISTS(SELECT 1 FROM KOSMOS_OCS.OCS_IORDER WHERE ORDERNO = O.ORDERNO AND DIVQTY IS NULL AND ORDERSITE NOT IN ('CAN')) THEN 1 END IS_ORDER";
            SQL = SQL + ComNum.VBLF + "      , NULL AS PC_BUSE";
            #region 그룹
            SQL = SQL + ComNum.VBLF + "      ,  CASE WHEN LENGTH(TRIM(O.GBGROUP)) > 0 THEN  ( ";
            SQL = SQL + ComNum.VBLF + "         SELECT ";
            SQL = SQL + ComNum.VBLF + "             LISTAGG(S.SUNAMEK , '\r\n') WITHIN GROUP(ORDER BY BDATE, GBGROUP, GBDIV, ENTDATE)  ";
            SQL = SQL + ComNum.VBLF + "         FROM KOSMOS_OCS.OCS_IORDER O2";
            SQL = SQL + ComNum.VBLF + "         INNER JOIN KOSMOS_OCS.OCS_ORDERCODE C ";
            SQL = SQL + ComNum.VBLF + "             ON O2.ORDERCODE = C.ORDERCODE ";
            SQL = SQL + ComNum.VBLF + "             AND O2.SLIPNO     =  C.SLIPNO   ";
            SQL = SQL + ComNum.VBLF + "             AND O2.ORDERCODE  =  C.ORDERCODE  ";
            SQL = SQL + ComNum.VBLF + "             AND (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
            SQL = SQL + ComNum.VBLF + "         INNER JOIN KOSMOS_PMPA.BAS_SUN S ";
            SQL = SQL + ComNum.VBLF + "             ON O2.SUCODE = S.SUNEXT ";
            SQL = SQL + ComNum.VBLF + "         WHERE O2.PTNO = O.PTNO";
            SQL = SQL + ComNum.VBLF + "             AND O2.BUN IN ( '20','23'  )  ";
            SQL = SQL + ComNum.VBLF + "             AND O2.BDATE = O.BDATE";
            SQL = SQL + ComNum.VBLF + "             AND (O2.GBPRN IN  NULL OR O2.GBPRN <> 'P')  ";
            SQL = SQL + ComNum.VBLF + "             AND (O2.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') OR O2.ORDERSITE IS NULL ) ";
            SQL = SQL + ComNum.VBLF + "             AND O2.GBPRN <>'S'  ";
            SQL = SQL + ComNum.VBLF + "             AND ( O2.GBSTATUS NOT IN ('D-','D+' )  OR  (  O2.GBSTATUS = 'D' AND O2.ACTDIV > 0 ) )   ";
            SQL = SQL + ComNum.VBLF + "             AND O2.GBPICKUP = '*'  ";
            SQL = SQL + ComNum.VBLF + "             AND ( O2.VERBC IS NULL OR O2.VERBC <>'Y' ) ";
            SQL = SQL + ComNum.VBLF + "             AND ( O2.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL) ";
            SQL = SQL + ComNum.VBLF + "             AND O2.QTY  <>  0   ";
            SQL = SQL + ComNum.VBLF + "             AND O2.ORDERNO <> O.ORDERNO";
            SQL = SQL + ComNum.VBLF + "         	AND O2.GBGROUP = O.GBGROUP";
            SQL = SQL + ComNum.VBLF + "       ) END NOTE";
            #endregion
            SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.OCS_IORDER O, ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.OPD_MASTER  M,                           ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_PATIENT P,                           ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_ORDERCODE           C,                           ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_ODOSAGE             D,                           ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_OCS.OCS_DOCTOR              N,                            ";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.BAS_SUN     S  ,";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_ADM.DRUG_MASTER2 F  ,";
            SQL = SQL + ComNum.VBLF + "        KOSMOS_ADM.DRUG_MASTER1 F2   ";
            SQL = SQL + ComNum.VBLF + " WHERE  O.PTNO = '" + p.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "    AND  O.BUN IN ( " + strBun + " ) ";
            SQL = SQL + ComNum.VBLF + "    AND  (O.GBPRN IN  NULL OR O.GBPRN <> 'P') ";
            SQL = SQL + ComNum.VBLF + "    AND  O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') ";
            SQL = SQL + ComNum.VBLF + "    AND  (  O.ORDERSITE  IN ('ERO')   OR  O.GBIOE IN ('E','EI') )";
            SQL = SQL + ComNum.VBLF + "    AND   O.BDATE >= TO_DATE('" + dtpOrderDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') - 1 ";
            SQL = SQL + ComNum.VBLF + "    AND   O.BDATE <= TO_DATE('" + dtpOrderDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "    AND    O.GBPRN <>'S' ";  //'JJY 추가(2000/05/22 'S는 선수납(선불);
            SQL = SQL + ComNum.VBLF + "    AND   ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND   ACTDIV > 0) )  ";
            SQL = SQL + ComNum.VBLF + "    AND    O.PTNO       =  M.PANO           ";
            SQL = SQL + ComNum.VBLF + "    AND   O.QTY  <>  '0'    ";
            SQL = SQL + ComNum.VBLF + "    AND  M.ACTDATE = TO_DATE('" + p.medFrDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "    AND  M.DEPTCODE = 'ER'";
            SQL = SQL + ComNum.VBLF + "    AND  O.GBTFLAG <> 'T'";        //'2010-04-27     양수령수간호사 퇴원약 제외해달라고 함;
            SQL = SQL + ComNum.VBLF + "    AND  O.PTNO       =  P.PANO(+)        ";
            SQL = SQL + ComNum.VBLF + "    AND  O.SLIPNO     =  C.SLIPNO(+)      ";
            SQL = SQL + ComNum.VBLF + "    AND  O.ORDERCODE  =  C.ORDERCODE(+)   ";
            SQL = SQL + ComNum.VBLF + "    AND  (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
            SQL = SQL + ComNum.VBLF + "    AND  O.DOSCODE    =  D.DOSCODE(+)     ";
            SQL = SQL + ComNum.VBLF + "    AND  O.DRCODE      =  N.SABUN(+)      ";
            SQL = SQL + ComNum.VBLF + "    AND  O.SUCODE = S.SUNEXT(+) ";
            SQL = SQL + ComNum.VBLF + "    AND  O.SUCODE = F.JEPCODE ";
            SQL = SQL + ComNum.VBLF + "    AND  F.SUGABUN = '20'  ";
            //SQL = SQL + ComNum.VBLF + "    AND  F.JEHYENGBUN = '02' ";
            SQL = SQL + ComNum.VBLF + "    AND  F.JEPCODE = F2.JEPCODE ";
            SQL = SQL + ComNum.VBLF + "    AND  (F2.GBIO = 'Y' AND F2.POJANG2 = 'ml' OR (F2.GBIO = 'Y' AND F2.HAMYANG2 = 'ml'))";
            #endregion //응급실

            //SQL = SQL + ComNum.VBLF + " UNION ALL ";

            #region //수술실
            //SQL = SQL + ComNum.VBLF + "SELECT ";
            //SQL = SQL + ComNum.VBLF + "    'OPR' AS SITEGB, ";
            //SQL = SQL + ComNum.VBLF + "    0 AS ORDERNO, ";
            //SQL = SQL + ComNum.VBLF + "    O.JEPCODE AS ORDERCODE, ";
            //SQL = SQL + ComNum.VBLF + "    TO_CHAR(O.OPDATE,'YYYY-MM-DD') AS BDATE , ";
            //SQL = SQL + ComNum.VBLF + "    J.NAME AS SUNAMEK,  ";
            //SQL = SQL + ComNum.VBLF + "    O.QTY AS GBDIV, ";
            //SQL = SQL + ComNum.VBLF + "    '' AS GBSTATUS, ";
            //SQL = SQL + ComNum.VBLF + "    '' AS GBGROUP, ";
            //SQL = SQL + ComNum.VBLF + "    O.ENTDATE, ";
            //SQL = SQL + ComNum.VBLF + "    O.ROWID AS ORDROWID ";
            //SQL = SQL + ComNum.VBLF + "    --C.UNIT,   ";
            //SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.ORAN_SLIP O ";
            //SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.OPR_BUSEJEPUM J ";
            //SQL = SQL + ComNum.VBLF + "    ON O.JEPCODE = J.JEPCODE ";
            //SQL = SQL + ComNum.VBLF + "    AND J.BUCODE = '033103'  ";
            //SQL = SQL + ComNum.VBLF + "    AND J.BUN = '04' ";
            //SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.BAS_SUT B    ";
            //SQL = SQL + ComNum.VBLF + "   ON J.JEPCODE = B.SUCODE    ";
            //SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_PMPA.BAS_SUN C    ";
            //SQL = SQL + ComNum.VBLF + "   ON B.SUNEXT = C.SUNEXT     ";
            //SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN KOSMOS_ADM.ORD_JEP D    ";
            //SQL = SQL + ComNum.VBLF + "   ON J.JEPCODE = D.JEPCODE  ";
            //SQL = SQL + ComNum.VBLF + "WHERE O.PANO = '" + p.ptNo + "' ";
            //SQL = SQL + ComNum.VBLF + "    AND   O.OPDATE >= TO_DATE('" + dtpOrderDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') - 1 ";
            //SQL = SQL + ComNum.VBLF + "    AND   O.OPDATE <= TO_DATE('" + dtpOrderDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
            #endregion //수술실

            SQL = SQL + ComNum.VBLF + " ORDER BY BDATE, SITEGB, GBGROUP, GBDIV, ENTDATE     ";
            
            #endregion Query

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

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

            for (i = 0; i < dt.Rows.Count; i++)
            {
                string pstrOrderDate = dt.Rows[i]["BDATE"].ToString().Replace("-", "").Trim();
                double pOrderNo = VB.Val(dt.Rows[i]["ORDERNO"].ToString().Trim());
                string pstrOrderCode = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                string pstrGbGroup = dt.Rows[i]["GBGROUP"].ToString().Trim();

                ssView_Sheet1.RowCount += 1;
                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = ""; //ACTSEQ
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["SITEGB"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["ORDROWID"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["BDATE"].ToString().Trim();
                if (dt.Rows[i]["SITEGB"].ToString().Trim() == "OPR")
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "[" + "수술" + "] " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                }
                else if (dt.Rows[i]["SITEGB"].ToString().Trim() == "ERD")
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "[" + "응급실" + "] " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                }
                else
                {
                    if (dt.Rows[i]["PC_BUSE"].ToString().Equals("1"))
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "[" + "마취과" + "] " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    }
                    else
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "[" + "병동" + "] " + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                    }
                }
                
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["GBDIV"].ToString().Trim();


                if (dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D" || dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D-" ||
                    dt.Rows[i]["IS_ORDER"].ToString().IsNullOrEmpty())                   
                    //dt.Rows[i]["SUMQTY"].To<int>() == 0) //DC표기
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = "(D/C)" + ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text.Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.Columns.Count - 1].ForeColor = Color.Red;
                }
                //if (VB.Val(dt.Rows[i]["REALQTY"].ToString().Trim()) < 0)
                //{
                //    ssView_Sheet1.Cells[i, 3].Text = "0";
                //}
                //else
                //{
                //    ssView_Sheet1.Cells[i, 3].Text = VB.Val(dt.Rows[i]["REALQTY"].ToString().Trim()).ToString();
                //}
                //ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["GBDIV"].ToString().Trim();

                //if (dt.Rows[i]["SITEGB"].ToString().Trim() != "OPR" && pstrGbGroup.Length > 0)
                //{                   
                //    GetDateMixInfoEx(ssView_Sheet1.RowCount - 1, pstrOrderDate, pOrderNo, pstrOrderCode, pstrGbGroup);
                //}

                if (dt.Rows[i]["NOTE"].ToString().Length > 0)
                {
                    //GetDateMixInfoEx(ssView_Sheet1.RowCount - 1, pstrOrderDate, pOrderNo, pstrOrderCode, pstrGbGroup);

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].NoteStyle = FarPoint.Win.Spread.NoteStyle.PopupStickyNote;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].NoteIndicatorPosition = FarPoint.Win.Spread.NoteIndicatorPosition.TopRight;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].NoteIndicatorColor = Color.Pink;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].NoteIndicatorSize = new Size(20, 20);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Note = dt.Rows[i]["NOTE"].ToString().Trim();
                    FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo nsinfo = new FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo();
                    nsinfo = ssView_Sheet1.GetStickyNoteStyleInfo(ssView_Sheet1.RowCount - 1, 6);
                    //nsinfo.BackColor = Color.Red;
                    nsinfo.Font = new Font("굴림", 10); 
                    nsinfo.ForeColor = Color.Black;
                    nsinfo.Width = 300; //가장 긴 텍스트 길이에 맞춰서 너비 설정
                    nsinfo.ShapeOutlineColor = Color.Red;
                    nsinfo.ShapeOutlineThickness = 1;
                    nsinfo.ShadowOffsetX = 3;
                    nsinfo.ShadowOffsetY = 3;
                    ssView_Sheet1.SetStickyNoteStyleInfo(ssView_Sheet1.RowCount - 1, 6, nsinfo);
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, 44);
                }

                GetDateActInfo(ssView_Sheet1.RowCount - 1, pstrOrderDate, pOrderNo, pstrOrderCode);

                ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, 44);
            }

            dt.Dispose();
            dt = null;

            GetOrderDataSpen();

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// 스프래드를 병합한다
        /// </summary>
        private void GetOrderDataSpen()
        {
            //string ppSITEGB = "";
            string pOrderNo = "";
            string pstrOrderDate = "";

            int intOrderNo = 0;
            int intOrderDate = 0;

            if (ssView_Sheet1.RowCount < 0) return;

            LineBorder lineBorder = new LineBorder(System.Drawing.Color.Black, 2, false, false, false, true);


            for (int i = 0; i < ssView_Sheet1.RowCount; i++)
            {
                if (pstrOrderDate != ssView_Sheet1.Cells[i, 5].Text.Trim())
                {
                    pstrOrderDate = ssView_Sheet1.Cells[i, 5].Text.Trim();

                    if (intOrderDate != 0)
                    {
                        if (i - intOrderDate - 1 < 0)
                        {
                            ssView_Sheet1.Cells[0, 5].RowSpan = intOrderDate;
                            ssView_Sheet1.Cells[0, 5].Border = lineBorder;
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i - intOrderDate - 1, 5].RowSpan = intOrderDate + 1;
                            ssView_Sheet1.Cells[i - intOrderDate - 1, 5].Border = lineBorder;
                            ssView_Sheet1.Rows[intOrderDate].Border = lineBorder;
                        }
                    }

                    pOrderNo = ssView_Sheet1.Cells[i, 3].Text.Trim();

                    if (intOrderNo != 0)
                    {
                        if (i - intOrderNo - 1 < 0)
                        {
                            ssView_Sheet1.Cells[0, 3].RowSpan = intOrderNo;
                            ssView_Sheet1.Cells[0, 4].RowSpan = intOrderNo;
                            ssView_Sheet1.Cells[0, 6].RowSpan = intOrderNo;
                            ssView_Sheet1.Cells[0, 7].RowSpan = intOrderNo;
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 3].RowSpan = intOrderNo + 1;
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 4].RowSpan = intOrderNo + 1;
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 6].RowSpan = intOrderNo + 1;
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 7].RowSpan = intOrderNo + 1;

                        }
                    }

                    intOrderNo = 0;
                    intOrderDate = 0;
                }
                else
                {
                    if (i == ssView_Sheet1.RowCount - 1)
                    {
                        ssView_Sheet1.Cells[i - intOrderDate - 1, 5].RowSpan = intOrderDate + 2;
                    }

                    intOrderDate = intOrderDate + 1;

                    if (pOrderNo != ssView_Sheet1.Cells[i, 3].Text.Trim())
                    {
                        pOrderNo = ssView_Sheet1.Cells[i, 3].Text.Trim();

                        if (intOrderNo != 0)
                        {
                            if (i - intOrderNo - 1 < 0)
                            {
                                ssView_Sheet1.Cells[0, 3].RowSpan = intOrderNo;
                                ssView_Sheet1.Cells[0, 4].RowSpan = intOrderNo;
                                ssView_Sheet1.Cells[0, 6].RowSpan = intOrderNo;
                                ssView_Sheet1.Cells[0, 7].RowSpan = intOrderNo;
                            }
                            else
                            {
                                ssView_Sheet1.Cells[i - intOrderNo - 1, 3].RowSpan = intOrderNo + 1;
                                ssView_Sheet1.Cells[i - intOrderNo - 1, 4].RowSpan = intOrderNo + 1;
                                ssView_Sheet1.Cells[i - intOrderNo - 1, 6].RowSpan = intOrderNo + 1;
                                ssView_Sheet1.Cells[i - intOrderNo - 1, 7].RowSpan = intOrderNo + 1;
                            }
                        }

                        intOrderNo = 0;
                    }
                    else
                    {
                        if (i == ssView_Sheet1.RowCount - 1)
                        {
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 3].RowSpan = intOrderNo + 2;
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 4].RowSpan = intOrderNo + 2;
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 6].RowSpan = intOrderNo + 2;
                            ssView_Sheet1.Cells[i - intOrderNo - 1, 7].RowSpan = intOrderNo + 2;
                        }
                        intOrderNo = intOrderNo + 1;
                    }
                }
            }
        }

        /// <summary>
        /// 수액 믹스 정보 표시 : 처방정보와 사용자가 믹스 한것을 표시한다.
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="pstrOrderDate"></param>
        /// <param name="pOrderNo"></param>
        /// <param name="pstrOrderCode"></param>
        /// <param name="pstrGbGroup"></param>
        private void GetDateMixInfoEx(int Row, string pstrOrderDate, double pOrderNo, string pstrOrderCode, string pstrGbGroup)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    O.ORDERCODE,  ";
            SQL = SQL + ComNum.VBLF + "    C.ORDERNAME, C.ORDERNAMES, ";
            SQL = SQL + ComNum.VBLF + "    S.SUNAMEK  ";
            SQL = SQL + ComNum.VBLF + "FROM KOSMOS_OCS.OCS_IORDER O ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_OCS.OCS_ORDERCODE C ";
            SQL = SQL + ComNum.VBLF + "    ON O.ORDERCODE = C.ORDERCODE ";
            SQL = SQL + ComNum.VBLF + "    AND O.SLIPNO     =  C.SLIPNO   ";
            SQL = SQL + ComNum.VBLF + "    AND O.ORDERCODE  =  C.ORDERCODE  ";
            SQL = SQL + ComNum.VBLF + "    AND (C.SENDDEPT  !=  'N' OR C.SENDDEPT IS NULL)   ";
            SQL = SQL + ComNum.VBLF + "INNER JOIN KOSMOS_PMPA.BAS_SUN S ";
            SQL = SQL + ComNum.VBLF + "    ON O.SUCODE = S.SUNEXT ";
            SQL = SQL + ComNum.VBLF + "WHERE O.PTNO = '" + p.ptNo + "' ";
            SQL = SQL + ComNum.VBLF + "    AND O.BUN IN ( '20','23'  )  ";
            SQL = SQL + ComNum.VBLF + "    AND O.BDATE = TO_DATE('" + pstrOrderDate + "','YYYY-MM-DD') ";
            SQL = SQL + ComNum.VBLF + "    AND (O.GBPRN IN  NULL OR O.GBPRN <> 'P')  ";
            SQL = SQL + ComNum.VBLF + "    AND (O.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') OR O.ORDERSITE IS NULL ) ";
            SQL = SQL + ComNum.VBLF + "    AND O.GBPRN <>'S'  ";
            SQL = SQL + ComNum.VBLF + "    AND ( O.GBSTATUS NOT IN ('D-','D+' )  OR  (  O.GBSTATUS = 'D' AND O.ACTDIV > 0 ) )   ";
            SQL = SQL + ComNum.VBLF + "    AND O.GBPICKUP = '*'  ";
            SQL = SQL + ComNum.VBLF + "    AND ( O.VERBC IS NULL OR O.VERBC <>'Y' ) ";
            SQL = SQL + ComNum.VBLF + "    AND ( O.GBIOE NOT IN ('E','EI') OR GBIOE IS NULL) ";
            SQL = SQL + ComNum.VBLF + "    AND O.QTY  <>  0   ";
            SQL = SQL + ComNum.VBLF + "    AND O.ORDERNO <> " + pOrderNo;
            SQL = SQL + ComNum.VBLF + "	   AND O.GBGROUP = '" + pstrGbGroup + "'";
            //SQL = SQL + ComNum.VBLF + "    AND O.GBGROUP IS NOT NULL ";
            //SQL = SQL + ComNum.VBLF + "	   AND O.GBGROUP = (SELECT MAX(O1.GBGROUP)  ";
            //SQL = SQL + ComNum.VBLF + "                        FROM KOSMOS_OCS.OCS_IORDER O1 ";
            //SQL = SQL + ComNum.VBLF + "                        WHERE O1.PTNO = '" + p.ptNo + "' ";
            //SQL = SQL + ComNum.VBLF + "                            AND O1.BDATE = TO_DATE('" + pstrOrderDate + "','YYYY-MM-DD') ";
            //SQL = SQL + ComNum.VBLF + "                            AND O1.ORDERNO = " + pOrderNo;
            //SQL = SQL + ComNum.VBLF + "    					       AND (O1.GBPRN IN  NULL OR O1.GBPRN <> 'P')  ";
            //SQL = SQL + ComNum.VBLF + "    					       AND (O1.ORDERSITE  NOT IN ( 'CAN','DC0','DC1','DC2','OPDX') OR O1.ORDERSITE IS NULL ) ";
            //SQL = SQL + ComNum.VBLF + "    					       AND O1.GBPRN <>'S'  ";
            //SQL = SQL + ComNum.VBLF + "    					       AND ( O1.GBSTATUS NOT IN ('D-','D+' )  OR  (  O1.GBSTATUS = 'D' AND   O1.ACTDIV >'0' ) )   ";
            //SQL = SQL + ComNum.VBLF + "    					       AND O1.GBPICKUP = '*'  ";
            //SQL = SQL + ComNum.VBLF + "    					       AND ( O1.VERBC IS NULL OR O1.VERBC <>'Y' ) ";
            //SQL = SQL + ComNum.VBLF + "    					       AND ( O1.GBIOE NOT IN ('E','EI') OR O1.GBIOE IS NULL) ";
            //SQL = SQL + ComNum.VBLF + "    					       AND O1.QTY  <>  '0'  ) ";

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            string strOrderName = ssView_Sheet1.Cells[Row, 6].Text.Trim();
            Font font = new Font("굴림", 10);
            Size TxtSize = TextRenderer.MeasureText(strOrderName, font);
            List<int> lstWidth = new List<int>();
            lstWidth.Add(TxtSize.Width);

            StringBuilder strNote = new StringBuilder();
            //strNote.AppendLine(strOrderName);

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strNote.AppendLine(dt.Rows[i]["SUNAMEK"].ToString().Trim());
                //텍스트길이 계산
                TxtSize = TextRenderer.MeasureText(dt.Rows[i]["SUNAMEK"].ToString().Trim(), font);
                //List에 넣기
                lstWidth.Add(TxtSize.Width);

                //ssView_Sheet1.Cells[Row, 4].Text = strOrderName + ComNum.VBLF + dt.Rows[i]["SUNAMEK"].ToString().Trim();
                //strOrderName = ssView_Sheet1.Cells[Row, 4].Text.Trim();
            }

            ssView_Sheet1.Cells[Row, 6].NoteStyle = FarPoint.Win.Spread.NoteStyle.PopupStickyNote;
            ssView_Sheet1.Cells[Row, 6].NoteIndicatorPosition = FarPoint.Win.Spread.NoteIndicatorPosition.TopRight;
            ssView_Sheet1.Cells[Row, 6].NoteIndicatorColor = Color.Pink;
            ssView_Sheet1.Cells[Row, 6].NoteIndicatorSize = new Size(20, 20);
            ssView_Sheet1.Cells[Row, 6].Note = strNote.ToString().Trim();
            FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo nsinfo = new FarPoint.Win.Spread.DrawingSpace.StickyNoteStyleInfo();
            nsinfo = ssView_Sheet1.GetStickyNoteStyleInfo(Row, 6);
            //nsinfo.BackColor = Color.Red;
            nsinfo.Font = font;
            nsinfo.ForeColor = Color.Black;
            nsinfo.Width = lstWidth.Max(); //가장 긴 텍스트 길이에 맞춰서 너비 설정
            nsinfo.ShapeOutlineColor = Color.Red;
            nsinfo.ShapeOutlineThickness = 1;
            nsinfo.ShadowOffsetX = 3;
            nsinfo.ShadowOffsetY = 3;
            ssView_Sheet1.SetStickyNoteStyleInfo(Row, 6, nsinfo);

            dt.Dispose();
            dt = null;

            ssView_Sheet1.SetRowHeight(Row, 44);

        }

        /// <summary>
        /// 수액 액팅정보 표시
        /// </summary>
        /// <param name="Row"></param>
        /// <param name="pstrOrderDate"></param>
        /// <param name="pOrderNo"></param>
        /// <param name="pstrOrderCode"></param>
        private void GetDateActInfo(int Row, string pstrOrderDate, double pOrderNo, string pstrOrderCode)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            SQL = " ";
            SQL = SQL + ComNum.VBLF + "SELECT";
            SQL = SQL + ComNum.VBLF + "    F.ACTSEQ, F.ACTGB, F.ACTQTY, F.ACTRMK, ";
            SQL = SQL + ComNum.VBLF + "    F.ACTDATE, F.ACTTIME, F.ACTUSEID, ";
            SQL = SQL + ComNum.VBLF + "    U.USENAME  || '(' || D.NAME || ')' AS USENAME";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUID F";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_EMR + "AVIEWEMRUSER U";
            SQL = SQL + ComNum.VBLF + "     ON F.ACTUSEID = U.USEID";
            SQL = SQL + ComNum.VBLF + "LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_BUSE D";
            SQL = SQL + ComNum.VBLF + "     ON TRIM(U.BUSECODE) = TRIM(D.BUCODE)";
            //            SQL = SQL + ComNum.VBLF + "WHERE F.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "WHERE F.PTNO = '" + p.ptNo + "'";
            SQL = SQL + ComNum.VBLF + "     AND F.ORDDATE = '" + pstrOrderDate + "'";
            SQL = SQL + ComNum.VBLF + "     AND F.ORDERCODE = '" + pstrOrderCode + "'";
            SQL = SQL + ComNum.VBLF + "     AND F.ORDNO = " + pOrderNo ;
            SQL = SQL + ComNum.VBLF + "     AND F.DCCLS = '0' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY F.ACTGB, (F.ACTDATE || F.ACTTIME) "; ;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                if ( i > 0)
                {
                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;
                    ssView_Sheet1.SetRowHeight(ssView_Sheet1.RowCount - 1, ComNum.SPDROWHT);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 1].Text;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 2].Text;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 3].Text;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 4].Text;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 5].Text;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 6].Text;
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 2, 7].Text;
                }

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ACTSEQ"].ToString().Trim();

                if (dt.Rows[i]["ACTGB"].ToString().Trim() == "00")
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "시작";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].BackColor = System.Drawing.Color.LightGreen;
                }
                else if (dt.Rows[i]["ACTGB"].ToString().Trim() == "01")
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "유지";
                }
                else if (dt.Rows[i]["ACTGB"].ToString().Trim() == "02")
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "종료";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].BackColor = System.Drawing.Color.LightPink;
                }
                else if (dt.Rows[i]["ACTGB"].ToString().Trim() == "03")
                {
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "원타임";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].BackColor = System.Drawing.Color.LightPink;
                }

                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = ComFunc.FormatStrToDate(dt.Rows[i]["ACTDATE"].ToString().Trim(), "D") + " " + ComFunc.FormatStrToDate(dt.Rows[i]["ACTTIME"].ToString().Trim(), "M");
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["ACTQTY"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["USENAME"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["ACTRMK"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["ACTUSEID"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 14].Text = dt.Rows[i]["ACTDATE"].ToString().Trim();
                ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 15].Text = dt.Rows[i]["ACTTIME"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0) return;

            string mACTSTR = "";

            mACTSTR = ssView_Sheet1.Cells[e.Row, 8].Text.Trim();

            if (e.Button == MouseButtons.Right)
            {
                if (e.Column == 8)
                {
                    if (mACTSTR == "")
                    {
                        SetActPop(e.Row);
                    }
                    else
                    {
                        SetAct(e.Row);
                    }
                }
                else
                {
                    SetActPop(e.Row);
                }
            }
            //else
            //{
            //    if (e.Column == 6)
            //    {
            //        if (mACTSTR == "")
            //        {
            //            SetActPop(e.Row);
            //        }
            //        else
            //        {
            //            SetAct(e.Row);
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 액팅관련 기본코드 조회
        /// </summary>
        /// <param name="Row"></param>
        /// <returns></returns>
        private string GetActGbInfo(int Row)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strActGb = "";

            string strSITEGB = ssView_Sheet1.Cells[Row, 1].Text.Trim();
            string strORDROWID = ssView_Sheet1.Cells[Row, 2].Text.Trim();
            double OrderNo = VB.Val(ssView_Sheet1.Cells[Row, 3].Text.Trim());
            string OrderCode = ssView_Sheet1.Cells[Row, 4].Text.Trim();
            string strOrderDate = ssView_Sheet1.Cells[Row, 5].Text.Trim();

            double ActSeq = VB.Val(ssView_Sheet1.Cells[Row, 0].Text.Trim());

            SQL = " ";
            SQL = SQL + ComNum.VBLF + "SELECT";
            SQL = SQL + ComNum.VBLF + "    F.ACTGB ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUID F";
            SQL = SQL + ComNum.VBLF + "WHERE F.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "     AND F.ORDDATE = '" + strOrderDate.Replace("-", "") + "'";
            SQL = SQL + ComNum.VBLF + "     AND F.ORDERCODE = '" + OrderCode + "'";
            SQL = SQL + ComNum.VBLF + "     AND F.ORDNO = " + OrderNo;
            if (strSITEGB == "OPR")
            {
                SQL = SQL + ComNum.VBLF + "     AND ORDROWID = '" + strORDROWID + "'";
            }
            else
            {
                SQL = SQL + ComNum.VBLF + "     AND ORDNO = " + OrderNo;
            }
            SQL = SQL + ComNum.VBLF + "     AND F.DCCLS = '0' ";
            SQL = SQL + ComNum.VBLF + "ORDER BY (F.ACTDATE || F.ACTTIME) "; ;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return strActGb;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                return strActGb;
            }

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strActGb = strActGb + "," + dt.Rows[i]["ACTGB"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;

            return strActGb;
        }

        /// <summary>
        /// 액팅정보를 표시한다
        /// </summary>
        /// <param name="Row"></param>
        private void SetAct(int Row)
        {
            string strSITEGB = ssView_Sheet1.Cells[Row, 1].Text.Trim();
            string strORDROWID = ssView_Sheet1.Cells[Row, 2].Text.Trim();
            string strOrderDate = ssView_Sheet1.Cells[Row, 5].Text.Trim();
            double OrderNo = VB.Val(ssView_Sheet1.Cells[Row, 3].Text.Trim());
            string OrderCode = ssView_Sheet1.Cells[Row, 4].Text.Trim();
            string OrderName = ssView_Sheet1.Cells[Row, 6].Text.Trim();
            double ActSeq = VB.Val(ssView_Sheet1.Cells[Row, 0].Text.Trim());
            string ACTUSEID = ssView_Sheet1.Cells[Row, 13].Text.Trim();

            string mACTSTR = "";
            string mACTGB = "00";

            if (ACTUSEID != clsType.User.IdNumber)
            {
                ComFunc.MsgBoxEx(this, "타인이 작성한 내용입니다.");
                return;
            }

            mACTSTR = ssView_Sheet1.Cells[Row, 8].Text.Trim();

            if (mACTSTR == "시작")
            {
                mACTGB = "00";
            }
            else if (mACTSTR == "유지")
            {
                mACTGB = "01";
            }
            else if (mACTSTR == "종료")
            {
                mACTGB = "02";
            }
            else if (mACTSTR == "원타임")
            {
                mACTGB = "03";
            }

            using (frmEmrBaseRingerIOAct frm = new frmEmrBaseRingerIOAct(p, dtpOrderDate.Value.ToString("yyyyMMdd"), strOrderDate,
                                                                        strSITEGB, strORDROWID, OrderNo, OrderCode, OrderName, ActSeq, mACTGB))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }

            GetOrderData();
        }

        /// <summary>
        /// 액팅화면 띄우기
        /// </summary>
        /// <param name="Row"></param>
        private void SetActPop(int Row)
        {
            mPopRow = Row;

            PopupMenu = null;

            string[] strActGb = VB.Split(GetActGbInfo(Row), ",");
            int intQty = ssView_Sheet1.Cells[Row, 7].Text.Trim().To<int>();

            PopupMenu = new ContextMenu();
            ssView.ContextMenu = null;

            PopupMenu = new ContextMenu();

            bool blnStart = false;
            bool blnEnd = false;

            Dictionary<string, int> ActGbs = new Dictionary<string, int>();
            ActGbs.Add("00", 0);
            ActGbs.Add("01", 0);
            ActGbs.Add("02", 0);
            ActGbs.Add("03", 0);

            int Qty = 0;

            for (int i = 0; i < strActGb.Length; i++)
            {
                
                if (strActGb[i].Trim() == "00")
                {
                    if (ActGbs.TryGetValue(strActGb[i], out Qty))
                    {
                        ActGbs[strActGb[i]] = Qty +1;
                        if (ActGbs[strActGb[i]] >= intQty)
                        {
                            blnStart = true;
                        }
                    }
                }
                else if (strActGb[i].Trim() == "02")
                {
                    if (ActGbs.TryGetValue(strActGb[i], out Qty))
                    {
                        ActGbs[strActGb[i]] = Qty + 1;
                        if (ActGbs[strActGb[i]] >= intQty)
                        {
                            blnEnd = true;
                        }
                    }
                }
            }

            if (blnStart == false)
            {
                PopupMenu.MenuItems.Add("시작", new System.EventHandler(mnuItemValue_Click));
            }

            if (blnEnd == false)
            {
                PopupMenu.MenuItems.Add("유지", new System.EventHandler(mnuItemValue_Click));
                PopupMenu.MenuItems.Add("종료", new System.EventHandler(mnuItemValue_Click));
                //PopupMenu.MenuItems.Add("주사 추가", new System.EventHandler(mnuItemValue_Click));
            }

            PopupMenu.MenuItems.Add("원타임", new System.EventHandler(mnuItemValue_Click));

            if (PopupMenu.MenuItems.Count == 0)
            {
                PopupMenu = null;
                return;
            }

            ssView.ContextMenu = PopupMenu; // 입력
        }

        /// <summary>
        /// 임상관찰 기록지로 정보를 옮긴다
        /// </summary>
        /// <returns></returns>
        private bool SaveChart()
        {
            if (mstrFormNo.Equals("1568"))
            {
                return false;
            }

            GetSetTodayItem();

            if (clsEmrQueryEtc.TotInOutItem(clsDB.DbCon, p.acpNo.ToString(), p.ptNo.ToString(), mstrFormNo, "IIO", dtpOrderDate.Value.ToString("yyyyMMdd")) == false) 
            {
                ComFunc.MsgBoxEx(this, "총섭취량, 배설량 아이템 저장중 오류가 발생했습니다");
                return false;
            }

            //2.자료를 조회해서 해당 시간이 존재하는지 확인한다
            if (SaveChartAll() == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 해당일자의 아이템을 만든다
        /// </summary>
        private void GetSetTodayItem()
        {

            if (mstrFormNo.Equals("1568"))
            {
                return;
            }

            //1.일자별 등록된 것이 있는지 파악해서 있으면 세팅을 하고
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;

            SQL = "";
            SQL = SQL + ComNum.VBLF + "SELECT ";
            SQL = SQL + ComNum.VBLF + "    A.CHARTDATE, B.ITEMCD, B.BASNAME, B.BASEXNAME, B.ITEMGROUP, B.ITEMGROUPNM ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN ( ";
            SQL = SQL + ComNum.VBLF + "    SELECT ";
            SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
            SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRBASCD BB ";
            SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('임상관찰그룹')  ";
            SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('임상관찰')  ";
            SQL = SQL + ComNum.VBLF + "    UNION ALL ";
            SQL = SQL + ComNum.VBLF + "    SELECT ";
            SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
            SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRBASCD BB ";
            SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('섭취배설그룹')  ";
            SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('섭취배설') ";
            SQL = SQL + ComNum.VBLF + "    UNION ALL ";
            SQL = SQL + ComNum.VBLF + "    SELECT ";
            SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
            SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRBASCD BB ";
            SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('특수치료그룹')  ";
            SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('특수치료') ";
            SQL = SQL + ComNum.VBLF + "    UNION ALL ";
            SQL = SQL + ComNum.VBLF + "    SELECT ";
            SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
            SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
            SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRBASCD B ";
            SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRBASCD BB ";
            SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
            SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('기본간호그룹')  ";
            SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
            SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('기본간호') ";
            SQL = SQL + ComNum.VBLF + "    ) B ";
            SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.ITEMCD ";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "'";
            SQL = SQL + ComNum.VBLF + "ORDER BY B.SORT1, B.SORT2, B.SORT3, B.SORT4, B.SORT5, B.SORT6 ";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                dt.Dispose();
                dt = null;
                return;
            }

            dt.Dispose();
            dt = null;

            //2.없으면 이전 날짜 가지고 와서 세팅

            SQL = "SELECT B.BASCD, B.BASNAME, B.BASEXNAME, B.DISSEQNO, A.CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
            SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
            SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD";
            SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '" + mstrFormNameGb + "'";
            SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')";
            SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
            SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(A1.CHARTDATE) AS CHARTDATE ";
            SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "AEMRBVITALSET A1";
            SQL = SQL + ComNum.VBLF + "                                        WHERE A1.ACPNO = " + p.acpNo;
            SQL = SQL + ComNum.VBLF + "                                            AND A1.FORMNO = " + mstrFormNo;
            SQL = SQL + ComNum.VBLF + "                                            AND A1.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
            SQL = SQL + ComNum.VBLF + "                                            AND A1.CHARTDATE <= '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "')";
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (dt.Rows.Count > 0)
            {
                clsDB.setBeginTran(clsDB.DbCon);

                try
                {
                    SQL = "";
                    SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALSET ";
                    SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, PTNO, CHARTDATE, JOBGB, ITEMCD, WRITEDATE, WRITETIME, WRITEUSEID)";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "     A.FORMNO, A.ACPNO, A.PTNO, ";
                    SQL = SQL + ComNum.VBLF + "     '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' AS CHARTDATE, ";
                    SQL = SQL + ComNum.VBLF + "     A.JOBGB, A.ITEMCD, A.WRITEDATE, A.WRITETIME, A.WRITEUSEID";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B";
                    SQL = SQL + ComNum.VBLF + "    ON B.BASCD = A.ITEMCD";
                    SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '" + mstrFormNameGb + "'";
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS IN ('임상관찰', '섭취배설', '특수치료', '기본간호')";
                    SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + mstrFormNo;
                    SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
                    SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = (SELECT MAX(A1.CHARTDATE) AS CHARTDATE ";
                    SQL = SQL + ComNum.VBLF + "                                        FROM " + ComNum.DB_EMR + "AEMRBVITALSET A1";
                    SQL = SQL + ComNum.VBLF + "                                        WHERE A1.ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "                                            AND A1.FORMNO = " + mstrFormNo;
                    SQL = SQL + ComNum.VBLF + "                                            AND A1.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
                    SQL = SQL + ComNum.VBLF + "                                            AND A1.CHARTDATE <= '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "')";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBoxEx(this, SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    dt.Dispose();
                    dt = null;
                    return;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBoxEx(this, ex.Message);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
            }
            dt.Dispose();
            dt = null;

            //3.없으면 기본값을 세팅

        }

        /// <summary>
        /// 차트쪽으로 저장을 한다
        /// </summary>
        /// <returns></returns>
        private bool SaveChartAll()
        {

            int i = 0;
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0;
            DateTime dtpSys = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                string strCurDateTime = ComQuery.CurrentDateTime(clsDB.DbCon, "A");
                string strCurDate = VB.Left(strCurDateTime, 8);
                string strCurTime = VB.Right(strCurDateTime, 6);

                //0. AEMRBVITALTIME 저장을 한다
                if (SaveAEMRBVITALTIME(strCurDate, strCurTime) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                //0. AEMRCHRATMST, AEMRCHARTROW 저장을 한다
                if (SaveAEMRCHRATMSTandAEMRCHARTROW(strCurDate, strCurTime) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                #region //1. DC가 안된것 업데이트
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    SUBSTR(ACTTIME, 1, 4) || '00' AS ACTTIME, ";
                SQL = SQL + ComNum.VBLF + "    SUM(ACTQTY) AS ACTQTY ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUID ";
                SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND ACTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "    AND ACTUSEID = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "    AND ACTGB <> '00' ";
                //SQL = SQL + ComNum.VBLF + "    AND DCCLS = '0' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY SUBSTR(ACTTIME, 1, 4) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SUBSTR(ACTTIME, 1, 4) ";

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                    Cursor.Current = Cursors.Default;
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        DataTable dtEmr = null;
                        long EmrNo = 0;
                        long EmrNoHis = 0;

                        #region EMRNO, EMRNOHIS 
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "             SELECT ";
                        SQL = SQL + ComNum.VBLF + "                 C.EMRNO, C.EMRNOHIS ";
                        SQL = SQL + ComNum.VBLF + "             FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                        SQL = SQL + ComNum.VBLF + "             INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                        SQL = SQL + ComNum.VBLF + "                 ON C.EMRNO = R.EMRNO ";
                        SQL = SQL + ComNum.VBLF + "                 AND C.EMRNOHIS = R.EMRNOHIS";
                        SQL = SQL + ComNum.VBLF + "                 AND R.ITEMCD = 'I0000030580' ";
                        SQL = SQL + ComNum.VBLF + "             WHERE C.ACPNO = " + p.acpNo;
                        SQL = SQL + ComNum.VBLF + "                 AND C.FORMNO = " + mstrFormNo;
                        SQL = SQL + ComNum.VBLF + "                 AND C.CHARTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                        SQL = SQL + ComNum.VBLF + "                 AND C.CHARTTIME = '" + dt.Rows[i]["ACTTIME"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "                 AND C.CHARTUSEID <> '합계' ";
                        SQL = SQL + ComNum.VBLF + "             GROUP BY C.EMRNO, C.EMRNOHIS";
                        #endregion

                        SqlErr = clsDB.GetDataTableREx(ref dtEmr, SQL, clsDB.DbCon);
                        if (SqlErr.NotEmpty())
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                            Cursor.Current = Cursors.Default;
                            return false;
                        }

                        if (dtEmr.Rows.Count > 0)
                        {
                            EmrNo = dtEmr.Rows[0]["EMRNO"].To<long>();
                            EmrNoHis = dtEmr.Rows[0]["EMRNOHIS"].To<long>();
                        }

                        dtEmr.Dispose();

                        // 정맥주입, 총섭취량 없으면 집어 넣는다
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT ";
                        SQL = SQL + ComNum.VBLF + "    ITEMCD ";
                        SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTROW ";
                        SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + EmrNo;

                        //SQL = SQL + ComNum.VBLF + "WHERE EMRNO = ( ";
                        //SQL = SQL + ComNum.VBLF + "             SELECT ";
                        //SQL = SQL + ComNum.VBLF + "                 C.EMRNO ";
                        //SQL = SQL + ComNum.VBLF + "             FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                        //SQL = SQL + ComNum.VBLF + "             INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                        //SQL = SQL + ComNum.VBLF + "                 ON C.EMRNO = R.EMRNO ";
                        //SQL = SQL + ComNum.VBLF + "                 AND C.EMRNOHIS = R.EMRNOHIS";
                        //SQL = SQL + ComNum.VBLF + "                 AND R.ITEMCD = 'I0000030580' ";
                        //SQL = SQL + ComNum.VBLF + "             WHERE C.ACPNO = " + p.acpNo;
                        //SQL = SQL + ComNum.VBLF + "                 AND C.FORMNO = " + mstrFormNo;
                        //SQL = SQL + ComNum.VBLF + "                 AND C.CHARTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                        //SQL = SQL + ComNum.VBLF + "                 AND C.CHARTTIME = '" + dt.Rows[i]["ACTTIME"].ToString().Trim() + "' ";
                        //SQL = SQL + ComNum.VBLF + "                 AND C.CHARTUSEID <> '합계' ";
                        //SQL = SQL + ComNum.VBLF + "             GROUP BY C.EMRNO";
                        //SQL = SQL + ComNum.VBLF + "             )";
                        //SQL = SQL + ComNum.VBLF + "     AND ITEMCD = 'I0000030580'";
                        SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                            Cursor.Current = Cursors.Default;
                            return false;
                        }

                        DataTable dtAct = null;
                        double dblActSum = 0;

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT ";
                        SQL = SQL + ComNum.VBLF + "    SUM(ACTQTY) AS ACTSUM ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUID ";
                        SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
                        SQL = SQL + ComNum.VBLF + "    AND ACTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                        //SQL = SQL + ComNum.VBLF + "    AND ACTUSEID = '" + clsType.User.IdNumber + "'"; //해당시간 전체 SUM
                        SQL = SQL + ComNum.VBLF + "    AND ACTGB <> '00' ";
                        SQL = SQL + ComNum.VBLF + "    AND DCCLS = '0' ";
                        SQL = SQL + ComNum.VBLF + "    AND ACTTIME = '" + dt.Rows[i]["ACTTIME"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTableREx(ref dtAct, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                            Cursor.Current = Cursors.Default;
                            return false;
                        }

                        if (dtAct.Rows.Count > 0)
                        {
                            dblActSum = VB.Val(dtAct.Rows[0]["ACTSUM"].ToString().Trim());
                        }
                        dtAct.Dispose();
                        dtAct = null;

                        if (dt1.Rows.Count > 0)
                        {
                            dt1.Dispose();
                            dt1 = null;

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "UPDATE  " + ComNum.DB_EMR + "AEMRCHARTROW SET ";
                            SQL = SQL + ComNum.VBLF + "      ITEMVALUE = '" + dblActSum + "'";
                            SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + EmrNo;

                            //SQL = SQL + ComNum.VBLF + "WHERE EMRNO = ( ";
                            //SQL = SQL + ComNum.VBLF + "             SELECT ";
                            //SQL = SQL + ComNum.VBLF + "                 C.EMRNO ";
                            //SQL = SQL + ComNum.VBLF + "             FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                            //SQL = SQL + ComNum.VBLF + "             INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                            //SQL = SQL + ComNum.VBLF + "                 ON C.EMRNO = R.EMRNO ";
                            //SQL = SQL + ComNum.VBLF + "                 AND C.EMRNOHIS = R.EMRNOHIS";
                            //SQL = SQL + ComNum.VBLF + "                 AND R.ITEMCD = 'I0000030580' ";
                            //SQL = SQL + ComNum.VBLF + "             WHERE C.ACPNO = " + p.acpNo;
                            //SQL = SQL + ComNum.VBLF + "                 AND C.FORMNO = " + mstrFormNo;
                            //SQL = SQL + ComNum.VBLF + "                 AND C.CHARTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                            //SQL = SQL + ComNum.VBLF + "                 AND C.CHARTTIME = '" + dt.Rows[i]["ACTTIME"].ToString().Trim() + "' ";
                            //SQL = SQL + ComNum.VBLF + "                 AND C.CHARTUSEID <> '합계' ";
                            //SQL = SQL + ComNum.VBLF + "             GROUP BY C.EMRNO";
                            //SQL = SQL + ComNum.VBLF + "             )";
                            SQL = SQL + ComNum.VBLF + "     AND ITEMCD = 'I0000030580'";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                                Cursor.Current = Cursors.Default;
                                return false;
                            }
                        }
                        else
                        {
                            dt1.Dispose();
                            dt1 = null;

                            if (dblActSum > 0)
                            {
                                EmrNo = 0;
                                EmrNoHis = 0;

                                #region EMRNO, EMRNOHIS 
                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "             SELECT ";
                                SQL = SQL + ComNum.VBLF + "                 C.EMRNO, C.EMRNOHIS ";
                                SQL = SQL + ComNum.VBLF + "             FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                                SQL = SQL + ComNum.VBLF + "             WHERE C.ACPNO = " + p.acpNo;
                                SQL = SQL + ComNum.VBLF + "                 AND C.FORMNO = " + mstrFormNo;
                                SQL = SQL + ComNum.VBLF + "                 AND C.CHARTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                                SQL = SQL + ComNum.VBLF + "                 AND C.CHARTTIME = '" + dt.Rows[i]["ACTTIME"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "                 AND C.CHARTUSEID <> '합계' ";
                                SQL = SQL + ComNum.VBLF + "             GROUP BY C.EMRNO, C.EMRNOHIS";
                                #endregion

                                SqlErr = clsDB.GetDataTableREx(ref dtEmr, SQL, clsDB.DbCon);
                                if (SqlErr.NotEmpty())
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }

                                if (dtEmr.Rows.Count > 0)
                                {
                                    EmrNo = dtEmr.Rows[0]["EMRNO"].To<long>();
                                    EmrNoHis = dtEmr.Rows[0]["EMRNOHIS"].To<long>();
                                }

                                dtEmr.Dispose();

                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_EMR.AEMRCHARTROW ";
                                SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                                SQL = SQL + ComNum.VBLF + "VALUES (";

                                #region 기존
                                //SQL = SQL + ComNum.VBLF + "             (SELECT ";
                                //SQL = SQL + ComNum.VBLF + "                 C.EMRNO ";
                                //SQL = SQL + ComNum.VBLF + "             FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                                ////SQL = SQL + ComNum.VBLF + "             INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                                ////SQL = SQL + ComNum.VBLF + "                 ON C.EMRNO = R.EMRNO ";
                                ////SQL = SQL + ComNum.VBLF + "                 AND C.EMRNOHIS = R.EMRNOHIS";
                                ////SQL = SQL + ComNum.VBLF + "                 AND R.ITEMCD = 'I0000030580' ";
                                //SQL = SQL + ComNum.VBLF + "             WHERE C.ACPNO = " + p.acpNo;
                                //SQL = SQL + ComNum.VBLF + "                 AND C.FORMNO = " + mstrFormNo;
                                //SQL = SQL + ComNum.VBLF + "                 AND C.CHARTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                                //SQL = SQL + ComNum.VBLF + "                 AND C.CHARTTIME = '" + dt.Rows[i]["ACTTIME"].ToString().Trim() + "' ";
                                //SQL = SQL + ComNum.VBLF + "                 AND C.CHARTUSEID <> '합계' ";
                                //SQL = SQL + ComNum.VBLF + "             GROUP BY C.EMRNO ) ,";  //EMRNO

                                //SQL = SQL + ComNum.VBLF + "             (SELECT ";
                                //SQL = SQL + ComNum.VBLF + "                 C.EMRNOHIS ";
                                //SQL = SQL + ComNum.VBLF + "             FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                                ////SQL = SQL + ComNum.VBLF + "             INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                                ////SQL = SQL + ComNum.VBLF + "                 ON C.EMRNO = R.EMRNO ";
                                ////SQL = SQL + ComNum.VBLF + "                 AND C.EMRNOHIS = R.EMRNOHIS";
                                ////SQL = SQL + ComNum.VBLF + "                 AND R.ITEMCD = 'I0000030580' ";
                                //SQL = SQL + ComNum.VBLF + "             WHERE C.ACPNO = " + p.acpNo;
                                //SQL = SQL + ComNum.VBLF + "                 AND C.FORMNO = " + mstrFormNo;
                                //SQL = SQL + ComNum.VBLF + "                 AND C.CHARTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                                //SQL = SQL + ComNum.VBLF + "                 AND C.CHARTTIME = '" + dt.Rows[i]["ACTTIME"].ToString().Trim() + "' ";
                                //SQL = SQL + ComNum.VBLF + "                 AND C.CHARTUSEID <> '합계' ";
                                //SQL = SQL + ComNum.VBLF + "             GROUP BY C.EMRNOHIS ) ,";  //EMRNOHIS

                                #endregion

                                SQL = SQL + ComNum.VBLF + EmrNo + ",";   //EMRNO
                                SQL = SQL + ComNum.VBLF + EmrNoHis + ",";   //EMRNOHIS
                                SQL = SQL + ComNum.VBLF + " 'I0000030580',";   //ITEMCD
                                SQL = SQL + ComNum.VBLF + " 'I0000030580',"; //ITEMNO
                                SQL = SQL + ComNum.VBLF + " '-1',"; //ITEMINDEX
                                SQL = SQL + ComNum.VBLF + " 'TEXT',";   //ITEMTYPE
                                SQL = SQL + ComNum.VBLF + " '" + dblActSum + "',";   //ITEMVALUE
                                SQL = SQL + ComNum.VBLF + " 0,";   //DSPSEQ  //dt1.Rows[j]["DISSEQNO"].ToString().Trim()
                                SQL = SQL + ComNum.VBLF + " '', ";   //ITEMVALUE1
                                SQL = SQL + ComNum.VBLF + " '',";   //INPUSEID
                                SQL = SQL + ComNum.VBLF + " '" + strCurDate + "', ";   //INPDATE
                                SQL = SQL + ComNum.VBLF + " '" + strCurTime + "' ";   //INPTIME
                                SQL = SQL + ComNum.VBLF + ")";

                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                        }

                        if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.IdNumber) == true)
                        {
                            clsEmrQuery.SaveEmrCert(clsDB.DbCon, EmrNo, false);
                        }
                    }
                }
                dt.Dispose();
                dt = null;
                #endregion //1. DC가 안된것 업데이트

                #region //2. DC된것 업데이트
                //SQL = "";
                //SQL = SQL + ComNum.VBLF + "SELECT ";
                //SQL = SQL + ComNum.VBLF + "    SUBSTR(A.ACTTIME, 1, 4) || '00' AS ACTTIME ";
                //SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUID A ";
                //SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                //SQL = SQL + ComNum.VBLF + "    ON A.ACPNO = C.ACPNO ";
                //SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE = C.CHARTDATE ";
                //SQL = SQL + ComNum.VBLF + "    AND SUBSTR(A.ACTTIME, 1, 4) || '00' = C.CHARTTIME ";
                //SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = " + mstrFormNo;
                //SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                //SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = R.EMRNO ";
                //SQL = SQL + ComNum.VBLF + "    AND R.ITEMCD = 'I0000030580' ";
                //SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
                //SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                //SQL = SQL + ComNum.VBLF + "    AND A.ACTUSEID = '" + clsType.User.IdNumber + "'";
                //SQL = SQL + ComNum.VBLF + "    AND A.ACTGB <> '00' ";
                //SQL = SQL + ComNum.VBLF + "    AND A.DCCLS = '1' ";
                //SQL = SQL + ComNum.VBLF + "    AND SUBSTR(A.ACTTIME, 1, 4) || '00' NOT IN(SELECT ";
                //SQL = SQL + ComNum.VBLF + "                                SUBSTR(A1.ACTTIME, 1, 4) || '00' ";
                //SQL = SQL + ComNum.VBLF + "                            FROM " + ComNum.DB_EMR + "AEMRBIOFLUID A1 ";
                //SQL = SQL + ComNum.VBLF + "                            WHERE A1.ACPNO = A.ACPNO ";
                //SQL = SQL + ComNum.VBLF + "                                AND A1.ACTDATE = A.ACTDATE ";
                //SQL = SQL + ComNum.VBLF + "                                AND A1.ACTGB <> '00' ";
                //SQL = SQL + ComNum.VBLF + "                                AND A1.DCCLS = '0') ";
                //SQL = SQL + ComNum.VBLF + "GROUP BY SUBSTR(A.ACTTIME, 1, 4) ";
                ////SQL = SQL + ComNum.VBLF + "ORDER BY SUBSTR(A.ACTTIME, 1, 4) ";
                //SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                //if (SqlErr != "")
                //{
                //    clsDB.setRollbackTran(clsDB.DbCon);
                //    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //    ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                //    Cursor.Current = Cursors.Default;
                //    return false;
                //}
                //if (dt.Rows.Count > 0)
                //{
                //    for (i = 0; i < dt.Rows.Count; i++)
                //    {
                //        SQL = "";
                //        SQL = SQL + ComNum.VBLF + "UPDATE  " + ComNum.DB_EMR + "AEMRCHARTROW SET ";
                //        SQL = SQL + ComNum.VBLF + "      ITEMVALUE = '0' ";
                //        SQL = SQL + ComNum.VBLF + "WHERE EMRNO = ( ";
                //        SQL = SQL + ComNum.VBLF + "             SELECT ";
                //        SQL = SQL + ComNum.VBLF + "                 C.EMRNO ";
                //        SQL = SQL + ComNum.VBLF + "             FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                //        SQL = SQL + ComNum.VBLF + "             INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                //        SQL = SQL + ComNum.VBLF + "                 ON C.EMRNO = R.EMRNO ";
                //        SQL = SQL + ComNum.VBLF + "                 AND R.ITEMCD = 'I0000030580' ";
                //        SQL = SQL + ComNum.VBLF + "             WHERE C.ACPNO = " + p.acpNo;
                //        SQL = SQL + ComNum.VBLF + "                 AND C.FORMNO = " + mstrFormNo;
                //        SQL = SQL + ComNum.VBLF + "                 AND C.CHARTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                //        SQL = SQL + ComNum.VBLF + "                 AND C.CHARTTIME = '" + dt.Rows[i]["ACTTIME"].ToString().Trim() + "' ";
                //        SQL = SQL + ComNum.VBLF + "                 AND C.CHARTUSEID <> '합계' ";
                //        SQL = SQL + ComNum.VBLF + "             GROUP BY C.EMRNO";
                //        SQL = SQL + ComNum.VBLF + "             )";
                //        SQL = SQL + ComNum.VBLF + "     AND ITEMCD = 'I0000030580'";

                //        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                //        if (SqlErr != "")
                //        {
                //            clsDB.setRollbackTran(clsDB.DbCon);
                //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                //            ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                //            Cursor.Current = Cursors.Default;
                //            return false;
                //        }
                //    }
                //}
                //dt.Dispose();
                //dt = null;
                #endregion //2. DC된것 업데이트

                #region //3.섭취합계를 다시 구한다
                if (dtpSys.Date < ("2020-09-22").To<DateTime>())
                {
                    SQL = "";
                    SQL = SQL + ComNum.VBLF + "SELECT ";
                    SQL = SQL + ComNum.VBLF + "    R.EMRNO, R.EMRNOHIS, SUM(R.ITEMVALUE) AS IOSUM";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                    SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = R.EMRNO ";
                    SQL = SQL + ComNum.VBLF + "   AND C.EMRNOHIS = R.EMRNOHIS";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRBASCD B ";
                    SQL = SQL + ComNum.VBLF + "    ON R.ITEMCD = B.BASCD ";
                    SQL = SQL + ComNum.VBLF + "    AND B.BSNSCLS = '기록지관리' ";
                    SQL = SQL + ComNum.VBLF + "    AND B.UNITCLS = '섭취배설' ";
                    SQL = SQL + ComNum.VBLF + "    AND B.VFLAG3 = '01.섭취' ";
                    SQL = SQL + ComNum.VBLF + "INNER JOIN ";
                    SQL = SQL + ComNum.VBLF + "    ( ";
                    SQL = SQL + ComNum.VBLF + "    SELECT ";
                    SQL = SQL + ComNum.VBLF + "        R1.ACPNO, R1.ACTDATE, SUBSTR(R1.ACTTIME, 1, 4) || '00' AS ACTTIME";
                    SQL = SQL + ComNum.VBLF + "    FROM " + ComNum.DB_EMR + "AEMRBIOFLUID R1 ";
                    SQL = SQL + ComNum.VBLF + "    WHERE R1.ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "        AND R1.ACTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                    SQL = SQL + ComNum.VBLF + "        AND R1.ACTUSEID = '" + clsType.User.IdNumber + "'";
                    SQL = SQL + ComNum.VBLF + "        AND R1.ACTGB <> '00' ";
                    //SQL = SQL + ComNum.VBLF + "        AND R1.DCCLS = '0' ";
                    SQL = SQL + ComNum.VBLF + "    GROUP BY R1.ACPNO, R1.ACTDATE, R1.ACTTIME ";
                    SQL = SQL + ComNum.VBLF + "    ) R ";
                    SQL = SQL + ComNum.VBLF + "    ON C.ACPNO = R.ACPNO ";
                    SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE = R.ACTDATE ";
                    SQL = SQL + ComNum.VBLF + "    AND C.CHARTTIME = R.ACTTIME ";
                    SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = " + p.acpNo;
                    SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = " + mstrFormNo;
                    SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                    //SQL = SQL + ComNum.VBLF + "    AND REGEXP_INSTR(REPLACE(R.ITEMVALUE, '.', ''),'[^0-9]') = 0 "; //숫자인 것만
                    //SQL = SQL + ComNum.VBLF + @"    AND (REGEXP_INSTR(R.ITEMVALUE,'[^0-9]') = 0 OR REGEXP_INSTR(R.ITEMVALUE,'^[0-9]\.[0-9]{1, 2}$') = 0) ";
                    SQL = SQL + ComNum.VBLF + @"   AND REGEXP_LIKE(R.ITEMVALUE,'^-?\d*\.?\d+([eE]-?\d+)?$')"; //소수점까지 체크가능
                    SQL = SQL + ComNum.VBLF + "GROUP BY R.EMRNO, R.EMRNOHIS ";

                    SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {

                            // 정맥주입, 총섭취량 없으면 집어 넣는다
                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "SELECT ";
                            SQL = SQL + ComNum.VBLF + "    ITEMCD ";
                            SQL = SQL + ComNum.VBLF + "FROM  " + ComNum.DB_EMR + "AEMRCHARTROW ";
                            SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + dt.Rows[i]["EMRNO"].ToString().Trim();
                            SQL = SQL + ComNum.VBLF + "  AND EMRNOHIS = " + dt.Rows[i]["EMRNOHIS"].ToString().Trim();
                            SQL = SQL + ComNum.VBLF + "  AND ITEMCD = 'I0000030622'";
                            SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                                Cursor.Current = Cursors.Default;
                                return false;
                            }

                            if (dt1.Rows.Count > 0)
                            {
                                dt1.Dispose();
                                dt1 = null;

                                SQL = "";
                                SQL = SQL + ComNum.VBLF + "UPDATE  " + ComNum.DB_EMR + "AEMRCHARTROW SET ";
                                string strSum = "";
                                if (VB.Val(dt.Rows[i]["IOSUM"].ToString().Trim()) != 0)
                                {
                                    strSum = dt.Rows[i]["IOSUM"].ToString().Trim();
                                }
                                SQL = SQL + ComNum.VBLF + "      ITEMVALUE = '" + strSum + "' ";
                                SQL = SQL + ComNum.VBLF + "WHERE EMRNO = " + dt.Rows[i]["EMRNO"].ToString().Trim();
                                SQL = SQL + ComNum.VBLF + "  AND EMRNOHIS = " + dt.Rows[i]["EMRNOHIS"].ToString().Trim();
                                SQL = SQL + ComNum.VBLF + "  AND ITEMCD = 'I0000030622'";
                                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                                    Cursor.Current = Cursors.Default;
                                    return false;
                                }
                            }
                            else
                            {
                                dt1.Dispose();
                                dt1 = null;
                                if (VB.Val(dt.Rows[i]["IOSUM"].ToString().Trim()) != 0)
                                {
                                    SQL = "";
                                    SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_EMR.AEMRCHARTROW ";
                                    SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                                    SQL = SQL + ComNum.VBLF + "VALUES (";
                                    SQL = SQL + ComNum.VBLF + " " + dt.Rows[i]["EMRNO"].ToString().Trim() + ",";  //EMRNO
                                    SQL = SQL + ComNum.VBLF + " " + dt.Rows[i]["EMRNOHIS"].ToString().Trim() + ",";  //EMRNOHIS
                                    SQL = SQL + ComNum.VBLF + " 'I0000030622',";   //ITEMCD
                                    SQL = SQL + ComNum.VBLF + " 'I0000030622',"; //ITEMNO
                                    SQL = SQL + ComNum.VBLF + " '-1',"; //ITEMINDEX
                                    SQL = SQL + ComNum.VBLF + " 'TEXT',";   //ITEMTYPE
                                    string strSum = "";
                                    if (VB.Val(dt.Rows[i]["IOSUM"].ToString().Trim()) != 0)
                                    {
                                        strSum = dt.Rows[i]["IOSUM"].ToString().Trim();
                                    }
                                    SQL = SQL + ComNum.VBLF + " '" + strSum + "',";   //ITEMVALUE
                                    SQL = SQL + ComNum.VBLF + " 0,";   //DSPSEQ  
                                    SQL = SQL + ComNum.VBLF + " '', ";   //ITEMVALUE1
                                    SQL = SQL + ComNum.VBLF + " '',";   //INPUSEID
                                    SQL = SQL + ComNum.VBLF + " '" + strCurDate + "', ";   //INPDATE
                                    SQL = SQL + ComNum.VBLF + " '" + strCurTime + "' ";   //INPTIME
                                    SQL = SQL + ComNum.VBLF + ")";
                                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.setRollbackTran(clsDB.DbCon);
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        ComFunc.MsgBox("IO 저장중 오류가 발생하였습니다.");
                                        Cursor.Current = Cursors.Default;
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
                #endregion //3.섭취합계를 다시 구한다

                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장 하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// AEMRCHRATMST and AEMRCHARTROW 저장을 한다
        /// </summary>
        /// <returns></returns>
        private bool SaveAEMRCHRATMSTandAEMRCHARTROW(string strCurDate, string strCurTime)
        {
            DataTable dt = null;
            int i = 0;
            int j = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            double dblEmrHisNo = 0;
            double dblEmrNoNew = 0;

            string strChartDate = dtpOrderDate.Value.ToString("yyyyMMdd");
            string strChartTime = "";
            string strCHARTUSEID = clsType.User.IdNumber;
            string strCOMPUSEID = clsType.User.IdNumber;
            string strSaveFlag = "SAVE";
            string strSAVEGB = "1";
            string strSAVECERT = "1"; // 0:임시저장, 1:인증저장
            string strFORMGB = "0";
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    SUBSTR(A.ACTTIME, 1, 4)  || '00' AS ACTTIME ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUID A ";
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTUSEID = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTGB <> '00' ";
                SQL = SQL + ComNum.VBLF + "    AND A.DCCLS = '0' ";
                SQL = SQL + ComNum.VBLF + "    AND SUBSTR(A.ACTTIME, 1, 4) NOT IN (SELECT SUBSTR(C.CHARTTIME, 1, 4) ";
                SQL = SQL + ComNum.VBLF + "                         FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                SQL = SQL + ComNum.VBLF + "                         INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                SQL = SQL + ComNum.VBLF + "                             ON C.EMRNO = R.EMRNO ";
                SQL = SQL + ComNum.VBLF + "                            AND C.EMRNOHIS = R.EMRNOHIS";
                //SQL = SQL + ComNum.VBLF + "                             AND R.ITEMCD = 'I0000030580' ";
                SQL = SQL + ComNum.VBLF + "                         WHERE C.ACPNO = A.ACPNO ";
                SQL = SQL + ComNum.VBLF + "                             AND C.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "                             AND C.CHARTDATE = A.ACTDATE ";
                SQL = SQL + ComNum.VBLF + "                             AND SUBSTR(C.CHARTTIME, 1, 4) = SUBSTR(A.ACTTIME, 1, 4) )";
                SQL = SQL + ComNum.VBLF + "GROUP BY SUBSTR(A.ACTTIME, 1, 4) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY SUBSTR(A.ACTTIME, 1, 4) ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("SaveAEMRCHRATMSTandAEMRCHARTROW 오류가 발생하였습니다.");
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        //해당 데이타가 있는지 한번더 조회 한다.
                        DataTable dt1 = null;
                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT SUBSTR(C.CHARTTIME, 1, 4) ";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRCHARTMST C ";
                        SQL = SQL + ComNum.VBLF + "INNER JOIN " + ComNum.DB_EMR + "AEMRCHARTROW R ";
                        SQL = SQL + ComNum.VBLF + "    ON C.EMRNO = R.EMRNO ";
                        SQL = SQL + ComNum.VBLF + "    AND C.EMRNOHIS = R.EMRNOHIS";
                        SQL = SQL + ComNum.VBLF + "    AND R.ITEMCD = 'I0000030580' ";
                        SQL = SQL + ComNum.VBLF + "WHERE C.ACPNO = " + p.acpNo;
                        SQL = SQL + ComNum.VBLF + "    AND C.FORMNO = " + mstrFormNo;
                        SQL = SQL + ComNum.VBLF + "    AND C.CHARTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                        SQL = SQL + ComNum.VBLF + "    AND SUBSTR(C.CHARTTIME, 1, 4) = SUBSTR('" + dt.Rows[i]["ACTTIME"].ToString().Trim() + "', 1, 4) ";
                        SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("SaveAEMRCHRATMSTandAEMRCHARTROW 오류가 발생하였습니다.");
                            return false;
                        }
                        if (dt1.Rows.Count > 0)
                        {
                            dt1.Dispose();
                            dt1 = null;
                            continue;
                        }
                        dt1.Dispose();
                        dt1 = null;

                        dblEmrHisNo = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "EMRXMLHISNO"));
                        dblEmrNoNew = VB.Val(ComQuery.gGetSequencesNo(clsDB.DbCon, ComNum.DB_EMR + "GETEMRXMLNO"));

                        strChartTime = dt.Rows[i]["ACTTIME"].ToString().Trim();

                        if (clsEmrQuery.SaveChartMstOnly(clsDB.DbCon, dblEmrNoNew, dblEmrHisNo, p, mstrFormNo, mstrUpdateNo,
                                    strChartDate, strChartTime,
                                    strCHARTUSEID, strCOMPUSEID, strSAVEGB, strSAVECERT, strFORMGB,
                                    strCurDate, strCurTime, strSaveFlag) == false)
                        {
                            ComFunc.MsgBox("SaveAEMRCHRATMSTandAEMRCHARTROW 오류가 발생했습니다.");
                            return false;
                        }

                        SQL = "";
                        SQL = SQL + ComNum.VBLF + "SELECT ";
                        SQL = SQL + ComNum.VBLF + "    A.CHARTDATE, B.ITEMCD, B.BASNAME, B.BASEXNAME, B.ITEMGROUP, B.ITEMGROUPNM";
                        SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBVITALSET A";
                        SQL = SQL + ComNum.VBLF + "INNER JOIN ( ";
                        SQL = SQL + ComNum.VBLF + "    SELECT ";
                        SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
                        SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
                        SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRBASCD B ";
                        SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRBASCD BB ";
                        SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
                        SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
                        SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('임상관찰그룹')  ";
                        SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
                        SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('임상관찰')  ";
                        SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                        SQL = SQL + ComNum.VBLF + "    SELECT ";
                        SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
                        SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
                        SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRBASCD B ";
                        SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRBASCD BB ";
                        SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
                        SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
                        SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('섭취배설그룹')  ";
                        SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
                        SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('섭취배설') ";
                        SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                        SQL = SQL + ComNum.VBLF + "    SELECT ";
                        SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
                        SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
                        SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRBASCD B ";
                        SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRBASCD BB ";
                        SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
                        SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
                        SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('특수치료그룹')  ";
                        SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
                        SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('특수치료') ";
                        SQL = SQL + ComNum.VBLF + "    UNION ALL ";
                        SQL = SQL + ComNum.VBLF + "    SELECT ";
                        SQL = SQL + ComNum.VBLF + "        B.BASCD AS ITEMCD, B.BASNAME, B.BASEXNAME, B.NFLAG1  AS SORT3, B.NFLAG2  AS SORT4, B.NFLAG3  AS SORT5, B.DISSEQNO  AS SORT6, ";
                        SQL = SQL + ComNum.VBLF + "        BB.BASCD AS ITEMGROUP, BB.BASNAME AS ITEMGROUPNM, BB.VFLAG1 AS SORT1, BB.DISSEQNO AS SORT2 ";
                        SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_EMR.AEMRBASCD B ";
                        SQL = SQL + ComNum.VBLF + "    INNER JOIN KOSMOS_EMR.AEMRBASCD BB ";
                        SQL = SQL + ComNum.VBLF + "        ON B.VFLAG1 = BB.BASCD ";
                        SQL = SQL + ComNum.VBLF + "        AND BB.BSNSCLS = '기록지관리' ";
                        SQL = SQL + ComNum.VBLF + "        AND BB.UNITCLS IN ('기본간호그룹')  ";
                        SQL = SQL + ComNum.VBLF + "    WHERE B.BSNSCLS = '기록지관리' ";
                        SQL = SQL + ComNum.VBLF + "        AND B.UNITCLS IN ('기본간호') ";
                        SQL = SQL + ComNum.VBLF + "    ) B ";
                        SQL = SQL + ComNum.VBLF + "    ON A.ITEMCD = B.ITEMCD ";
                        SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
                        SQL = SQL + ComNum.VBLF + "    AND A.FORMNO = " + mstrFormNo;
                        SQL = SQL + ComNum.VBLF + "    AND A.JOBGB IN ('IVT', 'IIO', 'IST', 'IBN')";
                        SQL = SQL + ComNum.VBLF + "    AND A.CHARTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "'";
                        SQL = SQL + ComNum.VBLF + "ORDER BY B.SORT1, B.SORT2, B.SORT3, B.SORT4, B.SORT5, B.SORT6 ";

                        SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("SaveAEMRCHRATMSTandAEMRCHARTROW 오류가 발생하였습니다.");
                            return false;
                        }

                        if (dt1.Rows.Count == 0)
                        {
                            dt1.Dispose();
                            dt1 = null;
                            ComFunc.MsgBox("SaveAEMRCHRATMSTandAEMRCHARTROW 오류가 발생하였습니다." + ComNum.VBLF + "해당일자에 등록된 아이템이 없습니다.");
                            return false;
                        }
                        
                        for (j = 0; j < dt1.Rows.Count; j++)
                        {
                            string ITEMCD = dt1.Rows[j]["ITEMCD"].ToString().Trim();
                            string ITEMNO = ITEMCD;
                            string ITEMINDEX = "-1";
                            string ITEMTYPE = "TEXT";

                            int intFind = VB.InStr(ITEMCD, "_");

                            if (intFind > 0)
                            {
                                ITEMNO = ComFunc.SptChar(ITEMCD, 0, "_");
                                ITEMINDEX = ComFunc.SptChar(ITEMCD, 1, "_");
                            }

                            SQL = "";
                            SQL = SQL + ComNum.VBLF + "INSERT INTO KOSMOS_EMR.AEMRCHARTROW ";
                            SQL = SQL + ComNum.VBLF + "    (EMRNO, EMRNOHIS, ITEMCD, ITEMNO, ITEMINDEX, ITEMTYPE, ITEMVALUE, DSPSEQ, ITEMVALUE1, INPUSEID, INPDATE, INPTIME )";
                            SQL = SQL + ComNum.VBLF + "VALUES (";
                            SQL = SQL + ComNum.VBLF +  dblEmrNoNew.ToString() + ",";    //EMRNO
                            SQL = SQL + ComNum.VBLF +  dblEmrHisNo.ToString() + ",";    //EMRNOHIS
                            SQL = SQL + ComNum.VBLF + " '" + ITEMCD + "',";   //ITEMCD
                            SQL = SQL + ComNum.VBLF + " '" + ITEMNO + "',"; //ITEMNO
                            SQL = SQL + ComNum.VBLF + " '" + ITEMINDEX + "',"; //ITEMINDEX
                            SQL = SQL + ComNum.VBLF + " '" + ITEMTYPE + "',";   //ITEMTYPE
                            SQL = SQL + ComNum.VBLF + " '',";   //ITEMVALUE
                            SQL = SQL + ComNum.VBLF + " " + j.ToString() + ",";   //DSPSEQ  //dt1.Rows[j]["DISSEQNO"].ToString().Trim()
                            SQL = SQL + ComNum.VBLF + " '', ";   //ITEMVALUE1
                            SQL = SQL + ComNum.VBLF + " '',";   //INPUSEID
                            SQL = SQL + ComNum.VBLF + " '', ";   //INPDATE
                            SQL = SQL + ComNum.VBLF + " '' ";   //INPTIME
                            SQL = SQL + ComNum.VBLF + ")";
                            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                ComFunc.MsgBox("SaveAEMRCHRATMSTandAEMRCHARTROW / AEMRCHARTROW 오류가 발생하였습니다.");
                                return false;
                            }
                        }

                        if (clsCertWork.Cert_Check(clsDB.DbCon, clsType.User.IdNumber) == true)
                        {
                            clsEmrQuery.SaveEmrCert(clsDB.DbCon, dblEmrNoNew, false);
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 차트시간 정보를 입력한다
        /// </summary>
        /// <param name="strCurDate"></param>
        /// <param name="strCurTime"></param>
        /// <returns></returns>
        private bool SaveAEMRBVITALTIME(string strCurDate, string strCurTime)
        {
            DataTable dt = null;
            int i = 0;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            OracleDataReader reader = null;

            string strChartDate = dtpOrderDate.Value.ToString("yyyyMMdd");
            string strJOBGB = "IVT";

            try
            {
                #region 재원환자의 경우 당일 기본 시간을 만들어 줘야 한다
                if (clsEmrQueryEtc.SetSaveDefaultVitalTime(clsDB.DbCon, p.acpNo, p.ward, dtpOrderDate.Value.ToString("yyyyMMdd"), mstrFormNo) == false)
                {
                    return false;
                }
                #endregion

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    SUBSTR(A.ACTTIME, 1, 4) || '00' AS ACTTIME";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUID A ";
                SQL = SQL + ComNum.VBLF + "WHERE A.ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND A.ACTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTUSEID = '" + clsType.User.IdNumber + "'";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTGB <> '00' ";
                SQL = SQL + ComNum.VBLF + "    AND A.DCCLS = '0' ";
                SQL = SQL + ComNum.VBLF + "    AND A.ACTTIME NOT IN (SELECT TIMEVALUE || '00' AS CHARTTIME  ";
                SQL = SQL + ComNum.VBLF + "                         FROM " + ComNum.DB_EMR + "AEMRBVITALTIME C ";
                SQL = SQL + ComNum.VBLF + "                         WHERE C.ACPNO = A.ACPNO ";
                SQL = SQL + ComNum.VBLF + "                             AND C.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "                             AND C.CHARTDATE = A.ACTDATE ";
                SQL = SQL + ComNum.VBLF + "                             AND C.FORMNO = " + mstrFormNo;
                SQL = SQL + ComNum.VBLF + "                             AND C.TIMEVALUE = SUBSTR(A.ACTTIME, 1, 4) )";
                SQL = SQL + ComNum.VBLF + "GROUP BY SUBSTR(A.ACTTIME, 1, 4) ";
                //SQL = SQL + ComNum.VBLF + "ORDER BY A.ACTTIME ";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("SaveAEMRBVITALTIME 오류가 발생하였습니다.");
                    return false;
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        #region 무결성 오류 나서 점검
                        SQL = "SELECT 1 AS CNT";
                        SQL = SQL + ComNum.VBLF + "FROM DUAL";
                        SQL = SQL + ComNum.VBLF + "WHERE EXISTS";
                        SQL = SQL + ComNum.VBLF + "(";
                        SQL = SQL + ComNum.VBLF + "SELECT 1";
                        SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_EMR.AEMRBVITALTIME";
                        SQL = SQL + ComNum.VBLF + "  WHERE FORMNO = " + mstrFormNo;
                        SQL = SQL + ComNum.VBLF + "   AND ACPNO = " + p.acpNo;
                        SQL = SQL + ComNum.VBLF + "   AND CHARTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "'";
                        SQL = SQL + ComNum.VBLF + "   AND JOBGB = '" + strJOBGB + "'";
                        SQL = SQL + ComNum.VBLF + "   AND TIMEVALUE = '" + VB.Left(dt.Rows[i]["ACTTIME"].ToString().Trim(), 4) + "'";
                        SQL = SQL + ComNum.VBLF + "   AND SUBGB = '0'";
                        SQL = SQL + ComNum.VBLF + ")";

                        SqlErr = clsDB.GetAdoRs(ref reader, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return false;
                        }

                        if (reader.HasRows == true)
                        {
                            reader.Dispose();
                            continue;
                        }
                        reader.Dispose();
                        #endregion

                        SQL = "INSERT INTO " + ComNum.DB_EMR + "AEMRBVITALTIME ";
                        SQL = SQL + ComNum.VBLF + "(FORMNO, ACPNO, JOBGB , CHARTDATE, TIMEVALUE, SUBGB, WRITEDATE, WRITETIME, WRITEUSEID)";
                        SQL = SQL + ComNum.VBLF + "VALUES (";
                        SQL = SQL + ComNum.VBLF + "" + mstrFormNo + ",";  //FORMNO
                        SQL = SQL + ComNum.VBLF + "" + p.acpNo + ",";  //ACPNO
                        SQL = SQL + ComNum.VBLF + "'" + strJOBGB + "',";    //JOBGB
                        SQL = SQL + ComNum.VBLF + "'" + dtpOrderDate.Value.ToString("yyyyMMdd") + "',";  //CHARTDATE
                        SQL = SQL + ComNum.VBLF + "'" + VB.Left(dt.Rows[i]["ACTTIME"].ToString().Trim(), 4) + "',"; //TIMEVALUE
                        SQL = SQL + ComNum.VBLF + "'0',";   //SUBGB
                        SQL = SQL + ComNum.VBLF + "'" + strCurDate + "',";  //WRITEDATE
                        SQL = SQL + ComNum.VBLF + "'" + strCurTime + "',";  //WRITETIME
                        SQL = SQL + ComNum.VBLF + "'" + clsType.User.IdNumber + "'";    //WRITEUSEID
                        SQL = SQL + ComNum.VBLF + ")";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            ComFunc.MsgBoxEx(this, SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return false;
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                return true;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// AEMRCHRATMST, AEMRCHARTROW 저장을 한다
        /// </summary>
        /// <returns></returns>
        private bool SaveAEMRCHARTMSTandROW()
        {
            //DataTable Dt = null;
            //int i = 0;
            string SQL = "";
            //string SqlErr = "";
            //int intRowAffected = 0;


            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT ";
                SQL = SQL + ComNum.VBLF + "    ACTTIME, SUM(ACTQTY) AS ACTQTY ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "AEMRBIOFLUID ";
                SQL = SQL + ComNum.VBLF + "WHERE ACPNO = " + p.acpNo;
                SQL = SQL + ComNum.VBLF + "    AND ACTDATE = '" + dtpOrderDate.Value.ToString("yyyyMMdd") + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DCCLS = '1' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY ACTTIME ";
                SQL = SQL + ComNum.VBLF + "ORDER BY ACTTIME ";




                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("삭제하였습니다.");
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        #endregion //함수

        
    }
}

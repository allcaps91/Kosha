using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using System;
using System.Data;
using System.Windows.Forms;
using System.Collections.Generic;

/// <summary>
/// Description : 반환값 없는 공용쿼리문
/// Author : 박병규, 김민철
/// Create Date : 2017.06.21
/// </summary>
/// <history>
/// </history>

namespace ComPmpaLibB
{
    public  class clsPmpaQuery
    {
        //clsUser CU = new clsUser();
        //ComFunc CF = new ComFunc();
        //clsPmpaFunc CPF = new clsPmpaFunc();
        //clsPmpaPb CPP = new clsPmpaPb();


        /// <summary>
        /// Description : 외래,응급실 주상병코드를 가져오기
        /// Author : 박병규
        /// Create Date : 2017.06.20 
        /// <param name="ArgPtno">등록번호</param>
        /// <param name="ArgBdate">발생일자</param>
        /// <param name="ArgDeptCode">진료과목</param>
        /// </summary>
        /// <seealso cref="vb의료급여승인 : GET_OPD_BOHO_MYSM"/> 
        public string GET_OPD_MYSM(PsmhDb pDbCon, string ArgPtno, string ArgBdate, string ArgDeptCode)
        {
            DataTable DtPa = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string rtnVal = string.Empty;

            SQL = "";
            if (clsPmpaType.BAT.DeptCode == "ER")
            {
                SQL += ComNum.VBLF + " SELECT ILLCODE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_EILLS ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PTNO      = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND ENTDATE   = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDeptCode + "' ";
            }
            else
            {
                SQL += ComNum.VBLF + " SELECT ILLCODE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OILLS ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PTNO      = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgBdate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDeptCode + "' ";
                SQL += ComNum.VBLF + "  ORDER BY SEQNO  ";
            }
            SqlErr = clsDB.GetDataTable(ref DtPa, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return "";
            }

            if (DtPa.Rows.Count == 0)
            {
                DtPa.Dispose();
                DtPa = null;

                //물리치료 상병명
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ILLCODE1 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_PTMASTER ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDeptCode + "' ";
                SqlErr = clsDB.GetDataTable(ref DtPa, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return "";
                }

                if (DtPa.Rows.Count == 0)
                {
                    DtPa.Dispose();
                    DtPa = null;
                    return "";
                }

                rtnVal = DtPa.Rows[0]["ILLCODE1"].ToString().Trim();

                DtPa.Dispose();
                DtPa = null;

                return rtnVal;
            }

            rtnVal = DtPa.Rows[0]["ILLCODE"].ToString().Trim();

            DtPa.Dispose();
            DtPa = null;

            return rtnVal;
        }
        
        /// <summary>
        /// Description : 원무접수권한(대리접수, 접수2)
        /// Author : 박병규
        /// Create Date : 2017.07.06 
        /// </summary>
        /// <seealso cref="OPD_세계병자의날.bas : 원무접수권한_대리접수2"/> 
        public void JupsuAuth(PsmhDb pDbCon)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";

            string strSabun;

            clsPmpaPb.GstrJupsuAuth = "";

            strSabun = clsPublic.GstrJobSabun;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT CODE";
            SQL += ComNum.VBLF + "   FROM ADMIN.BAS_BCODE";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND GUBUN  = '원무접수제한자'";
            SQL += ComNum.VBLF + "    AND CODE   = '" + strSabun + "'";
            SQL += ComNum.VBLF + "    AND (DELDATE >= TRUNC(SYSDATE) OR DELDATE IS NULL) ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);

            if (Dt.Rows.Count > 0)
            {
                clsPmpaPb.GstrJupsuAuth = "Y";
            }

            Dt.Dispose();
            Dt = null;
        }
        
        /// <summary>
        /// Description : 의료급여승인 내역 Data Insert
        /// CARD_APPROV_BI(의료급여승인 내역 관리) 테이블에 내역 기록
        /// Create Date : 2017.07.07
        /// Author : 김민철
        /// </summary>
        /// <param name="strPano">등록번호</param>
        /// <param name="strBi">자격</param>
        /// <param name="strDept">진료과</param>
        /// <param name="strOutDate">퇴원일자(없으면 NULL)</param>
        /// <param name="strGubun">승인 01, 취소 02</param>
        /// <param name="lngAmt">승인금액</param>
        /// <param name="strMSeqNo">승인번호</param>
        /// <param name="strCanDate">취소일자</param>
        /// <param name="strIO">입원 I, 외래 O</param>
        /// <param name="lngReAmt">취소금액</param>
        /// <seealso cref="Frm의료급여퇴원승인.frm : BOHO_ApproveAmt_Log"/> 
        public void Card_Approv_Bi_Insert(PsmhDb pDbCon, string strPano, string strBi, string strDept, string strOutDate, string strGubun, long lngAmt, string strMSeqNo, string strCanDate, string strIO, long lngReAmt)
        {
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            if (strGubun == "02")
            {
                lngAmt = lngAmt * -1;
                lngReAmt = lngReAmt * -1;
            }

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "CARD_APPROV_BI ";
                SQL += ComNum.VBLF + "        (ActDate, Pano, Bi, ";
                SQL += ComNum.VBLF + "         DeptCode, Part, BDate, ";
                SQL += ComNum.VBLF + "         Gubun, Amt, MSeqNo, ";
                SQL += ComNum.VBLF + "         EntDate, CanDate, GBIO, ";
                SQL += ComNum.VBLF + "         GBBUN,AMT1 )";
                SQL += ComNum.VBLF + " VALUES (TRUNC(SYSDATE), ";
                SQL += ComNum.VBLF + "         '" + strPano + "', ";
                SQL += ComNum.VBLF + "         '" + strBi + "', ";
                SQL += ComNum.VBLF + "         '" + strDept + "', ";

                if (clsPmpaPb.GstrIOGubun == "I")
                    SQL += ComNum.VBLF + "         '#', ";
                else
                    SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + ", ";

                SQL += ComNum.VBLF + "         TO_DATE('" + strOutDate + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '" + strGubun + "', ";
                SQL += ComNum.VBLF + "         " + lngAmt + ", ";
                SQL += ComNum.VBLF + "         '" + strMSeqNo + "',";
                SQL += ComNum.VBLF + "         SYSDATE, ";
                SQL += ComNum.VBLF + "         TO_DATE('" + strCanDate + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         '" + strIO + "',";
                if (lngReAmt != 0)
                {
                    SQL += ComNum.VBLF + " '2'," + lngReAmt + ")";
                }
                else
                {
                    SQL += ComNum.VBLF + " '1', 0)";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return;
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return;
            }
        }

        /// <summary>
        /// Description : DRG 자격 MASTER 변경작업
        /// Create Date : 2017.07.17
        /// Author : 김민철
        /// </summary>
        /// <param name="nIPDNO">입원번호</param>
        /// <param name="nOLDTRSNO">이전 자격번호</param>
        /// <param name="nNEWTRSNO">신규 자격번호</param>
        /// <seealso cref="Frm퇴원취소.frm : New_DRG_MASTER_INSERT"/> 
        public bool DRG_MASTER_NEW_INSERT(PsmhDb pDbCon, long nIPDNO, long nOLDTRSNO, long nNEWTRSNO)
        {
            DataTable Dt = new DataTable();
            string SQL = "";
            string SqlErr = "";
            int intRowCnt = 0;

            bool rtnVal = true;

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT ROWID FROM " + ComNum.DB_PMPA + "DRG_MASTER_NEW ";
                SQL += ComNum.VBLF + "  WHERE IPDNO =" + nIPDNO + " ";
                SQL += ComNum.VBLF + "    AND TRSNO =" + nOLDTRSNO + " ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return false;
                }
                
                if (Dt.Rows.Count > 0)
                {
                    //2021-11-02 접수권한인데 엉뚱한데 사용됨.  주석처리
                    //clsPmpaPb.GstrJupsuAuth = "Y";
                    //PS. VB6 루틴은 ROWCOUNT 값이 없을 경우 해당 루틴 EXIT 처리함.(컨버전 오류)
                    //    ROWCOUNT 여부와 상관없이 값이 없으면 하단 INSERT 쿼리 작동안하니 그냥 놔둬도 큰 의미 없을 듯.
                }

                Dt.Dispose();
                Dt = null;

                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "DRG_MASTER_NEW (";
                SQL += ComNum.VBLF + " TRSNO,IPDNO,PANO,SNAME,JINDATE,ILSU,ILLCODE1,ILLCODE2,ILLCODE3,ILLCODE4,";
                SQL += ComNum.VBLF + " ILLCODE5,ILLCODE6,ILLCODE7,ILLCODE8,ILLCODE9,ILLCODE10,OPCODE1,OPCODE2,OPCODE3,";
                SQL += ComNum.VBLF + " OPCODE4,OPCODE5,OPCODE6,OPCODE7,OPCODE8,OPCODE9,OPCODE10,EXCODE1,EXCODE2,EXCODE3,";
                SQL += ComNum.VBLF + " EXCODE4,EXCODE5,XRCODE1,XRCODE2,XRCODE3,XRCODE4,XRCODE5,JCODE1,JCODE2,JCODE3,";
                SQL += ComNum.VBLF + " JCODE4,JCODE5,MCODE1,MCODE2,MCODE3,MCODE4,MCODE5,BCODE1,BCODE2,BCODE3,BCODE4,";
                SQL += ComNum.VBLF + " BCODE5,ACODE,WEIGHT,CPR,MDCCODE,DRGCODE,MDCCODEOLD,DRGCODEOLD,ENTDATE1,ENTDATE2,";
                SQL += ComNum.VBLF + " ENTSABUN1,ENTSABUN2)";
                SQL += ComNum.VBLF + " SELECT " + nNEWTRSNO + ",IPDNO,PANO,SNAME,JINDATE,ILSU,ILLCODE1,ILLCODE2,ILLCODE3,ILLCODE4,";
                SQL += ComNum.VBLF + " ILLCODE5,ILLCODE6,ILLCODE7,ILLCODE8,ILLCODE9,ILLCODE10,OPCODE1,OPCODE2,OPCODE3,";
                SQL += ComNum.VBLF + " OPCODE4,OPCODE5,OPCODE6,OPCODE7,OPCODE8,OPCODE9,OPCODE10,EXCODE1,EXCODE2,EXCODE3,";
                SQL += ComNum.VBLF + " EXCODE4,EXCODE5,XRCODE1,XRCODE2,XRCODE3,XRCODE4,XRCODE5,JCODE1,JCODE2,JCODE3,";
                SQL += ComNum.VBLF + " JCODE4,JCODE5,MCODE1,MCODE2,MCODE3,MCODE4,MCODE5,BCODE1,BCODE2,BCODE3,BCODE4,";
                SQL += ComNum.VBLF + " BCODE5,ACODE,WEIGHT,CPR,MDCCODE,DRGCODE,MDCCODEOLD,DRGCODEOLD,ENTDATE1,ENTDATE2, ";
                SQL += ComNum.VBLF + " ENTSABUN1 , ENTSABUN2";
                SQL += ComNum.VBLF + "  From " + ComNum.DB_PMPA + "DRG_MASTER_NEW";
                SQL += ComNum.VBLF + " WHERE IPDNO =" + nIPDNO + " ";
                SQL += ComNum.VBLF + "   AND TRSNO =" + nOLDTRSNO + " ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return false;
                }

                return rtnVal;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return false;
            }
            
        }

        /// <summary>
        /// Description : 심사작업 HISTORY 관리
        /// Create Date : 2017.07.17
        /// Author : 김민철
        /// </summary>
        /// <param name="Argflag"></param>
        /// <param name="argGBN"></param>
        /// <param name="ArgPano"></param>
        /// <param name="ArgIpdNo"></param>
        /// <param name="ArgTRSNO"></param>
        /// <param name="ArgGBSTS"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgInDate"></param>
        /// <param name="ArgOutDate"></param>
        /// <param name="ArgSName"></param>
        /// <param name="ArgArcDate"></param>
        /// <seealso cref="IUMENT.bas : Simsa_History_SAVE"/> 
        public void Simsa_History_SAVE(PsmhDb pDbCon, string Argflag, string argGBN, string ArgPano, long ArgIpdNo, long ArgTRSNO, string ArgGBSTS, string ArgBi, string ArgInDate, string ArgOutDate, string ArgSName, string ArgArcDate)
        {
            string strSname = string.Empty;
            string strVCode = string.Empty;
            string strOGPDBun = string.Empty;
            string strOgPdBun2 = string.Empty;
            string strOGPDBundtl = string.Empty;

            long nAmt50 = 0;
            long nAmt51 = 0;
            long nAMT53 = 0;
            long nAMT54 = 0;
            long nAmt55 = 0;

            DataTable Dt = new DataTable();
            string SQL = string.Empty;            
            string SqlErr = "";
            int intRowCnt = 0;

            ComFunc CF = new ComFunc();

            PsmhDb NewCon = clsDB.DBConnect();

            try
            {
                strSname = CF.Read_SabunName(pDbCon, clsType.User.Sabun);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT VCODE,OGPDBUN,OGPDBUN2,OGPDBUNDTL,AMT50,AMT51,AMT53,AMT54,AMT55 ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + " WHERE IPDNO = " + ArgIpdNo + " ";
                SQL += ComNum.VBLF + "   AND TRSNO = " + ArgTRSNO + " ";
                SQL += ComNum.VBLF + " ORDER BY GBSTS ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, NewCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(NewCon);
                    NewCon.DisDBConnect();
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    strVCode = Dt.Rows[0]["VCODE"].ToString().Trim();
                    strOGPDBun = Dt.Rows[0]["OGPDBUN"].ToString().Trim();
                    strOgPdBun2 = Dt.Rows[0]["OGPDBUN2"].ToString().Trim();
                    strOGPDBundtl = Dt.Rows[0]["OGPDBUNDTL"].ToString().Trim();

                    nAmt50 = Convert.ToInt32(Dt.Rows[0]["AMT50"].ToString());
                    nAmt51 = Convert.ToInt32(Dt.Rows[0]["AMT51"].ToString());
                    nAMT53 = Convert.ToInt32(Dt.Rows[0]["AMT53"].ToString());
                    nAMT54 = Convert.ToInt32(Dt.Rows[0]["AMT54"].ToString());
                    nAmt55 = Convert.ToInt32(Dt.Rows[0]["AMT55"].ToString());
                }

                Dt.Dispose();
                Dt = null;

                switch (argGBN)
                {
                    case "심사완료":
                        SQL = "";
                        SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "IPD_SIMSA_HIS (  ";
                        SQL += ComNum.VBLF + " IPDNO,TRSNO,PANO,BI,INDATE,OUTDATE,SNAME,GbSTS,Flag,SIMSA_OK,SIMSA_SNAME,SIMSA_SABUN,ENTDATE,";
                        SQL += ComNum.VBLF + " ArcDate,VCODE,OGPDBUN,OGPDBUN2,OGPDBUNDTL,AMT50,AMT51,AMT53,AMT54,AMT55 ) VALUES ( ";
                        SQL += ComNum.VBLF + ArgIpdNo + "," + ArgTRSNO + ",'" + ArgPano + "','" + ArgBi + "', ";
                        SQL += ComNum.VBLF + " TO_DATE('" + ArgInDate + "','YYYY-MM-DD') , TO_DATE('" + ArgOutDate + "','YYYY-MM-DD') , ";
                        if (Argflag.Equals("Y"))
                        {
                            SQL += ComNum.VBLF + " '" + ArgSName + "','" + ArgGBSTS + "','Y', ";
                        }
                        else
                        {
                            SQL += ComNum.VBLF + " '" + ArgSName + "','" + ArgGBSTS + "','', ";
                        }
                        SQL = SQL + " SYSDATE,'" + strSname + "', " + clsType.User.IdNumber + ",SYSDATE,TO_DATE('" + ArgArcDate + "','YYYY-MM-DD'),";
                        SQL = SQL + " '" + strVCode + "','" + strOGPDBun + "','" + strOgPdBun2 + "','" + strOGPDBundtl + "',";
                        SQL = SQL + " " + nAmt50 + "," + nAmt51 + "," + nAMT53 + "," + nAMT54 + "," + nAmt55 + ") ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, NewCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(NewCon);
                            NewCon.DisDBConnect();
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            return;
                        }

                        break;

                    case "심사취소":
                        SQL = "";
                        SQL += ComNum.VBLF + " INSERT INTO  ADMIN.IPD_SIMSA_HIS ( IPDNO,TRSNO,PANO,BI,INDATE,OUTDATE,SNAME,GbSTS,Flag,";
                        SQL += ComNum.VBLF + "  SIMSA_NO,SIMSA_SNAME,SIMSA_SABUN,ENTDATE,ArcDate,VCODE,OGPDBUN,OGPDBUN2,OGPDBUNDTL,";
                        SQL += ComNum.VBLF + "  AMT50,AMT51,AMT53,AMT54,AMT55 ) VALUES (      ";
                        SQL += ComNum.VBLF + "  " + ArgIpdNo + "," + ArgTRSNO + ",'" + ArgPano + "','" + ArgBi + "',     ";
                        SQL += ComNum.VBLF + " TO_DATE('" + ArgInDate + "','YYYY-MM-DD') , TO_DATE('" + ArgOutDate + "','YYYY-MM-DD') , ";
                        SQL += ComNum.VBLF + " '" + ArgSName + "','" + ArgGBSTS + "','', ";
                        SQL += ComNum.VBLF + " SYSDATE,'" + strSname + "'," + clsType.User.IdNumber + ",SYSDATE,TO_DATE('" + ArgArcDate + "','YYYY-MM-DD'),";
                        SQL += ComNum.VBLF + " '" + strVCode + "','" + strOGPDBun + "','" + strOgPdBun2 + "','" + strOGPDBundtl + "',";
                        SQL += ComNum.VBLF + " " + nAmt50 + "," + nAmt51 + "," + nAMT53 + "," + nAMT54 + "," + nAmt55 + ") ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, NewCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(NewCon);
                            NewCon.DisDBConnect();
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            return;
                        }

                        break;
                }

                NewCon.DisDBConnect();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(NewCon);
                NewCon.DisDBConnect();
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
            
        }
        
        /// <summary>
        /// Description : 포스코예약 공지사항 저장
        /// Author : 박병규
        /// Create Date : 2017.08.03
        /// <param name="ArgGb"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgMsg"></param>
        /// </summary>
        /// <seealso cref="frmPoscoResMain.frm:포스코예약_공지사항"/>
        public void UPDATE_POSCO_MSG(PsmhDb pDbCon, string ArgGb, string ArgGubun, string ArgMsg)
        {
            DataTable DtMst = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string strRowid = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO_MSG ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + ArgGb + "' ";
            SQL += ComNum.VBLF + "    AND GUBUN = '" + ArgGubun + "' ";
            SqlErr = clsDB.GetDataTable(ref DtMst, SQL, pDbCon);

            if (DtMst.Rows.Count > 0)
                strRowid = DtMst.Rows[0]["ROWID"].ToString().Trim();

            DtMst.Dispose();
            DtMst = null;


            if (strRowid != "")
            {
                
                clsDB.setBeginTran(pDbCon);

                try
                {
                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT_POSCO_MSG  ";
                    SQL += ComNum.VBLF + "    SET BIGO      = '" + ArgMsg + "' ";
                    SQL += ComNum.VBLF + "   WHERE ROWID    = '" + strRowid + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(pDbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                        return;
                    }

                    clsDB.setCommitTran(pDbCon);

                    ComFunc.MsgBox("전체 공지사항내용이 저장되었습니다");

                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                    return;
                }
            }
        }
        
        /// <summary>
        /// Description : 전화예약접수 MASTER
        /// Author : 박병규
        /// Create Date : 2017.08.04
        /// <param name="ArgPtno"></param>
        /// <param name="ArgSname"></param>
        /// <param name="ArgJumin"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgSdate"></param>
        /// <param name="ArgEdate"></param>
        /// <param name="ArgJcode"></param>
        /// </summary>
        public void TELRESV_INSERT(PsmhDb pDbCon, string ArgPtno, string ArgSname, string ArgDept, string ArgDate, string ArgTime, string ArgGbn, string ArgGkiho, string ArgRemark)
        {
            clsPmpaFunc CPF = new clsPmpaFunc();

            DataTable DtMst = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;
            string strRowid = string.Empty;
            string strDrCode = string.Empty;
            
            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_TELRESV ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND RDATE     = TO_DATE('" + ArgDate + "', 'YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTable(ref DtMst, SQL, pDbCon);

            if (DtMst.Rows.Count > 0)
                strRowid = DtMst.Rows[0]["ROWID"].ToString().Trim();

            DtMst.Dispose();
            DtMst = null;

            strDrCode = CPF.READ_DOCTOR_SCHEDULE(pDbCon, ArgDept, ArgDate, ArgTime);
            

            
            clsDB.setBeginTran(pDbCon);

            try
            {

                SQL = "";
                if (strRowid == "")
                {
                    SQL += ComNum.VBLF + "INSERT INTO OPD_TELRESV ";
                    SQL += ComNum.VBLF + "       (RDate, RTime, Pano, ";
                    SQL += ComNum.VBLF + "        SName, DeptCode, Drcode, ";
                    SQL += ComNum.VBLF + "        EntDate, EntSabun, GbChojin, ";
                    SQL += ComNum.VBLF + "        GbFlag, GbSPC, GwaChoJae, ";
                    SQL += ComNum.VBLF + "        GKiho, Gubun, P_Exam, ";
                    SQL += ComNum.VBLF + "        P_Remark) ";
                    SQL += ComNum.VBLF + "VALUES (TO_DATE('" + ArgDate + "','YYYY-MM-DD'),";
                    SQL += ComNum.VBLF + "        '" + ArgTime + "', ";
                    SQL += ComNum.VBLF + "        '" + ArgPtno + "',";
                    SQL += ComNum.VBLF + "        '" + ArgSname + "', ";
                    SQL += ComNum.VBLF + "        '" + ArgDept + "',";
                    SQL += ComNum.VBLF + "        '" + strDrCode + "', ";
                    SQL += ComNum.VBLF + "        SYSDATE , ";
                    SQL += ComNum.VBLF + "        '6666', ";
                    SQL += ComNum.VBLF + "        'Y', ";
                    SQL += ComNum.VBLF + "        'N', ";
                    SQL += ComNum.VBLF + "        '0', ";
                    SQL += ComNum.VBLF + "        'C', ";
                    SQL += ComNum.VBLF + "        '" + ArgGkiho + "', ";
                    SQL += ComNum.VBLF + "        '02', ";
                    SQL += ComNum.VBLF + "        '" + ArgGbn + "', ";
                    SQL += ComNum.VBLF + "        '" + ArgRemark + "' ) ";
                }
                else
                {
                    SQL += ComNum.VBLF + "UPDATE OPD_TELRESV ";
                    SQL += ComNum.VBLF + "   SET RDate      = TO_DATE('" + ArgDate + "','YYYY-MM-DD'),";
                    SQL += ComNum.VBLF + "       RTime      = '" + ArgTime + "', ";
                    SQL += ComNum.VBLF + "       SName      = '" + ArgSname + "', ";
                    SQL += ComNum.VBLF + "       DeptCode   = '" + ArgDept + "', ";
                    SQL += ComNum.VBLF + "       Drcode     = '" + strDrCode + "', ";
                    SQL += ComNum.VBLF + "       EntDate    = SYSDATE, ";
                    SQL += ComNum.VBLF + "       EntSabun   = '6666', ";
                    SQL += ComNum.VBLF + "       GbChojin   = 'Y', ";
                    SQL += ComNum.VBLF + "       GbFlag     = 'N', ";
                    SQL += ComNum.VBLF + "       GbSPC      = '0', ";
                    SQL += ComNum.VBLF + "       GwaChoJae  = 'C', ";
                    SQL += ComNum.VBLF + "       Gubun      = '02', ";
                    SQL += ComNum.VBLF + "       P_Exam     = '" + ArgGbn + "', ";
                    SQL += ComNum.VBLF + "       P_Remark   = '" + ArgRemark + "', ";
                    SQL += ComNum.VBLF + "       GKiho      = '" + ArgGkiho + "' ";
                    SQL += ComNum.VBLF + " Where ROWID      = '" + strRowid + "' ";
                }
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return;
                }

                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return;
            }
        }
        
        /// <summary>
        /// Description : 가정의학과 예약 변경
        /// Author : 박병규
        /// Create Date : 2017.08.30
        /// <param name="ArgSname"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgGubun"></param>
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <seealso cref="Opd_FM_Resv.bas:UPDATE_FM_RESV"/>
        public void UPDATE_FM_RESV(PsmhDb pDbCon, string ArgSname, string ArgDept, string ArgGubun, string ArgDate)
        {
            DataTable DtMst = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "ETC_FM_RESV ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND RDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND SNAME     = '" + ArgSname + "' ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";
            SQL += ComNum.VBLF + "    AND GUBUN = '0' ";
            SqlErr = clsDB.GetDataTable(ref DtMst, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (DtMst.Rows.Count == 0)
            {
                DtMst.Dispose();
                DtMst = null;
                return;
            }

            DtMst.Dispose();
            DtMst = null;

            
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "ETC_FM_RESV ";

                if (ArgGubun.Equals("D"))
                    SQL += ComNum.VBLF + "    SET DELDATE = TRUNC(SYSDATE) ";
                else
                    SQL += ComNum.VBLF + "    SET GUBUN = '" + ArgGubun + "' ";

                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND SNAME     = '" + ArgSname + "' ";
                SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
                SQL += ComNum.VBLF + "    AND RDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    return;
                }

                clsDB.setCommitTran(pDbCon);
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon);
                return;
            }
        }
        
        /// <summary>
        /// Description : 감액구분 부담율
        /// Author : 박병규
        /// Create Date : 2017.08.31
        /// <param name="ArgJumin"></param>
        /// <param name="ArgDate"></param>
        /// </summary>
        /// <history>
        /// 감액코드 25 한얼시용자 -> 병원시용자로 같이 사용
        /// 감액코드 26 한얼직원-> 병원직원과 동일한 감액율 적용
        /// 감액코드 27 한얼가족-> 병원직원 직계존비속과 동일한 감액율 적용
        /// 공통사항 : 입원/외래 동일한 기준이며, 입원자는 2013-12-01일부 입원자에 한함.
        /// 치과 감액은 별도의 적용이 없음.
        /// </history>
        /// <seealso cref="OUMSAD.bas:READ_BAS_GAMF"/>
        public void READ_BAS_GAMF(PsmhDb pDbCon, string ArgJumin, string ArgDate = "")
        {
            DataTable DtMst = new DataTable();
            DataTable DtSub = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intSabun = 0;
            string strSabun = string.Empty;

            if (ArgDate == "") { ArgDate = clsPublic.GstrSysDate; }
            clsPmpaPb.GstrFlagGam = "NO";
            clsPmpaPb.GstrGamGubun = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT GAMCODE, GamMessage,                      --감액구분, 감액대상자내용";
            SQL += ComNum.VBLF + "        TO_CHAR(GamEnd, 'YYYY-MM-DD') GamEnd,     --감액적용종료일자";
            SQL += ComNum.VBLF + "        TO_CHAR(GamEnter, 'YYYY-MM-DD') GamENTER  --입사일자";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMF ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND GamJumin3 = '" + clsAES.AES(ArgJumin) + "' ";
            SQL += ComNum.VBLF + "    AND (GAMEND >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') OR GAMEND IS NULL) ";
            SqlErr = clsDB.GetDataTable(ref DtMst, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (DtMst.Rows.Count > 0)
            {
                if (DtMst.Rows[0]["GAMENTER"].ToString().Trim() != "")
                {
                    if (string.Compare(DtMst.Rows[0]["GAMENTER"].ToString().Trim(), ArgDate) > 0)
                    {
                        clsPmpaPb.GstrFlagGam = "NO";
                        clsPmpaPb.GstrGamGubun = "";
                        clsPmpaPb.GstrGamMsg = "";
                    }
                    else
                    {
                        clsPmpaPb.GstrGamEnd = DtMst.Rows[0]["GamEnd"].ToString().Trim();
                        clsPmpaPb.GstrFlagGam = "OK";
                        clsPmpaPb.GstrGamGubun = DtMst.Rows[0]["GAMCODE"].ToString().Trim();
                        clsPmpaPb.GstrGamMsg = DtMst.Rows[0]["GamMessage"].ToString().Trim();
                    }
                }
                else
                {
                    clsPmpaPb.GstrGamEnd = DtMst.Rows[0]["GamEnd"].ToString().Trim();
                    clsPmpaPb.GstrFlagGam = "OK";
                    clsPmpaPb.GstrGamGubun = DtMst.Rows[0]["GAMCODE"].ToString().Trim();
                    clsPmpaPb.GstrGamMsg = DtMst.Rows[0]["GamMessage"].ToString().Trim();
                }

                DtMst.Dispose();
                DtMst = null;
            }
            else
            {
                DtMst.Dispose();
                DtMst = null;

                SQL = "";
                SQL += ComNum.VBLF + " SELECT IPSADAY, KORNAME, TO_CHAR(TOIDAY, 'YYYY-MM-DD') TOIDAY ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_ERP + "INSA_MST ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND (Jumin  = '" + ArgJumin + "' OR Jumin3  = '" + clsAES.AES(ArgJumin) + "' ) ";
                SQL += ComNum.VBLF + "    AND TOIDAY    < TRUNC(SYSDATE)  ";
                SQL += ComNum.VBLF + "    AND SABUN     < '60000' ";
                SQL += ComNum.VBLF + "  UNION ALL ";
                SQL += ComNum.VBLF + " SELECT JDATE, NAME KORNAME, TO_CHAR(JDATE, 'YYYY-MM-DD') TOIDAY ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_BCODE ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND GUBUN     = '원무강제퇴사자감액' ";
                SQL += ComNum.VBLF + "    AND TRIM(CODE) = '" + ArgJumin + "' ";
                SQL += ComNum.VBLF + "  ORDER BY 1  DESC  ";
                SqlErr = clsDB.GetDataTable(ref DtMst, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (DtMst.Rows.Count > 0)
                {
                    clsPmpaPb.GstrGamEnd = "";
                    clsPmpaPb.GstrFlagGam = "OK";
                    clsPmpaPb.GstrGamGubun = "42";
                    clsPmpaPb.GstrGamMsg = "<" + DtMst.Rows[0]["KORNAME"].ToString().Trim() + "> 퇴사자";

                    DtMst.Dispose();
                    DtMst = null;
                }
                else
                {
                    DtMst.Dispose();
                    DtMst = null;

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT GAMPANO ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMFSINGA ";
                    SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                    SQL += ComNum.VBLF + "    AND GAMJUMIN_new  = '" + clsAES.AES(ArgJumin) + "' ";
                    SqlErr = clsDB.GetDataTable(ref DtMst, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    if (DtMst.Rows.Count > 0)
                    {
                        clsPmpaPb.GstrGamEnd = "";
                        clsPmpaPb.GstrFlagGam = "OK";
                        clsPmpaPb.GstrGamGubun = "51";
                        clsPmpaPb.GstrGamMsg = "신자감액";
                    }

                    DtMst.Dispose();
                    DtMst = null;
                }
            }

            //감액퇴사 점검
            SQL = "";
            SQL += ComNum.VBLF + " SELECT GAMSABUN ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_GAMF ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND GamJumin3  = '" + clsAES.AES(ArgJumin) + "' ";
            SQL += ComNum.VBLF + "    AND (GAMEND >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') OR GAMEND IS NULL) ";
            SQL += ComNum.VBLF + "    AND GAMSABUN IS NOT NULL ";
            SqlErr = clsDB.GetDataTable(ref DtMst, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (DtMst.Rows.Count > 0)
            {
                intSabun = Convert.ToInt32(DtMst.Rows[0]["GAMSABUN"].ToString().Trim());

                if (intSabun >= 600000)
                    strSabun = string.Format("{0:000000}", intSabun);
                else
                    strSabun = string.Format("{0:00000}", intSabun);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT SABUN ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_ERP + "INSA_MST ";
                SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
                SQL += ComNum.VBLF + "    AND SABUN = '" + strSabun + "' ";
                SQL += ComNum.VBLF + "    AND (TOIDAY >= TO_DATE('" + ArgDate + "','YYYY-MM-DD') OR TOIDAY IS NULL) ";
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (DtSub.Rows.Count == 0)
                {
                    clsPublic.GstrMsgTitle = "감액확인";
                    clsPublic.GstrMsgList = "감액등록을 확인바랍니다." + '\r';
                    clsPublic.GstrMsgList += "사원번호 : " + strSabun + '\r';
                    clsPublic.GstrMsgList += "은 퇴사 처리되었습니다." + '\r' + '\r';

                    ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                }

                DtSub.Dispose();
                DtSub = null;
            }

            DtMst.Dispose();
            DtMst = null;

            return;
        }

        /// <summary>
        /// Description : 성명+주민1 같은환자 메세지박스
        /// author : 박병규
        /// Create Date : 2017-10-19
        /// <param name=""></param>
        /// <seealso cref=""/>
        /// </summary>
        public void READ_PATIENT_OVERLAP(PsmhDb pDbCon)
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(COUNT(*), 0) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND Pano      <> '" + clsPmpaType.TBP.Ptno + "' ";
            SQL += ComNum.VBLF + "    AND SNAME     = '" + clsPmpaType.TBP.Sname + "' "; //이중차트 점검시 성명은 무시
            SQL += ComNum.VBLF + "    AND Jumin1    = '" + clsPmpaType.TBP.Jumin1 + "' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("동일 성명 및 주민번호 조회오류");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (Convert.ToInt32(DtQ.Rows[0]["CNT"].ToString()) > 0)
                    ComFunc.MsgBox("성명과 주민등록번호 앞자리 동일인이 " + Convert.ToInt32(DtQ.Rows[0]["CNT"].ToString()) + " 건 존재합니다.", "주민번호 확인요망");
            }

            DtQ.Dispose();
            DtQ = null;

            return;
        }

        /// <summary>
        /// Description : 신종플루 체크
        /// author : 박병규
        /// Create Date : 2017-10-19
        /// <param name="ArgPtno"></param>
        /// <param name="ArgJumin1"></param>
        /// <param name="ArgJumin2"></param>
        /// <seealso cref=""/>
        /// </summary>
        public void READ_INFLUE_CHECK(PsmhDb pDbCon, string ArgPtno, string ArgJumin1, string ArgJumin2)
        {
            clsPmpaFunc CPF = new clsPmpaFunc();

            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;

            strMsg = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Pano ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "EXAM_INFECTMASTER                    --혈액성 감염환자관리";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND PANO = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND (INFLUAG IS NOT NULL Or INFLUAPR IS NOT NULL ) ";
            SQL += ComNum.VBLF + "    AND PANO NOT IN (SELECT PANO ";
            SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "ETC_INFLU_OK ";
            SQL += ComNum.VBLF + "                      WHERE (DELDATE IS NULL OR DELDATE ='') )";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("신종플루체크 조회오류");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (DtQ.Rows.Count > 0)
                ComFunc.MsgBox("해당 환자는 인플렌자 양성환자임. 응급실(ER)로 접수요망.", "확인");

            DtQ.Dispose();
            DtQ = null;

            strMsg = CPF.READ_FLUE_RESERVED(pDbCon, ArgJumin1 + ArgJumin2, "1");
            if (strMsg != "")
                ComFunc.MsgBox(strMsg + '\r' + '\r' + "예방접종시 제외됨(접종당일 포함)", "알림");

            return;
        }
        
        /// <summary>
        /// Description : 외래전화예약접수 체크
        /// author : 박병규
        /// Create Date : 2017-10-19
        /// <param name=""></param>
        /// <seealso cref="ArgPtno"/>
        /// </summary>
        public void READ_TELRESV_CHECK(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable DtQ = new DataTable();
            DataTable DtQSub = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;

            strMsg = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DeptCode RDept, RTime, DrCode,            --진료과,예약시간,의사코드";
            SQL += ComNum.VBLF + "        ADMIN.FC_BAS_DOCTOR_DRNAME(DRCODE) DRNAME, ";
            SQL += ComNum.VBLF + "        GbFlag                                    --외래스테이션용-접수(Y)";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_TELRESV         --전화예약접수마스터";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND Pano  = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND RDate = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "  ORDER BY RTime ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("전화예약접수 Display 오류");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (DtQ.Rows[0]["RDept"].ToString().Trim() != "" || DtQ.Rows[0]["RDept"].ToString().Trim() != null)
                {
                    if (DtQ.Rows[0]["GbFlag"].ToString().Trim() == "Y")
                        strMsg = "외래과전용 전화접수 환자임(";
                    else
                        strMsg = "기록실 전화접수 환자임(";

                    for (int i = 0; i < DtQ.Rows.Count; i++)
                    {
                        strMsg += DtQ.Rows[0]["RDept"].ToString().Trim() + " ";
                        strMsg += DtQ.Rows[0]["RTime"].ToString().Trim() + " ";
                        strMsg += "[의사코드:" + DtQ.Rows[0]["DrCode"].ToString().Trim() + " " + DtQ.Rows[0]["DRNAME"].ToString().Trim() + "]" + '\r' + '\r';
                        strMsg += "전화접수시 의사코드로 접수요망.(단, 처방이 발생된 경우 처방발생 의사코드로 접수요망)";

                        if ((i + 1) == DtQ.Rows.Count)
                            strMsg += ")";
                        else
                            strMsg += ",";
                    }

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT DEPTCODE, DRCODE, NVL(COUNT(*), 0) CNT, ";
                    SQL += ComNum.VBLF + "        ADMIN.FC_BAS_DOCTOR_DRNAME(DRCODE) DRNAME ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND PTNO      = '" + ArgPtno + "' ";
                    SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND GBSUNAP   = '0' ";
                    SQL += ComNum.VBLF + "    AND DrCode IS NOT NULL ";
                    SQL += ComNum.VBLF + "  GROUP BY DEPTCODE,DRCODE ";
                    SQL += ComNum.VBLF + " union all SELECT DEPTCODE, DRCODE, NVL(COUNT(*), 0) CNT, ";
                    SQL += ComNum.VBLF + "        ADMIN.FC_BAS_DOCTOR_DRNAME(DRCODE) DRNAME ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ORAN_SLIP ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
                    SQL += ComNum.VBLF + "    AND OPDate     = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND SLIPSEND   IS  NULL ";
                    SQL += ComNum.VBLF + "    AND DrCode IS NOT NULL ";
                    SQL += ComNum.VBLF + "  GROUP BY DEPTCODE,DRCODE ";
                    SqlErr = clsDB.GetDataTable(ref DtQSub, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다.");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    if (DtQSub.Rows.Count > 0)
                    {
                        strMsg += '\r' + '\r' + "◆ 처방정보 ◆" + '\r' + '\r';

                        DataTable DtOrder = new DataTable();

                        for (int i = 0; i < DtQSub.Rows.Count; i++)
                        {
                            SQL = "";
                            SQL += ComNum.VBLF + " SELECT DEPTCODE, DRCODE, SuCode ";
                            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
                            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                            SQL += ComNum.VBLF + "    AND PTNO      = '" + ArgPtno + "' ";
                            SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "    AND GBSUNAP   = '0' ";
                            SQL += ComNum.VBLF + "    AND DrCode IS NOT NULL ";
                            SQL += ComNum.VBLF + "    AND DeptCode  = '" + DtQSub.Rows[i]["DEPTCODE"].ToString() + "' ";
                            SQL += ComNum.VBLF + "  GROUP BY DEPTCODE,DRCODE,SuCode ";
                            SQL += ComNum.VBLF + " UNION ALL SELECT DEPTCODE, DRCODE, SuCode ";
                            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ORAN_SLIP ";
                            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
                            SQL += ComNum.VBLF + "    AND OPDate     = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                            SQL += ComNum.VBLF + "    AND SLIPSEND   IS  NULL ";
                            SQL += ComNum.VBLF + "    AND DrCode IS NOT NULL ";
                            SQL += ComNum.VBLF + "    AND DeptCode  = '" + DtQSub.Rows[i]["DEPTCODE"].ToString() + "' ";
                            SQL += ComNum.VBLF + "  GROUP BY DEPTCODE,DRCODE,SuCode ";
                            SqlErr = clsDB.GetDataTable(ref DtOrder, SQL, pDbCon);

                            if (SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다.");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                                return;
                            }

                            string strTemp = "";

                            if (DtOrder.Rows.Count > 0)
                            {
                                for (int j = 0; j < DtOrder.Rows.Count; j++)
                                {
                                    if (j > 5) { break; }
                                    strTemp += DtOrder.Rows[j]["SUCODE"].ToString();
                                }

                                strMsg += DtQSub.Rows[i]["DEPTCODE"].ToString() + " " + DtQSub.Rows[i]["DRCODE"].ToString() + " " + DtQSub.Rows[i]["DRNAME"].ToString() + " 처방 " + DtQSub.Rows[i]["CNT"].ToString() + "건 [" + strTemp + "]" + '\r';
                            }

                            DtOrder.Dispose();
                            DtOrder = null;
                        }
                    }
                    DtQSub.Dispose();
                    DtQSub = null;

                    ComFunc.MsgBox(strMsg, "확인");

                    strMsg = "";

                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT DEPTCODE, DRCODE, SuCode ";
                    SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_OORDER ";
                    SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                    SQL += ComNum.VBLF + "    AND PTNO      = '" + ArgPtno + "' ";
                    SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND GBSUNAP   = '0' ";
                    SQL += ComNum.VBLF + "    AND SuCode IN ( '$$21','$$11') "; //$$21:접수하고안오심, $$11:예약후진료안옴
                    SqlErr = clsDB.GetDataTable(ref DtQSub, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("$$21, $$11 조회중 오류발생");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        return;
                    }

                    if (DtQSub.Rows.Count > 0)
                    {
                        strMsg += "◆ 처방확인사항 ◆" + '\r' + '\r';
                        strMsg += "$$21→접수후 미도착, $$11→예약후 미도착" + '\r' + '\r';

                        for (int i = 0; i < DtQSub.Rows.Count; i++)
                        {
                            strMsg += DtQSub.Rows[i]["DEPTCODE"].ToString() + " " + DtQSub.Rows[i]["DRCODE"].ToString() + " " + DtQSub.Rows[i]["SUCODE"].ToString() + " 처방발생" + '\r';
                        }
                    }
                    DtQSub.Dispose();
                    DtQSub = null;

                    if (strMsg != "")
                        ComFunc.MsgBox(strMsg, "확인");
                }

            }

            DtQ.Dispose();
            DtQ = null;

            return;
        }

        /// <summary>
        /// Description : 예약부도자 환불금 체크
        /// author : 박병규
        /// Create Date : 2017-10-19
        /// <param name="ArgPtno"></param>
        /// <seealso cref=""/>
        /// </summary>
        public bool READ_REFUND_CHECK(PsmhDb pDbCon, string ArgPtno, string ArgGubun = "")
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;
            bool rtnVal = false;

            strMsg = "";

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TO_CHAR(YDATE1,'YYYY-MM-DD') YDATE,        --";
            SQL += ComNum.VBLF + "       DEPTCODE REDEPT,                           --진료과";
            SQL += ComNum.VBLF + "       TO_CHAR(ACTDATE,'YYYY-MM-DD') REACTDATE    --예약부도정리일자";
            SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "OPD_REFUND           --예약부도자환불금관리테이블";
            SQL += ComNum.VBLF + " WHERE 1      = 1 ";
            SQL += ComNum.VBLF + "   AND PANO   = '" + ArgPtno + "' ";

            if (ArgGubun != "")
            {
                SQL += ComNum.VBLF + "   AND Gubun  = '" + ArgGubun + "' ";
            }

            SQL += ComNum.VBLF + "   AND (RDATE IS NULL OR RDATE = '') ";
            SqlErr = clsDB.GetDataTableEx(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("예약부도자 환불금 체크오류");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                rtnVal = true;

                if (ArgGubun == "00")
                    strMsg = "예약부도자" + '\r';
                else
                    strMsg = "접수부도자" + '\r';


                for (int i = 0; i < DtQ.Rows.Count; i++)
                {
                    strMsg += "접수일자 : " + DtQ.Rows[i]["YDATE"].ToString().Trim() + '\r';
                    strMsg += "진료과목 : " + DtQ.Rows[i]["REDEPT"].ToString().Trim() + '\r';
                    strMsg += "정리일자 : " + DtQ.Rows[i]["REACTDATE"].ToString().Trim() + '\r' + '\r';
                }

                ComFunc.MsgBox(strMsg, "확인");
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 예약검사부도자 환불금 체크
        /// author : 박병규
        /// Create Date : 2017-10-19
        /// <param name="ArgPtno"></param>
        /// <seealso cref=""/>
        /// </summary>
        public bool READ_REFUND_EXAM_CHECK(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;
            bool rtnVal = false;

            strMsg = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(YDATE1,'YYYY-MM-DD') YDATE, ";
            SQL += ComNum.VBLF + "        DEPTCODE REDEPT, ";
            SQL += ComNum.VBLF + "        TO_CHAR(ACTDATE,'YYYY-MM-DD') REACTDATE ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_REFUND_Exam ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND (RDATE IS NULL OR RDATE = '') ";
            SqlErr = clsDB.GetDataTableEx(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("예약검사부도자 환불금 체크오류");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtQ.Dispose();
                DtQ = null;
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                rtnVal = true;

                strMsg = "예약검사부도자" + '\r';

                for (int i = 0; i < DtQ.Rows.Count; i++)
                {
                    strMsg += "접수일자 : " + DtQ.Rows[i]["YDATE"].ToString().Trim() + '\r';
                    strMsg += "진료과목 : " + DtQ.Rows[i]["REDEPT"].ToString().Trim() + '\r';
                    strMsg += "정리일자 : " + DtQ.Rows[i]["REACTDATE"].ToString().Trim() + '\r' + '\r';
                }

                ComFunc.MsgBox(strMsg, "확인");
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }

        /// <summary>
        /// Description : 기타 환불금 체크
        /// author : 박병규
        /// Create Date : 2017-10-19
        /// <param name="ArgPtno"></param>
        /// <seealso cref=""/>
        /// </summary>
        public bool READ_REFUND_ETC_CHECK(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;
            bool rtnVal = false;

            strMsg = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(ACTDATE,'YYYY-MM-DD') ACTDATE, ";
            SQL += ComNum.VBLF + "        DEPTCODE REDEPT, ";
            SQL += ComNum.VBLF + "        CREMARK, CAMT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_REFUND_ETC ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND (RDATE IS NULL OR RDATE = '') ";
            SqlErr = clsDB.GetDataTableEx(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("기타환불대상자 체크오류");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return rtnVal;
            }

            if (DtQ.Rows.Count > 0)
            {
                rtnVal = true;

                strMsg = "일반 환불대상자" + '\r';

                for (int i = 0; i < DtQ.Rows.Count; i++)
                {
                    strMsg += "보관일자 : " + DtQ.Rows[i]["ACTDATE"].ToString().Trim() + '\r';
                    strMsg += "진료과목 : " + DtQ.Rows[i]["REDEPT"].ToString().Trim() + '\r';
                    strMsg += "보관사유 : " + DtQ.Rows[i]["CREMARK"].ToString().Trim() + '\r';
                    strMsg += "보관금액 : " + string.Format("{0:#,##0}", DtQ.Rows[i]["CAMT"].ToString().Trim()) + '\r' + '\r';
                }

                ComFunc.MsgBox(strMsg, "확인");
            }

            DtQ.Dispose();
            DtQ = null;

            return rtnVal;
        }
        
        /// <summary>
        /// Description : 감염관리정보
        /// author : 박병규
        /// Create Date : 2017-10-20
        /// <param name="ArgPtno"></param>
        /// <param name="oAir"></param>
        /// <param name="oCon"></param>
        /// <param name="oBlood"></param>
        /// <param name="oBimal"></param>
        /// <seealso cref=""/>
        /// </summary>
        public void READ_INFECT_MASTER(PsmhDb pDbCon, string ArgPtno, PictureBox oAir, PictureBox oCon, PictureBox oBlood, PictureBox oBimal, PictureBox oboho, PictureBox otrip)
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ADMIN.FC_EXAM_INFECT_MASTER_IMG_EX(PANO, TRUNC(SYSDATE)) INFECT,";
            SQL += ComNum.VBLF + "        ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "EXAM_INFECT_MASTER ";
            SQL += ComNum.VBLF + " WHERE 1      = 1 ";
            SQL += ComNum.VBLF + "   AND PANO   = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "   AND ODATE IS NULL ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("감염관리정보 조회 오류");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }

            if (DtQ.Rows.Count > 0)
            {
                //v_jusa || v_hand || v_hand2 || v_mask || v_mask2 || v_boho || v_trip;
                //혈액
                if (VB.Mid(DtQ.Rows[0]["INFECT"].ToString().Trim(), 1, 1) == "1")
                {
                    oBlood.Visible = true;
                    oBlood.Image = ComLibB.Properties.Resources.I1000000;
                }

                //접촉1
                if (VB.Mid(DtQ.Rows[0]["INFECT"].ToString().Trim(), 2, 1) == "1")
                {
                    oCon.Visible = true;
                    oCon.Image = ComLibB.Properties.Resources.I0100000;
                }
                //접촉2
                if (VB.Mid(DtQ.Rows[0]["INFECT"].ToString().Trim(), 3, 1) == "1")
                {
                    oCon.Visible = true;
                    oCon.Image = ComLibB.Properties.Resources.I0010000;
                }

                //공기
                if (VB.Mid(DtQ.Rows[0]["INFECT"].ToString().Trim(), 4, 1) == "1")
                {
                    oAir.Visible = true;
                    oAir.Image = ComLibB.Properties.Resources.I0001000;
                }

               
                //비말
                if (VB.Mid(DtQ.Rows[0]["INFECT"].ToString().Trim(), 5, 1) == "1")
                {
                    oBimal.Visible = true;
                    oBimal.Image = ComLibB.Properties.Resources.I0000100;
                }

              
                //보호
                if (VB.Mid(DtQ.Rows[0]["INFECT"].ToString().Trim(), 6, 1) == "1")
                {
                    oboho.Visible = true;
                    oboho.Image = ComLibB.Properties.Resources.I0000010;
                }
                //해외
                if (VB.Mid(DtQ.Rows[0]["INFECT"].ToString().Trim(), 7, 1) == "1")
                {
                    otrip.Visible = true;
                    otrip.Image = ComLibB.Properties.Resources.I0000001;
                }
            }

            DtQ.Dispose();
            DtQ = null;

            return;
        }

        /// <summary>
        /// Description : 입원예약 대상자안내
        /// author : 박병규
        /// Create Date : 2017-10-20
        /// <param name="ArgPtno"></param>
        /// <seealso cref=""/>
        /// </summary>
        public void READ_IPD_RESERVED(PsmhDb pDbCon, string ArgPtno)
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PANO, SNAME, TO_CHAR(REDATE,'YYYY-MM-DD') REDATE, ";
            SQL += ComNum.VBLF + "        DEPTCODE, DRCODE, WARDCODE, ";
            SQL += ComNum.VBLF + "        ROOMCODE, INSIL, Remark, ";
            SQL += ComNum.VBLF + "        GbSMS, GBCHK, SMSDATE, ";
            SQL += ComNum.VBLF + "        GbDRG, GbSpc, GBDSC ";
            SQL += ComNum.VBLF + "   From " + ComNum.DB_PMPA + "IPD_RESERVED ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND SDATE = TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTableEx(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return;
            }

            if (DtQ.Rows.Count > 0)
            {
                clsPublic.GstrMsgTitle = "◆ 입원예약대상자 ◆";
                clsPublic.GstrMsgList = "입원 예약일자 : " + DtQ.Rows[0]["REDATE"].ToString().Trim() + '\r';
                clsPublic.GstrMsgList += "입원 진료과목 : " + DtQ.Rows[0]["DEPTCODE"].ToString().Trim() + '\r';
                clsPublic.GstrMsgList += "입원 진료의사 : " + DtQ.Rows[0]["DRCODE"].ToString().Trim() + '\r';
                clsPublic.GstrMsgList += "입원 참고사항 : " + DtQ.Rows[0]["Remark"].ToString().Trim() + '\r';
                clsPublic.GstrMsgList += "입원 예약대상자임. 안내요망" + '\r' + '\r';

                ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
            }

            DtQ.Dispose();
            DtQ = null;
        }

        /// <summary>
        /// Description : 
        /// Author : 박병규
        /// Create Date : 2017.11.03
        /// </summary>
        /// <param name="ArgDate">수진일자</param>
        /// <param name="ArgDept">진료과코드</param>
        /// <param name="ArgDr">의사코드</param>
        /// <param name="ArgSch">스케줄무시여부</param>
        /// <seealso cref=""/>
        public void READ_FM_CHOJAE_INWON(PsmhDb pDbCon, string ArgDate, string ArgDept, string ArgDr, string ArgSch)
        {
            ComFunc CF = new ComFunc();
            clsPmpaPb CPP = new clsPmpaPb();

            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strTime = string.Empty;

            string strYoil = CF.READ_YOIL(pDbCon, ArgDate);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT TO_CHAR(SysDate,'HH24MI') Ptime  From Dual  ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (DtQ.Rows.Count > 0)
                strTime = DtQ.Rows[0]["PTime"].ToString().Trim() + '\r';

            DtQ.Dispose();
            DtQ = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SDATE,'YYYY-MM-DD') SDate, SUM(RINWON1) AS CHOINWON, ";
            SQL += ComNum.VBLF + "        SUM(RINWON2) AS JAEINWON, GUBUN ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "ETC_FM_SCH ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SDate     <= TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND DrCode    = '" + ArgDr + "' ";
            SQL += ComNum.VBLF + "    AND Yoil      = '" + strYoil + "' ";
            SQL += ComNum.VBLF + "  GROUP By SDate,Gubun ";
            SQL += ComNum.VBLF + "  ORDER By SDate DESC ";
            SqlErr = clsDB.GetDataTableEx(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (DtQ.Rows.Count > 0)
            {
                for (int i = 0; i <= 1; i++)
                {
                    if (DtQ.Rows[i]["GUBUN"].ToString().Trim() == "1")
                    {
                        clsPublic.GnChoInWon_A = Convert.ToInt32(DtQ.Rows[i]["CHOINWON"].ToString().Trim());
                        clsPublic.GnJaeInWon_A = Convert.ToInt32(DtQ.Rows[i]["JAEINWON"].ToString().Trim());
                    }
                    else if (DtQ.Rows[i]["GUBUN"].ToString().Trim() == "2")
                    {
                        clsPublic.GnChoInWon_P = Convert.ToInt32(DtQ.Rows[i]["CHOINWON"].ToString().Trim());
                        clsPublic.GnJaeInWon_P = Convert.ToInt32(DtQ.Rows[i]["JAEINWON"].ToString().Trim());
                    }
                }
            }

            DtQ.Dispose();
            DtQ = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT GBJIN, GBJIN2 ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SCHEDULE ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND DRCODE    = '" + ArgDr + "' ";
            SQL += ComNum.VBLF + "    AND SCHDATE   = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SqlErr = clsDB.GetDataTableEx(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (ArgSch != "NO")
                {
                    if (DtQ.Rows[0]["GBJIN"].ToString().Trim() != "1")
                    {
                        clsPublic.GnChoInWon_A = 0;
                        clsPublic.GnJaeInWon_A = 0;
                    }

                    if (DtQ.Rows[0]["GBJIN2"].ToString().Trim() != "1")
                    {
                        clsPublic.GnChoInWon_P = 0;
                        clsPublic.GnJaeInWon_P = 0;
                    }
                }
            }
            else
                CPP.Varient_Clear();

            DtQ.Dispose();
            DtQ = null;
        }
        
        /// <summary>
        /// Description : 휴대폰,메일 전송승인 Update
        /// author : 박병규
        /// Create Date : 2017-11-13
        /// <param name="ArgPtno"></param>
        /// <param name="ArgTel"></param>
        /// <param name="ArgHPhone"></param>
        /// <param name="ArgEmail"></param>
        /// <param name="strFlag">clsPmpaPb.strDataFlag</param>
        /// <seealso cref="OUMSAD.bas:BAS_SMS_Approve_Insert"/>
        /// </summary>
        public void BAS_SMS_APPROVE_INSERT(PsmhDb pDbCon, string ArgPtno, string ArgTel, string ArgHPhone, string ArgEmail, string strFlag)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            if (strFlag != "OK")
                return;

            //#region 환자인적사항 변경 내역 백업
            //ComFunc CF1 = new ComFunc();
            //Dictionary<string, string> dict = new Dictionary<string, string>();
            //dict.Add("BIRTH", ArgTel);
            //dict.Add("HPHONE", ArgHPhone);
            //CF1.INSERT_BAS_PATIENT_HIS(ArgPtno, dict);
            //#endregion


            SQL = "";
            SQL = SQL + ComNum.VBLF + "UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
            SQL = SQL + ComNum.VBLF + "   SET Tel       = '" + ArgTel + "',";
            SQL = SQL + ComNum.VBLF + "       HPhone    = '" + ArgHPhone + "',";
            SQL = SQL + ComNum.VBLF + "       EMail     = '" + ArgEmail + "' ";
            SQL = SQL + ComNum.VBLF + " WHERE Pano      = '" + ArgPtno + "' ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                clsPmpaPb.strDataFlag = "NO";
            }
        }

        /// <summary>
        /// Description : 휴대폰문자메세지전송
        /// author : 박병규
        /// Create Date : 2017-11-14
        /// <param name="strGbn"></param>
        /// <param name="strPtno"></param>
        /// <param name="strSName"></param>
        /// <param name="strHPhone"></param>
        /// <param name="strRHPhone"></param>
        /// <param name="strMsg"></param>
        /// <param name="JobMsg"></param>
        /// <seealso cref="vbSMS.bas:SMS_BROKER_SEND_SDATA"/>
        /// </summary>
        public void SMS_BROKER_SEND_SDATA(PsmhDb pDbCon, string strGbn, string strPtno, string strSname, string strHPhone, string strRHPhone, string strMsg, string JobMsg)
        {
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowCnt = 0; //변경된 Row 받는 변수

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS        --휴대폰문자메세지전송";
            SQL += ComNum.VBLF + "        (JOBDATE, PANO, SNAME,                    --전송일시,등록번호,성명";
            SQL += ComNum.VBLF + "         HPHONE, GUBUN, DEPTCODE,                 --휴대폰번호,통보구분,진료과";
            SQL += ComNum.VBLF + "         DRCODE, RTIME, RETTEL,                   --의사코드,예약일시,회신번호";
            SQL += ComNum.VBLF + "         SENDMSG , ENTSABUN, ENTDATE,             --전송메세지,등록사번,등록일시";
            SQL += ComNum.VBLF + "         BIGO, GBPUSH)                            --참고사항,";
            SQL += ComNum.VBLF + " VALUES (SYSDATE, ";
            SQL += ComNum.VBLF + "         '" + strPtno + "', ";
            SQL += ComNum.VBLF + "         '" + strSname + "', ";
            SQL += ComNum.VBLF + "         '" + strHPhone + "', ";
            SQL += ComNum.VBLF + "         '" + strGbn + "', ";
            SQL += ComNum.VBLF + "         'XX', ";
            SQL += ComNum.VBLF + "         '0000', ";
            SQL += ComNum.VBLF + "         SYSDATE, ";
            SQL += ComNum.VBLF + "         '" + strRHPhone + "', ";
            SQL += ComNum.VBLF + "         '" + strMsg + "', ";
            SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + ", ";
            SQL += ComNum.VBLF + "         SYSDATE, ";
            SQL += ComNum.VBLF + "         '" + JobMsg + "', ";
            SQL += ComNum.VBLF + "        'N')  ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return;
            }
        }

        /// <summary>
        /// Description : 원외처방전 마스타를 READ
        /// author : 박병규
        /// Create Date : 2017-11-17
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgSlipNo"></param>
        /// <seealso cref="Drug_out_atc.bas:READ_OutDrugMst_NEW"/>
        /// </summary>
        public void READ_OUTDRUGMST(PsmhDb pDbCon, string ArgPtno, string ArgDate, int ArgSlipNo)
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            SQL = "";
            SQL += ComNum.VBLF + "  SELECT a.Pano, b.Sname, a.DeptCode,                         --등록번호,수진자명,진료과";
            SQL += ComNum.VBLF + "         a.DrCode, a.SlipNo, a.Bi,                            --의사코드,원외처방전번호,환자종류";
            SQL += ComNum.VBLF + "         TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,                 --발생일자";
            SQL += ComNum.VBLF + "         TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') EntDate,     --등록시각(외래수납시각)";
            SQL += ComNum.VBLF + "         TO_CHAR(a.SendDate,'YYYY-MM-DD HH24:MI') SendDate,   --약국심사완료시각";
            SQL += ComNum.VBLF + "         TO_CHAR(a.PrtDate,'YYYY-MM-DD HH24:MI') PrtDate,     --인쇄시각";
            SQL += ComNum.VBLF + "         TO_CHAR(a.ActDate,'YYYY-MM-DD') ActDate,             --회계일자";
            SQL += ComNum.VBLF + "         a.Part, a.SeqNo, a.Flag,                             --수납조,수납영수증번호,상태";
            SQL += ComNum.VBLF + "         a.PrtDept, a.Remark, a.Diease1,                      --약국외인쇄부서,원외처방변경내역,상병코드1";
            SQL += ComNum.VBLF + "         a.Diease2, a.Diease1_RO, a.Diease2_RO,               --상병코드2,상병코드1RO,상병코드2RO";
            SQL += ComNum.VBLF + "         a.ROWID, a.DrBunho, a.IpdOpd,                        --ROWID,의사면허번호,입원/외래";
            SQL += ComNum.VBLF + "         b.Sex, b.Jumin1, b.Jumin2,                           --성별,주민번호1,주민번호2";
            SQL += ComNum.VBLF + "         b.Jumin3, a.PrtBun, a.HapPrint,                      --주민번호(암호),?,합산처방전여부확인";
            SQL += ComNum.VBLF + "         a.GbV252,a.GbV352                                    --V252,V352여부";
            SQL += ComNum.VBLF + "    FROM " + ComNum.DB_MED + "OCS_OUTDRUGMST a, ";
            SQL += ComNum.VBLF + "         " + ComNum.DB_PMPA + "BAS_PATIENT b ";
            SQL += ComNum.VBLF + "   WHERE 1            = 1 ";
            SQL += ComNum.VBLF + "     AND a.SlipDate   = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "     AND a.SlipNo     = " + ArgSlipNo + "%" + " ";
            SQL += ComNum.VBLF + "     AND A.PANO       = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "     AND a.Pano       = b.Pano(+) ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (DtQ.Rows.Count > 0)
            {
                clsPmpaType.TODM.SlipDate = ArgDate;
                clsPmpaType.TODM.SlipNo = ArgSlipNo;
                clsPmpaType.TODM.Ptno = DtQ.Rows[0]["PANO"].ToString().Trim();
                clsPmpaType.TODM.Sname = DtQ.Rows[0]["Sname"].ToString().Trim();
                clsPmpaType.TODM.BDate = DtQ.Rows[0]["BDate"].ToString();
                clsPmpaType.TODM.Jumin1 = DtQ.Rows[0]["Jumin1"].ToString();
                clsPmpaType.TODM.Jumin2 = clsAES.DeAES(DtQ.Rows[0]["Jumin3"].ToString());
                clsPmpaType.TODM.Age = ComFunc.AgeCalcEx(clsPmpaType.TODM.Jumin1 + clsPmpaType.TODM.Jumin2, clsPmpaType.TODM.BDate);
                clsPmpaType.TODM.Sex = DtQ.Rows[0]["Sex"].ToString();
                clsPmpaType.TODM.DeptCode = DtQ.Rows[0]["DeptCode"].ToString();
                clsPmpaType.TODM.DrCode = DtQ.Rows[0]["DrCode"].ToString();
                clsPmpaType.TODM.DrName = "";
                clsPmpaType.TODM.Bi = DtQ.Rows[0]["Bi"].ToString();
                clsPmpaType.TODM.Flag = DtQ.Rows[0]["Flag"].ToString();
                clsPmpaType.TODM.PrtDept = DtQ.Rows[0]["PrtDept"].ToString();
                clsPmpaType.TODM.ActDate = DtQ.Rows[0]["ActDate"].ToString();
                clsPmpaType.TODM.Part = DtQ.Rows[0]["Part"].ToString();
                clsPmpaType.TODM.SeqNo = Convert.ToInt32(DtQ.Rows[0]["SeqNo"].ToString());
                clsPmpaType.TODM.EntDate = DtQ.Rows[0]["EntDate"].ToString();
                clsPmpaType.TODM.SendDate = DtQ.Rows[0]["SendDate"].ToString();
                clsPmpaType.TODM.PrtDate = DtQ.Rows[0]["PrtDate"].ToString();
                clsPmpaType.TODM.Remark = DtQ.Rows[0]["Remark"].ToString();
                clsPmpaType.TODM.Diease1 = DtQ.Rows[0]["Diease1"].ToString().Trim();
                clsPmpaType.TODM.Diease2 = DtQ.Rows[0]["Diease2"].ToString().Trim();
                clsPmpaType.TODM.Diease1_RO = DtQ.Rows[0]["Diease1_RO"].ToString().Trim();
                clsPmpaType.TODM.Diease2_RO = DtQ.Rows[0]["Diease2_RO"].ToString().Trim();
                clsPmpaType.TODM.DrBunho = Convert.ToInt64(DtQ.Rows[0]["DrBunho"].ToString());
                clsPmpaType.TODM.IpdOpd = DtQ.Rows[0]["IpdOpd"].ToString();
                clsPmpaType.TODM.PrtBun = VB.Left(DtQ.Rows[0]["PRTBUN"].ToString(), 1);
                clsPmpaType.TODM.HapPrint = DtQ.Rows[0]["HapPrint"].ToString();
                clsPmpaType.TODM.GbV252 = DtQ.Rows[0]["GbV252"].ToString();
                clsPmpaType.TODM.GbV352 = DtQ.Rows[0]["GbV352"].ToString();
                clsPmpaType.TODM.ROWID = DtQ.Rows[0]["ROWID"].ToString();
            }
            else
            {
                clsPmpaType.TODM.SlipDate = ArgDate;
                clsPmpaType.TODM.SlipNo = ArgSlipNo;
                clsPmpaType.TODM.Ptno = "";
                clsPmpaType.TODM.Sname = "";
                clsPmpaType.TODM.BDate = "";
                clsPmpaType.TODM.Jumin1 = "";
                clsPmpaType.TODM.Jumin2 = "";
                clsPmpaType.TODM.Age = 0;
                clsPmpaType.TODM.Sex = "";
                clsPmpaType.TODM.DeptCode = "";
                clsPmpaType.TODM.DrCode = "";
                clsPmpaType.TODM.DrName = "";
                clsPmpaType.TODM.Bi = "";
                clsPmpaType.TODM.Flag = "";
                clsPmpaType.TODM.PrtDept = "";
                clsPmpaType.TODM.ActDate = "";
                clsPmpaType.TODM.Part = "";
                clsPmpaType.TODM.SeqNo = 0;
                clsPmpaType.TODM.EntDate = "";
                clsPmpaType.TODM.SendDate = "";
                clsPmpaType.TODM.PrtDate = "";
                clsPmpaType.TODM.Remark = "";
                clsPmpaType.TODM.Diease1 = "";
                clsPmpaType.TODM.Diease2 = "";
                clsPmpaType.TODM.Diease1_RO = "";
                clsPmpaType.TODM.Diease2_RO = "";
                clsPmpaType.TODM.DrBunho = 0;
                clsPmpaType.TODM.IpdOpd = "";
                clsPmpaType.TODM.PrtBun = "";
                clsPmpaType.TODM.HapPrint = "";
                clsPmpaType.TODM.GbV252 = "";
                clsPmpaType.TODM.GbV352 = "";
                clsPmpaType.TODM.ROWID = "";

                DtQ.Dispose();
                DtQ = null;
                return;
            }

            DtQ.Dispose();
            DtQ = null;

            if (clsPmpaType.TODM.Diease1 == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT IllCode ";
                SQL += ComNum.VBLF + "   FROM  " + ComNum.DB_MED + "OCS_OILLS ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND Ptno      = '" + clsPmpaType.TODM.Ptno + "' ";
                SQL += ComNum.VBLF + "    AND BDate     = TO_DATE('" + clsPmpaType.TODM.BDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DeptCode  = '" + clsPmpaType.TODM.DeptCode + "' ";
                SQL += ComNum.VBLF + "  ORDER BY SeqNo ";
                SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (DtQ.Rows.Count > 0)
                    clsPmpaType.TODM.Diease1 = DtQ.Rows[0]["IllCode"].ToString().Trim();

                if (DtQ.Rows.Count > 1)
                    clsPmpaType.TODM.Diease2 = DtQ.Rows[1]["IllCode"].ToString().Trim();

                DtQ.Dispose();
                DtQ = null;

                Cursor.Current = Cursors.WaitCursor;
                clsDB.setBeginTran(pDbCon);

                SQL = "";
                if (clsPmpaType.TODM.Diease1 != "" && clsPmpaType.TODM.Diease1 == "")
                {
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
                    SQL += ComNum.VBLF + "    SET Diease1   = '" + clsPmpaType.TODM.Diease1 + "' ";
                }
                else if (clsPmpaType.TODM.Diease1 == "" && clsPmpaType.TODM.Diease1 == "")
                {
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_MED + "OCS_OUTDRUGMST ";
                    SQL += ComNum.VBLF + "    SET Diease1   = '" + clsPmpaType.TODM.Diease1 + "', ";
                    SQL += ComNum.VBLF + "    SET Diease2   = '" + clsPmpaType.TODM.Diease2 + "' ";
                }
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND SlipDate  = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND SlipNo    = " + ArgSlipNo + "%" + " ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + clsPmpaType.TODM.Ptno + "'";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox("UPDATE OCS_OUTDRUGMST 오류");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                //MessageBox.Show("저장되었습니다.", "알림", MessageBoxButtons.OK, MessageBoxIcon.None);
                Cursor.Current = Cursors.Default;
            }

            if (clsPmpaType.TODM.DrBunho > 0) { return; }
            if (VB.Right(clsPmpaType.TODM.DrCode,2) == "99") { return; } //전공의

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DrBunho ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_MED + "OCS_DOCTOR ";
            SQL += ComNum.VBLF + "  WHERE DRCODE = '" + clsPmpaType.TODM.DrCode + "' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }

            if (DtQ.Rows.Count > 0)
                clsPmpaType.TODM.DrBunho = Convert.ToInt64(DtQ.Rows[0]["DrBunho"].ToString().Trim());

            DtQ.Dispose();
            DtQ = null;
        }

        /// <summary>
        /// Description : 예약부도자 SMS DB에 INSERT
        /// author : 박병규
        /// Create Date : 2017-12-14
        /// <param name="ArgDate"></param>
        /// <seealso cref="frmjepsufailedbuild.frm:SMS_Yeyak_Budo"/>
        /// </summary>
        public void SMS_YEYAK_BUDO(PsmhDb pDbCon, string ArgDate)
        {
            DataTable DtQ = new DataTable();
            DataTable DtQSub = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            int intRowCnt = 0;

            string strPtno = "";
            string strDept = "";
            string strTel = "";
            string strDrCode = "";
            string strRetTel = "";
            string strMsg = "";

            clsPmpaFunc CPF = new clsPmpaFunc();

            Cursor.Current = Cursors.WaitCursor;
            clsDB.setBeginTran(pDbCon);

            SQL = "";
            SQL += ComNum.VBLF + " SELECT a.Pano, a.DeptCode, a.DrCode, ";
            SQL += ComNum.VBLF + "        b.SName, b.HPhone, ADMIN.FC_BAS_CLINICDEPT_DEPTNAMEK(a.DeptCode) DEPTNAMEK ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_REFUND a, ";
            SQL += ComNum.VBLF + "        " + ComNum.DB_PMPA + "BAS_PATIENT b ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND a.ActDate = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND a.RDate IS NULL ";
            SQL += ComNum.VBLF + "    AND a.Pano    = b.Pano(+) ";
            SQL += ComNum.VBLF + "    AND SUBSTR(b.HPhone,1,3) IN ('010','011','016','017','018','019') ";
            SQL += ComNum.VBLF + "    AND a.GUBUN = '01' "; //예약비부도 대상자만
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return;
            }

            if (DtQ.Rows.Count > 0)
            {
                for (int i = 0; i < DtQ.Rows.Count; i++)
                {
                    strPtno = DtQ.Rows[i]["PANO"].ToString().Trim();
                    strDept = DtQ.Rows[i]["DEPTNAMEK"].ToString().Trim();
                    strTel =  CPF.TelNo_Edit_Process(DtQ.Rows[i]["HPHONE"].ToString().Trim());
                    strDrCode = DtQ.Rows[i]["DRCODE"].ToString().Trim();

                    //진료과별 회신번호 SET
                    switch (strDept)
                    {
                        case "MD":
                            if (strDrCode == "1107")
                                strRetTel = "0542894210";
                            else
                                strRetTel = "0542894518";//내과접수
                            break;

                        case "GS":
                            strRetTel = "0542894545"; //일반외과/정형외과 접수
                            break;

                        case "OB":
                        case "OG":
                            strRetTel = "0542894546"; //산부인과 스테이션   '~1
                            break;

                        case "PD":
                            strRetTel = "0542894557";  //소아과 스테이션
                            break;

                        case "OS":
                            strRetTel = "0542894545"; //일반외과/정형외과 접수
                            break;

                        case "NS":
                            strRetTel = "0542894577"; //신경외과 외래
                            break;

                        case "NP":
                            strRetTel = "0542894583"; //신경정신과 스테이션
                            break;

                        case "EN":
                            strRetTel = "0542894586"; //이비인후과 진료실
                            break;

                        case "OT":
                            strRetTel = "0542894589"; //안과 진료실
                            break;

                        case "UR":
                            strRetTel = "0542894591"; //비뇨기과 진료실
                            break;

                        case "DM":
                            strRetTel = "0542894593"; //피부과 진료실
                            break;

                        case "DN":
                        case "DT":
                            strRetTel = "0542894207"; //치과 진료실       ~1
                            break;

                        case "RT":
                        case "PC":
                            strRetTel = "0542894622"; //통증치료 진료실   ~1
                            break;

                        case "RM":
                            strRetTel = "0542894320"; //재활의학과 진료실
                            break;

                        default:
                            strRetTel = "0542720151"; //기타는 병원 대표전화
                            break;
                    }

                    strMsg = "★포항성모병원★";
                    strMsg += DtQ.Rows[i]["SNAME"].ToString().Trim() + "님 " + strDept + " ";
                    strMsg += "예약일자 지남. 내원부탁합니다!문의⇒해당과";

                    //이미 자료를 넘겼는지 확인
                    SQL = "";
                    SQL += ComNum.VBLF + " SELECT *  FROM " + ComNum.DB_PMPA + "ETC_SMS ";
                    SQL += ComNum.VBLF + "  WHERE JobDate   >= TO_DATE('" + clsPublic.GstrSysDate + "','YYYY-MM-DD') ";
                    SQL += ComNum.VBLF + "    AND JobDate   <= TO_DATE('" + clsPublic.GstrSysDate + " 23:59','YYYY-MM-DD HH24:MI') ";
                    SQL += ComNum.VBLF + "    AND Pano      = '" + strPtno + "' ";
                    SQL += ComNum.VBLF + "    AND Gubun     = '8' "; //예약부도자
                    SqlErr = clsDB.GetDataTable(ref DtQSub, SQL, pDbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        DtQSub.Dispose();
                        DtQSub = null;
                        return;
                    }

                    if (DtQSub.Rows.Count == 0)
                    {
                        SQL = "";
                        SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "ETC_SMS ";
                        SQL += ComNum.VBLF + "        (JobDate, Pano, SName, ";
                        SQL += ComNum.VBLF + "         HPhone, Gubun, DeptCode,";
                        SQL += ComNum.VBLF + "         RetTel, SendMsg, EntSabun, ";
                        SQL += ComNum.VBLF + "         EntDate) ";
                        SQL += ComNum.VBLF + " VALUES (SYSDATE, ";
                        SQL += ComNum.VBLF + "         '" + strPtno + "', ";
                        SQL += ComNum.VBLF + "         '" + DtQ.Rows[i]["SNAME"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "         '" + strTel + "', ";
                        SQL += ComNum.VBLF + "         '8', ";
                        SQL += ComNum.VBLF + "         '" + DtQ.Rows[i]["DEPTCODE"].ToString().Trim() + "', ";
                        SQL += ComNum.VBLF + "         '" + strRetTel + "', ";
                        SQL += ComNum.VBLF + "         '" + strMsg + "', ";
                        SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + ", ";
                        SQL += ComNum.VBLF + "         SYSDATE) ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(pDbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                            Cursor.Current = Cursors.Default;
                            return;
                        }


                    }

                    DtQSub.Dispose();
                    DtQSub = null;
                }
            }
            DtQ.Dispose();
            DtQ = null;

            clsDB.setCommitTran(pDbCon);

            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// Description : 의료급여_희귀난치자동등록
        /// author : 박병규
        /// Create Date : 2017-12-18
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgSdate"></param>
        /// <param name="ArgEdate"></param>
        /// <param name="ArgJcode"></param>
        /// <param name="ArgSname"></param>
        /// <param name="ArgJumin"></param>
        /// <seealso cref="oumsad02_new.frm:의료급여_희귀난치자동등록2"/>
        /// </summary>
        public void AutoSave_MedicalCare_Rareness(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgSdate, string ArgEdate, string ArgJcode, string ArgSname, string ArgJumin)
        {
            DataTable DtQ = new DataTable();
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            int intRowCnt = 0; //변경된 Row 받는 변수

            ComFunc CF = new ComFunc();

            string strRowID = "";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT ROWID ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_CANCER ";
            SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
            SQL += ComNum.VBLF + "    AND PANO  = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND GUBUN = '2' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                DtQ.Dispose();
                DtQ = null;
                return;
            }

            if (DtQ.Rows.Count > 0)
                strRowID = DtQ.Rows[0]["ROWID"].ToString();

            DtQ.Dispose();
            DtQ = null;

            clsDB.setBeginTran(pDbCon);

            SQL = "";
            if (strRowID == "")
            {
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "BAS_CANCER ";
                SQL += ComNum.VBLF + "        (PANO, SNAME , SEX, ";
                SQL += ComNum.VBLF + "         AGE, JBUNHO, FDATE, ";
                SQL += ComNum.VBLF + "         TDATE, DEPT1, Memo, ";
                SQL += ComNum.VBLF + "         ENTSABUN, GUBUN, Auto_Chk ) ";
                SQL += ComNum.VBLF + " VALUES ('" + ArgPtno + "', ";
                SQL += ComNum.VBLF + "         '" + ArgSname + "', ";
                SQL += ComNum.VBLF + "         '" + CF.SEX_SEARCH(VB.Right(ArgJumin, 7)) + "', ";
                SQL += ComNum.VBLF + "         '" + ComFunc.AgeCalcEx(ArgJumin, clsPublic.GstrSysDate) + "' , ";
                SQL += ComNum.VBLF + "         '" + ArgJcode + "', ";
                SQL += ComNum.VBLF + "         TO_DATE('" + ArgSdate + "','YYYY-MM-DD'), ";
                SQL += ComNum.VBLF + "         TO_DATE('" + ArgEdate + "','YYYY-MM-DD') ,";
                SQL += ComNum.VBLF + "         '" + ArgDept + "', ";
                SQL += ComNum.VBLF + "         '자동등록.', ";
                SQL += ComNum.VBLF + "         " + clsType.User.IdNumber + " , ";
                SQL += ComNum.VBLF + "         '2', ";
                SQL += ComNum.VBLF + "         'Y' ) ";
            }
            else
            {
                SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_CANCER ";
                SQL += ComNum.VBLF + "    SET JBUNHO    = '" + ArgJcode + "', "; // 희귀난치등록번호
                SQL += ComNum.VBLF + "        FDATE     = TO_DATE('" + ArgSdate + "','YYYY-MM-DD'), "; // 시작일
                SQL += ComNum.VBLF + "        TDATE     = TO_DATE('" + ArgEdate + "','YYYY-MM-DD'), "; // 종료일
                SQL += ComNum.VBLF + "        ENTSABUN  = " + clsType.User.IdNumber + " ";
                SQL += ComNum.VBLF + "  WHERE ROWID     = '" + strRowID + "' ";
            }
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                return;
            }

            clsDB.setCommitTran(pDbCon);
        }

        /// <summary>
        /// Description : BAS_SUGA_AMT 에서 금액읽어오기
        /// Author : 김민철
        /// Create Date : 2017.09.13
        /// <param name="ArgSuCode"></param>
        /// <param name="ArgSUNEXT"></param>
        /// <param name="ArgSuDate"></param>
        /// </summary>
        /// <seealso cref="VbSugaRead_new1.bas:Suga_Read_Amt_NEW"/>
        public void Read_Suga_Amt(PsmhDb pDbCon, string ArgSuCode, string ArgSUNEXT, string ArgSuDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            clsPmpaPb.GnIAmt = 0;
            clsPmpaPb.GnBAmt = 0;
            clsPmpaPb.GnTAmt = 0;
            clsPmpaPb.GnSAmt = 0;
            clsPmpaPb.GnSelAmt = 0;
            clsPmpaPb.GstrSugaNewReadOK = "NO";

            SQL = "";
            SQL += ComNum.VBLF + " SELECT TO_CHAR(SUDATE,'YYYY-MM-DD') SuDate,iAmt,bAmt,tAmt,sAmt,SelAmt ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_SUGA_AMT ";
            SQL += ComNum.VBLF + "  WHERE SuCode ='" + ArgSuCode + "' ";
            SQL += ComNum.VBLF + "    AND SuNext ='" + ArgSUNEXT + "' ";
            SQL += ComNum.VBLF + "    AND SuDate <=TO_DATE('" + ArgSuDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DelDate IS NULL ";
            SQL += ComNum.VBLF + "  ORDER BY SUDATE DESC ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                return;
            }
            if (DtQ.Rows.Count > 0)
            {
                clsPmpaPb.GnIAmt = Convert.ToInt64(VB.Val(DtQ.Rows[0]["IAMT"].ToString()));
                clsPmpaPb.GnBAmt = Convert.ToInt64(VB.Val(DtQ.Rows[0]["BAMT"].ToString()));
                clsPmpaPb.GnTAmt = Convert.ToInt64(VB.Val(DtQ.Rows[0]["TAMT"].ToString()));
                clsPmpaPb.GnSAmt = Convert.ToInt64(VB.Val(DtQ.Rows[0]["SAMT"].ToString()));
                clsPmpaPb.GnSelAmt = Convert.ToInt64(VB.Val(DtQ.Rows[0]["SELAMT"].ToString()));

                clsPmpaPb.GstrSugaNewReadOK = "OK";
            }

            DtQ.Dispose();
            DtQ = null;

            //해당값없을경우
            if (clsPmpaPb.GstrSugaNewReadOK.Equals("NO"))
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT a.Sucode, a.Bun,    a.Nu,    a.SugbA, a.SugbB,                         ";
                SQL += ComNum.VBLF + "       a.SugbC,  a.SugbD,  a.SugbE, a.SugbF, a.SugbG,                         ";
                SQL += ComNum.VBLF + "       a.SugbH,  a.SugbI,  a.SugbJ,  a.Iamt,  a.Tamt,                         ";
                SQL += ComNum.VBLF + "       a.Bamt,   TO_CHAR(a.Sudate, 'yyyy-mm-dd') SuDay,                       ";
                SQL += ComNum.VBLF + "       a.OldIamt,a.OldTamt,a.OldBamt,a.Sunext,                                ";
                SQL += ComNum.VBLF + "       TO_CHAR(a.Sudate3, 'yyyy-mm-dd') SuDay3,                               ";
                SQL += ComNum.VBLF + "       a.Iamt3, a.Tamt3, a.Bamt3, TO_CHAR(a.Sudate4, 'yyyy-mm-dd') SuDay4,    ";
                SQL += ComNum.VBLF + "       a.Iamt4, a.Tamt4, a.Bamt4, TO_CHAR(a.Sudate5, 'yyyy-mm-dd') SuDay5,    ";
                SQL += ComNum.VBLF + "       a.Iamt5, a.Tamt5, a.Bamt5,                                             ";
                SQL += ComNum.VBLF + "       nvl(a.DayMax,'0') DayMax,  nvl(a.TotMax,'0') TotMax, b.SugbQ,  b.SugbR                                  ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SUT a,                                       ";
                SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN b                                        ";
                SQL += ComNum.VBLF + " WHERE TRIM(a.Sucode) = '" + ArgSuCode + "'                                   ";
                SQL += ComNum.VBLF + "   AND a.SuNext = b.SuNext(+)                                                 ";
                SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
                if (DtQ.Rows.Count > 0)
                {
                    if (string.Compare(ArgSuDate, DtQ.Rows[0]["SuDay"].ToString().Trim()) <= 0)
                    {
                        clsPmpaPb.GnIAmt = Convert.ToInt64(DtQ.Rows[0]["IAMT"].ToString());
                        clsPmpaPb.GnBAmt = Convert.ToInt64(DtQ.Rows[0]["BAMT"].ToString());
                        clsPmpaPb.GnTAmt = Convert.ToInt64(DtQ.Rows[0]["TAMT"].ToString());
                        clsPmpaPb.GnSAmt = 0;
                        clsPmpaPb.GnSelAmt = 0;
                    }
                    else if (string.Compare(ArgSuDate, DtQ.Rows[0]["SuDay3"].ToString().Trim()) <= 0)
                    {
                        clsPmpaPb.GnIAmt = Convert.ToInt64(DtQ.Rows[0]["OldIamt"].ToString());
                        clsPmpaPb.GnBAmt = Convert.ToInt64(DtQ.Rows[0]["OldBamt"].ToString());
                        clsPmpaPb.GnTAmt = Convert.ToInt64(DtQ.Rows[0]["OldTamt"].ToString());
                        clsPmpaPb.GnSAmt = 0;
                        clsPmpaPb.GnSelAmt = 0;
                    }
                    else if (string.Compare(ArgSuDate, DtQ.Rows[0]["SuDay4"].ToString().Trim()) <= 0)
                    {
                        clsPmpaPb.GnIAmt = Convert.ToInt64(DtQ.Rows[0]["IAMT3"].ToString());
                        clsPmpaPb.GnBAmt = Convert.ToInt64(DtQ.Rows[0]["BAMT3"].ToString());
                        clsPmpaPb.GnTAmt = Convert.ToInt64(DtQ.Rows[0]["TAMT3"].ToString());
                        clsPmpaPb.GnSAmt = 0;
                        clsPmpaPb.GnSelAmt = 0;
                    }
                    else if (string.Compare(ArgSuDate, DtQ.Rows[0]["SuDay5"].ToString().Trim()) <= 0)
                    {
                        clsPmpaPb.GnIAmt = Convert.ToInt64(DtQ.Rows[0]["IAMT4"].ToString());
                        clsPmpaPb.GnBAmt = Convert.ToInt64(DtQ.Rows[0]["BAMT4"].ToString());
                        clsPmpaPb.GnTAmt = Convert.ToInt64(DtQ.Rows[0]["TAMT4"].ToString());
                        clsPmpaPb.GnSAmt = 0;
                        clsPmpaPb.GnSelAmt = 0;
                    }
                    else
                    {
                        clsPmpaPb.GnIAmt = Convert.ToInt64(DtQ.Rows[0]["IAMT5"].ToString());
                        clsPmpaPb.GnBAmt = Convert.ToInt64(DtQ.Rows[0]["BAMT5"].ToString());
                        clsPmpaPb.GnTAmt = Convert.ToInt64(DtQ.Rows[0]["TAMT5"].ToString());
                        clsPmpaPb.GnSAmt = 0;
                        clsPmpaPb.GnSelAmt = 0;
                    }
                }

                DtQ.Dispose();
                DtQ = null;
            }

        }

        public void Read_Suga_Amt_HU(PsmhDb pDbCon, string ArgSuCode, string ArgSUNEXT, string ArgSuDate)
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;

            clsPmpaPb.GnIAmt = 0;
            clsPmpaPb.GnBAmt = 0;
            clsPmpaPb.GnTAmt = 0;
            clsPmpaPb.GnSAmt = 0;
            clsPmpaPb.GnSelAmt = 0;
            clsPmpaPb.GstrSugaNewReadOK = "NO";

            

            //해당값없을경우
            if (clsPmpaPb.GstrSugaNewReadOK.Equals("NO"))
            {
                SQL = "";
                SQL += ComNum.VBLF + "SELECT a.Sucode, a.Bun,    a.Nu,    a.SugbA, a.SugbB,                         ";
                SQL += ComNum.VBLF + "       a.SugbC,  a.SugbD,  a.SugbE, a.SugbF, a.SugbG,                         ";
                SQL += ComNum.VBLF + "       a.SugbH,  a.SugbI,  a.SugbJ,  a.Iamt,  a.Tamt,                         ";
                SQL += ComNum.VBLF + "       a.Bamt,   TO_CHAR(a.Sudate, 'yyyy-mm-dd') SuDay,                       ";
                SQL += ComNum.VBLF + "       a.OldIamt,a.OldTamt,a.OldBamt,a.Sunext,                                ";
                SQL += ComNum.VBLF + "       TO_CHAR(a.Sudate3, 'yyyy-mm-dd') SuDay3,                               ";
                SQL += ComNum.VBLF + "       a.Iamt3, a.Tamt3, a.Bamt3, TO_CHAR(a.Sudate4, 'yyyy-mm-dd') SuDay4,    ";
                SQL += ComNum.VBLF + "       a.Iamt4, a.Tamt4, a.Bamt4, TO_CHAR(a.Sudate5, 'yyyy-mm-dd') SuDay5,    ";
                SQL += ComNum.VBLF + "       a.Iamt5, a.Tamt5, a.Bamt5,                                             ";
                SQL += ComNum.VBLF + "       a.DayMax, a.TotMax, b.SugbQ,  b.SugbR                                  ";
                SQL += ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "BAS_SUT a,                                       ";
                SQL += ComNum.VBLF + "       " + ComNum.DB_PMPA + "BAS_SUN b                                        ";
                SQL += ComNum.VBLF + " WHERE TRIM(a.Sucode) = '" + ArgSuCode + "'                                   ";
                SQL += ComNum.VBLF + "   AND a.SuNext = b.SuNext(+) and b.SUGBAF ='1'                               ";
                SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return;
                }
              
                if (DtQ.Rows.Count > 0)
                {
                    clsPmpaPb.GstrSugaNewReadOK = "OK";
                    if (string.Compare( DtQ.Rows[0]["SuDay"].ToString(),ArgSuDate) <= 0)
                    {
                        clsPmpaPb.GnIAmt = Convert.ToInt64(DtQ.Rows[0]["IAMT"].ToString());
                        clsPmpaPb.GnBAmt = Convert.ToInt64(DtQ.Rows[0]["BAMT"].ToString());
                        clsPmpaPb.GnTAmt = Convert.ToInt64(DtQ.Rows[0]["TAMT"].ToString());
                        clsPmpaPb.GnSAmt = 0;
                        clsPmpaPb.GnSelAmt = 0;
                    }
                    else if (string.Compare( DtQ.Rows[0]["SuDay3"].ToString(), ArgSuDate) <= 0)
                    {
                        clsPmpaPb.GnIAmt = Convert.ToInt64(DtQ.Rows[0]["OldIamt"].ToString());
                        clsPmpaPb.GnBAmt = Convert.ToInt64(DtQ.Rows[0]["OldBamt"].ToString());
                        clsPmpaPb.GnTAmt = Convert.ToInt64(DtQ.Rows[0]["OldTamt"].ToString());
                        clsPmpaPb.GnSAmt = 0;
                        clsPmpaPb.GnSelAmt = 0;
                    }
                    else if (string.Compare( DtQ.Rows[0]["SuDay4"].ToString(), ArgSuDate) <= 0)
                    {
                        clsPmpaPb.GnIAmt = Convert.ToInt64(DtQ.Rows[0]["IAMT3"].ToString());
                        clsPmpaPb.GnBAmt = Convert.ToInt64(DtQ.Rows[0]["BAMT3"].ToString());
                        clsPmpaPb.GnTAmt = Convert.ToInt64(DtQ.Rows[0]["TAMT3"].ToString());
                        clsPmpaPb.GnSAmt = 0;
                        clsPmpaPb.GnSelAmt = 0;
                    }
                    else if (string.Compare( DtQ.Rows[0]["SuDay5"].ToString(), ArgSuDate) <= 0)
                    {
                        clsPmpaPb.GnIAmt = Convert.ToInt64(DtQ.Rows[0]["IAMT4"].ToString());
                        clsPmpaPb.GnBAmt = Convert.ToInt64(DtQ.Rows[0]["BAMT4"].ToString());
                        clsPmpaPb.GnTAmt = Convert.ToInt64(DtQ.Rows[0]["TAMT4"].ToString());
                        clsPmpaPb.GnSAmt = 0;
                        clsPmpaPb.GnSelAmt = 0;
                    }
                    else
                    {
                        clsPmpaPb.GnIAmt = Convert.ToInt64(DtQ.Rows[0]["IAMT5"].ToString());
                        clsPmpaPb.GnBAmt = Convert.ToInt64(DtQ.Rows[0]["BAMT5"].ToString());
                        clsPmpaPb.GnTAmt = Convert.ToInt64(DtQ.Rows[0]["TAMT5"].ToString());
                        clsPmpaPb.GnSAmt = 0;
                        clsPmpaPb.GnSelAmt = 0;
                    }
                }

                DtQ.Dispose();
                DtQ = null;
            }

        }

        /// <summary>
        /// Description : 외래 당일 동명이인 점검
        /// author : 박병규
        /// Create Date : 2018.1.3
        /// <param name="ArgPtno"></param>
        /// <seealso cref=""/>
        /// </summary>
        public void Read_OPD_SameName(PsmhDb pDbCon, string ArgSname, string ArgDept)
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT COUNT(PANO) CNT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND ACTDATE   = TRUNC(SYSDATE) ";
            SQL += ComNum.VBLF + "    AND SNAME     = '" + ArgSname + "' ";
            SQL += ComNum.VBLF + "    AND DeptCode  = '" + ArgDept + "' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return;
            }

            if (DtQ.Rows.Count > 0)
            {
                if (VB.Val(DtQ.Rows[0]["CNT"].ToString().Trim()) > 1)
                {
                    clsPublic.GstrMsgTitle = "◆ 동명이인 ◆";
                    clsPublic.GstrMsgList = "당일 외래접수 환자중 동일진료과, 동일이름 접수건이 존재함." + '\r';
                    clsPublic.GstrMsgList += "주민번호를 꼭 확인후 수납하시기 바랍니다." + '\r';

                    ComFunc.MsgBox(clsPublic.GstrMsgList, clsPublic.GstrMsgTitle);
                }
            }

            DtQ.Dispose();
            DtQ = null;
        }

        /// <summary>
        /// Description : 계절독감 백신 수가 금액산정
        /// author : 박병규
        /// Create Date : 2018.1.5
        /// <param name="ArgSucode"></param>
        /// <param name="ArgGB">Y.필수예방접종</param>
        /// <seealso cref=""/>
        /// </summary>
        public void Get_Vaccine_Amt(PsmhDb pDbCon, string ArgSucode, string ArgGB = "")
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT MIRAMT, BONAMT, GAMAMT, ";
            SQL += ComNum.VBLF + "        GBES ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_VACC_MST ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND SUCODE    = '" + ArgSucode + "' ";
            SQL += ComNum.VBLF + "    AND DELDATE IS NULL ";

            if (ArgGB != "")
                SQL += ComNum.VBLF + "AND GBES      = 'Y' ";

            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("계절독감 백신 수가 금액산정 조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return;
            }

            if (DtQ.Rows.Count > 0)
            {
                clsPmpaPb.GnVacc_Mir += Convert.ToInt64(VB.Val(DtQ.Rows[0]["MIRAMT"].ToString()));
                clsPmpaPb.GnVacc_Gam += Convert.ToInt64(VB.Val(DtQ.Rows[0]["GAMAMT"].ToString()));
            }

            DtQ.Dispose();
            DtQ = null;
        }

        /// <summary>
        /// Description : 53:조합 54: 감액 56: 미수 64:급여+비급여
        /// author : 박병규
        /// Create Date : 2018.2.5
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgSeqNo"></param>
        /// <seealso cref="opdAcct.bas:HIC_Boho_Silp_Amt"/>
        /// </summary>
        public void HIC_Boho_Slip_Amt(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate, int ArgSeqNo)
        {
            DataTable DtQ = new DataTable();
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;

            clsPmpaPb.GnBohoAmt = 0;    //의료급여 1500, 2000
            clsPmpaPb.GnBohoAmt53 = 0;  //조합
            clsPmpaPb.GnBohoAmt54 = 0;  //감액
            clsPmpaPb.GnBohoAmt56 = 0;  //미수
            clsPmpaPb.GnBohoAmt64 = 0;  //영수금액
            clsPmpaPb.GnBohoAmt99 = 0;  //급여총액

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(AMT1+AMT2), 0) AMT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";

            if (ArgSeqNo != 0)
                SQL += ComNum.VBLF + "AND SEQNO NOT IN ( " + ArgSeqNo + ") ";

            SQL += ComNum.VBLF + "    AND NU        <= '20' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return;
            }

            if (DtQ.Rows.Count > 0)
                clsPmpaPb.GnBohoAmt99 += Convert.ToInt64(DtQ.Rows[0]["AMT"].ToString());

            DtQ.Dispose();
            DtQ = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(AMT1+AMT2), 0) AMT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND NU        = '64' ";

            if (ArgSeqNo != 0)
                SQL += ComNum.VBLF + "AND SEQNO NOT IN ( " + ArgSeqNo + ") ";

            SQL += ComNum.VBLF + "    AND DOSCODE   = '20' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return;
            }

            if (DtQ.Rows.Count > 0)
                clsPmpaPb.GnBohoAmt += Convert.ToInt64(DtQ.Rows[0]["AMT"].ToString());

            DtQ.Dispose();
            DtQ = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NU, NVL(SUM(AMT1+AMT2), 0) AMT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND NU        IN ('53', '54', '56', '64') ";

            if (ArgSeqNo != 0)
                SQL += ComNum.VBLF + "AND SEQNO NOT IN ( " + ArgSeqNo + ") ";

            SQL += ComNum.VBLF + "  GROUP BY NU";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return;
            }

            if (DtQ.Rows.Count > 0)
            {
                for (int i = 0; i < DtQ.Rows.Count; i++)
                {
                    switch (DtQ.Rows[i]["NU"].ToString().Trim())
                    {
                        case "53":
                            clsPmpaPb.GnBohoAmt53 += Convert.ToInt64(DtQ.Rows[i]["AMT"].ToString()) + clsPmpaPb.GnBohoAmt;
                            break;

                        case "54":
                            clsPmpaPb.GnBohoAmt54 += Convert.ToInt64(DtQ.Rows[i]["AMT"].ToString());
                            break;

                        case "56":
                            clsPmpaPb.GnBohoAmt56 += Convert.ToInt64(DtQ.Rows[i]["AMT"].ToString());
                            break;

                        case "64":
                            clsPmpaPb.GnBohoAmt64 += Convert.ToInt64(DtQ.Rows[i]["AMT"].ToString());
                            break;
                    }
                }
            }

            clsPmpaPb.GnBohoAmt64 = clsPmpaPb.GnBohoAmt99 - clsPmpaPb.GnBohoAmt53;

            DtQ.Dispose();
            DtQ = null;

            if (ArgDate == clsPublic.GstrSysDate)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT AMT4, AMT5, AMT6, AMT7 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
                SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtQ.Dispose();
                    DtQ = null;
                    return;
                }

                if (DtQ.Rows.Count > 0)
                {
                    clsPmpaPb.GnBohoAmt53 += Convert.ToInt64(VB.Val(DtQ.Rows[0]["AMT4"].ToString()));
                    clsPmpaPb.GnBohoAmt54 += Convert.ToInt64(VB.Val(DtQ.Rows[0]["AMT5"].ToString()));
                    clsPmpaPb.GnBohoAmt56 += Convert.ToInt64(VB.Val(DtQ.Rows[0]["AMT6"].ToString()));
                    clsPmpaPb.GnBohoAmt64 += Convert.ToInt64(VB.Val(DtQ.Rows[0]["AMT7"].ToString()));
                }

                DtQ.Dispose();
                DtQ = null;
            }

        }
        
        /// <summary>
        /// Description : 53:조합 54: 감액 56: 미수 64:급여+비급여
        /// author : 박병규
        /// Create Date : 2018.2.5
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDate"></param>
        /// <param name="ArgSeqNo"></param>
        /// <seealso cref="opdAcct.bas:HIC_차상위_Silp_Amt"/>
        /// </summary>
        public void HIC_NPG_Slip_Amt(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDate, int ArgSeqNo)
        {
            DataTable DtQ = new DataTable();

            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;

            clsPmpaPb.GnNPGAmt = 0;   
            clsPmpaPb.GnNPGAmt53 = 0;  //조합
            clsPmpaPb.GnNPGAmt54 = 0;  //감액
            clsPmpaPb.GnNPGAmt56 = 0;  //미수
            clsPmpaPb.GnNPGAmt64 = 0;  //영수금액
            clsPmpaPb.GnNPGAmt99 = 0;  //급여총액

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(AMT1+AMT2), 0) AMT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";

            if (ArgSeqNo != 0)
                SQL += ComNum.VBLF + "AND SEQNO NOT IN ( " + ArgSeqNo + ") ";

            SQL += ComNum.VBLF + "    AND NU        <= '20' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return;
            }

            if (DtQ.Rows.Count > 0)
                clsPmpaPb.GnNPGAmt99 += Convert.ToInt64(DtQ.Rows[0]["AMT"].ToString());

            DtQ.Dispose();
            DtQ = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NVL(SUM(AMT1+AMT2), 0) AMT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND NU        = '64' ";

            if (ArgSeqNo != 0)
                SQL += ComNum.VBLF + "AND SEQNO NOT IN ( " + ArgSeqNo + ") ";

            SQL += ComNum.VBLF + "    AND DOSCODE   = '20' ";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return;
            }

            if (DtQ.Rows.Count > 0)
                clsPmpaPb.GnNPGAmt += Convert.ToInt64(DtQ.Rows[0]["AMT"].ToString());

            DtQ.Dispose();
            DtQ = null;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT NU, NVL(SUM(AMT1+AMT2), 0) AMT ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_SLIP ";
            SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
            SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
            SQL += ComNum.VBLF + "    AND NU        IN ('53', '54', '56', '64') ";

            if (ArgSeqNo != 0)
                SQL += ComNum.VBLF + "AND SEQNO NOT IN ( " + ArgSeqNo + ") ";

            SQL += ComNum.VBLF + "  GROUP BY NU";
            SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                DtQ.Dispose();
                DtQ = null;
                return;
            }

            if (DtQ.Rows.Count > 0)
            {
                for (int i = 0; i < DtQ.Rows.Count; i++)
                {
                    switch (DtQ.Rows[0]["NU"].ToString().Trim())
                    {
                        case "53":
                            clsPmpaPb.GnNPGAmt53 += Convert.ToInt64(DtQ.Rows[i]["AMT"].ToString()) + clsPmpaPb.GnNPGAmt;
                            break;

                        case "54":
                            clsPmpaPb.GnNPGAmt54 += Convert.ToInt64(DtQ.Rows[i]["AMT"].ToString());
                            break;

                        case "56":
                            clsPmpaPb.GnNPGAmt56 += Convert.ToInt64(DtQ.Rows[i]["AMT"].ToString());
                            break;

                        case "64":
                            clsPmpaPb.GnNPGAmt64 += Convert.ToInt64(DtQ.Rows[i]["AMT"].ToString());
                            break;
                    }
                }
            }

            clsPmpaPb.GnNPGAmt64 = clsPmpaPb.GnNPGAmt99 - clsPmpaPb.GnNPGAmt53;

            DtQ.Dispose();
            DtQ = null;

            if (ArgDate == clsPublic.GstrSysDate)
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT NVL(AMT4, 0) AMT4, NVL(AMT5, 0) AMT5, NVL(AMT6, 0) AMT6, NVL(AMT7, 0) AMT7 ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND BDATE     = TO_DATE('" + ArgDate + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "    AND DEPTCODE  = '" + ArgDept + "' ";
                SqlErr = clsDB.GetDataTable(ref DtQ, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    DtQ.Dispose();
                    DtQ = null;
                    return;
                }

                if (DtQ.Rows.Count > 0)
                {
                    clsPmpaPb.GnNPGAmt53 += Convert.ToInt64(DtQ.Rows[0]["AMT4"].ToString());
                    clsPmpaPb.GnNPGAmt54 += Convert.ToInt64(DtQ.Rows[0]["AMT5"].ToString());
                    clsPmpaPb.GnNPGAmt56 += Convert.ToInt64(DtQ.Rows[0]["AMT6"].ToString());
                    clsPmpaPb.GnNPGAmt64 += Convert.ToInt64(DtQ.Rows[0]["AMT7"].ToString());
                }

                DtQ.Dispose();
                DtQ = null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pDbCon"></param>
        /// <param name="ArgPtno"></param>
        /// <param name="ArgDept"></param>
        /// <param name="ArgDr"></param>
        /// <param name="ArgBi"></param>
        /// <param name="ArgSucode"></param>
        /// <param name="ArgDrSabun"></param>
        /// <returns></returns>
        /// <seealso cref="oumsad_order_insert.bas : OPD_Order_Insert"/>
        public bool Insert_Opd_Order(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDr, string ArgBi, string ArgSucode, string ArgDrSabun)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;
            int intRowCnt = 0;

            bool rtnVal = false;

            clsDB.setBeginTran(pDbCon);

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_MED + "OCS_OORDER ";
            SQL += ComNum.VBLF + "        (PTNO, BDATE, DEPTCODE, ";
            SQL += ComNum.VBLF + "         SEQNO, ORDERCODE, SUCODE, ";
            SQL += ComNum.VBLF + "         BUN, SLIPNO, REALQTY, ";
            SQL += ComNum.VBLF + "         QTY, NAL, GBDIV, ";
            SQL += ComNum.VBLF + "         DOSCODE, GBBOTH, GBINFO, ";
            SQL += ComNum.VBLF + "         GBER, GBSELF, GBSPC, ";
            SQL += ComNum.VBLF + "         BI, DRCODE, REMARK, ";
            SQL += ComNum.VBLF + "         ENTDATE, GBSUNAP, TUYAKNO, ";
            SQL += ComNum.VBLF + "         ORDERNO, MULTI, MULTIREMARK, ";
            SQL += ComNum.VBLF + "         DUR, RESV, SCODESAYU, ";
            SQL += ComNum.VBLF + "         SCODEREMARK, GBSEND, CORDERCODE, ";
            SQL += ComNum.VBLF + "         CSUCODE, CBUN) ";
            SQL += ComNum.VBLF + " VALUES ('" + ArgPtno + "' , ";
            SQL += ComNum.VBLF + "         TRUNC(SYSDATE), ";
            SQL += ComNum.VBLF + "         '" + ArgDept + "', ";
            SQL += ComNum.VBLF + "         99, ";
            SQL += ComNum.VBLF + "         '" + ArgSucode + "', ";
            SQL += ComNum.VBLF + "         '" + ArgSucode + "', ";
            SQL += ComNum.VBLF + "         '20', ";
            SQL += ComNum.VBLF + "         '0005', ";
            SQL += ComNum.VBLF + "         '1', ";
            SQL += ComNum.VBLF + "         '1', ";
            SQL += ComNum.VBLF + "         '1', ";
            SQL += ComNum.VBLF + "         '1', ";
            SQL += ComNum.VBLF + "         '910101', ";
            SQL += ComNum.VBLF + "         '0', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '" + ArgBi + "', ";
            SQL += ComNum.VBLF + "         '" + ArgDr + "', ";
            SQL += ComNum.VBLF + "         '1차' ,";
            SQL += ComNum.VBLF + "         SYSDATE, ";
            SQL += ComNum.VBLF + "         '0', ";
            SQL += ComNum.VBLF + "         '0', ";
            SQL += ComNum.VBLF + "         ADMIN.SEQ_ORDERNO.NEXTVAL, ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '" + ArgSucode + "', ";
            SQL += ComNum.VBLF + "         '" + ArgSucode + "', ";
            SQL += ComNum.VBLF + "         '20') ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            clsDB.setCommitTran(pDbCon);

            rtnVal = true;

            return rtnVal;
        }

        public bool Insert_Opd_Order_EXAM(PsmhDb pDbCon, string ArgPtno, string ArgDept, string ArgDr, string ArgBi, string ArgSucode, string ArgDrSabun)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;
            int intRowCnt = 0;

            bool rtnVal = false;

            clsDB.setBeginTran(pDbCon);

            SQL = "";
            SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_MED + "OCS_OORDER ";
            SQL += ComNum.VBLF + "        (PTNO, BDATE, DEPTCODE, ";
            SQL += ComNum.VBLF + "         SEQNO, ORDERCODE, SUCODE, ";
            SQL += ComNum.VBLF + "         BUN, SLIPNO, REALQTY, ";
            SQL += ComNum.VBLF + "         QTY, NAL, GBDIV, ";
            SQL += ComNum.VBLF + "         DOSCODE, GBBOTH, GBINFO, ";
            SQL += ComNum.VBLF + "         GBER, GBSELF, GBSPC, ";
            SQL += ComNum.VBLF + "         BI, DRCODE, REMARK, ";
            SQL += ComNum.VBLF + "         ENTDATE, GBSUNAP, TUYAKNO, ";
            SQL += ComNum.VBLF + "         ORDERNO, MULTI, MULTIREMARK, ";
            SQL += ComNum.VBLF + "         DUR, RESV, SCODESAYU, ";
            SQL += ComNum.VBLF + "         SCODEREMARK, GBSEND, CORDERCODE, ";
            SQL += ComNum.VBLF + "         CSUCODE, CBUN) ";
            SQL += ComNum.VBLF + " VALUES ('" + ArgPtno + "' , ";
            SQL += ComNum.VBLF + "         TRUNC(SYSDATE), ";
            SQL += ComNum.VBLF + "         '" + ArgDept + "', ";
            SQL += ComNum.VBLF + "         99, ";
            SQL += ComNum.VBLF + "         '" + ArgSucode + "', ";
            SQL += ComNum.VBLF + "         '" + ArgSucode + "', ";
            SQL += ComNum.VBLF + "         '64', ";
            SQL += ComNum.VBLF + "         '0050', ";
            SQL += ComNum.VBLF + "         '1', ";
            SQL += ComNum.VBLF + "         '1', ";
            SQL += ComNum.VBLF + "         '1', ";
            SQL += ComNum.VBLF + "         '1', ";
            if (ArgSucode == "NCOV-1")
            {
                SQL += ComNum.VBLF + "         '090', ";           //용법
            }
            else
            {
                SQL += ComNum.VBLF + "         '', ";           //용법
            }
            //SQL += ComNum.VBLF + "         '', ";           //용법
            SQL += ComNum.VBLF + "         '0', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            if (ArgSucode == "NCOV-1")
            {
                SQL += ComNum.VBLF + "         '0', ";            //급여/비급여
            }
            else
            {
                SQL += ComNum.VBLF + "         '2', ";            //급여/비급여
            }
            //SQL += ComNum.VBLF + "         '2', ";            //급여/비급여
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '" + ArgBi + "', ";
            SQL += ComNum.VBLF + "         '" + ArgDr + "', ";
            SQL += ComNum.VBLF + "         '' ,";
            SQL += ComNum.VBLF + "         SYSDATE, ";
            SQL += ComNum.VBLF + "         '0', ";
            SQL += ComNum.VBLF + "         '0', ";
            SQL += ComNum.VBLF + "         ADMIN.SEQ_ORDERNO.NEXTVAL, ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '', ";
            SQL += ComNum.VBLF + "         '" + ArgSucode + "', ";
            SQL += ComNum.VBLF + "         '" + ArgSucode + "', ";
            SQL += ComNum.VBLF + "         '640') ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon);
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

            clsDB.setCommitTran(pDbCon);

            rtnVal = true;

            return rtnVal;
        }

        public bool Ins_Opd_Sunap(PsmhDb pDbCon, string ArgPano, string ArgAmt, int nSeqNo, string ArgDept, string ArgBi, string ArgPart, string ArgReMark, string ArgPrt, string ArgGubun)
        {
            string SQL = string.Empty;
            string SqlErr = string.Empty;
            string strMsg = string.Empty;
            int intRowCnt = 0;

            bool rtnVal = false;

            ComFunc.ReadSysDate(pDbCon);

            clsDB.setBeginTran(pDbCon);
            
            SQL = "";
            SQL += " INSERT INTO " + ComNum.DB_PMPA + "OPD_SUNAP (                                      \r\n";
            SQL += "        ACTDATE, PANO, AMT, PART, SEQNO,STIME, BIGO, REMARK,DEPTCODE,BI,CARDGB)     \r\n";
            SQL += " VALUES (                                                                           \r\n";
            SQL += "        TO_DATE('" + clsPublic.GstrSysDate + "', 'YYYY-MM-DD')                      \r\n";
            SQL += "        ,'" + ArgPano + "'                                                          \r\n";
            SQL += "        , " + ArgAmt + "                                                            \r\n";
            SQL += "        ,'" + ArgPart + "'                                                          \r\n";
            SQL += "        , " + nSeqNo + "                                                            \r\n";
            SQL += "        ,'" + clsPublic.GstrSysTime + "'                                            \r\n";
            SQL += "        ,'" + ArgPrt + "'                                                           \r\n";
            SQL += "        ,'" + ArgReMark + "'                                                        \r\n";
            SQL += "        ,'" + ArgDept + "'                                                          \r\n";
            SQL += "        ,'" + ArgBi + "'                                                            \r\n";
            SQL += "        ,'" + ArgGubun + "'                                                         \r\n";
            SQL += "        )                                                                               ";
            SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, pDbCon);

            if (SqlErr != "")
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(SqlErr);
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal; 
            }

            clsDB.setCommitTran(pDbCon);

            rtnVal = true;

            return rtnVal;
        }
    }
}

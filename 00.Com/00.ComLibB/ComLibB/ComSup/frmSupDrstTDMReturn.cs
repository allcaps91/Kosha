using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using ComEmrBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmSupDrstTDMReturn : Form
    {
        private struct Pat_Info
        {
            public int WEIGHT;
            public int HEIGHT;
            public string PANO;
            public string INDATE;
            public string SEX;
            public string DEPTCODE;
            public int AGE;
            public double IBW;
        }

        string SEQNO    = string.Empty;
        string REQDATE  = string.Empty;
        string JEPDATE  = string.Empty;

        string PROGRESS = "000";

        private struct SPD_INFO
        {        
            public int ROW;
            public int COL;
        }

        Dictionary<string, SPD_INFO> Keys_SPD = null;

        Pat_Info PAT = new Pat_Info();

        /// <summary>
        /// 검사교ㅕㄹ가
        /// </summary>
        frmViewResult frmViewResultX = null;

        /// <summary>
        /// 의뢰서 SEQNO로
        /// </summary>
        /// <param name="SEQNO"></param>
        public frmSupDrstTDMReturn(string SEQNO)
        {
            this.SEQNO = SEQNO;
            InitializeComponent();
        }

        private void frmSupDrstTDMReturn_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close();
                return;
            }

            //폼 기본값 세팅 등
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

            if (clsType.User.IdNumber.Equals("27111") ||
                clsType.User.IdNumber.Equals("48579") ||
                clsType.User.IdNumber.Equals("52301"))
            {
                //3분만 회신서 작성가능.
            }
            else
            {
                btnSave.Enabled = false;
                btnSaveTemp.Enabled = false;
                btnDelete.Enabled = false;
            }

            #region TAG 값 설정
            Keys_SPD = new Dictionary<string, SPD_INFO>();

            for (int ROW = 0; ROW < SSMain.ActiveSheet.RowCount; ROW++)
            {
                for (int COL = 0; COL < SSMain.ActiveSheet.ColumnCount; COL++)
                {
                    if (SSMain.ActiveSheet.Cells[ROW, COL].Tag.NotEmpty() &&
                        Keys_SPD.ContainsKey(SSMain.ActiveSheet.Cells[ROW, COL].Tag.ToString()) == false)
                    {
                        Keys_SPD.Add
                        (
                            SSMain.ActiveSheet.Cells[ROW, COL].Tag.ToString(),
                            new SPD_INFO
                            {
                                ROW = ROW,
                                COL = COL
                            }
                        );
                    }
                }
            }
            #endregion

            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return;

            Set_PatInfo();

            Set_Content();
        }

        private void Set_Content()
        {
            MParameter mParameter = new MParameter();

            mParameter.AppendSql("  SELECT  A.SEQNO                                                                                     ");
            mParameter.AppendSql("  	,   A.PROGRESS                                                                                  ");
            mParameter.AppendSql("  	,   A.PTNO                                                                                      ");
            mParameter.AppendSql("  	,   TO_CHAR(A.JEPDATE, 'YYYY-MM-DD') JEPDATE                                                    ");
            mParameter.AppendSql("  	,   B.SUBGB                                                                                     ");
            mParameter.AppendSql("  	,   B.SUBVAL                                                                                    ");
            mParameter.AppendSql("    FROM KOSMOS_OCS.OCS_TDM_RETURN A                                                                  ");
            mParameter.AppendSql("   INNER JOIN KOSMOS_OCS.OCS_TDM_RETURNSUB B                                                          ");
            mParameter.AppendSql("      ON A.SEQNO = B.SEQNO                                                                            ");
            mParameter.AppendSql("   WHERE A.SEQNO = :SEQNO                                                                             ");
            mParameter.AppendSql("     AND B.SUBGB NOT IN ('001', '002')                                                                    ");

            mParameter.AppendSql("   UNION ALL                                                                                          ");
            mParameter.AppendSql("  SELECT  :SEQNO                                                                                      ");
            mParameter.AppendSql("  	,   '000'                                                                                       ");
            mParameter.AppendSql("  	,   '' AS PTNO                                                                                  ");
            mParameter.AppendSql("  	,   '' AS JEPDATE                                                                               ");
            mParameter.AppendSql("  	,   CASE WHEN A.SUBGB = '000' THEN '003' ELSE A.SUBGB END SUBGB                                                                                     ");
            mParameter.AppendSql("  	,   A.SUBVAL                                                                                    ");
            mParameter.AppendSql("    FROM KOSMOS_OCS.OCS_TDM_MASTERSUB A                                                               ");
            mParameter.AppendSql("   WHERE A.SEQNO = :SEQNO                                                                             ");
            mParameter.AppendSql("     AND A.SUBGB IN ('000', '001', '002')                                                                    ");


            mParameter.Add("SEQNO", SEQNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int16);

            List<Dictionary<string, object>> dt = clsDB.ExecuteReader(mParameter, clsDB.DbCon);
            if (dt.Count == 0 || dt.Count > 0 && dt[0]["PTNO"].IsNullOrEmpty())
            {
                Set_Exam_Result();
                return;
            }

            PROGRESS = dt[0]["PROGRESS"].To<string>();
            SEQNO    = dt[0]["SEQNO"].To<string>();
            if (dt[0]["PTNO"].To<string>().NotEmpty())
            {
                PAT.PANO = dt[0]["PTNO"].To<string>();
            }
            JEPDATE  = dt[0]["JEPDATE"].To<string>();

            for(int i = 0; i < dt.Count; i++)
            {
                if (Keys_SPD.Any(d => d.Key.Left(3).Equals(dt[i]["SUBGB"].ToString())))
                {
                    SPD_INFO sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Left(3).Equals(dt[i]["SUBGB"].ToString())).Value;
                    SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[i]["SUBVAL"].ToString();
                }
            }
        }

        /// <summary>
        /// 키, 몸무게, IBW등 설정
        /// </summary>
        private void Set_PatInfo()
        {
            MParameter mParameter = new MParameter();

            mParameter.AppendSql(" WITH CHART_DATA AS                                                                                                  ");
            mParameter.AppendSql(" (                                                                                                                   ");
            mParameter.AppendSql("   SELECT  A.PTNO                                                                                                    ");
            mParameter.AppendSql("   	,	 I.SNAME                                                                                                   ");
            mParameter.AppendSql("   	,	 I.WARDCODE || '/' || I.ROOMCODE AS WARD_INFO                                                              ");
            mParameter.AppendSql("   	,	 A.MEDDEPTCD                                                                                               ");
            mParameter.AppendSql("   	,	 (SELECT DEPTNAMEK  FROM KOSMOS_PMPA.BAS_CLINICDEPT WHERE DEPTCODE = A.MEDDEPTCD) AS DEPTNAMEK             ");
            mParameter.AppendSql("   	,	 (SELECT DRNAME FROM KOSMOS_OCS.OCS_DOCTOR WHERE DRCODE = I.DRCODE) AS DRNAME                              ");
            mParameter.AppendSql("   	,	 I.SEX                                                                                                     ");
            mParameter.AppendSql("   	,	 I.AGE                                                                                                     ");
            mParameter.AppendSql("   	,	 I.HEIGHT                                                                                                   ");
            mParameter.AppendSql("   	,	 I.WEIGHT                                                                                                   ");
            mParameter.AppendSql("   	,	 A.EMRNO                                                                                                   ");
            mParameter.AppendSql("   	,	 A.EMRNOHIS                                                                                                ");
            mParameter.AppendSql("   	,	 A.CHARTDATE                                                                                               ");
            mParameter.AppendSql("   	,	 A.CHARTTIME                                                                                               ");
            mParameter.AppendSql("   	,	 TO_CHAR(I.INDATE,  'YYYY-MM-DD')  INDATE                                                                  ");
            mParameter.AppendSql("   	,	 TO_CHAR(M.REQDATE, 'YYYY-MM-DD') REQDATE                                                                  ");
            mParameter.AppendSql("     FROM KOSMOS_EMR.AEMRCHARTMST A                                                                                  ");
            mParameter.AppendSql("    INNER JOIN KOSMOS_OCS.OCS_TDM_MASTER M                                                                           ");
            mParameter.AppendSql("       ON M.SEQNO = :SEQNO                                                                                           ");
            mParameter.AppendSql("    INNER JOIN KOSMOS_PMPA.IPD_NEW_MASTER I                                                                          ");
            mParameter.AppendSql("       ON M.IPDNO = I.IPDNO                                                                                          ");
            mParameter.AppendSql("    WHERE A.PTNO = M.PTNO                                                                                            ");
            mParameter.AppendSql("      AND FORMNO IN (1562, 1969, 2135, 2201, 2431, 3150)                                                              ");
            mParameter.AppendSql("	    AND A.MEDFRDATE = TO_CHAR(I.INDATE, 'YYYYMMDD')	                                                               ");
            mParameter.AppendSql(")                                                                                                                    ");

            mParameter.AppendSql("  ,   PAT_DATA AS                                                                                                                                                  ");
            mParameter.AppendSql(" (                                                                                                                                                                 ");
            mParameter.AppendSql(" SELECT    A.EMRNO                                                                                                                                                 ");
            mParameter.AppendSql("   	,	 A.EMRNOHIS                                                                                                                                              ");
            mParameter.AppendSql("   	,	NVL(HEIGHT,  (SELECT ITEMVALUE                                                                                                                                       ");
            mParameter.AppendSql("   			FROM KOSMOS_EMR.AEMRCHARTROW                                                                                                                         ");
            mParameter.AppendSql(" 		       WHERE EMRNO = A.EMRNO                                                                                                                                 ");
            mParameter.AppendSql("   		   	 AND EMRNOHIS = A.EMRNOHIS                                                                                                                           ");
            mParameter.AppendSql("   		     AND ITEMCD = 'I0000000002'                                                                                                                          ");
            mParameter.AppendSql("	         )) AS HEIGHT                                                                                                                                             ");
            mParameter.AppendSql("	    ,	NVL(WEIGHT, (SELECT ITEMVALUE                                                                                                                                       ");
            mParameter.AppendSql("   			FROM KOSMOS_EMR.AEMRCHARTROW                                                                                                                         ");
            mParameter.AppendSql("   		   WHERE EMRNO = A.EMRNO                                                                                                                                 ");
            mParameter.AppendSql("   		   	 AND EMRNOHIS = A.EMRNOHIS                                                                                                                           ");
            mParameter.AppendSql("   		     AND ITEMCD = 'I0000000418'                                                                                                                          ");
            mParameter.AppendSql("	         )) AS WEIGHT                                                                                                                                              ");
            mParameter.AppendSql("   FROM CHART_DATA A                                                                                                                                               ");
            mParameter.AppendSql("  WHERE (CHARTDATE || CHARTTIME) = (SELECT MAX(CHARTDATE || CHARTTIME) FROM CHART_DATA)                                                                            ");
            mParameter.AppendSql(" )                                                                                                                                                                 ");

            mParameter.AppendSql(" SELECT    PTNO                                                                                                                                                    ");
            mParameter.AppendSql("   	,	 SNAME                                                                                                                                                   ");
            mParameter.AppendSql("   	,	 WARD_INFO                                                                                                                                               ");
            mParameter.AppendSql("   	,	 MEDDEPTCD                                                                                                                                               ");
            mParameter.AppendSql("   	,	 DEPTNAMEK                                                                                                                                               ");
            mParameter.AppendSql("   	,	 DEPTNAMEK AS DEPTNAMEK2                                                                                                                                 ");
            mParameter.AppendSql("   	,	 DRNAME                                                                                                                                                  ");
            mParameter.AppendSql("   	,	 CASE WHEN SEX = 'M' THEN '남' ELSE '여' END SEX                                                                                                          ");
            mParameter.AppendSql("   	,	 CASE WHEN SEX = 'M' THEN TRUNC(50 + 2.3 * ((TO_NUMBER(B.HEIGHT) / 2.54) - 60), 2) ELSE TRUNC(45.5 + 2.3 * ((TO_NUMBER(B.HEIGHT) / 2.54) - 60), 2) END IBW     ");
            mParameter.AppendSql("   	,	 AGE   	                                                                                                                                                 ");
            mParameter.AppendSql("   	,	 INDATE                                                                                                                                                  ");
            mParameter.AppendSql("   	,	 REQDATE                                                                                                                                                 ");
            mParameter.AppendSql("   	,	 B.HEIGHT                                                                                                                                                  ");
            mParameter.AppendSql("   	,	 B.WEIGHT                                                                                                                                                   ");
            mParameter.AppendSql("   FROM CHART_DATA A                                                                                                                                               ");
            mParameter.AppendSql("  INNER JOIN PAT_DATA B                                                                                                                                            ");
            mParameter.AppendSql("     ON A.EMRNO = B.EMRNO                                                                                                                                          ");
            mParameter.AppendSql("    AND A.EMRNOHIS = B.EMRNOHIS                                                                                                                                    ");

            mParameter.Add("SEQNO", SEQNO);

            List<Dictionary<string, object>> dt = clsDB.ExecuteReader(mParameter, clsDB.DbCon);
            if (dt.Count == 0)
                return;

            Set_Data(dt);
        }


        private void Set_Data(List<Dictionary<string, object>> dt)
        {
            #region 데이터 설정
            if (dt[0].ContainsKey("INDATE"))
            {
                PAT.INDATE = dt[0]["INDATE"].To<string>();
            }

            if (dt[0].ContainsKey("MEDDEPTCD"))
            {
                PAT.DEPTCODE = dt[0]["MEDDEPTCD"].To<string>();
            }

            if (dt[0].ContainsKey("PTNO"))
            {
                PAT.PANO = dt[0]["PTNO"].To<string>();
            }

            //if (dt[0].ContainsKey("PTN"))
            //{
            //    PAT.PANO = dt[0]["PTN"].To<string>();
            //}

            if (dt[0].ContainsKey("REQDATE"))
            {
                REQDATE = dt[0]["REQDATE"].To<string>();
            }

            if (dt[0].ContainsKey("REQ"))
            {
                REQDATE = dt[0]["REQ"].To<string>();
            }

            if (dt[0].ContainsKey("HEIGHT"))
            {
                PAT.HEIGHT = dt[0]["HEIGHT"].To<int>(0);
            }

            if (dt[0].ContainsKey("WEIGHT"))
            {
                PAT.WEIGHT = dt[0]["WEIGHT"].To<int>(0);
            }

            if (dt[0].ContainsKey("AGE"))
            {
                PAT.AGE = dt[0]["AGE"].To<int>(0);
            }

            if (dt[0].ContainsKey("SEX"))
            {
                PAT.SEX = dt[0]["SEX"].To<string>();
            }

            if (dt[0].ContainsKey("IBW"))
            {
                PAT.IBW = dt[0]["IBW"].To<double>(0);
            }

            dt[0]["BWADJ"] = dt[0]["IBW"].To<double>(0) +  ((dt[0]["WEIGHT"].To<double>(0) - dt[0]["IBW"].To<double>(0)) * 0.4);
            #endregion

            foreach (KeyValuePair<string, SPD_INFO> keyValue in Keys_SPD)
            {
                if (dt.Any(d => d.ContainsKey(keyValue.Key)))
                {
                    SPD_INFO sPD_INFO = keyValue.Value;
                    SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[0][keyValue.Key].ToString();
                }
            }

            SPD_INFO sPD_INFO2 = Keys_SPD.FirstOrDefault(d => d.Key.Equals("V14")).Value;
            SSMain.ActiveSheet.Cells[sPD_INFO2.ROW, sPD_INFO2.COL].Text = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>().ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 의뢰 약물 등 표시.
        /// </summary>
        private void Set_Exam_Result()
        {
            Dictionary<string, string> CodeList = new Dictionary<string, string>();
            CodeList.Add("MI35AR", "M00");
            CodeList.Add("DR08", "D00");
            CodeList.Add("CR35A", "ALT");
            CodeList.Add("CR34A", "AST");
            CodeList.Add("CR32C", "ALB");
            CodeList.Add("CR42A", "SCR");
            CodeList.Add("CRC", "SCR");
            CodeList.Add("SE04B", "CRP");

            MParameter mParameter = new MParameter();

            mParameter.AppendSql("WITH EXAM_DATA AS                                                                                                                             ");
            mParameter.AppendSql("(                                                                                                                                             ");
            mParameter.AppendSql("  SELECT  A.BLOODDATE                                                                                                                         ");
            mParameter.AppendSql("  	,   C.RESULTDATE                                                                                                                        ");
            mParameter.AppendSql("  	,   C.SUBCODE                                                                                                                           ");
            mParameter.AppendSql("  	,   C.RESULT                                                                                                                            ");
            mParameter.AppendSql("  	, ROW_NUMBER() OVER(PARTITION BY C.SUBCODE ORDER BY C.RESULTDATE DESC) AS ROW_NUM                                                       ");
            mParameter.AppendSql("    FROM KOSMOS_OCS.EXAM_SPECMST A                                                                                                            ");
            mParameter.AppendSql("   INNER JOIN KOSMOS_OCS.EXAM_RESULTC C                                                                                                       ");
            mParameter.AppendSql("      ON A.SPECNO = C.SPECNO                                                                                                                  ");
            mParameter.AppendSql("     AND C.RESULTDATE >= TO_DATE(:REQDATE, 'YYYY-MM-DD HH24:MI')                                                                              ");
            mParameter.AppendSql("     AND C.RESULT IS NOT NULL                                                                                                                 ");
            mParameter.AppendSql("     AND C.SUBCODE IN( :SBCODE)                                                                                                               ");
            mParameter.AppendSql("   WHERE A.PANO = :PANO                                                                                                                       ");
            mParameter.AppendSql("     AND A.BDATE >= TO_DATE(:INDATE, 'YYYY-MM-DD')                                                                                            ");
            mParameter.AppendSql(")                                                                                                                                             ");

            mParameter.AppendSql("  SELECT  TO_CHAR(BLOODDATE,  'YYYY-MM-DD HH24:MI:SS') BLOODDATE                                                                              ");
            mParameter.AppendSql("  	,   TO_CHAR(RESULTDATE, 'YYYY-MM-DD HH24:MI:SS') RESULTDATE                                                                             ");
            mParameter.AppendSql("  	,   SUBCODE                                                                                                                             ");
            mParameter.AppendSql("  	,   RESULT                                                                                                                              ");
            mParameter.AppendSql("  	,   ROW_NUM                                                                                                                             ");
            mParameter.AppendSql("    FROM EXAM_DATA                                                                                                                            ");
            mParameter.AppendSql("   WHERE ROW_NUM <= 2                                                                                                                         ");
            mParameter.AppendSql("   ORDER BY RESULTDATE DESC                                                                                                                   ");

            mParameter.Add("PANO", PAT.PANO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
            mParameter.Add("INDATE", PAT.INDATE);
            mParameter.Add("REQDATE", REQDATE + " 00:00");

            mParameter.AddInStatement("SBCODE", CodeList.Keys.ToList(), Oracle.ManagedDataAccess.Client.OracleDbType.Char);

            List<Dictionary<string, object>> dt = clsDB.ExecuteReader(mParameter, clsDB.DbCon);
            if (dt.Count == 0)
                return;

            List<Dictionary<string, object>> sub = null;

            foreach (KeyValuePair<string, string> keyValue in CodeList)
            {
                if (Keys_SPD.Any(d => d.Key.Equals(keyValue.Value)) && dt.Any(r => r.Any(f => f.Value.Trim().Equals(keyValue.Key))))
                {
                    SPD_INFO sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals(keyValue.Value)).Value;

                    sub = dt.Where(r => r.Any(f => f.Value.Trim().Equals(keyValue.Key))).ToList();
                    if (sub == null)
                        continue;
                    
                    SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = sub[0]["RESULT"].ToString();

                    if (keyValue.Value.Equals("SCR"))
                    {
                        double SCR = (140 - PAT.AGE) * PAT.WEIGHT / (72 * SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text.To<double>(0));
                        sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("CRC")).Value;
                        if (PAT.SEX.Equals("F"))
                        {
                            SCR *= 0.85;
                        }
                        SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = Math.Round(SCR, 2).To<string>();
                    }
                }
            }

            if (sub != null)
            {
                sub.Clear();
            }

            sub = dt.Where(r => r.Any(f => f.Value.Trim().Equals("DR08"))).ToList();
            if (sub == null)
                return;

            SPD_INFO item = Keys_SPD.FirstOrDefault(d => d.Key.Equals("D00")).Value;
            SSMain.ActiveSheet.Cells[item.ROW, item.COL].Text = sub[0]["RESULTDATE"].ToString();

            item = Keys_SPD.FirstOrDefault(d => d.Key.Equals("D01")).Value;
            SSMain.ActiveSheet.Cells[item.ROW, item.COL].Text = sub[0]["RESULT"].ToString();

            if (sub.Count > 1)
            {
                item = Keys_SPD.FirstOrDefault(d => d.Key.Equals("D02")).Value;
                SSMain.ActiveSheet.Cells[item.ROW, item.COL].Text = sub[1]["RESULTDATE"].ToString();

                item = Keys_SPD.FirstOrDefault(d => d.Key.Equals("D03")).Value;
                SSMain.ActiveSheet.Cells[item.ROW, item.COL].Text = sub[1]["RESULT"].ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false)
                return;

            ////시퀀스
            MTSResult result = SaveMstData(sender);
            if (result.Result != ResultType.Success)
            {
                result.ShowMessage();
                return;
            }
            else
            {
                result = SaveSubData(SEQNO);
                result.ShowMessage();
            }
        }

        private MTSResult SaveMstData(object sender)
        {
            MTSResult result = new MTSResult(true);
            MParameter mParameter = new MParameter();

            try
            {
                mParameter.AppendSql("	  MERGE INTO KOSMOS_OCS.OCS_TDM_RETURN D			 ");
                mParameter.AppendSql("	  USING DUAL			                             ");
                mParameter.AppendSql("	     ON (D.SEQNO = :SEQNO)                           ");
                mParameter.AppendSql("	   WHEN MATCHED THEN                                 ");
                mParameter.AppendSql("	     UPDATE                                          ");
                mParameter.AppendSql("	        SET CREATEDATE = SYSDATE                     ");
                mParameter.AppendSql("	        ,   PROGRESS   = :PROGRESS                   ");
                mParameter.AppendSql("	   WHEN NOT MATCHED THEN                             ");
                mParameter.AppendSql("	  INSERT 				                             ");
                mParameter.AppendSql("	  (                            				         ");
                mParameter.AppendSql("	      SEQNO											 ");
                mParameter.AppendSql("	    , PTNO                                           ");
                mParameter.AppendSql("	    , SABUN                                          ");
                mParameter.AppendSql("	    , PROGRESS                                       ");
                mParameter.AppendSql("	    , JEPDATE                                        ");
                mParameter.AppendSql("	    , CREATEDATE                                     ");
                mParameter.AppendSql("	  )                            				         ");

                mParameter.AppendSql("	    VALUES                                           ");
                mParameter.AppendSql("	  (                            				         ");
                mParameter.AppendSql("	         :SEQNO										 ");
                mParameter.AppendSql("	       , :PTNO                                       ");
                mParameter.AppendSql("	       , :SABUN                                      ");
                mParameter.AppendSql("	       , :PROGRESS                                   ");
                mParameter.AppendSql("	       , :JEPDATE                                    ");
                mParameter.AppendSql("	       , SYSDATE                                     ");
                mParameter.AppendSql("	  )                            				         ");

                mParameter.Add("SEQNO", SEQNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int32);
                mParameter.Add("PTNO", PAT.PANO, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                mParameter.Add("SABUN", clsType.User.Sabun, Oracle.ManagedDataAccess.Client.OracleDbType.Char);
                mParameter.Add("PROGRESS", sender.Equals(btnSaveTemp) ? "001" : "002", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2);
                mParameter.Add("JEPDATE", ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>().ToString("yyyy-MM-dd"));

                result.SetSuccessCountPlus(clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon));
                result.SetSuccessMessage("저장하였습니다.");
            }
            catch (Exception ex)
            {
                result.SetErrMessage(ex.Message);
            }

            return result;
        }

        private MTSResult SaveSubData(string SEQNO)
        {
            MTSResult result = new MTSResult(true);
            MParameter mParameter = new MParameter();

            try
            {

                mParameter = new MParameter();
                mParameter.AppendSql("DELETE KOSMOS_OCS.OCS_TDM_RETURNSUB			 ");
                mParameter.AppendSql(" WHERE SEQNO = :SEQNO						     ");

                mParameter.Add("SEQNO", SEQNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int32);
                result.SetSuccessCountPlus(clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon));

                foreach (KeyValuePair<string, SPD_INFO> keyValue in Keys_SPD)
                {
                    SPD_INFO sPD_INFO = keyValue.Value;

                    if (SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text.Trim().IsNullOrEmpty())
                        continue;

                    string VALGB = keyValue.Key;
                    if (Encoding.UTF8.GetBytes(VALGB).Length > 3)
                    {
                        VALGB = VALGB.Left(3);
                    }

                    string CONTENT = SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text.Trim();

                    mParameter = new MParameter();
                    mParameter.AppendSql("	  INSERT INTO KOSMOS_OCS.OCS_TDM_RETURNSUB			 ");
                    mParameter.AppendSql("	  (                            				         ");
                    mParameter.AppendSql("	      SEQNO											 ");
                    mParameter.AppendSql("	    , SUBGB                                          ");
                    mParameter.AppendSql("	    , SUBVAL                                         ");
                    mParameter.AppendSql("	  )                            				         ");

                    mParameter.AppendSql("	  SELECT :SEQNO										 ");
                    mParameter.AppendSql("	       , :SUBGB                                      ");
                    mParameter.AppendSql("	       , :SUBVAL                                     ");
                    mParameter.AppendSql("	    FROM DUAL                                        ");
                    mParameter.AppendSql("	   WHERE NOT EXISTS                                  ");
                    mParameter.AppendSql("	  (										             ");
                    mParameter.AppendSql("	  SELECT 1										     ");
                    mParameter.AppendSql("	    FROM KOSMOS_OCS.OCS_TDM_RETURNSUB                ");
                    mParameter.AppendSql("	   WHERE SEQNO = :SEQNO                              ");
                    mParameter.AppendSql("	     AND SUBGB = :SUBGB                              ");
                    mParameter.AppendSql("	  )										             ");

                    mParameter.Add("SEQNO", SEQNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int32);
                    mParameter.Add("SUBGB", VALGB, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2);
                    mParameter.Add("SUBVAL", CONTENT, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2);

                    result.SetSuccessCountPlus(clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon));
                }

                result.SetSuccessMessage("저장하였습니다.");
            }
            catch (Exception ex)
            {
                result.SetErrMessage(ex.Message);
            }

            return result;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false)
                return;

            MTSResult result = new MTSResult(true);
            MParameter mParameter = new MParameter();

            try
            {

                mParameter = new MParameter();
                mParameter.AppendSql("DELETE KOSMOS_OCS.OCS_TDM_RETURN  			 ");
                mParameter.AppendSql(" WHERE SEQNO = :SEQNO						     ");

                mParameter.Add("SEQNO", SEQNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int32);
                result.SetSuccessCountPlus(clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon));

                mParameter = new MParameter();
                mParameter.AppendSql("DELETE KOSMOS_OCS.OCS_TDM_RETURNSUB			 ");
                mParameter.AppendSql(" WHERE SEQNO = :SEQNO						     ");

                mParameter.Add("SEQNO", SEQNO, Oracle.ManagedDataAccess.Client.OracleDbType.Int32);
                result.SetSuccessCountPlus(clsDB.ExecuteNonQuery(mParameter, clsDB.DbCon));

                result.SetSuccessMessage("삭제하였습니다.");
            }
            catch (Exception ex)
            {
                result.SetErrMessage(ex.Message);
            }

            result.ShowMessage();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SSMain_EditModeOff(object sender, EventArgs e)
        {
            if (SSMain.ActiveSheet.ActiveCell.Tag.IsNullOrEmpty())
                return;

            string TAG = SSMain.ActiveSheet.ActiveCell.Tag.ToString();

            if (TAG.Equals("HEIGHT"))
            {
                PAT.HEIGHT = SSMain.ActiveSheet.ActiveCell.Text.Trim().To<int>(0);
            }

            if (TAG.Equals("WEIGHT"))
            {
                PAT.WEIGHT = SSMain.ActiveSheet.ActiveCell.Text.Trim().To<int>(0);
            }

            if (TAG.Equals("IBW"))
            {
                PAT.IBW = SSMain.ActiveSheet.ActiveCell.Text.Trim().To<double>(0);
            }

            double CalcWeight = 0;
            if (PAT.WEIGHT > 1.2 * PAT.IBW)
            {
                CalcWeight = PAT.IBW;
            }
            else if (PAT.WEIGHT < PAT.IBW)
            {
                CalcWeight = PAT.WEIGHT;
            }

            double SCR = 0;
            SPD_INFO sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("SCR")).Value;
            SCR = SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text.To<double>(0);

            double Temp = (140 - PAT.AGE) * CalcWeight / (72 * SCR);
            if (PAT.SEX.Equals("F") || PAT.SEX.Equals("여"))
            {
                Temp *= 0.85; //여자일경우
            }

            sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("CRC")).Value;
            SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = Math.Round(Temp, 2).ToString();
        }

        private void SSMain_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            int ROW = e.Row;
            //if (e.)

            if (e.Row == 18)
            {
                string strFormNo = "1796";
                double dUpdateNo = clsEmrQuery.GetMaxUpdateNo(clsDB.DbCon, VB.Val(strFormNo));
                EmrPatient p = clsEmrChart.SetEmrPatInfoOcs(clsDB.DbCon, PAT.PANO, "I" , PAT.INDATE.Replace("-", ""), PAT.DEPTCODE);
                //ActiveFormWrite = new frmEmrChartFlowOld(fWrite.FmFORMNO.ToString(), fWrite.FmUPDATENO.ToString(), AcpEmr, strEmrNo, "W", this);
                //using (frmEmrChartFlowOld fEmrChartFlowOld = new frmEmrChartFlowOld(strFormNo, dUpdateNo.ToString(), p, strEmrNo, "W", mEmrCallForm))
                using (frmEmrChartFlowOld fEmrChartFlowOld = new frmEmrChartFlowOld(strFormNo, dUpdateNo.ToString(), p, "0", "V"))
                {
                    fEmrChartFlowOld.StartPosition = FormStartPosition.CenterScreen;
                    fEmrChartFlowOld.Owner = this;
                    fEmrChartFlowOld.ShowDialog();
                }
            }
            else if (e.Row == 21)
            {
                if (clsType.User.IdNumber.Equals("27111") ||
                    clsType.User.IdNumber.Equals("48579") ||
                    clsType.User.IdNumber.Equals("52301") ||
                    clsType.User.DrCode.NotEmpty())
                {
                    if (frmViewResultX  != null)
                    {
                        frmViewResultX.rGetDate(PAT.PANO);
                        return;
                    }
                    else
                    {
                        frmViewResultX = new frmViewResult(PAT.PANO);
                        frmViewResultX.StartPosition = FormStartPosition.CenterScreen;
                        frmViewResultX.rEventClosed += FrmViewResultX_rEventClosed;
                        frmViewResultX.Show();
                    }
                }
            }
        }

        private void FrmViewResultX_rEventClosed()
        {
            if (frmViewResultX != null)
            {
                frmViewResultX.Dispose();
                frmViewResultX = null;
            }
        }

        private void SSMain_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {

        }

        private void frmSupDrstTDMReturn_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (frmViewResultX != null)
            {
                frmViewResultX.Dispose();
                frmViewResultX = null;
            }
        }
    }
}

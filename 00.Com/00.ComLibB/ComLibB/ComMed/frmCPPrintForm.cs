using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmCPPrintForm : Form
    {
        string CPCODE = string.Empty;

        private struct SPD_INFO
        {
            public int ROW;
            public int COL;
        }

        Dictionary<string, SPD_INFO> Keys_SPD = null;

        public frmCPPrintForm(string CPCODE)
        {
            this.CPCODE = CPCODE;
            InitializeComponent();
        }

        private void frmCPPrintForm_Load(object sender, EventArgs e)
        {
            //폼 권한 조회
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //}

            ////폼 기본값 세팅 등
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");

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

            Set_CP();
        }

        private void Set_CP()
        {
            MParameter mParameter = new MParameter();
            
            //mParameter.AppendSql("	--구분자(01:자격,02:중단사유,03:제외기준,04:진단코드,05:수술코드,06:지표내용,07:동의서)                                                                                                                                   ");

            mParameter.AppendSql("WITH CP_DATA AS                                                                                                                                                                                                               ");
            mParameter.AppendSql("(                                                                                                                                                                                                                             ");
            mParameter.AppendSql("SELECT	CASE WHEN A.GBIO  = 'I' THEN '□ 응급     ■ 입원  ' ELSE '■ 응급     □ 입원' END GBIO                                                                                                                                   ");
            mParameter.AppendSql("	,	CASE WHEN A.GUBUN = '01' THEN '■ 진단  □ 수술  □ 진단+수술 '                                                                                                                                                               ");
            mParameter.AppendSql("			 WHEN A.GUBUN = '02' THEN '□ 진단  ■ 수술  □ 진단+수술 '                                                                                                                                                               ");
            mParameter.AppendSql("			 WHEN A.GUBUN = '03' THEN '□ 진단  □ 수술  ■ 진단+수술 '                                                                                                                                                               ");
            mParameter.AppendSql("			 END APPLY                                                                                                                                                                                                          ");
            mParameter.AppendSql("	,	CASE WHEN A.SCALE = '01' THEN '■ 입퇴원  □  POST-OP 퇴원  □ 기타 '                                                                                                                                                         ");
            mParameter.AppendSql("			 WHEN A.SCALE = '02' THEN '□ 입퇴원  ■  POST-OP 퇴원  □ 기타'                                                                                                                                                          ");
            mParameter.AppendSql("			 ELSE '□ 입퇴원  □  POST-OP 퇴원  ■ 기타'                                                                                                                                                                              ");
            mParameter.AppendSql("			 END RANGE	                                                                                                                                                                                                        ");
            mParameter.AppendSql("	,	'나이 (  ' || A.FRAGE || '   )세이상 (  ' || A.TOAGE  || '    )세이하 ' AS AGE                                                                                                                                             ");
            mParameter.AppendSql("	,	CASE WHEN B.GUBUN = '01' AND B.CODE = '01' THEN '■ 보험     □ 보호  '                                                                                                                                                    ");
            mParameter.AppendSql("			 WHEN B.GUBUN = '01' AND B.CODE = '02' THEN '□ 보험     ■ 보호  '                                                                                                                                                    ");
            mParameter.AppendSql("			 END BI                                                                                                                                                                                                             ");
            mParameter.AppendSql("	,   '(  ' || TRIM(A.CPDAY) || '   )일' AS CPDAY                                                                                                                                                                             ");
            mParameter.AppendSql("	,	CASE WHEN A.GBINDICATOR = 'Y' THEN '■ 유    □ 무 ' ELSE '□ 유    ■ 무 ' END GBINDICATOR                                                                                                                                  ");
            mParameter.AppendSql("	,	CASE WHEN A.GBAGREE     = 'Y' THEN '■ 유    □ 무 ' ELSE '□ 유    ■ 무 ' END GBAGREE 	                                                                                                                                    ");
            mParameter.AppendSql("	,	B.CODE                                                                                                                                                                                                                  ");
            mParameter.AppendSql("	,	B.NAME                                                                                                                                                                                                                  ");
            mParameter.AppendSql("	,	B.GUBUN                                                                                                                                                                                                                 ");
            mParameter.AppendSql("	,	A.SDATE                                                                                                                                                                                                                 ");
            mParameter.AppendSql("	,	A.CPCODE                                                                                                                                                                                                                ");
            mParameter.AppendSql("	,	B.DSPSEQ AS ROW_NUM                                                                                                                                                                                                     ");
            mParameter.AppendSql("  FROM ADMIN.OCS_CP_MAIN A                                                                                                                                                                                               ");
            mParameter.AppendSql(" INNER JOIN ADMIN.OCS_CP_SUB B                                                                                                                                                                                           ");
            mParameter.AppendSql("    ON A.CPCODE = B.CPCODE                                                                                                                                                                                                    ");
            mParameter.AppendSql("   AND A.SDATE = B.SDATE                                                                                                                                                                                                      ");
            mParameter.AppendSql("   AND B.GUBUN != '07'                                                                                                                                                                                                        ");
            mParameter.AppendSql(" WHERE A.CPCODE = :CPCODE                                                                                                                                                                                                     ");
            mParameter.AppendSql("   AND A.SDATE = (SELECT MAX(SDATE) FROM ADMIN.OCS_CP_MAIN WHERE CPCODE = A.CPCODE)                                                                                                                                      ");
            mParameter.AppendSql(" ORDER BY B.GUBUN, B.DSPSEQ                                                                                                                                                                                                   ");
            mParameter.AppendSql(" )                                                                                                                                                                                                                            ");
            mParameter.AppendSql(" SELECT GBIO                                                                                                                                                                                                                  ");
            mParameter.AppendSql(" 	,	APPLY                                                                                                                                                                                                                   ");
            mParameter.AppendSql(" 	,	RANGE                                                                                                                                                                                                                   ");
            mParameter.AppendSql(" 	,	AGE                                                                                                                                                                                                                     ");
            mParameter.AppendSql(" 	,	BI                                                                                                                                                                                                                      ");
            mParameter.AppendSql(" 	,	CPDAY                                                                                                                                                                                                                   ");
            mParameter.AppendSql(" 	,	GBINDICATOR                                                                                                                                                                                                             ");
            mParameter.AppendSql(" 	,	GBAGREE                                                                                                                                                                                                                 ");
            mParameter.AppendSql("	,	CASE WHEN GUBUN = '01' THEN 'BI'                                                                                                                                                                                        ");
            mParameter.AppendSql("	         WHEN GUBUN = '02' THEN 'CANCER'                                                                                                                                                                                    ");
            mParameter.AppendSql("			 WHEN GUBUN = '03' THEN 'DROP'     	                                                                                                                                                                                ");
            mParameter.AppendSql("			 WHEN GUBUN = '04' THEN 'ILLCODE'  			                                                                                                                                                                        ");
            mParameter.AppendSql("			 WHEN GUBUN = '05' THEN 'OPCODE'  			                                                                                                                                                                        ");
            mParameter.AppendSql("	  	 END TAG_NAME  	                                                                                                                                                                                                        ");
            mParameter.AppendSql("  ,   ROW_NUM  	                                                                                                                                                                                                            ");
            mParameter.AppendSql(" 	,	GUBUN                                                                                                                                                                                                                   ");
            mParameter.AppendSql(" 	,	CODE                                                                                                                                                                                                                    ");
            mParameter.AppendSql(" 	,	NAME                                                                                                                                                                                                                    ");
            mParameter.AppendSql(" 	,	CASE WHEN GUBUN = '01' AND ROW_NUM = 0 THEN (SELECT REPLACE(WM_CONCAT(SUB.NAME), ',', '\r\n') FROM ADMIN.OCS_CP_SUB SUB WHERE SUB.CPCODE = A.CPCODE AND SUB.SDATE = A.SDATE AND SUB.GUBUN = '07') END AGREERECORD  ");
            mParameter.AppendSql(" 	,	CASE WHEN GUBUN = '01' AND ROW_NUM = 0 THEN (SELECT BASNAME FROM ADMIN.BAS_BASCD SUB WHERE SUB.GRPCDB = 'CP관리' AND SUB.GRPCD  = 'CP코드관리' AND SUB.BASCD = A.CPCODE) END CPNAME                                 ");
            mParameter.AppendSql("   FROM CP_DATA A                                                                                                                                                                                                             ");

            mParameter.Add("CPCODE", CPCODE);

            List<Dictionary<string, object>> dt = clsDB.ExecuteReader(mParameter, clsDB.DbCon);
            if (dt.Count == 0)
                return;


            SPD_INFO sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("GBIO")).Value;
            SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[0]["GBIO"].ToString();

            sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("CPNAME")).Value;
            SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[0]["CPNAME"].ToString();

            sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("APPLY")).Value;
            SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[0]["APPLY"].ToString();

            sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("RANGE")).Value;
            SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[0]["RANGE"].ToString();

            sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("AGE")).Value;
            SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[0]["AGE"].ToString();

            sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("CPDAY")).Value;
            SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[0]["CPDAY"].ToString();

            sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("GBINDICATOR")).Value;
            SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[0]["GBINDICATOR"].ToString();

            sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("GBAGREE")).Value;
            SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[0]["GBAGREE"].ToString();

            sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("AGREERECORD")).Value;
            SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[0]["AGREERECORD"].ToString();

            sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals("BI")).Value;
            SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt.Where(d => d.Any(r => r.Key.Equals("BI") && r.Value.NotEmpty())).FirstOrDefault()["BI"].ToString();


            for (int i = 0; i < dt.Count; i++)
            {
                string FIND_TAG = dt[i]["TAG_NAME"].ToString() + dt[i]["ROW_NUM"].ToString();
                if (Keys_SPD.Any(d => d.Key.Equals(FIND_TAG)))
                {
                    if (dt[i]["TAG_NAME"].ToString().IndexOf("DROP") != -1 ||
                        dt[i]["TAG_NAME"].ToString().IndexOf("CANCER") != -1)
                    {
                        sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals(FIND_TAG)).Value;
                        SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[i]["NAME"].ToString();
                    }
                    else
                    {
                        sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals(FIND_TAG)).Value;
                        SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[i]["CODE"].ToString();

                        FIND_TAG = dt[i]["TAG_NAME"].ToString() + "NAME" + dt[i]["ROW_NUM"].ToString();

                        sPD_INFO = Keys_SPD.FirstOrDefault(d => d.Key.Equals(FIND_TAG)).Value;
                        SSMain.ActiveSheet.Cells[sPD_INFO.ROW, sPD_INFO.COL].Text = dt[i]["NAME"].ToString();
                    }
                    
                }
            }
        }
    }
}

using ComBase;
using ComBase.Controls;
using ComBase.Mvc;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : SupDrst
    /// File Name       : frmSupDrstTDMConsultReturn.cs
    /// Description     : TDM 결과 등록
    /// Author          : 이현종
    /// Create Date     : 2021-07-22
    /// </summary>
    public partial class frmSupDrstTDMConsultReturn : Form
    {
        #region dto
        class TDMReturnModel : BaseDto
        {
            /// <summary>                          
            /// PROGRESS
            /// </summary>                         
            private string progress = string.Empty;
            public string PROGRESS
            {
                get { return progress == null ? string.Empty : progress; }
                set
                {
                    if (progress != value)
                    {
                        progress = value;
                        OnPropertyChanged("PROGRESS");
                    }
                }
            }

            /// <summary>                          
            /// REQDATE
            /// </summary>                         
            private string reqdate = string.Empty;
            public string REQDATE
            {
                get { return reqdate == null ? string.Empty : reqdate; }
                set
                {
                    if (reqdate != value)
                    {
                        reqdate = value;
                        OnPropertyChanged("REQDATE");
                    }
                }
            }

            /// <summary>                          
            /// REQNAME
            /// </summary>                         
            private string reqname = string.Empty;
            public string REQNAME
            {
                get { return reqname == null ? string.Empty : reqname; }
                set
                {
                    if (reqname != value)
                    {
                        reqname = value;
                        OnPropertyChanged("REQNAME");
                    }
                }
            }

            /// <summary>                          
            /// WARDCODE
            /// </summary>                         
            private string wardcode = string.Empty;
            public string WARDCODE
            {
                get { return wardcode == null ? string.Empty : wardcode; }
                set
                {
                    if (wardcode != value)
                    {
                        wardcode = value;
                        OnPropertyChanged("WARDCODE");
                    }
                }
            }

            /// <summary>                          
            /// ROOMCODE
            /// </summary>                         
            private long roomcode;
            public long ROOMCODE
            {
                get { return roomcode; }
                set
                {
                    if (roomcode != value)
                    {
                        roomcode = value;
                        OnPropertyChanged("ROOMCODE");
                    }
                }
            }

            /// <summary>                          
            /// IPDNO
            /// </summary>                         
            private long ipdno;
            public long IPDNO
            {
                get { return ipdno; }
                set
                {
                    if (ipdno != value)
                    {
                        ipdno = value;
                        OnPropertyChanged("IPDNO");
                    }
                }
            }

            /// <summary>                          
            /// SEQNO
            /// </summary>                         
            private long seqno;
            public long SEQNO
            {
                get { return seqno; }
                set
                {
                    if (seqno != value)
                    {
                        seqno = value;
                        OnPropertyChanged("SEQNO");
                    }
                }
            }

            /// <summary>                          
            /// PANO
            /// </summary>                         
            private string pano = string.Empty;
            public string PANO
            {
                get { return pano == null ? string.Empty : pano; }
                set
                {
                    if (pano != value)
                    {
                        pano = value;
                        OnPropertyChanged("PANO");
                    }
                }
            }

            /// <summary>                          
            /// SNAME
            /// </summary>                         
            private string sname = string.Empty;
            public string SNAME
            {
                get { return sname == null ? string.Empty : sname; }
                set
                {
                    if (sname != value)
                    {
                        sname = value;
                        OnPropertyChanged("SNAME");
                    }
                }
            }

            /// <summary>                          
            /// AGE
            /// </summary>                         
            private long age;
            public long AGE
            {
                get { return age; }
                set
                {
                    if (age != value)
                    {
                        age = value;
                        OnPropertyChanged("AGE");
                    }
                }
            }

            /// <summary>                          
            /// SEX
            /// </summary>                         
            private string sex = string.Empty;
            public string SEX
            {
                get { return sex == null ? string.Empty : sex; }
                set
                {
                    if (sex != value)
                    {
                        sex = value;
                        OnPropertyChanged("SEX");
                    }
                }
            }

            /// <summary>                          
            /// DEPTCODE
            /// </summary>                         
            private string deptcode = string.Empty;
            public string DEPTCODE
            {
                get { return deptcode == null ? string.Empty : deptcode; }
                set
                {
                    if (deptcode != value)
                    {
                        deptcode = value;
                        OnPropertyChanged("DEPTCODE");
                    }
                }
            }

            /// <summary>                          
            /// DRNAME
            /// </summary>                         
            private string drname = string.Empty;
            public string DRNAME
            {
                get { return drname == null ? string.Empty : drname; }
                set
                {
                    if (drname != value)
                    {
                        drname = value;
                        OnPropertyChanged("DRNAME");
                    }
                }
            }

            /// <summary>                          
            /// DRUG_NAME
            /// </summary>                         
            private string drug_name = string.Empty;
            public string DRUG_NAME
            {
                get { return drug_name == null ? string.Empty : drug_name; }
                set
                {
                    if (drug_name != value)
                    {
                        drug_name = value;
                        OnPropertyChanged("DRUG_NAME");
                    }
                }
            }

            /// <summary>                          
            /// RTNNAME
            /// </summary>                         
            private string rtnname = string.Empty;
            public string RTNNAME
            {
                get { return rtnname == null ? string.Empty : rtnname; }
                set
                {
                    if (rtnname != value)
                    {
                        rtnname = value;
                        OnPropertyChanged("RTNNAME");
                    }
                }
            }

            /// <summary>                          
            /// RTNDATE
            /// </summary>                         
            private string rtndate = string.Empty;
            public string RTNDATE
            {
                get { return rtndate == null ? string.Empty : rtndate; }
                set
                {
                    if (rtndate != value)
                    {
                        rtndate = value;
                        OnPropertyChanged("RTNDATE");
                    }
                }
            }
        }
        #endregion

        #region 
        class SearchTDMDto : BaseDto
        {
            /// <summary>                          
            /// RADIO
            /// </summary>                         
            private string radio = string.Empty;
            public string RADIO
            {
                get { return radio == null ? string.Empty : radio; }
                set
                {
                    if (radio != value)
                    {
                        radio = value;
                        OnPropertyChanged("RADIO");
                    }
                }
            }
        }
        #endregion

        List<TDMReturnModel> DataList = null;
        List<TDMReturnModel> DataList2 = null;
        SearchTDMDto SearchDto = new SearchTDMDto();
        string RDO_GBN = string.Empty;

        public frmSupDrstTDMConsultReturn()
        {
            InitializeComponent();
        }

        private void frmSupDrstTDMConsultReturn_Load(object sender, EventArgs e)
        {
            #region 스프레드
            ssView.Initialize(new SpreadOption { SpreadSelectionUnit = SelectionUnit.Cell, IsRowSelectColor = false, RowHeight = 28 });
            ssView.AddColumn("진행", nameof(TDMReturnModel.PROGRESS), 80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView.AddColumn("의뢰일", nameof(TDMReturnModel.REQDATE), 100, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView.AddColumn("의뢰자", nameof(TDMReturnModel.REQNAME), 80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView.AddColumn("병동", nameof(TDMReturnModel.WARDCODE), 40, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView.AddColumn("호실", nameof(TDMReturnModel.ROOMCODE), 40, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView.AddColumn("등록번호", nameof(TDMReturnModel.PANO), 80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView.AddColumn("성명", nameof(TDMReturnModel.SNAME), 80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsEditble = false });
            ssView.AddColumn("나이", nameof(TDMReturnModel.AGE), 40, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView.AddColumn("성별", nameof(TDMReturnModel.SEX), 40, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView.AddColumn("과", nameof(TDMReturnModel.DEPTCODE), 40, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView.AddColumn("담당의", nameof(TDMReturnModel.DRNAME), 60, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView.AddColumn("약물", nameof(TDMReturnModel.DRUG_NAME), 150, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsEditble = false });
            ssView.AddColumn("회신자", nameof(TDMReturnModel.RTNNAME), 80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView.AddColumn("회신일자", nameof(TDMReturnModel.RTNDATE), 140, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });

            ssView.VerticalScrollBarPolicy = ScrollBarPolicy.Always;
            ssView.HorizontalScrollBarPolicy = ScrollBarPolicy.AsNeeded;

            ssView2.Initialize(new SpreadOption { SpreadSelectionUnit = SelectionUnit.Cell, IsRowSelectColor = false, RowHeight = 28 });
            ssView2.AddColumn("진행", nameof(TDMReturnModel.PROGRESS), 80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView2.AddColumn("의뢰일", nameof(TDMReturnModel.REQDATE), 100, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView2.AddColumn("의뢰자", nameof(TDMReturnModel.REQNAME), 80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView2.AddColumn("병동", nameof(TDMReturnModel.WARDCODE), 40, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView2.AddColumn("호실", nameof(TDMReturnModel.ROOMCODE), 40, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView2.AddColumn("등록번호", nameof(TDMReturnModel.PANO), 80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView2.AddColumn("성명", nameof(TDMReturnModel.SNAME), 80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsEditble = false });
            ssView2.AddColumn("나이", nameof(TDMReturnModel.AGE), 40, FpSpreadCellType.NumberCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView2.AddColumn("성별", nameof(TDMReturnModel.SEX), 40, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView2.AddColumn("과", nameof(TDMReturnModel.DEPTCODE), 40, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView2.AddColumn("담당의", nameof(TDMReturnModel.DRNAME), 60, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView2.AddColumn("약물", nameof(TDMReturnModel.DRUG_NAME), 150, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Left, IsEditble = false });
            ssView2.AddColumn("회신자", nameof(TDMReturnModel.RTNNAME), 80, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });
            ssView2.AddColumn("회신일자", nameof(TDMReturnModel.RTNDATE), 140, FpSpreadCellType.TextCellType, new SpreadCellTypeOption { Aligen = CellHorizontalAlignment.Center, IsEditble = false });

            ssView2.VerticalScrollBarPolicy = ScrollBarPolicy.Always;
            ssView2.HorizontalScrollBarPolicy = ScrollBarPolicy.AsNeeded;
            #endregion

            dtpEDATE.Value = ComQuery.CurrentDateTime(clsDB.DbCon, "S").To<DateTime>();
            dtpSDATE.Value = dtpEDATE.Value.AddDays(-7);

            GetJepList();

            DataList = new List<TDMReturnModel>();
            DataList2 = new List<TDMReturnModel>();

            rdoGbn1.SetOptions(new RadioButtonOption { DataField = nameof(SearchDto.RADIO), CheckValue = "진행중" });
            rdoGbn3.SetOptions(new RadioButtonOption { DataField = nameof(SearchDto.RADIO), CheckValue = "완료"  });
            rdoGbn4.SetOptions(new RadioButtonOption { DataField = nameof(SearchDto.RADIO), CheckValue = "전체"  });

            SearchDto.RADIO = "전체";

            cboWard_SET();

            panel1.SetDataBinding(SearchDto);

        }

        private void cboWard_SET()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            MParameter mParameter = new MParameter();

            try
            {
                mParameter.AppendSql("SELECT                                                        ");
                mParameter.AppendSql("     WardCode, WardName                                       ");
                mParameter.AppendSql(" FROM " + ComNum.DB_PMPA + "BAS_WARD                          ");
                mParameter.AppendSql("WHERE WARDCODE NOT IN ('IU','NP','2W','IQ','ER')              ");
                mParameter.AppendSql("ORDER BY WardCode                                             ");

                List<TDMReturnModel> models = clsDB.ExecuteReader<TDMReturnModel>(mParameter, clsDB.DbCon);
                models.Insert(0, new TDMReturnModel() { WARDCODE = "전체" });
                models.Add(new TDMReturnModel() { WARDCODE = "SICU" });
                models.Add(new TDMReturnModel() { WARDCODE = "MICU" });
                models.Add(new TDMReturnModel() { WARDCODE = "HD" });
                models.Add(new TDMReturnModel() { WARDCODE = "ER" });
                models.Add(new TDMReturnModel() { WARDCODE = "RA" });
                models.Add(new TDMReturnModel() { WARDCODE = "TTE" });

                cboWard.SetDataSource(models, "WARDCODE", "WARDCODE", false);
                //cboWard.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
            
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, mParameter.NativeQuerySql, clsDB.DbCon); //에러로그 저장
            }
        }

        private void GetJepList()
        {
            cboName.Items.Clear();

            MParameter mParameter = new MParameter();

            mParameter.AppendSql("SELECT NAME                                       ");
            mParameter.AppendSql("  FROM ADMIN.BAS_BCODE                      ");
            mParameter.AppendSql(" WHERE GUBUN = :GUBUN                             ");
            mParameter.AppendSql(" ORDER BY SORT                                    ");

            mParameter.Add("GUBUN", "TDM_대상약물(성분명)");

            List<Dictionary<string, object>> dt = clsDB.ExecuteReader(mParameter, clsDB.DbCon);
            if (dt.Count == 0)
                return;

            for (int i = 0; i < dt.Count; i++)
            {
                cboName.Items.Add(dt[i]["NAME"].ToString().Trim());
            }

            cboName.SelectedIndex = 0;
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataList.Clear();

            MParameter mParameter = new MParameter();

            mParameter.AppendSql("WITH TDM_DATA AS                                                                                                                        ");
            mParameter.AppendSql("(                                                                                                                                       ");
            mParameter.AppendSql("      SELECT	COALESCE((SELECT (SELECT NAME FROM ADMIN.BAS_BCODE WHERE GUBUN = 'TDM진행상황' AND CODE = SUB.PROGRESS)               ");
            mParameter.AppendSql("      		   FROM ADMIN.OCS_TDM_RETURN SUB                                                                                     ");
            mParameter.AppendSql("      		  WHERE SUB.SEQNO = A.SEQNO 		                                                                                      ");
            mParameter.AppendSql("      		), '미확인') AS PROGRESS                                                                                                   ");
            mParameter.AppendSql("      	,	TO_CHAR(A.REQDATE, 'YYYY-MM-DD') REQDATE                                                                                  ");
            mParameter.AppendSql("      	,	(SELECT TRIM(DRNAME) FROM ADMIN.OCS_DOCTOR WHERE DRCODE = I.DRCODE) AS REQNAME	                                        ");
            mParameter.AppendSql("      	,	I.WARDCODE                                                                                                                ");
            mParameter.AppendSql("      	,	I.ROOMCODE                                                                                                                ");
            mParameter.AppendSql("      	,	I.PANO	                                                                                                                  ");
            mParameter.AppendSql("      	,	I.SNAME                                                                                                                   ");
            mParameter.AppendSql("      	,	I.AGE	                                                                                                                  ");
            mParameter.AppendSql("      	,	I.SEX                                                                                                                     ");
            mParameter.AppendSql("      	,	I.DEPTCODE	                                                                                                              ");
            mParameter.AppendSql("      	,	I.IPDNO	                                                                                                              ");
            mParameter.AppendSql("      	,	A.SEQNO	                                                                                                                  ");
            mParameter.AppendSql("      	,	(SELECT (SELECT EMP_NM FROM ADMIN.HR_EMP_BASIS WHERE EMP_ID = SUB.SABUN)                                             ");
            mParameter.AppendSql("      		   FROM ADMIN.OCS_TDM_RETURN SUB                                                                                     ");
            mParameter.AppendSql("      		  WHERE SUB.SEQNO = A.SEQNO 		                                                                                      ");
            mParameter.AppendSql("      		) AS RTNNAME                                                                                                              ");
            mParameter.AppendSql("      	,	(SELECT TO_CHAR(JEPDATE, 'YYYY-MM-DD')                                                                                    ");
            mParameter.AppendSql("      		   FROM ADMIN.OCS_TDM_RETURN SUB                                                                                     ");
            mParameter.AppendSql("      		  WHERE SUB.SEQNO = A.SEQNO 		                                                                                      ");
            mParameter.AppendSql("      		) AS RTNDATE                                                                                                              ");
            mParameter.AppendSql("      	,	(SELECT SUBVAL                                                                                                            ");
            mParameter.AppendSql("      		   FROM ADMIN.OCS_TDM_MASTERSUB SUB                                                                                  ");
            mParameter.AppendSql("      		  WHERE SUB.SEQNO = A.SEQNO 		                                                                                      ");
            mParameter.AppendSql("      		    AND SUB.SUBGB = '000' 		                                                                                          ");
            mParameter.AppendSql("      		) AS DRUG_NAME                                                                                                            ");
            mParameter.AppendSql("        FROM ADMIN.OCS_TDM_MASTER A                                                                                                ");
            mParameter.AppendSql("       INNER JOIN ADMIN.IPD_NEW_MASTER I                                                                                          ");
            mParameter.AppendSql("          ON A.IPDNO = I.IPDNO                                                                                                          ");
            mParameter.AppendSql("       WHERE REQDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                                                                   ");
            mParameter.AppendSql("         AND REQDATE <= TO_DATE(:EDATE, 'YYYY-MM-DD')                                                                                   ");
            mParameter.AppendSql(")                                                                                                                                       ");

            mParameter.AppendSql("  SELECT  PROGRESS                                                                                                                      ");
            mParameter.AppendSql("      ,	REQDATE                                                                                                                       ");
            mParameter.AppendSql("      ,	REQNAME	                                                                                                                      ");
            mParameter.AppendSql("      ,	WARDCODE                                                                                                                      ");
            mParameter.AppendSql("      ,	ROOMCODE                                                                                                                      ");
            mParameter.AppendSql("      ,	PANO	                                                                                                                      ");
            mParameter.AppendSql("      ,	SNAME                                                                                                                         ");
            mParameter.AppendSql("      ,	AGE	                                                                                                                          ");
            mParameter.AppendSql("      ,	SEX                                                                                                                           ");
            mParameter.AppendSql("      ,	IPDNO                                                                                                                           ");
            mParameter.AppendSql("      ,	SEQNO	                                                                                                                  ");
            mParameter.AppendSql("      ,	DEPTCODE	                                                                                                                  ");
            mParameter.AppendSql("      ,	REQNAME AS DRNAME 	                                                                                                          ");
            mParameter.AppendSql("      ,	A.DRUG_NAME                                                                                                                   ");
            mParameter.AppendSql("      ,	RTNNAME                                                                                                                       ");
            mParameter.AppendSql("      ,	RTNDATE                                                                                                                       ");
            mParameter.AppendSql("     FROM TDM_DATA A                                                                                                                    ");
            mParameter.AppendSql("    WHERE CASE WHEN :GBN = '전체' THEN 1                                                                                                 ");
            mParameter.AppendSql("               WHEN :GBN = '진행중' AND A.PROGRESS = '부분완료'  THEN 1                                                                    ");
            mParameter.AppendSql("               WHEN :GBN = A.PROGRESS THEN 1                                                                                            ");
            mParameter.AppendSql("           END = 1                                                                                                                      ");

            mParameter.AppendSql("      AND CASE WHEN :PTNO IS NULL OR A.PANO = :PTNO THEN 1                                                                               ");
            mParameter.AppendSql("           END = 1                                                                                                                      ");

            mParameter.AppendSql("      AND CASE WHEN :WARD IS NULL OR :WARD = '전체' OR A.WARDCODE = :WARD THEN 1                                                         ");
            mParameter.AppendSql("           END = 1                                                                                                                      ");

            mParameter.AppendSql("      AND CASE WHEN :DRUG_NAME IS NULL OR A.DRUG_NAME = :DRUG_NAME THEN 1                                                               ");
            mParameter.AppendSql("           END = 1                                                                                                                      ");


            mParameter.Add("FDATE", dtpSDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("EDATE", dtpEDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("GBN", SearchDto.RADIO);
            mParameter.Add("PTNO", txtPANO.Text.Trim());
            mParameter.Add("WARD", cboWard.Text.Trim());
            mParameter.Add("DRUG_NAME", cboName.Text.Trim());

            DataList = clsDB.ExecuteReader<TDMReturnModel>(mParameter, clsDB.DbCon);

            ssView.DataSource = DataList;

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {

        }

        private void btnEMR_Click(object sender, EventArgs e)
        {
            TDMReturnModel item = ssView.GetCurrentRowData() as TDMReturnModel;
            clsVbEmr.EXECUTE_NewTextEmrView(item.PANO);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (DataList == null)
                return;

            if (DataList.Count == 0)
                return;

            using (frmSupDrstTDMRequest frm = new frmSupDrstTDMRequest(DataList[e.Row].SEQNO.ToString()))
            {
                frm.StartPosition = FormStartPosition.CenterParent;
                frm.ShowDialog(this);
            }

            //using (frmSupDrstTDMReturn frm = new frmSupDrstTDMReturn(DataList[e.Row].SEQNO.ToString()))
            //{
            //    frm.StartPosition = FormStartPosition.CenterParent;
            //    frm.ShowDialog(this);
            //}
        }

        private void ssView_CellClick(object sender, CellClickEventArgs e)
        {
            DataList2.Clear();

            MParameter mParameter = new MParameter();

            mParameter.AppendSql("WITH TDM_DATA AS                                                                                                                        ");
            mParameter.AppendSql("(                                                                                                                                       ");
            mParameter.AppendSql("      SELECT	COALESCE((SELECT (SELECT NAME FROM ADMIN.BAS_BCODE WHERE GUBUN = 'TDM진행상황' AND CODE = SUB.PROGRESS)               ");
            mParameter.AppendSql("      		   FROM ADMIN.OCS_TDM_RETURN SUB                                                                                     ");
            mParameter.AppendSql("      		  WHERE SUB.SEQNO = A.SEQNO 		                                                                                      ");
            mParameter.AppendSql("      		), '미확인') AS PROGRESS                                                                                                   ");
            mParameter.AppendSql("      	,	TO_CHAR(A.REQDATE, 'YYYY-MM-DD') REQDATE                                                                                  ");
            mParameter.AppendSql("      	,	(SELECT TRIM(DRNAME) FROM ADMIN.OCS_DOCTOR WHERE DRCODE = I.DRCODE) AS REQNAME	                                        ");
            mParameter.AppendSql("      	,	I.WARDCODE                                                                                                                ");
            mParameter.AppendSql("      	,	I.ROOMCODE                                                                                                                ");
            mParameter.AppendSql("      	,	I.PANO	                                                                                                                  ");
            mParameter.AppendSql("      	,	I.SNAME                                                                                                                   ");
            mParameter.AppendSql("      	,	I.AGE	                                                                                                                  ");
            mParameter.AppendSql("      	,	I.SEX                                                                                                                     ");
            mParameter.AppendSql("      	,	I.DEPTCODE	                                                                                                              ");
            mParameter.AppendSql("      	,	A.SEQNO	                                                                                                                  ");
            mParameter.AppendSql("      	,	(SELECT (SELECT EMP_NM FROM ADMIN.HR_EMP_BASIS WHERE EMP_ID = SUB.SABUN)                                             ");
            mParameter.AppendSql("      		   FROM ADMIN.OCS_TDM_RETURN SUB                                                                                     ");
            mParameter.AppendSql("      		  WHERE SUB.SEQNO = A.SEQNO 		                                                                                      ");
            mParameter.AppendSql("      		) AS RTNNAME                                                                                                              ");
            mParameter.AppendSql("      	,	(SELECT TO_CHAR(JEPDATE, 'YYYY-MM-DD')                                                                                    ");
            mParameter.AppendSql("      		   FROM ADMIN.OCS_TDM_RETURN SUB                                                                                     ");
            mParameter.AppendSql("      		  WHERE SUB.SEQNO = A.SEQNO 		                                                                                      ");
            mParameter.AppendSql("      		) AS RTNDATE                                                                                                              ");
            mParameter.AppendSql("      	,	(SELECT SUBVAL                                                                                                            ");
            mParameter.AppendSql("      		   FROM ADMIN.OCS_TDM_MASTERSUB SUB                                                                                  ");
            mParameter.AppendSql("      		  WHERE SUB.SEQNO = A.SEQNO 		                                                                                      ");
            mParameter.AppendSql("      		    AND SUB.SUBGB = '000' 		                                                                                          ");
            mParameter.AppendSql("      		) AS DRUG_NAME                                                                                                            ");
            mParameter.AppendSql("        FROM ADMIN.OCS_TDM_MASTER A                                                                                                ");
            mParameter.AppendSql("       INNER JOIN ADMIN.IPD_NEW_MASTER I                                                                                          ");
            mParameter.AppendSql("          ON A.IPDNO = I.IPDNO                                                                                                          ");
            mParameter.AppendSql("       WHERE REQDATE >= TO_DATE(:FDATE, 'YYYY-MM-DD')                                                                                   ");
            mParameter.AppendSql("         AND REQDATE <= TO_DATE(:EDATE, 'YYYY-MM-DD')                                                                                   ");
            mParameter.AppendSql(")                                                                                                                                       ");

            mParameter.AppendSql("  SELECT  PROGRESS                                                                                                                      ");
            mParameter.AppendSql("      ,	REQDATE                                                                                                                       ");
            mParameter.AppendSql("      ,	REQNAME	                                                                                                                      ");
            mParameter.AppendSql("      ,	WARDCODE                                                                                                                      ");
            mParameter.AppendSql("      ,	ROOMCODE                                                                                                                      ");
            mParameter.AppendSql("      ,	PANO	                                                                                                                      ");
            mParameter.AppendSql("      ,	SNAME                                                                                                                         ");
            mParameter.AppendSql("      ,	AGE	                                                                                                                          ");
            mParameter.AppendSql("      ,	SEX                                                                                                                           ");
            mParameter.AppendSql("      ,	DEPTCODE	                                                                                                                  ");
            mParameter.AppendSql("      ,	REQNAME AS DRNAME 	                                                                                                          ");
            mParameter.AppendSql("      ,	A.DRUG_NAME                                                                                                                   ");
            mParameter.AppendSql("      ,	RTNNAME                                                                                                                       ");
            mParameter.AppendSql("      ,	RTNDATE                                                                                                                       ");
            mParameter.AppendSql("     FROM TDM_DATA A                                                                                                                    ");
            mParameter.AppendSql("    WHERE  A.PANO = :PTNO                                                                                                               ");

            mParameter.Add("FDATE", dtpSDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("EDATE", dtpEDATE.Value.ToString("yyyy-MM-dd"));
            mParameter.Add("PTNO", txtPANO.Text.Trim());

            DataList2 = clsDB.ExecuteReader<TDMReturnModel>(mParameter, clsDB.DbCon);

            ssView2.DataSource = DataList2;

        }
    }
}

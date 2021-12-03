using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Spread;
using ComBase.Mvc.Utils;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.OSHA.Model;
using HC.OSHA.Repository.StatusReport;
using HC_Core;
using HC_Core.Model;
using HC_Core.Model.DataSync;
using HC_Core.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace HC_OSHA
{
    /// <summary>
    /// 
    /// 1. export dmp
    /// 2. 테이블 드랍
    /// 3. import dmp
    /// </summary>
    public partial class StartExport : CommonForm
    {
        DataSyncRepository dataSyncRepository;
        HcUsersRepository hcUsersRepository;
        List<DbLinkModel> tableList = null;
        HcOshaRelationRepository hcOshaRelationRepository;


        public StartExport()
        {
            InitializeComponent();
            hcUsersRepository = new HcUsersRepository();
            dataSyncRepository = new DataSyncRepository();
            //siteList = new List<HC_OSHA_SITE_MODEL>();
            hcOshaRelationRepository = new HcOshaRelationRepository();
        }
        private void StartExport_Load(object sender, EventArgs e)
        {
            try
            {
                //bool isConnect = true;
                //DataSyncService.Instance.ConnectNotebook();
                //if (clsDB.DbCon == null)
                //{
                //    isConnect = false;
                //}
                //DataSyncService.Instance.ConnectOra7();
                //if (clsDB.DbCon == null)
                //{
                //    isConnect = false;

                //}
                //if (isConnect == false)
                //{
                //    MessageUtil.Alert("DB연결이 되지 않아 노트북으로 DB 가져오기를 할 수 없습니다.");
                //    return;
                //    this.Close();
                //}

                SSRelationList.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 40 });
                SSRelationList.AddColumnText("코드", nameof(HC_OSHA_RELATION_MODEL.CHILD_ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                SSRelationList.AddColumnText("하청사업장명", nameof(HC_OSHA_RELATION_MODEL.CHILD_NAME), 200, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                SSRelationList.SetDataSource(new List<HC_OSHA_RELATION_MODEL>());

                //SSOSHA.Initialize(new SpreadOption() { IsRowSelectColor = true, ColumnHeaderHeight = 40 });
                //SSOSHA.AddColumnText("코드", nameof(HC_OSHA_SITE_MODEL.ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                //SSOSHA.AddColumnText("사업장명", nameof(HC_OSHA_SITE_MODEL.NAME), 200, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                //SSOSHA.SetDataSource(new List<HC_OSHA_SITE_MODEL>());
                SSSiteList.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 40 });
                SSSiteList.AddColumnText("코드", nameof(HC_OSHA_SITE_MODEL.ID), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                SSSiteList.AddColumnText("사업장명", nameof(HC_OSHA_SITE_MODEL.NAME), 200, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                SSSiteList.AddColumnButton("삭제", 60, new SpreadCellTypeOption { ButtonText = "삭제" }).ButtonClick += StartExport_ButtonClick1;
                SSSiteList.SetDataSource(new List<HC_OSHA_SITE_MODEL>());

                SSTableList.Initialize(new SpreadOption() { IsRowSelectColor = true, ColumnHeaderHeight = 40 });
                SSTableList.AddColumnCheckBox("", "", 30, new CheckBoxBooleanCellType());
                SSTableList.AddColumnText("테이블명", nameof(DbLinkModel.TableName), 300, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                SSTableList.AddColumnText("설명", nameof(DbLinkModel.Description), 300, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                //SSTableList.AddColumnText("스키마여부", nameof(DbLinkModel.IsSchemaOnly), 60, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Center });
                //   SSTableList.AddColumnText("완료여부", nameof(DbLinkModel.IsComplete), 100, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                //   SSTableList.AddColumnText("오류", nameof(DbLinkModel.ErrorMessage), 300, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                SSTableList.SetDataSource(new List<DbLinkModel>());
                SetTables();

                for (int i = 0; i < SSTableList.ActiveSheet.RowCount; i++)
                {
                    SSTableList.ActiveSheet.Cells[i, 0].Value = true;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                Log.Error(ex);
                MessageUtil.Alert(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void StartExport_ButtonClick1(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            SSSiteList.DeleteRow();
        }

        /// <summary>
        /// 테이블 설정
        /// </summary>
        private void SetTables()
        {
            tableList = new List<DbLinkModel>();

            //검진테이블
            tableList.Add(new DbLinkModel("HIC_LTD"));
            tableList.Add(new DbLinkModel("HIC_CODE"));
            tableList.Add(new DbLinkModel("HIC_EXCODE"));
            tableList.Add(new DbLinkModel("HIC_RESCODE"));
            tableList.Add(new DbLinkModel("HIC_SPC_SCODE"));
            tableList.Add(new DbLinkModel("HIC_MCODE"));
            tableList.Add(new DbLinkModel("HIC_GUNDATE"));
            tableList.Add(new DbLinkModel("HIC_LTDINWON2"));
            tableList.Add(new DbLinkModel("HIC_RES_BOHUM2"));
            tableList.Add(new DbLinkModel("HIC_SPC_SOGENEXAM"));
            tableList.Add(new DbLinkModel("HIC_DOCTOR"));
            tableList.Add(new DbLinkModel("HIC_DOJANG"));
            tableList.Add(new DbLinkModel("HIC_EXJONG"));
            tableList.Add(new DbLinkModel("HIC_JEPSU_WORK"));
            tableList.Add(new DbLinkModel("BAS_BCODE"));


            //보건관리저문
            tableList.Add(new DbLinkModel("HIC_MACROWORD"));
            tableList.Add(new DbLinkModel("HIC_CODES"));
            tableList.Add(new DbLinkModel("HIC_USERS"));
            tableList.Add(new DbLinkModel("HIC_USERSIGN"));
            tableList.Add(new DbLinkModel("HIC_OSHA_SITE"));
            tableList.Add(new DbLinkModel("HIC_OSHA_ESTIMATE"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CONTRACT"));
            tableList.Add(new DbLinkModel("HIC_MSDS"));
            tableList.Add(new DbLinkModel("HIC_SITE_PRODUCT"));
            tableList.Add(new DbLinkModel("HIC_SITE_PRODUCT_MSDS"));
            tableList.Add(new DbLinkModel("HIC_SITE_WORKER"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CONTRACT_MANAGER"));
            tableList.Add(new DbLinkModel("HIC_OSHA_SCHEDULE"));
            tableList.Add(new DbLinkModel("HIC_OSHA_VISIT"));
            tableList.Add(new DbLinkModel("HIC_OSHA_VISIT_EDU"));
            tableList.Add(new DbLinkModel("HIC_OSHA_VISIT_COMMITTEE"));
            tableList.Add(new DbLinkModel("HIC_OSHA_VISIT_INFORMATION"));
            tableList.Add(new DbLinkModel("HIC_OSHA_VISIT_RECEIPT"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD6"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD5"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD3"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD4_1"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD4_2"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD4_3"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD7"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD7_1"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD9_1"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD9_2"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD9_3"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD9_4"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD9_5"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD10"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD11_1"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD11_2"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD13"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD15"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD16"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD17"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD19"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD20"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD21"));
            tableList.Add(new DbLinkModel("HIC_OSHA_CARD22"));
          
            tableList.Add(new DbLinkModel("HIC_OSHA_REPORT_NURSE"));
            tableList.Add(new DbLinkModel("HIC_OSHA_REPORT_ENGINEER"));
            tableList.Add(new DbLinkModel("HIC_OSHA_REPORT_DOCTOR"));
            tableList.Add(new DbLinkModel("HIC_OSHA_PRICE"));
            tableList.Add(new DbLinkModel("HIC_OSHA_VISIT_PRICE"));
            tableList.Add(new DbLinkModel("HIC_OSHA_HEALTHCHECK"));
            tableList.Add(new DbLinkModel("HIC_OSHA_HEALTHCHECK_MACROWORD"));
            tableList.Add(new DbLinkModel("HIC_OSHA_EQUIPMENT"));
            tableList.Add(new DbLinkModel("HIC_OSHA_MEMO"));
            tableList.Add(new DbLinkModel("HIC_OSHA_PATIENT_MEMO"));

            tableList.Add(new DbLinkModel("HIC_OSHA_PANJEONG"));
            tableList.Add(new DbLinkModel("HIC_OSHA_DATASYNC"));
            tableList.Add(new DbLinkModel("HIC_OSHA_GENEAL_RESULT"));
            tableList.Add(new DbLinkModel("HIC_OSHA_SPECIAL_RESULT"));
            tableList.Add(new DbLinkModel("HIC_OSHA_RELATION"));

            //  길광호 추가
            tableList.Add(new DbLinkModel("HIC_OSHA_PATIENT_REMARK"));  //  환자 특이사항
            tableList.Add(new DbLinkModel("HIC_OSHA_MAIL_SEND"));       //  메일 발송일자
            tableList.Add(new DbLinkModel("HIC_OSHA_WORKER_END"));      //  근로자 퇴직일자


            List<DbLinkModel> comments = dataSyncRepository.GetTableComments();
            foreach (DbLinkModel model in tableList)
            {
                foreach (DbLinkModel comment in comments)
                {
                   
                    if(comment.TableName.ToUpper() == model.TableName.ToUpper())
                    {
                        model.Description = comment.Description;
                    }
                }
            }

            SSTableList.SetDataSource(tableList);
        }

        private void DropTables()
        {
            try
            {
                for (int i = 0; i < SSTableList.RowCount(); i++)
                {
                    if (Convert.ToBoolean(SSTableList.ActiveSheet.Cells[i, 0].Value) == true)
                    {
                        DbLinkModel model = SSTableList.GetRowData(i) as DbLinkModel;
                       
                        if (dataSyncRepository.HasTable(model.TableName))
                        {
                            dataSyncRepository.DropTable(model.TableName);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageUtil.Alert("테이블 삭제중 오류가 발생했습니다 \n" + ex.Message );
            }
        }

        /// <summary>
        /// 노트북으로 가져오기
        /// 1. 서버 싱크 정보 확인 완료되지 않은 정버가 있는지 확인
        /// 2. 노트북의 싱크 정보 삭제
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (SSSiteList.ActiveSheet.RowCount == 0)
            {
                MessageUtil.Alert("사업장을 선택하세요");
                return;
            }

            DataSyncService.Instance.ConnectOra7();

            bool isBreak = false;
            try
            {
                // 원내DB에 업로드 되지 않은 데이타 확인
                List<DataSyncDto> syncList = dataSyncRepository.GetNotSyncAll();
                foreach (DataSyncDto sync in syncList)
                {
                    if(sync.ISSYNC == "N")
                    {
                        MessageUtil.Alert("DataSyncDto ID:" + sync.TABLENAME  + "는 업로드 되지 않았습니다. 노트북 가져오기는 중단합니다.");
                        isBreak = true;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                MessageUtil.Alert(ex.Message);
                return;
            }
            
            if (isBreak)
            {
                Log.Debug("엑스포트 종료");
                return;
            }

            if (MessageUtil.Confirm("노트북으로 가져오기 계속하시겠습니까?") == DialogResult.No)
            {
                return;
            }

            //검진
            List< HC_OSHA_SITE_MODEL> siteList = SSSiteList.GetEditbleData<HC_OSHA_SITE_MODEL>();
            ExportHcResult(siteList);

            DataSyncService.Instance.ConnectOra7();

            try
            {
                Cursor.Current = Cursors.WaitCursor;
                List<string> exportTables = new List<string>();
                for (int i = 0; i < SSTableList.RowCount(); i++)
                {
                    if (Convert.ToBoolean(SSTableList.ActiveSheet.Cells[i, 0].Value) == true)
                    {
                        DbLinkModel model = SSTableList.GetRowData(i) as DbLinkModel;
                        exportTables.Add(model.TableName);
                    }
                }
              
                if (exportTables.Count == 0 )
                {
                    return;
                }
              
                string tables = string.Join(",", exportTables);

                //비번 동기화
                foreach (HC_USER user in hcUsersRepository.FindAll())
                {
                    dataSyncRepository.UpdatePassword(user.UserId);
                }
                //시퀀스 쿼리문 
                List<SequenceModel> seqList = dataSyncRepository.GetSequencesSql();

                if (exportTables.Count > 0)
                {
                    DataSyncService.Instance.Export(tables);
                }

                try
                {
                    DataSyncService.Instance.ConnectNotebook();
                }
                catch (Exception ex)
                {
                    MessageUtil.Alert(ex.Message);
                    return;
                }

                string servieName = clsDB.DbCon.Con.ServiceName;
                if (servieName == "psmh_dev" || servieName == "orcl" || servieName == "XE")
                {
                    DropTables();
                }
                DataSyncService.Instance.Import(tables, seqList);

                dataSyncRepository.CreateDataSyncSequence();

                dataSyncRepository.CreateSiteView();

                dataSyncRepository.DeleteSync();

                MessageUtil.Info("노트북으로 DB 가져오기 완료!");
            }
            catch (Exception ex)
            {
                Log.Debug("3");
                Log.Error(ex);
                Cursor.Current = Cursors.Default;
                MessageUtil.Alert("오류:"+ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void ExportHcResult(List<HC_OSHA_SITE_MODEL> siteList)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //if (MessageUtil.Confirm("검진결과 내보내기 계속하시겠습니까?") == DialogResult.No)
                //{
                //    return;
                //}
                //검진 테이블스키마만 export 후 DBLINK를 통해 데이타 추출(대용량테이블들)    

                List<DbLinkModel> tables = new List<DbLinkModel>();
                tables.Add(new DbLinkModel("HIC_JEPSU", true));
                tables.Add(new DbLinkModel("HIC_RES_BOHUM1", true));
                tables.Add(new DbLinkModel("HIC_RES_SPECIAL", true));
                tables.Add(new DbLinkModel("HIC_SPC_PANJENG", true));
                tables.Add(new DbLinkModel("HIC_RESULT", true));
                tables.Add(new DbLinkModel("HIC_RES_PFT", true));
                tables.Add(new DbLinkModel("HIC_SPC_SAHU", true));
                tables.Add(new DbLinkModel("HEA_RES_PFT", true));
                
                List<string> exportSchemaTables = new List<string>();
                for (int i = 0; i < tables.Count; i++)
                {
                    exportSchemaTables.Add(tables[i].TableName);
                }

                string schemaTables = string.Join(",", exportSchemaTables);
                Log.Debug("11");

                DataSyncService.Instance.ExportSchema(schemaTables);

                try
                {
                    DataSyncService.Instance.ConnectNotebook();
                }
                catch (Exception ex)
                {
                    MessageUtil.Alert(ex.Message);
                    return;
                }

                try
                {
                    for (int i = 0; i < tables.Count; i++)
                    {
                        if (dataSyncRepository.HasTable(tables[i].TableName))
                        {
                            dataSyncRepository.DropTable(tables[i].TableName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageUtil.Alert("스키마 테이블 삭제중 오류가 발생했습니다 \n" + ex.Message);
                }

                DataSyncService.Instance.ImportSchema(schemaTables);

                Log.Debug("스키마 임포트 완료");

                dataSyncRepository.Create_HIC_PATIENT();

                Log.Debug("HIC_PATIENT 생성 완료");

                //사업장 검진결과는 dblink로 처리
                foreach (HC_OSHA_SITE_MODEL model in siteList)
                {
                    Log.Debug("22");
                    dataSyncRepository.INSERT_HIC_JEPSU(model.ID);

                    List<HIC_JEPSU_MODEL> wrtNoList = dataSyncRepository.GetHicJepsu(model.ID);
                    foreach (DbLinkModel db in tables)
                    {
                        if (db.TableName != "HIC_JEPSU")
                        {
                            dataSyncRepository.INSERT_HIC_DBLINK(model.ID, db.TableName);
                        }
                    }

                    //HIC_PATIENT는 암호화로 인해 export 불가
                    dataSyncRepository.INSERT_HIC_PATIENT(model.ID);
                }

                dataSyncRepository.CreateWorkerView();

                DataSyncService.Instance.ConnectOra7();

            //    MessageUtil.Info("검진결과 DB 내보내기 완료!");
            }
            catch (Exception ex)
            {
                Log.Debug("33");
                Log.Error(ex);
                MessageUtil.Alert(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void oshaSiteLastTree1_NodeClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Checked==false)
            {
                e.Node.Checked = true;
            }
            else
            {
                e.Node.Checked = false;
            }

           // CheckedNode(e.Node.Checked);
        }

        private void BtnExportSite_Click(object sender, EventArgs e)
        {
            if (MessageUtil.Confirm("사업장 가져오기 계속하시겠습니까?") == DialogResult.No)
            {
                return;
            }
            DataSyncService.Instance.ConnectOra7();

            List<DbLinkModel> tables = new List<DbLinkModel>();
            tables.Add(new DbLinkModel("HIC_LTD", true));
            tables.Add(new DbLinkModel("HIC_OSHA_SITE", true));
            tables.Add(new DbLinkModel("HIC_OSHA_CONTRACT", true));
            tables.Add(new DbLinkModel("HIC_OSHA_SCHEDULE", true));
            tables.Add(new DbLinkModel("HIC_OSHA_SITE", true));
            tables.Add(new DbLinkModel("HIC_OSHA_RELATION", true));

            List<string> exportTables = new List<string>();
            for (int i = 0; i < tables.Count; i++)
            {
                exportTables.Add(tables[i].TableName);
            }

            string tableList = string.Join(",", exportTables);

            DataSyncService.Instance.Export(tableList);

            try
            {
                DataSyncService.Instance.ConnectNotebook();
            }
            catch (Exception ex)
            {
                MessageUtil.Alert(ex.Message);
                return;
            }

            string servieName = clsDB.DbCon.Con.ServiceName;
            if (servieName == "psmh_dev" || servieName == "orcl" || servieName == "XE")
            {
                try
                {
                    for (int i = 0; i < tables.Count; i++)
                    {
                        if (dataSyncRepository.HasTable(tables[i].TableName))
                        {
                            dataSyncRepository.DropTable(tables[i].TableName);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageUtil.Alert("테이블 삭제중 오류가 발생했습니다 \n" + ex.Message);
                }
            }

            DataSyncService.Instance.Import(tableList, null);
            MessageUtil.Info("사업장 DB 가져오기 완료!");
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            List<HC_OSHA_SITE_MODEL> siteList = new List<HC_OSHA_SITE_MODEL>();
      
            HC_OSHA_SITE_MODEL model = oshaSiteList1.GetSite;

            bool isExist = false;
            for (int i = 0; i < SSSiteList.ActiveSheet.RowCount; i++)
            {
                HC_OSHA_SITE_MODEL m = SSSiteList.GetRowData(i) as HC_OSHA_SITE_MODEL;
                siteList.Add(m);
                if (m.ID == model.ID)
                {
                    isExist = true;
                }
            }
            if (isExist == false)
            {
                model.RowStatus = ComBase.Mvc.RowStatus.Insert;
                siteList.Add(model);
            }

            for(int i=0; i< SSRelationList.ActiveSheet.RowCount; i++)
            {
                HC_OSHA_RELATION_MODEL child = SSRelationList.GetRowData(i) as HC_OSHA_RELATION_MODEL;
                isExist = false;
                foreach (HC_OSHA_SITE_MODEL m in siteList)
                {
                    if(m.ID == child.CHILD_ID)
                    {
                        isExist = true;
                    }
                }
                if (isExist == false)
                {
                    HC_OSHA_SITE_MODEL dto = new HC_OSHA_SITE_MODEL();
                    dto.ID = child.CHILD_ID;
                    dto.NAME = child.CHILD_NAME;
                    dto.RowStatus = ComBase.Mvc.RowStatus.Insert;
                    siteList.Add(dto);
                }
            }

            SSSiteList.SetDataSource(new List<HC_OSHA_SITE_MODEL>());
            SSSiteList.SetDataSource(siteList);
        }

        private void oshaSiteList1_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void oshaSiteList1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            HC_OSHA_SITE_MODEL model = oshaSiteList1.GetSite;
            List<HC_OSHA_RELATION_MODEL> list = hcOshaRelationRepository.FindAll(model.ID);
            SSRelationList.SetDataSource(list);
        }
    }
}

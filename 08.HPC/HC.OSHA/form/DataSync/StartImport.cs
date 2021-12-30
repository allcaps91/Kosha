using ComBase;
using ComBase.Controls;
using ComBase.Mvc.Enums;
using ComBase.Mvc.Utils;
using HC.Core.Dto;
using HC.Core.Repository;
using HC.OSHA.Dto;
using HC.OSHA.Dto.StatusReport;
using HC.OSHA.Repository;
using HC.OSHA.Repository.Schedule;
using HC.OSHA.Repository.StatusReport;
using HC_Core;
using HC_Core.Model;
using HC_Core.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using HC.Core.Repository;
using HC.Core.Dto;
using HC.Core.Model;

namespace HC_OSHA
{
    /// <summary>
    /// 
    /// 노트북 DB를 서버에 업로드한다.
    /// 로그인 한 유저의 변경 이력만 업로드 가능함.
    /// </summary>
    public partial class StartImport : CommonForm
    {
        DataSyncRepository dataSyncRepository;

        public StartImport()
        {
            InitializeComponent();
            dataSyncRepository = new DataSyncRepository();
        }

        private void StartImport_Load(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                bool isConnect = true;
                DataSyncService.Instance.ConnectNotebook();
                if (clsDB.DbCon == null)
                {
                    isConnect = false;
                }
                DataSyncService.Instance.ConnectOra7();
                if (clsDB.DbCon == null)
                {
                    isConnect = false;

                }
                if (isConnect == false)
                {
                    MessageUtil.Alert("원내서버로 DB 올리기를 할 수 없습니다.");
                    this.Close();
                    return;
                }

                DateTime date = codeService.CurrentDate;
                DtpStartDate.SetValue(date.AddDays(-1));
                DtpendDate.SetValue(date.AddDays(1));

                SSDataSyncList.Initialize(new SpreadOption() { IsRowSelectColor = false, ColumnHeaderHeight = 40 });
                SSDataSyncList.AddColumnText("ID", nameof(DataSyncModel.ID), 50, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Center });
                SSDataSyncList.AddColumnText("테이블", nameof(DataSyncModel.TABLENAME), 267, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = true, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                SSDataSyncList.AddColumnText("코멘트", nameof(DataSyncModel.COMMENTS), 190, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                SSDataSyncList.AddColumnText("테이블키", nameof(DataSyncModel.TABLEKEY), 77, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                SSDataSyncList.AddColumnText("DML", nameof(DataSyncModel.DMLTYPE), 54, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Center });
                SSDataSyncList.AddColumnText("완료여부", nameof(DataSyncModel.ISSYNC), 40, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Center });
                SSDataSyncList.AddColumnText("오류", nameof(DataSyncModel.MESSAGE), 190, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                SSDataSyncList.AddColumnText("생성일시", nameof(DataSyncModel.CREATED), 165, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Left });
                SSDataSyncList.AddColumnText("사용자", nameof(DataSyncModel.CREATEDUSERNAME), 80, IsReadOnly.Y, new SpreadCellTypeOption { IsSort = false, Aligen = FarPoint.Win.Spread.CellHorizontalAlignment.Center });
                SSDataSyncList.AddColumnButton("", 60, new SpreadCellTypeOption { ButtonText = "완료" }).ButtonClick += StartImport_ButtonClick;

                //SSDataSyncList.SetDataSource(new List<DataSyncDto>());
                RdoAll.Checked = true;
                Search();
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                Cursor.Current = Cursors.Default;
                MessageUtil.Alert(ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void StartImport_ButtonClick(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            DataSyncModel model = SSDataSyncList.GetRowData(e.Row) as DataSyncModel;

            if (MessageUtil.Confirm(model.TABLENAME +" (" + model.TABLEKEY + ")완료 처리 하시겠습니까") == DialogResult.Yes)
            {
             
                dataSyncRepository.CompleteSync(model.ID);

                Search();
            }
         
        }

        private void Search()
        {
           
            string isSyncSearchCondition = string.Empty;
            if (RdoAll.Checked)
            {
                isSyncSearchCondition = string.Empty;
            }
            else if (RdoIsSyncY.Checked)
            {
                isSyncSearchCondition = "Y";
            }
            else
            {
                isSyncSearchCondition = "N";
            }

            List<DataSyncModel> list = dataSyncRepository.FinAll(DtpStartDate.GetValue(), DtpendDate.GetValue(), isSyncSearchCondition);

            SSDataSyncList.SetDataSource(list);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Search();
        }

        /// <summary>
        /// 노트북의 싱크이력을 가져온다.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnImport_Click(object sender, EventArgs e)
        {

        }

        private void RdoIsSyncN_CheckedChanged(object sender, EventArgs e)
        {
            Search();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnGetSync_Click(object sender, EventArgs e)
        {
             try
            {
                if(MessageUtil.Confirm("노트북 DB를 원내서버로 업로드 하시겠습니까?") == DialogResult.No)
                {
                    return;
                }
                Cursor.Current = Cursors.WaitCursor;

                //DataSyncService.Instance.ConnectNotebook();
                //if (clsDB.DbCon == null)
                //{
                //    return;
                //}
                //싱크정보 가져오기
                bool result = DataSyncService.Instance.CopyFromNotebook();
                if (!result)
                { 

                    MessageUtil.Alert("변경정보를 가져오는데 실패하였습니다");
                    return;
                }

                DataSyncService.Instance.ConnectOra7();
                
                List<DataSyncDto> list = dataSyncRepository.GetNotSyncAll();

                if(list.Count == 0)
                {
                    MessageUtil.Alert("가져올 데이타가 없습니다. 올리기를 중단합니다");
                    return;
                }
                else
                {
                    //테이블 유저 정보가 변경되면 안되는것들은 변경정보의 유저아이디를 반영해야함 예) 상용구.

                    foreach (DataSyncDto dto in list)
                    {
                        try
                        {
                            HIC_MACROWORD(dto);
                            HIC_OSHA_HEALTHCHECK_MACROWORD(dto);

                            HIC_SITE_PRODUCT(dto);
                            HIC_SITE_PRODUCT_MSDS(dto);
                            HIC_OSHA_CONTRACT(dto);
                            HIC_OSHA_CONTRACT_MANAGER(dto);
                            HIC_SITE_WORKER(dto);
                            HIC_PATIENT(dto);

                            HIC_OSHA_SCHEDULE(dto);
                            HIC_OSHA_VISIT(dto);
                            HIC_OSHA_VISIT_EDU(dto);
                            HIC_OSHA_VISIT_COMMITTEE(dto);
                            HIC_OSHA_VISIT_INFORMATION(dto);
                            HIC_OSHA_VISIT_RECEIPT(dto);
                            HIC_OSHA_CARD6(dto);
                            HIC_OSHA_CARD5(dto);
                            HIC_OSHA_CARD3(dto);
                            HIC_OSHA_CARD4_1(dto);
                            HIC_OSHA_CARD4_2(dto);
                            HIC_OSHA_CARD4_3(dto);
                            HIC_OSHA_CARD7(dto);
                            HIC_OSHA_CARD7_1(dto);
                            HIC_OSHA_CARD9_1(dto);
                            HIC_OSHA_CARD9_2(dto);
                            HIC_OSHA_CARD9_3(dto);
                            HIC_OSHA_CARD9_4(dto);
                            HIC_OSHA_CARD9_5(dto);
                            HIC_OSHA_CARD10(dto);
                            HIC_OSHA_CARD11_1(dto);
                            HIC_OSHA_CARD11_2(dto);
                            HIC_OSHA_CARD13(dto);
                            HIC_OSHA_CARD15(dto);
                            HIC_OSHA_CARD17(dto);
                            HIC_OSHA_CARD19(dto);
                            HIC_OSHA_CARD20(dto);
                            HIC_OSHA_CARD21(dto);
                            HIC_OSHA_CARD22(dto);
                   
                            HIC_OSHA_PRICE(dto);
                            HIC_OSHA_VISIT_PRICE(dto);
                            HIC_OSHA_EQUIPMENT(dto);
                            HIC_OSHA_RELATION(dto);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            dataSyncRepository.FaildSync(dto.ID, ex.Message);
                        }
                    } //for      


                    foreach (DataSyncDto dto in list)
                    {
                        try
                        {
                            HIC_OSHA_REPORT_NURSE(dto);
                            HIC_OSHA_REPORT_ENGINEER(dto);
                            HIC_OSHA_REPORT_DOCTOR(dto);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            dataSyncRepository.FaildSync(dto.ID, ex.Message);
                        }
                    } //for  

                    foreach (DataSyncDto dto in list)
                    {
                        try
                        {

                            HIC_OSHA_HEALTHCHECK(dto);
                            HIC_OSHA_MEMO(dto);
                            HIC_OSHA_PATIENT_MEMO(dto);

                            HIC_OSHA_PRICE(dto);
                            HIC_OSHA_VISIT_PRICE(dto);
                            HIC_OSHA_EQUIPMENT(dto);

                            //  길광호 추가
                            HIC_OSHA_PATIENT_REMARK(dto);
                            HIC_OSHA_MAIL_SEND(dto);
                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                            dataSyncRepository.FaildSync(dto.ID, ex.Message);
                        }
                    } //for  
                }
              
               // List<DataSyncDto> syncList = dataSyncRepository.FindAll();

                //노트북에 업로드 정보 남기기
                DataSyncService.Instance.ConnectNotebook();
                if (clsDB.DbCon == null)
                {
                    return;
                }
                dataSyncRepository.UpdateNotebookSync();
                Log.Debug("UPLODA END");
                MessageUtil.Info("원내서버로 데이타 올리기 완료");
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                Log.Error(ex);
                MessageUtil.Alert("데이타 가져오는중 오류가 발생했습니다+ \n" + ex.Message);
            }
            finally
            {
                DataSyncService.Instance.ConnectOra7();
                Search();
                DataSyncService.Instance.ConnectNotebook();
                Cursor.Current = Cursors.Default;


            }
        }

        private void HIC_MACROWORD(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_MACROWORD")
            {
                DataSyncService.Instance.ConnectNotebook();

                MacrowordRepository repo = new MacrowordRepository();
                MacrowordDto saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        MacrowordDto newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                    
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        MacrowordDto ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                    
                }

                dataSyncRepository.CompleteSync(dto.ID);

            }
        }


        private void HIC_OSHA_HEALTHCHECK_MACROWORD(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_HEALTHCHECK_MACROWORD")
            {
                DataSyncService.Instance.ConnectNotebook();

                HealthChecMacroRepository repo = new HealthChecMacroRepository();
                WorkerHealthCheckMacrowordDto saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        WorkerHealthCheckMacrowordDto newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                  
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        WorkerHealthCheckMacrowordDto ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
            
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }
        private void HIC_OSHA_REPORT_DOCTOR_SANGDAMSIGN(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_REPORT_DOCTOR_SANGDAMSIGN")
            {
                DataSyncService.Instance.ConnectNotebook();

                StatusReportDoctorRepository repo = new StatusReportDoctorRepository();
                StatusReportDoctorDto saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                if (saved != null)
                {
                    DataSyncService.Instance.ConnectOra7();
                    if (dto.DMLTYPE == "U")
                    {
                        repo.UpdateSangdamSign(saved.ID, saved.SANGDAMSIGN);
                    }

                    dataSyncRepository.CompleteSync(dto.ID);
                }

            }
        }
        private void HIC_OSHA_REPORT_NURSE_SANGDAMSIGN(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_REPORT_NURSE_SANGDAMSIGN")
            {
                DataSyncService.Instance.ConnectNotebook();

                StatusReportNurseRepository repo = new StatusReportNurseRepository();
                StatusReportNurseDto saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                if (saved != null)
                {
                    DataSyncService.Instance.ConnectOra7();
                    if (dto.DMLTYPE == "U")
                    {
                        repo.UpdateSangdamSign(saved.ID, saved.SANGDAMSIGN);
                    }

                    dataSyncRepository.CompleteSync(dto.ID);
                }
             
            }
        }
        private void HIC_OSHA_HEALTHCHECK(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_HEALTHCHECK")
            {
                DataSyncService.Instance.ConnectNotebook();

                HealthCheckRepository repo = new HealthCheckRepository();
                HealthCheckDto saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();
                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                       
                        HealthCheckDto newDto =  repo.Insert(saved);

                        UpdateNewKey(dto.TABLEKEY, newDto.id.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                        //신규 근로자 등록 키를 상담키의 근로자를 수정한다.
                        dataSyncRepository.UpdateHealthcheckWorker(saved.worker_id, dto.CREATEDUSER, newDto.id);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                      
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.id);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    HealthCheckDto ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                    if (ora7Saved != null)
                    {
                        if (dto.DMLTYPE == "D")
                        {
                            repo.Delete(ora7Saved.id);
                        }

                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        private void HIC_OSHA_MEMO(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_MEMO")
            {
                DataSyncService.Instance.ConnectNotebook();

                StatusReportMemoRepository repo = new StatusReportMemoRepository();
                HIC_OSHA_MEMO saved = repo.FindOne(long.Parse(dto.TABLEKEY));

                DataSyncService.Instance.ConnectOra7();
                if (saved != null)
                {
                    if (!saved.MEMO.IsNullOrEmpty())
                    {
                        if (dto.DMLTYPE == "I")
                        {
                            HIC_OSHA_MEMO tmp = repo.FindOne(saved.SITEID);
                            if (tmp == null)
                            {
                                repo.Insert(saved);
                            }
                            else
                            {
                                repo.Update(saved);
                            }

                        }
                        else if (dto.DMLTYPE == "U")
                        {
                            repo.Update(saved);
                        }

                    }
                    
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }

        private void HIC_OSHA_PATIENT_MEMO(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_PATIENT_MEMO")
            {
                DataSyncService.Instance.ConnectNotebook();

                HealthCheckMemoRepository repo = new HealthCheckMemoRepository();
                HIC_OSHA_PATIENT_MEMO saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();
                if (saved != null)
                {
                    
                    if (dto.DMLTYPE == "I")
                    {
                        HIC_OSHA_PATIENT_MEMO newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                        dataSyncRepository.UpdateMemoWorker(saved.WORKER_ID, dto.CREATEDUSER);
                        

                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HIC_OSHA_PATIENT_MEMO ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }

        private void HIC_OSHA_PATIENT_REMARK(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_PATIENT_REMARK")
            {
                DataSyncService.Instance.ConnectNotebook();

                HealthCheckMemoRepository repo = new HealthCheckMemoRepository();
                HIC_OSHA_PATIENT_REMARK saved = repo.FindOneRemark(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();
                if (saved != null)
                {

                    if (dto.DMLTYPE == "I")
                    {
                        HIC_OSHA_PATIENT_REMARK newDto = repo.InsertRemark(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                        dataSyncRepository.UpdateMemoWorker(saved.WORKER_ID, dto.CREATEDUSER);


                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                            repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.DeleteRemark(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HIC_OSHA_PATIENT_REMARK ora7Saved = repo.FindOneRemark(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.DeleteRemark(ora7Saved.ID);
                            }
                        }
                    }

                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }

        private void HIC_OSHA_MAIL_SEND(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_MAIL_SEND")
            {
                DataSyncService.Instance.ConnectNotebook();

                HicMailRepository repo = new HicMailRepository();
                HIC_OSHA_MAIL_SEND saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();
                if (saved != null)
                {

                    if (dto.DMLTYPE == "I")
                    {
                        HIC_OSHA_MAIL_SEND newDto = repo.InsertAndSelect(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                        dataSyncRepository.UpdateMemoWorker(saved.SEND_USER, dto.CREATEDUSER);


                    }
                    //else if (dto.DMLTYPE == "D")
                    //{
                    //    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                    //    if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                    //    {
                    //        repo.Delete(saved.ID);
                    //    }
                    //    else
                    //    {
                    //        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    //    }
                    //}
                }
                //else if (saved == null && dto.DMLTYPE == "D")
                //{
                //    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                //    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                //    {
                //        repo.DeleteRemark(tmp.NEWTABLEKEY.To<long>(0));
                //    }
                //    else
                //    {
                //        HIC_OSHA_PATIENT_REMARK ora7Saved = repo.FindOneRemark(tmp.TABLEKEY.To<long>(0));
                //        if (ora7Saved != null)
                //        {
                //            if (dto.DMLTYPE == "D")
                //            {
                //                repo.DeleteRemark(ora7Saved.ID);
                //            }
                //        }
                //    }
                //}
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }


        private void HIC_SITE_PRODUCT(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_SITE_PRODUCT")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcSiteProductRepository repo = new HcSiteProductRepository();
                HC_SITE_PRODUCT saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {

                        HC_SITE_PRODUCT newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }

                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_SITE_PRODUCT ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }
        private void HIC_SITE_PRODUCT_MSDS(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_SITE_PRODUCT_MSDS")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcSiteProductMsdsRepository repo = new HcSiteProductMsdsRepository();
                HC_SITE_PRODUCT_MSDS saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {

                        HC_SITE_PRODUCT_MSDS newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }

                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_SITE_PRODUCT_MSDS ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        private void HIC_OSHA_CONTRACT(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CONTRACT")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaContractRepository repo = new HcOshaContractRepository();
                HC_OSHA_CONTRACT saved = repo.FindByEstimateId(dto.TABLEKEY.To<long>(0));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {

                        HC_OSHA_CONTRACT newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ESTIMATE_ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ESTIMATE_ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CONTRACT ora7Saved = repo.FindByEstimateId(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ESTIMATE_ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }

        private void HIC_OSHA_CONTRACT_MANAGER(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CONTRACT_MANAGER")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaContractManagerRepository repo = new HcOshaContractManagerRepository();
                HC_OSHA_CONTRACT_MANAGER saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {

                        HC_OSHA_CONTRACT_MANAGER newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                   
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CONTRACT_MANAGER ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ESTIMATE_ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }

        /// <summary>
        /// 서버 및 로컬 DB의 변경된 키정보를 수정한다
        /// </summary>
        /// <param name="tableKey"></param>
        /// <param name="tableId"></param>
        /// <param name="tableName"></param>
        /// <param name="userId"></param>
        private void UpdateNewKey(string oldTableKey, string newTableId, string tableName, string userId)
        {
            DataSyncService.Instance.ConnectNotebook();
            dataSyncRepository.UpdateNewKey(oldTableKey, newTableId.ToString(), tableName, userId);
            DataSyncService.Instance.ConnectOra7();
            dataSyncRepository.UpdateNewKey(oldTableKey, newTableId.ToString(), tableName, userId);
        }
        private void HIC_SITE_WORKER(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_SITE_WORKER")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcSiteWorkerRepository repo = new HcSiteWorkerRepository();
                HC_SITE_WORKER saved = repo.FindOne(dto.TABLEKEY);
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
               
                    if (dto.DMLTYPE == "I")
                    {
                        HC_SITE_WORKER newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID, dto.TABLENAME, dto.CREATEDUSER);
                        
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                            repo.Delete(tmp.NEWTABLEKEY);
                        }
                    }

                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY);
                    }
                    else
                    {
                        HC_SITE_WORKER ora7Saved = repo.FindOne(tmp.TABLEKEY);
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        private void HIC_PATIENT(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_PATIENT")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcSiteWorkerRepository repo = new HcSiteWorkerRepository();
                HC_SITE_WORKER saved = repo.FindOne(dto.TABLEKEY);
                if (saved != null)
                {
                    DataSyncService.Instance.ConnectOra7();
                   
                    if (dto.DMLTYPE == "U")
                    {
                        repo.UpdatePatientWorkerRole(saved);
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        
        private void HIC_OSHA_SCHEDULE(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_SCHEDULE")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaScheduleRepository repo = new HcOshaScheduleRepository();
                HC_OSHA_SCHEDULE saved =  repo.FindById(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {

                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_SCHEDULE newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                            repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }

                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_SCHEDULE ora7Saved = repo.FindById(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
            //if (dto.TABLENAME == "HIC_OSHA_SCHEDULE")
            //{
            //    DataSyncService.Instance.ConnectNotebook();

            //    HcOshaScheduleRepository repo = new HcOshaScheduleRepository();
            //    HC_OSHA_SCHEDULE saved = repo.FindById(long.Parse(dto.TABLEKEY));
            //    DataSyncService.Instance.ConnectOra7();

            //    if (saved != null)
            //    {
            //        if (dto.DMLTYPE == "I")
            //        {
            //            repo.Insert(saved);
            //        }
            //        else if (dto.DMLTYPE == "U")
            //        {
            //            repo.Update(saved);
            //        }
            //        else if (dto.DMLTYPE == "D")
            //        {
            //            repo.Delete(saved.ID);
            //        }
            //    }

            //}
        }
        private void HIC_OSHA_VISIT(DataSyncDto dto)
        {
            //if (dto.TABLENAME == "HIC_OSHA_VISIT")
            //{
            //    DataSyncService.Instance.ConnectNotebook();

            //    HcOshaVisitRepository repo = new HcOshaVisitRepository();
            //    HC_OSHA_VISIT saved = repo.FindById(long.Parse(dto.TABLEKEY));
            //    DataSyncService.Instance.ConnectOra7();

            //    if (saved != null)
            //    {
            //        if (dto.DMLTYPE == "I")
            //        {
            //            repo.Insert(saved);
            //        }
            //        else if (dto.DMLTYPE == "U")
            //        {
            //            repo.Update(saved);
            //        }
            //        else if (dto.DMLTYPE == "D")
            //        {
            //            repo.Delete(saved);
            //        }
                    
            //    }
              
            //}
        }
        private void HIC_OSHA_VISIT_EDU(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_VISIT_EDU")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaVisitEduRepository repo = new HcOshaVisitEduRepository();
                HC_OSHA_VISIT_EDU saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_VISIT_EDU newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); 
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }

                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_VISIT_EDU ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }
        private void HIC_OSHA_VISIT_COMMITTEE(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_VISIT_COMMITTEE")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaVisitCommitteeRepository repo = new HcOshaVisitCommitteeRepository();
                HC_OSHA_VISIT_COMMITTEE saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_VISIT_COMMITTEE newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }

                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_VISIT_COMMITTEE ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }
        private void HIC_OSHA_VISIT_INFORMATION(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_VISIT_INFORMATION")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaVisitInformationRepository repo = new HcOshaVisitInformationRepository();
                HC_OSHA_VISIT_INFORMATION saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_VISIT_INFORMATION newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }

                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_VISIT_INFORMATION ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        private void HIC_OSHA_VISIT_RECEIPT(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_VISIT_INFORMATION")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaVisitReceiptRepository repo = new HcOshaVisitReceiptRepository();
                HC_OSHA_VISIT_RECEIPT saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_VISIT_RECEIPT newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }

        private void HIC_OSHA_CARD6(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD6")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard6Repository repo = new HcOshaCard6Repository();
                HC_OSHA_CARD6 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD6 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD6 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if(dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }
                            
                        }
                    }
                }
                
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        private void HIC_OSHA_CARD5(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD5")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard5Repository repo = new HcOshaCard5Repository();
                HC_OSHA_CARD5 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD5 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD5 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }

        }
        private void HIC_OSHA_CARD3(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD3")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard3Repository repo = new HcOshaCard3Repository();
                HC_OSHA_CARD3 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD3 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        repo.Update(saved);
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        repo.Delete(saved.ID);
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD3 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        private void HIC_OSHA_CARD4_1(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD4_1")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard4_1Repository repo = new HcOshaCard4_1Repository();
                HC_OSHA_CARD4_1 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD4_1 newDto = repo.Insert(saved);

                        DataSyncService.Instance.ConnectNotebook();
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                         DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }

                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD4_1 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }

                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        private void HIC_OSHA_CARD4_2(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD4_2")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard4_2Repository repo = new HcOshaCard4_2Repository();
                HC_OSHA_CARD4_2 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD4_2 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD4_2 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }

                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        private void HIC_OSHA_CARD4_3(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD4_3")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard4_3Repository repo = new HcOshaCard4_3Repository();
                HC_OSHA_CARD4_3 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD4_3 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD4_3 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }

                dataSyncRepository.CompleteSync(dto.ID);

            }
        }

        private void HIC_OSHA_CARD7(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD7")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard7Repository repo = new HcOshaCard7Repository();
                HC_OSHA_CARD7 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD7 newDto = repo.Insert(saved);
                        DataSyncService.Instance.ConnectNotebook();
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                   
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD7 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }

        private void HIC_OSHA_CARD7_1(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD7_1")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard7_1Repository repo = new HcOshaCard7_1Repository();
                HC_OSHA_CARD7_1 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD7_1 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                  
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD7_1 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }

        private void HIC_OSHA_CARD9_1(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD9_1")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard91Repository repo = new HcOshaCard91Repository();
                HC_OSHA_CARD9_1 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD9_1 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }

                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD9_1 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        private void HIC_OSHA_CARD9_2(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD9_2")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard92Repository repo = new HcOshaCard92Repository();
                HC_OSHA_CARD9_2 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD9_2 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);


                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                  
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD9_2 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }
        private void HIC_OSHA_CARD9_3(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD9_3")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard93Repository repo = new HcOshaCard93Repository();
                HC_OSHA_CARD9_3 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD9_3 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                   
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD9_3 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }
        private void HIC_OSHA_CARD9_4(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD9_4")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard94Repository repo = new HcOshaCard94Repository();
                HC_OSHA_CARD9_4 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {

                        HC_OSHA_CARD9_4 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);


                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                   
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD9_4 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);


            }
        }
        private void HIC_OSHA_CARD9_5(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD9_5")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard95Repository repo = new HcOshaCard95Repository();
                HC_OSHA_CARD9_5 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD9_5 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }

                    }
                  
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD9_5 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);


            }
        }
        private void HIC_OSHA_CARD10(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD10")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard10Repository repo = new HcOshaCard10Repository();
                HC_OSHA_CARD10 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD10 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                   
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD10 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }

        private void HIC_OSHA_CARD11_1(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD11_1")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard11_1Repository repo = new HcOshaCard11_1Repository();
                HC_OSHA_CARD11_1 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD11_1 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }

                    }
                   
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD11_1 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }
        private void HIC_OSHA_CARD11_2(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD11_2")
            {

                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard11_2Repository repo = new HcOshaCard11_2Repository();
                HC_OSHA_CARD11_2 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD11_2 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }

                    }

                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD11_2 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        private void HIC_OSHA_CARD13(DataSyncDto dto)
        {

            if (dto.TABLENAME == "HIC_OSHA_CARD13")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard13Repository repo = new HcOshaCard13Repository();
                HC_OSHA_CARD13 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD13 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }

                    }
                   
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD13 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }
        private void HIC_OSHA_CARD15(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD15")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard15Repository repo = new HcOshaCard15Repository();
                HC_OSHA_CARD15 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD15 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }

                    }
                   
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD15 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }
        private void HIC_OSHA_CARD17(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD17")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard17Repository repo = new HcOshaCard17Repository();
                HC_OSHA_CARD17 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD17 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {

                            repo.Update(saved);

                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }

                    }
                  
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD17 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }
        private void HIC_OSHA_CARD19(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD19")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard19Repository repo = new HcOshaCard19Repository();
                HC_OSHA_CARD19 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                
                DataSyncService.Instance.ConnectOra7();
                Log.Debug("dto19===>dto.TABLEKE" + dto.TABLEKEY);
                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD19 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호     
                        if(tmp != null)
                        {
                            if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                            {
                                HC_OSHA_CARD19 ora7saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                                if (ora7saved != null) 
                                {
                                    repo.Update(saved);
                                }
                                else
                                {// 서버의 것이 사라졌다면
                                    HC_OSHA_CARD19 newDto = repo.Insert(saved);
                                    UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                                }
                            }
                        }                        
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        //DataSyncService.Instance.ConnectOra7();
                        //DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                     
                        //if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        //{
                        //    repo.Delete(saved.ID);
                        //}
                        //else
                        //{
                        //    repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        //}
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    //DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    //if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    //{
                    //    repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    //}
                    //else
                    //{
                    //    HC_OSHA_CARD19 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                    //    if (ora7Saved != null)
                    //    {
                    //        if (dto.DMLTYPE == "D")
                    //        {
                    //            repo.Delete(ora7Saved.ID);
                    //        }

                    //    }
                    //}
                }
                dataSyncRepository.CompleteSync(dto.ID);


            }
        }
        private void HIC_OSHA_CARD20(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD20")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard20Repository repo = new HcOshaCard20Repository();
                HC_OSHA_CARD20 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();
                Log.Debug("dto19===>dto.TABLEKE" + dto.TABLEKEY);
                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD20 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);


                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                        
                            repo.Delete(saved.ID);

                        }
                        else
                        {
                         
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }

                    }
                   
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD20 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);



            }
        }
        private void HIC_OSHA_CARD21(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD21")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard21Repository repo = new HcOshaCard21Repository();
                HC_OSHA_CARD21 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();
                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD21 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);


                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);

                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }

                    }
                  
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD21 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        private void HIC_OSHA_CARD22(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_CARD22")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaCard22Repository repo = new HcOshaCard22Repository();
                HC_OSHA_CARD22 saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_CARD22 newDto = repo.Insert(saved);
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                   
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_CARD22 ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);


            }
        }

        private void HIC_OSHA_REPORT_NURSE(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_REPORT_NURSE")
            {
                DataSyncService.Instance.ConnectNotebook();

                StatusReportNurseRepository repo = new StatusReportNurseRepository();
                StatusReportNurseDto saved = repo.FindOne(long.Parse(dto.TABLEKEY));

                DataSyncService.Instance.ConnectOra7();
                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        long id = saved.ID;
                        StatusReportNurseDto newDto = repo.Insert(saved);
                        repo.UpdateOptioin(newDto.ID, saved.OPINION);

                        //업로드시 새로운 키가 발생되므로 이를 외래키로 사용하는 테이블 모두 새로운 키로 업데이트
                     
                        DataSyncService.Instance.ConnectNotebook();
                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);
                        
                        DataSyncService.Instance.ConnectNotebook();
                        if (newDto.ID > 0)
                        {
                            dataSyncRepository.UpdateHealthChek(newDto.ID, id, newDto.SITE_ID, "N");
                        }
                        else{
                            Log.Debug("간호사상태보고서 새로운 키가 0");
                        }
                        
                        DataSyncService.Instance.ConnectOra7();
                       

                        //repo.UpdateSelf(saved.ID, newDto.ID);

                    }
                    else if (dto.DMLTYPE == "U")////새로운키가 발생됐다면 업데이트는 실행하지않는다
                    {
                     
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); 
                   
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            //하루 두번 업로드 할경우 newTablekey가 생성되었지만 두번째 업로드시에는 newTablekey가 없다 따라서 다시 매칭해야한다
                            DataSyncDto newTableKeyDto = dataSyncRepository.FindNewTabnleKey(tmp.TABLENAME, tmp.CREATEDUSER, tmp.TABLEKEY);
                            if (newTableKeyDto != null)
                            {
                                saved.ID = newTableKeyDto.NEWTABLEKEY.To<long>(0);
                            }
                            repo.Update(saved);
                            repo.UpdateOptioin(saved.ID, saved.OPINION);
                        }
                        
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                     
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                        
                    }
                  
                   
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        StatusReportNurseDto ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }

        /// <summary>
        /// I: 새로 생성된 데이타는  서버에서 키를 발급하고 키를 노트북의 본래키를 가지고 잇는 변경정보의 새로운 키를 모두 발급하며 update 문은 무시한다.
        /// </summary>
        /// <param name="dto"></param>
        private void HIC_OSHA_REPORT_ENGINEER(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_REPORT_ENGINEER")
            {
                DataSyncService.Instance.ConnectNotebook();
                StatusReportEngineerRepository repo = new StatusReportEngineerRepository();
                StatusReportEngineerDto saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();
                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        StatusReportEngineerDto newDto = repo.Insert(saved);
                        repo.UpdateOptioin(newDto.ID, saved.OPINION);

                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                     //272는 양포
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            //하루 두번 업로드 할경우 newTablekey가 생성되었지만 두번째 업로드시에는 newTablekey가 없다 따라서 다시 매칭해야한다
                            DataSyncDto newTableKeyDto = dataSyncRepository.FindNewTabnleKey(tmp.TABLENAME, tmp.CREATEDUSER, tmp.TABLEKEY);
                            if (newTableKeyDto != null)
                            {
                                saved.ID = newTableKeyDto.NEWTABLEKEY.To<long>(0);
                            }
                            repo.Update(saved); //노트북데이타 272  한동을 양포에다 엄데이트 함.
                            repo.UpdateOptioin(saved.ID, saved.OPINION);
                        }
                  
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                      
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                   
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        StatusReportEngineerDto ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);
            }
        }
        /// <summary>
        /// 상태보고서 의사
        /// </summary>
        /// <param name="dto"></param>
        private void HIC_OSHA_REPORT_DOCTOR(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_REPORT_DOCTOR")
            {
                DataSyncService.Instance.ConnectNotebook();

                StatusReportDoctorRepository repo = new StatusReportDoctorRepository();
                StatusReportDoctorDto saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        long id = saved.ID;
                        StatusReportDoctorDto newDto = repo.Insert(saved);
                        repo.UpdateOptioin(newDto.ID, saved.OPINION);

                        UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);

                        DataSyncService.Instance.ConnectNotebook();
                        if (newDto.ID > 0)
                        {
                            dataSyncRepository.UpdateHealthChek(newDto.ID, id, newDto.SITE_ID, "Y");
                        }
                        else
                        {
                            Log.Debug("의사상태보고서 새로운 키가 0");
                        }
                            
                        DataSyncService.Instance.ConnectOra7();
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호                     
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {    //하루 두번 업로드 할경우 newTablekey가 생성되었지만 두번째 업로드시에는 newTablekey가 없다 따라서 다시 매칭해야한다
                            DataSyncDto newTableKeyDto = dataSyncRepository.FindNewTabnleKey(tmp.TABLENAME, tmp.CREATEDUSER, tmp.TABLEKEY);
                            if (newTableKeyDto != null)
                            {
                                saved.ID = newTableKeyDto.NEWTABLEKEY.To<long>(0);
                            }
                            repo.Update(saved);
                            repo.UpdateOptioin(saved.ID, saved.OPINION);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호
                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                  
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        StatusReportDoctorDto ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }
        private void HIC_OSHA_PRICE(DataSyncDto dto)
        {
            //if (dto.TABLENAME == "HIC_OSHA_PRICE")
            //{
            //    DataSyncService.Instance.ConnectNotebook();

            //    OshaPriceRepository repo = new OshaPriceRepository();
            //    OSHA_PRICE saved = repo.FindOne(long.Parse(dto.TABLEKEY));
            //    DataSyncService.Instance.ConnectOra7();

            //    if (saved != null)
            //    {
            //        if (dto.DMLTYPE == "I")
            //        {
            //            repo.Insert(saved);
            //        }                
            //        else if (dto.DMLTYPE == "D")
            //        {
            //            repo.Delete(saved.ID);
            //        }
            //        dataSyncRepository.CompleteSync(dto.ID);
            //    }
            //    else if (saved == null && dto.DMLTYPE == "D")
            //    {
            //        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
            //        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
            //        {
            //            repo.Delete(saved.ID);
            //        }
            //        else
            //        {
            //              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
            //        }
            //    }
            //    dataSyncRepository.CompleteSync(dto.ID);

            //}
        }
        private void HIC_OSHA_VISIT_PRICE(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_VISIT_PRICE")
            {
                //DataSyncService.Instance.ConnectNotebook();

                //OshaVisitPriceRepository repo = new OshaVisitPriceRepository();
                //OSHA_VISIT_PRICE saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                //DataSyncService.Instance.ConnectOra7();

                //if (saved != null)
                //{
                //    if (dto.DMLTYPE == "I")
                //    {
                //        repo.Insert(saved);
                //    }                   
                //    else if (dto.DMLTYPE == "D")
                //    {
                //        repo.Delete(saved);
                //    }
                //    dataSyncRepository.CompleteSync(dto.ID);
                //}
                //else
                //{
                //    repo.Delete(long.Parse(dto.TABLEKEY));
                //    dataSyncRepository.CompleteSync(dto.ID);
                //}
            }

        }

        private void HIC_OSHA_EQUIPMENT(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_EQUIPMENT")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaEquipmentRepository repo = new HcOshaEquipmentRepository();
                HC_OSHA_EQUIPMENT saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_EQUIPMENT newDto = repo.Insert(saved);

                        DataSyncService.Instance.ConnectNotebook();
                        dataSyncRepository.UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER); //노트북 새로운 키 발급
                        DataSyncService.Instance.ConnectOra7();
                        dataSyncRepository.UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);//서버의 새로
                    }
                    else if (dto.DMLTYPE == "U")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Update(saved);
                        }
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                              repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_EQUIPMENT ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }


        private void HIC_OSHA_RELATION(DataSyncDto dto)
        {
            if (dto.TABLENAME == "HIC_OSHA_RELATION")
            {
                DataSyncService.Instance.ConnectNotebook();

                HcOshaRelationRepository repo = new HcOshaRelationRepository();
                HC_OSHA_RELATION saved = repo.FindOne(long.Parse(dto.TABLEKEY));
                DataSyncService.Instance.ConnectOra7();

                if (saved != null)
                {
                    if (dto.DMLTYPE == "I")
                    {
                        HC_OSHA_RELATION newDto = repo.Insert(saved);

                        DataSyncService.Instance.ConnectNotebook();
                        dataSyncRepository.UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER); //노트북 새로운 키 발급
                        DataSyncService.Instance.ConnectOra7();
                        dataSyncRepository.UpdateNewKey(dto.TABLEKEY, newDto.ID.ToString(), dto.TABLENAME, dto.CREATEDUSER);//서버의 새로
                    }
                    else if (dto.DMLTYPE == "D")
                    {
                        DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID); //dto id는 서버에서 발급된 번호

                        if (tmp.NEWTABLEKEY.IsNullOrEmpty())
                        {
                            repo.Delete(saved.ID);
                        }
                        else
                        {
                            repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                        }
                    }
                }
                else if (saved == null && dto.DMLTYPE == "D")
                {
                    DataSyncDto tmp = dataSyncRepository.FindOne(dto.ID);
                    if (!tmp.NEWTABLEKEY.IsNullOrEmpty())
                    {
                        repo.Delete(tmp.NEWTABLEKEY.To<long>(0));
                    }
                    else
                    {
                        HC_OSHA_RELATION ora7Saved = repo.FindOne(tmp.TABLEKEY.To<long>(0));
                        if (ora7Saved != null)
                        {
                            if (dto.DMLTYPE == "D")
                            {
                                repo.Delete(ora7Saved.ID);
                            }

                        }
                    }
                }
                dataSyncRepository.CompleteSync(dto.ID);

            }
        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }//class

}
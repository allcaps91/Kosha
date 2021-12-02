using ComBase;
using ComBase.Mvc.Utils;
using HC.Core.Dto;
using HC.Core.Model;
using HC.Core.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using static HC.Core.Service.CommonService;

namespace HC.Core.Service
{
    public class LogService
    {
        private readonly static object Lock_Object = new object();
        private static LogService instance;
        private TaskHistoryRepository taskHistoryRepository;
        public LogService()
        {
            taskHistoryRepository = new TaskHistoryRepository();
        }
        public static LogService Instance
        {
            get
            {
                lock (Lock_Object)
                {
                    if (instance == null)
                    {
                        instance = new LogService();
                    }
                    return instance;
                }

            }
        }
        public enum TaskName
        {
            [Description("견적관리")]
            ESTIMATE,
            [Description("계약관리")]
            CONTRACT,
            [Description("방문등록")]
            VISIT,
            [Description("MSDS 제품")]
            MSDSPRODUCT,

            [Description("사업장 근로자 관리")]
            SITEWORKER,

            [Description("사업장 건강관리")]
            HEALTHCHECK,
            
            [Description("관리책임자, 선임관리")]
            SITE_CARD_1,

        }

        /// <summary>
        /// 사용자별 작업 내용 가져오기
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<TaskHistoryModel> FindAllByUser(string userId)
        {
            return taskHistoryRepository.FindAllByUser(userId);
        }
        /// <summary>
        /// 업무별 로그
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="taskName"></param>
        public void Task(long siteId, TaskName taskName)
        {
            try
            {
                TaskHistory dto = new TaskHistory()
                {
                    SITE_ID = siteId,
                    TASKNAME = EnumUtil.GetDescription(taskName)
                };

                taskHistoryRepository.Insert(dto);
                taskHistoryRepository.Update(siteId);
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
            
        }

        /// <summary>
        /// 사업장 수정일 로그
        /// </summary>
        public void SiteLog(long siteId)
        {
            try
            {
       
                taskHistoryRepository.Update(siteId);
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
        }
    }
}

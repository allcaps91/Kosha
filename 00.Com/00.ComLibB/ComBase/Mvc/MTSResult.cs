using ComBase.Controls;
using ComBase.Mvc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComBase.Mvc
{
    public class MTSResult
    {
        public object Data;
        private string ErrorMessage = string.Empty;
        private string SuccessMessage = string.Empty;
        private string CancelMessage = string.Empty;
        public int SuccessCount { get; private set; }
        public bool IsTransactions { get; private set; }
        public ResultType Result { get; private set; }

        public string GetErrorMessage { get { return ErrorMessage; } }
        public string GetCancelMessage { get { return CancelMessage; } }
        public string GetSuccessMessage { get { return SuccessMessage; } }
        public MTSResult()
        {
            SuccessCount = 0;
        }

        public MTSResult(bool isTransactions)
        {
            SuccessCount = 0;
            if(isTransactions)
            {
                IsTransactions = isTransactions;
            }
            clsDB.setBeginTran(clsDB.DbCon);
        }

        public void SetSuccessMessage(string msg)
        {
            Result = ResultType.Success;
            SuccessMessage = msg;

            if(IsTransactions)
            {
                clsDB.setCommitTran(clsDB.DbCon);
                IsTransactions = false;
            }
        }

        public void SetErrMessage(string msg)
        {
            Result = ResultType.Error;
            ErrorMessage = msg;

            if (IsTransactions)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                IsTransactions = false;
            }
        }

        public void SetCancelMessage(string msg = "")
        {
            Result = ResultType.Cancel;
            CancelMessage = msg;
            if (IsTransactions)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                IsTransactions = false;
            }
        }

        public void ShowMessage()
        {
            string msg = string.Empty;
            if(Result == ResultType.Error)
            {
                if (ErrorMessage.NotEmpty())
                {
                    MessageUtil.Info(ErrorMessage);
                }
            }
            else if(Result == ResultType.Success)
            {
                if (SuccessMessage.NotEmpty())
                {
                    MessageUtil.Info(SuccessMessage);
                }
            }
            else if (Result == ResultType.Cancel)
            {
                if (CancelMessage.NotEmpty())
                {
                    MessageUtil.Alert(CancelMessage);
                }
            }
        }

        public void SetSuccessCountPlus(int count)
        {
            SuccessCount += count;
        }

        public void SetSuccessCount(int count)
        {
            SuccessCount = count;
        }

        public void SetSuccessCount()
        {
            SuccessCount++;
        }
    }

    public enum ResultType
    {
        Error,
        Success,
        Cancel
    }
}

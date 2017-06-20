namespace BudgetControl.Models.Base
{
    public class ValidationResult
    {
        public ValidationResult(bool isSuccess)
        {
            this.IsSuccess = isSuccess;
        }

        public ValidationResult(string errorMessage, string memberName)
        {
            this.IsSuccess = false;
            this.ErrorMessage = errorMessage;
            this.MemberName = memberName;
        }

        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string MemberName { get; set; }

    }
}
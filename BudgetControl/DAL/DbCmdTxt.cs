using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.DAL
{
    public static class DbCmdTxt
    {
        public static string cmd_upsert_budget_transaction
        {
            get
            {
                return @"
UPDATE BudgetTransaction SET 
	Amount = @Amount
	, ModifiedBy = @ModifiedBy
	, ModifiedAt = @ModifiedAt
WHERE BudgetID = @BudgetID 
	AND PaymentID = @PaymentID
	AND Status = 1

IF @@ROWCOUNT = 0
	BEGIN
		INSERT INTO BudgetTransaction
			( 
				BudgetTransactionID
				, BudgetID
				, PaymentID
				, Description
				, Amount
				, PreviousAmount
				, RemainAmount
				, Type
				, Status
				, CreatedBy
				, CreatedAt
				, ModifiedBy
				, ModifiedAt
			)
			VALUES
			(
				@Id
				, @BudgetId
				, @PaymentID
				, @Description
				, @Amount
				, @PreviousAmount
				, @RemainAmount
				, @Type
				, @Status
				, @CreatedBy
				, @CreatedAt
				, @ModifiedBy
				, @ModifiedAt
			)
	END
                ";
            }
        }
    }
}
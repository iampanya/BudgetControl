using BudgetControl.DAL;
using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BudgetControl.Manager
{
    public class PaymentManager
    {
        private BudgetContext _db;

        #region Constructor

        public PaymentManager()
        {
            _db = new BudgetContext();
        }

        public PaymentManager(BudgetContext context)
        {
            this._db = context;
        }

        #endregion

        #region Properties

        public Payment Payment { get; set; }

        #endregion

        #region Methods

        public void Add(Payment payment)
        {
            // Create transaction
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // 1. Inital Payment data
                    this.Payment = new Payment(payment);
                    this.Payment.PrepareToSave();

                    // 2. Add payment to context
                    var paymentRepo = new PaymentRepository(_db);
                    paymentRepo.Add(this.Payment);
                    paymentRepo.Save();

                    // 3. Add each budget transaction by BudgetTransactionManager
                    BudgetTransactionManager transactionManager = new BudgetTransactionManager(_db);
                    var budgetTrans = payment.BudgetTransactions;

                    if (budgetTrans != null)
                    {
                        foreach (var tran in budgetTrans)
                        {
                            tran.PaymentID = this.Payment.PaymentID;
                            tran.Amount = 0 - tran.Amount;
                            transactionManager.Add(tran);
                        }
                    }

                    // 4. Comiit save change
                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw ex;
                }
            }
        }

        public void Update(Payment payment)
        {
            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    //1. Get  payment
                    Payment paymentindb;

                    List<BudgetTransaction> newtransactions = new List<BudgetTransaction>();

                    this.Payment = new Payment(payment);

                    var paymentRepo = new PaymentRepository(_db);
                    paymentindb = paymentRepo.GetById(payment.PaymentID);
                    paymentRepo.Update(this.Payment);
                    paymentRepo.Save();



                    var manager = new BudgetTransactionManager(_db);
                    paymentindb.BudgetTransactions = manager.SumTransaction(paymentindb.BudgetTransactions.ToList());


                    foreach (var item in paymentindb.BudgetTransactions)
                    {
                        var tran = payment.BudgetTransactions.Where(t => t.BudgetID == item.BudgetID).FirstOrDefault();

                        if (tran == null)
                        {
                            // delete 
                            newtransactions.Add(new BudgetTransaction()
                            {
                                PaymentID = payment.PaymentID,
                                BudgetID = item.BudgetID,
                                Amount = item.Amount,
                                RefID = item.BudgetTransactionID
                                
                            });
                        }
                        else
                        {
                            if (tran.Amount == item.Amount)
                            {
                                payment.BudgetTransactions.Remove(tran);
                            }
                            else
                            {
                                // Add new transaction
                                newtransactions.Add(new BudgetTransaction()
                                {
                                    PaymentID = payment.PaymentID,
                                    BudgetID = item.BudgetID,
                                    Amount = item.Amount - tran.Amount,
                                    RefID = item.BudgetTransactionID

                                });
                            }
                        }
                    }

                    foreach (var item in payment.BudgetTransactions)
                    {
                        var tran = newtransactions.FirstOrDefault(t => t.BudgetID == item.BudgetID);
                        if(tran == null)
                        {
                            newtransactions.Add(new BudgetTransaction()
                            {
                                PaymentID = payment.PaymentID,
                                BudgetID = item.BudgetID,
                                Amount = 0 - item.Amount
                            });
                        }
                    }

                    foreach (var item in newtransactions)
                    {
                        manager.Add(item);
                    }

                    dbTransaction.Commit();


                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw;
                }
            }
        }

        public void Delete(Payment payment)
        {

        }


        #endregion

    }
}
using BudgetControl.DAL;
using BudgetControl.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public Payment Add(Payment payment)
        {
            var transactions = payment.BudgetTransactions;

            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {
                    // 1. Inital Payment data
                    payment = new Payment(payment);
                    payment.PrepareToSave();

                    // 2. Add payment to context
                    var paymentRepo = new PaymentRepository(_db);
                    payment.Sequence = paymentRepo.Get().Where(p => p.CostCenterID == payment.CostCenterID).ToList().Count + 1;

                    // Get payment counter info
                    PaymentCounter pcounter = _db.PaymentCounters.Where(c => c.CostCenterID == payment.CostCenterID && c.Year == payment.Year).FirstOrDefault();
                    if(pcounter == null)
                    {
                        //if not exist, then add new one.
                        var shortName = _db.CostCenters.Where(c => c.CostCenterID == payment.CostCenterID).Select(s => s.ShortName).FirstOrDefault();
                        var split = shortName.Split(new char[] { ' ' });
                        var shortCode = split[split.Length - 1].Trim(new char[] { ' ', '.' });

                        pcounter = new PaymentCounter()
                        {
                            CostCenterID = payment.CostCenterID,
                            Year = payment.Year,
                            ShortCode = shortCode,
                            Number = 1
                        };

                        _db.PaymentCounters.Add(pcounter);
                        _db.SaveChanges();
                    }
                    else
                    {
                        pcounter.Number += 1;
                        pcounter.NewModifyTimeStamp();
                        _db.Entry(pcounter).State = EntityState.Modified;
                        _db.SaveChanges();
                    }
                    payment.PaymentNo = pcounter.ShortCode + "-" + pcounter.Number.ToString().PadLeft(4, '0');
                    paymentRepo.Add(payment);
                    paymentRepo.Save();

                    // 3. Add each budget transaction by BudgetTransactionManager
                    BudgetTransactionManager transactionManager = new BudgetTransactionManager(_db);
                    if (transactions != null)
                    {
                        foreach (var item in transactions)
                        {
                            item.PaymentID = payment.PaymentID;
                            transactionManager.AddOrUpdate(item);
                        }
                    }

                    dbTransaction.Commit();
                    return payment;
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
            var transactions = payment.BudgetTransactions.ToList();

            using (var dbTransaction = _db.Database.BeginTransaction())
            {
                try
                {

                    payment = new Payment(payment);

                    // Add payment to context
                    var paymentRepo = new PaymentRepository(_db);
                    paymentRepo.Update(payment);
                    paymentRepo.Save();

                    // Update transaction
                    var transmanager = new BudgetTransactionManager(_db);
                    transmanager.UpdateByPayment(payment, transactions);

                    dbTransaction.Commit();
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw ex;
                }
            }

        }

        public Payment Add_old(Payment payment)
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
                    this.Payment.Sequence = paymentRepo.Get().Where(p => p.CostCenterID == this.Payment.CostCenterID).ToList().Count + 1;
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
                    return this.Payment;
                }
                catch (Exception ex)
                {
                    dbTransaction.Rollback();
                    throw ex;
                }
            }
        }

        public void Update_old(Payment payment)
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



                    var transmanager = new BudgetTransactionManager(_db);
                    paymentindb.BudgetTransactions = transmanager.SumTransaction(paymentindb.BudgetTransactions.ToList());

                    // for update transaction
                    foreach (var itemindb in paymentindb.BudgetTransactions)
                    {
                        var tran = payment.BudgetTransactions.Where(t => t.BudgetID == itemindb.BudgetID).FirstOrDefault();


                        if (tran == null)
                        {
                            // delete 
                            if (isLastTransaction(itemindb))
                            {
                                // Delete
                                transmanager.Delete(itemindb);
                            }
                            else
                            {
                                newtransactions.Add(new BudgetTransaction()
                                {
                                    PaymentID = payment.PaymentID,
                                    BudgetID = itemindb.BudgetID,
                                    Amount = itemindb.Amount,
                                    RefID = itemindb.BudgetTransactionID
                                });
                            }
                        }
                        else
                        {
                            if (tran.Amount == itemindb.Amount)
                            {
                                payment.BudgetTransactions.Remove(tran);
                            }
                            else
                            {
                                if (isLastTransaction(itemindb))
                                {
                                    BudgetTransaction updateitem;
                                    //Update
                                    using (var transRepo = new TransactionRepository())
                                    {
                                        updateitem = transRepo.GetById(itemindb.BudgetTransactionID);
                                    }
                                    updateitem.Amount = updateitem.Amount + (itemindb.Amount - tran.Amount);
                                    transmanager.Update(itemindb);
                                }
                                else
                                {
                                    // Add new transaction
                                    newtransactions.Add(new BudgetTransaction()
                                    {
                                        PaymentID = payment.PaymentID,
                                        BudgetID = itemindb.BudgetID,
                                        Amount = itemindb.Amount - tran.Amount,
                                        RefID = itemindb.BudgetTransactionID

                                    });
                                }
                            }
                        }
                    }

                    // for new transaction
                    foreach (var item in payment.BudgetTransactions)
                    {
                        var tran = newtransactions.FirstOrDefault(t => t.BudgetID == item.BudgetID);
                        if (tran == null)
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
                        transmanager.Add(item);
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

        private bool isLastTransaction(BudgetTransaction transaction)
        {
            // disable last transaction
            return false;
            //using (var transRepo = new TransactionRepository())
            //{
            //    return !(transRepo.Get().Any(t => t.BudgetID == transaction.BudgetID && t.CreatedAt > transaction.CreatedAt));
            //}
        }

        #endregion

    }
}
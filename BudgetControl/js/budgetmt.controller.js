;
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('RequestBudgetMTCtrl', RequestBudgetMTCtrl);

    RequestBudgetMTCtrl.$inject = ['$scope', '$state', '$filter', '$uibModal', 'apiService', 'apiIdmService', 'handleResponse', 'authInfo', 'msgService'];

    function RequestBudgetMTCtrl($scope, $state, $filter, $uibModal, apiService, apiIdmService, hr, authInfo, msgService) {
        // variable
        var vm = this; vm.formRequest = {};            // payment form data 

        vm.years = [];              // budget year list in dropdown
        vm.transactions = [];       // transaction inside payment
        vm.Meals = [];


        vm.year = '222';


        //function 
        vm.calculateTotalAmount = calculateTotalAmount;
        //vm.addNewTransaction = addNewTransaction;
        //vm.removeTransaction = removeTransaction;
        //vm.updateTotalAmount = updateTotalAmount;
        vm.submit = submit;
        //vm.getEmployeeProfile = getEmployeeProfile;
        //vm.onRequestByOtherBlur = onRequestByOtherBlur;
        //vm.onYearChange = onYearChange;

        //initial form data
        prepareData();

        function prepareData() {
            console.log('prepare data');

            populateYearList();
        }

        function populateYearList() {

            vm.years = [];
            var currentYear = new Date().getFullYear();
            for (var i = 2022; i <= currentYear; i++) {
                vm.years.push(i + 543 + '');
            }

            vm.formRequest.Year = currentYear + 543 + '';
        }

        function calculateTotalAmount() {
            let mealCount = (vm.formRequest.MealMorning ? 1 : 0) + (vm.formRequest.MealAfternoon ? 1 : 0);
            vm.formRequest.TotalAmount = parseInt(vm.formRequest.ParticipantCount) * (35 * mealCount) ;
        }

        function onYearChange() {
            clearAllTransaction();
        }

        function addNewTransaction() {

            vm.addNewTransactionError = "";

            //1. Looking for budget is already added?
            var index = vm.transactions.findIndex(function (obj) {
                return obj.BudgetID == vm.selectbudget;
            })

            //2. If exist, then set status to active
            if (index > -1) {
                vm.addNewTransactionError = "*บัญชีนี้มีอยู่แล้วในรายการ";
            }
            //3. If not exist, then add to list
            else {
                var budget = $filter('filter')(vm.budgets, function (d) {
                    return d.BudgetID === vm.selectbudget
                })[0]

                vm.transactions.push({
                    BudgetTransactionID: '',
                    BudgetID: budget.BudgetID,
                    PaymentID: '',
                    Description: '',
                    Amount: '',
                    PreviousAmount: 0.00,
                    RemainAmount: 0.00,
                    Budget: budget,
                    Status: '0',
                })
            }

            //4. Set selectbudget to empty
            vm.selectbudget = ''

            //5. Update total amount
            updateTotalAmount();
        }

        function removeTransaction(budgetid) {
            //1. Find index of budgetid in transactions
            var index = vm.transactions.findIndex(function (obj) {
                return obj.BudgetID == budgetid;
            });

            //2. Set status to "Remove"
            vm.transactions.splice(index, 1);
            vm.transactions[index].Status = "Remove";

            // 3. Update total amount on payment
            updateTotalAmount();
        }

        function clearAllTransaction() {
            vm.transactions = [];
        }

        function updateTotalAmount() {
            // Initial total amount value to zero
            vm.payment.TotalAmount = 0;

            //for loop on each transaction and add to total amount.
            for (var i = 0; i < vm.transactions.length; i++) {
                vm.payment.TotalAmount += vm.transactions[i].Amount;

                if (vm.transactions[i].Status === '0') {
                    vm.payment.TotalAmount += vm.transactions[i].Amount;
                }
            }
        }

        function submit() {
            console.log(vm.formRequest);
            //vm.addNewTransactionError = "";
            //if (vm.transactions.length > 0) {
            //    vm.isSubmitting = true;
            //    preparePaymentToSave();
            //    apiService.payment().save(vm.payment).$promise.then(callPaymentSuccess, callPaymentError);
            //}
            //else {
            //    vm.addNewTransactionError = "* กรุณากดเพิ่มรายการงบประมาณ";
            //}

            //function callPaymentSuccess(response) {
            //    var paymentid = hr.respondSuccess(response);
            //    //TODO modal result.
            //    if (response.isSuccess) {
            //        openModal('lg', null, paymentid);
            //    }
            //    vm.isSubmitting = false;
            //}

            //function callPaymentError(e) {
            //    hr.repondError(e);
            //    vm.isSubmitting = false;
            //}
        }

        function preparePaymentToSave() {
            vm.payment.BudgetTransactions = vm.transactions;
            if (vm.payment.Type == vm.paymentType.internal) {
                vm.payment.RequestBy = vm.requestbynormal.employeecode;
                vm.payment.Contractor = {};
            }
            else if (vm.payment.Type == vm.paymentType.pea) {
                vm.payment.RequestBy = vm.requestbypea.employeecode;
                vm.payment.Contractor = {};
            }
            else if (vm.payment.Type == vm.paymentType.contractor) {
                vm.payment.RequestBy = '';
                vm.payment.Contractor = {
                    ID: vm.requestbyother.id,
                    Name: vm.requestbyother.name
                };
            }
        }

        function onRequestByOtherBlur() {
            //TODO : get contractor id if exist
            var contractor = vm.contractors.find(function (contractor) {
                return contractor.Name.trim() === vm.requestbyother.name.trim();
            })
            if (contractor) {
                vm.requestbyother.id = contractor.ID;
            }
            else {
                vm.requestbyother.id = '';
            }
        }

        function getEmployeeProfile() {
            msgService.clearMsg();
            if (vm.requestbypea.employeecode) {
                apiIdmService.employee()
                    .get({ empno: vm.requestbypea.employeecode })
                    .$promise.then(getEmployeeProfileSuccess, callError);
            }
            else {
                vm.requestbypea = {};
            }

            function getEmployeeProfileSuccess(response) {
                var empProfile = hr.respondSuccess(response);
                vm.requestbypea.name = empProfile.Description;
                vm.requestbypea.position = empProfile.JobTitle;
                vm.requestbypea.costcenterid = empProfile.CostCenterID;
            }
        }



        function callError(e) {
            hr.respondError(e);
        }

        //Modal result 
        function openModal(size, parentSelector, data) {
            var parentElem = parentSelector ?
                angular.element($document[0].querySelector('.modal-demo ' + parentSelector)) : undefined;
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'payments/result',
                controller: 'ResultPaymentCtrl',
                controllerAs: 'vm',
                size: size,
                backdrop: 'static',
                keyboard: false,
                appendTo: parentElem,
                resolve: {
                    paymentid: function () {
                        return data;
                    }
                }
            });

            modalInstance.result.then(function (selectedItem) {
                console.log('Modal dismissed at: ' + new Date());
            }, function () {
                console.log('Modal dismissed at: ' + new Date());
            });
        };
    }
})();


;
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('TransactionBudgetMTCtrl', TransactionBudgetMTCtrl);

    TransactionBudgetMTCtrl.$inject = ['$scope', '$state', '$filter', '$uibModal', 'apiService', 'apiIdmService', 'handleResponse', 'authInfo', 'msgService'];

    function TransactionBudgetMTCtrl($scope, $state, $filter, $uibModal, apiService, apiIdmService, hr, authInfo, msgService) {
        var vm = this;
        console.log('transaction page');
        return vm;
    }

})();

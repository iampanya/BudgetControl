;
/****** Payment Controller *******/
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('PaymentCtrl', PaymentCtrl);

    PaymentCtrl.$inject = ['$state', '$filter', '$uibModal', 'apiService', 'msgService', 'handleResponse', 'authInfo'];

    function PaymentCtrl($state, $filter, $uibModal, apiService, msgService, hr, authInfo) {
        var vm = this;
        vm.payments = [];
        vm.costcenters = [];
        vm.costcenter = '';
        vm.years = [];
        vm.year = '';
        vm.filter = '';


        vm.deletePayment = deletePayment;
        vm.sortBy = sortBy;
        vm.getPaymentData = getPaymentData;

        // Default sort by PaymentNO Desc
        vm.reverse = true;
        vm.propertyName = 'PaymentNo';

        initialData();

        function initialData() {
            vm.costcenter = authInfo.getWorkingCostCenter().CostCenterID;

            populateYearList();

            populateCostCenterList();

            getPaymentData();
        }

        function getPaymentData() {
            apiService.payment().get({ year: vm.year, costcenterid: vm.costcenter }).$promise.then(callSuccess, callError);

            function callSuccess(response) {
                vm.payments = hr.respondSuccess(response);    
                console.log(vm.payments);
            }
        }

        function populateYearList() {
            // Populate year list from 2559 to current + 1 and set default to current year
            vm.years = [];
            var currentYear = new Date().getFullYear();
            for (var i = 2016; i <= currentYear + 1; i++) {
                vm.years.push(i + 543 + '');
            }

            vm.year = currentYear + 543 + '';
        }

        function populateCostCenterList() {
            // 1. Get Costcenter from server
            apiService.costcenter().get().$promise.then(callCostCenterSuccess, callError);

            // Call CostCenter success
            function callCostCenterSuccess(response) {
                vm.costcenters = hr.respondSuccess(response);
                if (vm.costcenters.length < 1) {
                    vm.costcenters.push({ CostCenterID: authInfo.getWorkingCostCenter().CostCenterID, CostCenter: authInfo.getWorkingCostCenter() });
                }
            }
        }

        function callError(e) {
            hr.respondError(e);
        }


        function deletePayment(payment) {
            openModal('', null, payment);
        }

        function sortBy(propertyName) {
            if (propertyName === '') {
                vm.reverse = false;
                vm.propertyName = propertyName;
                return false;
            }
            vm.reverse = (vm.propertyName === propertyName) ? !vm.reverse : false;
            vm.propertyName = propertyName;
        }

        function openModal(size, parentSelector, data) {
            var parentElem = parentSelector ?
                angular.element($document[0].querySelector('.modal-demo ' + parentSelector)) : undefined;
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'payments/ConfirmDelete',
                controller: 'ConfirmDeletePaymentCtrl',
                controllerAs: 'vm',
                size: size,
                appendTo: parentElem,
                resolve: {
                    payment: function () {
                        return data;
                    }
                }
            });

            modalInstance.result.then(function (payment) {
                if (payment) {
                    var index = vm.payments.findIndex(function (obj) {
                        return obj.PaymentID == payment.PaymentID;
                    });
                    
                    vm.payments.splice(index, 1);
                }
            });
        };

    
    }


})();

/***********  ConfirmDeletePaymentCtrl **************/
(function () {
    angular.module('budgetApp')
        .controller('ConfirmDeletePaymentCtrl', ConfirmDeletePaymentCtrl);

    ConfirmDeletePaymentCtrl.$inject = ['$state', '$uibModalInstance', 'apiService', 'handleResponse', 'msgService', 'payment'];

    function ConfirmDeletePaymentCtrl($state, $uibModalInstance, apiService, hr, msgService, payment) {
        var vm = this;
        vm.payment = payment;
        vm.closemodal = closemodal;
        vm.removebudget = removebudget;
        vm.errorMsg = '';

        function closemodal() {
            $uibModalInstance.close();
        }

        function removebudget() {
            vm.errorMsg = '';
            apiService.payment().remove({ id: payment.PaymentID }).$promise.then(deleteSuccess, deleteError);



            function deleteSuccess(response) {
                if (hr.respondSuccess(response)) {
                    msgService.setSuccessMsg('ลบรายการสำเร็จ');
                    $uibModalInstance.close(payment);
                }
                else {
                    deleteError();
                }
            }

            function deleteError(e) {
                vm.errorMsg = '*** ไม่สามารถทำรายการได้ กรุณาลองอีกครั้ง ***';
                //hr.respondError(e);
                //$uibModalInstance.close();
            }
        }
    }
})();

/****** Create Payment Controller *******/
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('CreatePaymentCtrl', CreatePaymentCtrl);

    CreatePaymentCtrl.$inject = ['$scope', '$state', '$filter', '$uibModal', 'apiService', 'apiIdmService', 'handleResponse', 'authInfo', 'msgService'];

    function CreatePaymentCtrl($scope, $state, $filter, $uibModal, apiService, apiIdmService, hr, authInfo, msgService) {
        // variable
        var vm = this;
        vm.budgets = [];            // budget list in dropdown
        vm.employees = [];          // all employee in costcenters
        vm.displayEmployees = [];   // employee list in dropdown
        vm.years = [];              // budget year list in dropdown
        vm.payment = {};            // payment form data 
        vm.transactions = [];       // transaction inside payment
        vm.contractors = [];        // Contractor List
        vm.isSubmitting = false;

        // Payment Type
        vm.paymentType = {
            internal: 1,
            pea: 2,
            contractor: 3
        }

        vm.requestbynormal = {
            employeecode: ''
        }

        vm.requestbypea = {
            employeecode: '',
            name: '',
            position: '',
            costcenterid: ''
        }

        vm.requestbyother = {
            id: '',
            name: ''
        }

        // function 
        vm.addNewTransaction = addNewTransaction;
        vm.removeTransaction = removeTransaction;
        vm.updateTotalAmount = updateTotalAmount;
        vm.submit = submit;
        vm.getEmployeeProfile = getEmployeeProfile;
        vm.onRequestByOtherBlur = onRequestByOtherBlur;
        vm.onYearChange = onYearChange;

        // initial form data
        prepareData();

        // Watching 
        ////// - Watch year change to clear budget 
        //$scope.$watch(watchingYear, onYearChange);


        //function watchingCostCenterID(scope) {
        //    return vm.payment.CostCenterID;
        //}
 
        //function watchingYear(scope) {
        //    return vm.payment.Year;
        //}

        function onYearChange() {
            clearAllTransaction();
        }

        function addNewTransaction() {
            //
            vm.addNewTransactionError = "";

            // 1. Looking for budget is already added?
            var index = vm.transactions.findIndex(function (obj) {
                return obj.BudgetID == vm.selectbudget;
            })
            
            // 2. If exist, then set status to active
            if (index > -1) {
                vm.addNewTransactionError = "*บัญชีนี้มีอยู่แล้วในรายการ";
            }
            // 3. If not exist, then add to list
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

            // 4. Set selectbudget to empty
            vm.selectbudget = ''

            // 5. Update total amount
            updateTotalAmount();
        }

        function removeTransaction(budgetid) {
            // 1. Find index of budgetid in transactions
            var index = vm.transactions.findIndex(function (obj) {
                return obj.BudgetID == budgetid;
            });

            // 2. Set status to "Remove"
            vm.transactions.splice(index, 1);
            //vm.transactions[index].Status = "Remove";

            // 3. Update total amount on payment
            updateTotalAmount();
        }

        function clearAllTransaction() {
            vm.transactions = [];
        }

        function updateTotalAmount() {
            // Initial total amount value to zero
            vm.payment.TotalAmount = 0;

            // for loop on each transaction and add to total amount.
            for (var i = 0; i < vm.transactions.length; i++) {
                vm.payment.TotalAmount += vm.transactions[i].Amount;

                //if (vm.transactions[i].Status === '0') {
                //    vm.payment.TotalAmount += vm.transactions[i].Amount;
                //}
            }
        }

        function submit() {
            vm.addNewTransactionError = "";
            if (vm.transactions.length > 0) {
                vm.isSubmitting = true;
                preparePaymentToSave();                
                apiService.payment().save(vm.payment).$promise.then(callPaymentSuccess, callPaymentError);
            }
            else {
                vm.addNewTransactionError = "* กรุณากดเพิ่มรายการงบประมาณ";
            }

            function callPaymentSuccess(response) {
                var paymentid = hr.respondSuccess(response);
                //TODO modal result.
                if (response.isSuccess) {
                    openModal('lg', null, paymentid);
                }
                vm.isSubmitting = false;
            }

            function callPaymentError(e) {
                hr.repondError(e);
                vm.isSubmitting = false;
            }
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

        // prepare form data function
        function prepareData() {
            // Set default to normal
            vm.payment.Type = vm.paymentType.internal;

            // 1. Get working costcenter id
            vm.payment.CostCenterID = authInfo.getWorkingCostCenter().CostCenterID;
            
            // 2. Get list of employees
            apiService.employee().get().$promise.then(callEmpSuccess, callError);

            // 3. Get list of budgets
            apiService.budgetinfo()
                .get({ costcenterid: vm.payment.CostCenterID })
                .$promise.then(callBudgetSuccess, callError);

            // 4. Get list of contractors
            apiService.contractor().get().$promise.then(callContractorSuccess, callError);

            function callEmpSuccess(response) {
                vm.employees = hr.respondSuccess(response);
                if (vm.payment.CostCenterID) {
                    vm.displayEmployees = vm.employees.filter(function (data) {
                        return data.CostCenterID === vm.payment.CostCenterID;
                    });
                }
                // Set default RequestBy to current user
                vm.payment.RequestBy = authInfo.getUser().EmployeeID;
            }

            function callBudgetSuccess(response) {
                vm.budgets = hr.respondSuccess(response);

                //vm.years = $filter('unique')(vm.budgets, 'Year');
                var currentYear = new Date().getFullYear();
                for (var i = 2016; i <= currentYear + 1; i++) {
                    vm.years.push({ Year: i + 543 + '' });
                }

                vm.payment.Year = currentYear + 543 + '';
            }

            function callContractorSuccess(response) {
                vm.contractors = hr.respondSuccess(response);
            }
        }

        function callError(e) {
            hr.respondError(e);
        }

        // Modal result 
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


/****** Result Payment Controller *******/
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('ResultPaymentCtrl', ResultPaymentCtrl);

    // Inject dependency
    ResultPaymentCtrl.$inject = ['$state', '$uibModalInstance', 'apiService', 'handleResponse', 'paymentid'];
    function ResultPaymentCtrl($state, $uibModalInstance, apiService, hr, paymentid) {
        var vm = this;
        vm.paymentid = paymentid;
        vm.next = next;
        vm.update = update;
        vm.print = print;

        apiService.payment().get({ id: vm.paymentid, type: 'vm'}).$promise.then(callApiSuccess, callApiError);

        function callApiSuccess(response) {
            vm.payment = hr.respondSuccess(response);
            console.log(vm.payment);
        }

        function callApiError(e) {
            hr.respondError(e);
        }

        function next() {
            $uibModalInstance.close();
            $state.go('payment');
        }

        function update() {
            $uibModalInstance.close();
            $state.go('editpayment', { id: paymentid });
        }

        function print() {
            window.print();
        }
    }
})();



/****** Details Payment Controller *******/
(function () {
    'use strict';

    angular
        .module('budgetApp')
        .controller('DetailPaymentCtrl', DetailPaymentCtrl);

    DetailPaymentCtrl.$inject = ['$state', '$stateParams', 'apiService', 'handleResponse'];

    function DetailPaymentCtrl($state, $stateParams, apiService, hr ) {
        var vm = this;
        vm.paymentid = $stateParams.id;
        vm.print = print;

        apiService.payment().get({ id: vm.paymentid, type: 'vm' }).$promise.then(callApiSuccess, callApiError);

        function callApiSuccess(response) {
            vm.payment = hr.respondSuccess(response);
        }

        function callApiError(e) {
            hr.respondError(e);
        }

        function print() {
            window.print();
        }

    }

})();


/****** Edit Payment Controller *******/
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('EditPaymentCtrl', EditPaymentCtrl);

    EditPaymentCtrl.$inject = ['$scope', '$state', '$stateParams', '$filter', '$uibModal', 'apiService', 'apiIdmService', 'handleResponse', 'authInfo', 'msgService'];

    function EditPaymentCtrl($scope, $state, $stateParams, $filter, $uibModal, apiService, apiIdmService, hr, authInfo, msgService) {
        // variable
        var paymentid = $stateParams.id;
        var vm = this;

        vm.budgets = [];            // budget list in dropdown
        vm.employees = [];          // all employee in costcenters
        vm.displayEmployees = [];   // employee list in dropdown
        vm.years = [];              // budget year list in dropdown
        vm.payment = {};            // payment form data 
        vm.transactions = [];       // transaction inside payment
        vm.contractors = [];        // Contractor List

        vm.isSubmitting = false;

        // Payment Type
        vm.paymentType = {
            internal: 1,
            pea: 2,
            contractor: 3
        }

        vm.requestbynormal = {
            employeecode: ''
        }

        vm.requestbypea = {
            employeecode: '',
            name: '',
            position: '',
            costcenterid: ''
        }

        vm.requestbyother = {
            id: '',
            name: ''
        }

        // function 
        vm.addNewTransaction = addNewTransaction;
        vm.removeTransaction = removeTransaction;
        vm.updateTotalAmount = updateTotalAmount;
        vm.submit = submit;
        vm.getEmployeeProfile = getEmployeeProfile;
        vm.onRequestByOtherBlur = onRequestByOtherBlur;
        vm.onYearChange = onYearChange;

        // initial form data
        prepareData();

        // Watching 
        //// - Watch year change to clear budget 
        //$scope.$watch(watchingYear, onYearChange);

        //function watchingYear(scope) {
        //    return vm.payment.Year;
        //}

        function onYearChange() {
            clearAllTransaction();
        }

        function addNewTransaction() {

            vm.addNewTransactionError = "";

            // 1. Looking for budget is already added?
            var index = vm.transactions.findIndex(function (obj) {
                return obj.BudgetID == vm.selectbudget;
            })

            // 2. If exist, then set status to active
            if (index > -1) {
                vm.addNewTransactionError = "* บัญชีนี้มีอยู่แล้วในรายการ";
            }
            // 3. If not exist, then add to list
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

            // 4. Set selectbudget to empty
            vm.selectbudget = ''

            // 5. Update total amount
            updateTotalAmount();
        }

        function removeTransaction(budgetid) {
            // 1. Find index of budgetid in transactions
            var index = vm.transactions.findIndex(function (obj) {
                return obj.BudgetID == budgetid;
            });

            // 2. Set status to "Remove"
            vm.transactions.splice(index, 1);
            //vm.transactions[index].Status = "Remove";

            // 3. Update total amount on payment
            updateTotalAmount();
        }

        function clearAllTransaction() {
            vm.transactions = [];
        }

        function updateTotalAmount() {
            // Initial total amount value to zero
            vm.payment.TotalAmount = 0;

            // for loop on each transaction and add to total amount.
            for (var i = 0; i < vm.transactions.length; i++) {
                vm.payment.TotalAmount += vm.transactions[i].Amount;
            }
        }

        function submit() {
            vm.addNewTransactionError = "";
            if (vm.transactions.length > 0) {
                vm.isSubmitting = true;
                preparePaymentToSave();
                apiService.payment().update(vm.payment).$promise.then(callPaymentSuccess, callPaymentError);
            }
            else {
                vm.addNewTransactionError = "* กรุณากดเพิ่มรายการงบประมาณ";
            }

            function callPaymentSuccess(response) {
                hr.respondSuccess(response);
                
                if (response.isSuccess) {
                    openModal('lg', null, vm.payment.PaymentID);
                }
                vm.isSubmitting = false;
            }

            function callPaymentError(e) {
                hr.repondError(e);
                vm.isSubmitting = false;
            }
        }

        function preparePaymentToSave() {
            vm.payment.BudgetTransactions = vm.transactions;
            console.log(vm.payment.Type);
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
            console.log(vm.requestbyother);
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

        // prepare form data function
        function prepareData() {

            var costcenterid = authInfo.getWorkingCostCenter().CostCenterID;

            // 1. Get list of employees
            apiService.employee().get().$promise.then(callEmpSuccess, callError);

            // 2. Get list of budgets
            apiService.budgetinfo().get({costcenterid: costcenterid}).$promise.then(callBudgetSuccess, callError);

            // 3. Get list of contractors
            apiService.contractor().get().$promise.then(callContractorSuccess, callError);

            // 4. Get payment info
            apiService.payment().get({ id: paymentid }).$promise.then(callPaymentSuccess, callError);


            // function section // 
            function callEmpSuccess(response) {
                vm.employees = hr.respondSuccess(response);
            }

            function callBudgetSuccess(response) {
                vm.budgets = hr.respondSuccess(response);
               
            }

            function callContractorSuccess(response) {
                vm.contractors = hr.respondSuccess(response);
            }

            function callPaymentSuccess(response) {
                vm.payment = hr.respondSuccess(response);
                
                // Year list
                var currentYear = new Date().getFullYear();
                for (var i = 2016; i <= currentYear + 1; i++) {
                    vm.years.push({ Year: i + 543 + '' });
                }

                // Transaction List
                vm.transactions = vm.payment.BudgetTransactions;

                // Request by
                if (vm.payment.CostCenterID) {
                    vm.displayEmployees = vm.employees.filter(function (data) {
                        return data.CostCenterID === vm.payment.CostCenterID;
                    });
                }

                if (vm.payment.Type == vm.paymentType.internal) {
                    vm.requestbynormal.employeecode = vm.payment.RequestBy;
                }
                else if (vm.payment.Type == vm.paymentType.pea) {
                    vm.requestbypea.employeecode = vm.payment.RequestBy;
                    getEmployeeProfile();
                }
                else if (vm.payment.Type == vm.paymentType.contractor) {
                    vm.requestbyother.id = vm.payment.ContractorID;
                    vm.requestbyother.name = vm.payment.Contractor.Name;
                }
            }
            
        }

        function callError(e) {
            hr.respondError(e);
        }

        // Modal result 

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

            }, function () {

            });
        };

    }

})();

(function () {
    'use strict';

    angular
        .module('budgetApp')
        .controller('PaymentTransactionCtrl', PaymentTransactionCtrl);

    PaymentTransactionCtrl.$inject = ['$state', '$stateParams', 'apiService', 'handleResponse'];

    function PaymentTransactionCtrl($state, $stateParams, apiService, hr) {
        var vm = this;
     
        apiService.paymenttransaction().get({ year: '2561', costcenterid: 'H301023010' }).$promise.then(callApiSuccess, callApiError);

        function callApiSuccess(response) {
            vm.transaction = hr.respondSuccess(response);
            console.log(vm.transaction);
        }

        function callApiError(e) {
            hr.respondError(e);
        }

        vm.gridOptions = {
            enableSorting: true,
            enableFiltering: true,
            enableColumnResizing: true,
            columnDefs: [
                {
                    name: 'rowNum',
                    displayName: 'ลำดับ',
                    cellTemplate: '<div class="ui-grid-cell-contents text-center">{{grid.renderContainers.body.visibleRowCache.indexOf(row) + 1}}</div>',
                    enableFiltering: false,
                    headerCellClass: 'text-center',
                    width: 50
                },

                //{ name: 'rowNum', displayName: 'Row Number', cellTemplate: '{{rowRenderIndex + 1}}' },
                //{ field: 'index', displayName: 'Index', width: '50', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.renderContainers.body.visibleRowCache.indexOf(row)+(grid.options.paginationPageSize*(grid.options.paginationCurrentPage-1))+1}}</div>' },
                { name: 'รหัสบัญชี', field: 'AccountID', headerCellClass: 'text-center', cellClass: 'text-center'},
                { name: 'ชื่อบัญชี', field: 'AccountName', headerCellClass: 'text-center', cellClass: 'text-center' },
                { name: 'เลขที่การเบิกจ่าย', field: 'PaymentNo', headerCellClass: 'text-center', cellClass: 'text-center' },
                { name: 'รายละเอียด', field: 'Description', headerCellClass: 'text-center', cellClass: 'text-center' },
                { name: 'ผู้เบิก', field: 'RequestByFullName', headerCellClass: 'text-center', cellClass: 'text-center' },
                { name: 'วันที่', field: 'CreatedAtText', headerCellClass: 'text-center', cellClass: 'text-center' },
                { name: 'จำนวน', field: 'Amount', headerCellClass: 'text-center', cellClass: 'text-center' }
            ],
            data: 'vm.transaction'
        };
    }
})();


//budgetApp.controller('DetailPaymentController',
//    ['$scope', '$stateParams', '$location', 'apiService',
//        function ($scope, $stateParams, $location, apiService) {

//            $scope.PaymentID = $stateParams.id
//            $scope.payment = {}

//            //Get payment detail
//            apiService.payment().get({ id: $scope.PaymentID }).$promise.then(function (data) {
//                if (data.isSuccess) {
//                    $scope.payment = data.Result
//                }
//                else {
//                    console.log(data)
//                }
//            }, function (error) {
//                console.log(error)
//            })

//            $scope.removePayment = function (paymentid) {
//                var payment = {}
//                payment.id = paymentid
//                var confirm = window.confirm('ยืนยันการลบ?')
//                if (confirm) {
//                    apiService.payment().remove(payment).$promise.then(function (data) {
//                        if (data.isSuccess) {
//                            $location.path('payment')
//                        }
//                    })
//                }
//            }

//        }])

//budgetApp.controller('CreatePaymentController',
//    ['$scope', '$location', '$filter', 'apiService', 'funcFactory',
//        function ($scope, $location, $filter, apiService, funcFactory) {

//            apiService.emptypayment().get().$promise.then(function (data) {
//                funcFactory.getData($scope, 'payment', data)
//                $scope.payment.Year = '2560'
//            }, function (error) {
//                funcFactory.handleError($scope, error)
//            })

//            //$scope.emplist
//            apiService.employee().get().$promise.then(function (data) {
//                funcFactory.getData($scope, 'emplist', data)

//            }, function (error) {
//                funcFactory.handleError($scope, error)
//            })

//            //$scope.controller
//            apiService.controller().get().$promise.then(function (data) {
//                funcFactory.getData($scope, 'controller', data)
//            }, function (error) {
//                funcFactory.handleError($scope, error)
//            })

//            //$scope.budgetlist
//            apiService.budget().get().$promise.then(function (data) {
//                funcFactory.getData($scope, 'budgetlist', data)
//            }, function (error) {
//                funcFactory.handleError($scope, error)
//            })


//            //Watch RequestBy index change
//            $scope.$watch('payment.RequestBy', function () {
//                //Check payment and emplist are loaded
//                if ($scope.payment && $scope.emplist) {
//                    var requester = $filter('filter')($scope.emplist, function (d) {
//                        return d.EmployeeID === $scope.payment.RequestBy
//                    })
//                    $scope.payment.CostCenterID = requester[0].CostCenterID
//                }
//            })

//            //function addNewStatement() 
//            $scope.addNewStatement = funcFactory.payment.addNewStatement.bind(this, $scope)

//            $scope.removeStatement = funcFactory.payment.removeStatement.bind(this, $scope)

//            $scope.updateTotalAmount = funcFactory.payment.updateTotalAmount.bind(this, $scope)


//            //function submit()
//            $scope.submit = function () {
//                apiService.payment().save($scope.payment).$promise.then(function (data) {
//                    console.log(data)
//                    if (data.isSuccess) {
//                        $location.path('/payment/' + data.Result)
//                    }
//                    else {
//                        funcFactory.setError($scope, data)
//                    }
//                }, function (error) {

//                })
//            }



//        }])

//budgetApp.controller('EditPaymentController', ['$scope', '$location', '$filter', '$stateParams', 'apiService', 'funcFactory'
//    , function ($scope, $location, $filter, $routeParams, apiService, funcFactory) {


//        var paymentid = $routeParams.id

//        apiService.payment().get({ id: paymentid }).$promise.then(function (data) {
//            funcFactory.getData($scope, 'payment', data)
//        }, function (error) {
//            funcFactory.handleError($scope, error)
//        })

//        //$scope.emplist
//        apiService.employee().get().$promise.then(function (data) {
//            funcFactory.getData($scope, 'emplist', data)
//        }, function (error) {
//            funcFactory.handleError($scope, error)
//        })

//        //$scope.controller
//        apiService.controller().get().$promise.then(function (data) {
//            funcFactory.getData($scope, 'controller', data)
//        }, function (error) {
//            funcFactory.handleError($scope, error)
//        })

//        //$scope.budgetlist
//        apiService.budget().get().$promise.then(function (data) {
//            funcFactory.getData($scope, 'budgetlist', data)
//        }, function (error) {
//            funcFactory.handleError($scope, error)
//        })


//        //function addNewStatement() 
//        $scope.addNewStatement = funcFactory.payment.addNewStatement.bind(this, $scope)

//        //function removeStatement
//        $scope.removeStatement = funcFactory.payment.removeStatement.bind(this, $scope)

//        //function updateTotalAmount
//        $scope.updateTotalAmount = funcFactory.payment.updateTotalAmount.bind(this, $scope)

//        //function submit()
//        $scope.submit = function () {
//            apiService.payment().save($scope.payment).$promise.then(function (data) {
//                console.log(data)
//                if (data.isSuccess) {
//                    $location.path('/payment/' + data.Result)
//                }
//                else {
//                    funcFactory.handleError($scope, error)
//                }
//            }, function (error) {
//                funcFactory.setError($scope, data)
//            })
//        }

//    }])

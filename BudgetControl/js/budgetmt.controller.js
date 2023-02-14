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

            initDatePicker('#textSeminarDate');

            //setTimeout(() => {
            //    console.log('init datepicker');
            //    initDatePicker('#textSeminarDate');
            //}, 1000)
        }

        function populateYearList() {

            vm.years = [];
            var currentYear = new Date().getFullYear();
            for (var i = 2023; i <= currentYear; i++) {
                vm.years.push(i + 543 + '');
            }

            vm.formRequest.Year = currentYear + 543 + '';
        }

        function calculateTotalAmount() {
            //let mealCount = (vm.formRequest.HasMealMorning ? 1 : 0) + (vm.formRequest.HasMealAfternoon ? 1 : 0);
            vm.formRequest.TotalAmount = parseInt(vm.formRequest.MealCount) * 35;

        }

        function submit() {
            console.log(vm.formRequest);
            vm.isSubmitting = true;
            apiService.budgetmt().save(vm.formRequest).$promise.then(callSaveRequestSuccess, callSaveRequestError);

            function callSaveRequestSuccess(response) {
                console.log(response);

                if (response.isSuccess) {
                    let message = `ศูนย์ต้นทุน ${response.Result.BudgetCostCenter} <br/> รหัสบัญชี ${response.Result.AccountID} <br/> ลำดับการเบิกที่: ${response.Result.MTNo}`;
                    Swal.fire('บันทึกข้อมูลสำเร็จ', message, 'success').then(gotoIndexPage);
                }
                else {
                    Swal.fire('Error', response.Message, 'error');
                }
                vm.isSubmitting = false;
            }

            function callSaveRequestError(e) {
                hr.repondError(e);
                vm.isSubmitting = false;
            }
        }

        function gotoIndexPage() {
            $state.go('transcationbudgetmt');
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
        vm.transactions = [];
        vm.years = [];
        vm.costcenters = [ { id: 'H301000040', name: 'กฟก.2 กอก. ผพอ.' }, { id: 'H301010000', name: 'กฟก.2 ฝวบ.' }, { id: 'H301020000', name: 'กฟก.2 ฝบพ.' }, { id: 'H301030000', name: 'กฟก.2 ฝปบ.' }];
        vm.formRequest = {};
        vm.getTransactionList = getTransactionList;
        var table;

        //methods
        vm.modalConfirmDelete = modalConfirmDelete;

        prepareData();

        function prepareData() {
            populateYearList();
            populateCostcenterList();
            getTransactionList();
        }

        function getTransactionList() {
            console.log('get transaction list');
            vm.transactions = [];
            apiService.budgetmt().get({ year: vm.formRequest.Year, costcenter: vm.formRequest.CostCenter }).$promise.then(callApiSuccess, callApiError)

            function callApiSuccess(data) {
                if (data.isSuccess) {
                    vm.transactions = data.Result;
                    if (table) {
                        table.clear().destroy();
                    }

                    setTimeout(function () {
                        table = $('#tableList').DataTable({
                            dom: 'lBftp',
                            buttons: [{ extend: 'excel', className: 'btn btn-success', title: 'สรุปการเบิกจ่ายงบฝึกอบรม' }]
                        });
                    }, 1000);
                }
                else {
                    Swal.fire('Error', response.Message, 'error');
                }
            }

            function callApiError(e) {
                hr.repondError(e);
                vm.isSubmitting = false;
            }
        }

        function populateCostcenterList() {
            let workingCostcenter = authInfo.getWorkingCostCenter().CostCenterID;
            //let budgetCostCenter = workingCostcenter.substring(0, 6) + '0000';
            //budgetCostCenter = budgetCostCenter == 'H301000000' ? 'H301000040' : budgetCostCenter;
            //console.log(budgetCostCenter);
            vm.costcenters = vm.costcenters.filter((x) => x.id == workingCostcenter || x.id == 'H301000040' || workingCostcenter == 'H301000040');
            //vm.costcenters = vm.costcenters.filter((x) => x.id == workingCostcenter || workingCostcenter == 'H301000040');
            vm.formRequest.CostCenter = workingCostcenter;
        }

        function populateYearList() {

            vm.years = [];
            var currentYear = new Date().getFullYear();
            for (var i = 2022; i <= currentYear; i++) {
                vm.years.push(i + 543 + '');
            }

            vm.formRequest.Year = currentYear + 543 + '';
        }

        async function modalConfirmDelete(transaction) {
            console.log(transaction);
            var result = await Swal.fire({
                icon: 'question',
                title: 'ยืนยันการลบ?',
                text: transaction.MTNo,
                showDenyButton: false,
                showCancelButton: true,
                confirmButtonText: 'ยืนยัน',
                cancelButtonText: `ปิดหน้าต่าง`,
            });

            if (result.isConfirmed) {
                deleteTransaction(transaction);
            }
        }

        function deleteTransaction(transaction) {
            apiService.budgetmt().delete({ id: transaction.Id }).$promise.then(callApiSuccess, callApiError);

            function callApiSuccess(data) {
                if (data.isSuccess) {
                    Swal.fire('Success', 'บันทึกข้อมูลสำเร็จ', 'success').then(getTransactionList);
                }
                else {
                    Swal.fire('Error', data.Message, 'error');
                }
            }

            function callApiError(e) {
                hr.repondError(e);
                vm.isSubmitting = false;
            }
        }
    }

})();


(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('EditRequestBudgetMTCtrl', EditRequestBudgetMTCtrl);

    EditRequestBudgetMTCtrl.$inject = ['$scope', '$state', '$stateParams', '$filter', '$uibModal', 'apiService', 'apiIdmService', 'handleResponse', 'authInfo', 'msgService'];

    function EditRequestBudgetMTCtrl($scope, $state, $stateParams, $filter, $uibModal, apiService, apiIdmService, hr, authInfo, msgService) {
        // variable
        console.log($stateParams);
        var transactionid = $stateParams.id;
        var vm = this; vm.formRequest = {};            // payment form data 

        console.log(transactionid);
        vm.years = [];              // budget year list in dropdown
        vm.formRequest = {};


        //function 
        vm.calculateTotalAmount = calculateTotalAmount;
        vm.submit = submit;

        //initial form data
        prepareData();

        function prepareData() {

            populateYearList();

            apiService.requestmt().get({ id: transactionid }).$promise.then(callAPISuccess, callAPIError);

            initDatePicker('#textSeminarDate');

            function callAPISuccess(response) {
                if (response.isSuccess) {
                    vm.formRequest = response.Result;
                }
                else {
                    Swal.fire('Error', response.Message, 'error');
                }
                vm.isSubmitting = false;
            }

            function callAPIError(e) {
                hr.repondError(e);
            }

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
            //let mealCount = (vm.formRequest.HasMealMorning ? 1 : 0) + (vm.formRequest.HasMealAfternoon ? 1 : 0);
            vm.formRequest.TotalAmount = parseInt(vm.formRequest.MealCount) * 35;
        }



        function submit() {
            console.log(vm.formRequest);
            vm.isSubmitting = true;
            apiService.budgetmt().update(vm.formRequest).$promise.then(callSaveRequestSuccess, callSaveRequestError);

            function callSaveRequestSuccess(response) {
                console.log(response);

                if (response.isSuccess) {

                    let message = `ศูนย์ต้นทุน H301000040 <br/> รหัสบัญชี 52030030 <br/> ลำดับการเบิกที่: ${response.Result.MTNo}`;
                    Swal.fire('บันทึกข้อมูลสำเร็จ', message, 'success').then(gotoIndexPage);
                }
                else {
                    Swal.fire('Error', response.Message, 'error');
                }
                vm.isSubmitting = false;
            }

            function callSaveRequestError(e) {
                hr.repondError(e);
                vm.isSubmitting = false;
            }
        }

        function gotoIndexPage() {
            $state.go('transcationbudgetmt');
        }


        function callError(e) {
            hr.respondError(e);
        }


    }
})();

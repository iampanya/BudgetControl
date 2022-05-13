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
            let mealCount = (vm.formRequest.HasMealMorning ? 1 : 0) + (vm.formRequest.HasMealAfternoon ? 1 : 0);
            vm.formRequest.TotalAmount = parseInt(vm.formRequest.ParticipantCount) * (35 * mealCount);
        }

        function submit() {
            console.log(vm.formRequest);
            vm.isSubmitting = true;
            apiService.budgetmt().save(vm.formRequest).$promise.then(callSaveRequestSuccess, callSaveRequestError);

            function callSaveRequestSuccess(response) {
                console.log(response);

                if (response.isSuccess) {
                    Swal.fire('บันทึกข้อมูลสำเร็จ', response.Result.MTNo, 'success').then(gotoIndexPage);
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
        vm.formRequest = {};

        prepareData();

        function prepareData() {
            populateYearList();
            getTransactionList();
        }

        function getTransactionList() {
            apiService.budgetmt().get({ year: vm.formRequest.Year }).$promise.then(callApiSuccess, callApiError)

            function callApiSuccess(data) {
                if (data.isSuccess) {
                    vm.transactions = data.Result;

                    console.log(vm.transactions);
                    
                    setTimeout(function () {
                        $('#tableList').DataTable();
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

        function populateYearList() {

            vm.years = [];
            var currentYear = new Date().getFullYear();
            for (var i = 2022; i <= currentYear; i++) {
                vm.years.push(i + 543 + '');
            }

            vm.formRequest.Year = currentYear + 543 + '';
        }



    }

})();

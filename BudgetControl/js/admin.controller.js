﻿
/**
 * ManageWorkingCC Controller
 * **/
(function () {
    'use strict';

    angular.module('budgetApp')
        .controller('ManageWorkingCCCtrl', ManageWorkingCCCtrl);

    ManageWorkingCCCtrl.$inject = ['$uibModal', 'apiService'];

    function ManageWorkingCCCtrl($uibModal, apiService) {
        var vm = this;
        vm.filter = '';
        vm.conditionList = [];

        vm.openCreateForm = openCreateForm;
        vm.getConditionList = getConditionList;
        vm.deleteCondition = deleteCondition;


        function getConditionList() {
            apiService.workingCondition().get({ filter: vm.filter }).$promise
                .then(
                    function (data) {
                        if (data.isSuccess) {
                            vm.conditionList = data.Result;
                        }
                        else {

                        }
                    },
                    function (error) {
                        console.log(error);
                    }
                );
        }

        function deleteCondition(condition) {
            console.log('delete id : ' + condition.Id);
        }



        function openCreateForm() {

            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'admin/AddWorkingCC',
                controller: 'AddWorkingCCCtrl',
                controllerAs: 'vm',
                size: ''
            });

            modalInstance.result.then(function (data) {
                console.log(data);
            });
        }
    }

    })();


/**
 * AddWorkingCC Controller
 * **/
(function () {
    'use strict';

    angular.module('budgetApp')
        .controller('AddWorkingCCCtrl', AddWorkingCCCtrl);

    AddWorkingCCCtrl.$inject = ['$uibModal', '$uibModalInstance', 'userApi']

    function AddWorkingCCCtrl($uibModal, $uibModalInstance, userApi) {
        var vm = this;
        vm.save = save;
        vm.close = close;
        vm.errorMessage = '';
        vm.form = {
            EmployeeNo: '',
            CostCenterCode: '',
            Condition: '',
            CCAStart: '',
            CCAEnd: '',
        }

        function save() {
            clearError();
            userApi.addWorkingList().send(vm.form).$promise.then(
                function (data) {
                    if (data.isSuccess) {
                        $uibModalInstance.close(true);
                    }
                    else {
                        displayError(data.Message);                        
                    }
                },
                function (error) {
                    displayError(error);
                    console.log(error);
                }
            )
        }
        
        function close() {
            console.log('close called');
        }

        function displayError(msg) {
            vm.errorMessage = msg;
        }

        function clearError() {
            vm.errorMessage = '';
        }

        

    }
})();
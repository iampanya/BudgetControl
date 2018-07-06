
/**
 * ManageWorkingCC Controller
 * **/
(function () {
    'use strict';

    angular.module('budgetApp')
        .controller('ManageWorkingCCCtrl', ManageWorkingCCCtrl);

    ManageWorkingCCCtrl.$inject = ['$uibModal', 'apiService', 'msgService'];

    function ManageWorkingCCCtrl($uibModal, apiService, msgService) {
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
                            msgService.setErrorMsg('ไม่สามารถดึงข้อมูลได้ : ' + data.Message);
                        }
                    },
                function (error) {
                    msgService.setErrorMsg('ไม่สามารถดึงข้อมูลได้ : ' + error.statusText);
                        console.log(error);
                    }
                );
        }

        function deleteCondition(condition) {
            apiService.deleteCondition().delete({ id: condition.Id }).$promise
                .then(
                function (data) {
                    if (data.isSuccess) {
                        var index = vm.conditionList.findIndex(function (obj) {
                            return obj.Id == condition.Id;
                        });
                        vm.conditionList.splice(index, 1);
                        msgService.setSuccessMsg('ลบรายการสำเร็จ');
                    }
                    else {
                        msgService.setErrorMsg('ไม่สามารถลบรายการได้ : ' + data.Message);
                    }
                },
                function (error) {
                    console.log(error);
                    msgService.setErrorMsg('ไม่สามารถลบรายการได้ : ' + data.statusText);
                }
                );
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
                    displayError(error.statusText);
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
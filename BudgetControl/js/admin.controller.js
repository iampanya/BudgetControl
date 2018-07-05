
/**
 * ManageWorkingCC Controller
 * **/
(function () {
    'use strict';

    angular.module('budgetApp')
        .controller('ManageWorkingCCCtrl', ManageWorkingCCCtrl);

    ManageWorkingCCCtrl.$inject = ['$uibModal']

    function ManageWorkingCCCtrl($uibModal) {
        var vm = this;
        vm.openCreateForm = openCreateForm;

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

    AddWorkingCCCtrl.$inject = ['$uibModal', '$uibModalInstance']

    function AddWorkingCCCtrl($uibModal, $uibModalInstance) {
        var vm = this;
        vm.save = save;
        vm.close = close;
        vm.form = {
            EmployeeNo: '',
            CostCenterCode: '',
            Condition: '',
            CCAStart: '',
            CCAEnd: '',
        }

        function save() {
            console.log('save called');
            console.log(vm.form);
        }

        function close() {
            console.log('close called');
        }

    }
})();
(function () {
    'use strict';

    angular.module('budgetApp')
        .controller('ManageWorkingCCCtrl', ManageWorkingCCCtrl);

    ManageWorkingCCCtrl.$inject = ['$uibModal']

    function ManageWorkingCCCtrl($uibModal) {
        var vm = this;
        vm.openCreateForm = openCreateForm;

        function openCreateForm() {
            console.log('open form');
        }
        return vm;
    }

})();
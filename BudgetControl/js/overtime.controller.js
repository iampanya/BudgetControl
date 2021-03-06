﻿(function () {
    'use strict'
    angular
        .module('budgetApp')
        .controller('OverTimeCtrl', OverTimeCtrl);

    OverTimeCtrl.$inject = ['$state']

    function OverTimeCtrl($state) {

    }

})();

(function () {
    'use strict'
    angular
        .module('budgetApp')
        .controller('CreateOverTimeCtrl', CreateOverTimeCtrl);

    CreateOverTimeCtrl.$inject = ['$state']

    function CreateOverTimeCtrl($state) {
        var vm = this;

        vm.otList = [];
        vm.addList = addList;
        vm.removeList = removeList;

        vm.addList();

        function addList() {
            vm.otList.push({
                salary: '0',
                multiply: "1",
                hours: 1,
                money: 0
            });
        }


        function removeList() {
            vm.otList = [];
            addList();
        }
        return vm;
    }

})();
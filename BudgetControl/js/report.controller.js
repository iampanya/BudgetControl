(function () {
    angular.module('budgetApp')
        .controller('ReportCtrl', ReportCtrl);

    function ReportCtrl() {
        var vm = this;
        vm.test = 'test page';
    }
})();
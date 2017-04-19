(function () {
    angular.module('budgetApp')
        .controller('ReportCtrl', ReportCtrl);

    function ReportCtrl() {
        var vm = this;
        vm.test = 'test page';
    }
})();


/***************** Individual Report **********************/
(function () {
    angular.module('budgetApp')
        .controller('IndividualReportCtrl', IndividualReportCtrl);

    IndividualReportCtrl.$inject = ['$stateParams', 'apiService', 'handleResponse', 'authInfo'];

    function IndividualReportCtrl($stateParams, apiService, hr, authInfo) {
        var vm = this;
        vm.userid = $stateParams.id || authInfo.getUser().EmployeeID;
        vm.employees = [];
        vm.budgets = [];
        vm.year = '2560';
        vm.getReportData = getReportData;

        apiService.employee().get().$promise.then(callEmpSuccess, callApiError);

        function callEmpSuccess(response) {
            vm.employees = hr.respondSuccess(response);
            // Set default RequestBy to current user
            getReportData();
        }

        function getReportData() {
            apiService.individualreport().get({ id: vm.userid, year: vm.year }).$promise.then(callApiSuccess, callApiError);
        }
        
        function callApiSuccess(response) {
            vm.budgets = hr.respondSuccess(response);
            console.log(vm.budgets);
        }

        function callApiError(e) {
            hr.respondError(e);
        }

        
    }
})();



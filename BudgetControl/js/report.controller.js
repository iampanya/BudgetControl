(function () {
    angular.module('budgetApp')
        .controller('ReportCtrl', ReportCtrl);

    ReportCtrl.$inject = ['apiService', 'handleResponse', 'authInfo'];

    function ReportCtrl(apiService, hr, authInfo) {
        var vm = this;
        vm.working = authInfo.getWorkingCostCenter();
        vm.budgets = [];
        vm.getReportData = getReportData;

        populateYearList();
        getReportData();

        function getReportData() {
            apiService.summaryreport().get({ year: vm.year }).$promise.then(callApiSuccess, callApiError);
        }

        function callApiSuccess(response) {
            vm.budgets = hr.respondSuccess(response);
            console.log(vm.budgets);
        }

        function callApiError(e) {
            hr.respondError(e);
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
        vm.getReportData = getReportData;
        vm.onYearChange = onYearChange;

        populateYearList();
        getEmployee();

        function getEmployee() {
            apiService.employee().get({ retireYear: vm.year }).$promise.then(callEmpSuccess, callApiError);

            function callEmpSuccess(response) {
                vm.employees = hr.respondSuccess(response);
                // Set default RequestBy to current user
                getReportData();
            }
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

        function populateYearList() {
            // Populate year list from 2559 to current + 1 and set default to current year
            vm.years = [];
            var currentYear = new Date().getFullYear();
            for (var i = 2016; i <= currentYear + 1; i++) {
                vm.years.push(i + 543 + '');
            }

            vm.year = currentYear + 543 + '';
        }

        function onYearChange() {
            apiService.employee().get({ retireYear: vm.year }).$promise.then(callEmpSuccess, callApiError);

            function callEmpSuccess(response) {
                vm.employees = hr.respondSuccess(response);
                // Set default RequestBy to current user
            }
        }
        
    }
})();



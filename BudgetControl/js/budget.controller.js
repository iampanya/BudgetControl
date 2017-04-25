
budgetApp.controller('BudgetController', ['$scope', '$location', 'apiService', function ($scope, $location, apiService) {

    apiService.budget().get().$promise.then(function (data) {
        console.log(data)
        if (data.isSuccess) {
            $scope.budgets = data.Result
        }
    }, function (error) {

    })

    $scope.rowClick = function (budgetid) {
        $location.path('/budget/' + budgetid)
    }

}])



budgetApp.controller('DetailBudgetController', ['$scope', '$location', '$routeParams', 'apiService', 'funcFactory', function ($scope, $location, $routeParams, apiService, funcFoctory) {

    $scope.BudgetID = $routeParams.id

    apiService.budget().get({ id: $scope.BudgetID }).$promise.then(function (data) {
        funcFoctory.getData($scope, 'budget', data)
    }, function (error) {
        funcFoctory.handleError($scope.error)
    })

    $scope.rowClick = function (paymentid) {
        $location.path('/payment/' + paymentid)
    }
}])

budgetApp.controller('CreateBudgetController', ['$scope', 'apiService', 'funcFactory', function ($scope, apiService, funcFactory) {

    //Get Budget
    apiService.account().get().$promise.then(function (data) {
        funcFactory.getData($scope, 'accounts', data)
        console.log($scope.accounts)
    }, function (error) {
        funcFactory.handleError($scope, error)
    })

    $scope.Account = {
        AccountID: '',
        AccountName: ''
    }

    $scope.Budget = {

    }

    $scope.accountChange = function () {
        $scope.Account.AccountName = findAccount($scope.Account.AccountID)
    }

    function findAccount(accountid) {
        var index = $scope.accounts.findIndex(function (obj) {
            return obj.AccountID == accountid
        })
        if (index >= 0) {
            return $scope.accounts[index].AccountName
        }
        else {
            return '';
        }

    }


    $scope.submit = function () {
        if (findAccount($scope.Account.AccountID)) {
            console.log('older')
        }
        else {
            console.log('new')
        }
    }

}])


    ;

/********** BUDGET CONTROLLER *****************/

(function () {
    angular.module('budgetApp')
        .controller('BudgetCtrl', BudgetCtrl);

    BudgetCtrl.$inject = ['$state', '$filter', '$uibModal', 'apiService', 'msgService', 'handleResponse'];

    function BudgetCtrl($state, $filter, $uibModal, apiService, msgService, hr) {
        var vm = this;
        vm.budgets = [];
        vm.budgetyear = [];
        vm.costcenters = [];
        vm.deleteBudget = deleteBudget;

        initialData();

        function initialData() {
            // 1. Get budget data from server.
            apiService.budget().get().$promise.then(callSuccess, callError);

            //// 1.1 Call to server success.
            function callSuccess(response) {
                // Move data to budget list
                vm.budgets = hr.respondSuccess(response);

                // Get unique Costcenter and initial it.
                vm.costcenters = $filter('unique')(vm.budgets, 'CostCenterID');
                vm.costcenter = vm.costcenters[0].CostCenterID

                // Get unique budget year and initial it.
                vm.years = $filter('unique')(vm.budgets, 'Year');
                vm.year = '2560';//vm.years[0].Year;
            }

            //// 1.2 Call to server fail.
            function callError(e) {
                hr.respondError(e);
            }
        }

        function deleteBudget(budget) {
            openModal('', null, budget)
        }


        function openModal(size, parentSelector, data) {
            var parentElem = parentSelector ?
                angular.element($document[0].querySelector('.modal-demo ' + parentSelector)) : undefined;
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'budgets/ConfirmDelete',
                controller: 'ConfirmDeleteBudgetCtrl',
                controllerAs: 'vm',
                size: size,
                //backdrop: 'static',
                //keyboard: false,
                appendTo: parentElem,
                resolve: {
                    budget: function () {
                        return data;
                    }
                }
            });

            modalInstance.result.then(function () {
                console.log('result modal');
                initialData();
            });
        };
    }

})();


(function () {
    angular.module('budgetApp')
        .controller('ConfirmDeleteBudgetCtrl', ConfirmDeleteBudgetCtrl);

    ConfirmDeleteBudgetCtrl.$inject = ['$state', '$uibModalInstance', 'apiService', 'handleResponse', 'msgService', 'budget']
    function ConfirmDeleteBudgetCtrl($state, $uibModalInstance, apiService, hr, msgService, budget) {

        var vm = this;
        vm.budget = budget;
        vm.closemodal = closemodal;
        vm.removebudget = removebudget;


        function closemodal() {
            $uibModalInstance.close();
        }

        function removebudget() {
            apiService.budget().remove({ id: budget.BudgetID }).$promise.then(deleteSuccess, deleteError);



            function deleteSuccess(response) {
                if (hr.respondSuccess(response)) {
                    msgService.setSuccessMsg('ลบรายการสำเร็จ');
                    $uibModalInstance.close();
                }
            }

            function deleteError(e) {
                hr.respondError(e);
                $uibModalInstance.close();
            }
        }
    }

})();

/********** DETAIL CONTROLLER *****************/
(function () {
    angular.module('budgetApp')
        .controller('DetailBudgetCtrl', DetailBudgetCtrl);

    DetailBudgetCtrl.$inject = ['$state', '$stateParams', 'apiService', 'msgService', 'handleResponse'];

    function DetailBudgetCtrl($state, $stateParams, apiService, msgService, hr) {
        var id = $stateParams.id;
        var vm = this;

        apiService.budget().get({ id: id }).$promise.then(callSuccess, callError);

        function callSuccess(response) {
            vm.budget = hr.respondSuccess(response);
            console.log(vm.budget);
        }

        function callError(e) {
            hr.respondError(e);
        }

    }

})();


/******** UPLOAD CONTROLLER ******************/
(function () {
    angular.module('budgetApp')
        .controller('UploadCtrl', UploadCtrl);

    UploadCtrl.$inject = ['$http', 'apiService', 'msgService'];
    function UploadCtrl($http, apiService, msgService) {

        // DECLARATION
        var vm = this;

        //// Function
        vm.readfile = readfile;
        vm.upload = upload;

        //// Input form data
        vm.form = {
            year: '2560',
            filedata: ''
        };


        vm.filedata = [];

        resetBtnRead();
        resetBtnSave();

        function readfile() {
            disableBtnRead();
            vm.filedata = [];
            var f = document.getElementById('fileBudget').files[0];
            var r = new FileReader();

            r.onloadend = function (e) {
                vm.form.filedata = e.target.result;
                $http.post('data/ReadBudgetFile', vm.form)
                    .then(readFileSuccess, readFileError)

            }

            r.readAsText(f);

            function readFileSuccess(response) {
                if (response.data.isSuccess) {
                    vm.filedata = response.data.Result;
                }
                else {
                    readFileError(response.data.Message);
                }
                resetBtnRead();
            }
            function readFileError(e) {
                console.log(e);
                resetBtnRead();
            }

        }

        function upload() {
            disableBtnSave();
            apiService.uploadbudget().save(vm.filedata).$promise.then(uploadSuccess, uploadError);

            function uploadSuccess(data) {
                if (data.isSuccess) {
                    resetBtnSave();
                    msgService.setSuccessMsg('นำเข้าข้อมูลจาก SAP สำเร็จ');
                }
                else {
                    uploadError(data.Message);
                }
            }
            function uploadError(e) {
                msgService.setErrorMsg(e);
                console.log(e);
                resetBtnSave();
            }
        }

        function resetBtnRead() {
            vm.btnRead = {
                name: 'อ่าน',
                disbled: false
            }
        }

        function disableBtnRead() {
            vm.btnRead = {
                name: 'กำลังอ่านข้อมูล . . .',
                disabled: true
            }
        }

        function resetBtnSave() {
            vm.btnSave = {
                name: 'นำเข้าข้อมูล',
                disbled: false
            }
        }

        function disableBtnSave() {
            vm.btnSave = {
                name: 'กำลังนำเข้าข้อมูล . . .',
                disbled: false
            }
        }

    }

})();


/********** CREATE CONTROLLER **************/

(function () {
    angular.module('budgetApp')
        .controller('CreateBudgetCtrl', CreateBudgetCtrl);

    CreateBudgetCtrl.$inject = ['apiService', 'handleResponse', 'msgService'];

    function CreateBudgetCtrl(apiService, hr, msgService) {
        var vm = this;
        // function
        vm.accountChange = accountChange;
        vm.submit = submit;

        // variable
        vm.accounts = [];
        vm.costcenters = [];
        vm.readonly = false;
        vm.form = {
            year: new Date().getFullYear() + 543 + '',
            accountid: '',
            accountname: '',
            costcenterid: '',
        }
        vm.btnSubmit = {
            text: 'บันทึก',
            disabled: false
        }

        apiService.account().get().$promise.then(callAccountSuccess, callError);

        apiService.costcenter().get().$promise.then(callCostCenterSuccess, callError);


        function callAccountSuccess(response) {
            vm.accounts = hr.respondSuccess(response);
        }

        function callCostCenterSuccess(response) {
            vm.costcenters = hr.respondSuccess(response);
            console.log(vm.costcenters);
        }

        function callError(e) {
            resetBtnSubmit();
            hr.respondError(e);
        }

        function accountChange() {
            vm.form.accountname = findAccount(vm.form.accountid)
        }

        function findAccount(accountid) {
            var index = vm.accounts.findIndex(function (obj) {
                return obj.AccountID == accountid
            })
            if (index >= 0) {
                vm.readonly = true;
                return vm.accounts[index].AccountName
            }
            else {
                vm.readonly = false;
                return '';
            }

        }

        function submit(form) {
            if (form.$valid) {
                disabledBtnSubmit();
                apiService.budget().save({ form: vm.form }).$promise.then(callCreateBudgetSuccess, callError);
            }
        }

        function callCreateBudgetSuccess(response) {
            vm.result = hr.respondSuccess(response);
            if (vm.result) {
                msgService.setSuccessMsg('เพิ่มข้อมูลงบประมาณสำเร็จ');
            }
            resetBtnSubmit();
        }

        function resetBtnSubmit() {
            vm.btnSubmit.text = 'บันทึก';
            vm.btnSubmit.disabled = false;
        }

        function disabledBtnSubmit() {
            vm.btnSubmit.text = 'กำลังบันทึก . . .';
            vm.btnSubmit.disabled = true;
        }

        //apiService.account().get().$promise.then(function (data) {
        //    funcFactory.getData($scope, 'accounts', data)
        //    console.log($scope.accounts)
        //}, function (error) {
        //    funcFactory.handleError($scope, error)
        //})
    }

})();


/******** Edit CONTROLLER ******************/
(function () {
    angular.module('budgetApp')
        .controller('EditBudgetCtrl', EditBudgetCtrl);

    EditBudgetCtrl.$inject = ['$state', '$stateParams', 'apiService', 'msgService', 'handleResponse'];

    function EditBudgetCtrl($state, $stateParams, apiService, msgService, hr) {
        var id = $stateParams.id;
        var vm = this;
        vm.submit = submit;

        apiService.budget().get({ id: id }).$promise.then(callSuccess, callError);

        function callSuccess(response) {
            vm.budget = hr.respondSuccess(response);
        }

        function callError(e) {
            hr.respondError(e);
        }

        function submit() {
            apiService.budget().update({ budget: vm.budget }).$promise.then(updateSuccess, updateError);

            function updateSuccess(response) {
                if (hr.respondSuccess(response)) {
                    var state = { name: 'budget', title: 'รายการงบประมาณ' };
                    msgService.setSuccessMsg('แก้ไขข้อมูลงบประมาณสำเร็จ', state);
                }
            }

            function updateError(e) {
                hr.respondError(e);
            }
        }

    }

})();
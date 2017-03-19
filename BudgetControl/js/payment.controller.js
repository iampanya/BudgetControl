; (function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('PaymentCtrl', PaymentCtrl);

    PaymentCtrl.$inject = ['$state', 'apiService', 'msgService', 'handleResponse'];

    function PaymentCtrl($state, apiService, msgService, hr) {
        var vm = this;
        vm.payments = [];
        vm.rowClick = rowClick;

        // 1. Get payment data from server.
        apiService.payment().get().$promise.then(callSuccess, callError);

        //// 1.1 Call to server success.
        function callSuccess(response) {
            vm.payments = hr.respondSuccess(response);
            console.log(vm.payments);
        }

        //// 1.2 Call to server fail.
        function callError(e) {
            hr.respondError(e);
        }


        function rowClick(paymentid) {
            // Go to detail page.
            $state.go('viewpayment', { id: paymentid });
        }


    }


})();


/****** Create Payment Controller *******/
(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('CreatePaymentCtrl', CreatePaymentCtrl);

    CreatePaymentCtrl.$inject = ['$scope', '$state', '$filter', '$uibModal', 'apiService', 'handleResponse', 'authInfo'];

    function CreatePaymentCtrl($scope, $state, $filter, $uibModal, apiService, hr, authInfo) {
        // variable
        var vm = this;
        vm.budgets = [];            // budget list in dropdown
        vm.employees = [];          // employee list in dropdown
        vm.years = [];              // budget year list in dropdown
        vm.payment = {};            // payment form data 
        vm.transactions = [];       // transaction inside payment

        // function 
        vm.addNewTransaction = addNewTransaction;
        vm.removeTransaction = removeTransaction;
        vm.updateTotalAmount = updateTotalAmount;
        vm.submit = submit;

        // initial form data
        prepareData();

        // Watching 
        //// - Watch RequestBy to changing CostCenter
        $scope.$watch(watchingRequestBy, onRequestByChange);

        function watchingRequestBy(scope) {
            return vm.payment.RequestBy;
        }

        function onRequestByChange() {
            // 1. Check data are loaded.
            if (vm.payment !== {} && vm.employees !== []) {

                // 2. Get requester data from employee list
                var requester = $filter('filter')(vm.employees, function (d) {
                    return d.EmployeeID === vm.payment.RequestBy
                })

                // 3. if found, then set CostCenter value
                if (requester.length > 0) {
                    vm.payment.CostCenterID = requester[0].CostCenterID;
                    //vm.payment.CostCenter = requester[0].CostCenter;
                }

                // 4. if not found, then set CostCenter to nothing.
                else {
                    vm.payment.CostCenterID = '-';
                }
            }
        }

        function addNewTransaction() {
            // 1. Looking for budget is already added?
            var index = vm.transactions.findIndex(function (obj) {
                return obj.BudgetID == vm.selectbudget;
            })

            // 2. If exist, then set status to active
            if (index > -1) {
                vm.transactions[index].Status = "Active"
            }
                // 3. If not exist, then add to list
            else {
                var budget = $filter('filter')(vm.budgets, function (d) {
                    return d.BudgetID === vm.selectbudget
                })[0]

                vm.transactions.push({
                    BudgetTransactionID: '',
                    BudgetID: budget.BudgetID,
                    PaymentID: '',
                    Description: '',
                    Amount: 0.00,
                    PreviousAmount: 0.00,
                    RemainAmount: 0.00,
                    Budget: budget,
                    Status: 'Active',
                })
            }

            // 4. Set selectbudget to empty
            vm.selectbudget = ''

            // 5. Update total amount
            updateTotalAmount();
        }

        function removeTransaction(budgetid) {
            // 1. Find index of budgetid in transactions
            var index = vm.transactions.findIndex(function (obj) {
                return obj.BudgetID == budgetid;
            });

            // 2. Set status to "Remove"
            vm.transactions[index].Status = "Remove";

            // 3. Update total amount on payment
            updateTotalAmount();
        }

        function updateTotalAmount() {
            // Initial total amount value to zero
            vm.payment.TotalAmount = 0;

            // for loop on each transaction and add to total amount.
            for (var i = 0; i < vm.transactions.length; i++) {
                if (vm.transactions[i].Status === 'Active') {
                    vm.payment.TotalAmount += vm.transactions[i].Amount;
                }
            }
        }
        function submit() {
            console.log('submit call');
            vm.payment.BudgetTransactions = vm.transactions;
            //apiService.payment().save(vm.payment).$promise.then(callPaymentSuccess, callPaymentError);

            openModal('lg', null, 'paymentid');

            function callPaymentSuccess(response) {
                hr.respondSuccess(response);
                //TODO modal result.
            }

            function callPaymentError(e) {
                hr.repondError(e);
            }
        }




        // prepare form data function
        function prepareData() {
            // 1. Get list of employees
            apiService.employee().get().$promise.then(callEmpSuccess, callError);

            // 2. Get list of budgets
            apiService.budget().get().$promise.then(callBudgetSuccess, callError);


            // function section // 
            function callEmpSuccess(response) {
                vm.employees = hr.respondSuccess(response);
                // Set default RequestBy to current user
                vm.payment.RequestBy = authInfo.getUser().EmployeeID;
            }

            function callBudgetSuccess(response) {
                vm.budgets = hr.respondSuccess(response);
                vm.years = $filter('unique')(vm.budgets, 'Year');
                vm.payment.Year = vm.years[0].Year;
            }

            function callError(e) {
                hr.respondError(e);
            }

        }




        // modal test

        var items = ['item1', 'item2', 'item3'];
        function openModal (size, parentSelector, data) {
            var parentElem = parentSelector ?
                angular.element($document[0].querySelector('.modal-demo ' + parentSelector)) : undefined;
            var modalInstance = $uibModal.open({
                animation: true,
                ariaLabelledBy: 'modal-title',
                ariaDescribedBy: 'modal-body',
                templateUrl: 'myModalContent.html',
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

(function () {
    'use strict';
    angular
        .module('budgetApp')
        .controller('ResultPaymentCtrl', ResultPaymentCtrl);


    ResultPaymentCtrl.$inject = ['$state', '$uibModalInstance', 'paymentid'];
    function ResultPaymentCtrl($state, $uibModalInstance, paymentid) {
        var vm = this;
        vm.paymentid = paymentid;
        console.log(paymentid);
        vm.next = function () {
            $uibModalInstance.close();
            $state.go('payment');
        }

        vm.update = function () {
            $uibModalInstance.close();
            $state.go('editpayment');
        }
    }
})();

angular.module('budgetApp').component('modalComponent', {
    templateUrl: 'myModalContent.html',
    bindings: {
        resolve: '<',
        close: '&',
        dismiss: '&'
    },
    controller: function () {
        var $ctrl = this;

        $ctrl.$onInit = function () {
            $ctrl.items = $ctrl.resolve.items;
            $ctrl.selected = {
                item: $ctrl.items[0]
            };
        };

        $ctrl.ok = function () {
            $ctrl.close({ $value: $ctrl.selected.item });
        };

        $ctrl.cancel = function () {
            $ctrl.dismiss({ $value: 'cancel' });
        };
    }
});



budgetApp.controller('PaymentController', ['$scope', '$location', '$state', '$stateParams', 'apiService', 'funcFactory', function ($scope, $location, $state, $stateParams, apiService, funcFactory) {

    $scope.listpayment = {}
    $scope.errormessage = ''

    apiService.payment().get().$promise.then(function (data) {
        funcFactory.getData($scope, 'listpayment', data)
    }, function (error) {
        funcFactory.handleError($scope, error)
    })
    //paymentFactory.get().$promise.then(function (data) {
    //    if (data.isSuccess) {
    //        $scope.listpayment = data.Result
    //        console.log(data)
    //    }
    //    else {
    //        $scope.errormessage = data.Message
    //    }
    //}, function (error) {
    //    $scope.errormessage = error
    //    console.log($scope.errormessage)
    //})

    $scope.rowClick = function (paymentid) {
        //$location.path('/payment/' + paymentid)
        $state.go('viewpayment', { id: paymentid })
    }
}])

budgetApp.controller('DetailPaymentController',
        ['$scope', '$stateParams', '$location', 'apiService',
        function ($scope, $stateParams, $location, apiService) {

            $scope.PaymentID = $stateParams.id
            $scope.payment = {}

            //Get payment detail
            apiService.payment().get({ id: $scope.PaymentID }).$promise.then(function (data) {
                if (data.isSuccess) {
                    $scope.payment = data.Result
                }
                else {
                    console.log(data)
                }
            }, function (error) {
                console.log(error)
            })

            $scope.removePayment = function (paymentid) {
                var payment = {}
                payment.id = paymentid
                var confirm = window.confirm('ยืนยันการลบ?')
                if (confirm) {
                    apiService.payment().remove(payment).$promise.then(function (data) {
                        if (data.isSuccess) {
                            $location.path('payment')
                        }
                    })
                }
            }

        }])

budgetApp.controller('CreatePaymentController',
    ['$scope', '$location', '$filter', 'apiService', 'funcFactory',
    function ($scope, $location, $filter, apiService, funcFactory) {

        apiService.emptypayment().get().$promise.then(function (data) {
            funcFactory.getData($scope, 'payment', data)
            $scope.payment.Year = '2560'
        }, function (error) {
            funcFactory.handleError($scope, error)
        })

        //$scope.emplist
        apiService.employee().get().$promise.then(function (data) {
            funcFactory.getData($scope, 'emplist', data)

        }, function (error) {
            funcFactory.handleError($scope, error)
        })

        //$scope.controller
        apiService.controller().get().$promise.then(function (data) {
            funcFactory.getData($scope, 'controller', data)
        }, function (error) {
            funcFactory.handleError($scope, error)
        })

        //$scope.budgetlist
        apiService.budget().get().$promise.then(function (data) {
            funcFactory.getData($scope, 'budgetlist', data)
        }, function (error) {
            funcFactory.handleError($scope, error)
        })


        //Watch RequestBy index change
        $scope.$watch('payment.RequestBy', function () {
            //Check payment and emplist are loaded
            if ($scope.payment && $scope.emplist) {
                var requester = $filter('filter')($scope.emplist, function (d) {
                    return d.EmployeeID === $scope.payment.RequestBy
                })
                $scope.payment.CostCenterID = requester[0].CostCenterID
            }
        })

        //function addNewStatement() 
        $scope.addNewStatement = funcFactory.payment.addNewStatement.bind(this, $scope)

        $scope.removeStatement = funcFactory.payment.removeStatement.bind(this, $scope)

        $scope.updateTotalAmount = funcFactory.payment.updateTotalAmount.bind(this, $scope)


        //function submit()
        $scope.submit = function () {
            apiService.payment().save($scope.payment).$promise.then(function (data) {
                console.log(data)
                if (data.isSuccess) {
                    $location.path('/payment/' + data.Result)
                }
                else {
                    funcFactory.setError($scope, data)
                }
            }, function (error) {

            })
        }



    }])

budgetApp.controller('EditPaymentController', ['$scope', '$location', '$filter', '$stateParams', 'apiService', 'funcFactory'
    , function ($scope, $location, $filter, $routeParams, apiService, funcFactory) {


        var paymentid = $routeParams.id

        apiService.payment().get({ id: paymentid }).$promise.then(function (data) {
            funcFactory.getData($scope, 'payment', data)
        }, function (error) {
            funcFactory.handleError($scope, error)
        })

        //$scope.emplist
        apiService.employee().get().$promise.then(function (data) {
            funcFactory.getData($scope, 'emplist', data)
        }, function (error) {
            funcFactory.handleError($scope, error)
        })

        //$scope.controller
        apiService.controller().get().$promise.then(function (data) {
            funcFactory.getData($scope, 'controller', data)
        }, function (error) {
            funcFactory.handleError($scope, error)
        })

        //$scope.budgetlist
        apiService.budget().get().$promise.then(function (data) {
            funcFactory.getData($scope, 'budgetlist', data)
        }, function (error) {
            funcFactory.handleError($scope, error)
        })


        //function addNewStatement() 
        $scope.addNewStatement = funcFactory.payment.addNewStatement.bind(this, $scope)

        //function removeStatement
        $scope.removeStatement = funcFactory.payment.removeStatement.bind(this, $scope)

        //function updateTotalAmount
        $scope.updateTotalAmount = funcFactory.payment.updateTotalAmount.bind(this, $scope)

        //function submit()
        $scope.submit = function () {
            apiService.payment().save($scope.payment).$promise.then(function (data) {
                console.log(data)
                if (data.isSuccess) {
                    $location.path('/payment/' + data.Result)
                }
                else {
                    funcFactory.handleError($scope, error)
                }
            }, function (error) {
                funcFactory.setError($scope, data)
            })
        }

    }])

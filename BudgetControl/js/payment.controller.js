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
        $state.go('viewpayment', {id: paymentid})
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

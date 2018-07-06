'use strict';

budgetApp.factory('apiService', ['$resource', function ($resource) {
    var api = {}

    api.employee = function () {
        return $resource('data/employee/:id?retireYear=:retireYear')
    }

    api.costcenter = function () {
        return $resource('data/costcenter/:id');
    }

    api.budget = function () {
        return $resource('data/budget/:id?year=:year&costcenterid=:costcenterid', null, {
            'save': { method: 'POST' },
            'update': { method: 'PUT' },
            'remove': { method: 'DELETE' }
        });
    }

    api.uploadbudget = function () {
        return $resource('data/uploadbudget/')
    }

    api.payment = function () {
        return $resource('data/payment/:id?year=:year&costcenterid=:costcenterid&type=:type', null, {
            'save': { method: 'POST' },
            'update': { method: 'PUT' },
            'remove': { method: 'DELETE'}
        });
    }


    api.summaryreport = function () {
        return $resource('data/SummaryReport?year=:year');
    }

    api.individualreport = function () {
        return $resource('data/Individual/:id?year=:year');
    }

    api.emptypayment = function () {
        return $resource('data/emptypayment')
    }

    api.controller = function () {
        return $resource('data/controller/:id?paymentid=:paymentid')
    }

    api.account = function () {
        return $resource('data/account/:id')
    }

    api.contractor = function () {
        return $resource(
            'data/contractor/:id',
            null,
            {
                'save': { method: 'POST' },
                'update': { method: 'PUT' },
                'remove': { method: 'DELETE'}
            }
        );
    }

    api.budgetinfo = function () {
        return $resource('api/budget?costcenterid=:costcenterid');
    }

    api.workingCondition = function () {
        return $resource('workingcc/getcondition?filter=:filter');
    }

    return api
}])


budgetApp.factory('funcFactory', ['$filter', function ($filter) {
    var func = {}

    func.getData = function ($scope, name, value) {

        if (value.isSuccess) {
            $scope[name] = value.Result
        }
        else {
            setError($scope, value)
        }
    }

    func.setError = function ($scope, error) {
        $scope.hasError = true
        $scope.errorMsg = error.Message
    }

    func.handleError = function ($scope, error) {
        $scope.hasError = true
        $scope.errorMsg = error.toString()
    }

    func.payment = {}
    func.payment.addNewStatement = function ($scope) {
        var index = $scope.payment.Statements.findIndex(function (obj) {
            return obj.BudgetID == $scope.selectbudget
        })

        if (index === -1) {
            var budget = $filter('filter')($scope.budgetlist, function (d) {
                return d.BudgetID === $scope.selectbudget
            })[0]

            $scope.payment.Statements.push({
                StatementID: '',
                BudgetID: budget.BudgetID,
                PaymentID: '',
                BudgetYear: budget.Year,
                AccountID: budget.AccountID,
                AccountName: budget.AccountName,
                CostCenterID: budget.CostCenterID,
                CostCenterName: budget.CostCenterName,
                BudgetAmount: budget.BudgetAmount,
                WithdrawAmount: 0,
                Description: '',
                Status: 'Active',
            })
        }
        else {
            $scope.payment.Statements[index].Status = "Active"
        }

        $scope.selectbudget = ''
    }

    //func.payment.removeStatement = function ($scope, index) {
    //    $scope.payment.Statements[index].Status = "Remove"
    //    func.payment.updateTotalAmount($scope)
    //    console.log($scope.payment)
    //    //$scope.payment.Statements.splice(index, 1)
    //    //func.payment.updateTotalAmount($scope)
    //}

    func.payment.removeStatement = function ($scope, budgetid) {
        var index = $scope.payment.Statements.findIndex(function (obj) {
            return obj.BudgetID == budgetid
        })

        $scope.payment.Statements[index].Status = "Remove"
        func.payment.updateTotalAmount($scope)



        //$scope.payment.Statements[index].Status = "Remove"
        //func.payment.updateTotalAmount($scope)
        //console.log($scope.payment)
        //$scope.payment.Statements.splice(index, 1)
        //func.payment.updateTotalAmount($scope)
    }

    func.payment.updateTotalAmount = function ($scope) {
        $scope.payment.TotalAmount = 0
        for (var i = 0; i < $scope.payment.Statements.length; i++) {
            if ($scope.payment.Statements[i].Status === 'Active') {
                $scope.payment.TotalAmount += $scope.payment.Statements[i].WithdrawAmount
            }
        }
    }


    return func
}])


